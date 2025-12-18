using System.Collections;
using System.Collections.Generic;
using Scripts.Bullets;
using UnityEngine;

namespace Scripts.Player
{
    public sealed class PlayerShoots : MonoBehaviour
    {
        [SerializeField] private GameObject _bullet;
        [SerializeField] private Transform _shootingPoint;
        [SerializeField] private int _initialSize = 20;
        [SerializeField] private float _spawnInterval = 1.0f;

        [Header("Bonus settings")] [SerializeField]
        private float _speedBonusAmount = 0.5f;

        [SerializeField] private float _bonusDuration = 5f;
        public AudioSource AudioSource;

        private int _bulletsPerShot = 1;
        private bool _hasSpeedBonus = false;
        private bool _hasDoubleBulletBonus = false;

        public bool HasSpeedBonus => _hasSpeedBonus;
        public bool HasDoubleBulletBonus => _hasDoubleBulletBonus;

        private Queue<GameObject> _bulletPool;
        private float _timer;

        private void Start()
        {
            PoolBullets();
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            Shoot();
        }

        public void Shoot()
        {
            if (_timer < _spawnInterval) return;

            _timer = 0f;

            for (int i = 0; i < _bulletsPerShot; i++)
            {
                var bullet = GetPooledObject();
                if (bullet != null)
                {
                    bullet.transform.position = _shootingPoint.position + new Vector3(i * 0.2f, 0f, 0f);
                    bullet.transform.rotation = _shootingPoint.rotation;
                    bullet.SetActive(true);
                }
            }
            
            AudioSource.Play();
        }

        public void ReturnObjectToPool(GameObject bullet)
        {
            bullet.SetActive(false);
            _bulletPool.Enqueue(bullet);
        }
        
        public void ApplySpeedBonus()
        {
            if (_hasSpeedBonus)
            {
                return;
            }
            _hasSpeedBonus = true;
            _spawnInterval = Mathf.Max(0.1f, _spawnInterval - _speedBonusAmount);
            StartCoroutine(RemoveSpeedBonusAfterDelay());
        }

        public void ApplyDoubleBulletBonus()
        {
            if (_hasDoubleBulletBonus)
            {
                return;
            }
            _hasDoubleBulletBonus = true;
            _bulletsPerShot = 2;
            StartCoroutine(RemoveDoubleBulletAfterDelay());
        }

        private IEnumerator RemoveSpeedBonusAfterDelay()
        {
            yield return new WaitForSeconds(_bonusDuration);
            _spawnInterval += _speedBonusAmount;
            _hasSpeedBonus = false;
        }
        private IEnumerator RemoveDoubleBulletAfterDelay()
        {
            yield return new WaitForSeconds(_bonusDuration);
            _bulletsPerShot = 1;
            _hasDoubleBulletBonus = false;
        }

        private void PoolBullets()
        {
            _bulletPool = new Queue<GameObject>();
            for (int i = 0; i < _initialSize; i++)
            {
                var newBullet = Instantiate(_bullet);
                newBullet.SetActive(false);

                var bulletComp = newBullet.GetComponent<BulletPlayer>();
                if (bulletComp != null)
                    bulletComp.SetPoolOwner(this);

                _bulletPool.Enqueue(newBullet);
            }
        }

        private GameObject GetPooledObject()
        {
            if (_bulletPool.Count > 0)
            {
                return _bulletPool.Dequeue();
            }

            var bullet = Instantiate(_bullet);
            bullet.SetActive(false);
            var bulletComp = bullet.GetComponent<BulletPlayer>();
            if (bulletComp != null)
            {
                bulletComp.SetPoolOwner(this);
            }

            return bullet;
        }
    }
}
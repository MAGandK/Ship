using System.Collections;
using Scripts.Player;
using UnityEngine;

namespace Scripts.Bullets
{
    public sealed class BulletPlayer : MonoBehaviour
    {
        [SerializeField] private int _speed;
        [SerializeField] private float _lifetime;
        private Rigidbody _rb;
        private PlayerShoots _shoots;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            StartCoroutine(LifeBullet());
        }

        private void OnDisable()
        {
            StopCoroutine(LifeBullet());
            if (_rb !=null)
            {
                _rb.linearVelocity = Vector3.zero;
            }
        }

        private void FixedUpdate()
        {
            _rb.linearVelocity = transform.forward * _speed;
        }

        private IEnumerator LifeBullet()
        {
            yield return new WaitForSeconds(_lifetime);
            if (_shoots != null)
            {
                _shoots.ReturnObjectToPool(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        
        public void SetPoolOwner(PlayerShoots owner)
        {
            _shoots= owner;
        }
    }
}
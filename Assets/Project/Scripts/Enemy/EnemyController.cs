using Scripts.Background;
using Scripts.Bullets;
using Scripts.Game;
using UnityEngine;

namespace Scripts.Enemy
{
    public sealed class EnemyController : MonoBehaviour, IDestructible
    {
        [SerializeField] private GameObject _explosionEnemy;
        [SerializeField] private float _explosionLifetime = 2.0f;
        private KillCounter _killCounter;

        public void SetKillCounter(KillCounter killCounter)
        {
            _killCounter = killCounter;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<BackgroundScroller>() || other.GetComponent<BulletEnemyMove>())
            {
                return;
            }

            if (_explosionEnemy != null)
            {
                var newExplosion = Instantiate(_explosionEnemy, transform.position, transform.rotation);
                Destroy(newExplosion, _explosionLifetime);
            }

            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage();
            }
            
            _killCounter.AddKill();
            EnemySpawner.ReturnEnemyToPool(gameObject);
        }

        public void Destroy()
        {
            gameObject.SetActive(false);
        }
    }
}
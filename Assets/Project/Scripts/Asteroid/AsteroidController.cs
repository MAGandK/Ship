using Scripts.Background;
using Scripts.Bullets;
using Scripts.Enemy;
using Scripts.Game;
using Scripts.Player;
using UnityEngine;

namespace Scripts.Asteroid
{
    public class AsteroidController : MonoBehaviour, IDestructible
    {
        [SerializeField] private GameObject _explosion;
        [SerializeField] private float _explosionLifetime = 2.0f;

        private KillCounter _killCounter;

        public void SetKillCounter(KillCounter killCounter)
        {
            _killCounter = killCounter;
        }

        public virtual void Destroy()
        {
            SpawnExplosion();
            AsteroidSpawner.ReturnAsteroidToPool(gameObject);
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<BackgroundScroller>())
            {
                return;
            }

            if (other.GetComponent<EnemyController>() || other.GetComponent<BulletEnemyMove>())
            {
                DieAsteroid();
                Destroy(other.gameObject);
            }
            else if (other.GetComponentInParent<PlayerController>() || other.GetComponent<BulletPlayer>())
            {
                _killCounter?.AddKill();
                Destroy();
            }
        }

        private void DieAsteroid()
        {
            AsteroidSpawner.ReturnAsteroidToPool(gameObject);
        }

        private void SpawnExplosion()
        {
            if (_explosion == null) return;

            var explosion = Instantiate(_explosion, transform.position, transform.rotation);
            Destroy(explosion, _explosionLifetime);
        }
    }
}
using Scripts.Game;
using UnityEngine;

namespace Scripts.Bullets
{
    public sealed class BulletEnemyMove : MonoBehaviour, IDestructible
    {
        [SerializeField] private float _bulletEnemySpeed;
        private Rigidbody _rbBulletEnemy;

        private void Start()
        {
            _rbBulletEnemy = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            _rbBulletEnemy.linearVelocity = -transform.forward * _bulletEnemySpeed;
        }
        
        public void Destroy()
        {
            gameObject.SetActive(false);
        }
    }
}
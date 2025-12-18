using UnityEngine;

namespace Scripts.Bullets
{
    public sealed class WeaponController : MonoBehaviour
    {
        [SerializeField] private GameObject _shot;
        [SerializeField] private Transform _shotSpawn;
        [SerializeField] private float _fireRate;
        [SerializeField] private float _delay;
        public AudioSource AudioSource;
        private void Start()
        {
            InvokeRepeating("Fire", _delay, _fireRate);
        }

        private void Fire()
        {
            Instantiate(_shot, _shotSpawn.position, _shotSpawn.rotation);
            AudioSource.Play();
        }
    }
}
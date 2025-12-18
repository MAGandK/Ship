using UnityEngine;

namespace Scripts.Asteroid
{
    public sealed class AsteroidMovement : MonoBehaviour
    {
        [SerializeField] private float _speedMin;
        [SerializeField] private float _speedMax;
        [SerializeField] private float _tumbleMin;
        [SerializeField] private float _tumbleMax;

        private float _speed;
        private float _tumble;
        private Rigidbody _rbAsteroid;

        private void Awake()
        {
            _rbAsteroid = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            var speed = Random.Range(_speedMin, _speedMax);
            var tumble = Random.Range(_tumbleMin, _tumbleMax);

            _rbAsteroid.linearVelocity = transform.forward * -speed;
            _rbAsteroid.angularVelocity = Random.insideUnitSphere * tumble;
        }
    }
}
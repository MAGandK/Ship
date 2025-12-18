using System.Collections;
using Scripts.Configs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Enemy
{
    public sealed class EnemyWave : MonoBehaviour
    {
        [Header("Moving")] [SerializeField] private float _speedMin;
        [SerializeField] private float _speedMax;

        [SerializeField] private BoundaryConfig _boundary;
        [SerializeField] private float _tilt;

        [SerializeField] private float _dodge;
        [SerializeField] private float _smoothing;
        [SerializeField] private Vector2 _startWait;
        [SerializeField] private Vector2 _waveTimer;
        [SerializeField] private Vector2 _waveWait;

        private float _currentSpeed;
        private float _targetWave;

        private Rigidbody _rbEnemy;

        private void Start()
        {
            _rbEnemy = GetComponent<Rigidbody>();
            _currentSpeed = Random.Range(_speedMin, _speedMax);

            StartCoroutine(Evasion());
        }

        private IEnumerator Evasion()
        {
            yield return new WaitForSeconds(Random.Range(_startWait.x, _startWait.y));

            while (true)
            {
                _targetWave = Random.Range(1, _dodge) * -Mathf.Sign(transform.position.x);

                yield return new WaitForSeconds(Random.Range(_waveTimer.x, _waveTimer.y));

                _targetWave = 0;
                yield return new WaitForSeconds(Random.Range(_waveWait.x, _waveWait.y));
            }
        }

        private void FixedUpdate()
        {
            var newManeuver = Mathf.MoveTowards(_rbEnemy.linearVelocity.x, _targetWave, _smoothing * Time.deltaTime);
            _rbEnemy.linearVelocity = new Vector3(newManeuver, 0.0f, -_currentSpeed);
            _rbEnemy.position = new Vector3
            (
                Mathf.Clamp(_rbEnemy.position.x, _boundary.XMin, _boundary.XMax),
                0.0f,
                _rbEnemy.position.z
            );

            _rbEnemy.rotation = Quaternion.Euler(0, 0, _rbEnemy.linearVelocity.x * -_tilt);
        }
    }
}
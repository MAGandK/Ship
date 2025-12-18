using System.Collections.Generic;
using System.Linq;
using Scripts.Configs;
using Scripts.Game;
using UnityEngine;
using Zenject;

namespace Scripts.Asteroid
{
    public sealed class AsteroidSpawner : MonoBehaviour
    {
        private const int BONUS_MIN_COUNT = 3;  
        [SerializeField] private GameObject[] _asteroids;
        [SerializeField] private GameObject _bonusAsteroid;
        [SerializeField] private Transform _spawnAsteroids;
        [SerializeField] private int _initialSize;
        [SerializeField] private float _spawnInterval;
        [SerializeField] private BoundaryConfig _boundaryAsteroid;
        private KillCounter _killCounter;
        private List<GameObject> _asteroidsPrefabs;
        private float _timer;

        [Inject]
        private void Construct(KillCounter killCounter)
        {
            _killCounter = killCounter;
        }

        private void Start()
        {
            _asteroidsPrefabs = new List<GameObject>();
            for (int i = 0; i < BONUS_MIN_COUNT; i++)
            {
                var bonus = Instantiate(_bonusAsteroid);
                bonus.SetActive(false);
                _asteroidsPrefabs.Add(bonus);
            }
            
            for (int i = 0; i < _initialSize; i++)
            {
                var newAsteroidPrefab = Instantiate(_asteroids[Random.Range(0, _asteroids.Length)]);
                newAsteroidPrefab.SetActive(false);
                var controller = newAsteroidPrefab.GetComponent<AsteroidController>();
                if (controller != null)
                    controller.SetKillCounter(_killCounter);
                _asteroidsPrefabs.Add(newAsteroidPrefab);
            }
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (!(_timer >= _spawnInterval)) return;
            _timer = 0;
            SpawnAsteroid();
        }

        private GameObject GetPoolAsteroid()
        {
            var inactiveAsteroids = _asteroidsPrefabs.Where(a => a != null && !a.activeInHierarchy).ToList();
            if (inactiveAsteroids.Count > 0)
            {
                return inactiveAsteroids[Random.Range(0, inactiveAsteroids.Count)];
            }

          
            var newAsteroid = Instantiate(_asteroids[Random.Range(0, _asteroids.Length)]);
            newAsteroid.SetActive(false);

            var controller = newAsteroid.GetComponent<AsteroidController>();
            controller?.SetKillCounter(_killCounter);

            _asteroidsPrefabs.Add(newAsteroid);
            return newAsteroid;
        }

        private void SpawnAsteroid()
        {
            var asteroid = GetPoolAsteroid();

            if (asteroid is null)
            {
                return;
            }

            asteroid.transform.position = new Vector3(Random.Range(_boundaryAsteroid.XMin, _boundaryAsteroid.XMax),
                _spawnAsteroids.position.y, _spawnAsteroids.position.z);

            asteroid.transform.rotation = _spawnAsteroids.transform.rotation;
            asteroid.SetActive(true);
        }

        internal static void ReturnAsteroidToPool(GameObject asteroid)
        {
            asteroid.SetActive(false);
        }
    }
}
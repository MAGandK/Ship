using System.Collections.Generic;
using Scripts.Configs;
using Scripts.Game;
using UnityEngine;
using Zenject;

namespace Scripts.Enemy
{
    public sealed class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _enemyPrefabs;
        [SerializeField] private Transform _spawnEnemy;
        [SerializeField] private int _initialSize;
        [SerializeField] private float _spawnInterval;
        [SerializeField] private BoundaryConfig _boundary;
        private KillCounter _killCounter;
        private List<GameObject> _enemies;
        private float _timer;

        [Inject]
        private void Construct(KillCounter killCounter)
        {
            _killCounter = killCounter;
        }
        
        private void Start()
        {
            _enemies = new List<GameObject>();

            for (int i = 0; i < _initialSize; i++)
            {
                var newEnemy = Instantiate(_enemyPrefabs, _spawnEnemy.position, _spawnEnemy.rotation);
                newEnemy.SetActive(false);

                var controller = newEnemy.GetComponent<EnemyController>();
                if (controller != null)
                {
                    controller.SetKillCounter(_killCounter);
                }
                _enemies.Add(newEnemy);
            }
        }
    
        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= _spawnInterval)
            {
                _timer = 0;
                SpawnEnemies();
            }
        }

        private GameObject GetPoolEnemy()
        {
            foreach (var enemy in _enemies)
            {
                if (enemy != null && !enemy.activeInHierarchy)
                {
                    return enemy;
                }
            }
            var newEnemy  = Instantiate(_enemyPrefabs, _spawnEnemy.position, _spawnEnemy.rotation);
            newEnemy.SetActive(false);
            var controller = newEnemy.GetComponent<EnemyController>();
            if (controller is not null)
            {
                controller.SetKillCounter(_killCounter);
            }
            _enemies.Add(newEnemy);
            return newEnemy ;
        }
        private void SpawnEnemies()
        {
            GameObject enemy = GetPoolEnemy();

            if (enemy is not null)
            {
                enemy.transform.position = new Vector3
                (
                    Random.Range(_boundary.XMin, _boundary.XMax), 
                    _spawnEnemy.position.y,
                    _spawnEnemy.position.z);
            
                enemy.transform.rotation = _spawnEnemy.transform.rotation;
                enemy.SetActive(true);
            }
        }

        public static void ReturnEnemyToPool(GameObject enemy)
        {
            enemy.SetActive(false);
        }
    }
}

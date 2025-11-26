using Firat0667.CaseLib.Pattern.Pool;
using UnityEngine;
namespace Firat0667.CaseLib.Game
{
    /// <summary>
    /// Spawner that spawns enemies at random intervals and makes them jump to a random position within a specified radius.
    /// </summary>
    public class EnemySpawner : Spawner
    {
        [SerializeField] private float minSpawnInterval = 15f;
        [SerializeField] private float maxSpawnInterval = 30f;

        [Header("EnemySpawnAnim")]
        [SerializeField] private float spawnRadius = 2f; 
        [SerializeField] private float jumpPower = 3f; 
        [SerializeField] private int jumpCount = 1; 
        [SerializeField] private float jumpDuration = 0.5f; 

        private float _nextSpawnTime;

        private void Start()
        {
            SetNextSpawnTime();
        }

        private void Update()
        {

            if (Time.time >= _nextSpawnTime)
            {
                SpawnEnemyWithJump();

                SetNextSpawnTime();
            }
        }

        private void SetNextSpawnTime()
        {
            _nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
        }
        private void SpawnEnemyWithJump()
        {
            GameObject enemy = _spawnPool.Retrieve();

            enemy.transform.position = _spawnTransform.position;
            enemy.SetActive(true);

            Vector3 randomDirection = Random.insideUnitSphere.normalized; 
            Vector3 randomTarget = _spawnTransform.position + randomDirection * spawnRadius;

            //enemy.transform.DOJump(randomTarget, jumpPower, jumpCount, jumpDuration)
            //    .SetEase(Ease.OutQuad) 
            //    .OnComplete(() => OnEnemyLanded(enemy)); 
        }


        private void OnEnemyLanded(GameObject enemy)
        {
     
        }
    }
}

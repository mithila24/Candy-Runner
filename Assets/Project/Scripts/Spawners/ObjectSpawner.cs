using UnityEngine;
using System.Collections;

namespace Project.Scripts
{
    public class ObjectSpawner : MonoBehaviour
    {
        [Header("Spawn Position")]
        public float spawnX = 12f;

        [Header("Candy Spawn")]
        public float candyMinY = -2f;
        public float candyMaxY = 3f;

        [Header("Obstacle Spawn")]
        public float obstacleY = -1f;

        [Header("PowerUp Spawn")]
        public float powerUpMinY = 0f;
        public float powerUpMaxY = 3f;

        [Header("Spawn Timing")]
        public float minSpawnTime = 0.5f;
        public float maxSpawnTime = 1f;

        [Header("Pool Sizes")]
        [SerializeField]
        private int candyPoolSize = 10;

        [SerializeField]
        private int obstaclePoolSize = 10;

        [SerializeField]
        private int powerupPoolSize = 3;

        [Header("Spawn Chances")]
        [Range(0,100)]
        public int candyChance = 70;

        [Range(0,100)]
        public int obstacleChance = 20;

        [Range(0,100)]
        public int powerupChance = 10;

        private Coroutine spawnRoutine;

        private void Start()
        {
            // SAFETY CHECKS
            if(PoolManager.Instance == null)
            {
                Debug.LogError(
                    "PoolManager Missing!"
                );

                return;
            }

            if(GameManager.instance == null)
            {
                Debug.LogError(
                    "GameManager Missing!"
                );

                return;
            }

            // CREATE POOLS
            PoolManager.Instance
                .CreateObjPool(
                    "candies",
                    candyPoolSize
                );

            PoolManager.Instance
                .CreateObjPool(
                    "obstaclesbundle",
                    obstaclePoolSize
                );

            PoolManager.Instance
                .CreateObjPool(
                    "shield",
                    powerupPoolSize
                );

            PoolManager.Instance
                .CreateObjPool(
                    "boost",
                    powerupPoolSize
                );

            Debug.Log(
                "ObjectSpawner Started"
            );

            spawnRoutine =
                StartCoroutine(
                    SpawnRoutine()
                );
        }

        private IEnumerator SpawnRoutine()
        {
            // START DELAY
            yield return
                new WaitForSeconds(2f);

            while (true)
            {
                // GAME MANAGER SAFETY
                if(GameManager.instance == null)
                {
                    yield break;
                }

                // STOP SPAWNING ON GAMEOVER
                if(GameManager.instance.gameover)
                {
                    Debug.Log(
                        "Spawning Stopped"
                    );

                    yield break;
                }

                SpawnRandomObject();

                yield return
                    new WaitForSeconds(
                        Random.Range(
                            minSpawnTime,
                            maxSpawnTime
                        )
                    );
            }
        }

        private void SpawnRandomObject()
        {
            int randomValue =
                Random.Range(0, 100);

            GameObject obj = null;

            float randomY = 0f;

            // CANDY
            if (randomValue < candyChance)
            {
                obj =
                    PoolManager.Instance
                    .GetObject("candies");

                randomY =
                    Random.Range(
                        candyMinY,
                        candyMaxY
                    );
            }

            // OBSTACLE
            else if (
                randomValue <
                candyChance +
                obstacleChance
            )
            {
                obj =
                    PoolManager.Instance
                    .GetObject(
                        "obstaclesbundle"
                    );

                randomY = obstacleY;
            }

            // POWERUPS
            else
            {
                int powerupRandom =
                    Random.Range(0, 2);

                // SHIELD
                if (powerupRandom == 0)
                {
                    obj =
                        PoolManager.Instance
                        .GetObject("shield");

                    Debug.Log(
                        "Shield Spawned"
                    );
                }

                // BOOST
                else
                {
                    obj =
                        PoolManager.Instance
                        .GetObject("boost");

                    Debug.Log(
                        "Boost Spawned"
                    );
                }

                randomY =
                    Random.Range(
                        powerUpMinY,
                        powerUpMaxY
                    );
            }

            // NULL SAFETY
            if (obj == null)
            {
                Debug.LogWarning(
                    "Spawn Failed!"
                );

                return;
            }

            obj.transform.position =
                new Vector3(
                    spawnX,
                    randomY,
                    0f
                );

            obj.transform.rotation =
                Quaternion.identity;

            obj.SetActive(true);
        }

        // STOP SPAWNING
        public void StopSpawning()
        {
            if(spawnRoutine != null)
            {
                StopCoroutine(
                    spawnRoutine
                );

                Debug.Log(
                    "SpawnRoutine Stopped"
                );
            }
        }
    }
}
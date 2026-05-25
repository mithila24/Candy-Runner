using UnityEngine;
using System.Collections;

namespace Project.Scripts
{
    public class ObjectSpawner : MonoBehaviour
    {
        public float spawnX = 12f;
        public float candyMinY = -2f;
        public float candyMaxY = 3f;
        
        public float obstacleY = -1f;
        
        public float powerUpMinY = 0f;
        public float powerUpMaxY = 3f;
        
        public float minSpawnTime = 0.5f;
        public float maxSpawnTime = 1f;

        private int candyPoolSize = 10;
        private int obstaclePoolSize = 10;

       
        private void Start()
        {
         
            PoolManager.Instance.CreateObjPool("candies", candyPoolSize);
            PoolManager.Instance.CreateObjPool("obstaclesbundle", obstaclePoolSize);
            PoolManager.Instance.CreateObjPool("shield", 3);
            PoolManager.Instance.CreateObjPool("boost", 3);
            
            StartCoroutine(SpawnRoutine());
        }

        IEnumerator SpawnRoutine()
        {

            yield return new WaitForSeconds(2f);
            while (!GameManager.instance.gameover)
            {
                SpawnRandomObject();
                yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
            }
        }

        void SpawnRandomObject()
        {
            int randomValue = Random.Range(0, 100);
            GameObject obj = null;
            float randomY = 0f;
            
            if (randomValue < 70)
            {
                obj = PoolManager.Instance.GetObject("candies");
                randomY = Random.Range(candyMinY, candyMaxY ); 
            }
            
            else if (randomValue < 90)
            {
                obj = PoolManager.Instance.GetObject("obstaclesbundle");
                randomY = obstacleY;
            }
            
            else
            {
                int powerupRandom = Random.Range(0, 2);
                
                if (powerupRandom == 0)
                {
                    obj = PoolManager.Instance.GetObject("shield");
                }
                
                else
                {
                    obj = PoolManager.Instance.GetObject("boost");
                }
             
                randomY = Random.Range(powerUpMinY, powerUpMaxY);
            }

            if (obj == null)
                return;

            obj.transform.position = new Vector3(spawnX, randomY, 0f);
            obj.transform.rotation = Quaternion.identity;
            obj.SetActive(true);
        }
    }
}
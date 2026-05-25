using UnityEngine;

namespace Project.Scripts
{
    public class CandySpawner : MonoBehaviour
    {
        
        public float minY = -2f;
        public float maxY = 3f;
        public float spawnX = 12f;

        void Start()
        {
            PoolManager.Instance.CreateObjPool("candies",15);
            InvokeRepeating( nameof(SpawnCandy), 1f, Random.Range(2,4) );
        }

        void SpawnCandy()
        {
            GameObject candy = PoolManager.Instance.GetObject("candies");
            float randomY = Random.Range(minY, maxY);
            candy.transform.position = new Vector3(spawnX, randomY, 0f );
            candy.SetActive(true);
        }
    }
}
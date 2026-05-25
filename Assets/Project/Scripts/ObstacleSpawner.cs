using UnityEngine;
namespace Project.Scripts
{
    public class ObstacleSpawner : MonoBehaviour
    {
      
       public float XAxis = 12f;
        public float YAxis = 12f;
       

        private void Start()
        {
          
            PoolManager.Instance.CreateObjPool("obstaclesbundle", 10);
            InvokeRepeating(nameof(SpawnObstacle), 2f, Random.Range(2,8));
        }

        private void SpawnObstacle()
        {
            GameObject obstacle = PoolManager.Instance.GetObject("obstaclesbundle");
            if (obstacle == null)
                return;
            
            obstacle.transform.position = new Vector3(XAxis, YAxis, 0f);
            obstacle.transform.rotation = Quaternion.identity;
            obstacle.SetActive(true);
        }
    }
}
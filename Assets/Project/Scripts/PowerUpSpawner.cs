using UnityEngine;

namespace Project.Scripts
{
    public class PowerUpSpawner : MonoBehaviour
    {
        public float XAxis = 12f;
        public float YAxis = 12f;

        private void Start()
        {
          
            PoolManager.Instance.CreateObjPool("powerupbundle", 10);
            InvokeRepeating(nameof(SpawnObstacle), 2f, Random.Range(2,8));
        }

        private void SpawnObstacle()
        {
            GameObject power = PoolManager.Instance.GetObject("powerupbundle"); 
            if (power == null)
                return;
            
            power.transform.position = new Vector3(XAxis, YAxis, 0f);
            power.transform.rotation = Quaternion.identity;
            power.SetActive(true);
        }
    }
}

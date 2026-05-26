using UnityEngine;
using System.Collections.Generic;

namespace Project.Scripts
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance;

        private Dictionary<string, List<GameObject>>
            pools = new Dictionary<string, List<GameObject>>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);

                return;
            }
        }

        // CREATE POOL
        public void CreateObjPool(
            string category,
            int poolSize
        )
        {
            // INVALID SIZE CHECK
            if(poolSize <= 0)
            {
                Debug.LogWarning("Invalid Pool Size For : " + category);
                return;
            }

            // ALREADY EXISTS
            if (pools.ContainsKey(category))
            {
                Debug.Log(category + " Pool Already Exists");
                return;
            }

            Debug.Log("Creating Pool : " + category);

            pools.Add(category, new List<GameObject>());

            for (int i = 0; i < poolSize; i++)
            {
                CreateNewObject(category);
            }

            Debug.Log(category + " Pool Created Successfully");
        }

        // GET OBJECT FROM POOL
        public GameObject GetObject(string category)
        {
            // CATEGORY CHECK
            if(!pools.ContainsKey(category))
            {
                Debug.LogError("Pool Category Not Found : " + category);
                return null;
            }

            foreach (GameObject obj in pools[category])
            {
                // NULL SAFETY
                if(obj == null)
                {
                    continue;
                }

                // INACTIVE OBJECT
                if (!obj.activeInHierarchy)
                {
                    return obj;
                }
            }

            // CREATE NEW OBJECT
            return CreateNewObject(category);
        }

        // CREATE NEW OBJECT
        private GameObject CreateNewObject(string category)
        {
            GameObject prefab = null;
            // SHIELD
            if (category == "shield")
            {
                prefab = AssetBundleManager.Instance.GetPrefabByTag("powerupbundle", "shield");
            }

            // BOOST
            else if (category == "boost")
            {
                prefab = AssetBundleManager.Instance.GetPrefabByTag("powerupbundle", "boost");
            }

            // RANDOM PREFAB
            else
            {
                prefab = AssetBundleManager.Instance.GetRandomPrefab(category);
            }

            // PREFAB NULL CHECK
            if(prefab == null)
            {
                Debug.LogError("Prefab Is NULL For : " + category);
                return null;
            }

            // CREATE OBJECT
            GameObject obj = Instantiate(prefab);
            // CHANGE CANDY SPRITE
            if (category == "candies" && GameManager.instance.changeCandyWithoutAssetbundleBuild)
            {
                // UPDATE SPRITE
                if (obj.TryGetComponent(
                    out SpriteRenderer sr))
                {
                    sr.sprite =
                        GameManager.instance
                        .candySprite;
                }

                // UPDATE COLLIDER
                if (obj.TryGetComponent(
                    out PolygonCollider2D p))
                {
                    // Updating collider after
                    // runtime sprite replacement

                    Destroy(p);

                    obj.AddComponent
                        <PolygonCollider2D>()
                        .isTrigger = true;
                }
            }

            obj.SetActive(false);

            pools[category].Add(obj);

            return obj;
        }
    }
}
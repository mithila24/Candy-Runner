using UnityEngine;
using System.Collections.Generic;

namespace Project.Scripts
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance;

        private Dictionary<string, List<GameObject>> pools = new Dictionary<string, List<GameObject>>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }


        public void CreateObjPool(string category, int poolSize)
        {
            if (pools.ContainsKey(category)) return;
            pools.Add(category, new List<GameObject>());
            for (int i = 0; i < poolSize; i++)
            {
                CreateNewObject(category);
            }
        }

        public GameObject GetObject(string category)
        {
            foreach (GameObject obj in pools[category])
            {
                if (!obj.activeInHierarchy)
                {
                    return obj;
                }
            }
            return CreateNewObject(category);
        }

        private GameObject CreateNewObject(string category)
        {
            GameObject prefab = null;
            if (category == "shield")
            {
                prefab = AssetBundleManager.Instance.GetPrefabByTag("powerupbundle", "shield");
            }
            else if (category == "boost")
            {
                prefab = AssetBundleManager.Instance.GetPrefabByTag("powerupbundle", "boost");
            }

            else
            {
                prefab = AssetBundleManager.Instance.GetRandomPrefab(category);
            }

            GameObject obj = Instantiate(prefab);
            if (category == "candies" && GameManager.instance.changeCandyWithoutAssetbundleBuild)
            {
                if (obj.TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
                {
                    sr.sprite = GameManager.instance.candySprite;
                }

                if (obj.TryGetComponent<PolygonCollider2D>(out PolygonCollider2D p))
                {
                    Destroy(p);
                    obj.AddComponent<PolygonCollider2D>().isTrigger = true;
                    
                }
            }

            obj.SetActive(false);
            pools[category].Add(obj);
            return obj;
        }
    }
}
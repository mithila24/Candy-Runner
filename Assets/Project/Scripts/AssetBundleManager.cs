using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Scripts
{
    public class AssetBundleManager : MonoBehaviour
    {
        public static AssetBundleManager Instance;
        private Dictionary<string, AssetBundle> loadedBundles = new Dictionary<string, AssetBundle>();

        private Dictionary<string, GameObject[]> loadedAssets = new Dictionary<string, GameObject[]>();

        public bool IsLoadingComplete { get; private set; }
        private async void Awake()
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

            await LoadAllBundles();
        }

        private async Task LoadAllBundles()
        {
            await LoadBundle("candies");
            await LoadBundle("obstaclesbundle");
            await LoadBundle("powerupbundle");
            IsLoadingComplete = true;
            
        }

        private async Task LoadBundle(string bundleName)
        {
            if (loadedBundles.ContainsKey(bundleName))
            {
                return;
            }
            string path = Path.Combine(Application.streamingAssetsPath, bundleName);
            
            if (!File.Exists(path))
            {
                Debug.LogError(bundleName + " NOT FOUND at path:\n" + path);
                return;
            }

           
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
            while (!request.isDone)
            {
                await Task.Yield();
            }
            AssetBundle bundle = request.assetBundle;

            if (bundle == null)
            {
                return;
            }

            loadedBundles.Add(bundleName, bundle);
            
            GameObject[] prefabs = bundle.LoadAllAssets<GameObject>();
            loadedAssets.Add(bundleName, prefabs);
            Debug.Log(bundleName + " Loaded Successfully!");
        }

      
        public GameObject[] LoadAllPrefabs(string bundleName)
        {
            if (!loadedAssets.ContainsKey(bundleName))
            {
                return null;
            }

            return loadedAssets[bundleName];
        }

      
        public GameObject GetRandomPrefab(string bundleName)
        {
            GameObject[] prefabs = LoadAllPrefabs(bundleName);
            if (prefabs == null) { return null; }
            int randomIndex = Random.Range(0, prefabs.Length);
            return prefabs[randomIndex];
        }

      
        public GameObject GetPrefabByTag(string bundleName, string tagName)
        {
            if (!loadedAssets.ContainsKey(bundleName))
            {
                Debug.LogError(bundleName + " not loaded!");
                return null;
            }

            GameObject[] prefabs =
                loadedAssets[bundleName];

            foreach (GameObject prefab in prefabs)
            {
                if (prefab.CompareTag(tagName))
                {
                    return prefab;
                }
            }
            return null;
        }

       
    }
}
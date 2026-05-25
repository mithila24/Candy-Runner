using System.IO;
using TMPro;
using UnityEngine;
using Project.Scripts.Data;

namespace Project.Scripts.UI
{
    public class ProfileManager : MonoBehaviour
    {
        public static ProfileManager instance;
        public TMP_InputField nameInput;
        public TMP_InputField passwordInput;
        public TMP_Text messageText;
        public Transform contentParent;
        public GameObject profileItemPrefab;
        public GameObject profilesScrollView;
        public GameObject addProfilePanel;
        public static PlayerData currentProfile;
        private string folderPath;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }

            folderPath = Path.Combine(Application.persistentDataPath, "Profiles");
Debug.Log("Path "+ folderPath);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            
            LoadSavedProfile();
        }

 
        public void SubmitProfile()
        {
            string playerName = nameInput.text.Trim();
            string password = passwordInput.text.Trim();
            if (string.IsNullOrEmpty(playerName) || string.IsNullOrEmpty(password))
            {
                messageText.text = "Name or Password cannot be Empty";
                return;
            }
            string filePath = Path.Combine(folderPath, playerName + ".json");
            if (File.Exists(filePath))
            {
                messageText.text = "Username already exists!";
                return;
            }
            
            PlayerData data = new PlayerData
            {
                playerName = playerName,
                password = password,
                highScore = 0
            };
            
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, json);
            messageText.text = "Profile Created Successfully!!";
            nameInput.text = "";
            passwordInput.text = "";
            SelectProfile(data);
            HomeScreenPlayButton.instance.LoadGameScene();
        }

       
        public void LoadProfiles()
        {
            string[] files = Directory.GetFiles(folderPath, "*.json");
            foreach (string file in files)
            {
                string json = File.ReadAllText(file);
                PlayerData data = JsonUtility.FromJson<PlayerData>(json);

                GameObject item = Instantiate(profileItemPrefab, contentParent);

                ProfileUICard itemUI = item.GetComponent<ProfileUICard>();
                itemUI.Setup(data);
            }
        }

      
        public void ShowProfiles()
        {
            profilesScrollView.SetActive(true);
            foreach (Transform child in contentParent)
            {
                Destroy(child.gameObject);
            }
            LoadProfiles();
        }
        
        public void SelectProfile(PlayerData profile)
        {
            
            currentProfile = profile;
            PlayerPrefs.SetString("SelectedProfile", profile.playerName);
            PlayerPrefs.Save();
            
        }

  
        public void SaveCurrentProfile()
        {
            if(currentProfile == null)
                return;

            string filePath = Path.Combine(folderPath, currentProfile.playerName + ".json");
            string json = JsonUtility.ToJson(currentProfile, true );
            File.WriteAllText(filePath, json);
         
        }
        
        void LoadSavedProfile()
        {
            if (!PlayerPrefs.HasKey("SelectedProfile")) return;
            string savedProfile = PlayerPrefs.GetString("SelectedProfile");
            string filePath = Path.Combine(folderPath, savedProfile + ".json");
            if (!File.Exists(filePath)) return;
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json );
            currentProfile = data;
            
            
        }
    }
}
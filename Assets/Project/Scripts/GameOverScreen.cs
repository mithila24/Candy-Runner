using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Project.Scripts.Data;
using Project.Scripts.UI;

namespace Project.Scripts
{
    public class GameOverScreen : MonoBehaviour
    {
        public TextMeshProUGUI finalScoreText;
        public TextMeshProUGUI highScoreText;
        public Transform leaderboardParent;
        public LeaderPlayer leaderboardItemPrefab;
        public GameObject finalPanel, leaderPanel, leaderButton, createProfileBtn;

        public GameObject img1, img2;
        private string folderPath;

        private void Start()
        {
            folderPath = Path.Combine(Application.persistentDataPath, "Profiles");
            SetupCurrentPlayerUI();
            finalPanel.SetActive(true);
            leaderPanel.SetActive(false);
            if (ProfileManager.currentProfile != null) ShowLeaderboard();
            img1.transform.DORotate(new Vector3(0, 0, -15f), 1.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).From(new Vector3(0, 0, 15f));
            img2.transform.DORotate(new Vector3(0, 0, -15f), 1.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).From(new Vector3(0, 0, 15f));
        }


        void SetupCurrentPlayerUI()
        {
            if (ProfileManager.currentProfile != null)
            {
                leaderButton.gameObject.SetActive(true);
                createProfileBtn.SetActive(false);
                highScoreText.text = "High Score : " + ProfileManager.currentProfile.highScore;
                finalScoreText.text =  ProfileManager.currentProfile.playerName + "\nYou Collected " + GameManager.instance.score + " Candies!!";
            }
            else
            {
                finalScoreText.text = "You Collected " + GameManager.instance.score + " Candies!!";
                createProfileBtn.SetActive(true);
                leaderButton.gameObject.SetActive(false);
            }
        }


        void ShowLeaderboard()
        {
            List<PlayerData> profiles = new List<PlayerData>();
            string[] files = Directory.GetFiles(folderPath, "*.json");
            foreach (string file in files)
            {
                string json = File.ReadAllText(file);
                PlayerData data = JsonUtility.FromJson<PlayerData>(json);
                profiles.Add(data);
            }

            if (profiles.Count == 0)
            {
                Debug.Log("No players found");
            }
            profiles.Sort((a, b) => b.highScore.CompareTo(a.highScore));
            foreach (Transform child in leaderboardParent)
            {
                Destroy(child.gameObject);
            }
            
            for (int i = 0; i < profiles.Count; i++)
            {
                LeaderPlayer item = Instantiate(leaderboardItemPrefab, leaderboardParent);
                item.nameText.text = (i + 1) + ". " + profiles[i].playerName;
                item.scoreText.text = profiles[i].highScore.ToString();
            }
        }
    }
}
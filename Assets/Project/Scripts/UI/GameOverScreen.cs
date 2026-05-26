using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Project.Scripts.Data;
using Project.Scripts.UI;

namespace Project.Scripts
{
    public class GameOverScreen : MonoBehaviour
    {
        [Header("UI")]
        public TextMeshProUGUI finalScoreText;
        public TextMeshProUGUI highScoreText;

        [Header("Leaderboard")]
        public Transform leaderboardParent;
        public LeaderPlayer leaderboardItemPrefab;

        [Header("Panels")]
        public GameObject finalPanel;
        public GameObject leaderPanel;

        [Header("Buttons")]
        public GameObject leaderButton;
        public GameObject createProfileBtn;

        [Header("Decorations")]
        public GameObject img1;
        public GameObject img2;

        private string folderPath;

        private void Start()
        {
            folderPath =
                Path.Combine(
                    Application.persistentDataPath,
                    "Profiles"
                );

            // SAFETY CHECKS
            if(GameManager.instance == null)
            {
                Debug.LogError(
                    "GameManager Missing!"
                );

                return;
            }

           

            // PANEL SETUP
            if(finalPanel != null)
            {
                finalPanel.SetActive(true);
            }

            if(leaderPanel != null)
            {
                leaderPanel.SetActive(false);
            }

            // SHOW LEADERBOARD
            if(ProfileManager.currentProfile != null)
            {
                ShowLeaderboard();
            }

            // DECORATION ANIMATION
            AnimateImages();

            Debug.Log(
                "GameOverScreen Loaded"
            );
        }

        // CURRENT PLAYER UI
        public void SetupCurrentPlayerUI()
        {
            // PROFILE EXISTS
            if (ProfileManager.currentProfile != null)
            {
                if(leaderButton != null)
                {
                    leaderButton.SetActive(true);
                }

                if(createProfileBtn != null)
                {
                    createProfileBtn.SetActive(false);
                }

                if(highScoreText != null)
                {
                    highScoreText.text =
                        "High Score : " +
                        ProfileManager.currentProfile
                        .highScore;
                }

                if(finalScoreText != null)
                {
                    finalScoreText.text =
                        ProfileManager.currentProfile
                        .playerName +
                        "\nYou Collected " +
                        GameManager.instance.score +
                        " Candies!!";
                }

                Debug.Log(
                    "Current Profile Loaded : " +
                    ProfileManager.currentProfile
                    .playerName
                );
            }

            // NO PROFILE
            else
            {
                if(finalScoreText != null)
                {
                    finalScoreText.text =
                        "You Collected " +
                        GameManager.instance.score +
                        " Candies!!";
                }

                if(createProfileBtn != null)
                {
                    createProfileBtn.SetActive(true);
                }

                if(leaderButton != null)
                {
                    leaderButton.SetActive(false);
                }

                Debug.LogWarning(
                    "No Profile Selected"
                );
            }
        }

        // LEADERBOARD
        private void ShowLeaderboard()
        {
            // FOLDER CHECK
            if(!Directory.Exists(folderPath))
            {
                Debug.LogWarning(
                    "Profiles Folder Missing!"
                );

                return;
            }

            List<PlayerData> profiles =
                new List<PlayerData>();

            string[] files =
                Directory.GetFiles(
                    folderPath,
                    "*.json"
                );

            // LOAD PROFILES
            foreach (string file in files)
            {
                string json =
                    File.ReadAllText(file);

                PlayerData data =
                    JsonUtility.FromJson
                        <PlayerData>(json);

                // NULL SAFETY
                if(data != null)
                {
                    profiles.Add(data);
                }
            }

            // EMPTY LEADERBOARD
            if (profiles.Count == 0)
            {
                Debug.LogWarning(
                    "No Players Found"
                );

                return;
            }

            // SORT HIGH SCORE
            profiles.Sort(
                (a, b) =>
                    b.highScore.CompareTo(
                        a.highScore
                    )
            );

            // CLEAR OLD UI
            foreach (Transform child
                     in leaderboardParent)
            {
                Destroy(child.gameObject);
            }

            // CREATE UI ITEMS
            for (int i = 0;
                 i < profiles.Count;
                 i++)
            {
                LeaderPlayer item =
                    Instantiate(
                        leaderboardItemPrefab,
                        leaderboardParent
                    );

                // NULL CHECK
                if(item == null)
                {
                    continue;
                }

                item.nameText.text =
                    (i + 1) + ". " +
                    profiles[i].playerName;

                item.scoreText.text =
                    profiles[i]
                    .highScore
                    .ToString();
            }

            Debug.Log(
                "Leaderboard Loaded Successfully"
            );
        }

        // IMAGE ANIMATION
        private void AnimateImages()
        {
            if(img1 != null)
            {
                img1.transform
                    .DORotate(
                        new Vector3(0,0,-15f),
                        1.5f
                    )
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo)
                    .From(
                        new Vector3(0,0,15f)
                    );
            }

            if(img2 != null)
            {
                img2.transform
                    .DORotate(
                        new Vector3(0,0,-15f),
                        1.5f
                    )
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo)
                    .From(
                        new Vector3(0,0,15f)
                    );
            }
        }
    }
}
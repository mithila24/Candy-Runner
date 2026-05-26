using Project.Scripts.Data;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class ProfileUICard : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text highScoreText;
        private PlayerData profileData;

        public void Setup(PlayerData data)
        {
            profileData = data;
            nameText.text = data.playerName;
            highScoreText.text = "High Score : " + data.highScore;
        }

        public void SelectProfile()
        {
            ProfileManager.instance.SelectProfile(profileData);
            Debug.Log("Selected Profile : " + profileData.playerName);
            GameManager.instance.ResetGame();
            HomeScreenPlayButton.instance.LoadGameScene();
        }

    
    }
}
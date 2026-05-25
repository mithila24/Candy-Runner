using System.Collections;
using DG.Tweening;
using Project.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Serialization;

namespace Project.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public GameObject backButton;
        public TMP_Text scoreText;
        public GameObject ScorePanel;
        public GameObject LoadingPanel, loadingStar;
        public GameObject[] hearts;
        public int score = 0;
        public int lives = 3;
        public GameObject gameOverScreen;
        public AudioSource audioSource;
        public AudioClip noSound;
        public AudioClip UISound;
       [HideInInspector]  public bool speedUp, gameover;
        public bool changeCandyWithoutAssetbundleBuild;
        public Sprite candySprite;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            UpdateScore();
            UpdateLivesUI();
            gameOverScreen.SetActive(false);
            LoadingPanel.SetActive(false);
        }


        public void ShowBackButton(bool show)
        {
            backButton.SetActive(show);
        }

        public void ShowScoreUI(bool show)
        {
            ScorePanel.gameObject.SetActive(show);
        }

        public void ChangeScene(string scene)
        {
           StartCoroutine(LoadScene(scene));
          
        }

        public IEnumerator LoadScene(string scene)
        {
            LoadingPanel.SetActive(true);
            loadingStar.transform.DORotate(new Vector3(0, 0, -180f), 1.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            SceneManager.LoadScene(scene);
            yield return new WaitForSeconds(1f);
            LoadingPanel.SetActive(false);
        }

        public void PlayUISound()
        {
            audioSource.PlayOneShot(UISound);
        }

        public void AddScore(int amount)
        {
            score += amount;
            UpdateScore();
        }


        public void HitObstacle()
        {
            lives--;
            UpdateLivesUI();
            if (lives <= 0)
            {
                audioSource.PlayOneShot(noSound);
                GameOver();
            }
        }


        public void UpdateLivesUI()
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                hearts[i].SetActive(i < lives);
            }
        }

        public void UpdateScore()
        {
            if (scoreText != null)
            {
                scoreText.text = score.ToString();
            }
        }

        void GameOver()
        {
            var profile = ProfileManager.currentProfile;

            if (profile != null && score > profile.highScore)
            {
                profile.highScore = score;
                ProfileManager.instance.SaveCurrentProfile();
            }

            gameover = true;
            gameOverScreen.SetActive(true);
        
        }

        public void ResetGame()
        {
            score = 0;
            lives = 3;
            speedUp =  false;
            gameover = false;
            UpdateScore();
            UpdateLivesUI();
        }
    }
}
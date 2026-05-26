using DG.Tweening;
using UnityEngine;

namespace Project.Scripts
{
    public class HomeScreenPlayButton : MonoBehaviour
    {
        [Header("UI")]
        public GameObject playbtn;
        public GameObject profilePanel;

        public static HomeScreenPlayButton instance;

        private Tween playButtonTween;
        private Tween profileTween;

        private void Awake()
        {
            // SINGLETON SAFETY
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);

                return;
            }
        }

        private void Start()
        {
            // NULL SAFETY
            if(playbtn == null)
            {
                Debug.LogError("Play Button Missing!");
            }

            if(profilePanel == null)
            {
                Debug.LogError("Profile Panel Missing!");
            }

            if(GameManager.instance == null)
            {
                Debug.LogError("GameManager Missing!");
                return;
            }

            // PLAY BUTTON ANIMATION
            if(playbtn != null)
            {
                playButtonTween =
                    playbtn.transform
                    .DOScale(1f, 2f)
                    .From(0.85f)
                    .SetLoops(
                        -1,
                        LoopType.Yoyo
                    );
            }

            // PROFILE PANEL SETUP
            if(profilePanel != null)
            {
                profilePanel.transform.localScale = Vector3.zero;
                profilePanel.SetActive(true);
            }

            // UI SETUP
            GameManager.instance.ShowBackButton(false);
            GameManager.instance.ShowScoreUI(false);
            Debug.Log("HomeScreen Initialized");
        }

        // SHOW/HIDE PROFILE PANEL
        public void ShowProfile(bool show)
        {
            if(profilePanel == null)
            {
                return;
            }

            // KILL OLD TWEEN
            profileTween?.Kill();

            profileTween =
                profilePanel.transform
                .DOScale(
                    show ?
                    Vector3.one :
                    Vector3.zero,
                    0.25f
                )
                .SetEase(Ease.Linear);

            Debug.Log(
                show ?
                "Profile Panel Opened" :
                "Profile Panel Closed"
            );
        }

        // LOAD GAME
        public void LoadGameScene()
        {
            if(GameManager.instance == null)
            {
                Debug.LogError( "GameManager Missing!");
                return;
            }

            Debug.Log("Loading GameScene");
            var g = GameManager.instance;

            // RESET GAME
            g.ResetGame();

            // PLAY SOUND
            g.PlayUISound();

            // SHOW GAME UI
            g.ShowBackButton(true);

            g.ShowScoreUI(true);

            // HIDE GAMEOVER SCREEN
            if(g.gameOverScreen != null)
            {
                g.gameOverScreen.gameObject.SetActive(false);
            }

            // LOAD SCENE
            g.ChangeScene("GameScene");
        }

        private void OnDestroy()
        {
            // CLEANUP TWEENS
            playButtonTween?.Kill();
            profileTween?.Kill();
        }
    }
}
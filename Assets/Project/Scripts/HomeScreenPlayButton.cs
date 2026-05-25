using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts
{
    public class HomeScreenPlayButton : MonoBehaviour
    {
        public GameObject playbtn;
        public GameObject profilePanel;

        public static HomeScreenPlayButton instance;

        private void Start()
        {
            instance = this;
            playbtn.transform.DOScale(1f, 2f).From(0.85f).SetLoops(-1, LoopType.Yoyo);
            profilePanel.transform.localScale = Vector3.zero;
            profilePanel.gameObject.SetActive(true);
            GameManager.instance.ShowBackButton(false);
            GameManager.instance.ShowScoreUI(false);
        }

        public void ShowProfile(bool show)
        {
            profilePanel.transform.DOScale(show ? Vector3.one : Vector3.zero, 0.25f).SetEase(Ease.Linear);
        }

        public void LoadGameScene()
        {
            var g = GameManager.instance;
            g.ResetGame();
            g.PlayUISound();
            g.ShowBackButton(true);
            g.ShowScoreUI(true);
            g.gameOverScreen.gameObject.SetActive(false);
            g.ChangeScene("GameScene");
        }
    }
}
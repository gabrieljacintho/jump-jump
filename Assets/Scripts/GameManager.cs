using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [NonSerialized]
    public bool gameInProgress = false;

    public PlayerControl playerControl = null;

    public GameObject[] platforms = null;
    [SerializeField]
    private PlatformControl[] platformControls = null;
    [NonSerialized]
    public int currentPlatform = 0;
    [NonSerialized]
    public int nextPlatformPositionY = 14;

    private void Awake()
    {
        instance = this;
    }
    public void StartGame()
    {
        gameInProgress = true;
        platformControls[currentPlatform].Move();
        ScoreManager.instance.score = 0;
        UIManager.instance.PutPauseButton();
        UIManager.instance.ActivateScoreText();
        UIManager.instance.ActivateHighScoreTextAnimation();
    }

    public void GameOver()
    {
        gameInProgress = false;
        UIManager.instance.RemovePauseButton();
        UIManager.instance.ActivateScoreText();
        UIManager.instance.ActivateHighScoreTextAnimation();
        ScoreManager.instance.scoreExceeded = false;
        ScoreManager.instance.UpdateHighScore();
        AdsManager.instance.ShowAd();
    }
}

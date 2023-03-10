using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField]
    private AudioClip scoreExceededAudio = null;

    [NonSerialized]
    public int score;
    [NonSerialized]
    public bool scoreExceeded = false;

    private void Awake()
    {
        instance = this;
    }

    public void IncreaseScore()
    {
        score++;

        if (scoreExceeded || score > PlayerPrefs.GetInt("HighScore", 0))
        {
            if (!scoreExceeded && PlayerPrefs.GetInt("HighScore", 0) > 0)
            {
                scoreExceeded = true;
                UIManager.instance.ActivateHighScoreTextAnimation();
                SongsManager.instance.audioSource.PlayOneShot(scoreExceededAudio, 0.8f);
            }

            UIManager.instance.UpdateHighScoreToEqualsScore();
        }

        if (UIManager.instance.scoreText.gameObject.activeSelf) UIManager.instance.UpdateScoreText();
    }

    public void UpdateHighScore()
    {
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
            UIManager.instance.UpdateHighScoreText();
        }
    }
}

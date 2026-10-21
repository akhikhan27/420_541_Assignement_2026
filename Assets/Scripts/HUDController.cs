using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("Score & HP")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text hpText;

    [SerializeField] private TMP_Text levelTimerText;
    private float levelElapsedTime = 0f;
    private bool isTimerRunning = true;

    [SerializeField] private TMP_Text speedTimerText;
    [SerializeField] private TMP_Text jumpTimerText;
    [SerializeField] private TMP_Text scoreBoostTimerText;

    private void Update()
    {
        UpdateScore();
        UpdateHP();
        UpdateLevelTimer();
        UpdatePowerUpTimers();
    }

    private void UpdateScore()
    {
        if (scoreText != null && ScoreManager.Instance != null)
        {
            scoreText.text = "Score: " + ScoreManager.Instance.Score;
        }
    }

    private void UpdateHP()
    {
        if (hpText != null && PlayerHealth.Instance != null)
        {
            hpText.text = "HP: " + PlayerHealth.Instance.CurrentHealth + "/" + PlayerHealth.Instance.MaxHealth;
        }
    }

    private void UpdateLevelTimer()
    {
        if (!isTimerRunning || levelTimerText == null) return;

        levelElapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(levelElapsedTime / 60f);
        int seconds = Mathf.FloorToInt(levelElapsedTime % 60f);
        levelTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StopTimer() => isTimerRunning = false;

    private void UpdatePowerUpTimers()
    {
        if (PlayerPowerUps.Instance == null) return;

        if (speedTimerText != null)
        {
            if (PlayerPowerUps.Instance.SpeedTimeLeft > 0f)
            {
                speedTimerText.gameObject.SetActive(true);
                speedTimerText.text = Mathf.Ceil(PlayerPowerUps.Instance.SpeedTimeLeft) + "s";
            }
            else
            {
                speedTimerText.gameObject.SetActive(false);
            }
        }

        if (jumpTimerText != null)
        {
            if (PlayerPowerUps.Instance.JumpTimeLeft > 0f)
            {
                jumpTimerText.gameObject.SetActive(true);
                jumpTimerText.text = Mathf.Ceil(PlayerPowerUps.Instance.JumpTimeLeft) + "s";
            }
            else
            {
                jumpTimerText.gameObject.SetActive(false);
            }
        }

        if (scoreBoostTimerText != null)
        {
            if (PlayerPowerUps.Instance.ScoreBoostTimeLeft > 0f)
            {
                scoreBoostTimerText.gameObject.SetActive(true);
                scoreBoostTimerText.text = Mathf.Ceil(PlayerPowerUps.Instance.ScoreBoostTimeLeft) + "s";
            }
            else
            {
                scoreBoostTimerText.gameObject.SetActive(false);
            }
        }
    }
}
using UnityEngine;
using TMPro;

public class PowerUpHUD : MonoBehaviour
{
    [SerializeField] private PlayerPowerUps powerUps;

    [Header("Speed Boost")]
    [SerializeField] private GameObject speedIndicator;
    [SerializeField] private TMP_Text speedTimeText;
 
    [Header("Jump Boost")]
    [SerializeField] private GameObject jumpIndicator;
    [SerializeField] private TMP_Text jumpTimeText;

    [Header("Score Boost")]
    [SerializeField] private GameObject scoreBoostIndicator;
    [SerializeField] private TMP_Text scoreBoostTimeText;

    private void Start()
    {
        if (speedIndicator != null) speedIndicator.SetActive(false);
        if (jumpIndicator != null) jumpIndicator.SetActive(false);
        if (scoreBoostIndicator != null) scoreBoostIndicator.SetActive(false);
    }

    private void Update()
    {
        if (powerUps == null) return;

        // Speed
        float speedTime = powerUps.SpeedTimeLeft;
        if (speedIndicator != null) speedIndicator.SetActive(speedTime > 0f);
        if (speedTimeText != null) speedTimeText.text = Mathf.CeilToInt(speedTime) + "s";

        // Jump
        float jumpTime = powerUps.JumpTimeLeft;
        if (jumpIndicator != null) jumpIndicator.SetActive(jumpTime > 0f);
        if (jumpTimeText != null) jumpTimeText.text = Mathf.CeilToInt(jumpTime) + "s";

        // Score Boost
        float scoreBoostTime = powerUps.ScoreBoostTimeLeft;
        if (scoreBoostIndicator != null) scoreBoostIndicator.SetActive(scoreBoostTime > 0f);
        if (scoreBoostTimeText != null) scoreBoostTimeText.text = Mathf.CeilToInt(scoreBoostTime) + "s";
    }
}
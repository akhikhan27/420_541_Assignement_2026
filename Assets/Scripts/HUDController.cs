using UnityEngine;
using TMPro; // TMP_Text
 
// Updates the HUD every frame from the ScoreManager and PlayerPowerUps.
public class HUDController : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TMP_Text scoreText;
    [Header("Health")]
    [SerializeField] private TMP_Text hpText;
 
    private void Update()
    {
        UpdateScore();
        UpdateHP();
    }
 
    private void UpdateScore()
    {
        // The Score property from Lab 4: anyone can read it
        scoreText.text = "Score: " + ScoreManager.Instance.Score;
    }

    public void UpdateHP()
    {
        hpText.text = "HP: " + PlayerHealth.Instance.CurrentHealth + "/" + PlayerHealth.Instance.MaxHealth;
    }
 
}

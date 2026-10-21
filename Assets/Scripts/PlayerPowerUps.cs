using UnityEngine;
 
// Gives the player a speed boost for a limited time.
public class PlayerPowerUps : MonoBehaviour
{
    public static PlayerPowerUps Instance { get; private set; }

    [SerializeField] private float boostMultiplier = 1.5f; // 50% faster
 
    // Seconds of boost left. The HUD reads it; only this script changes it.
    public float SpeedTimeLeft { get; private set; }
    public float JumpTimeLeft { get; private set; }
    public float ScoreBoostTimeLeft { get; private set; }

    public bool IsJumpActive => JumpTimeLeft > 0f;
    public bool IsScoreBoostActive => ScoreBoostTimeLeft > 0f;
 
    private PlayerMovement movement;
 
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }
 
    // Called by the pickup. Collecting another boost restarts the timer.
    public void ActivateSpeedBoost(float duration)
    {
        SpeedTimeLeft = duration;
        if (movement != null)
        {
            movement.SpeedMultiplier = boostMultiplier;
        }
    }

    public void ActivateJumpBoost(float duration)
    {
        JumpTimeLeft = duration;
    }

    public void ActivateScoreBoost(float duration)
    {
        ScoreBoostTimeLeft = duration;
    }
 
    private void Update()
    {
        if (SpeedTimeLeft > 0f)
        {
            SpeedTimeLeft -= Time.deltaTime;
            if (SpeedTimeLeft <= 0f && movement != null)
            {
                movement.SpeedMultiplier = 1f; // back to normal speed
            }
        }

        if (JumpTimeLeft > 0f)
        {
            JumpTimeLeft -= Time.deltaTime;
        }

        if (ScoreBoostTimeLeft > 0f)
        {
            ScoreBoostTimeLeft -= Time.deltaTime;
        }
    }
}
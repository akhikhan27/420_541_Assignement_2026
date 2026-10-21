using UnityEngine;

public enum PowerUpType { Speed, Jump, ScoreBoost }

[RequireComponent(typeof(AudioSource))]
public class PowerUpPickup : MonoBehaviour
{
    [SerializeField] private PowerUpType powerUpType;
    [SerializeField] private float duration = 5f;
    [SerializeField] private float respawnDelay = 30f;
    [SerializeField] private GameObject visuals;      
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private GameObject pickupEffect; 
 
    private void OnTriggerEnter(Collider other)
    {
        PlayerPowerUps powerUps = other.GetComponent<PlayerPowerUps>();
        if (powerUps == null) return;
 
        // Switch on the Inspector enum so each item triggers its correct method
        switch (powerUpType)
        {
            case PowerUpType.Speed:
                powerUps.ActivateSpeedBoost(duration);
                break;
            case PowerUpType.Jump:
                powerUps.ActivateJumpBoost(duration);
                break;
            case PowerUpType.ScoreBoost:
                powerUps.ActivateScoreBoost(duration);
                break;
        }
 
        if (pickupSound != null)
        {
            GetComponent<AudioSource>().PlayOneShot(pickupSound);
        }
        
        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }
 
        if (visuals != null) visuals.SetActive(false);
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        
        Invoke(nameof(Reappear), respawnDelay);
    }
 
    private void Reappear()
    {
        if (visuals != null) visuals.SetActive(true);
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = true;
    }
}
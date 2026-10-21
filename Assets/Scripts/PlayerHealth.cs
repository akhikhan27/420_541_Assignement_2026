using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    [SerializeField] private float invincibilityDuration = 1.5f;
    private float lastDamageTime = -999f;

    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        MaxHealth = 3;
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (Time.time < lastDamageTime + invincibilityDuration) return;

        CurrentHealth -= damage;
        lastDamageTime = Time.time; 
        
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
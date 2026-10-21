using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if(health != null){
                health.TakeDamage(damageAmount);
            }
        }
    }
}
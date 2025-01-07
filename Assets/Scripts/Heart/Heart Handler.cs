using UnityEngine;

public class HeartHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (HealthManager.health < 3)
            {
                HealthManager.health++;
                gameObject.SetActive(false);
            }
        }
    }

}
using UnityEngine;

public class HeartHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Untagged"))
        {
            if (HealthManager.health < 3)
            {
                HealthManager.health++;
                gameObject.SetActive(false);
            }
        }
    }

}
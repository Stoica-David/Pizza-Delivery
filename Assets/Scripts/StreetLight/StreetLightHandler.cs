using UnityEngine;

public class StreetLightHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (HealthManager.health > 0)
            {
                Debug.Log("FROM STREETLIGHTHANDLER");

                HealthManager.health--;
                gameObject.SetActive(false);
                CarHandler.isHurt = true;
            }
        }
    }

}
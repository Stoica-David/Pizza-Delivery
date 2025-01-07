using UnityEngine;

public class PizzaHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CarHandler.instance.ShowBottomPizzaBox();
            gameObject.SetActive(false);

        }
    }

}
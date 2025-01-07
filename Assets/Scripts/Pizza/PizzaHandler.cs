using UnityEngine;

public class PizzaHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Untagged"))
        {
            CarHandler.instance.ShowBottomPizzaBox();
            gameObject.SetActive(false);

        }
    }

}
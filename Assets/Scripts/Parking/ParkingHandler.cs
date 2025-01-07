using UnityEngine;

public class ParkingZone : MonoBehaviour
{
    private bool isCarParked = false;
    private float parkedTime = 0f;
    private float requiredParkTime = 2f; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Untagged")) 
        {
            isCarParked = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isCarParked && other.CompareTag("Untagged"))
        {
            parkedTime += Time.deltaTime;

            if (parkedTime >= requiredParkTime)
            {
                ScoreManager.instance.ModifyPoints(50);
                ScoreManager.instance.AddPizza();
                CarHandler.instance.HideTopPizzaBox();
                isCarParked = false;
                parkedTime = 0f;
                gameObject.SetActive(false); 
             
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Untagged"))
        {
            isCarParked = false;
            parkedTime = 0f;
        }
    }
}
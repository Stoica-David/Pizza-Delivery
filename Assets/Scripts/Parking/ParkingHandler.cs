﻿using System.Collections;
using UnityEngine;

public class ParkingZone : MonoBehaviour
{
    //[SerializeField]
    AudioSource deliveredAS;

    private bool isCarParked = false;
    private float parkedTime = 0f;
    private float requiredParkTime = 2f;

    void Awake()
    {
        deliveredAS = GetComponent<AudioSource>();
    }

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
                CarHandler.instance.HideTopPizzaBox();
                isCarParked = false;
                parkedTime = 0f;

                //gameObject.SetActive(false);
                StartCoroutine(PlayAndDeactivate());

            }
        }
    }

    IEnumerator PlayAndDeactivate()
    {
        deliveredAS.pitch = 1.0f;
        deliveredAS.volume = 1.0f;
        deliveredAS.Play();
        yield return new WaitForSeconds(deliveredAS.clip.length);
        gameObject.SetActive(false);
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
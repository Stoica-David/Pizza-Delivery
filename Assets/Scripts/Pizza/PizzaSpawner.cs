using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] pizzaPrefab; // Prefab-urile pentru zonele de parcare

    GameObject[] pizzaPool = new GameObject[40]; // Pool-ul de zone de parcare

    Transform playerCarTransform;

    // Timing
    float timeLastZoneSpawned = 0;
    WaitForSeconds wait = new WaitForSeconds(20.0f); // Interval de așteptare între generări

    // Start is called before the first frame update
    void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;

        int prefabIndex = 0;

        // Inițializăm pool-ul de zone de parcare
        for (int i = 0; i < pizzaPool.Length; i++)
        {
            pizzaPool[i] = Instantiate(pizzaPrefab[prefabIndex]);
            pizzaPool[i].SetActive(false);

            prefabIndex++;

            // Reîncepem de la începutul listei de prefabricate dacă am ajuns la capăt
            if (prefabIndex > pizzaPrefab.Length - 1)
                prefabIndex = 0;
        }

        StartCoroutine(UpdateLessOftenCO());
    }

    IEnumerator UpdateLessOftenCO()
    {
        while (true)
        {
            CleanUpZonesBeyondView();
            SpawnNewParkingZones();

            yield return wait;
        }
    }

    void SpawnNewParkingZones()
    {
        // Evităm generarea prea frecventă
        if (Time.time - timeLastZoneSpawned < 3)
            return;

        GameObject zoneToSpawn = null;

        // Găsim o zonă de parcare inactivă în pool
        foreach (GameObject parkingZone in pizzaPool)
        {
            if (parkingZone.activeInHierarchy)
                continue;

            zoneToSpawn = parkingZone;
            break;
        }

        // Dacă nu există zone disponibile, ieșim din funcție
        if (zoneToSpawn == null)
            return;

        float fixedX = Random.value > 0.5f ? -2f : 2f;
        float fixedY = 0.2f;
        float spawnZ = playerCarTransform.position.z + 50;

        Vector3 spawnPosition = new Vector3(fixedX, fixedY, spawnZ);

        // Setăm poziția și activăm zona de parcare
        zoneToSpawn.transform.position = spawnPosition;
        zoneToSpawn.SetActive(true);

        timeLastZoneSpawned = Time.time;
    }

    void CleanUpZonesBeyondView()
    {
        foreach (GameObject parkingZone in pizzaPool)
        {
            // Sărim peste zonele inactive
            if (!parkingZone.activeInHierarchy)
                continue;

            // Dezactivăm zonele de parcare prea departe de jucător
            if (parkingZone.transform.position.z - playerCarTransform.position.z > 200)
                parkingZone.SetActive(false);

            if (parkingZone.transform.position.z - playerCarTransform.position.z < -50)
                parkingZone.SetActive(false);
        }
    }
}

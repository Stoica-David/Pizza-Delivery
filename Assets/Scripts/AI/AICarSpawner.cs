using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AICarSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] carAIPrefabs;

    GameObject[] carAIPool;

    Transform playerCarTransform;

    //Timing
    float timeLastCarSpawned = 0;
    WaitForSeconds wait;

    //Overlapped check
    [SerializeField]
    LayerMask otherCarsLayerMask;
    Collider[] overlappedCheckCollider = new Collider[1];

    // Start is called before the first frame update
    void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;

        //check current scene
        if(SceneManager.GetActiveScene().name == "Level 1")
        {
            carAIPool = new GameObject[40];
            WaitForSeconds wait = new WaitForSeconds(0.5f);
        }
        else if (SceneManager.GetActiveScene().name == "Level 2")
        {
            carAIPool = new GameObject[60];
            WaitForSeconds wait = new WaitForSeconds(0.3f);
        }
        else if (SceneManager.GetActiveScene().name == "Level 3")
        {
            carAIPool = new GameObject[80];
            WaitForSeconds wait = new WaitForSeconds(0.2f);
        }
        else
        {
            carAIPool = new GameObject[40];
            WaitForSeconds wait = new WaitForSeconds(0.2f);
        }

        int prefabIndex = 0;

        //add console comments about the carAIPrefabs array length
        Debug.Log("carAIPrefabs.Length: " + carAIPrefabs.Length);


        for (int i = 0; i < carAIPool.Length; i++)
        {
            carAIPool[i] = Instantiate(carAIPrefabs[prefabIndex]);
            carAIPool[i].SetActive(false);

            prefabIndex++;

            //Loop the prefab index if we run out of prefabs
            if (prefabIndex > carAIPrefabs.Length - 1)
                prefabIndex = 0;
        }

        StartCoroutine(UpdateLessOftenCO());
    }

    IEnumerator UpdateLessOftenCO()
    {
        while (true)
        {
            CleanUpCarsBeyondView();
            SpawnNewCars();

            yield return wait;
        }
    }

    void SpawnNewCars()
    {
        if (Time.time - timeLastCarSpawned < 2)
            return;

        GameObject carToSpawn = null;

        //Find a car to spawn
        foreach (GameObject aiCar in carAIPool)
        {
            //Skip active cars
            if (aiCar.activeInHierarchy)
                continue;

            carToSpawn = aiCar;
            break;
        }

        //No car available to spawn
        if (carToSpawn == null)
            return;

        Vector3 spawnPosition = new Vector3(0, 0, playerCarTransform.transform.position.z + 50);

        if(Physics.OverlapBoxNonAlloc(spawnPosition, Vector3.one *2, overlappedCheckCollider, Quaternion.identity, otherCarsLayerMask) > 0)
            return;

        carToSpawn.transform.position = spawnPosition;
        carToSpawn.SetActive(true);

        timeLastCarSpawned = Time.time;
    }

    void CleanUpCarsBeyondView()
    {
        foreach (GameObject aiCar in carAIPool)
        {
            //Skip inactive cars
            if (!aiCar.activeInHierarchy)
                continue;

            //Check if AI car is too far ahead
            if (aiCar.transform.position.z - playerCarTransform.position.z > 200)
                aiCar.SetActive(false);

            //Check if AI car is too far behind
            if (aiCar.transform.position.z - playerCarTransform.position.z < -50)
                aiCar.SetActive(false);
        }
    }


}

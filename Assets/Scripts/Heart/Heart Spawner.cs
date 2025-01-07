using System.Collections;
using UnityEngine;

public class HeartSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] heartPrefabs;

    GameObject[] heartPool = new GameObject[10];

    Transform playerCarTransform;

    float timeLastHeartSpawned = 0;
    WaitForSeconds wait = new WaitForSeconds(0.5f);

    void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;

        int prefabIndex = 0;

        Debug.Log("heartPrefabs.Length: " + heartPrefabs.Length);

        for (int i = 0; i < heartPrefabs.Length; i++)
        {
            heartPool[i] = Instantiate(heartPrefabs[prefabIndex]);
            heartPool[i].SetActive(false);

            prefabIndex++;

            if (prefabIndex >= heartPrefabs.Length - 1)
                prefabIndex = 0;
        }

        StartCoroutine(UpdateLessOftenCO());
    }

    IEnumerator UpdateLessOftenCO()
    {
        while(true)
        {
            CleanUpHeartsBeyondView();
            SpawnNewHearts();

            yield return wait;
        }
    }

    void SpawnNewHearts()
    {
        if (Time.time - timeLastHeartSpawned < 3)
            return;

        GameObject heartToSpawn = null;

        foreach (GameObject heart in heartPool)
        {
            if (heart.activeInHierarchy)
                continue;

            heartToSpawn = heart;
            break;
        }

        if (heartToSpawn == null)
            return;

        float fixedX = Random.value > 0.5f ? -2f : 2f;
        float fixedY = 0.2f;
        float spawnZ = playerCarTransform.position.z + 50;

        Vector3 spawnPosition = new Vector3(fixedX, fixedY, spawnZ);

        Debug.Log("Spawned Heart Pos: " +  spawnPosition);

        heartToSpawn.transform.position = spawnPosition;
        heartToSpawn.SetActive(true);

        timeLastHeartSpawned = Time.time;
    }

    void CleanUpHeartsBeyondView()
    {
        foreach (GameObject heart in heartPool)
        {
            //Skip inactive hearts
            if (!heart.activeInHierarchy)
                continue;

            //Check if heart is too far ahead
            if (heart.transform.position.z - playerCarTransform.position.z > 200)
                heart.SetActive(false);

            //Check if heart is too far behind
            if (heart.transform.position.z - playerCarTransform.position.z < -50)
                heart.SetActive(false);
        }
    }
}

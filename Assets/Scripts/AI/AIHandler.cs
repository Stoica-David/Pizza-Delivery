using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHandler : MonoBehaviour
{
    [SerializeField]
    CarHandler carHandler;

    //Collision detection
    [SerializeField]
    LayerMask otherCarsLayerMask;

    [SerializeField]
    MeshCollider meshCollider;

    [Header("SFX")]
    [SerializeField]
    AudioSource honkAS;

    RaycastHit[] raycastHits = new RaycastHit[1];
    bool isCarAhead = false;
    float carAheadDistance = 0f;

    //Lanes
    int drivingInLane = 0;

    //Timing
    WaitForSeconds wait = new WaitForSeconds(0.2f);

    private void Awake()
    {
        if (CompareTag("Player"))
        {
            Destroy(this);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateLessOftenCO());
    }

    // Update is called once per frame
    void Update()
    {
        float accelerationInput = 1.0f;
        float steerInput = 0.0f;

        if (isCarAhead)
        {
            accelerationInput = -1;
            if(!honkAS.isPlaying)
            {
                honkAS.volume = Random.Range(0.2f, 0.6f);
                honkAS.pitch = Random.Range(0.8f, 1.2f);
                honkAS.Play();
            }
        }

        //Randomly add a car either on right or left lane
        
        float desiredPositionX = Utils.CarLanes[drivingInLane];

        float difference = desiredPositionX - transform.position.x;

        if (Mathf.Abs(difference) > 0.05f)
            steerInput = 1.0f * difference;

        steerInput = Mathf.Clamp(steerInput, -1.0f, 1.0f);

        carHandler.SetInput(new Vector2(steerInput, accelerationInput));
    }

    IEnumerator UpdateLessOftenCO()
    {
        while (true)
        {
            isCarAhead = CheckIfOtherCarsIsAhead();
            yield return wait;
        }
    }

    bool CheckIfOtherCarsIsAhead()
    {
        meshCollider.enabled = false;

        int numberOfHits = Physics.BoxCastNonAlloc(transform.position, Vector3.one * 0.25f, transform.forward, raycastHits, Quaternion.identity, 2, otherCarsLayerMask);

        meshCollider.enabled = true;

        if (numberOfHits > 0)
        {
            carAheadDistance = (transform.position - raycastHits[0].point).magnitude;
            return true;
        }


        return false;
    }

    //Events
    private void OnEnable()
    {
        //Set random speed
        carHandler.SetMaxSpeed(Random.Range(2, 4));

        //Set a random lane
        drivingInLane = Random.Range(0, Utils.CarLanes.Length);
    }
}

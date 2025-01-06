using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHandler : MonoBehaviour
{
    public static CarHandler instance;

    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    Transform gameModel;

    [SerializeField]
    MeshRenderer carMeshRender;

    [SerializeField]
    ExplodeHandler explodeHandler;

    [Header("SFX")]
    [SerializeField]
    AudioSource carEngineAS;

    [SerializeField]
    AnimationCurve carPitchAC;

    [SerializeField]
    AudioSource carSkidAS;

    [SerializeField]
    AudioSource carCrashAS;

    //Max values
    float maxSteerVelocity = 2;
    float maxForwardVelocity = 30;
    int maxScoreUpdate = 500;
    float carMaxSpeedPercentage = 0;

    //Multipliers
    float accelerationMultiplier = 3;
    float breaksMultiplier = 15;
    float steeringMultiplier = 5;

    //Input
    Vector2 input = Vector2.zero;

    private Vector3 previousPosition; 

    //Emissive property
    int _EmissionColor = Shader.PropertyToID("_EmissionColor");
    Color emissiveColor = Color.white;
    float emissiveColorMultiplier = 0f;

    //Exploded state
    bool isExploded = false;

    bool isPlayer = true;

    bool areLightsOn = false;

    bool canCollideWithAll = true;

    private void Awake()
    {
        instance = this;
        previousPosition = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        isPlayer = CompareTag("Player");

        if (isPlayer)
        {
            HealthManager.health = 3;
            carEngineAS.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isExploded)
        { 
            FadeOutCarAudio();
            return;
        }

        //Rotate car model when "turning"
        gameModel.transform.rotation = Quaternion.Euler(0, rb.linearVelocity.x * 5, 0);

        if (Input.GetKeyDown(KeyCode.E))
        {
            areLightsOn = !areLightsOn;
        }
        
        if (carMeshRender != null)
        {
            float desiredCarEmissiveColorMultiplier = 0f;

            if (input.y < 0 || areLightsOn)
                desiredCarEmissiveColorMultiplier = 4.0f;

            emissiveColorMultiplier = Mathf.Lerp(emissiveColorMultiplier, desiredCarEmissiveColorMultiplier, Time.deltaTime * 4);

            carMeshRender.material.SetColor(_EmissionColor, emissiveColor * emissiveColorMultiplier);
        }

        previousPosition = transform.position;

        UpdateCarAudio();
    }

    private void FixedUpdate()
    {
        //Is exploded
        if (isExploded)
        {
            //Apply drag
            rb.linearDamping = rb.linearVelocity.z * 0.1f;
            rb.linearDamping = Mathf.Clamp(rb.linearDamping, 1.5f, 10);

            //Move towards after a the car has exploded
            rb.MovePosition(Vector3.Lerp(transform.position, new Vector3(0, 0, transform.position.z), Time.deltaTime * 0.5f));

            return;
        }

        //Apply Acceleration
        if (input.y > 0)
            Accelerate();
        else
            rb.linearDamping = 0.2f;

        //Apply Brakes
        if (input.y < 0)
            Brake();

        Steer();

        //Force the car not to go backwards
        if (rb.linearVelocity.z <= 0)
            rb.linearVelocity = Vector3.zero;

        float multiplier = transform.position.z - previousPosition.z;
        if (multiplier >= 0.02189209 && !isExploded)
        {
            ScoreManager.instance.ModifyPoints(5 * Math.Min(Math.Max(1, (int)(multiplier * 10)), maxScoreUpdate));
        }
    }

    void Accelerate()
    {
        rb.linearDamping = 0;

        //Stay within the speed limit
        if (rb.linearVelocity.z >= maxForwardVelocity)
            return;

        rb.AddForce(rb.transform.forward * accelerationMultiplier * input.y);
    }

    void Brake()
    {
        //Don't brake unless we are going forward
        if (rb.linearVelocity.z <= 0)
            return;

        rb.AddForce(rb.transform.forward * breaksMultiplier * input.y);
    }

    void Steer()
    {
        if (Mathf.Abs(input.x) > 0)
        {
            //Move the car sideways
            float speedBaseSteerLimit = rb.linearVelocity.z / 5.0f;
            speedBaseSteerLimit = Mathf.Clamp01(speedBaseSteerLimit);

            rb.AddForce(rb.transform.right * steeringMultiplier * input.x * speedBaseSteerLimit);

            //Normalize the X Velocity
            float normalizedX = rb.linearVelocity.x / maxSteerVelocity;

            //Ensure that we don't allow it to get bigger than 1 in magnitued. 
            normalizedX = Mathf.Clamp(normalizedX, -1.0f, 1.0f);

            //Make sure we stay within the turn speed limit
            rb.linearVelocity = new Vector3(normalizedX * maxSteerVelocity, 0, rb.linearVelocity.z);
        }
        else 
        {
            //Auto center car
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, new Vector3(0, 0, rb.linearVelocity.z), Time.fixedDeltaTime * 3);
        }
    }

    void UpdateCarAudio()
    {
        if(!isPlayer) 
            return;

        float carMaxSpeed = rb.linearVelocity.z / maxForwardVelocity;

        carEngineAS.pitch = carPitchAC.Evaluate(carMaxSpeed);

        if (input.y < 0 && carMaxSpeed > 0.2f)
        {
            if (!carSkidAS.isPlaying)
                carSkidAS.Play();
            carSkidAS.volume = Mathf.Lerp(carSkidAS.volume, 1, Time.deltaTime * 10);
        }
        else
        {
            carSkidAS.volume = Mathf.Lerp(carSkidAS.volume, 0, Time.deltaTime * 30);
        }
    }

    void FadeOutCarAudio()
    {
        if(!isPlayer) return;
        carEngineAS.volume = Mathf.Lerp(carEngineAS.volume, 0, Time.deltaTime * 10);
        carSkidAS.volume = Mathf.Lerp(carSkidAS.volume, 0, Time.deltaTime * 10);
    }

    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();

        input = inputVector;
    }

    public void SetMaxSpeed(float newMaxSpeed)
    {
        maxForwardVelocity = newMaxSpeed;
    }

    IEnumerator SlowDownTimeCO()
    {
        while (Time.timeScale > 0.2f)
        {
            Time.timeScale -= Time.deltaTime * 2;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        while (Time.timeScale <= 1.0f)
        {
            Time.timeScale += Time.deltaTime;

            yield return null;
        }

        Time.timeScale = 1.0f;
    }

    public void RestoreTimeCO()
    {
        Time.timeScale = 1.0f;
    }

    //Events
    private void OnCollisionEnter(Collision collision)
    {
        if (!isPlayer)
        {
            if (collision.transform.root.CompareTag("Untagged"))
                return;

            if (collision.transform.root.CompareTag("CarAI"))
                return;

            gameObject.SetActive(false);

            return;
        }

        if (!canCollideWithAll)
        {
            if (collision.transform.root.CompareTag("CarAI"))
                return;

            if (collision.transform.root.CompareTag("CarPart"))
                return;

            // ADD HERE ALL TAGS AGAINST WHICH THE CAR MUST BE IMMUNE
        }
        else
        {
            HealthManager.health -= 1;
            ScoreManager.instance.ModifyPoints(-300);


            if (HealthManager.health <= 0)
            {
                Vector3 velocity = rb.linearVelocity;
                explodeHandler.Explode(velocity * 45);

                isExploded = true;

                StartCoroutine(SlowDownTimeCO());
            }
            else
            {
                StartCoroutine(GetHurt());
            }
        }

        carCrashAS.volume = carMaxSpeedPercentage;
        carCrashAS.volume = Mathf.Clamp(carCrashAS.volume, 0.2f, 1);

        carCrashAS.pitch = carMaxSpeedPercentage;
        carCrashAS.pitch = Mathf.Clamp(carCrashAS.pitch, 0.2f, 1);

        carCrashAS.Play();
    }

    IEnumerator GetHurt()
    {
        canCollideWithAll = false;
        GetComponent<Animator>().SetLayerWeight(1, 1);
        yield return new WaitForSeconds(4);
        canCollideWithAll = true;
        GetComponent<Animator>().SetLayerWeight(1, 0);
    }
}

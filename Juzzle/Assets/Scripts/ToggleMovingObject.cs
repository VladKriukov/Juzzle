using UnityEngine;

[RequireComponent(typeof(TimeBody))]
public class ToggleMovingObject : MonoBehaviour, Link
{

    [SerializeField] Transform positionOn;
    [SerializeField] Transform positionOff;
    [SerializeField] float speed = 5f;
    [SerializeField] float accelerationSpeed = 4f;

    public bool active;

    TimeBody timeBody;

    float currentSpeed;
    Vector3 destination;
    public Vector3 deltaMovement;
    Vector3 previousPosition;

    AudioSource audioSource;

    void Awake()
    {
        timeBody = GetComponent<TimeBody>();
        audioSource = GetComponent<AudioSource>();
        if (active)
        {
            transform.position = positionOn.position;
            destination = positionOn.position;
        }
        else
        {
            transform.position = positionOff.position;
            destination = positionOff.position;
        }
        deltaMovement = transform.position;
    }

    void FixedUpdate()
    {
        if (timeBody.isRewinding == false)
        {
            /*
            if (active)
            {
                currentSpeed += accelerationSpeed * Time.deltaTime;
            }
            else
            {
                currentSpeed -= accelerationSpeed * Time.deltaTime;
            }

            currentSpeed = Mathf.Clamp(currentSpeed, 0, speed);*/
            //Activate(active);
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
            if (Vector3.Distance(transform.position, destination) <= 0.1f) audioSource.enabled = false;
        }
        deltaMovement = transform.position - previousPosition;
        previousPosition = transform.position;
    }

    // todo: make rotatable

    public void Activate(bool b)
    {
        active = b;
        if (b == false)
        {
            destination = positionOff.position;
        }
        else
        {
            destination = positionOn.position;
        }
        audioSource.enabled = true;
    }

    public void Lock(bool b)
    {
        timeBody.isRewindable = !b;
    }

}

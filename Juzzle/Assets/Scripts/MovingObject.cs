using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TimeBody))]
public class MovingObject : MonoBehaviour, Link
{

    [SerializeField] Transform destinations;
    [SerializeField] float speed = 5f;
    [SerializeField] float accelerationSpeed = 2f;
    [SerializeField] float waitTime;
    public bool active;
    List<Transform> points = new List<Transform>();

    public Vector3 deltaMovement;
    Vector3 previousPosition;

    Vector3 destination;
    Quaternion targetRotation;
    [HideInInspector] public int index;
    bool isWaiting;
    float timer;
    float currentSpeed;

    TimeBody timeBody;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        timeBody = GetComponent<TimeBody>();
        foreach (Transform item in destinations)
        {
            points.Add(item);
        }
        destination = points[0].position;
        targetRotation = points[0].rotation;
        transform.position = points[0].position;
        deltaMovement = transform.position;
    }

    void FixedUpdate()
    {
        if (timeBody.isRewinding == false)
        {
            if (active == true)
            {
                MoveToPoint();
                audioSource.enabled = true;
            }
            else
            {
                currentSpeed -= accelerationSpeed * Time.deltaTime;
                currentSpeed = Mathf.Clamp(currentSpeed, 0, speed);
                transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * currentSpeed);
                audioSource.enabled = false;
            }
        }
        deltaMovement = transform.position - previousPosition;
        previousPosition = transform.position;
    }

    void MoveToPoint()
    {
        currentSpeed += accelerationSpeed * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, speed);
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * currentSpeed);
        //transform.eulerAngles = Vector3.RotateTowards(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z), targetRotation.eulerAngles, speed * Time.deltaTime * Mathf.Deg2Rad, speed * Time.deltaTime);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * currentSpeed * 10f);

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (Vector3.Distance(transform.position, destination) <= 0.1f && Vector3.Distance(transform.eulerAngles,  targetRotation.eulerAngles) < 1f && isWaiting == false)
        {
            timer = waitTime;
            isWaiting = true;
            audioSource.Stop();
        }

        if (timer <= 0 && Vector3.Distance(transform.position, destination) <= 0.1f && Vector3.Distance(transform.eulerAngles, targetRotation.eulerAngles) < 1f)
        {
            NextPoint();
        }
    }

    void NextPoint()
    {
        index++;
        if (index >= points.Count)
        {
            index = 0;
        }
        destination = points[index].position;
        targetRotation = points[index].rotation;
        isWaiting = false;
        audioSource.Play();
    }

    public void Activate(bool b)
    {
        active = b;
    }

    public void Lock(bool b)
    {
        timeBody.isRewindable = !b;
    }
}

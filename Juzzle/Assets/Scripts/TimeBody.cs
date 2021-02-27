using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{

    public bool isRewindable = true;
    public bool isRewinding { get; private set; }

    List<PointInTime> pointsInTime;

    Rigidbody rb;
    Lockable lockable;
    MovingObject movingObject;

    void Start()
    {
        pointsInTime = new List<PointInTime>();
        if (GetComponent<Rigidbody>() != null)
            rb = GetComponent<Rigidbody>();
        if (GetComponent<Lockable>() != null)
            lockable = GetComponent<Lockable>();
        if (GetComponent<MovingObject>() != null)
            movingObject = GetComponent<MovingObject>();
    }

    void OnDisable()
    {
        RewindReset.OnClearRecording -= RewindReset_OnClearRecording;
        Game.timeBodies.Remove(this);
    }

    void OnEnable()
    {
        RewindReset.OnClearRecording += RewindReset_OnClearRecording;
        Game.timeBodies.Add(this);
    }

    void RewindReset_OnClearRecording()
    {
        pointsInTime.Clear();
    }

    void FixedUpdate()
    {
        if (isRewindable)
        {
            if (isRewinding)
                Rewind();
            else
            {
                Record();
            }
        }
    }

    void Rewind()
    {
        if (pointsInTime.Count > 0)
        {
            transform.position = pointsInTime[0].position;
            transform.rotation = pointsInTime[0].rotation;
            if (lockable != null)
            {
                lockable.activated = pointsInTime[0].isOn;
            }
            if (movingObject)
            {
                movingObject.index = pointsInTime[0].positionIndex;
            }
            pointsInTime.RemoveAt(0);
            if (pointsInTime.Count > 0)
            {
                // 2x speed rewind
                pointsInTime.RemoveAt(0);
            }
        }
        else
        {
            SetRewinding(false);
            Debug.Log("No more to rewind to");
        }
    }

    void Record()
    {
        if (pointsInTime.Count > Mathf.Round(Game.recordTime / Time.fixedDeltaTime))
        {
            Debug.Log("Gone past recording limit");
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }

        if (lockable != null)
        {
            pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, lockable.activated));
        }
        else if (movingObject != null)
        {
            pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, movingObject.index));
        }
        else
        {
            pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
        }
    }

    public void SetRewinding(bool b)
    {
        if (isRewindable)
        {
            isRewinding = b;
            if (rb && !GetComponent<RigidbodyActivator>())
                rb.isKinematic = b;
        }
    }

    public int PointsInTimeLeft()
    {
        return pointsInTime.Count;
    }

}

using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, Interractable
{

    public bool isOn = true;
    [SerializeField] bool locked;
    [SerializeField] List<GameObject> links = new List<GameObject>();

    Animator animator;
    TimeBody timeBody;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        timeBody = GetComponent<TimeBody>();
        CheckOn();
        CheckLocked();
    }

    public void Interract()
    {
        isOn = !isOn;
        CheckOn();
    }

    public void Lock()
    {
        locked = !locked;
        timeBody.isRewindable = !locked;
        CheckLocked();
    }

    void Update()
    {
        CheckOn();
    }

    void CheckOn()
    {
        foreach (var item in links)
        {
            if (item.GetComponent<Link>() != null)
            {
                item.GetComponent<Link>().Activate(isOn);
            }
        }
        animator.SetBool("IsOn", isOn);
    }

    void CheckLocked()
    {
        foreach (var item in links)
        {
            if (item.GetComponent<ToggleMovingObject>() != null)
            {
                item.GetComponent<ToggleMovingObject>().Lock(locked);
            }
        }
    }

}

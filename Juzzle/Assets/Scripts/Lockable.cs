using UnityEngine;

[RequireComponent(typeof(TimeBody))]
public class Lockable : MonoBehaviour
{

    public bool locked;
    public bool activatable;
    public bool activated;

    [SerializeField] GameObject[] links;

    public UnityEngine.Events.UnityEvent OnLock;
    public UnityEngine.Events.UnityEvent OnUnlock;

    public UnityEngine.Events.UnityEvent OnActivate;
    public UnityEngine.Events.UnityEvent OnDeactivate;

    TimeBody timeBody;

    Animator animator;

    void Start()
    {
        if (GetComponent<Animator>() != null) animator = GetComponent<Animator>();
        timeBody = GetComponent<TimeBody>();
        foreach (var item in links)
        {
            if (item.GetComponent<Link>() != null)
                item.GetComponent<Link>().Lock(locked);
        }
    }

    void Update()
    {
        if (!locked)
        {
            if (animator) animator.SetBool("IsOn", activated);
            foreach (var item in links)
            {
                if (item.GetComponent<Link>() != null)
                    item.GetComponent<Link>().Activate(activated);
            }
        }
    }

    public void Lock()
    {
        locked = !locked;
        timeBody.isRewindable = !locked;
        FireLockEvent();
    }

    public void Activate()
    {
        if (activatable)
        {
            activated = !activated;
            FireActivateEvent();
        }
    }

    void FireLockEvent()
    {
        if (locked == true)
        {
            OnLock.Invoke();
        }
        else
        {
            OnUnlock.Invoke();
        }
        foreach (var item in links)
        {
            if (item.GetComponent<Link>() != null)
                item.GetComponent<Link>().Lock(locked);
        }
    }

    void FireActivateEvent()
    {
        if (activated == true)
        {
            OnActivate.Invoke();
        }
        else
        {
            OnDeactivate.Invoke();
        }
        foreach (var item in links)
        {
            if (item.GetComponent<Link>() != null)
                item.GetComponent<Link>().Activate(activated);
        }
        if (animator) animator.SetBool("IsOn", activated);
    }

}

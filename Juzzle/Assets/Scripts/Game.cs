using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Game : MonoBehaviour
{
    public static float recordTime = 30f;

    [SerializeField] PostProcessVolume rewindPostProcessing;

    Animator animator;

    public static List<TimeBody> timeBodies = new List<TimeBody>();

    TimeBody playerTimeBody;
    AudioSource playerAudioSource;

    void OnDisable()
    {
        PlayerMovement.OnDie -= PlayerMovement_OnDie;
    }

    void OnEnable()
    {
        animator = GetComponent<Animator>();
        playerTimeBody = transform.GetChild(0).GetComponent<TimeBody>();
        playerAudioSource = playerTimeBody.GetComponent<AudioSource>();
        PlayerMovement.OnDie += PlayerMovement_OnDie;
    }

    public void DoTransition()
    {
        animator.SetTrigger("LevelTransition");
        transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<ParticleSystem>().Play();
    }

    void PlayerMovement_OnDie()
    {
        SetRewinding(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetRewinding(true);
            playerAudioSource.Play();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            SetRewinding(false);
            rewindPostProcessing.weight = 0;
        }
        if (animator.GetBool("Rewinding"))
        {
            rewindPostProcessing.weight = Random.Range(0.8f, 1);
            if (playerTimeBody.PointsInTimeLeft() <= 1)
            {
                SetRewinding(false);
                rewindPostProcessing.weight = 0;
            }
        }
    }
    void SetRewinding(bool b)
    {
        animator.SetBool("Rewinding", b);
        foreach (var item in timeBodies)
        {
            item.SetRewinding(b);
        }
        if (b == false)
        {
            playerAudioSource.Stop();
        }
    }
}

public interface Interractable
{
    void Interract();
    void Lock();
}

public interface Link
{
    void Activate(bool b);
    void Lock(bool b);
}

public interface LevelActivatable
{
    void Activate();
}
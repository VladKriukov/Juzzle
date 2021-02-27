using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TimeBody))]
public class RigidbodyActivator : MonoBehaviour, LevelActivatable
{

    [SerializeField] ParticleSystem collisionParticles;
    [SerializeField] AudioClip[] hitSounds;
    Rigidbody rb;

    AudioSource audioSource;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public void Activate()
    {
        rb.isKinematic = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collisionParticles)
        {
            collisionParticles.transform.position = collision.contacts[0].point;
            collisionParticles.Play();
        }
        if (GetComponent<AudioSource>() != null)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = hitSounds[Random.Range(0, hitSounds.Length)];
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.volume = Random.Range(0.7f, 1f);
            audioSource.Play();
        }
    }
}

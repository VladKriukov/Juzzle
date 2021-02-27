using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RewindReset : MonoBehaviour
{

    [SerializeField] ParticleSystem particles;
    [SerializeField] TimeBody[] previousLevelTimeBodies;
    [SerializeField] TimeBody[] nextLevelActivatables;
    [SerializeField] AudioClip[] partyHorns;

    public delegate void ClearRecording();
    public static event ClearRecording OnClearRecording;

    public UnityEngine.Events.UnityEvent PublicOnActivated;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnClearRecording?.Invoke();
            GetComponent<BoxCollider>().enabled = false;
            foreach (var item in previousLevelTimeBodies)
            {
                item.isRewindable = false;
            }
            foreach (var item in nextLevelActivatables)
            {
                if (item.GetComponent<LevelActivatable>() != null)
                {
                    item.GetComponent<LevelActivatable>().Activate();
                }
            }
            particles.GetComponent<AudioSource>().clip = partyHorns[Random.Range(0, partyHorns.Length)];
            particles.gameObject.SetActive(true);
            PublicOnActivated.Invoke();
        }
    }

}

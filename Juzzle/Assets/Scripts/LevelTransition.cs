using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider))]
public class LevelTransition : MonoBehaviour
{

    [SerializeField] LevelTransition previousLevelTransition;
    [SerializeField] LevelTransition nextLevelTransition;
    [SerializeField] GameObject thisLevel;
    [SerializeField] GameObject nextLevel;

    Animator animator;
    BoxCollider boxCollider;

    Game game;

    void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            game = other.transform.parent.GetComponent<Game>();
            boxCollider.enabled = false;
            Invoke(nameof(StartTransition), 0.25f);
        }
    }

    void StartTransition()
    {
        animator.SetTrigger("StartTransition");
        Invoke(nameof(EnableTransitionFX), 1.5f);
    }

    void EnableTransitionFX()
    {
        game.DoTransition();
    }

    void EndTransition()
    {
        if (previousLevelTransition)
            previousLevelTransition.gameObject.SetActive(false);

        if (nextLevelTransition)
            nextLevelTransition.gameObject.SetActive(true);

        if (nextLevel)
        {
            thisLevel.SetActive(false);
            nextLevel.SetActive(true);
        }
    }

}

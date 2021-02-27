using UnityEngine;

public class Interractor : MonoBehaviour
{

    [SerializeField] float interractDistance = 5f;
    
    RaycastHit hit;

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interractDistance) && hit.transform.GetComponent<Lockable>() != null)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if (Input.GetMouseButtonDown(0))
            {
                hit.transform.GetComponent<Lockable>().Activate();
            }
            if (Input.GetMouseButtonDown(1))
            {
                hit.transform.GetComponent<Lockable>().Lock();
            }
        }
    }

}

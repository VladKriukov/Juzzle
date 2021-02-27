using UnityEngine;

public class MouseLook : MonoBehaviour
{

    [SerializeField] float mouseSensitivity = 100f;
    Transform playerBody;

    float xRotation;

    InputManager inputManager;
    TimeBody timeBody;

    void Awake()
    {
        playerBody = transform.parent;
        inputManager = transform.parent.GetComponent<InputManager>();
        timeBody = playerBody.GetComponent<TimeBody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        xRotation -= inputManager.mouseY * mouseSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        if (timeBody.isRewinding == false)
        {
            playerBody.Rotate(Vector3.up, inputManager.mouseX * mouseSensitivity * Time.deltaTime);
        }
    }

}

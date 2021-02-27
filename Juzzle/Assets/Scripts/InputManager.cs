using UnityEngine;

public class InputManager : MonoBehaviour
{

    public float horizontal;
    public float vertical;
    public bool shift;
    public bool jump;

    public float mouseX;
    public float mouseY;

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        shift = Input.GetKey(KeyCode.LeftShift);
        jump = Input.GetKey(KeyCode.Space);

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
    }

}

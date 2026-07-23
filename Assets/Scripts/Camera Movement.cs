using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    [SerializeField] private Transform body;

    private float xRota;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void FixedUpdate()
    {
        //if (PauseManager.gamePaused)
        //    return;
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        xRota -= mouseY;
        xRota = Mathf.Clamp(xRota, -60f, 60f);

        transform.localRotation = Quaternion.Euler(xRota, 0, 0);
        body.Rotate(Vector3.up * mouseX);
    }
}

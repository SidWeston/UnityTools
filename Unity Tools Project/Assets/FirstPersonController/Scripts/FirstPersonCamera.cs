using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("First Person Camera")]
    [Header("Camera")]
    public Camera playerCamera;

    [Header("Player Object Reference")]
    public GameObject playerCharacter;

    [Header("Camera Sensitivity")]
    //how much will the camera turn when the mouse is moved
    public float lookSensitivity;

    private float yRotation;

    //input variables
    private Vector2 mouseVector;

    // Start is called before the first frame update
    void Start()
    {
        //set cursor to be hidden and locked to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //multiply mouse direction by sensitivity
        mouseVector *= lookSensitivity * Time.deltaTime;
        yRotation -= mouseVector.y;
        yRotation = Mathf.Clamp(yRotation, -90, 90);
        playerCamera.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        playerCharacter.transform.Rotate(Vector3.up * mouseVector.x);
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        mouseVector = context.ReadValue<Vector2>();
    }

}

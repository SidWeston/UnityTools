using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSway : MonoBehaviour
{
    //variables
    public float swayIntensity = 1.0f;
    public float swaySmoothness = 1.0f;

    private Quaternion origin;
    private Quaternion targetRotation;

    //input variables
    private Vector2 mouseVector;

    private void Start()
    {
        origin = transform.localRotation;
    }

    private void Update()
    {
        UpdateSway();
    }

    private void UpdateSway()
    {
        //calculate target rotation
        Quaternion xAdjustment = Quaternion.AngleAxis(swayIntensity * -mouseVector.x, Vector3.up);
        Quaternion yAdjustment = Quaternion.AngleAxis(swayIntensity * mouseVector.y, Vector3.right);
        targetRotation = origin * xAdjustment * yAdjustment;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * swaySmoothness);
    }

    //input functions
    public void OnMouseMove(InputAction.CallbackContext context)
    {
        mouseVector = context.ReadValue<Vector2>();
    }

}

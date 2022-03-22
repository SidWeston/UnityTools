using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject currentWeapon; //the weapon the player currently has

    public GameObject bulletSpawnLocation;

    [SerializeField] private Camera playerCamera; //reference to the player camera to help determine the direction the projectile should fire

    private IEnumerator FireWeapon()
    { 

        while(true)
        {
            Ray rayToScreen = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //creates a ray to the middle of the screen
            RaycastHit hit;
            Vector3 targetPoint;

            if (Physics.Raycast(rayToScreen, out hit))
            {
                targetPoint = hit.point;
            }
            else //if false, the player is shooting into the air
            {
                targetPoint = rayToScreen.GetPoint(75); //gets a vector3 point in the direction of the raycast that is far away from the player
            }
            Vector3 direction = targetPoint - bulletSpawnLocation.transform.position;

            currentWeapon.GetComponent<WeaponBase>().FireWeapon(direction);

            yield return new WaitForSeconds(currentWeapon.GetComponent<WeaponBase>().timeBetweenShots);
        }

    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if(context.ReadValue<float>() > 0)
        {
            StartCoroutine("FireWeapon");
        }
        else if(context.ReadValue<float>() <= 0)
        {
            StopCoroutine("FireWeapon");
        }
    }
}

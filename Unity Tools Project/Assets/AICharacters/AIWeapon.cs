using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeapon : MonoBehaviour
{

    public GameObject currentWeapon;
    public GameObject bulletSpawnLocation;

    private GameObject playerRef;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    private IEnumerator FireWeapon()
    {
        while (true)
        {
            Ray rayToTarget = new Ray(bulletSpawnLocation.transform.position, Vector3.Normalize(playerRef.transform.position - bulletSpawnLocation.transform.position)); //creates a ray to the middle of the screen
            RaycastHit hit;
            Vector3 targetPoint;

            if (Physics.Raycast(rayToTarget, out hit))
            {
                targetPoint = hit.point;
            }
            else //if false, the player is shooting into the air
            {
                targetPoint = rayToTarget.GetPoint(75); //gets a vector3 point in the direction of the raycast that is far away from the player
            }
            Vector3 direction = targetPoint - bulletSpawnLocation.transform.position;

            currentWeapon.GetComponent<WeaponBase>().FireWeapon(direction);

            yield return new WaitForSeconds(currentWeapon.GetComponent<WeaponBase>().timeBetweenShots);
        }

    }
}

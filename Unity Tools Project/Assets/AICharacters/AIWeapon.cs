using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeapon : MonoBehaviour
{

    public GameObject currentWeapon;
    public GameObject bulletSpawnLocation;

    public float spread = 0.5f;

    private GameObject playerRef;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    private IEnumerator FireWeapon()
    {
        while (true)
        {
            Ray rayToTarget = new Ray(bulletSpawnLocation.transform.position, Vector3.Normalize(playerRef.transform.position - bulletSpawnLocation.transform.position)); //creates a ray to the target
            RaycastHit hit;
            Vector3 targetPoint;

            if (Physics.Raycast(rayToTarget, out hit))
            {
                targetPoint = hit.point;
            }
            else //if false, the unit is shooting into the air
            {
                targetPoint = rayToTarget.GetPoint(75); //gets a vector3 point in the direction of the raycast that is far away from the player
            }
            Vector3 direction = targetPoint - bulletSpawnLocation.transform.position;
            direction.x += Random.Range(-spread, spread);
            direction.y += Random.Range(-spread / 2, spread / 2);

            currentWeapon.GetComponent<WeaponBase>().FireWeapon(direction);

            yield return new WaitForSeconds(currentWeapon.GetComponent<WeaponBase>().timeBetweenShots);
        }

    }
}

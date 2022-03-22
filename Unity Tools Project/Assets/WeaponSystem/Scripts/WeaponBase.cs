using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    //name of the weapon
    public string weaponName;

    //locations where the bullet class can be instantiated
    public GameObject bulletSpawnLocation; //on most weapons, there will only be one location but some weapons will have multiple barrels
    public GameObject bullet; //the bullet that is spawned when the weapon is fired

    public bool doesRichochet; //if the bullet ricochets or not, will be used by the projectile
    //if the weapon is a projectile (bullet has travel time)
    public bool isProjectile = true; //if false, the weapon will fire a raycast instead of a projectile

    public int magazineSize, bulletsLeft;
    [Range(0, 0.2f)]
    public float spreadX, spreadY;
    public float timeBetweenShots, bulletVelocity, reloadTime;
    public bool shooting, readyToShoot, reloading;

    // Start is called before the first frame update
    public virtual void Start()
    {
        readyToShoot = true;
        bulletsLeft = magazineSize;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!bulletSpawnLocation) //if the bullet spawn location isnt set it will attempt to find it in the scene
        {
            bulletSpawnLocation = transform.parent.GetChild(0).gameObject;
        }
    }

    //virtual functions to be overridden if needed on specific weapons
    public virtual void FireWeapon(Vector3 directionToFire)
    {

        if (isProjectile && readyToShoot && bulletsLeft > 0)
        {

            readyToShoot = false;
            bulletsLeft--;
            GameObject currentBullet = Instantiate(bullet, bulletSpawnLocation.transform.position, Quaternion.identity);
            currentBullet.GetComponent<Projectile>().whoFired = this.gameObject;
            Destroy(currentBullet, 10.0f);
            float spreadAmountX = Random.Range(-spreadX, spreadX);
            float spreadAmountY = Random.Range(-spreadY / 2, spreadY / 2);
            currentBullet.transform.forward = directionToFire.normalized;
            currentBullet.transform.forward += new Vector3(spreadAmountX, spreadAmountY, 0);
            currentBullet.GetComponent<Rigidbody>().AddForce(currentBullet.transform.forward * bulletVelocity, ForceMode.Impulse);

            Invoke("ResetShot", timeBetweenShots);

        }
        else if (bulletsLeft == 0 && !reloading)
        {
            reloading = true;
            Invoke("ReloadFinish", reloadTime);
        }
        else
        {
            //TODO: Implement Raycasts
        }

    }

    public virtual void ResetShot()
    {
        readyToShoot = true;
    }

    //called to reset ammo in gun when reload time i
    public virtual void ReloadFinish()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}

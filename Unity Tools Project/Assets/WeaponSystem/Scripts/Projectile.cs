using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float bulletDamage;
    public GameObject whoFired;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (whoFired = collision.gameObject)
        {
            return; //early out
        }
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<AIHealth>().ApplyDamage(bulletDamage);
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == whoFired)
        {
            return; //early out
        }
        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<AIHealth>().ApplyDamage(bulletDamage);
        }
    }
}

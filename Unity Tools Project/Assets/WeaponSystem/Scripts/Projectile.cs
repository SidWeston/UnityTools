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
            //collision.rigidbody.AddForceAtPosition(GetComponent<Rigidbody>().velocity * 50f, collision.contacts[0].point);
            
        }
    }
}

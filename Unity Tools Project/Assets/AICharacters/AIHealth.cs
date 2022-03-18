using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AIHealth : MonoBehaviour
{
    public float currentHealth = 100.0f;
    [HideInInspector]
    public float maxHealth;

    private void Start()
    {
        maxHealth = currentHealth;
    }

    public void ApplyDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if(currentHealth <= 0)
        {
            //TODO:
            //Add death state
        }
    }

    public void HealCharacter(float healAmount)
    {
        currentHealth += healAmount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }


    public void SendDamage(InputAction.CallbackContext context)
    {
        if(context.ReadValue<float>() == 0)
        {
            ApplyDamage(20.0f);
            Debug.Log(currentHealth);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : MonoBehaviour
{
    public float currentHealth = 100.0f;
    private float maxHealth;

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
}

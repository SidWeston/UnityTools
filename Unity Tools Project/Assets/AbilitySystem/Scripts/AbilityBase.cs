using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType
{ 
    Movement,
    Offensive,
    Defensive,
    Other
}


public class AbilityBase : MonoBehaviour
{
    //what type of ability is it - this will determine what slot of ability it can be put into
    public AbilityType abilityType;
    //the minimum time between each use of the ability
    public float abilityCooldown;

    public bool passiveAbility = false;

    public virtual void ActivateAbility()
    {

    }

    public virtual void DeactivateAbility()
    {

    }

}

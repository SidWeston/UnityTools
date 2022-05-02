using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasAmmo : ConditionalNode
{
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(controller.unitWeapon.currentWeapon.GetComponent<WeaponBase>().bulletsLeft > 0)
        {
            if(children[0] != null)
            {
                children[0].Update();
            }
            else
            {
                return State.Failure;
            }
        }
        else if(controller.unitWeapon.currentWeapon.GetComponent<WeaponBase>().bulletsLeft <= 0)
        {
            if(children[1] != null)
            {
                children[1].Update();
            }
            else
            {
                return State.Failure;
            }
            
        }

        return State.Success;
    }
}

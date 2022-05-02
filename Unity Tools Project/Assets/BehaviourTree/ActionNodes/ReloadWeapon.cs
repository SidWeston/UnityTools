using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadWeapon : ActionNode
{
    public WeaponBase aiWeapon;

    protected override void OnStart()
    {
        aiWeapon = controller.unitWeapon.currentWeapon.GetComponent<WeaponBase>();
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(!aiWeapon)
        {
            return State.Failure;
        }
        


        return State.Success;
    }
}

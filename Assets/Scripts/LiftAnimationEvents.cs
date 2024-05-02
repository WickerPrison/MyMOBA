using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftAnimationEvents : MonoBehaviour
{
    LiftAbilities abilitiesScript;

    private void Start()
    {
        abilitiesScript = GetComponentInParent<LiftAbilities>();
    }

    public void PushOff()
    {
        abilitiesScript.sliding = true;
    }

    public void FoodHeist()
    {
        
    }

    public void EatPancakes()
    {
    }

    public void Regrowth()
    {
        abilitiesScript.RegrowthFinal();
    }
}

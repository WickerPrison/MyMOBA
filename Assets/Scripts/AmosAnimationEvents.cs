using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmosAnimationEvents : MonoBehaviour
{
    AmosAbilities abilitiesScript;

    private void Start()
    {
        abilitiesScript = GetComponentInParent<AmosAbilities>();
    }

    public void AutoShotgun()
    {
        abilitiesScript.AutoShotgunFinish();
    }

    public void ThrowGrenade()
    {
    }

    public void SuppressingFire()
    {
        abilitiesScript.SuppressingFireFinal();
    }

    public void IAmThatGuy()
    {
        abilitiesScript.IAmThatGuyFinal();
    }

    public void RapidFire()
    {
        abilitiesScript.RapidFireFinish();
    }
}

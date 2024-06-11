using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class VinAnimationEvents : MonoBehaviour
{
    [SerializeField] VinAbilities abilitiesScript;

    public void GlassDaggers()
    {
        abilitiesScript.GlassDaggersFinish();
    }

    public void MetalVial()
    {
        abilitiesScript.MetalVial();
    }

    public void SteelpushTrue()
    {
        //abilitiesScript.steelpush = true;
    }

    public void SteelpushFalse()
    {
        //abilitiesScript.steelpush = false;
        //abilitiesScript.SteelLanding();
    }
}

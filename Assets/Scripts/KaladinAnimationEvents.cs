using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaladinAnimationEvents : MonoBehaviour
{
    KaladinAbilities abilities;

    private void Start()
    {
        abilities = GetComponentInParent<KaladinAbilities>();
    }

    public void SylSpear()
    {
        abilities.SylSpearFinish();
    }

    public void Gravitation()
    {
        abilities.GravitationFinish();
    }

    public void SayTheWords()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SokkaAnimationEvents : MonoBehaviour
{
    [SerializeField] SpriteRenderer boomerang;
    [SerializeField] SpriteRenderer spaceSword;
    [SerializeField] SpriteRenderer stinkBomb;
    [SerializeField] SokkaAbilities abilitiesScript;
    [SerializeField] GameObject boomerangPrefab;
    [SerializeField] Transform throwingHand;
    [SerializeField] GameObject stinkBombPrefab;
    [SerializeField] Transform bombThrowingHand;

    public void SwitchWeapons()
    {
        if(abilitiesScript.weapon == 0)
        {
            abilitiesScript.weapon = 1;
            boomerang.enabled = false;
            spaceSword.enabled = true;
        }
        else
        {
            abilitiesScript.weapon = 0;
            boomerang.enabled = true;
            spaceSword.enabled = false;
        }
    }

    public void SpaceSword()
    {
        abilitiesScript.SwordFinal();
    }

    public void ThrowBoomerang()
    {
        boomerang.enabled = false;
        Boomerang boomerangScript = Instantiate(boomerangPrefab).GetComponent<Boomerang>();
        boomerangScript.transform.position = throwingHand.position;
        boomerangScript.target = abilitiesScript.currentTarget.transform;
        boomerangScript.returnPosition = throwingHand;
        boomerangScript.abilities = abilitiesScript;
    }

    public void CatchBoomerang()
    {
        boomerang.enabled = true;
    }

    public void FlyingKickAPow()
    {
        abilitiesScript.FlyingKickAPowFinal();
    }

    public void DrawStinkBomb()
    {
        stinkBomb.enabled = true;
    }

    public void ThrowStinkBomb()
    {
        stinkBomb.enabled = false;
        StinkBomb stinkBombScript = Instantiate(stinkBombPrefab).GetComponent<StinkBomb>();
        stinkBombScript.transform.position = bombThrowingHand.position;
        stinkBombScript.target = abilitiesScript.stinkBombTarget;
        stinkBombScript.myTeam = abilitiesScript.gameObject.tag;
    }
}

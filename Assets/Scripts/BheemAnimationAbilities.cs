using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BheemAnimationAbilities : MonoBehaviour
{
    [SerializeField] GameObject sleepPotPrefab;
    [SerializeField] Transform backHand;
    Projectile sleepPot;
    BheemAbilities abilitiesScript;

    private void Start()
    {
        abilitiesScript = GetComponentInParent<BheemAbilities>();
    }

    public void Spear()
    {
        abilitiesScript.SpearFinish();
    }

    public void Leaves()
    {
        abilitiesScript.LeavesFinal();
    }

    public void ThrowSleepPot()
    {
        sleepPot = Instantiate(sleepPotPrefab).GetComponent<Projectile>();
        sleepPot.transform.position = backHand.position;
        sleepPot.target = abilitiesScript.sleepPotTarget;
        sleepPot.myTeam = abilitiesScript.gameObject.tag;
        sleepPot.abilities = abilitiesScript;
    }
}

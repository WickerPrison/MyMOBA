using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BheemAbilities : CharacterAbilities
{
    [SerializeField] PathfindingParameters attackParameters;
    [SerializeField] PathfindingParameters buffParameters;
    [SerializeField] Animator animator;
    PlayerScript currentTarget;
    [SerializeField] int spearDamage = 4;

    PlayerScript holyThreaded;
    int holyThreadHealing = 3;

    int sleepPotDuration = 2;
    [System.NonSerialized] public TileScript sleepPotTarget;


    public override void Start()
    {
        base.Start();
        // Spear cost and cooldown
        playerScript.actionPointCosts.Add(1);
        playerScript.maxAbilityCooldowns.Add(0);
        // holy thread cost and cooldown
        playerScript.actionPointCosts.Add(0);
        playerScript.maxAbilityCooldowns.Add(0);
        // leaves cost and cooldown
        playerScript.actionPointCosts.Add(1);
        playerScript.maxAbilityCooldowns.Add(4);
        playerScript.silenceableAbilities.Add(4);
        // sleep pot cost and cooldown
        playerScript.actionPointCosts.Add(1);
        playerScript.maxAbilityCooldowns.Add(5);

        playerScript.abilityCooldowns = new int[playerScript.maxAbilityCooldowns.Count];

        holyThreaded = playerScript;
    }

    private void Update()
    {
        if (playerScript.activeAbility <= 1)
        {
            return;
        }

        pathfinding.ResetTiles();
        TileScript currentTile = pathfinding.GetCurrentTile();
        currentTile.UpdateSelectionColor(1, true);
        TileScript mouseTile = mouseOverTiles.GetClickedTile();

        switch (playerScript.activeAbility)
        {
            case 2:
                pathfinding.Pathfinder(currentTile, attackParameters, 1, true);
                if (mouseTile != null && mouseTile.selectable && mouseTile.occupied && !mouseTile.occupation.CompareTag(gameObject.tag))
                {
                    mouseTile.UpdateSelectionColor(attackParameters.selectionColor, false);
                }
                break;
            case 3:
                pathfinding.Pathfinder(currentTile, buffParameters, 1, true);
                Pathfinding threadedPathfinder = holyThreaded.GetComponent<Pathfinding>();
                if (!holyThreaded.dead)
                {
                    TileScript threadedTile = threadedPathfinder.GetCurrentTile();
                    threadedTile.UpdateSelectionColor(buffParameters.selectionColor, true);
                }
                currentTile.UpdateSelectionColor(buffParameters.selectionColor, true);
                if (mouseTile != null && mouseTile.selectable && mouseTile.occupied && mouseTile.occupation.CompareTag(gameObject.tag) && mouseTile.occupation != holyThreaded)
                {
                    mouseTile.UpdateSelectionColor(buffParameters.selectionColor, false);
                }
                break;
            case 4:
                pathfinding.Pathfinder(currentTile, buffParameters, 1, true);
                currentTile.UpdateSelectionColor(buffParameters.selectionColor, true);
                if (mouseTile != null && mouseTile.selectable && mouseTile.occupied && mouseTile.occupation.CompareTag(gameObject.tag))
                {
                    mouseTile.UpdateSelectionColor(buffParameters.selectionColor, false);
                }
                break;
            case 5:
                pathfinding.Pathfinder(currentTile, attackParameters, 3, true);
                if (mouseTile != null && mouseTile.selectable && mouseTile.occupied && !mouseTile.occupation.CompareTag(gameObject.tag))
                {
                    mouseTile.UpdateSelectionColor(attackParameters.selectionColor, false);
                }
                break;
        }
    }


    public override void UseAbility()
    {
        TileScript clickedTile = mouseOverTiles.GetClickedTile();
        if (clickedTile == null || !clickedTile.selectable)
        {
            return;
        }

        switch (playerScript.activeAbility)
        {
            case 2:
                SpearInitiate(clickedTile);
                break;
            case 3:
                HolyThread(clickedTile);
                break;
            case 4:
                LeavesInitiate(clickedTile);
                break;
            case 5: 
                SleepPotInitiate(clickedTile);
                break;
        }
    }

    void SpearInitiate(TileScript clickedTile)
    {
        PlayerScript enemyScript = clickedTile.occupation.GetComponent<PlayerScript>();
        if (enemyScript != null && !enemyScript.gameObject.CompareTag(gameObject.tag))
        {
            playerScript.FaceCharacter(clickedTile.transform);
            currentTarget = enemyScript;
            playerScript.ActivateAbility(0);
            playerScript.actionPoints -= playerScript.actionPointCosts[2];
            animator.Play("Spear");
        }
    }

    public void SpearFinish()
    {
        holyThreaded.GetHealed(holyThreadHealing);
        currentTarget.TakeDamage(spearDamage);
        currentTarget = null;
    }

    void HolyThread(TileScript clickedTile)
    {
        PlayerScript holyThreadTarget = clickedTile.occupation.GetComponent<PlayerScript>();
        if(holyThreadTarget != null && holyThreadTarget.gameObject.CompareTag(gameObject.tag) && holyThreadTarget != holyThreaded)
        {
            holyThreaded = holyThreadTarget;
            playerScript.FaceCharacter(clickedTile.transform);
            playerScript.ActivateAbility(0);
            playerScript.actionPoints -= playerScript.actionPointCosts[3];
            animator.Play("HolyThread");
        }
    }

    void LeavesInitiate(TileScript clickedTile)
    {
        PlayerScript ally = clickedTile.occupation.GetComponent<PlayerScript>();
        if(ally != null && ally.CompareTag(gameObject.tag) && ally.health < ally.maxHealth)
        {
            playerScript.FaceCharacter(clickedTile.transform);
            playerScript.ActivateAbility(0);
            playerScript.actionPoints -= playerScript.actionPointCosts[4];
            playerScript.abilityCooldowns[4] = playerScript.maxAbilityCooldowns[4];
            currentTarget = ally;
            animator.Play("Leaves");
        }
    }

    public void LeavesFinal()
    {
        currentTarget.GetHealed(currentTarget.maxHealth);
        currentTarget = null;
    }

    void SleepPotInitiate(TileScript clickedTile)
    {
        PlayerScript enemyScript = clickedTile.occupation.GetComponent<PlayerScript>();
        if (enemyScript != null && !enemyScript.gameObject.CompareTag(gameObject.tag))
        {
            sleepPotTarget = clickedTile;
            playerScript.FaceCharacter(clickedTile.transform);
            currentTarget = enemyScript;
            playerScript.ActivateAbility(0);
            playerScript.actionPoints -= playerScript.actionPointCosts[5];
            playerScript.abilityCooldowns[5] = playerScript.maxAbilityCooldowns[5];
            animator.Play("SleepPot");
        }
    }

    public override void ProjectileHit(TileScript target)
    {
        base.ProjectileHit(target);

        PlayerScript enemyScript = target.occupation.GetComponent<PlayerScript>();
        if(enemyScript.sleep < sleepPotDuration)
        {
            enemyScript.sleep = sleepPotDuration;
        }
    }

    public override void ActivateAbility(int abilityID)
    {
        base.ActivateAbility(abilityID);
        playerScript.ActivateAbility(abilityID);
    }
}

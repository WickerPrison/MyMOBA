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
    [SerializeField] GameObject sleepPotPrefab;

    PlayerScript holyThreaded;
    int holyThreadHealing = 3;

    int sleepPotDuration = 2;

    int inspiringSongTurnMeter = 500;


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
        if (playerScript.activeAbility <= 1 && !playerScript.ultimateActive)
        {
            return;
        }

        pathfinding.ResetTiles();
        TileScript currentTile = pathfinding.GetCurrentTile();
        currentTile.UpdateSelectionColor(1, true);
        TileScript mouseTile = mouseOverTiles.GetClickedTile();

        if (playerScript.ultimateActive)
        {
            pathfinding.Pathfinder(currentTile, buffParameters, 3, true);
            if(mouseTile != null && mouseTile.selectable)
            {
                pathfinding.ResetTiles();
                pathfinding.Pathfinder(currentTile, buffParameters, 3, false);
                currentTile.UpdateSelectionColor(buffParameters.selectionColor, false);
            }
        }

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

        if(playerScript.ultimateActive && playerScript.actionPoints >= playerScript.ultimateAPCost)
        {
            InspiringSong();
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
            currentTarget = enemyScript;
            playerScript.ActivateAbility(0);
            playerScript.actionPoints -= playerScript.actionPointCosts[2];
            tokenAnimations.MeleeAttack(enemyScript.transform.position, SpearFinish);
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
            holyThreaded.characterEvents.Debuff();
            holyThreaded = holyThreadTarget;
            holyThreaded.characterEvents.Buff();
            playerScript.ActivateAbility(0);
            playerScript.actionPoints -= playerScript.actionPointCosts[3];
        }
    }

    void LeavesInitiate(TileScript clickedTile)
    {
        PlayerScript ally = clickedTile.occupation.GetComponent<PlayerScript>();
        if(ally != null && ally.CompareTag(gameObject.tag) && ally.health < ally.maxHealth)
        {
            playerScript.ActivateAbility(0);
            playerScript.actionPoints -= playerScript.actionPointCosts[4];
            playerScript.abilityCooldowns[4] = playerScript.maxAbilityCooldowns[4];
            ally.GetHealed(ally.maxHealth);
        }
    }

    void SleepPotInitiate(TileScript clickedTile)
    {
        PlayerScript enemyScript = clickedTile.occupation.GetComponent<PlayerScript>();
        if (enemyScript != null && !enemyScript.gameObject.CompareTag(gameObject.tag))
        {
            currentTarget = enemyScript;
            playerScript.ActivateAbility(0);
            playerScript.actionPoints -= playerScript.actionPointCosts[5];
            playerScript.abilityCooldowns[5] = playerScript.maxAbilityCooldowns[5];

            Projectile sleepPot = Instantiate(sleepPotPrefab).GetComponent<Projectile>();
            sleepPot.transform.position = transform.position;
            sleepPot.target = clickedTile;
            sleepPot.myTeam = gameObject.tag;
            sleepPot.abilities = this;
        }
    }

    public override void ProjectileHit(TileScript target)
    {
        base.ProjectileHit(target);

        PlayerScript enemyScript = target.occupation.GetComponent<PlayerScript>();
        enemyScript.characterEvents.Sleep();
        if(enemyScript.sleep < sleepPotDuration)
        {
            enemyScript.sleep = sleepPotDuration;
        }
    }

    void InspiringSong()
    {
        playerScript.ultimateActive = false;
        playerScript.ActivateAbility(0);
        playerScript.actionPoints -= playerScript.ultimateAPCost;
        playerScript.ultimateCD = playerScript.maxUltimateCD;
        foreach(PlayerScript player in tm.players)
        {
            Pathfinding playerPathfinding = player.GetComponent<Pathfinding>();
            TileScript playerTile = playerPathfinding.GetCurrentTile(); 
            if(player.CompareTag(gameObject.tag) && playerTile.selectable)
            {
                player.IncreaseTurnMeter(inspiringSongTurnMeter);
                player.frenzy += 2;
                player.characterEvents.Frenzy(true);
            }
        }
    }

    public override void ActivateAbility(int abilityID)
    {
        base.ActivateAbility(abilityID);
        playerScript.ActivateAbility(abilityID);
    }
}

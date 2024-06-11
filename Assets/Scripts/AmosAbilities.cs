using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AmosAbilities : CharacterAbilities
{
    [SerializeField] PathfindingParameters attackParameters;
    [SerializeField] Animator animator;
    [SerializeField] GameObject grenadePrefab;
    [SerializeField] Transform backHand;
    Grenade grenade;
    TileScript grenadeDestination;
    PlayerScript currentTarget;
    int attackRange = 3;
    [SerializeField] int autoshotgunDamage = 6;

    int suppressingFireAmount = 300;

    float iAmThatGuyPercent = 0.4f;

    public override void Start()
    {
        base.Start();
        // auto shotgun cost and cooldown
        playerScript.actionPointCosts.Add(1);
        playerScript.maxAbilityCooldowns.Add(0);
        // grenade cost and cooldown
        playerScript.actionPointCosts.Add(1);
        playerScript.maxAbilityCooldowns.Add(2);
        playerScript.silenceableAbilities.Add(3);
        // suppresing fire cost and cooldown
        playerScript.actionPointCosts.Add(1);
        playerScript.maxAbilityCooldowns.Add(4);
        playerScript.silenceableAbilities.Add(4);
        // Rapid fire cost and cooldown
        playerScript.actionPointCosts.Add(1);
        playerScript.maxAbilityCooldowns.Add(4);
        playerScript.silenceableAbilities.Add(5);

        playerScript.abilityCooldowns = new int[playerScript.maxAbilityCooldowns.Count];
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
            pathfinding.Pathfinder(currentTile, attackParameters, attackRange, true);
            if (mouseTile != null && mouseTile.selectable && mouseTile.occupied && !mouseTile.occupation.CompareTag(gameObject.tag))
            {
                PlayerScript enemyScript = mouseTile.occupation.GetComponent<PlayerScript>();
                if (enemyScript.health / enemyScript.maxHealth <= iAmThatGuyPercent)
                {
                    mouseTile.UpdateSelectionColor(attackParameters.selectionColor, false);
                }
            }
            return;
        }

        switch (playerScript.activeAbility)
        {
            case 2:
                pathfinding.Pathfinder(currentTile, attackParameters, attackRange, true);
                if (mouseTile != null && mouseTile.selectable && mouseTile.occupied && !mouseTile.occupation.CompareTag(gameObject.tag))
                {
                    mouseTile.UpdateSelectionColor(attackParameters.selectionColor, false);
                }
                break;
            case 3:
                pathfinding.Pathfinder(currentTile, attackParameters, 3, true);
                if (mouseTile != null && mouseTile.selectable)
                {
                    mouseTile.UpdateSelectionColor(attackParameters.selectionColor, false);
                    foreach (TileScript adjacentTile in mouseTile.adjacencyList)
                    {
                        adjacentTile.UpdateSelectionColor(attackParameters.selectionColor, false);
                    }
                }
                break;
            case 4:
                pathfinding.Pathfinder(currentTile, attackParameters, 1, true);
                if (mouseTile != null && mouseTile.selectable)
                {
                    string direction = pathfinding.GetDirectionOfAdjacentTile(currentTile, mouseTile);
                    pathfinding.Cone(mouseTile, attackParameters, 4, direction, false);
                    mouseTile.UpdateSelectionColor(attackParameters.selectionColor, false);
                }
                break;
            case 5:
                pathfinding.Pathfinder(currentTile, attackParameters, attackRange, true);
                if (mouseTile != null && mouseTile.selectable && mouseTile.occupied && !mouseTile.occupation.CompareTag(gameObject.tag))
                {
                    mouseTile.UpdateSelectionColor(attackParameters.selectionColor, false);
                }
                break;
        }
    }

    public override void StartTurn()
    {
        base.StartTurn();

        if(grenade != null)
        {
            grenade.Explode();
        }
    }


    public override void UseAbility()
    {
        TileScript clickedTile = mouseOverTiles.GetClickedTile();
        if (clickedTile == null || !clickedTile.selectable)
        {
            return;
        }

        if (playerScript.ultimateActive)
        {
            IAmThatGuyInitiate(clickedTile);
            return;
        }

        switch (playerScript.activeAbility)
        {
            case 2:
                AutoShotgunInitiate(clickedTile);
                break;
            case 3:
                ThrowGrenadeInitiate(clickedTile);
                break;
            case 4:
                SuppressingFireInitiate(clickedTile);
                break;
            case 5:
                RapidFireInitiate(clickedTile);
                break;
        }
    }

    void AutoShotgunInitiate(TileScript clickedTile)
    {
        PlayerScript enemyScript = clickedTile.occupation.GetComponent<PlayerScript>();
        if (enemyScript != null && !enemyScript.gameObject.CompareTag(gameObject.tag))
        {
            playerScript.FaceCharacter(clickedTile.transform);
            currentTarget = enemyScript;
            tokenAnimations.Recoil(currentTarget.transform.position, AutoShotgunFinish);
        }
    }

    public void AutoShotgunFinish()
    {
        currentTarget.TakeDamage(autoshotgunDamage);
        playerScript.ActivateAbility(0);
        playerScript.actionPoints = 0;
        currentTarget = null;
    }

    void ThrowGrenadeInitiate(TileScript clickedTile)
    {
        playerScript.FaceCharacter(clickedTile.transform);
        playerScript.ActivateAbility(0);
        playerScript.actionPoints -= playerScript.actionPointCosts[3];
        playerScript.abilityCooldowns[3] = playerScript.maxAbilityCooldowns[3];
        grenadeDestination = clickedTile;
        grenade = Instantiate(grenadePrefab).GetComponent<Grenade>();
        grenade.targetTile = grenadeDestination;
        grenade.startingPosition = transform.position;
    }

    void SuppressingFireInitiate(TileScript clickedTile)
    {
        Action[] suppressingActions = {
            () => { },
            () => { },
            () => { }
        };
        tokenAnimations.Recoil(clickedTile.transform.position, SuppressingFireFinal, suppressingActions);
    }

    public void SuppressingFireFinal()
    {
        foreach (PlayerScript player in tm.players)
        {
            Pathfinding playerPathfinding = player.GetComponent<Pathfinding>();
            TileScript playerTile = playerPathfinding.GetCurrentTile();
            if (!player.CompareTag(gameObject.tag) && playerTile.selectable)
            {
                player.DecreaseTurnMeter(suppressingFireAmount);
            }
        }

        playerScript.ActivateAbility(0);
        playerScript.actionPoints -= playerScript.actionPointCosts[4];
        playerScript.abilityCooldowns[4] = playerScript.maxAbilityCooldowns[4];
        currentTarget = null;
    }

    void RapidFireInitiate(TileScript clickedTile)
    {
        PlayerScript enemyScript = clickedTile.occupation.GetComponent<PlayerScript>();
        if (enemyScript != null && !enemyScript.gameObject.CompareTag(gameObject.tag))
        {
            playerScript.FaceCharacter(clickedTile.transform);
            currentTarget = enemyScript;
            tokenAnimations.Recoil(currentTarget.transform.position, RapidFireFinish, new Action[] {AutoShotgunFinish});
        }
    }

    public void RapidFireFinish()
    {
        currentTarget.TakeDamage(autoshotgunDamage);
    }

    void IAmThatGuyInitiate(TileScript clickedTile)
    {
        PlayerScript enemyScript = clickedTile.occupation.GetComponent<PlayerScript>();
        if (enemyScript != null && !enemyScript.gameObject.CompareTag(gameObject.tag) && enemyScript.health / enemyScript.maxHealth <= iAmThatGuyPercent)
        {
            playerScript.FaceCharacter(clickedTile.transform);
            currentTarget = enemyScript;
            tokenAnimations.Recoil(currentTarget.transform.position, IAmThatGuyFinal);
        }
    }

    public void IAmThatGuyFinal()
    {
        currentTarget.health = 0;
        playerScript.ultimateActive = false;
        playerScript.ActivateAbility(0);
        playerScript.actionPoints -= playerScript.ultimateAPCost;
        playerScript.ultimateCD = playerScript.maxUltimateCD;
        currentTarget = null;
    }

    public override void ActivateAbility(int abilityID)
    {
        base.ActivateAbility(abilityID);
        playerScript.ActivateAbility(abilityID);
    }
}

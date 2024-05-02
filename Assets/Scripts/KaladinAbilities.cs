using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class KaladinAbilities : CharacterAbilities
{
    [SerializeField] PathfindingParameters attackParameters;
    [SerializeField] PathfindingParameters lashingParameters;
    [SerializeField] PathfindingParameters gravitationMovementParameters;
    [SerializeField] Animator animator;
    [SerializeField] Color stormlightColor;
    StormlightAnimations stormlightAnimations;
    PlayerScript currentLivingShardplate;
    PlayerScript currentTarget;
    TileScript lashingTile;
    UIManager uim;

    int stormlight;
    [SerializeField] int maxStormlight = 10;

    int sylSpearRange = 1;
    [SerializeField] int sylSpearDamage = 5;

    int gravitationRange = 1;
    bool gravitationTargetSelected = false;
    int gravitationMovementRange = 2;
    bool adhesionActive = false;
    int adhesionDuration = 2;
    int livingShardplateRange = 5;

    public override void Start()
    {
        base.Start();
        stormlightAnimations = GetComponentInChildren<StormlightAnimations>();

        // spear attack cost and cooldown
        playerScript.actionPointCosts.Add(1);
        playerScript.maxAbilityCooldowns.Add(0);
        // moving other player cost and cooldown
        playerScript.actionPointCosts.Add(1);
        playerScript.maxAbilityCooldowns.Add(2);
        playerScript.silenceableAbilities.Add(3);
        // stick to ground cost and cooldown
        playerScript.actionPointCosts.Add(0);
        playerScript.maxAbilityCooldowns.Add(3);
        playerScript.silenceableAbilities.Add(4);
        // living shardplate cost and cooldown
        playerScript.actionPointCosts.Add(0);
        playerScript.maxAbilityCooldowns.Add(0);
        playerScript.silenceableAbilities.Add(5);

        playerScript.abilityCooldowns = new int[playerScript.maxAbilityCooldowns.Count];

        currentLivingShardplate = playerScript;
        playerScript.livingShardplate = true;
        playerScript.CalculateArmor();
        stormlight = maxStormlight;

        uim = tm.gameObject.GetComponent<UIManager>();
    }

    private void Update()
    {
        if(playerScript.activeAbility <= 1 && !playerScript.ultimateActive)
        {
            gravitationTargetSelected = false;
            return;
        }

        pathfinding.ResetTiles();
        TileScript currentTile = pathfinding.GetCurrentTile();
        currentTile.UpdateSelectionColor(1, true);
        TileScript mouseTile = mouseOverTiles.GetClickedTile();

        if (playerScript.ultimateActive)
        {
            gravitationTargetSelected = false;
            currentTile.selectable = true;
            currentTile.UpdateSelectionColor(3, true);
            if (mouseTile == currentTile)
            {
                mouseTile.UpdateSelectionColor(3, false);
            }
            return;
        }

        switch (playerScript.activeAbility)
        {
            case 2:
                gravitationTargetSelected = false;
                pathfinding.Pathfinder(currentTile, attackParameters, sylSpearRange, true);
                if (mouseTile != null && mouseTile.selectable && mouseTile.occupied && !mouseTile.occupation.CompareTag(gameObject.tag))
                {
                    mouseTile.UpdateSelectionColor(attackParameters.selectionColor, false);
                }
                break;

            case 3:
                if (gravitationTargetSelected)
                {
                    Pathfinding targetPathfinding = currentTarget.GetComponent<Pathfinding>();
                    pathfinding.Pathfinder(targetPathfinding.GetCurrentTile(), gravitationMovementParameters, gravitationMovementRange, true);
                    if(mouseTile != null && mouseTile.selectable)
                    {
                        mouseTile.UpdateSelectionColor(gravitationMovementParameters.selectionColor, false);
                    }
                   
                }
                else
                {
                    pathfinding.Pathfinder(currentTile, lashingParameters, gravitationRange, true);
                    if(mouseTile != null && mouseTile.occupied && mouseTile.selectable && mouseTile != currentTile)
                    {
                        mouseTile.UpdateSelectionColor(lashingParameters.selectionColor, false);
                    }
                }
                break;
            case 4:
                gravitationTargetSelected = false;
                currentTile.selectable = true;
                currentTile.UpdateSelectionColor(3, true);
                if(mouseTile == currentTile)
                {
                    mouseTile.UpdateSelectionColor(3, false);
                }
                break;
            case 5:
                gravitationTargetSelected = false;
                pathfinding.Pathfinder(currentTile, lashingParameters, livingShardplateRange, true);
                if(mouseTile!= null && mouseTile.selectable && mouseTile.occupation.CompareTag(gameObject.tag) && !mouseTile.occupation.GetComponent<PlayerScript>().livingShardplate)
                {
                    mouseTile.UpdateSelectionColor(lashingParameters.selectionColor, false);
                }
                break;
        }
    }

    public override void StartTurn()
    {
        base.StartTurn();

        if(playerScript.health < playerScript.maxHealth && stormlight > 0)
        {
            playerScript.GetHealed(2);
            stormlight -= 1;
        }
        uim.UpdateMana(stormlight, stormlightColor);
    }

    public override void EndTurn()
    {
        base.EndTurn();
        TileScript currentTile = pathfinding.GetCurrentTile();
        if (currentTile != null && currentTile.tileType == 1 && currentTile.spawnPointTeam == playerScript.myTeam)
        {
            stormlight += 5;
            if(stormlight > maxStormlight)
            {
                stormlight = maxStormlight;
            }
        }
        uim.UpdateMana(0, stormlightColor);
    }

    public override void Respawn()
    {
        base.Respawn();
        stormlight = maxStormlight;
        uim.UpdateMana(stormlight, stormlightColor);
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
            SayTheWordsInitiate(clickedTile);
        }

        switch (playerScript.activeAbility)
        {
            case 2:
                SylSpearInitiate(clickedTile);
                break;
            case 3:
                GravitationInitiate(clickedTile);
                break;
            case 4:
                Adhesion(clickedTile);
                break;
            case 5:
                LivingShardplate(clickedTile);
                break;
        }
    }

    void SylSpearInitiate(TileScript clickedTile)
    {
        PlayerScript enemyScript = clickedTile.occupation.GetComponent<PlayerScript>();
        if(enemyScript != null && !enemyScript.gameObject.CompareTag(gameObject.tag))
        {
            currentTarget = enemyScript;
            tokenAnimations.MeleeAttack(enemyScript.transform.position, SylSpearFinish);
        }
    }

    public void SylSpearFinish()
    {
        currentTarget.TakeDamage(sylSpearDamage);
        if (adhesionActive && currentTarget.rooted <= adhesionDuration)
        {
            stormlightAnimations.EndStormlight();
            currentTarget.rooted = adhesionDuration;
            adhesionActive = false;
        }

        playerScript.ActivateAbility(0);
        playerScript.actionPoints = 0;
        currentTarget = null;
    }

    void GravitationInitiate(TileScript clickedTile)
    {
        PlayerScript characterScript = clickedTile.occupation.GetComponent<PlayerScript>();
        if(characterScript != null && !gravitationTargetSelected && characterScript != playerScript)
        {
            currentTarget = characterScript;
            gravitationTargetSelected = true;
        }
        else if(gravitationTargetSelected && clickedTile != null && clickedTile.selectable && !clickedTile.occupied)
        {
            lashingTile = clickedTile;
            GravitationFinish();
        }
    }

    public void GravitationFinish()
    {
        playerScript.abilityCooldowns[3] = playerScript.maxAbilityCooldowns[3];
        playerScript.ActivateAbility(0);
        playerScript.actionPoints -= playerScript.actionPointCosts[3];
        stormlight -= 2;
        uim.UpdateMana(stormlight, stormlightColor);
        PlayerMovement targetMovement = currentTarget.GetComponent<PlayerMovement>();
        targetMovement.GetPath(lashingTile);
        targetMovement.FollowPath();
        if(adhesionActive && currentTarget.rooted <= adhesionDuration)
        {
            stormlightAnimations.EndStormlight();
            currentTarget.rooted = adhesionDuration;
            adhesionActive = false;
        }
        currentTarget = null;
    }

    void Adhesion(TileScript clickedTile)
    {
        if(clickedTile.occupation = gameObject)
        {
            playerScript.abilityCooldowns[4] = playerScript.maxAbilityCooldowns[4];
            playerScript.ActivateAbility(0);
            stormlight -= 2;
            uim.UpdateMana(stormlight, stormlightColor);
            adhesionActive = true;
            stormlightAnimations.StartStormlight();
        }
    }

    void LivingShardplate(TileScript clickedTile)
    {
        PlayerScript allyScript = clickedTile.occupation.GetComponent<PlayerScript>();
        if(allyScript != null && allyScript.gameObject.CompareTag(gameObject.tag) && !allyScript.livingShardplate)
        {
            playerScript.abilityCooldowns[5] = playerScript.maxAbilityCooldowns[5];
            playerScript.ActivateAbility(0);
            playerScript.livingShardplate = false;
            currentLivingShardplate.CalculateArmor();
            allyScript.livingShardplate = true;
            currentLivingShardplate = allyScript;
            currentLivingShardplate.CalculateArmor();
            animator.Play("Adhesion");
        }
    }

    void SayTheWordsInitiate(TileScript clickedTile)
    {
        if(clickedTile.occupation == gameObject)
        {
            playerScript.ultimateActive = false;
            playerScript.ultimateCD = playerScript.maxUltimateCD;
            animator.Play("SayTheWords");
        }
    }

    public void SayTheWordsFinal()
    {
        playerScript.GetHealed(playerScript.maxHealth);
        stormlight = maxStormlight;
        uim.UpdateMana(stormlight, stormlightColor);
    }


    public override void ActivateAbility(int abilityID)
    {
        base.ActivateAbility(abilityID);
        if (abilityID == 2 || abilityID == 5)
        {
            playerScript.ActivateAbility(abilityID);
        }
        else if(abilityID == 3 || abilityID == 4)
        {
            if (stormlight >= 2)
            {
                playerScript.ActivateAbility(abilityID);
            }
        }
    }

    public override bool ShouldBeGreyedOut(int abilityID)
    {
        if (abilityID == 5 && playerScript.rooted > 0)
        {
            return true;
        }

        if (abilityID > 2 && abilityID < 5 && stormlight < 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

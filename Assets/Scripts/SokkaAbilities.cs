using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SokkaAbilities : CharacterAbilities
{
    [System.NonSerialized] public int weapon = 0;
    [SerializeField] Animator animator;
    [SerializeField] PathfindingParameters attackParameters;
    [SerializeField] PathfindingParameters buffParameters;
    [SerializeField] SpriteRenderer dayOfBlackSunSprite;
    int boomerangRange = 3;
    [SerializeField] int boomerangDamage = 5;
    int swordRange = 1;
    [SerializeField] int swordDamage = 5;
    int sneakAttackDuration = 2;
    int sneakAttackTurnMeter = 170;
    int kickPushDistance = 2;
    public PlayerScript currentTarget;
    public TileScript stinkBombTarget;

    int dayOfBlackSunMaxDuration = 2;
    int dayOfBlackSunDuration = 0;

    public override void Start()
    {
        base.Start();
        // boomerang/sword attack cost and cooldown
        playerScript.actionPointCosts.Add(1);
        playerScript.maxAbilityCooldowns.Add(0);
        // switch weapon cost and cooldown
        playerScript.actionPointCosts.Add(0);
        playerScript.maxAbilityCooldowns.Add(0);
        // Sneak Attack cost and cooldown
        playerScript.actionPointCosts.Add(0);
        playerScript.maxAbilityCooldowns.Add(3);
        playerScript.silenceableAbilities.Add(4);
        // flying kick a pow cost and cooldown
        playerScript.actionPointCosts.Add(1);
        playerScript.maxAbilityCooldowns.Add(3);
        playerScript.silenceableAbilities.Add(5);
        // stink bomb cost and cooldown
        playerScript.actionPointCosts.Add(1);
        playerScript.maxAbilityCooldowns.Add(4);
        playerScript.silenceableAbilities.Add(6);

        playerScript.abilityCooldowns = new int[playerScript.maxAbilityCooldowns.Count];

        dayOfBlackSunSprite.transform.parent = Camera.main.transform;
        dayOfBlackSunSprite.transform.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
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
            currentTile.selectable = true;
            currentTile.UpdateSelectionColor(3, true);
            if (mouseTile == currentTile && playerScript.actionPoints >= playerScript.ultimateAPCost)
            {
                mouseTile.UpdateSelectionColor(3, false);
            }
            return;
        }

        switch (playerScript.activeAbility)
        {
            case 2:
                if(weapon == 0)
                {
                    pathfinding.Pathfinder(currentTile, attackParameters, boomerangRange, true);
                }
                else
                {
                    pathfinding.Pathfinder(currentTile, attackParameters, swordRange, true);
                }

                if (mouseTile != null && mouseTile.selectable && mouseTile.occupied && !mouseTile.occupation.CompareTag(gameObject.tag))
                {
                    mouseTile.UpdateSelectionColor(attackParameters.selectionColor, false);
                }
                break;
            case 4:
                pathfinding.Pathfinder(currentTile, buffParameters, 2,true);
                if(mouseTile != null && mouseTile.selectable)
                {
                    pathfinding.ResetTiles();
                    pathfinding.Pathfinder(currentTile, buffParameters, 2, false);
                    currentTile.UpdateSelectionColor(buffParameters.selectionColor, false);
                }
                break;
            case 5:
                pathfinding.Pathfinder(currentTile, attackParameters, 1, true);
                if (mouseTile != null && mouseTile.selectable && mouseTile.occupied && !mouseTile.occupation.CompareTag(gameObject.tag))
                {
                    mouseTile.UpdateSelectionColor(attackParameters.selectionColor, false);
                }
                break;
            case 6:
                pathfinding.Pathfinder(currentTile, attackParameters, 3, true);
                if(mouseTile != null && mouseTile.selectable)
                {
                    mouseTile.UpdateSelectionColor(attackParameters.selectionColor, false);
                    foreach(TileScript adjacentTile in mouseTile.adjacencyList)
                    {
                        adjacentTile.UpdateSelectionColor(attackParameters.selectionColor, false);
                    }
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        if (dayOfBlackSunDuration > 0 && dayOfBlackSunSprite.color.a < 0.7f)
        {
            Color color = dayOfBlackSunSprite.color;
            color.a += 1 * Time.fixedDeltaTime;
            if(color.a > 0.7f)
            {
                color.a = 0.7f;
            }
            dayOfBlackSunSprite.color = color;
        }
        else if (dayOfBlackSunDuration <= 0 && dayOfBlackSunSprite.color.a > 0)
        {
            Color color = dayOfBlackSunSprite.color;
            color.a -= 1 * Time.fixedDeltaTime;
            dayOfBlackSunSprite.color = color;
        }
    }

    public override void StartTurn()
    {
        base.StartTurn();
        if(dayOfBlackSunDuration > 0)
        {
            dayOfBlackSunDuration--;
            if(dayOfBlackSunDuration <= 0)
            {
                DayOfBlackSunEnd();
            }
        }
    }

    public override void UseAbility()
    {
        TileScript clickedTile = mouseOverTiles.GetClickedTile();
        if (clickedTile == null || !clickedTile.selectable || mouseOverTiles.mouseOverUI)
        {
            return;
        }

        if (playerScript.ultimateActive && playerScript.actionPoints >= playerScript.ultimateAPCost)
        {
            if(clickedTile == pathfinding.GetCurrentTile())
            {
                DayOfBlackSunStart();
            }
        }

        switch (playerScript.activeAbility)
        {
            case 2:
                if(weapon == 0)
                {
                    BoomerangInitiate(clickedTile);
                }
                else
                {
                    SwordInitiate(clickedTile);
                }
                break;
            case 4:
                SneakAttack();
                break;
            case 5:
                FlyingKickAPowInitiate(clickedTile);
                break;
            case 6:
                StinkBombInitiate(clickedTile);
                break;
        }
    }

    void BoomerangInitiate(TileScript clickedTile)
    {
        PlayerScript enemyScript = clickedTile.occupation.GetComponent<PlayerScript>();
        if (enemyScript != null && !enemyScript.gameObject.CompareTag(gameObject.tag))
        {
            playerScript.FaceCharacter(clickedTile.transform);
            currentTarget = enemyScript;
            animator.Play("Boomerang");
        }
    }

    public void BoomerangHit()
    {
        currentTarget.TakeDamage(boomerangDamage);
        playerScript.ActivateAbility(0);
        playerScript.actionPoints = 0;
        currentTarget = null;
    }

    public void CatchBoomerang()
    {
        animator.Play("CatchBoomerang");
    }
 
    void SwordInitiate(TileScript clickedTile)
    {
        PlayerScript enemyScript = clickedTile.occupation.GetComponent<PlayerScript>();
        if (enemyScript != null && !enemyScript.gameObject.CompareTag(gameObject.tag))
        {
            playerScript.FaceCharacter(clickedTile.transform);
            currentTarget = enemyScript;
            animator.Play("SpaceSword");
        }
    }
    
    public void SwordFinal()
    {
        currentTarget.TakeDamage(swordDamage + currentTarget.armor);
        playerScript.ActivateAbility(0);
        playerScript.actionPoints = 0;
        currentTarget = null;
    }

    void SneakAttack()
    {
        foreach(PlayerScript player in tm.players)
        {
            Pathfinding playerPathfinding = player.GetComponent<Pathfinding>();
            TileScript playerTile = playerPathfinding.GetCurrentTile();
            if (player.CompareTag(gameObject.tag) && playerTile.selectable)
            {
                if(player.speedBost <= sneakAttackDuration)
                {
                    player.speedBost = sneakAttackDuration;
                }

                player.IncreaseTurnMeter(sneakAttackTurnMeter);
            }
        }

        playerScript.ActivateAbility(0);
        playerScript.abilityCooldowns[4] = playerScript.maxAbilityCooldowns[4];
        currentTarget = null;
    }

    void FlyingKickAPowInitiate(TileScript clickedTile)
    {
        PlayerScript enemyScript = clickedTile.occupation.GetComponent<PlayerScript>();
        if (enemyScript != null && !enemyScript.gameObject.CompareTag(gameObject.tag))
        {
            playerScript.FaceCharacter(clickedTile.transform);
            currentTarget = enemyScript;
            animator.Play("FlyingKickAPow");
        }
    }

    public void FlyingKickAPowFinal()
    {
        bool willMove = false;
        TileScript currentTile = pathfinding.GetCurrentTile();
        Pathfinding enemyPathfinding = currentTarget.GetComponent<Pathfinding>();
        PlayerMovement enemyMovement = currentTarget.GetComponent<PlayerMovement>();
        TileScript enemyTile = enemyPathfinding.GetCurrentTile();
        string direction = pathfinding.GetDirectionOfAdjacentTile(currentTile, enemyTile);
        TileScript nextTile = enemyTile;
        for(int i = 0; i < kickPushDistance; i++)
        {
            nextTile = nextTile.GetAdjacentTile(direction);
            if(nextTile != null)
            {
                if (!nextTile.occupied)
                {
                    enemyMovement.GetPath(nextTile);
                    willMove = true;
                }
                else
                {
                    currentTarget.stun += 2;
                    break;
                }
            }
            else
            {
                return;
            }
        }

        if (willMove)
        {
            enemyMovement.FollowPath();
        }

        playerScript.ActivateAbility(0);
        playerScript.abilityCooldowns[5] = playerScript.maxAbilityCooldowns[5];
        playerScript.actionPoints -= playerScript.actionPointCosts[5];
        currentTarget = null;
    }

    void StinkBombInitiate(TileScript clickedTile)
    {
        playerScript.FaceCharacter(clickedTile.transform);
        stinkBombTarget = clickedTile;
        animator.Play("StinkBomb");
        playerScript.ActivateAbility(0);
        playerScript.abilityCooldowns[6] = playerScript.maxAbilityCooldowns[6];
        playerScript.actionPoints = 0;
        currentTarget = null;
    }

    void DayOfBlackSunStart()
    {
        playerScript.ultimateActive = false;
        playerScript.ultimateCD = playerScript.maxUltimateCD;
        dayOfBlackSunDuration = dayOfBlackSunMaxDuration;
        foreach(PlayerScript player in tm.players)
        {
            if (!player.CompareTag(gameObject.tag))
            {
                player.dayOfBlackSun = true;
            }
        }
    }

    void DayOfBlackSunEnd()
    {
        foreach(PlayerScript player in tm.players)
        {
            player.dayOfBlackSun = false;
        }
    }

    public override void ActivateAbility(int abilityID)
    {
        base.ActivateAbility(abilityID);
        if(abilityID == 3)
        {
            animator.Play("SwitchWeapon");
        }
        else
        {
            playerScript.ActivateAbility(abilityID);
        }
    }
}

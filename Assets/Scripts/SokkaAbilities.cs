using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SokkaAbilities : CharacterAbilities
{
    [System.NonSerialized] public int weapon = 0;
    [SerializeField] Animator animator;
    [SerializeField] PathfindingParameters attackParameters;
    [SerializeField] PathfindingParameters buffParameters;
    [SerializeField] SpriteRenderer dayOfBlackSunSprite;
    [SerializeField] GameObject boomerangPrefab;
    [SerializeField] GameObject stinkBombPrefab;
    public PlayerScript currentTarget;
    public TileScript stinkBombTarget;
    UIManager uim;

    int dayOfBlackSunDuration = 0;

    public override void Start()
    {
        base.Start();
        //// boomerang/sword attack cost and cooldown
        //playerScript.actionPointCosts.Add(1);
        //playerScript.maxAbilityCooldowns.Add(0);
        //// switch weapon cost and cooldown
        //playerScript.actionPointCosts.Add(1);
        //playerScript.maxAbilityCooldowns.Add(0);
        //// Sneak Attack cost and cooldown
        //playerScript.actionPointCosts.Add(0);
        //playerScript.maxAbilityCooldowns.Add(3);
        //playerScript.silenceableAbilities.Add(4);
        //// flying kick a pow cost and cooldown
        //playerScript.actionPointCosts.Add(1);
        //playerScript.maxAbilityCooldowns.Add(3);
        //playerScript.silenceableAbilities.Add(5);
        //// stink bomb cost and cooldown
        //playerScript.actionPointCosts.Add(1);
        //playerScript.maxAbilityCooldowns.Add(4);
        //playerScript.silenceableAbilities.Add(6);

        //playerScript.abilityCooldowns = new int[playerScript.maxAbilityCooldowns.Count];

        uim = tm.gameObject.GetComponent<UIManager>();

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
                pathfinding.Pathfinder(currentTile, attackParameters, playerScript.characterData.abilities[1].range, true);
                if (mouseTile != null && mouseTile.selectable && mouseTile.occupied && !mouseTile.occupation.CompareTag(gameObject.tag))
                {
                    mouseTile.UpdateSelectionColor(attackParameters.selectionColor, false);
                }
                break;
            case 3:
                pathfinding.Pathfinder(currentTile, attackParameters, playerScript.characterData.abilities[2].range, true);
                if (mouseTile != null && mouseTile.selectable && mouseTile.occupied && !mouseTile.occupation.CompareTag(gameObject.tag))
                {
                    mouseTile.UpdateSelectionColor(attackParameters.selectionColor, false);
                }
                break;
            case 4:
                pathfinding.Pathfinder(currentTile, buffParameters, playerScript.characterData.abilities[3].range,true);
                if(mouseTile != null && mouseTile.selectable)
                {
                    pathfinding.ResetTiles();
                    pathfinding.Pathfinder(currentTile, buffParameters, 2, false);
                    currentTile.UpdateSelectionColor(buffParameters.selectionColor, false);
                }
                break;
            case 5:
                pathfinding.Pathfinder(currentTile, attackParameters, playerScript.characterData.abilities[4].range, true);
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
                BoomerangInitiate(clickedTile);
                break;
            case 3:
                SwordInitiate(clickedTile);
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
            Boomerang boomerangScript = Instantiate(boomerangPrefab).GetComponent<Boomerang>();
            boomerangScript.transform.position = transform.position;
            boomerangScript.target = currentTarget.transform;
            boomerangScript.returnPosition = transform;
            boomerangScript.abilities = this;
        }
    }

    public void BoomerangHit()
    {
        currentTarget.TakeDamage(playerScript.characterData.abilities[1].damage);
        playerScript.ActivateAbility(0);
        playerScript.actionPoints = 0;
        currentTarget = null;
    }
 
    void SwordInitiate(TileScript clickedTile)
    {
        PlayerScript enemyScript = clickedTile.occupation.GetComponent<PlayerScript>();
        if (enemyScript != null && !enemyScript.gameObject.CompareTag(gameObject.tag))
        {
            playerScript.FaceCharacter(clickedTile.transform);
            currentTarget = enemyScript;
            tokenAnimations.MeleeAttack(enemyScript.transform.position, SwordFinal);
        }
    }
    
    public void SwordFinal()
    {
        currentTarget.TakeDamage(playerScript.characterData.abilities[2].damage + currentTarget.armor);
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
                if(player.speedBoost <= playerScript.characterData.abilities[3].duration)
                {
                    player.speedBoost = playerScript.characterData.abilities[3].duration;
                }

                player.IncreaseTurnMeter(playerScript.characterData.abilities[3].gainTurnMeter);
            }
        }

        playerScript.CalculateMoveSpeed();
        uim.UpdateTooltips(playerScript);
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
            tokenAnimations.MeleeAttack(enemyScript.transform.position, FlyingKickAPowFinal);
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
        for(int i = 0; i < playerScript.characterData.abilities[4].moveEffectRange; i++)
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
                    currentTarget.stun += playerScript.characterData.abilities[4].duration;
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
        playerScript.ActivateAbility(0);
        playerScript.abilityCooldowns[6] = playerScript.maxAbilityCooldowns[6];
        playerScript.actionPoints = 0;
        currentTarget = null;
        StinkBomb stinkBombScript = Instantiate(stinkBombPrefab).GetComponent<StinkBomb>();
        stinkBombScript.transform.position = transform.position;
        stinkBombScript.target = stinkBombTarget;
        stinkBombScript.myTeam = gameObject.tag;
        stinkBombScript.silenceDuration = playerScript.characterData.abilities[5].duration;
    }

    void DayOfBlackSunStart()
    {
        playerScript.ultimateActive = false;
        playerScript.ultimateCD = playerScript.maxUltimateCD;
        dayOfBlackSunDuration = playerScript.characterData.ultimate.duration;
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
        playerScript.ActivateAbility(abilityID);
    }
}

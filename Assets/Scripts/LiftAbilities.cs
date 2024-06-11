using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftAbilities : CharacterAbilities
{
    [SerializeField] PathfindingParameters slicknessParameters;
    [SerializeField] PathfindingParameters pickpocketParameters;
    [SerializeField] PathfindingParameters healingParameters;
    [SerializeField] Color lifelightColor;
    StormlightAnimations stormlightAnimations;
    PlayerMovement playerMovement;
    PlayerScript currentTarget;
    UIManager uim;
    int lifelight;
    [SerializeField] int maxLifelight = 7;

    int foodHeistTurnMeter = 300;

    TileScript slicknessDestination;
    Vector2 slicknessDirection;
    float slicknessSpeed = 5;
    public bool sliding = false;
    int food = 0;
    int foodMax = 10;

    [SerializeField] int regrowthAmount = 5;

    int awesomenessRange = 5;
    int ultimateHealingAmount = 6;
    public int awesomenessNumber;

    public override void Start()
    {
        base.Start();
        playerMovement = GetComponent<PlayerMovement>();
        stormlightAnimations = GetComponentInChildren<StormlightAnimations>();

        // Food heist cost and cooldown
        playerScript.actionPointCosts.Add(1);
        playerScript.maxAbilityCooldowns.Add(1);
        // Eat pancakes cost and cooldown
        playerScript.actionPointCosts.Add(1);
        playerScript.maxAbilityCooldowns.Add(0);
        // Slickness cost and cooldown
        playerScript.actionPointCosts.Add(0);
        playerScript.maxAbilityCooldowns.Add(1);
        playerScript.silenceableAbilities.Add(4);
        // Awesomeness cost and cooldown
        playerScript.actionPointCosts.Add(1);
        playerScript.maxAbilityCooldowns.Add(0);
        playerScript.silenceableAbilities.Add(5);

        playerScript.abilityCooldowns = new int[playerScript.maxAbilityCooldowns.Count];

        lifelight = maxLifelight;
        uim = tm.gameObject.GetComponent<UIManager>();
    }

    public override void StartTurn()
    {
        base.StartTurn();
        if(playerScript.rooted > 0)
        {
            playerScript.rooted = 0;
            playerScript.greyedOutAbilities.Remove(1);
        }

        if (playerScript.health < playerScript.maxHealth && lifelight > 0)
        {
            playerScript.GetHealed(2);
            lifelight -= 1;
        }
        uim.UpdateMana(lifelight, lifelightColor);
    }

    public override void EndTurn()
    {
        base.EndTurn();
        TileScript currentTile = pathfinding.GetCurrentTile();
        uim.UpdateMana(0, lifelightColor);
    }

    public override void Respawn()
    {
        base.Respawn();
        lifelight = maxLifelight;
        uim.UpdateMana(lifelight, lifelightColor);
    }

    private void Update()
    {
        if (sliding)
        {
            Slickness();
            pathfinding.ResetTiles();
            return;
        }

        if (playerScript.activeAbility <= 1 && !playerScript.ultimateActive)
        {
            return;
        }

        pathfinding.ResetTiles();
        TileScript currentTile = pathfinding.GetCurrentTile();
        currentTile.UpdateSelectionColor(1, true);
        TileScript mouseTile = mouseOverTiles.GetClickedTile();

        if(playerScript.ultimateActive)
        {
            pathfinding.Pathfinder(currentTile, slicknessParameters, awesomenessRange, true);
            if (mouseTile != null && mouseTile.selectable && !mouseTile.occupied)
            {
                mouseTile.UpdateSelectionColor(slicknessParameters.selectionColor, false);
            }
            return;
        }

        switch (playerScript.activeAbility)
        {
            case 2:
                pathfinding.Pathfinder(currentTile, pickpocketParameters, 1, true);
                if(mouseTile != null && mouseTile.selectable && mouseTile.occupied && mouseTile.occupation != gameObject)
                {
                    if(mouseTile.occupation.CompareTag("Team1") || mouseTile.occupation.CompareTag("Team2"))
                    {
                        mouseTile.UpdateSelectionColor(pickpocketParameters.selectionColor, false);
                    }
                }
                break;
            case 3:
                currentTile.selectable = true;
                currentTile.UpdateSelectionColor(3, true);
                if (mouseTile == currentTile && lifelight < maxLifelight && food > 0)
                {
                    mouseTile.UpdateSelectionColor(3, false);
                }
                break;
            case 4:
                pathfinding.Pathfinder(currentTile, slicknessParameters, 4, true);
                if (mouseTile != null && mouseTile.selectable && !mouseTile.occupied)
                {
                    mouseTile.UpdateSelectionColor(slicknessParameters.selectionColor, false);
                }
                break;
            case 5:
                pathfinding.Pathfinder(currentTile, healingParameters, 1, true);
                currentTile.selectable = true;
                currentTile.UpdateSelectionColor(healingParameters.selectionColor, true);
                if (mouseTile != null && mouseTile.selectable && mouseTile.occupied)
                {
                    PlayerScript ally = mouseTile.occupation.GetComponent<PlayerScript>();
                    if (ally != null && mouseTile.occupation.CompareTag(gameObject.tag) && ally.health < ally.maxHealth)
                    {
                        mouseTile.UpdateSelectionColor(healingParameters.selectionColor, false);
                    }
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

        if (playerScript.ultimateActive)
        {
            AwesomenessInitiate(clickedTile);
            return;
        }

        switch (playerScript.activeAbility)
        {
            case 2:
                FoodHeist(clickedTile);
                break;
            case 3:
                EatPancakes();
                break;
            case 4:
                SlicknessInitiate(clickedTile);
                break;
            case 5:
                RegrowthInitiate(clickedTile);
                break;
        }
    }

    void FoodHeist(TileScript clickedTile)
    {
        PlayerScript targetScript = clickedTile.occupation.GetComponent<PlayerScript>();
        if (targetScript != null)
        {
            playerScript.FaceCharacter(clickedTile.transform);
            currentTarget = targetScript;
            playerScript.actionPoints -= playerScript.actionPointCosts[2];
            playerScript.abilityCooldowns[2] = playerScript.maxAbilityCooldowns[2];
            playerScript.ActivateAbility(0);
            currentTarget.DecreaseTurnMeter(foodHeistTurnMeter);
            playerScript.characterEvents.Buff();
            food += 5;
            if (food > foodMax)
            {
                food = foodMax;
            }
            currentTarget = null;
        }
    }

    void EatPancakes()
    {
        if(lifelight < maxLifelight && food > 0)
        {
            playerScript.actionPoints -= playerScript.actionPointCosts[3];
            playerScript.abilityCooldowns[3] = playerScript.maxAbilityCooldowns[3];
            playerScript.ActivateAbility(0);
            int lifeLightDiff = maxLifelight - lifelight;
            if (lifeLightDiff >= food)
            {
                lifelight += food;
                food = 0;
            }
            else
            {
                lifelight = maxLifelight;
                food -= lifeLightDiff;
            }
            uim.UpdateMana(lifelight, lifelightColor);
            stormlightAnimations.StormlightFlare();
        }
    }

    void SlicknessInitiate(TileScript clickedTile)
    {
        if (!clickedTile.occupied)
        {
            playerScript.FaceCharacter(clickedTile.transform);
            slicknessDestination = clickedTile;
            slicknessDirection = slicknessDestination.transform.position - transform.position;
            playerMovement.movePoint.position = transform.position;
            playerScript.abilityCooldowns[4] = playerScript.maxAbilityCooldowns[4];
            lifelight -= 1;
            uim.UpdateMana(lifelight, lifelightColor);
            playerScript.ActivateAbility(0);
            TileScript currentTile = pathfinding.GetCurrentTile();
            currentTile.occupied = false;
            currentTile.occupation = currentTile.gameObject;
            stormlightAnimations.StartStormlight(() => sliding = true);
        }
    }

    public void Slickness()
    {
        playerMovement.movePoint.Translate(slicknessDirection.normalized * Time.deltaTime * slicknessSpeed);
        transform.position = playerMovement.movePoint.position;

        if (Vector2.Distance(playerMovement.movePoint.position, slicknessDestination.transform.position) <= Time.deltaTime * slicknessSpeed)
        {
            transform.position = slicknessDestination.transform.position;
            sliding = false;
            TileScript currentTile = pathfinding.GetCurrentTile();
            playerMovement.OccupyTile(currentTile);
            if (playerScript.ultimateActive)
            {
                AwesomenessArrival(currentTile);
            }
            else
            {
                stormlightAnimations.EndStormlight();
            }
        }
    }

    void RegrowthInitiate(TileScript clickedTile)
    {
        PlayerScript ally = clickedTile.occupation.GetComponent<PlayerScript>();
        if(ally != null && ally.gameObject.CompareTag(gameObject.tag) && ally.health < ally.maxHealth)
        {
            currentTarget = ally;
            playerScript.actionPoints -= playerScript.actionPointCosts[5];
            playerScript.abilityCooldowns[5] = playerScript.maxAbilityCooldowns[5];
            playerScript.ActivateAbility(0);
            stormlightAnimations.StartStormlight(RegrowthFinal);
        }
    }

    public void RegrowthFinal()
    {
        lifelight -= 2;
        uim.UpdateMana(lifelight, lifelightColor);
        currentTarget.GetHealed(regrowthAmount);
        currentTarget = null;
        StartCoroutine(Delay(0.3f, stormlightAnimations.EndStormlight));
    }

    void AwesomenessInitiate( TileScript clickedTile)
    {
        if (!clickedTile.occupied)
        {
            stormlightAnimations.StartStormlight(() => sliding = true);
            slicknessDestination = clickedTile;
            slicknessDirection = slicknessDestination.transform.position - transform.position;
            playerMovement.movePoint.position = transform.position;

            TileScript currentTile = pathfinding.GetCurrentTile();
            currentTile.occupied = false;
            currentTile.occupation = currentTile.gameObject;
        }
    }

    void AwesomenessArrival(TileScript currentTile)
    {
        awesomenessNumber--;
        stormlightAnimations.StormlightPulse();
        foreach(TileScript tile in currentTile.adjacencyList)
        {
            if (tile.occupation.CompareTag(gameObject.tag))
            {
                PlayerScript allyScript = tile.occupation.GetComponent<PlayerScript>();
                allyScript.GetHealed(ultimateHealingAmount);
            }
        }

        if(awesomenessNumber == 0)
        {
            playerScript.ultimateActive = false;
            playerScript.ActivateAbility(0);
            playerScript.actionPoints -= playerScript.ultimateAPCost;
            playerScript.ultimateCD = playerScript.maxUltimateCD;
            currentTarget = null;
            stormlightAnimations.EndStormlight();
        }
    }

    public override bool ShouldBeGreyedOut(int abilityID)
    {
        if (abilityID == 4 && lifelight < 1)
        {
            return true;
        }
        else if(abilityID == 5 && lifelight < 2)
        {
            return true;
        }
        else if(abilityID == 3)
        {
            if(lifelight == maxLifelight || food <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public override int AbilityButtonCounter(int abilityID)
    {
        if (abilityID == 3)
        {
            return food;
        }
        else
        {
            return -1;
        }
    }

    public override void ActivateAbility(int abilityID)
    {
        base.ActivateAbility(abilityID);
        if(abilityID < 4)
        {
            playerScript.ActivateAbility(abilityID);
        }
        else if (abilityID == 4 && lifelight >= 1)
        {
            playerScript.ActivateAbility(abilityID);
        }
        else if (abilityID == 5 && lifelight >= 2)
        {
            playerScript.ActivateAbility(abilityID);
        }
    }

    public override void ActivateUltimate()
    {
        base.ActivateUltimate();
        awesomenessNumber = 3;
    }
}

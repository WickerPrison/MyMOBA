using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VinAbilities : CharacterAbilities
{
    [SerializeField] PathfindingParameters attackParameters;
    [SerializeField] PathfindingParameters flyingParameters;
    [SerializeField] Animator animator;
    [SerializeField] Color metalColor;
    PlayerMovement playerMovement;
    PlayerScript currentTarget;
    UIManager uim;
    int maxMetals = 5;
    int metals;

    int glassDaggersRange = 1;
    [SerializeField] int glassDaggersDamage = 6;

    int pewter;

    TileScript steelpushDestination;
    public bool steelpush = false;
    float steelpushTotalDistance;
    float steelpushDistance;
    Vector2 steelpushDirection;
    float steelpushSpeed = 4;
    float arcHeight = 3;
    float midpoint;
    float arcWidth;

    bool duralumin;

    int oneWithMistsMax = 2;
    int oneWithMists = 0;

    public override void Start()
    {
        base.Start();
        // glass daggers cost and cooldown
        playerScript.actionPointCosts.Add(1);
        playerScript.maxAbilityCooldowns.Add(0);
        // metal vial cost and cooldown
        playerScript.actionPointCosts.Add(1);
        playerScript.maxAbilityCooldowns.Add(1);
        // pewter cost and cooldown
        playerScript.actionPointCosts.Add(0);
        playerScript.maxAbilityCooldowns.Add(1);
        playerScript.silenceableAbilities.Add(4);
        // steel cost and cooldown
        playerScript.actionPointCosts.Add(1);
        playerScript.maxAbilityCooldowns.Add(0);
        playerScript.silenceableAbilities.Add(5);
        // duralumin cost and cooldown
        playerScript.actionPointCosts.Add(0);
        playerScript.maxAbilityCooldowns.Add(5);
        playerScript.silenceableAbilities.Add(6);


        playerScript.abilityCooldowns = new int[playerScript.maxAbilityCooldowns.Count];

        playerMovement = GetComponent<PlayerMovement>();
        uim = tm.gameObject.GetComponent<UIManager>();
        metals = maxMetals;
    }

    private void Update()
    {
        if (steelpush)
        {
            SteelPush();
            pathfinding.ResetTiles();
            return;
        }

        if (playerScript.activeAbility <= 1 && !playerScript.ultimateActive)
        {
            return;
        }

        pathfinding.ResetTiles();
        TileScript currentTile = pathfinding.GetCurrentTile();
        if(currentTile != null)
        {
            currentTile.UpdateSelectionColor(1, true);
        }
        TileScript mouseTile = mouseOverTiles.GetClickedTile();

        if (playerScript.ultimateActive)
        {
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
                pathfinding.Pathfinder(currentTile, attackParameters, glassDaggersRange, true);
                if (mouseTile != null && mouseTile.selectable && mouseTile.occupied && !mouseTile.occupation.CompareTag(gameObject.tag))
                {
                    mouseTile.UpdateSelectionColor(attackParameters.selectionColor, false);
                }
                break;
            case 3:
                currentTile.selectable = true;
                currentTile.UpdateSelectionColor(3, true);
                if (mouseTile == currentTile)
                {
                    mouseTile.UpdateSelectionColor(3, false);
                }
                break;
            case 4:
                currentTile.selectable = true;
                currentTile.UpdateSelectionColor(3, true);
                if (mouseTile == currentTile)
                {
                    mouseTile.UpdateSelectionColor(3, false);
                }
                break;
            case 5:
                if (duralumin)
                {
                    pathfinding.Pathfinder(currentTile, flyingParameters, 3 * metals, true);
                }
                else
                {
                    pathfinding.Pathfinder(currentTile, flyingParameters, 5, true);
                }

                if (mouseTile != null && mouseTile.selectable && !mouseTile.occupied)
                {
                    mouseTile.UpdateSelectionColor(flyingParameters.selectionColor, false);
                }
                break;
            case 6:
                currentTile.selectable = true;
                currentTile.UpdateSelectionColor(3,true);
                if (mouseTile == currentTile)
                {
                    mouseTile.UpdateSelectionColor(3, false);
                }
                break;
        }
    }

    public override void StartTurn()
    {
        base.StartTurn();
        uim.UpdateMana(metals, metalColor);
        pewter = 0;
        playerScript.CalculateArmor();
        if(oneWithMists > 0)
        {
            oneWithMists--;
        }
    }

    public override int ArmorModifications(int armor)
    {
        return armor + pewter;
    }

    public override void EndTurn()
    {
        base.EndTurn();
        uim.UpdateMana(0, metalColor);
    }

    public override void Respawn()
    {
        base.Respawn();
        metals = maxMetals;
        uim.UpdateMana(metals, metalColor);
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
            OneWithTheMists();
            return;
        }

        switch (playerScript.activeAbility)
        {
            case 2:
                GlassDaggersInitiate(clickedTile);
                break;
            case 3:
                animator.Play("MetalVial");
                break;
            case 4:
                Pewter();
                break;
            case 5:
                SteelInitiate(clickedTile);
                break;
            case 6:
                duralumin = true;
                playerScript.abilityCooldowns[6] = playerScript.maxAbilityCooldowns[6];
                playerScript.ActivateAbility(0);
                animator.Play("Duralumin");
                break;
        }
    }

    void GlassDaggersInitiate(TileScript clickedTile)
    {
        PlayerScript enemyScript = clickedTile.occupation.GetComponent<PlayerScript>();
        if (enemyScript != null && !enemyScript.gameObject.CompareTag(gameObject.tag))
        {
            playerScript.FaceCharacter(clickedTile.transform);
            currentTarget = enemyScript;
            animator.Play("GlassDaggers");
        }
    }

    public void GlassDaggersFinish()
    {
        currentTarget.TakeDamage(glassDaggersDamage + pewter);
        playerScript.ActivateAbility(0);
        playerScript.actionPoints = 0;
        currentTarget = null;
    }

    public void MetalVial()
    {
        metals = maxMetals;
        uim.UpdateMana(metals, metalColor);
        playerScript.actionPoints -= playerScript.actionPointCosts[3];
        playerScript.abilityCooldowns[3] = playerScript.maxAbilityCooldowns[3];
        playerScript.ActivateAbility(0);
    }

    void Pewter()
    {
        animator.Play("Pewter");
        int amount;
        if (duralumin)
        {
            amount = metals;
            duralumin = false;
        }
        else
        {
            amount = 2;
        }
        playerScript.actionPoints -= playerScript.actionPointCosts[4];
        playerScript.abilityCooldowns[4] = playerScript.maxAbilityCooldowns[4];
        playerScript.ActivateAbility(0);
        pewter = amount;
        metals -= amount;
        if (oneWithMists > 0)
        {
            metals = maxMetals;
        }
        uim.UpdateMana(metals, metalColor);
        playerScript.CalculateArmor();
    }

    void SteelInitiate(TileScript clickedTile)
    {
        if (!clickedTile.occupied)
        {
            animator.Play("Takeoff");
            playerScript.FaceCharacter(clickedTile.transform);
            steelpushDestination = clickedTile;
            steelpushTotalDistance = Vector2.Distance(transform.position, steelpushDestination.transform.position);
            steelpushDirection = steelpushDestination.transform.position - transform.position;
            playerMovement.movePoint.position = transform.position;
            midpoint = steelpushTotalDistance / 2;
            arcWidth = arcHeight / Mathf.Pow(midpoint, 2);
            playerScript.actionPoints -= playerScript.actionPointCosts[5];
            playerScript.abilityCooldowns[5] = playerScript.maxAbilityCooldowns[5];
            if (duralumin)
            {
                duralumin = false;
                metals = 0;
            }
            else
            {
                metals -= 2;
            }

            if(oneWithMists > 0)
            {
                metals = maxMetals;
            }
            uim.UpdateMana(metals, metalColor);
            playerScript.ActivateAbility(0);
            TileScript currentTile = pathfinding.GetCurrentTile();
            currentTile.occupied = false;
            currentTile.occupation = currentTile.gameObject;
        }
    }


    void SteelPush()
    {
        playerMovement.movePoint.Translate(steelpushDirection.normalized * Time.deltaTime * steelpushSpeed);
        steelpushDistance = Vector2.Distance(playerMovement.movePoint.position, steelpushDestination.transform.position);
        float currentHeight = -arcWidth * Mathf.Pow(steelpushDistance - midpoint, 2) + arcHeight;
        transform.position = playerMovement.movePoint.position + new Vector3(0, currentHeight,0);
        float distanceDecimal = steelpushDistance / steelpushTotalDistance;
        animator.SetFloat("SteelpushDistance", distanceDecimal);
        if(steelpushDistance <= 0.1f)
        {
            transform.position = steelpushDestination.transform.position;
            steelpush = false;
        }
    }

    public void SteelLanding()
    {
        TileScript currentTile = pathfinding.GetCurrentTile();
        playerMovement.OccupyTile(currentTile);
        currentTile.occupied = true;
        currentTile.occupation = this.gameObject;
    }

    public void OneWithTheMists()
    {
        playerScript.ultimateActive = false;
        playerScript.ultimateCD = playerScript.maxUltimateCD;
        oneWithMists = oneWithMistsMax;
        metals = maxMetals;
        uim.UpdateMana(metals, metalColor);
    }

    public override bool ShouldBeGreyedOut(int abilityID)
    {
        if(abilityID == 5 && playerScript.rooted > 0)
        {
            return true;
        }

        if(abilityID > 3 && metals < 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void ActivateAbility(int abilityID)
    {
        base.ActivateAbility(abilityID);

        if(abilityID > 3)
        {
            if (abilityID == 5 && playerScript.rooted > 0)
            {
                return;
            }

            if (metals > 1)
            {
                playerScript.ActivateAbility(abilityID);
            }
        }
        else
        {
            playerScript.ActivateAbility(abilityID);
        }

    }
}

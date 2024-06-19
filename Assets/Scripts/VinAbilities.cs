using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VinAbilities : CharacterAbilities, IDamageCalc, IMoveCalc, IValueCalc
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
    TokenJump tokenJump;

    int pewter;

    bool steelpush = false;

    bool duralumin;

    int oneWithMistsMax = 2;
    int oneWithMists = 0;

    public override void Start()
    {
        base.Start();

        playerMovement = GetComponent<PlayerMovement>();
        uim = tm.gameObject.GetComponent<UIManager>();
        tokenJump = GetComponentInChildren<TokenJump>();
        metals = maxMetals;
    }

    private void Update()
    {
        if (steelpush)
        {
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
                pathfinding.Pathfinder(currentTile, attackParameters, playerScript.characterData.abilities[1].range, true);
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

                pathfinding.Pathfinder(currentTile, flyingParameters, MoveCalc(), true);

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
                MetalVial();
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
                playerScript.characterEvents.CustomEffect(1);
                uim.UpdateTooltips(playerScript);
                break;
        }
    }

    public int DamageCalc()
    {
        return playerScript.characterData.abilities[1].damage + pewter;
    }

    void GlassDaggersInitiate(TileScript clickedTile)
    {
        PlayerScript enemyScript = clickedTile.occupation.GetComponent<PlayerScript>();
        if (enemyScript != null && !enemyScript.gameObject.CompareTag(gameObject.tag))
        {
            playerScript.FaceCharacter(clickedTile.transform);
            currentTarget = enemyScript;
            tokenAnimations.MeleeAttack(currentTarget.transform.position, GlassDaggersFinish);
        }
    }

    public void GlassDaggersFinish()
    {
        currentTarget.TakeDamage(DamageCalc());
        playerScript.ActivateAbility(0);
        playerScript.actionPoints = 0;
        currentTarget = null;
        uim.UpdateTooltips(playerScript);
    }

    public void MetalVial()
    {
        playerScript.characterEvents.Buff();
        metals = maxMetals;
        uim.UpdateMana(metals, metalColor);
        playerScript.actionPoints -= playerScript.actionPointCosts[3];
        playerScript.abilityCooldowns[3] = playerScript.maxAbilityCooldowns[3];
        playerScript.ActivateAbility(0);
    }

    public int ValueCalc()
    {
        if (duralumin)
        {
            return metals;
        }
        else
        {
            return playerScript.characterData.abilities[3].value;
        }
    }

    void Pewter()
    {
        playerScript.characterEvents.CustomEffect(0);
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
        uim.UpdateTooltips(playerScript);
        playerScript.CalculateArmor();
    }

    public int MoveCalc()
    {
        if (duralumin)
        {
            return 3 * metals;
        }
        else
        {
           return 5;
        }
    }

    void SteelInitiate(TileScript clickedTile)
    {
        if (!clickedTile.occupied)
        {
            tokenJump.Jump(transform.position, clickedTile.transform.position, () => steelpush = false);
            steelpush = true;
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
            uim.UpdateTooltips(playerScript);
            playerScript.ActivateAbility(0);
            TileScript currentTile = pathfinding.GetCurrentTile();
            currentTile.occupied = false;
            currentTile.occupation = currentTile.gameObject;
        }
    }

    public void OneWithTheMists()
    {
        playerScript.characterEvents.CustomEffect(2);
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

        if(abilityID == 4 && metals < playerScript.characterData.abilities[3].manaCost)
        {
            return true;
        }

        if (abilityID == 5 && metals < playerScript.characterData.abilities[4].manaCost)
        {
            return true;
        }

        if (abilityID == 6)
        {
            if(metals < playerScript.characterData.abilities[3].manaCost || metals < playerScript.characterData.abilities[4].manaCost)
            {
                return true;
            }
        }

        return false;
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

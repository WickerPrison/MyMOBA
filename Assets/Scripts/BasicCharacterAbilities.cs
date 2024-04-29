using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCharacterAbilities : CharacterAbilities
{
    [SerializeField] PathfindingParameters pathfindingParameters;
    int attackRange = 2;
    int attackDamage = 5;

    public override void Start()
    {
        base.Start();
        playerScript.actionPointCosts.Add(1);
        playerScript.maxAbilityCooldowns.Add(2);
        playerScript.abilityCooldowns = new int[playerScript.maxAbilityCooldowns.Count];
        playerScript.silenceableAbilities.Add(2);
    }

    public override void UseAbility()
    {
        TileScript clickedTile = mouseOverTiles.GetClickedTile();
        if (clickedTile == null || !clickedTile.selectable)
        {
            return;
        }

        if (playerScript.activeAbility == 2 && clickedTile.occupation.GetComponent<PlayerScript>() != null)
        {
            PlayerScript enemyScript = clickedTile.occupation.GetComponent<PlayerScript>();
            if(enemyScript != null && !enemyScript.gameObject.CompareTag(gameObject.tag))
            {
                playerScript.abilityCooldowns[2] = playerScript.maxAbilityCooldowns[2];
                enemyScript.TakeDamage(attackDamage);
                playerScript.ActivateAbility(0);
                playerScript.actionPoints = 0;
            }
        }
    }

    private void Update()
    {
        if(playerScript.activeAbility == 2)
        {
            pathfinding.ResetTiles();
            TileScript currentTile = pathfinding.GetCurrentTile();
            currentTile.UpdateSelectionColor(1, true);
            pathfinding.Pathfinder(currentTile, pathfindingParameters, attackRange, true);
        }
    }

    public override void ActivateAbility(int abilityID)
    {
        base.ActivateAbility(abilityID);
        playerScript.ActivateAbility(abilityID);
    }
}

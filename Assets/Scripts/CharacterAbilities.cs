using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterAbilities : MonoBehaviour
{
    [System.NonSerialized] public PlayerScript playerScript;
    [System.NonSerialized] public InputManager im;
    [System.NonSerialized] public TurnManager tm;
    [System.NonSerialized] public Pathfinding pathfinding;
    [System.NonSerialized] public MouseOverTiles mouseOverTiles;
    [System.NonSerialized] public TokenAnimations tokenAnimations;

    // Start is called before the first frame update
    public virtual void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        tm = im.gameObject.GetComponent<TurnManager>();
        mouseOverTiles = im.gameObject.GetComponent<MouseOverTiles>();
        playerScript = GetComponent<PlayerScript>();
        pathfinding = GetComponent<Pathfinding>();
        tokenAnimations = GetComponentInChildren<TokenAnimations>();
        SetupControls();
    }

    public virtual void UseAbility()
    {

    }

    public virtual void ActivateAbility(int abilityID)
    {
        playerScript.ultimateActive = false;
    }
    public virtual void ActivateUltimate()
    {
        if (CanUseUltimate())
        {
            playerScript.ActivateAbility(0);
            playerScript.ultimateActive = true;
        }
    }

    public virtual void SetupControls()
    {
        im.controls[tm.players.IndexOf(playerScript)].PlayerTurn.Two.performed += ctx => ActivateAbility(2);
        im.controls[tm.players.IndexOf(playerScript)].PlayerTurn.Three.performed += ctx => ActivateAbility(3);
        im.controls[tm.players.IndexOf(playerScript)].PlayerTurn.Four.performed += ctx => ActivateAbility(4);
        im.controls[tm.players.IndexOf(playerScript)].PlayerTurn.Five.performed += ctx => ActivateAbility(5);
        im.controls[tm.players.IndexOf(playerScript)].PlayerTurn.Six.performed += ctx => ActivateAbility(6);
        im.controls[tm.players.IndexOf(playerScript)].PlayerTurn.LeftClick.performed += ctx => UseAbility();
        im.controls[tm.players.IndexOf(playerScript)].PlayerTurn.Ultimate.performed += ctx => ActivateUltimate();
    }

    public virtual void StartTurn()
    {
        for (int i = 0; i < playerScript.abilityCooldowns.Length; i++)
        {
            if (playerScript.abilityCooldowns[i] > 0)
            {
                playerScript.abilityCooldowns[i]--;
            }
        }
    }

    public virtual void EndTurn()
    {
        ActivateAbility(0);
        TileScript currentTile = pathfinding.GetCurrentTile();
        if(currentTile != null && currentTile.tileType == 1 && currentTile.spawnPointTeam == playerScript.myTeam)
        {
            playerScript.GetHealed(5);
        }
    }

    public virtual void Respawn()
    {

    }

    public virtual int ArmorModifications(int armor)
    {
        return armor;
    }

    public virtual bool ShouldBeGreyedOut(int abilityID)
    {
        return false;
    }

    public virtual int AbilityButtonCounter(int abilityID)
    {
        return -1;
    }

    public virtual void ProjectileHit(TileScript target)
    {

    }

    public virtual void SteppedOnTile(TileScript tile)
    {

    }

    public virtual void FinishedMoving()
    {

    }

    public virtual bool CanUseUltimate()
    {
        if (playerScript.ultimateCD > 0)
        {
            return false;
        }

        if (playerScript.actionPoints < 1)
        {
            return false;
        }

        return true;
    }
}

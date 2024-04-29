using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UltimateCharger : MonoBehaviour
{
    Pathfinding pathfinder;
    TileScript currentTile;
    SortingGroup sortingGroup;
    TurnManager tm;
    InputManager im;
    MouseOverTiles mouseOverTiles;
    [System.NonSerialized] public bool charged = true;
    bool nextToPlayer = false;
    [SerializeField] GameObject indicator;
    UltimateChargeManager chargeManager;

    // Start is called before the first frame update
    void Start()
    {
        pathfinder = GetComponent<Pathfinding>();
        sortingGroup = GetComponent<SortingGroup>();
        tm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TurnManager>();
        im = tm.gameObject.GetComponent<InputManager>();
        chargeManager = GameObject.FindGameObjectWithTag("GameMode").GetComponent<UltimateChargeManager>();
        mouseOverTiles = tm.gameObject.GetComponent<MouseOverTiles>();

        chargeManager.chargers.Add(this);
        currentTile = pathfinder.GetCurrentTile();
        transform.position = currentTile.transform.position;
        sortingGroup.sortingOrder = (int)currentTile.gridPosition.x;
        currentTile.occupied = true;
        currentTile.occupation = gameObject;

        SetupInputs();
    }

    // Update is called once per frame
    void Update()
    {
        if (charged)
        {
            nextToPlayer = false;
            foreach(TileScript tile in currentTile.adjacencyList)
            {
                if(tile.occupation == tm.currentPlayer.gameObject)
                {
                    nextToPlayer = true;
                }
            }

            if (nextToPlayer)
            {
                TileScript mouseTile = mouseOverTiles.GetClickedTile();
                if(mouseTile != null && mouseTile == currentTile && tm.currentPlayer.actionPoints > 0)
                {
                    currentTile.UpdateSelectionColor(3, false);
                }
                else
                {
                    currentTile.UpdateSelectionColor(3, true);
                }
            }
        }
    }

    void Interact()
    {
        if (nextToPlayer && charged && tm.currentPlayer.actionPoints > 0)
        {
            foreach(PlayerScript player in tm.players)
            {
                if (player.CompareTag(tm.currentPlayer.tag))
                {
                    player.ReduceUltimateCD();
                }
            }
            tm.currentPlayer.actionPoints -= 1;
            ChargeOff();
        }
    }

    public void ChargeOn()
    {
        charged = true;
        indicator.SetActive(true);
    }

    void ChargeOff()
    {
        charged = false;
        indicator.SetActive(false);
        nextToPlayer = false;
    }

    void SetupInputs()
    {
        foreach(PlayerScript playerScript in tm.players)
        {
            im.controls[tm.players.IndexOf(playerScript)].PlayerTurn.LeftClick.performed += ctx => Interact();
        }
    }
}

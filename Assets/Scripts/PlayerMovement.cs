using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    public Transform movePoint;
    PlayerScript playerScript;
    CharacterAbilities characterAbilities;
    TileScript currentTile;
    float walkSpeed = 5;
    MouseOverTiles mouseOverTiles;
    InputManager im;
    TurnManager tm;
    Stack<TileScript> path = new Stack<TileScript>();
    bool moving = false;
    TileScript nextTile;
    TileScript pathPoint;
    Vector2 moveDirection;
    public int playerID;
    Pathfinding pathfinding;
    [SerializeField] PathfindingParameters pathfindingParameters;
    [SerializeField] Animator animator;
    SortingGroup sortingGroup;
    [SerializeField] Canvas healthbarCanvas;


    // Start is called before the first frame update
    void Start()
    {
        sortingGroup = GetComponent<SortingGroup>();
        playerScript = gameObject.GetComponent<PlayerScript>();
        characterAbilities = gameObject.GetComponent<CharacterAbilities>();
        pathfinding = GetComponent<Pathfinding>();
        mouseOverTiles = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MouseOverTiles>();
        tm = mouseOverTiles.gameObject.GetComponent<TurnManager>();
        SetUpControls();
        currentTile = pathfinding.GetCurrentTile();
        OccupyTile(currentTile);
        transform.position = new Vector3(currentTile.transform.position.x, currentTile.transform.position.y, 0);
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerScript.dead)
        {
            return;
        }

        moveDirection = movePoint.position - transform.position;
        if (!moving)
        {
            moveDirection = Vector2.zero;
            if(playerScript.movementAbility && playerScript.rooted <= 0)
            {
                pathfinding.ResetTiles();
                currentTile = pathfinding.GetCurrentTile();
                currentTile.UpdateSelectionColor(1, true);
                if(path.Count == 0)
                {
                    pathfinding.Pathfinder(currentTile, pathfindingParameters, playerScript.moveSpeed + playerScript.moveSpeedModifier, true);
                }
                else
                {
                    pathfinding.Pathfinder(pathPoint, pathfindingParameters, playerScript.moveSpeed + playerScript.moveSpeedModifier - path.Count + 1, true);
                }

                TileScript mouseTile = mouseOverTiles.GetClickedTile();
                if (mouseTile != null && mouseTile.selectable && !mouseTile.occupied)
                {
                    mouseTile.UpdateSelectionColor(pathfindingParameters.selectionColor, false);
                }
            }
        }

        if (moving && moveDirection.magnitude <= Time.deltaTime * walkSpeed)
        {
            TileScript steppedOnTile = pathfinding.GetCurrentTile();
            characterAbilities.SteppedOnTile(steppedOnTile);
            if (path.Count > 0)
            {
                FollowPath();
            }
            else
            {
                currentTile = pathfinding.GetCurrentTile();
                moving = false;
                OccupyTile(currentTile);
                playerScript.actionPoints -= playerScript.movementCost;
                playerScript.ActivateAbility(0);
                characterAbilities.FinishedMoving();
            }
        }

        transform.Translate(moveDirection.normalized * Time.deltaTime * walkSpeed);
        animator.SetBool("Moving", moving);

        if (moving)
        {
            pathfinding.ResetTiles();
        }
    }

    void MoveToTile()
    {
        if(!playerScript.movementAbility || playerScript.rooted > 0 || mouseOverTiles.mouseOverUI)
        {
            return;
        }

        TileScript clickedTile = mouseOverTiles.GetClickedTile();
        if (clickedTile != null && clickedTile.selectable && !clickedTile.occupied)
        {
            playerScript.FaceCharacter(clickedTile.transform);
            GetPath(clickedTile);
            FollowPath();
        }
    }

    void SetPathingTile()
    {
        if (!playerScript.movementAbility || playerScript.rooted > 0 || mouseOverTiles.mouseOverUI)
        {
            return;
        }

        TileScript clickedTile = mouseOverTiles.GetClickedTile();
        if (clickedTile != null && clickedTile.selectable)
        {
            GetPath(clickedTile);
            if(path.Count - 1 == playerScript.moveSpeed + playerScript.moveSpeedModifier && !clickedTile.occupied)
            {
                playerScript.FaceCharacter(clickedTile.transform);
                FollowPath();
            }
            else if(path.Count - 1 < playerScript.moveSpeed + playerScript.moveSpeedModifier)
            {
                pathPoint = clickedTile;
            }
        }
    }

    public void ClearPath()
    {
        path.Clear();
    }

    public void GetPath(TileScript destinationTile)
    {
        TileScript[] tempPath = path.ToArray();
        System.Array.Reverse(tempPath);
        path.Clear();

        TileScript next = destinationTile;
        while(next != null)
        {
            path.Push(next);
            next = next.previousTile;
        }
        
        for(int i = 1; i < tempPath.Length; i++)
        {
            path.Push(tempPath[i]);
        }
    }

    public void FollowPath()
    {
        currentTile.occupied = false;
        currentTile.occupation = currentTile.gameObject;
        moving = true;
        nextTile = path.Pop();
        movePoint.position = nextTile.transform.position;
    }

    public void OccupyTile(TileScript tile)
    {
        sortingGroup.sortingOrder = (int)tile.gridPosition.x;
        tile.occupied = true;
        tile.occupation = gameObject;
        healthbarCanvas.sortingOrder = sortingGroup.sortingOrder;
    }

    void SetUpControls()
    {
        im = mouseOverTiles.gameObject.GetComponent<InputManager>();

        im.controls[tm.players.IndexOf(playerScript)].PlayerTurn.LeftClick.performed += ctx => MoveToTile();
        im.controls[tm.players.IndexOf(playerScript)].PlayerTurn.RightClick.performed += ctx => SetPathingTile();
    }
}

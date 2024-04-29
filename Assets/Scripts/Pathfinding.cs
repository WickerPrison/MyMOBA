using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    LayerMask layerMask;
    TileHolder tileHolder;
    
    private void Start()
    {
        layerMask = LayerMask.GetMask("Tiles");
        tileHolder = GameObject.FindGameObjectWithTag("TileHolder").GetComponent<TileHolder>();
    }

    public void Pathfinder(TileScript startingTile, PathfindingParameters pathfindingParameters, int range, bool outline)
    {
        startingTile.tileDistance = 0;

        Queue<TileScript> searching = new Queue<TileScript>();

        searching.Enqueue(startingTile);
        startingTile.visited = true;
        startingTile.previousTile = null;

        while (searching.Count > 0)
        {
            TileScript tile = searching.Dequeue();

            tile.selectable = true;

            foreach (TileScript adjacentTile in tile.adjacencyList)
            {
                if (ShouldAddNextTile(tile, adjacentTile, pathfindingParameters, range) && CanBeMovedThrough(adjacentTile, pathfindingParameters))
                {
                    adjacentTile.previousTile = tile;
                    adjacentTile.visited = true;
                    adjacentTile.tileDistance = tile.tileDistance + adjacentTile.moveCost;
                    searching.Enqueue(adjacentTile);
                    if (ShouldBeSelectable(adjacentTile, pathfindingParameters))
                    {
                        adjacentTile.selectable = true;
                        adjacentTile.UpdateSelectionColor(pathfindingParameters.selectionColor, outline);
                    }
                }
            }
        }
    }

    public void Cone(TileScript startingTile, PathfindingParameters pathfindingParameters, int range, string direction, bool outline)
    {
        startingTile.tileDistance = 0;

        Queue<TileScript> searching = new Queue<TileScript>();

        searching.Enqueue(startingTile);
        startingTile.visited = true;
        startingTile.previousTile = null;

        while (searching.Count > 0)
        {
            TileScript tile = searching.Dequeue();

            tile.selectable = true;

            List<TileScript> coneAdjacent = new List<TileScript>();

            QueueAdjacentTile(coneAdjacent, tile, direction, 0);
            if(tile.tileDistance %2 == 0)
            {
                QueueAdjacentTile(coneAdjacent, tile, direction, 1);
                QueueAdjacentTile(coneAdjacent, tile, direction, -1);
            }


            foreach (TileScript adjacentTile in coneAdjacent)
            {
                if (ShouldAddNextTile(tile, adjacentTile, pathfindingParameters, range) && CanBeMovedThrough(adjacentTile, pathfindingParameters))
                {
                    adjacentTile.previousTile = tile;
                    adjacentTile.visited = true;
                    adjacentTile.tileDistance = tile.tileDistance + adjacentTile.moveCost;
                    searching.Enqueue(adjacentTile);
                    if (ShouldBeSelectable(adjacentTile, pathfindingParameters))
                    {
                        adjacentTile.selectable = true;
                        adjacentTile.UpdateSelectionColor(pathfindingParameters.selectionColor, outline);
                    }
                }
            }
        }
    }

    bool ShouldAddNextTile(TileScript tile, TileScript adjacentTile, PathfindingParameters pathfindingParameters, int range)
    {
        if (adjacentTile.visited)
        {
            return false;
        }

        int tileDistance;
        if (pathfindingParameters.applyMoveCost)
        {
            tileDistance = tile.tileDistance + adjacentTile.moveCost;
        }
        else
        {
            tileDistance = tile.tileDistance + 1;
        }

        if(tileDistance > range)
        {
            return false;
        }

        return true;
    }

    bool CanBeMovedThrough(TileScript tile, PathfindingParameters pathfindingParameters)
    {
        if (pathfindingParameters.canMoveThrough.Contains("All"))
        {
            return true;
        }

        if (pathfindingParameters.passThroughSameTeam && tile.occupation.CompareTag(gameObject.tag))
        {
            return true;
        }

        if (tile.occupied)
        {
            if (pathfindingParameters.canMoveThrough.Contains(tile.occupation.tag))
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
            return true;
        }
    }

    bool ShouldBeSelectable(TileScript adjacentTile, PathfindingParameters pathfindingParameters)
    {
        if (!adjacentTile.occupied)
        {
            return true;
        }

        if (pathfindingParameters.canSelectOccupations.Contains(adjacentTile.occupation.tag))
        {
            return true;
        }

        if(pathfindingParameters.passThroughSameTeam && gameObject.CompareTag(adjacentTile.occupation.tag))
        {
            return true;
        }

        return false;
    }

    public string GetDirectionOfAdjacentTile(TileScript currentTile, TileScript adjacentTile)
    {
        foreach(string direction in currentTile.directions)
        {
            TileScript checkTile = currentTile.GetAdjacentTile(direction);
            if(checkTile == adjacentTile)
            {
                return direction;
            }
        }
        return null;
    }

    void QueueAdjacentTile(List<TileScript> tiles, TileScript tile, string direction, int rotation)
    {
        string adjacentDirection = tile.GetAdjacentDirection(direction, rotation);
        //Debug.Log(adjacentDirection);
        TileScript nextTile = tile.GetAdjacentTile(adjacentDirection);
        if(nextTile != null)
        {
            tiles.Add(nextTile);
        }
    }

    public TileScript GetCurrentTile()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 10, layerMask);

        if (hit.collider != null)
        {
            TileScript tileScript = hit.transform.gameObject.GetComponent<TileScript>();
            return tileScript;
        }
        return null;
    }

    public void ResetTiles()
    {
        foreach (TileScript tile in tileHolder.tileArray)
        {
            tile.ResetTile();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public int tileType;
    public bool occupied;
    public Vector2 gridPosition;
    public List<TileScript> adjacencyList = new List<TileScript>();
    [SerializeField] List<Sprite> sprites;
    [SerializeField] List<Color> selectionColor;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer selectionSprite;
    [SerializeField] SpriteRenderer outlineHex;
    TileHolder tileHolder;
    public string[] directions = {"Right", "DownRight", "DownLeft", "Left", "UpLeft", "UpRight"};
    public int tileDistance = 0;
    public bool visited = false;
    public TileScript previousTile;
    public int moveCost = 1;
    public bool selectable = false;
    public GameObject occupation;
    public int spawnPointTeam;

    private void Start()
    {
        if(occupation == null)
        {
            occupation = gameObject;
        }
        tileHolder = GetComponentInParent<TileHolder>();
        tileHolder.tileArray[(int)gridPosition.x, (int)gridPosition.y] = this;

        if(tileType == 1 || tileType == 2)
        {
            spawnPointTeam = tileType;
        }

        StartCoroutine(BuildAdjacencyList());
    }

    public void ResetTile()
    {
        selectionSprite.color = selectionColor[0];
        outlineHex.color = selectionColor[0];
        visited = false;
        selectable = false;
        tileDistance = 0;
        previousTile = null;
        UpdateTile();
    }

    public void UpdateTile()
    {
        spriteRenderer.sprite = sprites[tileType];
    }

    public void UpdateSelectionColor(int color, bool outline)
    {
        if (outline)
        {
            outlineHex.color = selectionColor[color];
            selectionSprite.color = selectionColor[0];
        }
        else
        {
            selectionSprite.color = selectionColor[color];
            outlineHex.color = selectionColor[0];
        }
    }

    IEnumerator BuildAdjacencyList()
    {
        yield return new WaitForEndOfFrame();

        foreach(string direction in directions)
        {
            TileScript tile = GetAdjacentTile(direction);
            if(tile != null)
            {
                adjacencyList.Add(tile);
            }
        }
        
    }

    public string GetAdjacentDirection(string direction, int rotation)
    {
        int directionIndex = 0;
        for(int i = 0; i < directions.Length; i++)
        {
            if(direction == directions[i])
            {
                directionIndex = i;
                break;
            }
        }

        directionIndex += rotation;
        if(directionIndex > 5)
        {
            directionIndex -= 6;
        }
        else if(directionIndex < 0)
        {
            directionIndex += 6;
        }

        return directions[directionIndex];
    }

    public TileScript GetAdjacentTile(string direction)
    {
        TileScript tile;
        switch (direction)
        {
            case "Right":
                if(gridPosition.y >= tileHolder.columns - 1)
                {
                    break;
                }
                tile = tileHolder.tileArray[(int)gridPosition.x, (int)gridPosition.y + 1];
                return tile;
            case "Left":
                if(gridPosition.y <= 0)
                {
                    break;
                }
                tile = tileHolder.tileArray[(int)gridPosition.x, (int)gridPosition.y - 1];
                return tile;

            case "UpRight":
                if(gridPosition.x <= 0)
                {
                    return null;
                }
                if((int)gridPosition.x%2 == 0)
                {
                        tile = tileHolder.tileArray[(int)gridPosition.x - 1, (int)gridPosition.y];  
                }
                else
                {
                    if(gridPosition.y >= tileHolder.columns - 1)
                    {
                        return null;
                    }
                    else
                    {
                        tile = tileHolder.tileArray[(int)gridPosition.x - 1, (int)gridPosition.y + 1];
                    }
                }
                return tile;
            case "UpLeft":
                if (gridPosition.x <= 0)
                {
                    return null;
                }
                if ((int)gridPosition.x % 2 == 0)
                {
                    if(gridPosition.y <= 0)
                    {
                        return null;
                    }
                    else
                    {
                        tile = tileHolder.tileArray[(int)gridPosition.x - 1, (int)gridPosition.y - 1];
                    }
                }
                else
                {
                    tile = tileHolder.tileArray[(int)gridPosition.x - 1, (int)gridPosition.y];   
                }
                return tile;
            case "DownRight":
                if (gridPosition.x >= tileHolder.rows - 1)
                {
                    return null;
                }

                if ((int)gridPosition.x % 2 == 0)
                {
                    tile = tileHolder.tileArray[(int)gridPosition.x + 1, (int)gridPosition.y]; 
                }
                else
                {
                    if ((int)gridPosition.y >= tileHolder.columns - 1)
                    {
                        return null;
                    }
                    else
                    {
                        tile = tileHolder.tileArray[(int)gridPosition.x + 1, (int)gridPosition.y + 1];
                    }
                }
                return tile;
            case "DownLeft":
                if (gridPosition.x >= tileHolder.rows - 1)
                {
                    return null;
                }

                if ((int)gridPosition.x % 2 == 0)
                {
                    if ((int)gridPosition.y <= 0)
                    {
                        return null;
                    }
                    else
                    {
                        tile = tileHolder.tileArray[(int)gridPosition.x + 1, (int)gridPosition.y - 1];
                    }
                }
                else
                {
                    tile = tileHolder.tileArray[(int)gridPosition.x + 1, (int)gridPosition.y];
                }
                return tile;
        }
        return null;
    }

    private void OnDrawGizmosSelected()
    {
        UpdateTile();
    }
}

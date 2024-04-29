using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseOverTiles : MonoBehaviour
{
    LayerMask layerMask;
    TileScript selectedTile;
    TileHolder tileHolder;
    public bool mouseOverUI = false;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Tiles");
        tileHolder = GameObject.FindGameObjectWithTag("TileHolder").GetComponent<TileHolder>();
    }

    void Testing()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 10, layerMask);

        if (hit.collider != null)
        {
            TileScript tileScript = hit.transform.gameObject.GetComponent<TileScript>();
            tileScript.UpdateSelectionColor(3, true);
            TileScript otherTile = tileScript.GetAdjacentTile("DownLeft");
            if (otherTile != null)
            {
                otherTile.UpdateSelectionColor(3, true);
            }
        }
    }

    public void MouseOverUIEnter()
    {
        mouseOverUI = true;
    }

    public void MouseOverUIExit()
    {
        mouseOverUI = false;
    }

    public TileScript GetClickedTile()
    {
        if (mouseOverUI)
        {
            return null;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 10, layerMask);

        if (hit.collider != null)
        {
            TileScript tileScript = hit.transform.gameObject.GetComponent<TileScript>();
            return tileScript;
        }
        return null;
    }
}

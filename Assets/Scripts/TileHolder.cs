using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHolder : MonoBehaviour
{
    public TileScript[,] tileArray;
    public int rows;
    public int columns;

    private void Awake()
    {
        tileArray = new TileScript[rows, columns];
    }
}

using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTiles : EditorWindow
{
    int rowsNum = 10;
    int columnsNum = 10;
    //[SerializeField] List<GameObject tileTypes;
    int[,] gridArray;
    GameObject tilePrefab;
    GameObject tileHolder;
    bool useDirt = true;
    bool useGrass = true;
    bool useWater = true;
    List<bool> allTiles = new List<bool>();
    List<int> allowedTiles = new List<int>();
    TextAsset levelCSV;
    float offset = 0.8625f;

    [MenuItem("Tools/Spawn Tiles")]
    public static void ShowWindow()
    {
        GetWindow(typeof(SpawnTiles));
    }

    private void OnGUI()
    {
        tilePrefab = EditorGUILayout.ObjectField("Tile Prefab", tilePrefab, typeof(GameObject), false) as GameObject;
        levelCSV = EditorGUILayout.ObjectField("Level CSV", levelCSV, typeof(TextAsset), false) as TextAsset;
        rowsNum = EditorGUILayout.IntField("Number of Rows", rowsNum);
        columnsNum = EditorGUILayout.IntField("Number of Columns", columnsNum);

        GUILayout.Label("Allowed Tile Types", EditorStyles.boldLabel);
        useDirt = EditorGUILayout.Toggle("     Dirt Allowed", useDirt);
        useGrass = EditorGUILayout.Toggle("     Grass Allowed", useGrass);
        useWater = EditorGUILayout.Toggle("     Water Allowed", useWater);

        if (GUILayout.Button("Spawn Tiles"))
        {
            ChooseTileTypes();
            if (levelCSV == null)
            {
                GenerateGrid();
            }
            else
            {
                ReadCSV();
            }
            SpawnTheTiles();
        }
    }

    void GenerateGrid()
    {
        gridArray = new int[columnsNum, rowsNum];

        for (int column = 0; column < columnsNum; column++)
        {
            for (int row = 0; row < rowsNum; row++)
            {
                int index = Random.Range(0, allowedTiles.Count);
                gridArray[column, row] = allowedTiles[index];
            }
        }
    }

    void ReadCSV()
    {
        string[] levelLayout = levelCSV.text.Split('\n');
        rowsNum = levelLayout.Length - 1;
        columnsNum = levelLayout[0].Split('|').Length;
        gridArray = new int[rowsNum, columnsNum];
        for (int row = 0; row < rowsNum; row++)
        {
            string[] line = levelLayout[row].Split('|');
            for (int column = 0; column < line.Length; column++)
            {
                gridArray[row, column] = int.Parse(line[column]);
            }
        }
    }

    void ChooseTileTypes()
    {
        allTiles.Clear();
        allowedTiles.Clear();
        allTiles.Add(useDirt);
        allTiles.Add(useGrass);
        allTiles.Add(useWater);

        for (int n = 0; n < allTiles.Count; n++)
        {
            if (allTiles[n])
            {
                allowedTiles.Add(n);
            }
        }
    }

    void SpawnTheTiles()
    {
        GameObject[] tileHolders = GameObject.FindGameObjectsWithTag("TileHolder");
        foreach (GameObject holder in tileHolders)
        {
            DestroyImmediate(holder);
        }

        tileHolder = new GameObject();
        tileHolder.name = "Tile Holder";
        tileHolder.tag = "TileHolder";
        TileHolder tileHolderScript = tileHolder.AddComponent<TileHolder>();
        tileHolderScript.rows = gridArray.GetLength(0);
        tileHolderScript.columns = gridArray.GetLength(1);

        foreach (Transform child in tileHolder.transform)
        {
            DestroyImmediate(child.gameObject);
        }

        for (int row = 0; row < gridArray.GetLength(0); row++)
        {
            for (int column = 0; column < gridArray.GetLength(1); column++)
            {
                TileScript tile = Instantiate(tilePrefab).GetComponent<TileScript>();
                tile.tileType = gridArray[row, column];
                tile.gridPosition = new Vector2(row, column);
                tile.UpdateTile();
                tile.transform.position = new Vector3(column - Mathf.Floor(gridArray.GetLength(1) / 2), rowsNum - 1 - row * offset - Mathf.Floor(gridArray.GetLength(0) * offset / 2), 2);
                tile.transform.parent = tileHolder.transform;
                if(row%2 != 0)
                {
                    tile.transform.position += new Vector3(0.5f, 0, 0);
                }
            }
        }
    }
}

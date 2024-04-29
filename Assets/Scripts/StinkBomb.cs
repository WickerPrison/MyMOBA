using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StinkBomb : MonoBehaviour
{
    public TileScript target;
    public string myTeam;
    float speed = 8;
    List<TileScript> affectedTiles = new List<TileScript>();

    private void Update()
    {
        transform.LookAt(target.transform.position);
        Vector3 direction = target.transform.position - transform.position;
        transform.Translate(direction.normalized * Time.deltaTime * speed);

        if(Vector3.Distance(transform.position, target.transform.position) < 0.1)
        {
            affectedTiles.Add(target);
            foreach(TileScript tile in target.adjacencyList)
            {
                affectedTiles.Add(tile);
            }

            foreach(TileScript tile in affectedTiles)
            {
                PlayerScript enemy = tile.occupation.GetComponent<PlayerScript>();
                if(enemy != null && !enemy.CompareTag(myTeam) && enemy.silenced <= 2)
                {
                    enemy.silenced = 2;
                }

            }

            Destroy(gameObject);
        }
    }
}

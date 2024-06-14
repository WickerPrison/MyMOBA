using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Grenade : MonoBehaviour
{
    [SerializeField] Transform movePoint;
    [SerializeField] GameObject explosion;
    public TileScript targetTile;
    public Vector3 startingPosition;
    float throwDistance;
    float distance;
    Vector2 direction;
    float midpoint;
    float arcWidth;
    float arcHeight = 1;
    float throwSpeed = 3.5f;
    bool atDestination = false;
    [System.NonSerialized] public int grenadeDamage;


    private void Start()
    {
        movePoint.parent = null;
        transform.position = startingPosition;
        throwDistance = Vector2.Distance(transform.position, targetTile.transform.position);
        direction = targetTile.transform.position - transform.position;
        movePoint.position = transform.position;
        midpoint = throwDistance / 2;
        arcWidth = arcHeight / Mathf.Pow(midpoint, 2);
    }

    private void Update()
    {
        if (!atDestination)
        {
            movePoint.Translate(direction.normalized * Time.deltaTime * throwSpeed);
            distance = Vector2.Distance(movePoint.position,targetTile.transform.position);
            float currentHeight = -arcWidth * Mathf.Pow(distance - midpoint, 2) + arcHeight;
            transform.position = movePoint.position + new Vector3(0, currentHeight, 0);
            float distanceDecimal = distance / throwDistance;
            if (distance <= 0.1f)
            {
                transform.position = targetTile.transform.position;
                atDestination = true;
                targetTile.threats += 1;
                foreach(TileScript tile in targetTile.adjacencyList)
                {
                    tile.threats += 1;
                }
            }
        }
    }

    public void Explode()
    {
        targetTile.threats -= 1;

        PlayerScript targetScript = targetTile.occupation.GetComponent<PlayerScript>();
        if (targetScript != null)
        {
            targetScript.TakeDamage(grenadeDamage);
        }

        foreach(TileScript tile in targetTile.adjacencyList)
        {
            tile.threats -= 1;
            targetScript = tile.occupation.GetComponent<PlayerScript>();
            if (targetScript != null)
            {
                targetScript.TakeDamage(grenadeDamage);
            }
        }

        Instantiate(explosion).transform.position = transform.position;
        Destroy(gameObject);
    }
}

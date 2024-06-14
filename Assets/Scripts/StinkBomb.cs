 using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class StinkBomb : MonoBehaviour
{
    public TileScript target;
    public string myTeam;
    float speed = 4;
    List<TileScript> affectedTiles = new List<TileScript>();
    SpriteRenderer sprite;
    ParticleSystem particles;
    bool exploding = false;
    [System.NonSerialized] public int silenceDuration;

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (exploding) return;
        Vector3 direction = target.transform.position - transform.position;
        transform.right = direction.normalized;
        transform.position += direction.normalized * Time.deltaTime * speed;

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
                if(enemy != null && !enemy.CompareTag(myTeam) && enemy.Silenced <= 2)
                {
                    enemy.Silenced = silenceDuration;
                }

            }
            sprite.enabled = false;
            exploding = true;
            StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
        particles.Play();
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}

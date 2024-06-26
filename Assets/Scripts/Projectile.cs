using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [System.NonSerialized] public TileScript target;
    [System.NonSerialized] public string myTeam;
    [System.NonSerialized] public CharacterAbilities abilities;
    float speed = 8;

    private void Update()
    {
        transform.LookAt(target.transform.position);
        Vector3 direction = target.transform.position - transform.position;
        transform.Translate(direction.normalized * Time.deltaTime * speed);

        if (Vector3.Distance(transform.position, target.transform.position) < 0.1)
        {
            abilities.ProjectileHit(target);

            Destroy(gameObject);
        }
    }
}

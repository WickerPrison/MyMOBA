using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    float throwSpeed = 5;
    float rotateSpeed = 400;
    public Transform target;
    public Transform returnPosition;
    public SokkaAbilities abilities;
    SpriteRenderer sprite;
    bool hitTarget = false;
    Vector2 direction;

    private void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hitTarget)
        {
            if(Vector2.Distance(target.position,transform.position) < 0.1f)
            {
                abilities.BoomerangHit();
                hitTarget = true;
            }
            direction = target.position - transform.position;   
        }
        else
        {
            direction = returnPosition.position - transform.position;
            if(Vector2.Distance(returnPosition.position, transform.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }

        transform.Translate(direction.normalized * throwSpeed * Time.deltaTime);
        sprite.transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
    }
}

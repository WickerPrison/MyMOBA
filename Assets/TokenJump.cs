using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenJump : MonoBehaviour
{
    PlayerMovement playerMovement;
    Vector2 destination;
    float totalDistance;
    float currentDistance;
    Vector2 direction;
    bool jump = false;
    float speed;
    Action arrivalFunction;
    Pathfinding pathfinding;
    Transform player;
    float initialSize;
    float maxSize;

    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        pathfinding = GetComponentInParent<Pathfinding>();
        player = playerMovement.transform;

        initialSize = transform.localScale.x;
        maxSize = transform.localScale.x * 1.5f;
    }

    private void Update()
    {
        if (jump)
        {
            player.Translate(direction * Time.deltaTime * speed);
            currentDistance = Vector2.Distance(player.position, destination);

            float interpolator = Mathf.Abs(currentDistance - totalDistance / 2) * 2 / totalDistance;
            float size = Mathf.Lerp(maxSize, initialSize, interpolator);
            transform.localScale = new Vector3(size, size, 1);

            if(currentDistance <= speed * Time.deltaTime)
            {
                player.position = destination;
                jump = false;
                if(arrivalFunction != null)
                {
                    JumpLanding();
                }
            }
        }
    }

    public void Jump(Vector2 startPos, Vector2 endPos, Action callback = null
    , float jumpSpeed = 4)
    {
        speed = jumpSpeed;
        destination = endPos;
        totalDistance = Vector2.Distance(startPos, endPos);
        direction = (endPos - startPos).normalized;
        arrivalFunction = callback;
        jump = true;
    }

    void JumpLanding()
    {
        TileScript currentTile = pathfinding.GetCurrentTile();
        playerMovement.OccupyTile(currentTile);
        currentTile.occupied = true;
        currentTile.occupation = player.gameObject;
        arrivalFunction();
    }
}

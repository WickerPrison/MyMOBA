using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenAnimations : MonoBehaviour
{
    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();

    public void MeleeAttack(Vector2 targetPos, Action finalFunction)
    {
        StartCoroutine(MeleeAttackRoutine(targetPos, finalFunction));
    }

    IEnumerator MeleeAttackRoutine(Vector2 targetPos, Action finalFunction)
    {
        Vector2 initialPos = transform.position;
        Vector2 attackDirection = (targetPos - initialPos).normalized;
        float time = 0.2f;
        float timer = time; 
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            transform.position = Vector2.Lerp(initialPos - attackDirection / 2, initialPos, timer / time);
            yield return endOfFrame;
        }

        time = 0.15f;
        timer = time;
        Vector2 startPos = transform.position;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            transform.position = Vector2.Lerp(targetPos, startPos, timer / time);
            yield return endOfFrame;
        }

        finalFunction();

        time = 0.2f;
        timer = time;
        startPos = transform.position;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            transform.position = Vector2.Lerp(initialPos, startPos, timer / time);
            yield return endOfFrame;
        }
        transform.localPosition = Vector2.zero;
    }
}
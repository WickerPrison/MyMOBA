using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

public class TokenAnimations : MonoBehaviour
{
    MuzzleFlash muzzleFlash;
    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();

    private void Start()
    {
        muzzleFlash = GetComponentInChildren<MuzzleFlash>();
    }

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


    public void Recoil(Vector2 targetPos, Action finalFunction = null, Action[] repeatActions = null)
    {
        if(finalFunction == null)
        {
            finalFunction = () => { };
        }

        StartCoroutine(RecoilRoutine(targetPos, finalFunction, repeatActions));   
    }

    IEnumerator RecoilRoutine(Vector2 targetPos, Action finalFunction, Action[] repeatActions = null)
    {

        if (muzzleFlash != null)
        {
            muzzleFlash.Flash(targetPos);
        }

        Vector2 initialPos = transform.position;
        Vector2 attackDirection = (targetPos - initialPos).normalized;
        float time = 0.05f;
        float timer = time;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            transform.position = Vector2.Lerp(initialPos - attackDirection / 3, initialPos, timer / time);
            yield return endOfFrame;
        }

        finalFunction();

        time = 0.15f;
        timer = time;
        Vector2 startPos = transform.position;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            transform.position = Vector2.Lerp(initialPos, startPos, timer / time);
            yield return endOfFrame;
        }
        transform.localPosition = Vector2.zero;

        if(repeatActions != null && repeatActions.Length > 0)
        {
            Action action = repeatActions[0];
            repeatActions = repeatActions.Skip(1).ToArray();
            StartCoroutine(RecoilRoutine(targetPos, action, repeatActions));
        }
    }
}

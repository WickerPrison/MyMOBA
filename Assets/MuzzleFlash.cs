using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    [SerializeField] SpriteRenderer flash;
    [SerializeField] Gradient gradient;
    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();

    public void Flash(Vector2 targetPos)
    {
        Vector2 direction = targetPos - (Vector2)transform.position;
        transform.up = direction.normalized;
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        float time = 0.2f;
        float timer = time;

        while(timer > 0)
        {
            timer -= Time.deltaTime;

            flash.color = gradient.Evaluate(1 - timer / time);
            yield return endOfFrame;
        }
    }
}

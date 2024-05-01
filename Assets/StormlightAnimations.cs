using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormlightAnimations : MonoBehaviour
{
    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StormlightFlare()
    {
        StartCoroutine(StormlightFlareRoutine());
    }

    IEnumerator StormlightFlareRoutine()
    {
        float time = 0.2f;
        float timer = time;
        float amplitude;
        while ( timer > 0)
        {
            timer -= Time.deltaTime;
            amplitude = Mathf.Lerp(0.1f, 1, timer / time);
            spriteRenderer.material.SetFloat("_Amplitude", amplitude);
            yield return endOfFrame;
        }

        timer = time;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            amplitude = Mathf.Lerp(1, 0.1f, timer / time);
            spriteRenderer.material.SetFloat("_Amplitude", amplitude);
            yield return endOfFrame;
        }
    }

    public void StartStormlight()
    {
        StartCoroutine(StartStormlightRoutine());
    }

    IEnumerator StartStormlightRoutine()
    {
        float time = 0.2f;
        float timer = time;
        float amplitude;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            amplitude = Mathf.Lerp(0.1f, 1, timer / time);
            spriteRenderer.material.SetFloat("_Amplitude", amplitude);
            yield return endOfFrame;
        }
    }

    public void EndStormlight()
    {
        StartCoroutine(EndStormlightRoutine());
    }

    IEnumerator EndStormlightRoutine()
    {
        float time = 0.2f;
        float timer = time;
        float amplitude;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            amplitude = Mathf.Lerp(1,0.1f, timer / time);
            spriteRenderer.material.SetFloat("_Amplitude", amplitude);
            yield return endOfFrame;
        }
    }
}

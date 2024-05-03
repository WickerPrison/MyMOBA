using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormlightAnimations : MonoBehaviour
{
    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    SpriteRenderer spriteRenderer;
    WaitForSeconds flareTime = new WaitForSeconds(0.5f);
    ParticleSystem pulse;
    ParticleSystemRenderer pulseRender;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        pulse = GetComponent<ParticleSystem>();
        pulseRender = GetComponent<ParticleSystemRenderer>();
        pulseRender.material.SetFloat("_Amplitude", 0.1f);
    }

    public void StormlightFlare()
    {
        StartCoroutine(StormlightFlareRoutine());
    }

    IEnumerator StormlightFlareRoutine()
    {
        float time = 0.25f;
        float timer = time;
        float amplitude;
        while ( timer > 0)
        {
            timer -= Time.deltaTime;
            amplitude = Mathf.Lerp(0.1f, 1, timer / time);
            spriteRenderer.material.SetFloat("_Amplitude", amplitude);
            yield return endOfFrame;
        }

        yield return flareTime;

        timer = time;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            amplitude = Mathf.Lerp(1, 0.1f, timer / time);
            spriteRenderer.material.SetFloat("_Amplitude", amplitude);
            yield return endOfFrame;
        }
    }

    public void StartStormlight(Action callbackFunction = null)
    {
        if (spriteRenderer.material.GetFloat("_Amplitude") <= 0.2)
        {
            callbackFunction();
            return;
        }

        StartCoroutine(StartStormlightRoutine(callbackFunction));
    }

    IEnumerator StartStormlightRoutine(Action callbackFunction)
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

        if(callbackFunction != null)
        {
            callbackFunction();
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

    public void StormlightPulse()
    { 
        pulse.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralEffects : MonoBehaviour
{
    [SerializeField] Color buff;
    [SerializeField] Color deBuff;
    [SerializeField] SpriteRenderer armorSprite;
    [SerializeField] SpriteRenderer turnMeterSprite;
    [SerializeField] SpriteRenderer buffSprite;
    [SerializeField] SpriteRenderer healSprite;
    CharacterEvents characterEvents;
    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();

    private void Awake()
    {
        characterEvents = GetComponentInParent<CharacterEvents>();
    }

    private void OnGainArmor(object sender, System.EventArgs e)
    {
        armorSprite.color = buff;
        StartCoroutine(BuffAnimation(armorSprite));
    }

    private void OnLoseArmor(object sender, System.EventArgs e)
    {
        armorSprite.color = deBuff;
        StartCoroutine(BuffAnimation(armorSprite));
    }

    private void OnLoseTurnMeter(object sender, System.EventArgs e)
    {
        turnMeterSprite.color = deBuff;
        StartCoroutine(BuffAnimation(turnMeterSprite));
    }

    private void OnBuff(object sender, System.EventArgs e)
    {
        StartCoroutine(BuffAnimation(buffSprite));
    }

    private void OnHeal(object sender, System.EventArgs e)
    {
        StartCoroutine(BuffAnimation(healSprite));
    }

    public IEnumerator BuffAnimation(SpriteRenderer sprite)
    {
        float fadeIn = 0.2f;
        float stay = 0.2f;
        float fadeOut = 0.2f;
        float timer = fadeIn;
        Color color = sprite.color;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, timer / fadeIn);
            sprite.color = color;
            yield return endOfFrame;
        }

        yield return new WaitForSeconds(stay);

        timer = fadeOut;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, timer / fadeIn);
            sprite.color = color;
            yield return endOfFrame;
        }
    }

    private void OnEnable()
    {
        characterEvents.OnGainArmor += OnGainArmor;
        characterEvents.OnLoseArmor += OnLoseArmor;
        characterEvents.OnLoseTurnMeter += OnLoseTurnMeter;
        characterEvents.OnBuff += OnBuff;
        characterEvents.OnHeal += OnHeal;
    }


    private void OnDisable()
    {
        characterEvents.OnGainArmor -= OnGainArmor;
        characterEvents.OnLoseArmor -= OnLoseArmor;
        characterEvents.OnLoseTurnMeter -= OnLoseTurnMeter;
        characterEvents.OnBuff -= OnBuff;
        characterEvents.OnHeal -= OnHeal;
    }
}

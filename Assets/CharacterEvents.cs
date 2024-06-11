using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEvents : MonoBehaviour
{
    public event EventHandler OnTakeDamage;
    public event EventHandler OnGainArmor;
    public event EventHandler OnLoseArmor;
    public event EventHandler OnLoseTurnMeter;
    public event EventHandler OnGainTurnMeter;
    public event EventHandler OnBuff;
    public event EventHandler OnDebuff;
    public event EventHandler OnHeal;
    public event EventHandler OnSilenced;

    public event Action<CharacterEvents, int> OnCustomEffect;

    public void TakeDamage()
    {
        OnTakeDamage?.Invoke(this, EventArgs.Empty);
    }

    public void GainArmor()
    {
        OnGainArmor?.Invoke(this, EventArgs.Empty);
    }

    public void LoseArmor()
    {
        OnLoseArmor?.Invoke(this, EventArgs.Empty);
    }

    public void LoseTurnMeter()
    {
        OnLoseTurnMeter?.Invoke(this, EventArgs.Empty);
    }

    public void GainTurnMeter()
    {
        OnGainTurnMeter?.Invoke(this, EventArgs.Empty);
    }

    public void Buff()
    {
        OnBuff?.Invoke(this, EventArgs.Empty);
    }

    public void Debuff()
    {
        OnDebuff?.Invoke(this, EventArgs.Empty);
    }

    public void Heal()
    {
        OnHeal?.Invoke(this, EventArgs.Empty);
    }

    public void Silenced()
    {
        OnSilenced?.Invoke(this, EventArgs.Empty);
    }

    public void CustomEffect(int effectIndex)
    {
        OnCustomEffect?.Invoke(this, effectIndex);
    }

    //private void OnEnable()
    //{
    //    playerEvents.onPlayerStagger += PlayerEvents_onPlayerStagger;
    //    playerEvents.onEndPlayerStagger += onEndPlayerStagger;
    //}
}

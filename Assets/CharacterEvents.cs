using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEvents : MonoBehaviour
{
    public event EventHandler OnTakeDamage;
    public event EventHandler OnGainArmor;
    public event EventHandler OnLoseArmor;

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

    //private void OnEnable()
    //{
    //    playerEvents.onPlayerStagger += PlayerEvents_onPlayerStagger;
    //    playerEvents.onEndPlayerStagger += onEndPlayerStagger;
    //}
}

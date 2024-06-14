using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityData
{
    //required values
    public string name;
    public int cooldown;
    public int APcost;
    public bool silenceable;

    // optional values
    public int range;
    public int moveEffectRange;
    public int damage;
    public int duration;
    public int gainTurnMeter;
    public string description;
    public string[] variables;
    public string[] additions;
}

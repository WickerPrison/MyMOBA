using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDamageCalc
{
    int DamageCalc();
}


interface IMoveCalc
{
    int MoveCalc();
}

interface IValueCalc
{
    int ValueCalc();
}
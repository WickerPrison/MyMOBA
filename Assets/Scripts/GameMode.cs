using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameMode : MonoBehaviour
{
    [System.NonSerialized] public TurnManager tm;
    public float xCameraBoundary;
    public float yCameraBoundaryPos;
    public float yCameraBoundaryNeg;
    public float zoomInCameraBoundary;
    public float zoomOutCameraBoundary;

    public virtual void Start()
    {
        tm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TurnManager>();
    }

    public virtual void EndOfRound()
    {

    }

    public virtual void GameModeTurnMeter()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PathfindingParameters : ScriptableObject
{
    public List<string> canMoveThrough;
    public List<string> canSelectOccupations;
    public bool applyMoveCost;
    public bool passThroughSameTeam;
    public int selectionColor;
}

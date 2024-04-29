using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KingOfTheHill : GameMode
{
    [SerializeField] RectTransform turnMeterFill;
    [SerializeField] TextMeshProUGUI team1ScoreText;
    [SerializeField] TextMeshProUGUI team2ScoreText;
    int team1OnPoint;
    int team2OnPoint;
    int team1Score;
    int team2Score;
    int roundTurnMeter = 0;
    int roundTurnMeterSpeed = 100;
    float turnMeterInitialScale;
    UltimateChargeManager chargeManager;

    public override void Start()
    {
        base.Start();
        chargeManager = GetComponent<UltimateChargeManager>();
        turnMeterInitialScale = turnMeterFill.localScale.x;
        UpdateUI();
    }

    public override void GameModeTurnMeter()
    {
        base.GameModeTurnMeter();
        roundTurnMeter += roundTurnMeterSpeed;
        if (roundTurnMeter >= 1000)
        {
            roundTurnMeter = 0;
            EndOfRound();
        }
        UpdateUI();
    }

    public override void EndOfRound()
    {
        team1OnPoint = 0;
        team2OnPoint = 0;
        foreach(PlayerScript player in tm.players)
        {
            Pathfinding pathfinding = player.gameObject.GetComponent<Pathfinding>();
            TileScript currentTile = pathfinding.GetCurrentTile();
            if(currentTile != null && currentTile.tileType == 3)
            {
                if (player.gameObject.CompareTag("Team1"))
                {
                    team1OnPoint++;
                }
                else if (player.gameObject.CompareTag("Team2"))
                {
                    team2OnPoint++;
                }
            }
        }

        if(team1OnPoint > team2OnPoint)
        {
            team1Score++;
        }
        else if(team2OnPoint > team1OnPoint)
        {
            team2Score++;
        }

        foreach(UltimateCharger charger in chargeManager.chargers)
        {
            charger.ChargeOn();
        }
    }

    void UpdateUI()
    {
        float turnMeterRatio = (float)roundTurnMeter / 1000f;
        if (turnMeterRatio > 1)
        {
            turnMeterRatio = 1;
        }
        turnMeterFill.localScale = new Vector3(turnMeterInitialScale * turnMeterRatio, turnMeterFill.localScale.y, turnMeterFill.localScale.z);
        team1ScoreText.text = team1Score.ToString();
        team2ScoreText.text = team2Score.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnManager : MonoBehaviour
{
    public List<PlayerScript> players = new List<PlayerScript>();
    public PlayerScript currentPlayer;
    int currentPlayerID = 0;
    InputManager im;
    UIManager uim;
    GameMode gameMode;

    private void Awake()
    {
        im = GetComponent<InputManager>();
        uim = im.gameObject.GetComponent<UIManager>();
        players.Sort(SortByTacticalSpeed);
        players.Reverse();
        currentPlayer = players[currentPlayerID];
        im.DisableAll();
        im.controls[currentPlayerID].Enable();
    }

    private void Start()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        uim.SetupUIforTurn(currentPlayer);
        currentPlayer.StartTurn();

        while (currentPlayer.turnMeter < 1000)
        {
            foreach (PlayerScript player in players)
            {
                player.IncrementTurnMeter();
            }
        }
    }

    /*
    public void NextTurn()
    {
        currentPlayer.EndTurn();
        if(currentPlayerID < players.Count - 1)
        {
            currentPlayerID++;
        }
        else
        {
            gameMode.EndOfRound();
            StartCoroutine(WaitTillEndOfFrame());
            currentPlayerID = 0;
        }
        currentPlayer = players[currentPlayerID];
        players[currentPlayerID].StartTurn();
        im.DisableAll();
        im.controls[currentPlayerID].Enable();
    }
    */
    public void NextTurn()
    {
        currentPlayer.EndTurn();

        PlayerScript highestTurnMeter = GetHighestTurnMeter();
        while(highestTurnMeter.turnMeter < 1000)
        {
            gameMode.GameModeTurnMeter();
            foreach(PlayerScript player in players)
            {
                player.IncrementTurnMeter();
            }
            highestTurnMeter = GetHighestTurnMeter();
        }
        currentPlayer = highestTurnMeter;
        currentPlayer.StartTurn();
        currentPlayerID = players.IndexOf(currentPlayer);
        im.DisableAll();
        im.controls[currentPlayerID].Enable();
    }

    PlayerScript GetHighestTurnMeter()
    {
        PlayerScript highestTurnMeter = players[0];
        foreach(PlayerScript player in players)
        {
            if(player.turnMeter > highestTurnMeter.turnMeter)
            {
                highestTurnMeter = player;
            }
        }

        return highestTurnMeter;
    }

    public void EndPlayerTurn()
    {
        currentPlayer.NextTurn();
    }


    IEnumerator WaitTillEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
    }

    int SortByTacticalSpeed(PlayerScript p1, PlayerScript p2)
    {
        return p1.tacticalSpeed.CompareTo(p2.tacticalSpeed);
    }
}

﻿using UnityEngine;
using System.IO;
using OsuTournamentBot;

public class GameManager : MonoBehaviour
{
    public GameStates currentState;
    IrcClient mp1;
	void Start()
    {
        currentState = GameStates.Setup;

        string ircUserName = "[_Yui_]";
        string ircAuthKey = "52af51d2";

        mp1 = new IrcClient("irc.ppy.sh", 6667, ircUserName, ircAuthKey);
        mp1.joinRoom("osu");
    }
	
	void Update()
    {
        if(!mp1.tcpClient.Connected)
        {
            mp1.connect();
        }

        string message = mp1.readMessage();

        switch (currentState)
        {
            case GameStates.Setup:
                break;

            case GameStates.Inivtes:
                break;

            case GameStates.PlayerConfirmation:
                break;

            case GameStates.Greeting:
                break;

            case GameStates.Rolling:
                break;

            case GameStates.WarmUps:
                break;

            case GameStates.Banning:
                break;

            case GameStates.Picking:
                break;

            case GameStates.PlayerSwapping:
                break;

            case GameStates.StartMap:
                break;

            case GameStates.MapRunning:
                break;

            case GameStates.MapEnding:
                break;

            case GameStates.MapAnalysis:
                break;

            case GameStates.MatchEnding:
                break;

            case GameStates.MatchConclusion:
                break;
        }
	}
}
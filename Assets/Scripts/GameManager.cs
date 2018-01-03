using System.Collections.Generic;
using UnityEngine;
using System.IO;
using GSheets;
using OsuTournamentBot;

public class GameManager : MonoBehaviour
{
    public GameStates currentState;
    IrcClient mp1;
    readonly Spreadsheet sh = new Spreadsheet("https://docs.google.com/spreadsheets/d/1-8Ibp7Qnk6jKYGI4Voy2bWDBbegZQFgFmCfh0U5F30Q/edit#gid=0");
    readonly List<string> x = new List<string> {"Value changed", "To something else!"};
    private bool _xD = false;
    private bool runonce = true;
	void Start()
    {
        StartCoroutine(this.sh.GetValue("A1", "B1"));
        //StartCoroutine(sh.WriteValue("A1", "B1", x));
        this.sh.GetOAuth2Key();

        // StartCoroutine(sh.GetValue("a1", "b1"));
        
        /* currentState = GameStates.Setup;
 
         string ircUserName = "[_Yui_]";
         string ircAuthKey = "";
 
         mp1 = new IrcClient("irc.ppy.sh", 6667, ircUserName, ircAuthKey);
         mp1.joinRoom("osu");
         
         */
    }
	
	void Update()
    {
        if (ApiKeys.OAuth2Key != string.Empty)
        {
            this._xD = true;
        }
        
        if (this._xD)
        {
            if (this.runonce == true)
            {
                this.runonce = false;
                StartCoroutine(this.sh.WriteValue("A1", "B1", this.x));
            }
        }

        /*
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
        */
	}
}

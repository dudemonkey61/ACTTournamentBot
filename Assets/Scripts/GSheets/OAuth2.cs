using System;
using System.Security.Cryptography;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using NHttp;
using UnityEngine;
using UnityEngine.Networking;

namespace GSheets
{
    internal sealed class OAuth2
    {
        private string _state;
        private string _codeverifier;
        private readonly HttpServer _server = new HttpServer();
        private readonly RandomNumberGenerator _rngc = new RNGCryptoServiceProvider();

        /// <summary>
        /// Starts verification for an OAuth2 Key. Will open browser.
        /// </summary>
        internal void StartOauthVerify()
        {
            // _state to prevent CSRF.
            byte[] randBytes = new byte[32];
            this._rngc.GetBytes(randBytes);
            this._state = Convert.ToBase64String(randBytes);
            
            Application.OpenURL("https://accounts.google.com/o/oauth2/v2/auth?client_id=" + ApiKeys.OAuth2ClientId +
                                "&redirect_uri=http://localhost:4406" +
                                "&scope=https://www.googleapis.com/auth/drive " +
                                "https://www.googleapis.com/auth/drive.file " +
                                "https://www.googleapis.com/auth/drive.readonly " +
                                "https://www.googleapis.com/auth/drive.readonly " +
                                "https://www.googleapis.com/auth/spreadsheets.readonly" +
                                "&access_type=online" +
                                "&prompt=consent" +
                                "&state=" + this._state +
                                "&response_type=code");
            StartServer();
        }
        
        /// <summary>
        /// Starts the server for OAuth2 Verification.
        /// </summary>
        private void StartServer()
        {
            this._server.RequestReceived += (s, e) =>
                                            {
                                                // Deal with the OAuth Key.
                                                if (e.Request.Params.Get("state") != null)
                                                {

                                                    string state = e.Request.Params.Get("state");
                                                    string oacode = e.Request.Params.Get("code");
                                                    string error = e.Request.Params.Get("error");

                                                    this._state = state;

                                                    // If it returns an error, throw it as an exception.
                                                    if (error != null)
                                                    {
                                                        throw new Exception(error);
                                                    }

                                                    // If it doesn't, proceed.
                                                    // Verify if the state we sent is correct.
                                                    if (this._state != state)
                                                    {
                                                        Debug.Log("state failed");
                                                        throw new Exception(
                                                            "Invalid state recieved. Please try again.");
                                                    }

                                                    // If it does, extract the OAuth2 code, then request a token!
                                                    ApiKeys.OAuth2Key = oacode;
                                                    RequestToken();
                                                    
                                                }
                                                
                                                // Deal with the OAuth Token.
                                                else if (e.Request.Params.Get("access_token") != null)
                                                {
                                                    string accesstoken = e.Request.Params.Get("access_token");
                                                    string expiry = e.Request.Params.Get("expires_in");
                                                    string refresh = e.Request.Params.Get("refresh_token");

                                                    Debug.Log(accesstoken);
                                                    Debug.Log(expiry);
                                                    Debug.Log(refresh);
                                                    ApiKeys.OAuth2AccessToken = accesstoken;
                                                    ApiKeys.OAuth2TokenExpireTime = Convert.ToInt32(expiry);
                                                    ApiKeys.OAuth2ExpiryToken = refresh;

                                                    // At this point, the server is of no use anymore. Go Garbage Collection!
                                                    StopServer();

                                                }
                                            };
            this._server.EndPoint = new IPEndPoint(IPAddress.Loopback, 4406);
            this._server.Start();
        }

        private void StopServer()
        {
            if (this._server.State == HttpServerState.Started)
            {    
                this._server.Stop();
            }
        }

        private void RequestToken()
        {
            // Todo.
            // Testing on Postman returns errors, so i can't really test if this works.
            var form = new WWWForm();
            form.AddField("code", ApiKeys.OAuth2Key);
            form.AddField("client_id", ApiKeys.OAuth2ClientId);
            form.AddField("client_secret", ApiKeys.OAuth2ClientSecret);
            form.AddField("redirect_uri", "localhost:4406");
            form.AddField("grant_type", "authorization_code");
            byte[] randBytes = new byte[32];
            this._rngc.GetBytes(randBytes);
            this._codeverifier = Convert.ToBase64String(randBytes);
            form.AddField("code_verifier", this._codeverifier);
            var req = UnityWebRequest.Post(
                "https://www.googleapis.com/oauth2/v4/token", form);

            req.SendWebRequest();
        }
    }
}
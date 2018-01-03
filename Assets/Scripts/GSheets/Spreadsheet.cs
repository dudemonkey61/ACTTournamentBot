using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace GSheets {
	internal sealed class Spreadsheet {

		private readonly string _sheetId;
	
		/// <summary>
		/// Creates a new <see cref="Spreadsheet"/>.
		/// </summary>
		/// <param name="sheetLink"></param>
		/// <exception cref="ArgumentException">Invalid Link Provided.</exception>
		public Spreadsheet(string sheetLink)
		{
			var r = new Regex(@"(http|https)://docs.google.com/spreadsheets/d/(?<spsheetid>.*)/(edit#gid=(?<sheetid>.*))");
			var m = r.Match(sheetLink);
			if (r.IsMatch(sheetLink))
			{
				this._sheetId = m.Groups["spsheetid"].Value;
			}
			else
			{
				throw new ArgumentException("Link is not valid.");
			}
		}

		/// <summary>
		/// Gets multiple values from the <see cref="Spreadsheet"/>
		/// </summary>
		/// <param name="sheetname">Name of the sheet. Defaults to default/first sheet if not specified.</param>
		/// <param name="range1">First range. Must be A1 Notation.</param>
		/// <param name="range2">Second range. Must be A1 Notation.</param>
		/// <returns>Returns a List&lt;String&gt; of all returned values.</returns>
		public IEnumerator GetValue(string range1, string range2, string sheetname = "unassigned")
		{
			string apirequest = string.Concat("https://sheets.googleapis.com/v4/spreadsheets/", this._sheetId, "/values/",
				ConvertToRange(range1, range2, sheetname), "?key=", ApiKeys.GoogleApiKey);
			var net = new WWW(apirequest);
			yield return net;

			var resp2 = JsonConvert.DeserializeObject<SpreadsheetValuesResponse>(net.text);

			List<List<string>> x = resp2.Values;
		
			yield return x;
		}
	
		/// <summary>
		/// Write multiple values to the <see cref="Spreadsheet"/>
		/// </summary>
		/// <param name="range1">First range. Must be A1 Notation.</param>
		/// <param name="range2">Second range. Must be A1 Notation.</param>
		/// <param name="values">Values to write. Must not exceed specified range.</param>
		/// <param name="sheetname">Name of the sheet. Defaults to default/first sheet if not specified.</param>
		/// <returns></returns>
		public IEnumerator WriteValue(string range1, string range2, List<string> values, string sheetname = "unassigned")
		{
			if (ApiKeys.OAuth2Key == string.Empty){
				throw new Exception("No OAuth2 Key found. Generate using Spreadsheet.GetOAuth2Key(). Will open in browser.");
			}
			string apirequest = string.Concat("https://sheets.googleapis.com/v4/spreadsheets/", this._sheetId, "/values/",
				ConvertToRange(range1, range2, sheetname), "?key=", ApiKeys.GoogleApiKey, "&valueInputOption=USER_ENTERED&includeValuesInResponse=false");
			var response = new SpreadsheetValuesResponse
			{
				MajorDimension = "ROWS",
				Range = ConvertToRange(range1, range2, sheetname),
				Values = new List<List<string>> {values}
			};
			
			var www = UnityWebRequest.Put(apirequest,JsonConvert.SerializeObject(response));
			www.SetRequestHeader("Authorization", "Bearer "+ ApiKeys.OAuth2AccessToken);
			yield return www.SendWebRequest();
 
			if(www.isHttpError | www.isNetworkError)
			{
				Debug.Log("Something happened. Try again.");
			}
			else {
				Debug.Log("Upload complete!");
			}
		}

		public void GetOAuth2Key()
		{
			var oa = new OAuth2();
			oa.StartOauthVerify();
		}
	
		private static string ConvertToRange( string range1, string range2, string sheetName = "unassigned")
		{
			if (sheetName == "unassigned"){
				return range1 + ":" + range2;
			}
			else return sheetName + "!" + range1 + ":" + range2;
		}

	}
}

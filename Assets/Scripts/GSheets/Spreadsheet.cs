using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GSheets;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;

public class Spreadsheet {

	private string SheetID;
	
	/// <summary>
	/// Creates a new <see cref="Spreadsheet"/>.
	/// </summary>
	/// <param name="sheetLink"></param>
	/// <exception cref="ArgumentException">Invalid Link Provided.</exception>
	public Spreadsheet(string sheetLink)
	{
		Regex r = new Regex(@"(http|https)://docs.google.com/spreadsheets/d/(?<spsheetid>.*)/(edit#gid=(?<sheetid>.*))");
		var m = r.Match(sheetLink);
		if (r.IsMatch(sheetLink))
		{
			this.SheetID = m.Groups["spsheetid"].Value;
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
		string apirequest = String.Concat("https://sheets.googleapis.com/v4/spreadsheets/", this.SheetID, "/values/",
			ConvertToRange(range1, range2, sheetname), "?key=", ApiKeys.GoogleApiKey);
		WWW net = new WWW(apirequest);
		yield return net;

		SpreadsheetValuesResponse resp2 = JsonConvert.DeserializeObject<SpreadsheetValuesResponse>(net.text);

		var x = resp2.values;
		
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
		Debug.Log("inside writevalue.");
		string apirequest = String.Concat("https://sheets.googleapis.com/v4/spreadsheets/", this.SheetID, "/values/",
			ConvertToRange(range1, range2, sheetname), "?key=", ApiKeys.GoogleApiKey, "&valueInputOption=USER_ENTERED&includeValuesInResponse=false");
		SpreadsheetValuesResponse response = new SpreadsheetValuesResponse();
		response.majorDimension = "ROWS";
		response.range = ConvertToRange(range1, range2, sheetname);
		response.values = new List<List<string>>{values};

		UnityWebRequest www = UnityWebRequest.Put(apirequest,JsonConvert.SerializeObject(response));
		// todo: OAuth2.cs
		
		yield return www.SendWebRequest();
 
		if(www.isHttpError | www.isNetworkError) {
			Debug.Log(www.error);
		}
		else {
			Debug.Log("Upload complete!");
		}
	}
	
	private string ConvertToRange( string range1, string range2, string sheetName = "unassigned")
	{
		if (sheetName == "unassigned"){
			return range1 + ":" + range2;
		}
		else return sheetName + "!" + range1 + ":" + range2;
	}

}

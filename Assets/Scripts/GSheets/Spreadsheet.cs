using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Spreadsheet {

	private string SheetID;
	
	public Spreadsheet(string SheetLink)
	{
		Regex r = new Regex(@"(http|https)://docs.google.com/spreadsheets/d/(?<sheetid>.*)/(edit#.*)");
		System.Text.RegularExpressions.Match m = r.Match(SheetLink); // conflict with Match in the current sln.
		if (r.IsMatch(SheetLink))
		{
			this.SheetID = m.Groups["sheetid"].Value;
		}
		else
		{
			throw new ArgumentException("Link is not valid.");
		}
	}

	public void GetValue(int range1, int range2)
	{
		WWW net = new WWW("https://sheets.googleapis.com/v4/" + this.SheetID + "?key=" + ApiKeys.GoogleApiKey);
	}
}

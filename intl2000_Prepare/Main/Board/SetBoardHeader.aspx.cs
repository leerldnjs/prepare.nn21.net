using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Board_SetBoardHeader : System.Web.UI.Page
{
	protected StringBuilder OptionValue;
    protected void Page_Load(object sender, EventArgs e)
    {
		LoadAleadySaved(Request.Params["C"].ToString());
	}
	private Boolean LoadAleadySaved(string BoardCode)
	{
		string Format = "<option value={0}>{1}</option>";
		DBConn DB = new DBConn();
		OptionValue = new StringBuilder();

		DB.SqlCmd.CommandText = @"SELECT [Pk], [Header] FROM [BoardLibHeader] WHERE [BoardCode]='" + BoardCode + "' ORDER BY [OrderBy] ASC;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			OptionValue.Append(string.Format(Format, "\""+RS[0] + "\"", RS[1] + ""));
		}
		OptionValue.Append(string.Format(Format, "\"N\" selected=\"selected\" ", "---- 글머리 추가 ----")); 
		return true;
	}
}
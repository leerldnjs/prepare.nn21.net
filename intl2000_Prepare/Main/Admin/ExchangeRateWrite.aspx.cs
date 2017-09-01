using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Admin_ExchangeRateWrite : System.Web.UI.Page
{
	private DBConn DB;
	private String SelectedTitle;
	protected StringBuilder ExchangeRateListTableBody;
	protected StringBuilder St_ExchangeRateTitle;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["MemberInfo"] == null) { Response.Redirect("../Default.aspx"); }

		SelectedTitle = Request.Params["S"] + "" == "" ? "1" : Request.Params["S"];
		LoadExchangeRateTitle(SelectedTitle);
		//LoadExchangeRateHistory(SelectedTitle, Request.Params["F"] + "", Request.Params["T"] + "");
	}
	private void LoadExchangeRateTitle(string selectedtitle)
	{
		DB = new DBConn();
		St_ExchangeRateTitle = new StringBuilder();
		St_ExchangeRateTitle.Append("<select id=\"St_ExchangeRateTitle\" size=\"1\" onchange=\"SelectTitleNMonetaryUnit();\" >");
		DB.SqlCmd.CommandText = "SELECT [ETCSettingPk], [Value] FROM ETCSetting WHERE GubunCL=0";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read())
		{
			//Response.Write(RS[0] + "");
			if (RS[0] + "" == selectedtitle)
			{
				St_ExchangeRateTitle.Append("<option value=\"" + RS[0] + "\" selected=\"selected\" >" + RS[1] + "</option>");
			}
			else
			{
				St_ExchangeRateTitle.Append("<option value=\"" + RS[0] + "\">" + RS[1] + "</option>");
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		St_ExchangeRateTitle.Append("</select>");
	}
}
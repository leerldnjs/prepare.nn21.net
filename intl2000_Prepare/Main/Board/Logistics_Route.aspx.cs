using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Board_Logistics_Route :System.Web.UI.Page
{
	protected string Html_RouteAll;
	protected String[] MemberInfo;
	protected static int Current_Row;
	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) { Response.Redirect("../Default.aspx"); }
		MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);

	}

	[WebMethod]
	public static string MakeHtml_RouteAll(string QueryWhere) {

		DBConn DB = new DBConn();
		DB.DBCon.Open();
		string Query = @"SELECT MAX([DocumentPk]) AS DocumentPk
      ,[Value0]
      ,[Value1]
      ,[Value2]
      ,[Value3]
      ,[Value4]
      ,[Value5]
      ,[Value6]
      ,[Value7]
      ,[Value8]
  FROM [dbo].[Document]
	WHERE [TYPE] =  'NN21.COM_ROUTE' " + QueryWhere + @"
	AND [Status] = 0
	GROUP BY [Value0], [Value1], [Value2], [Value3], [Value4], [Value5], [Value6], [Value7], [Value8]
	ORDER BY [Value0], [Value1], [Value2], [Value3], [Value4], [Value5], [Value6], [Value7], [Value8]";
		DB.SqlCmd.CommandText = Query;
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("<table id=\"RouteTable\" class=\"table table-hover table-striped\"><thead><tr><th style=\"display:none;\">출발대륙</th><th>출발국가</th><th style=\"display:none;\">출발지역</th><th>출발지점</th><th style=\"display:none;\">도착대륙</th><th>도착국가</th><th style=\"display:none;\">도착지역</th><th>도착지점</th><th>운송구분</th></thead><tbody>");
		int i = 1;
		while (RS.Read()) {
			ReturnValue.Append("<tr id=\"RouteData_" + i + "\" onclick=\"Onclick_Route('" + i + "', '" + RS["DocumentPk"] + "')\"><td style=\"display:none;\">" + RS["VALUE0"] + "" + "</td><td>" + RS["VALUE1"] + "" + "</td><td style=\"display:none;\">" + RS["VALUE2"] + "" + "</td><td>" + RS["VALUE3"] + "" + "</td><td style=\"display:none;\">" + RS["VALUE4"] + "" + "</td><td>" + RS["VALUE5"] + "" + "</td><td style=\"display:none;\">" + RS["VALUE6"] + "" + "</td><td>" + RS["VALUE7"] + "" + "</td><td>" + RS["VALUE8"] + "" + "</td></tr>");
			i++;
		}

		RS.Close();
		DB.DBCon.Close();
		return ReturnValue + "</tbody></table>";
	}

	[WebMethod]
	public static string MakeHtml_Schedule(string RoutePk) {
		StringBuilder ReturnValue = new StringBuilder();
		DBConn DB = new DBConn();
		DB.DBCon.Open();

		DB.SqlCmd.CommandText = @"
		DECLARE @FROM_BRANCH NVARCHAR(100);
		DECLARE @TO_BRANCH NVARCHAR(100);
		DECLARE @ROUTE_WAY NVARCHAR(100);
		
		SELECT @FROM_BRANCH = [Value3], @TO_BRANCH = [Value7], @ROUTE_WAY = [Value8] FROM [dbo].[Document] WHERE [Type] = 'NN21.COM_ROUTE' AND DocumentPk = " + RoutePk + @"
		
		SELECT [DocumentPk]
			  ,[Value3]
			  ,[Value7]
			  ,[Value8]
			  ,[Value9]
		      ,[Value10]
		      ,[Value11]
		      ,[Value12]
		      ,[Value13]
		      ,[Value14]
		      ,[Value15]
		FROM [dbo].[Document] 
		WHERE [Type] = 'NN21.COM_ROUTE' 
		AND [Status] = 1
		AND [Value3] = @FROM_BRANCH
		AND [Value7] = @TO_BRANCH
		AND [Value8] = @ROUTE_WAY
		ORDER BY [Value9], [Value10]";

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		int i = 0;
		string Current = "";
		while (RS.Read()) {
			ReturnValue.Append("<div class=\"row\"><div class=\"col-xs-2 col-xs-offset-1\"><input type=\"text\" id=\"VesselName_" + i + "\" class=\"form-control\" value=\"" + RS["Value9"] + "" + "\" placeholder=\"선사\" readonly=\"readonly\" /></div>");
			ReturnValue.Append("<div class=\"col-xs-1\"><input type=\"text\" id=\"ShipWay_" + i + "\" class=\"form-control\" value=\"" + RS["Value10"] + "\" readonly=\"readonly\" /></div>");
			ReturnValue.Append("<div class=\"col-xs-2\"><input type=\"text\" id=\"Container_" + i + "\" class=\"form-control\" value=\"" + RS["Value11"] + "\" readonly=\"readonly\" /></div>");
			ReturnValue.Append("<div class=\"col-xs-1\"><input type=\"text\" id=\"Datetime_Dead_" + i + "\" class=\"form-control\" value=\"" + RS["Value12"] + "" + "\" placeholder=\"마감일\" /></div>");
			ReturnValue.Append("<div class=\"col-xs-1\"><input type=\"text\" id=\"Datetime_From_" + i + "\" class=\"form-control\" value=\"" + RS["Value13"] + "" + "\" placeholder=\"출항일\" /></div>");
			ReturnValue.Append("<div class=\"col-xs-1\"><input type=\"text\" id=\"Datetime_Take_" + i + "\" class=\"form-control\" value=\"" + RS["Value14"] + "" + "\" placeholder=\"운항일\" /></div>");
			ReturnValue.Append("<div class=\"col-xs-1\"><input type=\"text\" id=\"Datetime_To_" + i + "\" class=\"form-control\" value=\"" + RS["Value15"] + "" + "\" placeholder=\"도착일\" /></div>");
			ReturnValue.Append("<div class=\"col-xs-1\" style=\"text-align:right;\"><input type=\"button\" id=\"BTN_Submit_Schedule_" + i + "\" onclick=\"Set_Document('Schedule', '" + i + "', '" + RS["DocumentPk"] + "', '')\" class=\"btn btn-primary btn-xs\" value=\"일정저장\" style=\"margin-top:4px;\"/></div>");
			ReturnValue.Append("<div class=\"col-xs-1\"><input type=\"button\" id=\"BTN_Delete_Schedule" + i + "\" onclick=\"Delete_Route('" + RS["DocumentPk"] + "')\" class=\"btn btn-danger btn-xs\" value=\"일정삭제\" onclick=\"\" style=\"margin-top:4px;\"/></div></div>");
			Current = RS["Value3"] + "";
			i++;
		}
		Current_Row = i;

		RS.Close();
		DB.DBCon.Close();
		return ReturnValue + "";
	}

	[WebMethod]
	public static string MakeHtml_AddVessel(string TransportCategory) {
		StringBuilder ReturnValue = new StringBuilder();

		ReturnValue.Append("<div class=\"row\"><div class=\"col-xs-6 col-xs-offset-1\"><input type=\"text\" id=\"VesselName_49\" class=\"form-control\" value=\"\" placeholder=\"선사\" readonly=\"readonly\" /></div>");
		ReturnValue.Append("<div class=\"col-xs-4\"><input type=\"text\" id=\"ShipWay_49\" class=\"form-control\" placeholder=\"운송구분\" readonly=\"readonly\" /></div>");

		/*
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("<div class=\"row\"><div class=\"col-xs-2 col-xs-offset-1\"><input type=\"text\" id=\"VesselName_" + Current_Row + "\" class=\"form-control\" value=\"\" placeholder=\"선사\" readonly=\"readonly\" /></div>");
		ReturnValue.Append("<div class=\"col-xs-1\"><input type=\"text\" id=\"ShipWay_" + Current_Row + "\" class=\"form-control\" placeholder=\"운송구분\" readonly=\"readonly\" /></div>");
		ReturnValue.Append("<div class=\"col-xs-2\"><input type=\"text\" id=\"Container_" + Current_Row + "\" class=\"form-control\" placeholder=\"운송항목\" readonly=\"readonly\" /></div>");
		ReturnValue.Append("<div class=\"col-xs-1\"><input type=\"text\" id=\"Datetime_Dead_" + Current_Row + "\" class=\"form-control\" value=\"\" placeholder=\"마감일\" /></div>");
		ReturnValue.Append("<div class=\"col-xs-1\"><input type=\"text\" id=\"Datetime_From_" + Current_Row + "\" class=\"form-control\" value=\"\" placeholder=\"출항일\" /></div>");
		ReturnValue.Append("<div class=\"col-xs-1\"><input type=\"text\" id=\"Datetime_Take_" + Current_Row + "\" class=\"form-control\" value=\"\" placeholder=\"운항일\" /></div>");
		ReturnValue.Append("<div class=\"col-xs-1\"><input type=\"text\" id=\"Datetime_To_" + Current_Row + "\" class=\"form-control\" value=\"\" placeholder=\"도착일\" /></div>");
		ReturnValue.Append("<div class=\"col-xs-1\" style=\"text-align:right;\"><input type=\"button\" id=\"BTN_Submit_Schedule" + Current_Row + "\" onclick=\"Set_Document('Schedule', '" + Current_Row + "', '')\" class=\"btn btn-primary btn-xs\" value=\"일정저장\" style=\"margin-top:4px;\"/></div></div>");
		*/
		return ReturnValue + "";
		
	}

	[WebMethod]
	public static string MakeHtml_Container(string RoutePk, string RouteCategory) {
		StringBuilder ReturnValue = new StringBuilder();
		DBConn DB = new DBConn();
		DB.DBCon.Open();

		DB.SqlCmd.CommandText = @"
		DECLARE @FROM_BRANCH NVARCHAR(100);
		DECLARE @TO_BRANCH NVARCHAR(100);
		DECLARE @ROUTE_WAY NVARCHAR(100);
		
		SELECT @FROM_BRANCH = [Value3], @TO_BRANCH = [Value7], @ROUTE_WAY = [Value8] FROM [dbo].[Document] WHERE [Type] = 'NN21.COM_ROUTE' AND DocumentPk = " + RoutePk + @"
		
		SELECT [Value9], [Value10]
		FROM [dbo].[Document] 
		WHERE [Type] = 'NN21.COM_ROUTE' 
		AND [Status] = 1
		AND [Value3] = @FROM_BRANCH
		AND [Value7] = @TO_BRANCH
		AND [Value8] = @ROUTE_WAY
		GROUP BY [Value9], [Value10]";

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		ReturnValue.Append("<select id=\"st_VesselChoose\" class=\"form-control\" style=\"margin-bottom:30px;\" onchange=\"javascript:{ AddContainer_Vessel = this.value.split(',!'); }\" >");
		ReturnValue.Append("<option value=\"\">선사선택</option>");
		while (RS.Read()) {
			ReturnValue.Append("<option value=\"" + RS["Value9"] + ",!" + RS["Value10"] + "\">" + RS["Value9"] + "</option>");
		}
		ReturnValue.Append("</select>");
		RS.Dispose();

		DB.SqlCmd.CommandText = @"SELECT [VALUE_0] FROM [dbo].[SAVED] WHERE [CODE] = 'nn21com_Route_Container' AND [VALUE_4] = '" + RouteCategory + @"' ;";
		RS = DB.SqlCmd.ExecuteReader();

		while (RS.Read()) {
			ReturnValue.Append("<div><div class=\"col-xs-5 col-xs-offset-2\"><label class=\"control-label\">" + RS["VALUE_0"] + "</label></div><label class=\"switch\"><input type=\"checkbox\" value=\"" + RS["VALUE_0"] + "\"><span></span></label><div>");
		}
		RS.Close();
		DB.DBCon.Close();

		return ReturnValue + "";
	}

	[WebMethod]
	public static string AddContainer_Check(string FromBranch, string ToBranch, string RouteCategory, string Vessel, string ShipWay) {
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"DELETE FROM [dbo].[Document] 
		WHERE [Type] = 'NN21.COM_ROUTE'
		AND [Status] = 1
		AND [Value3] = N'" + FromBranch + @"'
		AND [Value7] = N'" + ToBranch + @"'
		AND [Value8] = N'" + RouteCategory + @"'
		AND [Value9] = N'" + Vessel + @"'
		AND [Value10] = N'" + ShipWay + @"'
		AND [Value11] IS NULL";
		DB.SqlCmd.ExecuteNonQuery();

		DB.DBCon.Close();

		return "1";
	}

	[WebMethod]
	public static string Duplicate_Check(string Value0, string Value1, string Value2, string Value3, string Value4, string Value5, string Value6, string Value7, string Value8) {
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"SELECT COUNT(*) FROM [dbo].[Document] 
		WHERE [TYPE] = 'NN21.COM_ROUTE'
		AND [Status] = 0
		AND [Value0] = '" + Value0 + @"'
		AND [Value1] = '" + Value1 + @"'
		AND [Value2] = '" + Value2 + @"'
		AND [Value3] = '" + Value3 + @"'
		AND [Value4] = '" + Value4 + @"'
		AND [Value5] = '" + Value5 + @"'
		AND [Value6] = '" + Value6 + @"'
		AND [Value7] = '" + Value7 + @"'
		AND [Value8] = '" + Value8 + @"'";
		string Count = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();

		if (Count == "0") {
			return "0";
		}
		else {
			return "1";
		}
	}

}
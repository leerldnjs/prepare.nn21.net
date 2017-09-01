using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Request_RequestWrite : System.Web.UI.Page
{
	protected String CompanyPk;
	protected String Mode;
	protected String RequestFormPk;
	protected String AccountID;
	protected String SelectArrivalBranch;
	protected String SelectDepartureBranch;
	protected String OnlyStorcedIn;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["MemberInfo"] == null) {
			Response.Redirect("~/Default.aspx");
		}
		Mode = Request.Params["M"] + "" == "" ? "Write" : Request.Params["M"] + "";
		AccountID = Session["ID"] + "";

        // 입고지사,배송지사 보이지 않도록 수정~ (2012.05.25)
		//OnlyStorcedIn = "";
		//if (Request.Params["G"] == "Admin") {
		//	SelectArrivalBranch = "<div style=\"padding:10px; \">배송지사 : <select id=\"ArrivalBranch\">" + Common.SelectOurBranch + "</select></div>";
		//	SelectDepartureBranch = "<div style=\"padding:10px; \">입고지사 : <select id=\"DepartureBranch\">" + Common.SelectOurBranch + "</select></div>";

		OnlyStorcedIn = "";
		if (Request.Params["G"] == "Admin") {
            SelectArrivalBranch = "<input type=\"hidden\" id=\"ArrivalBranch\" />";
            SelectDepartureBranch = "<input type=\"hidden\" id=\"DepartureBranch\" />";
		} else {
			SelectArrivalBranch = "<input type=\"hidden\" id=\"ArrivalBranch\" />";
			SelectDepartureBranch = "<input type=\"hidden\" id=\"DepartureBranch\" />";
		}
		if (Request.Params["G"] == "Admin") {
			if (Mode == "Modify") {
				LogedWithoutRecentRequest1.Visible = false;
				Loged1.Visible = false;

			} else {
				LogedWithoutRecentRequest1.Visible = true;
				Loged1.Visible = false;
			
			}
			if (Mode == "Write") {
				CompanyPk = Request.Params["P"] + "";
				RequestFormPk = "";
			} else {
				CompanyPk = "";
				RequestFormPk = Request.Params["P"] + "";
			}
		} else {
			LogedWithoutRecentRequest1.Visible = false;
			Loged1.Visible = true;
			CompanyPk = Session["CompanyPk"] + "";
			RequestFormPk = Request.Params["P"] + "";
		}
	}
}
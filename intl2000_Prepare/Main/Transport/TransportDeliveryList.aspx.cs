using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Transport_TransportDeliveryList :System.Web.UI.Page
{
	protected string ListWho;
	protected string Html_ResponsibleStaff;
	protected string Html_DeliveryList;
	protected string OurBranchPk;
	protected string[] MEMBERINFO;

	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) {
			Response.Redirect("/Default.aspx");
		}
		else {
			MEMBERINFO = (Session["MemberInfo"] + "").Split(Common.Splite11, StringSplitOptions.None);
		}

		OurBranchPk = MEMBERINFO[1];
		ListWho = Request.Params["W"].ToString();

		Html_ResponsibleStaff = MakeHtml_ResponsibleStaff();
		Html_DeliveryList = MakeHtml_DeliveryList();
	}

	private string MakeHtml_ResponsibleStaff() {
		StringBuilder ReturnValue = new StringBuilder();
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"SELECT [AccountPk], [AccountID] FROM [dbo].[Account_] WHERE [CompanyPk] = " + OurBranchPk + @" AND [Duties] LIKE '%업무부%' ";

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		ReturnValue.Append("<div class=\"col-xs-1\"><input type=\"button\" class=\"btn btn-xs btn-default form-control\" value=\"ALL\" /></div>");
		while (RS.Read()) {
			ReturnValue.Append("<div class=\"col-xs-1\"><input type=\"button\" class=\"btn btn-xs btn-default form-control\" value=\"" + RS["AccountID"] + "\" onclick=\"RetrieveStaff(this.value)\" /></div>");
		}

		DB.DBCon.Close();
		return ReturnValue.ToString();
	}

	private string MakeHtml_DeliveryList() {
		StringBuilder ReturnValue = new StringBuilder();
		TransportC TC = new TransportC();
		List<sDeliveryListHead> Delivery = new List<sDeliveryListHead>();
		DBConn DB = new DBConn();
		DB.DBCon.Open();

		Delivery = TC.LoadList_DeliveryList(ListWho, ref DB);

		for(int i = 0; i < Delivery.Count; i++) {
			ReturnValue.Append("<table class=\"table text-xs\" style=\"margin-top:5%;\" >");
			ReturnValue.Append("<tbody><tr class=\"bg-danger\"><td>" + Delivery[i].TransportWay + "</td>");
			ReturnValue.Append("<td>" + Delivery[i].TransportTitle + "</td>");
			ReturnValue.Append("<td>" + Delivery[i].TransportVesselName + "</td>");
			ReturnValue.Append("<td>" + Delivery[i].TransportVoyageNo + "</td>");
			ReturnValue.Append("<td>" + Delivery[i].TransportAreaFrom + "</td>");
			ReturnValue.Append("<td>" + Delivery[i].TransportAreaTo + "</td>");
			ReturnValue.Append("<td>" + Delivery[i].PackedNo + "</td>");
			ReturnValue.Append("<td>" + Delivery[i].PackedSize + "</td>");
			ReturnValue.Append("<td>" + Delivery[i].PackedSeal + "</td></tr></tbody>");

			ReturnValue.Append("<tbody>");
			ReturnValue.Append("<tr><td>BL No</td>");
			ReturnValue.Append("<td>고객코드</td>");
			ReturnValue.Append("<td>고객명</td>");
			ReturnValue.Append("<td>수량</td>");
			ReturnValue.Append("<td>단위</td>");
			ReturnValue.Append("<td>체적</td>");
			ReturnValue.Append("<td>중량</td>");
			ReturnValue.Append("<td>CO</td>");
			ReturnValue.Append("<td>화물상태</td>");
			ReturnValue.Append("<td>통관상태</td>");
			ReturnValue.Append("<td>청구상태</td>");
			ReturnValue.Append("<td>출고일</td>");
			ReturnValue.Append("<td>출고지</td>");
			ReturnValue.Append("<td>배차</td>");
			ReturnValue.Append("<td>상세</td></tr>");

			for (int j = 0; j < Delivery[i].arrBody.Count; j++) {
				ReturnValue.Append("<tr><td>" + Delivery[i].arrBody[j].BlNo + "</td>");
				ReturnValue.Append("<td>" + Delivery[i].arrBody[j].CompanyCode + "</td>");
				ReturnValue.Append("<td>" + Delivery[i].arrBody[j].CompanyName + "</td>");
				ReturnValue.Append("<td>" + Delivery[i].arrBody[j].PackedCount + "</td>");
				ReturnValue.Append("<td>" + Common.GetPackingUnit(Delivery[i].arrBody[j].PackingUnit) + "</td>");
				ReturnValue.Append("<td>" + Delivery[i].arrBody[j].Volume + "</td>");
				ReturnValue.Append("<td>" + Delivery[i].arrBody[j].Weight + "</td>");

				string[] DocumentRequest = Delivery[i].arrBody[j].DocumentRequest.Split(Common.Splite11, StringSplitOptions.RemoveEmptyEntries);
				string ReqDoc = "";
				foreach(string RD in DocumentRequest) {
					switch (RD) {
						case "31":
							ReqDoc += "화주원산지제공";
							break;

						case "32":
							ReqDoc += "원산지신청";
							break;

						case "34":
							ReqDoc += "FTA";
							break;
					}
				}

				ReturnValue.Append("<td>" + ReqDoc + "</td>");
				ReturnValue.Append("<td>" + "" +"</td>");
				ReturnValue.Append("<td>" + "" +"</td>");
				ReturnValue.Append("<td>" + "" +"</td>");
				ReturnValue.Append("<td>" + Delivery[i].arrBody[j].ArrivalDate + "</td>");
				ReturnValue.Append("<td>" + "<input type=\"button\" class=\"btn btn-xs btn-info\" value=\"출고지\" />" + "</td>");
				ReturnValue.Append("<td>" + "<input type=\"button\" class=\"btn btn-xs btn-info\" value=\"배차\" />" + "</td>");
				ReturnValue.Append("<td>" + "▼" + "</td>");
			}

			ReturnValue.Append("</tbody>");
			ReturnValue.Append("</table>");
		}

		DB.DBCon.Close();
		return ReturnValue.ToString();
	} 
}
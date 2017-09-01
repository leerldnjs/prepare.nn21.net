using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Admin_TransportBetweenCompanyList : System.Web.UI.Page
{
	protected String[] MEMBERINFO;
	protected String Gubun;
	private Int32 PageNo;
	private Int32 TotalRecord;
	private int pagelength = 50;
	protected String HTMLBody;
	protected String HTMLDriver;
	private DBConn DB;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) {
			Response.Redirect("../Default.aspx");
		}
		try {
			if (Request["Language"].Length == 2) {
				Session["Language"] = Request["Language"];
			}
		} catch (Exception) {
		}
		switch (Session["Language"] + "") {
			case "en":
				Page.UICulture = "en";
				break;
			case "ko":
				Page.UICulture = "ko";
				break;
			case "zh":
				Page.UICulture = "zh-cn";
				break;
		}
		MEMBERINFO = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);

		PageNo = Request.Params["PageNo"] + "" == "" ? 1 : Int32.Parse(Request.Params["PageNo"] + "");
		DB = new DBConn();

		//HTMLHeader = LoadHeader(StorageCode, MEMBERINFO[1]);
		//LoadAleadySetDriver(StorageCode, MEMBERINFO[1]);
		LoadBody(MEMBERINFO[1]);

		if (MEMBERINFO[0] == "Customs") {
			LogedWithoutRecentRequest1.Visible = false;
			Loged1.Visible = true;
		} else if (MEMBERINFO[0] == "OurBranchOut") {
			LogedWithoutRecentRequest1.Visible = false;
			Loged2.Visible = true;
		}
	}
	private Boolean LoadBody(string BranchPk)
	{
		DB.SqlCmd.CommandText = @"
DECLARE @BranchCode int;
SET @BranchCode=" + BranchPk + @";
SELECT COUNT(*)
FROM [dbo].[TRANSPORT_BODY] AS TB
	left join OurBranchStorageCode AS OBSC ON TB.[WAREHOUSE_PK_DEPARTURE]=OBSC.OurBranchStoragePk
	left join [dbo].[TRANSPORT_HEAD] AS TBH ON TBH.[TRANSPORT_PK]=TB.[TRANSPORT_HEAD_PK]
	left join RequestForm AS R ON TB.[REQUEST_PK]=R.RequestFormPk 
	left join RegionCode AS RC ON R.DepartureRegionCode=RC.RegionCode 
	left join Company AS Consignee ON Consignee.CompanyPk=R.ConsigneePk 
WHERE OBSC.OurBranchCode=@BranchCode 
AND TBH.[TRANSPORT_WAY] = 'Delivery'
AND TB.[PACKED_COUNT]>0 
AND isnull(TBH.[VOYAGE_NO], '')<>'' ";
		DB.DBCon.Open();
		int TotalRecord = Int32.Parse(DB.SqlCmd.ExecuteScalar() + "");


		string InnerTableRowFormat = "<tr>" +
	"<td class='TBody1' >{0}</td>" +
		"<td class='TBody1' style=\"font-weight:bold;\"><a onclick=\"Goto('RequestForm', '{7}');\"><span style=\"cursor:hand; font-weight:bold;\">{4}</span></a></td>" +
	"<td class='TBody1' ><a onclick=\"Goto('RequestForm', '{7}');\"><span style=\"cursor:hand; \">{3}</span></a></td>" +
	"<td class='TBody1' ><a onclick=\"Goto('RequestForm', '{7}');\"><span style=\"cursor:hand; \">{1}</span></a></td>" +
	"<td class='TBody1' style=\"text-align:left; padding-left:10px; \" >{2}</td>" +
	"<td class='TBody1' >{5}</td>" +
	"<td class='TBody1' >{8}</td>" +
	"<td class='TBody1' >{6}</td></tr>";

		DB.SqlCmd.CommandText = @"	
DECLARE @BranchCode int;
SET @BranchCode=" + BranchPk + @";
SELECT TOP " + pagelength * PageNo + @"
	TB.[TRANSPORT_BODY_PK], TB.[REQUEST_PK]
	, OBSC.StorageName 
	, Consignee.CompanyName AS ConsigneeName, Consignee.CompanyNamee AS ConsigneeNamee
	, R.ConsigneePk, R.ShipperCode, R.ConsigneeCode, R.[DepartureDate], R.[ArrivalDate], R.TransportWayCL
	, RC.Name AS DRCName
	, R.TotalPackedCount, R.PackingUnit 
	, TBH.[VOYAGE_NO], TBH.[TITLE], TBH.[VESSELNAME], TBH.[DATETIME_FROM], TB.[PACKED_COUNT], RFCH.[BRANCH_COMPANY_PK], RFCH.[CUSTOMER_COMPANY_PK], RFCH.[TOTAL_PRICE]
FROM [dbo].[TRANSPORT_BODY] AS TB
	left join OurBranchStorageCode AS OBSC ON TB.[WAREHOUSE_PK_DEPARTURE]=OBSC.OurBranchStoragePk
	left join [dbo].[TRANSPORT_HEAD] AS TBH ON TBH.[TRANSPORT_PK]=TB.[TRANSPORT_HEAD_PK]
	left join RequestForm AS R ON TB.[REQUEST_PK]=R.RequestFormPk 
	left join RegionCode AS RC ON R.DepartureRegionCode=RC.RegionCode 
	left join Company AS Consignee ON Consignee.CompanyPk=R.ConsigneePk 
	LEFT JOIN [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH ON TB.[REQUEST_PK] = RFCH.[TABLE_PK]
	LEFT JOIN [dbo].[REQUESTFORMCALCULATE_BODY] AS RFCB ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK] = RFCB.[REQUESTFORMCALCULATE_HEAD_PK]
WHERE OBSC.OurBranchCode=@BranchCode 
AND TBH.[TRANSPORT_WAY] = 'Delivery'
AND RFCH.[TABLE_NAME] = 'RequestForm'
AND RFCB.[PRICE_CODE] LIKE '%Delivery%'
AND TB.[PACKED_COUNT]>0 
AND isnull(TBH.[VOYAGE_NO], '')<>''  
AND  TBH.[TRANSPORT_STATUS]=6
ORDER BY  TBH.[DATETIME_FROM] DESC, TBH.[VOYAGE_NO] ASC, TBH.[TITLE] ASC, Consignee.CompanyPk, TBH.[TRANSPORT_STATUS] ASC;";
		//Response.Write(DB.SqlCmd.CommandText);
		
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder returnvalue = new StringBuilder();
		StringBuilder innertable = new StringBuilder();
		string[] innertableData;
		string TempType = "#@!";
		string TempTitle = "#@!";

		string tempOutDate = "";
		Int32 TCursor = 0;
		bool TCheck = false;
		while (RS.Read()) {
			if (TCheck == false && TCursor < (PageNo - 1) * pagelength) {
				TCursor++;
				continue;
			}
			if (!TCheck) {
				TCheck = true;
			}

			if (tempOutDate != RS["DATETIME_FROM"] + "") {
				tempOutDate = RS["DATETIME_FROM"] + "";
				innertable.Append("<tr><td colspan=\"8\" class='THead1' style=\"text-align:left; font-weight:bold; padding:5px; \" >&nbsp;&nbsp;" + 
					String.Format("{0}. {1}. {2}", tempOutDate.Substring(0, 4), tempOutDate.Substring(4, 2), tempOutDate.Substring(6, 2))+
					"</td></tr>");
			}
			if (TempType == "#@!") {
				TempType = RS["VOYAGE_NO"] + "";
			} else if (TempType != RS["VOYAGE_NO"] + "") {
				TempType = RS["VOYAGE_NO"] + "";
			}

			if (TempTitle != RS["TITLE"] + "") {
				TempTitle = RS["TITLE"] + "";
				innertable.Append("<tr><td colspan=\"8\" style=\"text-align:left; padding:5px; color:gray; font-weight:bold; \" >&nbsp;&nbsp;&nbsp;&nbsp; < " + TempType + "&nbsp;&nbsp;" + TempTitle + " ></td></tr>");
			}

			innertableData = new string[9];
			innertableData[0] = RS["DRCName"] + "";
			string TdepartureDate = RS["DepartureDate"] + "";
			string TarrivalDate = RS["ArrivalDate"] + "";
            if (RS["DepartureDate"] + "" != "")
            {
                if (RS["ArrivalDate"] + "" != "")
                {
                    innertableData[1] = string.Format("{0}/{1}", TdepartureDate.Substring(4, 2), TdepartureDate.Substring(6, 2)) + " - " + string.Format("{0}/{1}", TarrivalDate.Substring(4, 2), TarrivalDate.Substring(6, 2));
                }
                else 
                {
                    innertableData[1] = "도착일자미정";
                }
            }
            innertableData[2] = Common.GetTransportWay(RS["TransportWayCL"] + "");
			innertableData[3] = RS["ShipperCode"] + " - " + RS["ConsigneeCode"];
			innertableData[4] = RS["ConsigneeName"] + (RS["ConsigneeNamee"] + "" == "" ? "&nbsp;" : "/" + RS["ConsigneeNamee"]);
			innertableData[5] = RS["PACKED_COUNT"] + "/" + RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "");

			if (RS["BRANCH_COMPANY_PK"] + "" == RS["CUSTOMER_COMPANY_PK"] + "") {
				innertableData[6] = "<span style=\"cursor:hand;\" onclick=\"DeliveryModify('" + RS["TRANSPORT_BODY_PK"] + "','" + RS["REQUEST_PK"] + "' , '" + RS["ConsigneePk"] + "', '" + MEMBERINFO[1] + "');\" >  " +
						"현불 " + Common.NumberFormat(RS["TOTAL_PRICE"] + "") + "</span>";
			} else {
				innertableData[6] = "<span style=\"cursor:hand;\" onclick=\"DeliveryModify('" + RS["TRANSPORT_BODY_PK"] + "','" + RS["REQUEST_PK"] + "' , '" + RS["ConsigneePk"] + "', '" + MEMBERINFO[1] + "');\" >  " +
						"착불 " + Common.NumberFormat(RS["TOTAL_PRICE"] + "") + "</span>";
			}
			innertableData[7] = RS["REQUEST_PK"] + "";
			innertableData[8] = RS["StorageName"] + "";
			innertable.Append(String.Format(InnerTableRowFormat, innertableData));
		}
		RS.Dispose();
		DB.DBCon.Close();
		if (innertable + "" != "") {
			HTMLBody = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"padding:3px; width:830px; \"><tr>" +
					"	<td style=\"width:50px; font-size:1px; \">&nbsp;</td>" +
					"	<td style=\"font-size:1px; \">&nbsp;</td>" +
					"	<td style=\"width:130px; font-size:1px; \">&nbsp;</td>" +
					"	<td style=\"width:100px; font-size:1px; \">&nbsp;</td>" +
					"	<td style=\"width:70px; font-size:1px; \">&nbsp;</td>" +
					"	<td style=\"width:90px; font-size:1px; \">&nbsp;</td>" +
					"	<td style=\"width:110px; font-size:1px; \">&nbsp;</td>" +
					"	<td style=\"width:100px; font-size:1px; \">&nbsp;</td>" +
					"</tr>" + innertable + "<TR Height='20px'><td colspan='8' style='background-color:#F5F5F5; text-align:center; padding:20px; '>" + new Common().SetPageListByNo(pagelength, PageNo, TotalRecord, "TransportBetweenCompanyList.aspx", "?") + "</TD></TR></Table>";
		}
		return true;
	}
}
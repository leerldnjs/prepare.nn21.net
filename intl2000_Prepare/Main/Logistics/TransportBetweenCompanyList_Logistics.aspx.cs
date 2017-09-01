using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Logistics_TransportBetweenCompanyList_Logistics : System.Web.UI.Page
{
	protected String[] MEMBERINFO;
	protected String Gubun;
	private Int32 PageNo;
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

		LoadBody(MEMBERINFO[1]);

	}
	private Boolean LoadBody(string BranchPk)
	{
		DB.SqlCmd.CommandText = @"
DECLARE @BranchCode int;
SET @BranchCode=" + BranchPk + @";
SELECT COUNT(*)
FROM OurBranchStorageOut AS OBS 
	left join OurBranchStorageCode AS OBSC ON OBS.StorageCode=OBSC.OurBranchStoragePk
	left join [dbo].[TRANSPORT_HEAD] AS TBH ON TBH.[TRANSPORT_PK]=OBS.TransportBetweenBranchPk
	left join RequestForm AS R ON OBS.RequestFormPk=R.RequestFormPk 
	left join RegionCode AS RC ON R.DepartureRegionCode=RC.RegionCode 
	left join Company AS Consignee ON Consignee.CompanyPk=R.ConsigneePk 
	left join TransportBC AS TBC ON TBC.TransportBetweenCompanyPk=OBS.TransportBetweenCompanyPk 
WHERE OBSC.OurBranchCode=@BranchCode and OBS.BoxCount>0 
and	(isnull(type,'') like '%온세상%' or isnull(TBC.title,'') like '%온세상%')
and OBS.StatusCL=6";

		DB.DBCon.Open();
		int TotalRecord = Int32.Parse(DB.SqlCmd.ExecuteScalar() + "");


		string InnerTableRowFormat = "<tr>" +
	"<td class='TBody1' ><span>{0}</span></td>" +
	"<td class='TBody1' ><span>{1}</span></td>" +
	//"<td class='TBody1' style=\"font-weight:bold;\"><a href=\"../Logistics/CompanyView_Logistics.aspx?M=View&S={2}\"><span style=\"cursor:hand; font-weight:bold;\">{4}</span></a></td>" +
	"<td class='TBody1' style=\"font-weight:bold;\"><span>{4}</span></td>" +
	"<td class='TBody1' ><span>{3}</span></td>" +
	"<td class='TBody1' >{5}</td>" +
	"<td class='TBody1' >{8}</td>" +
    "<td class='TBody1' >{6}</td>" +
	"<td class='TBody1' >{9}</td></tr>";

		DB.SqlCmd.CommandText = @"	
DECLARE @BranchCode int;
SET @BranchCode=" + BranchPk + @";
SELECT TOP " + pagelength * PageNo + @"
	OBS.OurBranchStorageOutPk, OBS.RequestFormPk
	, OBSC.StorageName 
	, Consignee.CompanyName AS ConsigneeName, Consignee.CompanyNamee AS ConsigneeNamee
	, R.ConsigneePk, R.ShipperCode, R.ConsigneeCode, R.[DepartureDate], R.[ArrivalDate], R.TransportWayCL
	, RC.Name AS DRCName
	, RF.TotalPackedCount, RF.PackingUnit 
	, TBC.Type, TBC.Title, TBC.DriverName, TBC.FromDate, TBC.PackedCount , TBC.[DepositWhere], TBC.[Price], TBC.DeliveryPrice TBCDeliveryPrice
FROM OurBranchStorageOut AS OBS 
	left join OurBranchStorageCode AS OBSC ON OBS.StorageCode=OBSC.OurBranchStoragePk
	left join [dbo].[TRANSPORT_HEAD] AS TBH ON TBH.[TRANSPORT_PK]=OBS.TransportBetweenBranchPk
	left join RequestForm AS R ON OBS.RequestFormPk=R.RequestFormPk 
	left join RegionCode AS RC ON R.DepartureRegionCode=RC.RegionCode 
	left join Company AS Consignee ON Consignee.CompanyPk=R.ConsigneePk 
	left join [dbo].[RequestForm] AS RF ON RF.RequestFormPk=OBS.RequestFormPk 
	left join TransportBC AS TBC ON TBC.TransportBetweenCompanyPk=OBS.TransportBetweenCompanyPk 
WHERE OBSC.OurBranchCode=@BranchCode and OBS.BoxCount>0 
and	(isnull(type,'') like '%온세상%' or isnull(TBC.title,'') like '%온세상%')
and OBS.StatusCL=6
ORDER BY  TBC.[FromDate] DESC, TBC.Type ASC, TBC.Title ASC, Consignee.CompanyPk, StaTusCL ASC;";
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

			if (tempOutDate != RS["FromDate"] + "") {
				tempOutDate = RS["FromDate"] + "";
				innertable.Append("<tr><td colspan=\"9\" class='THead1' style=\"text-align:left; font-weight:bold; padding:5px; \" >&nbsp;&nbsp;" + 
					String.Format("{0}. {1}. {2}", tempOutDate.Substring(0, 4), tempOutDate.Substring(4, 2), tempOutDate.Substring(6, 2))+
					"</td></tr>");
			}
			if (TempType == "#@!") {
				TempType = RS["Type"] + "";
			} else if (TempType != RS["Type"] + "") {
				TempType = RS["Type"] + "";
			}

			if (TempTitle != RS["Title"] + "") {
				TempTitle = RS["Title"] + "";
                //innertable.Append("<tr><td colspan=\"8\" style=\"text-align:left; padding:5px; color:gray; font-weight:bold; \" >&nbsp;&nbsp;&nbsp;&nbsp; < " + TempType + "&nbsp;&nbsp;" + TempTitle + " ></td></tr>");
			}

			innertableData = new string[10];
			
			string TdepartureDate = RS["DepartureDate"] + "";
			string TarrivalDate = RS["ArrivalDate"] + "";
            if (RS["DepartureDate"] + "" != "")
            {
                if (RS["ArrivalDate"] + "" != "")
                {
					innertableData[0] = string.Format("{0}/{1}", TdepartureDate.Substring(4, 2), TdepartureDate.Substring(6, 2));
                    innertableData[1] = string.Format("{0}/{1}", TarrivalDate.Substring(4, 2), TarrivalDate.Substring(6, 2));
                }
                else 
                {
					innertableData[0] = string.Format("{0}/{1}", TdepartureDate.Substring(4, 2), TdepartureDate.Substring(6, 2));
                    innertableData[1] = "도착일자미정";
                }
            }
			innertableData[2] = RS["ConsigneePk"] + "";
			innertableData[3] = RS["ConsigneeCode"] + "";
			innertableData[4] = RS["ConsigneeName"] + (RS["ConsigneeNamee"] + "" == "" ? "&nbsp;" : "/" + RS["ConsigneeNamee"]);
			innertableData[5] = RS["PackedCount"] + "/" + RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "");

			if (RS["DepositWhere"] + "" == "0") {
				innertableData[6] = "<span style=\"cursor:hand;\" onclick=\"DeliveryModify('" + RS["OurBranchStorageOutPk"] + "','" + RS["RequestFormPk"] + "' , '" + RS["ConsigneePk"] + "', '" + MEMBERINFO[1] + "');\" >  " +
						"현불 " + Common.NumberFormat(RS["Price"] + "") + "</span>";
			} else {
				innertableData[6] = "<span style=\"cursor:hand;\" onclick=\"DeliveryModify('" + RS["OurBranchStorageOutPk"] + "','" + RS["RequestFormPk"] + "' , '" + RS["ConsigneePk"] + "', '" + MEMBERINFO[1] + "');\" >  " +
						"착불 " + Common.NumberFormat(RS["Price"] + "") + "</span>";
			}
			innertableData[7] = RS["RequestFormPk"] + "";
			innertableData[8] = RS["StorageName"] + "";
            innertableData[9] = Common.NumberFormat(RS["TBCDeliveryPrice"] + "");
			innertable.Append(String.Format(InnerTableRowFormat, innertableData));
		}
		RS.Dispose();
		DB.DBCon.Close();
		if (innertable + "" != "") {
			HTMLBody = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"padding:3px; width:950px; \"><tr>" +
					"	<td style=\"width:55px; font-size:1px; \">&nbsp;</td>" +
					"	<td style=\"width:55px; font-size:1px; \">&nbsp;</td>" +
					"	<td style=\"font-size:1px; \">&nbsp;</td>" +
					"	<td style=\"width:100px; font-size:1px; \">&nbsp;</td>" +
					"	<td style=\"width:70px; font-size:1px; \">&nbsp;</td>" +
					"	<td style=\"width:110px; font-size:1px; \">&nbsp;</td>" +
					"	<td style=\"width:90px; font-size:1px; \">&nbsp;</td>" +
                    "	<td style=\"width:90px; font-size:1px; \">&nbsp;</td>" +
					"</tr>" + innertable + "<TR Height='20px'><td colspan='9' style='background-color:#F5F5F5; text-align:center; padding:20px; '>" + new Common().SetPageListByNo(pagelength, PageNo, TotalRecord, "TransportBetweenCompanyList_Logistics.aspx", "?") + "</TD></TR></Table>";
		}
		return true;
	}
}
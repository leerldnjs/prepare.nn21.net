using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Text;
using System.Data.SqlClient;

public partial class Admin_StorageOutForCustoms : System.Web.UI.Page
{
	protected String[] MEMBERINFO;
	protected String Gubun;
	protected Int32 PageNo;
	protected Int32 TotalRecord;
	protected String StorageCode;
	protected String HTMLHeader;
	protected String HTMLBody;
	protected String HTMLDriver;
	private DBConn DB;
	protected Int32 ChkCount = -1;
    protected String TodayAll;
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
		if (MEMBERINFO[0] != "Customs") {
			LogedWithoutRecentRequest11.Visible = true;
			Loged1.Visible = false;
		} else {
			LogedWithoutRecentRequest11.Visible = false;
			Loged1.Visible = true;
		}

		StorageCode = Request.Params["S"] + "" == "" ? "0" : Request.Params["S"] + "";
		PageNo      = Request.Params["PageNo"] + "" == "" ? 1 : Int32.Parse(Request.Params["PageNo"] + "");
        TodayAll    = Request.Params["TD"];
		DB = new DBConn();
			
			HTMLHeader = "";
			HTMLBody = LoadBodyForCustoms(StorageCode, MEMBERINFO[1], PageNo,MEMBERINFO[0]);
    }

	private String LoadBodyForCustoms(string StorageCode, string CompanyPk, int PageNo, string Gubun)
	{
		string INPUTBL = "<input type=\"button\" value=\"B\" style=\"width:20px; padding:0px;  \" onclick=\"Print('B', '{0}');\" />&nbsp;<input type=\"button\" value=\"I\" style=\"width:20px; padding:0px;  \"  onclick=\"Print('I', '{0}');\" />&nbsp;<input type=\"button\" value=\"P\" style=\"width:20px; padding:0px;  \"  onclick=\"Print('P', '{0}');\" />&nbsp;<input type=\"button\" value=\"Ex\" style=\"width:20px; padding:0px;  \"  onclick=\"InvoiceExcelDown('{0}');\" />";
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("	<table border='0' cellpadding='0' cellspacing='0' style='width:1050px;' ><thead><tr style=\"height:40px;\">" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; width:50px; \" >From</td>" +
										"		<td class='THead1' style='width:130px;'>BLNo</td>" +
										"		<td class='THead1' style='width:60px;' >s</td>" +
										"		<td class='THead1' style='width:60px;' >c</td>" +
										"		<td class='THead1' style='width:50px;'>Arrival</td>" +
										"		<td class='THead1' style='width:250px;'>CompanyName</td>" +
										"		<td class='THead1' style='width:120px;'>세관</td>" +
										"		<td class='THead1' style='width:60px;'>CT</td>" +
										"		<td class='THead1' style='width:60px;'>Weight</td>" +
										"		<td class='THead1' >Volume</td>" +
										"		<td class='THead1' style='width:120px;'>서류</td>" +
										"	</tr></thead>");
		string EachGroup = "	<tr style=\"height:20px;\"><td class='TBody1' style=\"text-align:left; \" colspan='11'  >{0}</td></tr>";
		string EachRow = "	<tr style=\"height:20px; \">" +
									"		<td class='TBody1' style=\"text-align:center; \" ><a onclick=\"Goto('CheckDescription','{2}');\">{4}</a></td>" +
									"		<td class='TBody1' ><a onclick=\"Goto('RequestForm','{3}');\">{8}</a></td>" +
									"		<td class='TBody1' >{5}</td>" +
									"		<td class='TBody1' >{6}</td>" +
									"		<td class='TBody1' >{7}</td>" +
									"		<td class='TBody1' ><strong><a onclick=\"Goto('CheckDescription','{2}');\">{9}</a></strong></td>" +
									"		<td class='TBody1' >{15}</td>" +
									"		<td class='TBody1' {14}>{10}</td>" +
									"		<td class='TBody1' {14}>{11}</td>" +
									"		<td class='TBody1' {14}>{12}</td>" +
									"		<td class='TBody1' >{13}</td>" +
									"	</tr>";

		string QueryWhere = "";
		//성심이면
		if (Gubun == "Customs") {
			QueryWhere = " and isnull(R.GubunCL,0)=1 ";
		} else {
			QueryWhere = " and isnull(R.GubunCL,0)<>1 ";
		}
		DB.DBCon.Open();
		//DB.SqlCmd.CommandText = @"EXECUTE SP_PrepareDeliveryListCustoms @BranchPk=" + CompanyPk + ";";
		//Response.Write(DB.SqlCmd.CommandText);
		DB.SqlCmd.CommandText = @"
DECLARE @BranchPk int;
SET @BranchPk="+CompanyPk+ @";
	SELECT   * 
FROM 
(
SELECT OBS.[WAREHOUSE_PK], OBS.[REQUEST_PK], OBS.[PACKED_COUNT], OBS.[TRANSPORT_HEAD_PK]
	, OBSC.OurBranchStoragePk, OBSC.OurBranchCode, OBSC.StorageName, OBSC.StorageAddress, OBSC.TEL, OBSC.FAX 
	, TBH.[TRANSPORT_WAY], TBH.[VALUE_STRING_0], TBH.[BRANCHPK_FROM], TBH.[BRANCHPK_TO], TBH.[DATETIME_FROM], TBH.[DATETIME_TO], TBH.[VESSELNAME] 
	, TBH.[VOYAGE_NO], TBH.[VALUE_STRING_1], TBH.[VALUE_STRING_2], TBH.[VALUE_STRING_3], TBP.[SEAL_NO]
	, FromBranch.CompanyName AS FromBranchName, Consignee.CompanyName AS ConsigneeName
	, R.ShipperPk, R.ConsigneePk, R.ShipperCode, R.ConsigneeCode, R.DepartureDate, R.ArrivalDate, R.DepartureRegionCode, R.ArrivalRegionCode, R.TransportWayCL, R.DocumentRequestCL, R.MonetaryUnitCL, R.StepCL, R.DocumentStepCL, R.ShipperSignID, R.ShipperSignDate, R.ConsigneeSignID, R.ConsigneeSignDate 
	, DRC.Name AS DRCName
	, R.TotalPackedCount, R.PackingUnit, R.TotalGrossWeight, R.TotalVolume
	, CD.CommercialDocumentHeadPk, CD.BLNo AS HOUSEBL, CD.InvoiceNo, CD.Shipper, CD.ShipperAddress, CD.Consignee, CD.ConsigneeAddress, CD.NotifyParty, CD.NotifyPartyAddress, CD.PortOfLoading, CD.FinalDestination, CD.Carrier, CD.SailingOn, CD.PaymentTerms, CD.OtherReferences, CD.StampImg, CD.FOBorCNF, CD.VoyageNo, CD.VoyageCompany, CD.ContainerNo, CD.SealNo, CD.ContainerSize, CD.Registerd, CD.ClearanceDate, CD.StepCL AS CDStepCL
	, H.HighlighterPk, H.Color 
FROM [dbo].[STORAGE] AS OBS 
	left join OurBranchStorageCode AS OBSC ON OBS.[WAREHOUSE_PK]=OBSC.OurBranchStoragePk
	left join [dbo].[TRANSPORT_HEAD] AS TBH ON TBH.TRANSPORT_PK=OBS.[TRANSPORT_HEAD_PK] 
	left join Company AS FromBranch ON FromBranch.CompanyPk=TBH.[BRANCHPK_FROM] 
	left join RequestForm AS R ON OBS.[REQUEST_PK]=R.RequestFormPk 
	left join Company AS Consignee ON Consignee.CompanyPk=R.ConsigneePk 
	left join RegionCode AS DRC ON R.DepartureRegionCode=DRC.RegionCode 
	left join CommerdialConnectionWithRequest AS CCWR ON CCWR.RequestFormPk=OBS.[REQUEST_PK]
	left join CommercialDocument AS CD ON CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk 
	left join [dbo].[TRANSPORT_PACKED] AS TBP ON TBH.[TRANSPORT_PK] = TBP.[TRANSPORT_HEAD_PK]
	left join (
		SELECT [HighlighterPk], [GubunPk], [Color] FROM Highlighter WHERE GubunCL=0 
	) AS H ON CCWR.CommercialDocumentPk=H.GubunPk 
WHERE TBH.[TRANSPORT_STATUS]=2 and TBH.BRANCHPK_TO=@BranchPk and  R.DocumentStepCL in (1, 2, 5, 6, 7, 8, 9, 10, 11, 12) " + QueryWhere + @"

union All 

SELECT OBS.StorageCode, OBS.RequestFormPk, OBS.BoxCount, OBS.TransportBetweenBranchPk
	, OBSC.OurBranchStoragePk, OBSC.OurBranchCode, OBSC.StorageName , OBSC.StorageAddress, OBSC.TEL, OBSC.FAX 
	, TBH.[TRANSPORT_WAY], TBH.[VALUE_STRING_0], TBH.[BRANCHPK_FROM], TBH.[BRANCHPK_TO], TBH.[DATETIME_FROM], TBH.[DATETIME_TO], TBH.[VESSELNAME] 
	, TBH.[VOYAGE_NO], TBH.[VALUE_STRING_1], TBH.[VALUE_STRING_2], TBH.[VALUE_STRING_3], TBP.[SEAL_NO]
	, FromBranch.CompanyName AS FromBranchName, Consignee.CompanyName AS ConsigneeName
	, R.ShipperPk, R.ConsigneePk, R.ShipperCode, R.ConsigneeCode, R.DepartureDate, R.ArrivalDate, R.DepartureRegionCode, R.ArrivalRegionCode, R.TransportWayCL, R.DocumentRequestCL, R.MonetaryUnitCL, R.StepCL, R.DocumentStepCL, R.ShipperSignID, R.ShipperSignDate, R.ConsigneeSignID, R.ConsigneeSignDate 
	, DRC.Name AS DRCName
	, R.TotalPackedCount, R.PackingUnit, R.TotalGrossWeight, R.TotalVolume
	, CD.CommercialDocumentHeadPk, CD.BLNo AS HOUSEBL, CD.InvoiceNo, CD.Shipper, CD.ShipperAddress, CD.Consignee, CD.ConsigneeAddress, CD.NotifyParty, CD.NotifyPartyAddress, CD.PortOfLoading, CD.FinalDestination, CD.Carrier, CD.SailingOn, CD.PaymentTerms, CD.OtherReferences, CD.StampImg, CD.FOBorCNF, CD.VoyageNo, CD.VoyageCompany, CD.ContainerNo, CD.SealNo, CD.ContainerSize, CD.Registerd, CD.ClearanceDate, CD.StepCL AS CDStepCL
	, H.HighlighterPk, H.Color 
FROM OurBranchStorageOut AS OBS 
	left join OurBranchStorageCode AS OBSC ON OBS.StorageCode=OBSC.OurBranchStoragePk
	left join [dbo].[TRANSPORT_HEAD] AS TBH ON TBH.[TRANSPORT_PK]=OBS.TransportBetweenBranchPk 
	left join Company AS FromBranch ON FromBranch.CompanyPk=TBH.[BRANCHPK_FROM] 
	left join RequestForm AS R ON OBS.RequestFormPk=R.RequestFormPk 
	left join Company AS Consignee ON Consignee.CompanyPk=R.ConsigneePk 
	left join RegionCode AS DRC ON R.DepartureRegionCode=DRC.RegionCode 
	left join CommerdialConnectionWithRequest AS CCWR ON CCWR.RequestFormPk=OBS.RequestFormPk
	left join CommercialDocument AS CD ON CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk 
	left join [dbo].[TRANSPORT_PACKED] AS TBP ON TBH.[TRANSPORT_PK] = TBP.[TRANSPORT_HEAD_PK]
	left join (
		SELECT [HighlighterPk], [GubunPk], [Color] FROM Highlighter WHERE GubunCL=0 
	) AS H ON CCWR.CommercialDocumentPk=H.GubunPk 
WHERE OBSC.OurBranchCode=@BranchPk and R.DocumentStepCL in (1, 2, 5, 6, 7, 8, 9, 10, 11, 12) " + QueryWhere + @"


) COMMUNITY
ORDER BY COMMUNITY.[DATETIME_TO] DESC, COMMUNITY.[TRANSPORT_HEAD_PK] ASC, COMMUNITY.DocumentStepCL DESC, COMMUNITY.[VALUE_STRING_0] ASC ;";
		string tempStorageCode = "";
		string tempTransportBetweenBranchPk = "";
		object[] rowValue = new object[18];
		string tempRequestFormPk = "";
		string tempStyle;
		string tempCommercialDocumentHeadPk = "";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder DeliveryList = new StringBuilder();

		string tempStorageTitle = "";
		string tempContainerTitle = "";
		while (RS.Read()) {
			if (StorageCode == "0") {
				if (tempStorageCode != RS["StorageCode"] + "") {
					tempStorageCode = RS["StorageCode"] + "";
					tempStorageTitle = String.Format(EachGroup, "<span style=\"font-size:20px; font-weight:bold; padding-left:130px; \">" + RS["StorageName"] + "</span></a> " + RS["StorageAddress"] + " TEL : " + RS["TEL"] + " FAX : " + RS["FAX"]);
				} else {
					tempStorageTitle = "";
				}
			}
			//컨테이너 타이틀
			if (tempTransportBetweenBranchPk != RS["TRANSPORT_HEAD_PK"] + "") {
				string TempTitle;
				switch (RS["TransportCL"] + "") {
					case "1":
						TempTitle = "<span style=\"font-size:15px; font-weight:bold; padding-left:50px; \">" +
							"AIR From " + RS["FromBranchName"] + " " + (RS["DATETIME_FROM"] + "").Substring(5, 5) + "~" + (RS["DATETIME_TO"] + "").Substring(5, 5) + "</span>";
						break;
					case "2":
						TempTitle = "<span style=\"font-size:15px; font-weight:bold; padding-left:50px; \">" +
							"Car From " + RS["FromBranchName"] + " " + (RS["DATETIME_FROM"] + "").Substring(5, 5) + "~" + (RS["DATETIME_TO"] + "").Substring(5, 5) + "</span>";
						break;
					case "3":
						string companyname = RS["FromBranchName"] + "" == "" ? "" : (RS["FromBranchName"] + "").Substring(0, 2);
						TempTitle = "<span style=\"font-size:15px; font-weight:bold; padding-left:50px; \">" +
							"Ship From " + companyname + " " + (RS["DATETIME_TO"] + "").Substring(5, 5) + "</span>&nbsp;" + RS["VALUE_STRING_0"] + " " + RS["VOYAGE_NO"] + " " + RS["SEAL_NO"];
						break;
					case "4":
						TempTitle = "<span style=\"font-size:15px; font-weight:bold; padding-left:50px; \">" +
							"OtherComapny From " + RS["FromBranchName"] + " " + (RS["DATETIME_FROM"] + "").Substring(5, 5) + "~" + (RS["DATETIME_TO"] + "").Substring(5, 5) + "</span>";
						break;
					default:
						TempTitle = "??";
						break;
				}
				tempTransportBetweenBranchPk = RS["TRANSPORT_HEAD_PK"] + "";
				tempContainerTitle = String.Format(EachGroup, TempTitle);
			} else {
				tempContainerTitle = "";
			}
			//컨테이너 타이틀

			if (tempRequestFormPk != RS["REQUEST_PK"] + "") {
				if (tempRequestFormPk != "") {
					ReturnValue.Append(String.Format(EachRow, rowValue));
				}
				ReturnValue.Append(tempStorageTitle + tempContainerTitle);

				tempRequestFormPk = RS["REQUEST_PK"] + "";
				rowValue = new object[18];
				rowValue[0] = RS["ShipperPk"] + "";
				rowValue[1] = RS["ConsigneePk"] + "";
				rowValue[2] = RS["CommercialDocumentHeadPk"] + "";
				rowValue[3] = tempRequestFormPk;
				rowValue[4] = RS["DRCName"] + "";
				rowValue[5] = RS["ShipperCode"] + "";
				rowValue[6] = RS["ConsigneeCode"] + "";
				rowValue[7] = RS["ArrivalDate"] + "" == "" ? "&nbsp;" : string.Format("{0}/{1}", (RS["ArrivalDate"] + "").Substring(4, 2), (RS["ArrivalDate"] + "").Substring(6, 2));
				rowValue[8] = RS["HOUSEBL"] + "";
				rowValue[9] = RS["ConsigneeName"] + "";
				rowValue[10] = RS["TotalPackedCount"] + " " + Common.GetPackingUnit(RS["PackingUnit"] + "");
				rowValue[11] = Common.NumberFormat(RS["TotalGrossWeight"] + "");
				rowValue[12] = Common.NumberFormat(RS["TotalVolume"] + "");
				rowValue[13] = string.Format(INPUTBL, RS["CommercialDocumentHeadPk"] + "");
				if (RS["HighlighterPk"] + "" == "") {
					rowValue[14] = "onclick=\"setHighlight('0', 'N', '" + RS["CommercialDocumentHeadPk"] + "');\"";
				} else {
					switch (RS["Color"] + "") {
						case "0":
							rowValue[14] = "onclick=\"setHighlight('0', 'A', '" + RS["HighlighterPk"] + "');\" style=\"background-color:#87CEFA;\" ";
							break;
						case "1":
							rowValue[14] = "onclick=\"setHighlight('0', 'D', '" + RS["HighlighterPk"] + "');\" style=\"background-color:#E9967A;\" ";
							break;
					}
				}

				string CDStepCL = "&nbsp;";
				if (tempCommercialDocumentHeadPk != RS["CommercialDocumentHeadPk"] + "") {
					tempCommercialDocumentHeadPk = RS["CommercialDocumentHeadPk"] + "";
					switch (RS["DocumentStepCL"] + "") {
						case "1":
							CDStepCL = "BL생성";
							break;
						case "2":
							CDStepCL = "관부가세 책정중";
							break;	//통관비 확정
						case "3":
							CDStepCL = "자가통관";
							break;
						case "4":
							CDStepCL = "Sample";
							break;
						case "5":
							ChkCount++;
							CDStepCL = "<label for=\"CBEachBL[" + ChkCount + "]\" >신고대기</label>&nbsp;<input type=\"checkbox\" id=\"CBEachBL[" + ChkCount + "]\" value=\"" + RS["CommercialDocumentHeadPk"] + "\" />";
							break;	//통관비 확정
						case "6":
							ChkCount++;
							CDStepCL = "<label for=\"CBEachBL[" + ChkCount + "]\" >신고해주세요!</label>&nbsp;<input type=\"checkbox\" id=\"CBEachBL[" + ChkCount + "]\" value=\"" + RS["CommercialDocumentHeadPk"] + "\" />";
							break;
						case "7":
							ChkCount++;
							CDStepCL = "<label for=\"CBEachBL[" + ChkCount + "]\" >검사생략</label>&nbsp;<input type=\"checkbox\" id=\"CBEachBL[" + ChkCount + "]\" value=\"" + RS["CommercialDocumentHeadPk"] + "\" />";
							break;
						case "8":
							ChkCount++;
							CDStepCL = "<label for=\"CBEachBL[" + ChkCount + "]\" >서류제출</label>&nbsp;<input type=\"checkbox\" id=\"CBEachBL[" + ChkCount + "]\" value=\"" + RS["CommercialDocumentHeadPk"] + "\" />";
							break;
						case "9":
							ChkCount++;
							CDStepCL = "<label for=\"CBEachBL[" + ChkCount + "]\" >세관검사</label>&nbsp;<input type=\"checkbox\" id=\"CBEachBL[" + ChkCount + "]\" value=\"" + RS["CommercialDocumentHeadPk"] + "\" />";
							break;
						case "10":
							ChkCount++;
							CDStepCL = "<label for=\"CBEachBL[" + ChkCount + "]\" >세금납부해주세요!</label>&nbsp;<input type=\"checkbox\" id=\"CBEachBL[" + ChkCount + "]\" value=\"" + RS["CommercialDocumentHeadPk"] + "\" />";
							break;
						case "11":
							ChkCount++;
							CDStepCL = "<label for=\"CBEachBL[" + ChkCount + "]\" >세금납부해주세요!</label>&nbsp;<input type=\"checkbox\" id=\"CBEachBL[" + ChkCount + "]\" value=\"" + RS["CommercialDocumentHeadPk"] + "\" />";
							break;
						case "12":
							ChkCount++;
							CDStepCL = "<label for=\"CBEachBL[" + ChkCount + "]\" >세금납부해주세요!</label>&nbsp;<input type=\"checkbox\" id=\"CBEachBL[" + ChkCount + "]\" value=\"" + RS["CommercialDocumentHeadPk"] + "\" />";
							break;
						case "13":
							CDStepCL = "면허완료";
							break;
						case "14":
							CDStepCL = "면허완료";
							break;
						case "15":
							CDStepCL = "면허완료";
							break;
						default:
							CDStepCL = "미확정";
							break;
					}
				}
				rowValue[15] = CDStepCL;
			}
		}
		ReturnValue.Append(String.Format(EachRow, rowValue));
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue + "";
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Admin_PrepareDelivery : System.Web.UI.Page
{
	protected String[] MEMBERINFO;
	protected String Gubun;
	protected Int32 PageNo;
	protected Int32 TotalRecord;
	protected String StorageCode;
	protected String HTMLHeader;
	protected String HTMLBody;
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

		StorageCode = Request.Params["S"] + "" == "" ? "0" : Request.Params["S"] + "";
		PageNo = Request.Params["PageNo"] + "" == "" ? 1 : Int32.Parse(Request.Params["PageNo"] + "");
		DB = new DBConn();

		if (MEMBERINFO[0] == "Customs") {
			LogedWithoutRecentRequest111.Visible = false;
			Loged1.Visible = true;
			HTMLHeader = "";
			HTMLBody = LoadBodyForCustoms(StorageCode, MEMBERINFO[1], PageNo);
		} else {
			HTMLHeader = LoadHeader(StorageCode, MEMBERINFO[1]);
			HTMLBody = LoadBody(StorageCode, MEMBERINFO[1], PageNo);
		}
	}
	private String LoadHeader(string StorageCode, string CompanyPk)
	{
		string EachHeadBTN = "<input type=\"button\" {0} value=\"{1}\" onclick=\"GotoDifferantStorage('{2}');\" />&nbsp;&nbsp;";
		StringBuilder ReturnValue = new StringBuilder();
		string BTNToday = string.Format(EachHeadBTN, StorageCode == "0" ? "style=\"font-weight:bold;\"" : "", "TOTAL", "0");

		DB.SqlCmd.CommandText = @"	SELECT OBSC.OurBranchStoragePk, OBSC.StorageName, Ct.C 
															FROM OurBranchStorageCode AS OBSC left join ( 
																SELECT StorageCode, count(*) AS C 
																FROM OurBranchStorageOut 
																WHERE StatusCL<6 
																group by StorageCode 
															) AS Ct ON OBSC.OurBranchStoragePk=Ct.StorageCode 
															WHERE OBSC.OurBranchCode=" + CompanyPk + " and isnull(Ct.C, 0)<>0;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			string tempStyle = StorageCode == RS[0] + "" ? "style=\"font-weight:bold;\"" : "";
			ReturnValue.Append(string.Format(EachHeadBTN, tempStyle, RS[1] + "(" + RS[2] + ")", RS[0]));
		}
		RS.Dispose();
		DB.DBCon.Close();
		return "<div>" + BTNToday + "</div>" + ReturnValue;
	}
	private String LoadBody(string StorageCode, string CompanyPk, int PageNo)
	{
		int EachPageRowCount = 15;
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("	<table border='0' cellpadding='0' cellspacing='0' style='width:1050px;' ><thead><tr style=\"height:40px;\">" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; width:40px; \" >F</td>" +
										"		<td class='THead1' style='width:125px;'>BLNo</td>" +
										"		<td class='THead1' style='width:55px;' >s</td>" +
										"		<td class='THead1' style='width:55px;' >c</td>" +
										"		<td class='THead1' style='width:43px;'>Arrival</td>" +
										"		<td class='THead1' style='width:200px;'>CompanyName</td>" +
										"		<td class='THead1' style='width:100px;'>CT / Total</td>" +
										"		<td class='THead1' style='width:50px;'>Weight</td>" +
										"		<td class='THead1' style='width:50px;'>Volume</td>" +
										"		<td class='THead1' style='width:25px;'>SP</td>" +
										"		<td class='THead1' >CP</td>" +
										"		<td class='THead1' style='width:180px;'>출고지</td>" +
										"		<td class='THead1' style='width:75px;'>세관</td>" +
										"	</tr></thead>");
		string EachGroup = "	<tr style=\"height:20px;\"><td class='TBody1' style=\"text-align:left; \" colspan='13'  >{0}</td></tr>";
		string EachRow = "	<tr style=\"height:20px;\">" +
									"		<td class='TBody1' style=\"text-align:center; \" ><a onclick=\"Goto('CheckDescription','{2}');\">{4}</a></td>" +
									"		<td class='TBody1' ><a onclick=\"Goto('CheckDescription','{2}');\">{8}</a></td>" +
									"		<td class='TBody1' ><a onclick=\"Goto('Company', '{0}');\">{5}</a></td>" +
									"		<td class='TBody1' ><a onclick=\"Goto('Company', '{1}');\">{6}</a></td>" +
									"		<td class='TBody1' >{7}</td>" +
									"		<td class='TBody1' ><a onclick=\"Goto('RequestForm', '{3}');\"><span style=\"cursor:hand; font-weight:bold;\">{9}</span></a></td>" +
									"		<td class='TBody1' {17} >{10}</td>" +
									"		<td class='TBody1' {17} >{11}</td>" +
									"		<td class='TBody1' {17} >{12}</td>" +
									"		<td class='TBody1' >{13}</td>" +
									"		<td class='TBody1' >{14}</td>" +
									"		<td class='TBody1' style=\"text-align:left;\" >{16}</td>" +
									"		<td class='TBody1' >{15}</td>" +
									"	</tr>";
		if (StorageCode == "0") {
			DB.SqlCmd.CommandText = @"
	declare @AA int;
SELECT @AA=count(*)
FROM OurBranchStorageOut AS OBS 
	left join OurBranchStorageCode AS OBSC ON OBS.StorageCode=OBSC.OurBranchStoragePk
	left join TransportBBMain AS TBH ON TBH.TBBPk=OBS.TransportBetweenBranchPk 
	left join RequestForm AS R On OBS.RequestFormPk=R.RequestFormPk
WHERE OBS.StatusCL>3 and OBSC.OurBranchCode=" + CompanyPk + @" and OBS.BoxCount>0 and ( R.DocumentStepCL in (5, 6, 7, 8, 9, 10, 11, 12) or isnull(OBS.StatusCL, 0)<6 )
group by TBH.TBBPk; 
SELECT @@RowCount;";
		} else {
			DB.SqlCmd.CommandText = @"
	declare @aa int; 
	SELECT @aa=count(*)
	FROM OurBranchStorageOut AS OBS 
		left join OurBranchStorageCode AS OBSC ON OBS.StorageCode=OBSC.OurBranchStoragePk 
		left join TransportBBMain AS TBH ON TBH.TBBPk=OBS.TransportBetweenBranchPk 
		left join RequestForm AS R On OBS.RequestFormPk=R.RequestFormPk
	WHERE OBS.StatusCL>3 and OBS.StorageCode=" + StorageCode + @" and OBS.BoxCount>0 and ( R.DocumentStepCL in (5, 6, 7, 8, 9, 10, 11, 12) or isnull(OBS.StatusCL, 0)<6 )
	GROUP BY OBS.TransportBetweenBranchPk ;	
	SELECT @@ROWCOUNT; ";
		}
		DB.DBCon.Open();
		TotalRecord = Int32.Parse(DB.SqlCmd.ExecuteScalar() + "");
		//Response.Write(TotalRecord + "_" + StorageCode);

		if (StorageCode == "0")
			DB.SqlCmd.CommandText = "EXEC SP_PrepareDeliveryListByBranchPk @BranchPk=" + CompanyPk + ";";
		else
			DB.SqlCmd.CommandText = "EXEC SP_PrepareDeliveryListByStorageCode @StorageCode=" + StorageCode + ";";

		//DB.SqlCmd.CommandText = " EXEC SP_LoadPrepareDelivery @CompanyPk=" + CompanyPk + ", @Gubun=" + StorageCode + ";";
		//Response.Write(DB.SqlCmd.CommandText);
		string tempStorageCode = "";
		string tempTransportBetweenBranchPk = "";
		object[] rowValue = new object[18];
		string tempRequestFormPk = "";
		string tempCommercialDocumentHeadPk = "";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		int rowcount = 0;

		StringBuilder DeliveryList = new StringBuilder();

		string tempStorageTitle = "";
		string tempContainerTitle = "";
		bool isDeposited = true;
		bool isContinue = true;
		int nowTempPage = 1;

		while (true) {
			if (PageNo == nowTempPage) {
				break;
			}

			int temprowcount = 0;
			while (RS.Read()) {
				if (tempTransportBetweenBranchPk != RS["TransportBetweenBranchPk"] + "") {
					tempTransportBetweenBranchPk = RS["TransportBetweenBranchPk"] + "";
					temprowcount++;

					if (temprowcount == EachPageRowCount) {
						nowTempPage++;
						if (PageNo == nowTempPage) {
							while (RS.Read()) {
								if (tempTransportBetweenBranchPk == RS["TransportBetweenBranchPk"] + "") {
									continue;
								}
								break;
							}
							isContinue = false;
						} else {
							isContinue = true;
						}
						break;
					}
				}
			}
			if (isContinue) {
				continue;
			} else {
				break;
			}
		}

		bool check = false;
		if (PageNo == 1) {
			if (RS.Read()) {
				check = true;
			}
		} else {
			check = true;
		}

		while (check) {
			//창고 타이틀
			if (StorageCode == "0") {
				if (tempStorageCode != RS["StorageCode"] + "") {
					tempStorageCode = RS["StorageCode"] + "";
					tempStorageTitle = String.Format(EachGroup, "<a onclick=\"Goto('Storage','" + tempStorageCode + "');\"><span style=\"font-size:20px; font-weight:bold; cursor:hand; padding-left:130px; \">" + RS["StorageName"] + "</span></a>");
				} else {
					tempStorageTitle = "";
				}
			}
			//컨테이너 타이틀
			if (tempTransportBetweenBranchPk != RS["TransportBetweenBranchPk"] + "") {
				rowcount++;
				if (rowcount > EachPageRowCount) {
					break;
				}

				string TempTitle;
				String[] Description = (RS["Description"] + "").Split(Common.Splite321, StringSplitOptions.None);
				tempTransportBetweenBranchPk = RS["TransportBetweenBranchPk"] + "";
				switch (RS["TransportCL"] + "") {
					case "1":
						TempTitle = "<a onclick=\"Goto('TBBPk','" + tempTransportBetweenBranchPk + "');\"><span style=\"font-size:15px; font-weight:bold; cursor:hand; padding-left:50px; \">" +
							"AIR From " + RS["FromBranchName"] + " " + (RS["FromDateTime"] + "").Substring(5, 5) + "~" + (RS["ToDateTime"] + "").Substring(5, 5) + "</span></a>";
						break;
					case "2":
						TempTitle = "<a onclick=\"Goto('TBBPk','" + tempTransportBetweenBranchPk + "');\"><span style=\"font-size:15px; font-weight:bold; cursor:hand; padding-left:50px; \">" +
							"Car From " + RS["FromBranchName"] + " " + (RS["FromDateTime"] + "").Substring(5, 5) + "~" + (RS["ToDateTime"] + "").Substring(5, 5) + "</span></a>";
						break;
					case "3":
						string companyname = RS["FromBranchName"] + "" == "" ? "" : (RS["FromBranchName"] + "").Substring(0, 2);
						TempTitle = "<a onclick=\"Goto('TBBPk','" + tempTransportBetweenBranchPk + "');\"><span style=\"font-size:15px; font-weight:bold; cursor:hand; padding-left:50px; \">" +
							"Ship From " + companyname + " " + (RS["ToDateTime"] + "").Substring(5, 5) + "</span></a>&nbsp;" + Description[0] + " " + Description[5] + " " + Description[7];
						break;
					case "4":
						TempTitle = "<a onclick=\"Goto('TBBPk','" + tempTransportBetweenBranchPk + "');\"><span style=\"font-size:15px; font-weight:bold; cursor:hand; padding-left:50px; \">" +
							"HandCarry From " + RS["FromBranchName"] + " " + (RS["FromDateTime"] + "").Substring(5, 5) + "~" + (RS["ToDateTime"] + "").Substring(5, 5) + "</span></a>";
						break;
					default:
						TempTitle = "??";
						break;
				}

				tempContainerTitle = String.Format(EachGroup, TempTitle);
			} else {
				tempContainerTitle = "";
			}
			//컨테이너 타이틀

			if (tempRequestFormPk != RS["RequestFormPk"] + "") {
				isDeposited = true;
				if (tempRequestFormPk != "") {
					rowValue[16] = DeliveryList + "";
					ReturnValue.Append(String.Format(EachRow, rowValue));
				}
				ReturnValue.Append(tempStorageTitle + tempContainerTitle);
				DeliveryList = new StringBuilder();

				tempRequestFormPk = RS["RequestFormPk"] + "";
				rowValue = new object[18];
				rowValue[0] = RS["ShipperPk"] + "";
				rowValue[1] = RS["ConsigneePk"] + "";
				rowValue[2] = RS["CommercialDocumentHeadPk"] + "";
				rowValue[3] = tempRequestFormPk;
				rowValue[4] = RS["DRCName"] + "";
				rowValue[5] = RS["ShipperCode"] + "";
				rowValue[6] = RS["ConsigneeCode"] + "";
				rowValue[7] = RS["ArrivalDate"] + "" == "" ? "미지정" : string.Format("{0}/{1}", (RS["ArrivalDate"] + "").Substring(4, 2), (RS["ArrivalDate"] + "").Substring(6, 2));
				rowValue[8] = RS["HOUSEBL"] + "";
				rowValue[9] = RS["ConsigneeName"] + "";
				rowValue[10] = RS["BoxCount"] + " / " + RS["TotalPackedCount"] + " " + Common.GetPackingUnit(RS["PackingUnit"] + "");
				rowValue[11] = Common.NumberFormat(RS["TotalGrossWeight"] + "");
				rowValue[12] = Common.NumberFormat(RS["TotalVolume"] + "");

				if (RS["ShipperCharge"] + "" == "0.0000") {
					rowValue[13] = "--";
				} else if (RS["ShipperDepositedDate"] + "" == "") {
					rowValue[13] = "<span onclick=\"CollectPayment('" + RS["RequestFormPk"] + "', '" + MEMBERINFO[2] + "', 'S', '" + RS["ShipperPk"] + "');\" style=\"cursor:hand;\" >" +
											"		<img src=\"../Images/CheckFalse.jpg\" style=\"width:20px;\" alt=\"\" />" +
											"	</span>";
					isDeposited = false;
				} else {
					rowValue[13] = "<span onclick=\"CollectPayment('" + RS["RequestFormPk"] + "', '" + MEMBERINFO[2] + "', 'S', '" + RS["ShipperPk"] + "');\" style=\"cursor:hand;\" >" +
											"		<img src=\"../Images/CheckTrue.jpg\" style=\"width:20px;\" alt=\"\" /></span>";
				}

				if (RS["ConsigneeCharge"] + "" == "0.0000") {
					rowValue[14] = "--";
				} else if (RS["ConsigneeDepositedDate"] + "" == "") {
					rowValue[14] = "<span onclick=\"CollectPayment('" + RS["RequestFormPk"] + "', '" + MEMBERINFO[2] + "', 'C', '" + RS["ConsigneePk"] + "');\" style=\"cursor:hand;\" >" +
											"		<img src=\"../Images/CheckFalse.jpg\" style=\"width:20px;\" alt=\"\" /></span>";
					isDeposited = false;
				} else {
					rowValue[14] = "	<span onclick=\"CollectPayment('" + RS["RequestFormPk"] + "', '" + MEMBERINFO[2] + "', 'C', '" + RS["ConsigneePk"] + "');\" style=\"cursor:hand;\" >" +
											"		<img src=\"../Images/CheckTrue.jpg\" style=\"width:20px;\" alt=\"\" /></span>";
				}

				string CDStepCL;
				if (tempCommercialDocumentHeadPk != RS["CommercialDocumentHeadPk"] + "") {
					tempCommercialDocumentHeadPk = RS["CommercialDocumentHeadPk"] + "";
					switch (RS["DocumentStepCL"] + "") {
						case "1":
							CDStepCL = "BL생성";
							break;
						case "2":
							CDStepCL = "세금책정중";
							break;	//통관비 확정
						case "3":
							CDStepCL = "자가통관";
							break;
						case "4":
							CDStepCL = "Sample";
							break;
						case "5":
							CDStepCL = "<input type=\"button\" value=\"통관지시\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id, '6', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
							break;	//통관비 확정
						case "6":
							CDStepCL = "<input type=\"button\" value=\"략\" style=\"padding:0px;\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id, '7', '" + RS["CommercialDocumentHeadPk"] + "')\" />" +
					"<input type=\"button\" value=\"서\" style=\"padding:0px;\" id=\"BTN" + tempCommercialDocumentHeadPk + "0\" onclick=\"DocumentStepCLTo(this.id, '8', '" + RS["CommercialDocumentHeadPk"] + "')\" />" +
					"<input type=\"button\" value=\"검\" style=\"padding:0px;\" id=\"BTN" + tempCommercialDocumentHeadPk + "00\" onclick=\"DocumentStepCLTo(this.id, '9', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
							break;	//통관지시
						case "7":
							CDStepCL = "생략 <input type=\"button\" value=\"세납\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id, '10', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
							break;
						case "8":
							CDStepCL = "서류 <input type=\"button\" value=\"세납\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id, '11', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
							break;
						case "9":
							CDStepCL = "검사 <input type=\"button\" value=\"세납\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id, '12', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
							break;
						case "10":
							CDStepCL = "세납 <input type=\"button\" value=\"면허\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id,'13', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
							break;
						case "11":
							CDStepCL = "세납 <input type=\"button\" value=\"면허\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id,'14', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
							break;
						case "12":
							CDStepCL = "세납 <input type=\"button\" value=\"면허\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id,'15', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
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
				} else {
					CDStepCL = "``";
				}
				rowValue[15] = CDStepCL;
				if (RS["HighlighterPk"] + "" == "") {
					rowValue[17] = "onclick=\"setHighlight('1', 'N', '" + RS["CommercialDocumentHeadPk"] + "');\"";
				} else {
					switch (RS["Color"] + "") {
						case "0":
							rowValue[17] = "onclick=\"setHighlight('1', 'D', '" + RS["HighlighterPk"] + "');\" style=\"background-color:#87CEFA;\" ";
							break;
						case "1":
							rowValue[17] = "onclick=\"setHighlight('1', 'D', '" + RS["HighlighterPk"] + "');\" style=\"background-color:#E9967A;\" ";
							break;
					}
				}
			}

			if (RS["TransportBetweenCompanyPk"] + "" != "") {
				if (RS["Type"] + "" == "") {
					DeliveryList.Append("<input type=\"button\" value=\"P\" style=\"padding:0px;\" onclick=\"DeliveryPrint(" + RS["TransportBetweenCompanyPk"] + ");\" />&nbsp;" +
						"<span style=\"cursor:hand;\" onclick=\"DeliveryModify('" + RS["OurBranchStorageOutPk"] + "','" + RS["RequestFormPk"] + "' , '" + RS["ConsigneePk"] + "', '" + MEMBERINFO[1] + "');\" >  " +
						RS["PackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "") + " : " +
						((RS["WarehouseInfo"] + "").Substring(0, (RS["WarehouseInfo"] + "").IndexOf("@@")).Length > 18 ? (RS["WarehouseInfo"] + "").Substring(0, 18) + "..." : (RS["WarehouseInfo"] + "").Substring(0, (RS["WarehouseInfo"] + "").IndexOf("@@"))) + "</span>	<br />");
				} else if (RS["StatusCL"] + "" == "6") {
					DeliveryList.Append("&nbsp;<span style=\"color:green;\">출고완료</span>");
				} else {
					string tempisdeposited = isDeposited ? "T" : "F";
					DeliveryList.Append("&nbsp;<span style=\"cursor:hand;\" onclick=\"DeliveryModify('" + RS["OurBranchStorageOutPk"] + "','" + RS["RequestFormPk"] + "' , '" + RS["ConsigneePk"] + "', '" + MEMBERINFO[1] + "');\" >  " +
													RS["PackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "") + " : " +
													"<strong>" + (RS["FromDate"] + "" == "" ? "" : (RS["FromDate"] + "").Substring(4, 4)) + "</strong> " + ((RS["Title"] + "").Length > 9 ? (RS["Title"] + "").Substring(0, 8) : RS["Title"] + "") +
													"</span>" +
													"	<input type=\"button\" value=\"出\" style=\"padding:0px;\" onclick=\"GoDeliveryOrder('" + RS["TransportBetweenCompanyPk"] + "' , '" + RS["RequestFormPk"] + "', '" + RS["ConsigneePk"] + "', '" + MEMBERINFO[1] + "');\" /><br />");
				}
			} else {
				DeliveryList.Append("&nbsp;<input type=\"button\" value=\"add\" onclick=\"DeliverySet('" + RS["OurBranchStorageOutPk"] + "' , '" + RS["RequestFormPk"] + "', '" + RS["ConsigneePk"] + "', '" + MEMBERINFO[1] + "');\" /><br />");
			}

			if (RS.Read()) {
				continue;
			} else {
				break;
			}
		}
		rowValue[16] = DeliveryList + "";
		ReturnValue.Append(String.Format(EachRow, rowValue));
		RS.Dispose();
		DB.DBCon.Close();
		ReturnValue.Append("<TR Height='20px'><td colspan='13' style='background-color:#F5F5F5; text-align:center; padding:20px; '>" + new Common().SetPageListByNo(EachPageRowCount, PageNo, TotalRecord, "PrepareDelivery.aspx", "?S=" + StorageCode + "&") + "</TD></TR></Table>");
		return ReturnValue + "";
	}
	private String LoadBodyForCustoms(string StorageCode, string CompanyPk, int PageNo)
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
										"		<td class='THead1' style='width:60px;'>CT</td>" +
										"		<td class='THead1' style='width:60px;'>Weight</td>" +
										"		<td class='THead1' >Volume</td>" +
										"		<td class='THead1' style='width:120px;'>서류</td>" +
										"		<td class='THead1' style='width:90px;'>세관</td>" +
										"	</tr></thead>");
		string EachGroup = "	<tr style=\"height:20px;\"><td class='TBody1' style=\"text-align:left; \" colspan='11'  >{0}</td></tr>";
		string EachRow = "	<tr style=\"height:20px; \">" +
									"		<td class='TBody1' style=\"text-align:center; \" ><a onclick=\"Goto('CheckDescription','{2}');\">{4}</a></td>" +
									"		<td class='TBody1' ><a onclick=\"Goto('CheckDescription','{2}');\">{8}</a></td>" +
									"		<td class='TBody1' >{5}</td>" +
									"		<td class='TBody1' >{6}</td>" +
									"		<td class='TBody1' >{7}</td>" +
									"		<td class='TBody1' ><strong><a onclick=\"Goto('CheckDescription','{2}');\">{9}</a></strong></td>" +
									"		<td class='TBody1' {14}>{10}</td>" +
									"		<td class='TBody1' {14}>{11}</td>" +
									"		<td class='TBody1' {14}>{12}</td>" +
									"		<td class='TBody1' >{13}</td>" +
									"		<td class='TBody1' >{15}</td>" +
									"	</tr>";
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"EXECUTE SP_PrepareDeliveryListCustoms @BranchPk=" + CompanyPk + ";";
		//Response.Write(DB.SqlCmd.CommandText);
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
					tempStorageTitle = String.Format(EachGroup, "<span style=\"font-size:20px; font-weight:bold; padding-left:130px; \">" + RS["StorageName"] + "</span>");
				} else {
					tempStorageTitle = "";
				}
			}
			//컨테이너 타이틀
			if (tempTransportBetweenBranchPk != RS["TransportBetweenBranchPk"] + "") {
				string TempTitle;
				String[] Description = (RS["Description"] + "").Split(Common.Splite321, StringSplitOptions.None);
				switch (RS["TransportCL"] + "") {
					case "1":
						TempTitle = "<span style=\"font-size:15px; font-weight:bold; padding-left:50px; \">" +
							"AIR From " + RS["FromBranchName"] + " " + (RS["FromDateTime"] + "").Substring(5, 5) + "~" + (RS["ToDateTime"] + "").Substring(5, 5) + "</span>";
						break;
					case "2":
						TempTitle = "<span style=\"font-size:15px; font-weight:bold; padding-left:50px; \">" +
							"Car From " + RS["FromBranchName"] + " " + (RS["FromDateTime"] + "").Substring(5, 5) + "~" + (RS["ToDateTime"] + "").Substring(5, 5) + "</span>";
						break;
					case "3":
						string companyname = RS["FromBranchName"] + "" == "" ? "" : (RS["FromBranchName"] + "").Substring(0, 2);
						TempTitle = "<span style=\"font-size:15px; font-weight:bold; padding-left:50px; \">" +
							"Ship From " + companyname + " " + (RS["ToDateTime"] + "").Substring(5, 5) + "</span>&nbsp;" + Description[0] + " " + Description[5] + " " + Description[7];
						break;
					case "4":
						TempTitle = "<span style=\"font-size:15px; font-weight:bold; padding-left:50px; \">" +
							"HandCarry From " + RS["FromBranchName"] + " " + (RS["FromDateTime"] + "").Substring(5, 5) + "~" + (RS["ToDateTime"] + "").Substring(5, 5) + "</span>";
						break;
					default:
						TempTitle = "??";
						break;
				}
				tempTransportBetweenBranchPk = RS["TransportBetweenBranchPk"] + "";
				tempContainerTitle = String.Format(EachGroup, TempTitle);
			} else {
				tempContainerTitle = "";
			}
			//컨테이너 타이틀

			if (tempRequestFormPk != RS["RequestFormPk"] + "") {
				if (tempRequestFormPk != "") {
					ReturnValue.Append(String.Format(EachRow, rowValue));
				}
				ReturnValue.Append(tempStorageTitle + tempContainerTitle);

				tempRequestFormPk = RS["RequestFormPk"] + "";
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
							CDStepCL = "<input type=\"button\" value=\"세금확정\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id, '6', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
							break;	//통관비 확정
						case "6":
							CDStepCL = "통관 <input type=\"button\" value=\"략\" style=\"padding:0px;\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id, '7', '" + RS["CommercialDocumentHeadPk"] + "')\" />" +
														"<input type=\"button\" value=\"서\" style=\"padding:0px;\" id=\"BTN" + tempCommercialDocumentHeadPk + "0\" onclick=\"DocumentStepCLTo(this.id, '8', '" + RS["CommercialDocumentHeadPk"] + "')\" />" +
														"<input type=\"button\" value=\"검\" style=\"padding:0px;\" id=\"BTN" + tempCommercialDocumentHeadPk + "00\" onclick=\"DocumentStepCLTo(this.id, '9', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
							tempStyle = "Background-color:orange;";
							break;	//통관지시
						case "7":
							CDStepCL = "생략<input type=\"button\" value=\"면허\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id,'13', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
							break;
						case "8":
							CDStepCL = "제출<input type=\"button\" value=\"면허\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id,'14', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
							break;
						case "9":
							CDStepCL = "검사<input type=\"button\" value=\"면허\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id,'15', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
							break;
						case "10":
							CDStepCL = "세납 <input type=\"button\" value=\"면허\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id,'13', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
							tempStyle = "Background-color:green;";
							break;
						case "11":
							CDStepCL = "세납 <input type=\"button\" value=\"면허\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id,'14', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
							tempStyle = "Background-color:green;";
							break;
						case "12":
							CDStepCL = "세납 <input type=\"button\" value=\"면허\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id,'15', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
							tempStyle = "Background-color:green;";
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
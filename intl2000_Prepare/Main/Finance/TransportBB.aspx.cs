using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Text;
using System.Data.SqlClient;

public partial class Finance_TransportBB : System.Web.UI.Page {
	private DBConn DB;
	private SqlDataReader RS;
	protected String DepartureBranch;
	protected String ArrivalBranch;
	protected String TransportBBList;
	protected String StartDate;
	protected String EndDate;
	protected String Type;
	protected String Warehouse;
	protected String OptionDB;
	protected String OptionAB;
	protected String OptionT;
	protected String OptionWarehouse;
	protected String ArrivalPortchecked;
	protected String[] MemberInfo;

	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) {
			Response.Redirect("../Default.aspx");
		}
		MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		DepartureBranch = Request.Params["DB"] + "";
		ArrivalBranch = Request.Params["AB"] + "" == "" ? "3157" : Request.Params["AB"] + "";
		StartDate = Request.Params["SD"] + "";
		EndDate = Request.Params["ED"] + "";
		Type = Request.Params["T"] + "";
		Warehouse = Request.Params["W"] + "";
		ArrivalPortchecked = Request.Params["Y"] + "";

		DB = new DBConn();

		OptionWarehouse = LoadWarehouse(ArrivalBranch);
		OptionDB = LoadBranch(DepartureBranch);
		OptionAB = LoadBranch(ArrivalBranch);
		OptionT = LoadOptionT(Type);
		TransportBBList = LoadTransportBBList(DepartureBranch, ArrivalBranch, StartDate, EndDate, Type, Warehouse, ArrivalPortchecked);
	}
	private String LoadBranch(string BranchCode) {
		List<string[]> Branch = new List<string[]>();
		Branch.Add(new string[] { "3157", "KRIC 인천" });
		Branch.Add(new string[] { "3095", "JPOK Osaka" });
		Branch.Add(new string[] { "3843", "CNGZ 广州" });
		Branch.Add(new string[] { "2886", "CNYT	烟台" });
		Branch.Add(new string[] { "2887", "CNSY 瀋陽" });
		Branch.Add(new string[] { "2888", "CNYW	义乌" });
		Branch.Add(new string[] { "3388", "CNQD 青岛国际物流" });
		Branch.Add(new string[] { "11456", "CNHZ 杭州" });
		Branch.Add(new string[] { "7898", "CNSX	衢州" });
		Branch.Add(new string[] { "12437", "VTHM Hochimin" });
		Branch.Add(new string[] { "12438", "VTHN Hanoi" });
		Branch.Add(new string[] { "12527", "VTDN Danang" });
		Branch.Add(new string[] { "12464", "MMYG" });
		Branch.Add(new string[] { "3798", "OtherLocation" });

		StringBuilder ReturnValue = new StringBuilder();
		for (int i = 0; i < Branch.Count; i++) {
			if (BranchCode == Branch[i][0]) {
				ReturnValue.Append("<option value=\"" + Branch[i][0] + "\"  selected=\"selected\" >" + Branch[i][1] + "</option>");
			}
			else {
				ReturnValue.Append("<option value=\"" + Branch[i][0] + "\" >" + Branch[i][1] + "</option>");
			}
		}
		return ReturnValue.ToString();

	}
	private String LoadOptionT(string Type) {
		string[] Selected = new string[] { "", "", "", "", "" };
		int TypeIndex = 0;
		if (Type != "") {
			switch (Type) {
				case "":
					TypeIndex = 0;
					break;
				case "Air":
					TypeIndex = 1;
					break;
				case "Car":
					TypeIndex = 2;
					break;
				case "Ship":
					TypeIndex = 3;
					break;
				case "Sub":
					TypeIndex = 4;
					break;
			}
			Selected[TypeIndex] = " selected=\"selected\" ";
		}

		return "<option value=\"\" " + Selected[0] + ">ALL</option><option value=\"Air\" " + Selected[1] + ">AIR</option><option value=\"Car\" " + Selected[2] + ">CAR</option><option value=\"Ship\" " + Selected[3] + ">SHIP</option>" +
			"<option value=\"Sub\" " + Selected[4] + ">SubCompany</option>";
	}
	private String LoadWarehouse(string BranchPk) {
		StringBuilder Option = new StringBuilder();
		DB.SqlCmd.CommandText = "SELECT [OurBranchStoragePk], [StorageName] FROM [OurBranchStorageCode] WHERE [OurBranchCode]=" + BranchPk + "order by StorageName";
		DB.DBCon.Open();
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			if (Warehouse == RS[0] + "") {
				Option.Append("<option selected=\"selected\" value=\"" + RS[0] + "\">" + RS[1] + "</option>");
			} else {
				Option.Append("<option value=\"" + RS[0] + "\">" + RS[1] + "</option>");
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		return Option.ToString();
	}
	private String LoadTransportBBList(string DepartureBranch, string ArrivalBranch, string StartDate, string EndDate, string Type, string Warehouse, string ArrivalPortchecked) {
		string ROW = "<tr height=\"30px; \">" +
		"<td class=\"TBody1\">{4}</td>" +
		"<td style=\"text-align:center;\" class=\"TBody1\" >{0}</td>" +
		"<td class=\"TBody1\" style=\"text-align:left;\" >{1}</td>" +
		"<td class=\"TBody1\" style=\"text-align:center;\" >{13}</td>" +
		"<td class=\"TBody1\" style=\"text-align:center;\" >{2}</td>" +
		"<td class=\"TBody1\" style=\"text-align:center;\" ><a href=\"/Transport/TransportView.aspx?S={7}\"> {3}</td>" +
		"<td class=\"TBody1\" style=\"text-align:center;\">{5}</td>" +
		"<td class=\"TBody1\" style=\"text-align:center;\">{9}&nbsp;</td>" +
		"<td class=\"TBody1\" style=\"text-align:center;\">{10}&nbsp;</td>" +
		"<td class=\"TBody1\" style=\"text-align:center;\">{11}&nbsp;</td>" +
		"<td class=\"TBody1\" style=\"text-align:center;\">{8}</td>" +
		"<td class=\"TBody1\" style=\"text-align:center;\">{12}</td>" +
		"</tr>";

		string WHERE = " TBH.[BRANCHPK_TO]=" + ArrivalBranch;

		if (ArrivalPortchecked == "Y") {
			WHERE += " and (TBH.[AREA_TO] like '%PYONGT%' or TBH.[AREA_TO] like '%PYEONGT%')";
		}
		if (ArrivalPortchecked == "N") {
			WHERE += " and (TBH.[AREA_TO] not like '%PYONGT%' or TBH.[AREA_TO] not like '%PYEONGT%')";
		}
		if (DepartureBranch != "") {
			WHERE += " and TBH.[BRANCHPK_FROM]=" + DepartureBranch;
		}
		if (StartDate != "") {
			WHERE += " and replace(replace(replace(TBH.[DATETIME_TO], '.', ''), ':', ''), ' ', '')>='" + StartDate + "' ";
		}
		if (EndDate != "") {
			WHERE += " and replace(replace(replace(TBH.[DATETIME_TO], '.', ''), ':', ''), ' ', '')<='" + EndDate + "' ";
		}
		if (Type != "") {
			WHERE += " and TBH.[TRANSPORT_WAY]='" + Type + "' ";
		}
		if (Warehouse != "") {
			WHERE += " and OBSC.OurBranchStoragePk=" + Warehouse;
		}
		string TOP = "";
		if (StartDate == "" && EndDate == "") {
			TOP = " TOP 50 ";
		}

		StringBuilder ReturnValue = new StringBuilder();

		DB.SqlCmd.CommandText = @"	
	SELECT " + TOP + @" TBH.[TRANSPORT_PK], TBH.[TRANSPORT_WAY], TBH.[VALUE_STRING_0], TBH.[BRANCHPK_FROM], TBH.[DATETIME_FROM], TBH.[DATETIME_TO], TBH.[AREA_FROM], TBH.[AREA_TO], TBH.[VESSELNAME], TBH.[VOYAGE_NO], TBH.[VALUE_STRING_1], TBH.[VALUE_STRING_2], TBH.[VALUE_STRING_3], TBH.[TRANSPORT_STATUS], C.CompanyName, RC.Name, OBSC.StorageName,TBBC.Title,TBBC.MonetaryUnitCL,TBBC.Price,Storage.countCheck, TP.[NO], TP.[SIZE], TP.[SEAL_NO]
	FROM [dbo].[TRANSPORT_HEAD] AS TBH 
		left join Company AS C on TBH.[BRANCHPK_FROM]=C.CompanyPk 
		left join RegionCode AS RC On C.RegionCode=RC.RegionCode 
		left join (
			SELECT SUM([StorageCode])/COUNT(StorageCode) AS StorageCode,[TransportBetweenBranchPk],count(*) countCheck
			FROM OurBranchStorageOut
			Group by [TransportBetweenBranchPk]
			) AS Storage ON Storage.TransportBetweenBranchPk=TBH.[TRANSPORT_PK] 
		left join OurBranchStorageCode AS OBSC ON OBSC.OurBranchStoragePk=Storage.StorageCode
        left join  TransportBBCharge TBBC  on TBH.[TRANSPORT_PK]=TBBC.TransportBBHeadPk
		left join  [dbo].[TRANSPORT_PACKED] AS TP ON TBH.[TRANSPORT_PK] = TP.[TRANSPORT_HEAD_PK]
	WHERE " + WHERE + " and isnull(TBH.[TRANSPORT_STATUS], 3)>2 " +
"	ORDER BY [DATETIME_TO] DESC; ";
		DB.DBCon.Open();
		string[] RowData = new string[] { };
		string TempTBBPk = "";
		RS = DB.SqlCmd.ExecuteReader();
		string TempMonth = "";
		while (RS.Read()) {
			string Title = "";
			string Container = "";
			string ShipDirect = "";
			//if (ArrivalPortchecked == "Y") {
			//	if (description[2].ToString().Trim().Length > 6) {
			//		if (description[2].ToString().Trim().Substring(0, 6) != "PYONGT") {
			//			continue;
			//		}
			//	} else {
			//		continue;
			//	}
			//} else if (ArrivalPortchecked == "N") {
			//	if (description[2].ToString().Trim().Length > 6) {
			//		if (description[2].ToString().Trim().Substring(0, 6) == "PYONGT") {
			//			continue;
			//		}
			//	}
			//}
			if (RS["TRANSPORT_WAY"] + "" == "Ship") {
				if (RS["VESSELNAME"].ToString().Length > 15) {
					Title += RS["VESSELNAME"].ToString().Substring(0, 14) + " <em>" + RS["NO"] + "</em> ";
					Container = RS["SIZE"] + "";
				} else {
					Title += RS["VESSELNAME"] + " <em>" + RS["NO"] + "</em> ";
					Container = RS["SIZE"] + "";
				}
			} else {
				Title = RS["VESSELNAME"] + "" == "" ? "&nbsp;" : RS["VESSELNAME"].ToString();
				Container = "";
			}
			if (RS["VESSELNAME"].ToString().Length > 4) {
				if (RS["VESSELNAME"].ToString().Substring(0, 5) == "集装箱拖货") {
					ShipDirect = "Direct";
				} else {
					ShipDirect = "";
				}
			} else {
				ShipDirect = "";
			}

			if (TempTBBPk != RS["TRANSPORT_PK"] + "") {
				if (TempTBBPk != "") {
					ReturnValue.Append(String.Format(ROW, RowData));
				}

				RowData = new string[] { 
               //Common.GetBetweenBranchTransportWay(RS["TransportCL"] + "") + " from " + (RS["CompanyName"] + "").Substring(0, 2),
                Common.GetBetweenBranchTransportWay(RS["TRANSPORT_WAY"] + "") + " from "  ,
				RS["VALUE_STRING_0"] + "" == "" ? "&nbsp;" : RS["VALUE_STRING_0"] + "",
				Container,
				Title,
				(RS["DATETIME_FROM"] + "").Substring(4, 4) + " - " + (RS["DATETIME_TO"] + "").Substring(4, 4),
				RS["AREA_FROM"] + " ~ " + RS["AREA_FROM"],
				Common.GetBetweenBranchTransportStepCL(RS["TRANSPORT_STATUS"] + ""),
				RS[0]+"",
				RS["StorageName"]+"",
				RS["Title"].ToString() +"",
				Common.GetMonetaryUnit(RS["MonetaryUnitCL"].ToString()) ,
						 Common.NumberFormat(RS["Price"].ToString()),
				ShipDirect,
				RS["countCheck"]+""
				};
				TempTBBPk = RS["TRANSPORT_PK"] + "";
			} else {
				RowData[9] += "<br />" + RS["Title"].ToString() + "";
				RowData[10] += "<br/>" + Common.GetMonetaryUnit(RS["MonetaryUnitCL"].ToString());
				RowData[11] += "<br/>" + Common.NumberFormat(RS["Price"].ToString());
			}

			if (TempMonth != (RS["DATETIME_TO"] + "").Substring(0, 8)) {
				TempMonth = (RS["DATETIME_TO"] + "").Substring(0, 8);
				ReturnValue.Append("<tr><td colspan=\"6\"><strong><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + TempMonth + "</strong></td></tr>");
			}

		}
		if (TempTBBPk != "") {
			ReturnValue.Append(String.Format(ROW, RowData));
		}
		RS.Dispose();
		DB.DBCon.Close();

		string TABLE = "<table border='0' cellpadding='0' cellspacing='0' style=\"width:1050px;\" ><thead><tr height='30px'>" +
								"<td class='THead1' style='width:85px;' >Date</td>" +
								"<td class='THead1' style='width:90px;' >Description</td>" +
								"<td class='THead1' style='width:90px;' >BLNo</td>" +
								"<td class='THead1' style='width:20px;' >C</td>" +
								"<td class='THead1' style='width:50px;' >Container</td>" +
								"<td class='THead1' >Title</td>" +
								"<td class='THead1' style='width:90px;' >Area</td>" +
								"<td class='THead1' style='width:80px;'>Target</td>" +
								"<td class='THead1' style='width:10px;'></td>" +
								"<td class='THead1' style='width:100px;'>Price</td>" +
								"<td class='THead1' >Storage</td>" +
								"<td class='THead1' style='width:60px;'></td>" +
							"</tr></thead>{0}</table>";
		return string.Format(TABLE, ReturnValue);
	}
}
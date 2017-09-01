using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Finance_DeliveryCharge : System.Web.UI.Page
{
	private DBConn DB;
	protected String HTMLList;
	//protected String SelectMonth;
	protected String ArrivalBranch;
	protected String Gubun;
	//protected String Month;
	protected String StartDate;
	protected String EndDate;
	protected String SelectedType;
    protected String SelectedStorage;
	protected void Page_Load(object sender, EventArgs e)
	{
		ArrivalBranch = Request.Params["AB"] + "" == "" ? "3157" : Request.Params["AB"] + "";
		Gubun = Request.Params["G"] + "";
		//Month = Request.Params["M"] + "";
		StartDate = Request.Params["SD"] + "";
		EndDate = Request.Params["ED"] + "";
		SelectedType = Request.Params["SelectedType"] + "";
        SelectedStorage = Request.Params["SelectedStorage"] + "";
		DB = new DBConn();
        HTMLList = LoadDeliveryList(ArrivalBranch, StartDate, EndDate, Gubun, SelectedType, SelectedStorage);

	}
	
	private String LoadDeliveryList(string ArrivalBranch, string StartDate, string EndDate, string Gubun, string SelectedType,string SelectedStorage)
	{	
		string WHERE = "";
		if (StartDate != "") {
			WHERE += "and ArrivalDate>='" + StartDate + "' ";
		}
		if (EndDate != "") {
			WHERE += " and ArrivalDate<='" + EndDate + "' ";
		}

		//기간 없을시 top 50 추가
		string TOP = "";
		if (StartDate == "" && EndDate == "") {
			TOP = " TOP 50 ";
		}	

		if (Gubun == "After") {

			//--and R.TransportWayCL<>31  fcl 포함
			DB.SqlCmd.CommandText = @"
SELECT " + TOP + @"
	R.RequestFormPk, R.ShipperCode, R.ConsigneeCode, R.DepartureDate, R.ArrivalDate , 
	C.CompanyName, 
	TWCD.Initial, 
	R.TotalPackedCount, R.PackingUnit , R.TotalGrossWeight, R.TotalVolume, RFCH.[MONETARY_UNIT],
	DeliveryCharge.Price AS TotalPrice 
	, OBSO.BoxCount 
	, TBC.TransportBetweenCompanyPk, TBC.Type, TBC.Title, TBC.DriverName, TBC.CarSize, TBC.FromDate, TBC.WarehouseInfo, TBC.PackedCount, TBC.PackingUnit AS PU, TBC.Price, TBC.StepCL
	, OBSC.StorageName
FROM RequestForm AS R 
	left join RegionCode AS ARC ON R.ArrivalRegioncode=ARC.RegionCode 
	left join Company AS C ON R.ConsigneePk=C.CompanyPk 
	left join TransportWayCLDescription AS TWCD ON R.TransportWayCL=TWCD.TransportWayCL 
	LEFT JOIN [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH ON R.[RequestFormPk] = RFCH.[TABLE_PK]
	left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) AS Price 
		FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [CATEGORY]='대행비' group by [REQUESTFORMCALCULATE_HEAD_PK]
	) AS DeliveryCharge ON DeliveryCharge.[REQUESTFORMCALCULATE_HEAD_PK]=RFCH.[REQUESTFORMCALCULATE_HEAD_PK]
	left join OurBranchStorageOut AS OBSO ON R.RequestFormPk=OBSO.RequestFormPk
	left join TransportBC AS TBC ON TBC.TransportBetweenCompanyPk=OBSO.TransportBetweenCompanyPk
	left join OurBranchStorageCode AS OBSC ON OBSO.StorageCode=OBSC.OurBranchStoragePk 
WHERE ARC.OurBranchCode=" + ArrivalBranch + @" 
AND ISNULL(RFCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
and isnull(DeliveryCharge.Price, 0) = 0
" + WHERE + @"
ORDER BY TBC.FromDate,R.ConsigneeCode ";
			//기간으로 바꾸면서 주석 부분
//WHERE R.TransportWayCL<>31 and ARC.OurBranchCode=" + ArrivalBranch + @" and left(ArrivalDate, 6)='" + Month + @"' and isnull(DeliveryCharge.Price, 0) = 0
		} else {
			DB.SqlCmd.CommandText = @"
SELECT " + TOP + @"
	R.RequestFormPk, R.ShipperCode, R.ConsigneeCode, R.DepartureDate, R.ArrivalDate , 
	C.CompanyName, 
	TWCD.Initial, 
	R.TotalPackedCount, R.PackingUnit , R.TotalGrossWeight, R.TotalVolume, RFCH.[MONETARY_UNIT],
	 DeliveryCharge.Price AS TotalPrice 
	, OBSO.BoxCount 
	, TBC.TransportBetweenCompanyPk, TBC.Type, TBC.Title, TBC.DriverName, TBC.CarSize, TBC.FromDate, TBC.WarehouseInfo, TBC.PackedCount, TBC.PackingUnit AS PU, TBC.Price, TBC.StepCL
	, OBSC.StorageName
FROM RequestForm AS R 
	left join RegionCode AS ARC ON R.ArrivalRegioncode=ARC.RegionCode 
	left join Company AS C ON R.ConsigneePk=C.CompanyPk 
	left join TransportWayCLDescription AS TWCD ON R.TransportWayCL=TWCD.TransportWayCL 
	LEFT JOIN [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH ON R.[RequestFormPk] = RFCH.[TABLE_PK]
	left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], sum([ORIGINAL_PRICE]) AS Price 
		FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [CATEGORY]='대행비' group by [REQUESTFORMCALCULATE_HEAD_PK]
	) AS DeliveryCharge ON DeliveryCharge.[REQUESTFORMCALCULATE_HEAD_PK]=RFCH.[REQUESTFORMCALCULATE_HEAD_PK]
	left join OurBranchStorageOut AS OBSO ON R.RequestFormPk=OBSO.RequestFormPk
	left join TransportBC AS TBC ON TBC.TransportBetweenCompanyPk=OBSO.TransportBetweenCompanyPk
	left join OurBranchStorageCode AS OBSC ON OBSO.StorageCode=OBSC.OurBranchStoragePk 
WHERE TBC.DepositWhere=0 
	AND ISNULL(RFCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
	and ARC.OurBranchCode=" + ArrivalBranch + @" 
	and isnull(DeliveryCharge.Price, 0) <> 0
" + WHERE + @"
ORDER BY TBC.FromDate,R.ConsigneeCode";			
		}
		//Response.Write(DB.SqlCmd.CommandText);
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		string ROW = "<tr height=\"30px; \">" +
			"<td {0} class=\"{16}\">{1}</td>" +
			"<td {0} class=\"{16}\">{2}</td>" +
			"<td {0} class=\"{16}\">{3}</td>" +
			"<td {0} class=\"{16}\"><a href=\"../Admin/RequestView.aspx?g=s&pk={14}\" >{4}</a></td>" +
			"<td {0} class=\"{16}\" >{5}</td>" +
			"<td {0} class=\"{16}\" >{6}</td>" +
			"<td {0} class=\"{16}\">{7}</td>" +
			"<td {0} class=\"{16}\">{8}</td>" +
			"<td {0} class=\"{16}\">{9}</td>" +
			"<td class=\"{16}\">{10}</td>" +
			"<td class=\"{16}\">{11}</td>" +
			"<td class=\"{16}\">{12}</td>" +
			"<td class=\"{16}\">{13}</td>" +
			"<td class=\"{16}\">{15}</td></tr>";
		string ROW2 = "<tr height=\"30px; \">" +
			"<td class=\"{5}\">{0}</td>" +
			"<td class=\"{5}\">{1}</td>" +
			"<td class=\"{5}\">{2}</td>" +
			"<td class=\"{5}\">{3}</td>" +
			"<td class=\"{5}\">{4}</td>" +
			"</tr>";

		string TABLE = "<table border='0' cellpadding='0' cellspacing='0' style=\"width:1050px;\" ><thead><tr height='30px'>" +
									"<td class='THead1' style='width:50px;' >일정</td>" +
									"<td class='THead1' style='width:20px;' >i</td>" +
									"<td class='THead1' style='width:70px;' >code</td>" +
									"<td class='THead1' >company</td>" +
									"<td class='THead1' style='width:60px;' >CT</td>" +
									"<td class='THead1' style='width:60px;' colspan='2' >청구금액</td>" +
									"<td class='THead1' style='width:80px;' >{2}</td>" +
									"<td class='THead1' style='width:100px;' colspan='2'>{1}</td>" +
									"<td class='THead1' style='width:60px;' >수량</td>" +
									"<td class='THead1' style='width:80px;' >출발일</td>" +
									"<td class='THead1' style='width:100px;' >송장금액</td>" +
									"<td class='THead1' style='width:60px;' >C</td>" +
								"</tr></thead>{0}</table>";
		string TempRequestFormPk = "";
		StringBuilder ReturnValue = new StringBuilder();
		List<string[]> RowValue = new List<string[]>();
		List<string> TypeSum = new List<string>();
        List<string> StorageSum = new List<string>();

		while (RS.Read()) {
			bool IsTypeNew = true;
			for (var i = 0; i < TypeSum.Count; i++) {
				if (TypeSum[i] == RS["Type"] + "") {
					IsTypeNew = false;
					break;
				}
			}
			if (IsTypeNew) {
				TypeSum.Add(RS["Type"] + "");
			}
            bool IsStorageNew = true;
            for (var i = 0; i < StorageSum.Count; i++)
            {
                if (StorageSum[i] == RS["StorageName"] + "")
                {
                    IsStorageNew = false;
                    break;
                }
            }
            if (IsStorageNew)
            {
                StorageSum.Add(RS["StorageName"] + "");
            }

			if (SelectedType != "All" && SelectedType != "") {
				if (RS["Type"] + "" != SelectedType) {
					continue;
				}
			}
            if (SelectedStorage != "All" && SelectedStorage != "")
            {
                if (RS["StorageName"] + "" != SelectedStorage)
                {
                    continue;
                }
            }

			if (TempRequestFormPk != RS["RequestFormPk"] + "") {
				if (TempRequestFormPk != "") {
					for (int i = 0; i < RowValue.Count; i++) {
						if (i == 0) {
							RowValue[0][0] = RowValue.Count + "" != "1" ? "rowspan=\"" + RowValue.Count + "\"" : "";
							ReturnValue.Append(string.Format(ROW, RowValue[0]));
						} else {
							ReturnValue.Append(string.Format(ROW2, RowValue[i]));
						}
					}
					RowValue = new List<string[]>();
				}
				TempRequestFormPk = RS["RequestFormPk"] + "";
			}

			if (RowValue.Count > 0) {
				RowValue.Add(new string[]{
					TDForEmpty(RS["Title"]+""), 
					RS["PackedCount"]+Common.GetPackingUnit(RS["PackingUnit"]+""), 
					RS["FromDate"]+""==""?"&nbsp;":(RS["FromDate"]+"").Substring(2, 6), 
					Common.GetMonetaryUnit(RS["ConsigneeMonetaryUnit"]+"")+" "+Common.NumberFormat(RS["Price"]+""), 
					RS["StepCL"]+""==""?"<input type=\"button\" onclick=\"TBCConfirm('"+RS["TransportBetweenCompanyPk"]+"')\" value=\"C\" />":"", 
					RS["StepCL"]+""==""?"TBody1":"TBody1G"
				});
			} else {
				RowValue.Add(new string[]{
					"", 
					(RS["ArrivalDate"]+"").Substring(4, 2)+"/"+(RS["ArrivalDate"]+"").Substring(6, 2), 
					RS["Initial"]+"", 
					RS["ConsigneeCode"]+"", 
					RS["CompanyName"]+"", 
					RS["TotalPackedCount"]+Common.GetPackingUnit(RS["PackingUnit"]+""), 
					Common.GetMonetaryUnit(RS["MONETARY_UNIT"]+""), 
					Common.NumberFormat(RS["TotalPrice"]+""), 
					TDForEmpty(RS["StorageName"]+""), 
					TDForEmpty(RS["Type"]+""),
					TDForEmpty(RS["Title"]+""), 
					RS["PackedCount"]+Common.GetPackingUnit(RS["PU"]+""), 
					RS["FromDate"]+""==""?"&nbsp;":(RS["FromDate"]+"").Substring(2, 6), 
					Common.GetMonetaryUnit(RS["MONETARY_UNIT"]+"")+" "+Common.NumberFormat(RS["Price"]+""), 
					RS["RequestFormPk"]+"", 
					RS["StepCL"]+""==""?"<input type=\"button\" onclick=\"TBCConfirm('"+RS["TransportBetweenCompanyPk"]+"')\" value=\"C\" />":"", 
					RS["StepCL"]+""==""?"TBody1":"TBody1G"
				});
			}
		}
		for (int i = 0; i < RowValue.Count; i++) {
			if (i == 0) {
				RowValue[0][0] = RowValue.Count + "" != "1" ? "rowspan=\"" + RowValue.Count + "\"" : "";
				ReturnValue.Append(string.Format(ROW, RowValue[0]));
			} else {
				ReturnValue.Append(string.Format(ROW2, RowValue[i]));
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		StringBuilder Html_SelectOption_TypeSum = new StringBuilder();
		Html_SelectOption_TypeSum.Append("<option value=\"All\">전체</option>");
		for (var i = 0; i < TypeSum.Count; i++) {
			if (SelectedType == TypeSum[i]) {
				Html_SelectOption_TypeSum.Append("<option selected='selected' value=\"" + TypeSum[i] + "\">" + TypeSum[i] + "</option>");
			} else {
				Html_SelectOption_TypeSum.Append("<option value=\"" + TypeSum[i] + "\">" + TypeSum[i] + "</option>");						
			}
		}
        StringBuilder Html_SelectOption_Storage = new StringBuilder();
        Html_SelectOption_Storage.Append("<option value=\"All\">전체</option>");
        for (var i = 0; i < StorageSum.Count; i++)
        {
            if (SelectedStorage == StorageSum[i])
            {
                Html_SelectOption_Storage.Append("<option selected='selected' value=\"" + StorageSum[i] + "\">" + StorageSum[i] + "</option>");
            }
            else
            {
                Html_SelectOption_Storage.Append("<option value=\"" + StorageSum[i] + "\">" + StorageSum[i] + "</option>");
            }
        }

        return String.Format(TABLE, ReturnValue + "", "<select name=\"SelectedType\" onchange=\"St_Gubun_Change();\">" + Html_SelectOption_TypeSum + "</select>", "<select name=\"SelectedStorage\" onchange=\"St_Storage_Change();\">" + Html_SelectOption_Storage + "</select>");
	}
	private String TDForEmpty(string value)
	{
		if (value == "") {
			return "&nbsp;";
		} else {
			return value;
		}
	}
}
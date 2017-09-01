using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Finance_Sales : System.Web.UI.Page
{
	private DBConn DB;
	protected String HTML;

	protected String StartDate;
	protected String EndDate;
	protected String DepartureBranch;
	protected String ArrivalBranch;

	private List<string[]> WrongSpell;

	protected void Page_Load(object sender, EventArgs e)
    {
		DB = new DBConn();

		DepartureBranch = Request.Params["DB"] + "";
		ArrivalBranch = Request.Params["AB"] + "";

		StartDate = Request.Params["SD"] + "";
		EndDate = Request.Params["ED"] + "";

		if (StartDate != "")
		{
			DB.DBCon.Open();
			string[] temp = LoadTitle();
			HTML = LoadHTML(temp);
			DB.DBCon.Close();
		}
    }
	private String[] LoadTitle()
	{
		string QueryDeparture, QueryArrival, QueryStartDate, QueryEndDate;
		QueryDeparture = DepartureBranch == "" ? "" : " and DRC.OurBranchCode=" + DepartureBranch;
		QueryArrival = ArrivalBranch == "" ? "" : " and ARC.OurBranchCode=" + ArrivalBranch;
		QueryStartDate = StartDate == "" ? "" : " and ArrivalDate>='" + StartDate + "'";
		QueryEndDate = EndDate == "" ? "" : "and ArrivalDate<='" + EndDate + "'";

		DB.SqlCmd.CommandText = @"SELECT RFCB.[TITLE]
FROM RequestForm AS R 
	left join RegionCode AS DRC ON R.DepartureRegioncode=DRC.RegionCode 
	left join RegionCode AS ARC ON R.ArrivalRegioncode=ARC.RegionCode 
	Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as RFCH on R.RequestFormPk=RFCH.[TABLE_PK] 
	left join [dbo].[REQUESTFORMCALCULATE_BODY] AS RFCB ON RFCH.[TABLE_PK]=RFCB.[REQUESTFORMCALCULATE_HEAD_PK]
WHERE R.TransportWayCL<>31 " + QueryDeparture + QueryArrival + QueryStartDate + QueryEndDate + @" 
AND ISNULL(RFCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
 group by RFCB.[TITLE] ;";
		//Response.Write(DB.SqlCmd.CommandText+"<br/>");
		//return new string[] { "" };
		//DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		List<string> tempList = new List<string>();
		WrongSpell = new List<string[]>();
		tempList.Add("OCEAN FREIGHT");
		WrongSpell.Add(new string[] { "국제운송료", "국제운송비", "국제운송비 4DAY", "국제운송비 5DAY", "국제운송비외", "O/F", "OCEAN FREIGHT", "OCEAN FREIGHT 3DAY", "OCEAN FREIGHT 4DAY", "OCEAN FREIGHT 5DAY", "OCEAN FREIGHT 7DAY", "해운비" });

		tempList.Add("PACKING CHARGE");
		WrongSpell.Add(new string[] { "재포장비", "pachaging costs", "PACKING CHARGE" });
		tempList.Add("PICKUP CHARGE");
		WrongSpell.Add(new string[] { "상해픽업비", "온주픽업비", "피겁비", "픽업비", "PACK UP CHARGE", "pick up costs", "PICKUP CHARGE", "픽업" });
		tempList.Add("SUBSTITUTE CHARGE");
		WrongSpell.Add(new string[] { "단증비", "SUBSTITUTE CHARGE" });

		tempList.Add("SURVEY CHARGE");
		WrongSpell.Add(new string[] { "SURVER CHARGE", "검역비", "SURVEY CHARGE" });


		tempList.Add("C/O CHARGE");
		WrongSpell.Add(new string[] { "C/O", "C/O CHARGE", "CO", "CO비" });


		tempList.Add("CHINA OTHER CHARGE");
		WrongSpell.Add(new string[] { "CHINA OTHER CHARGE" });

		tempList.Add("DELIVERY ORDER CHARGE");
		WrongSpell.Add(new string[] { "D/O", "DELIVERY ORDER CHARGE", "DOC", "D/O비" });

		tempList.Add("HANDLING CHARGE");
		WrongSpell.Add(new string[] { "H/C", "HANDLING CHARGE", "핸드링비" });

		tempList.Add("BONDED WAREHOUSE CHARGE");
		WrongSpell.Add(new string[] { "보관비", "보세장치료", "보세창고료", "창고료", "BONDED WAREHOUSE CHARGE", "창고료비" });

		tempList.Add("WHARFAGE");
		WrongSpell.Add(new string[] { "WHARFAGE", "부두발생비" });

		while (RS.Read())
		{
			bool CheckIsIn = false;
			for (int i = 0; i < WrongSpell.Count; i++)
			{
				foreach (string each in WrongSpell[i])
				{
					if (each == RS[0] + "")
					{
						CheckIsIn = true;
						break;
					}
				}
				if (CheckIsIn)
				{
					break;
				}
			}
			if (CheckIsIn) { continue; }
			tempList.Add(RS[0] + "");
		}
		RS.Dispose();
		return tempList.ToArray();
	}
	private String LoadHTML(string[] Header)
	{
		StringBuilder ReturnValue = new StringBuilder();

		StringBuilder temp = new StringBuilder();
		foreach (string title in Header)
		{
			temp.Append("<td colspan='2'>" + title + "&nbsp;</td>");
		}
		string QueryDeparture, QueryArrival, QueryStartDate, QueryEndDate;
		QueryDeparture = DepartureBranch == "" ? "" : " and DRC.OurBranchCode=" + DepartureBranch;
		QueryArrival = ArrivalBranch == "" ? "" : " and ARC.OurBranchCode=" + ArrivalBranch;
		QueryStartDate = StartDate == "" ? "" : " and ArrivalDate>='" + StartDate + "'";
		QueryEndDate = EndDate == "" ? "" : "and ArrivalDate<='" + EndDate + "'";

		ReturnValue.Append(@"<table border='1'>
			<tr>
				<td>발송지사</td>
				<td>발화인</td>
				<td>수하인</td>
				<td>출발</td>
				<td>도착</td>
				<td>출발지</td>
				<td>업체명</td>
				<td>운송방법</td>
				<td>포장수량</td>
				<td>단위</td>
				<td>중량</td>
				<td>체적</td>
				<td>&nbsp;</td>				
				<td>운송비</td>
                <td>관부가세</td>
                <td>배달비</td>
				<td>청구금 총액</td>
				
				" + temp + @"
			</tr>");
		DB.SqlCmd.CommandText = @"
SELECT 
    R.RequestFormPk, R.ShipperCode, R.ConsigneeCode, R.DepartureDate, R.ArrivalDate
	, DRC.Name, DRC.OurBranchCode, C.CompanyName, TWCD.Description, R.TotalPackedCount, R.PackingUnit
    , R.TotalGrossWeight, R.TotalVolume, R.ExchangeDate, RFCH.[MONETARY_UNIT], RFCH.[TOTAL_PRICE]
    , RFCH.[DEPOSITED_PRICE], RFCH.[LAST_DEPOSITED_DATE]
    , RFCB.[REQUESTFORMCALCULATE_HEAD_PK], RFCB.[TITLE], RFCB.[ORIGINAL_PRICE], RFCB.[ORIGINAL_MONETARY_UNIT]
    , Freight.[FSum] AS FSum, Delivery.[DSum] AS DSum, Tariff.[TSum] AS TSum
FROM RequestForm AS R 
	left join RegionCode AS DRC ON R.DepartureRegioncode=DRC.RegionCode 
	left join RegionCode AS ARC ON R.ArrivalRegioncode=ARC.RegionCode 
	left join Company AS C ON R.ConsigneePk=C.CompanyPk
	left join TransportWayCLDescription AS TWCD ON R.TransportWayCL=TWCD.TransportWayCL
	Left join [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH ON R.RequestFormPk=RFCH.[TABLE_PK] 
	left join [dbo].[REQUESTFORMCALCULATE_BODY] AS RFCB ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK]=RFCB.[REQUESTFORMCALCULATE_HEAD_PK]
	LEFT JOIN (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], SUM([ORIGINAL_PRICE]) AS FSum FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [CATEGORY] = '해운비' GROUP BY [REQUESTFORMCALCULATE_HEAD_PK]
		) AS Freight ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK] = Freight.[REQUESTFORMCALCULATE_HEAD_PK] 
	LEFT JOIN (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], SUM([ORIGINAL_PRICE]) AS DSum FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [CATEGORY] = '대행비' GROUP BY [REQUESTFORMCALCULATE_HEAD_PK]
		) AS Delivery ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK] = Delivery.[REQUESTFORMCALCULATE_HEAD_PK] 
	LEFT JOIN (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], SUM([ORIGINAL_PRICE]) AS TSum FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [CATEGORY] = '제세금' GROUP BY [REQUESTFORMCALCULATE_HEAD_PK]
		) AS Tariff ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK] = Tariff.[REQUESTFORMCALCULATE_HEAD_PK]
WHERE R.TransportWayCL<>31 " + QueryDeparture + QueryArrival + QueryStartDate + QueryEndDate + @"  
AND ISNULL(RFCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
 ORDER BY ArrivalDate";
		//return DB.SqlCmd.CommandText;
		string EachRowBegin = @"<tr>
				<td>{22}</td>
				<td><a href='../Admin/RequestView.aspx?G=C&pk={0}'>{1}</a></td>
				<td>{2}</td>
				<td>{3}</td>
				<td>{4}</td>
				<td>{5}</td>
				<td>{6}</td>
				<td>{7}</td>
				<td>{8}</td>
				<td>{9}</td>
				<td>{10}</td>
				<td>{11}</td>
				<td>{14}</td>
				<td>{15}</td>
				<td>{18}</td>
				<td>{19}</td>				
				<td>{20}</td>";
		//Response.Write(DB.SqlCmd.CommandText);
		//return "";

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string[] Values = new string[170];
		StringBuilder temphtml = new StringBuilder();
		decimal TotalWeight = 0;
		decimal TotalVolume = 0;
		decimal[] TotalValue = new decimal[170] { 
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

		string tempRequestFormPk = "";
		while (RS.Read())
		{
			if (tempRequestFormPk != RS["RequestFormPk"] + "")
			{
				if (tempRequestFormPk != "")
				{
					temphtml = new StringBuilder();
					for (int j = 0; j < Header.Length; j++)
					{
						if (Values[j] + "" == "")
						{
							temphtml.Append("<td>&nbsp;</td><td>&nbsp;</td>");
						}
						else
						{
							temphtml.Append("<td>" + Values[j] + "</td>");
						}
					}
					ReturnValue.Append(temphtml + "</tr>");
				}

				tempRequestFormPk = RS["RequestFormPk"] + "";
				TotalWeight += RS["TotalGrossWeight"] + "" == "" ? 0 : decimal.Parse(RS["TotalGrossWeight"] + "");
				TotalVolume += RS["TotalVolume"] + "" == "" ? 0 : decimal.Parse(RS["TotalVolume"] + "");

				decimal TotalCharge = RS["TOTAL_PRICE"] + "" == "" ? 0 : decimal.Parse(RS["TOTAL_PRICE"] + "");

				string OurBranchName;
				switch (RS["OurBranchCode"] + "")
				{
					case "2886": OurBranchName = "烟台"; break;
					case "2887": OurBranchName = "瀋陽"; break;
					case "2888": OurBranchName = "义乌"; break;
					case "3095": OurBranchName = "JP"; break;
					case "3157": OurBranchName = "KR"; break;
					case "3388": OurBranchName = "青岛"; break;
					case "3798": OurBranchName = "Other"; break;
					case "7898":OurBranchName = "衢州";break;
					default: OurBranchName = "广州"; break;
				}

				decimal Left = TotalCharge - decimal.Parse(RS["DEPOSITED_PRICE"] + "" == "" ? "0" : RS["DEPOSITED_PRICE"] + "");
				ReturnValue.Append(String.Format(EachRowBegin,
					RS["RequestFormPk"] + "",
					RS["ShipperCode"] + "",
					RS["ConsigneeCode"] + "",
					RS["DepartureDate"] + "",
					RS["ArrivalDate"] + "",
					RS["Name"] + "",
					RS["CompanyName"] + "",
					RS["Description"] + "",
					RS["TotalPackedCount"] + "",
					Common.GetPackingUnit(RS["PackingUnit"] + ""),
					Common.NumberFormat(RS["TotalGrossWeight"] + ""),
					Common.NumberFormat(RS["TotalVolume"] + ""),
					RS["TOTAL_PRICE"] + "" == "0.0000" ? "&nbsp;" : Common.GetMonetaryUnit(RS["MONETARY_UNIT"] + ""),
					RS["TOTAL_PRICE"] + "" == "0.0000" ? "&nbsp;" : Common.NumberFormat(RS["TOTAL_PRICE"] + ""),
					RS["TOTAL_PRICE"] + "" == "0.0000" ? "&nbsp;" : Common.GetMonetaryUnit(RS["MONETARY_UNIT"] + ""),
					RS["FSum"] + "" == "0.0000" ? "&nbsp;" : Common.NumberFormat(RS["FSum"] + ""),
					RS["DEPOSITED_PRICE"] + "" == "" ? "&nbsp;" : Common.NumberFormat(RS["DEPOSITED_PRICE"] + ""),
					RS["DEPOSITED_PRICE"] + "" == "" ? "&nbsp;" : Common.NumberFormat(RS["DEPOSITED_PRICE"] + ""),
					Common.NumberFormat(RS["TSum"] + ""),
					Common.NumberFormat(RS["DSum"] + ""),
					Common.NumberFormat(TotalCharge + ""),
					Common.NumberFormat(
						Left.ToString()
					),
					OurBranchName
					));		//(TotalCharge - decimal.Parse(RS["ConsigneeDeposited"] + "")).ToString()

				Values = new string[170];
			}

			string tempValue = RS["TITLE"] + "";
			for (int i = 0; i < Header.Length; i++)
			{
				bool isin = false;
				if (i < WrongSpell.Count)
				{
					foreach (string wrongspell in WrongSpell[i])
					{
						if (wrongspell == tempValue)
						{
							Values[i] = Common.GetMonetaryUnit(RS["ORIGINAL_MONETARY_UNIT"] + "") + "</td><td>" + Common.NumberFormat(RS["ORIGINAL_PRICE"] + "");
							if (RS["ORIGINAL_PRICE"] + "" != "")
							{
								TotalValue[i] += decimal.Parse(RS["ORIGINAL_PRICE"] + "");
							}
							isin = true;
							break;
						}
					}
					if (isin) { break; }
				}
				else
				{
					if (Header[i] == tempValue)
					{
						Values[i] = Common.GetMonetaryUnit(RS["ORIGINAL_MONETARY_UNIT"] + "") + "</td><td>" + Common.NumberFormat(RS["ORIGINAL_PRICE"] + "");
						if (RS["ORIGINAL_PRICE"] + "" != "")
						{
							TotalValue[i] += decimal.Parse(RS["ORIGINAL_PRICE"] + "");
						}
						break;
					}
				}
			}
		}
		temphtml = new StringBuilder();
		for (int j = 0; j < Header.Length; j++)
		{
			if (Values[j] + "" == "")
			{
				temphtml.Append("<td>&nbsp;</td><td>&nbsp;</td>");
			}
			else
			{
				temphtml.Append("<td>" + Values[j] + "&nbsp;</td>");
			}
		}
		ReturnValue.Append(temphtml + "</tr>");
		RS.Dispose();
		ReturnValue.Append(@"<tr><td>&nbsp;</td>
				<td>&nbsp;</td>
				<td>&nbsp;</td>
				<td>&nbsp;</td>
				<td>&nbsp;</td>
				<td>&nbsp;</td>
				<td>&nbsp;</td>
				<td>&nbsp;</td>
				<td>&nbsp;</td>
<td>&nbsp;</td>
				<td>" + Common.NumberFormat("" + TotalWeight) + @"</td>
				<td>" + Common.NumberFormat("" + TotalVolume) + @"</td>
				<td>&nbsp;</td>
<td>&nbsp;</td>
<td>&nbsp;</td>
<td>&nbsp;</td><td>&nbsp;</td>				
				<td>&nbsp;</td>");
		for (int j = 0; j < Header.Length; j++)
		{
			ReturnValue.Append("<td colspan='2'>" + Common.NumberFormat(TotalValue[j] + "") + "</td>");
		}
		return ReturnValue + "";
	}
}
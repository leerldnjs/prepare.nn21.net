using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.cClearance
{
	public class YuhanN
	{
		private DBConn DB_Yuhan;
		private DBConn DB_Intl2000;
		private string MRefNo;
		public YuhanN() { }
		public sYuhan_MasterN Load_YuhanMaster(string TBBHPk) {
			sYuhan_MasterN Returnvalue;
			string query = @"
				SELECT DocumentPk  
				FROM [INTL2010].[dbo].[Document] 
				WHERE [Type]='MasterBLN' and [TypePk]=" + TBBHPk + ";";
			DB_Intl2000 = new DBConn();
			DB_Intl2000.DBCon.Open();
			DB_Intl2000.SqlCmd.CommandText = query;
			string SavedCount = DB_Intl2000.SqlCmd.ExecuteScalar() + "";
			DB_Intl2000.DBCon.Close();

			if (SavedCount == "") {
				Returnvalue = Make_YuhanMaster(TBBHPk);
			} else {
				Returnvalue = Load_YuhanMaster_FromIntl2000(TBBHPk);
			}
			return Returnvalue;
		}

		public sYuhan_MasterN Load_YuhanMaster_FromIntl2000(string TBBHPk) {
			sYuhan_MasterN Current = new sYuhan_MasterN();
			Current.TBBHPk = TBBHPk;
			DB_Intl2000 = new DBConn();
			DB_Intl2000.SqlCmd.CommandText = @"
				SELECT 
					[DocumentPk],[Type],[TypePk],[Status]
					,[Value0],[Value1],[Value2],[Value3],[Value4],[Value5],[Value6],[Value7],[Value8],[Value9]
					,[Value10],[Value11],[Value12],[Value13],[Value14],[Value15],[Value16],[Value17],[Value18],[Value19]
					,isnull([ValueInt0], 0) AS ValueInt0
					,isnull([ValueDecimal0], 0) AS ValueDecimal0 
					,isnull([ValueDecimal1], 0) AS ValueDecimal1 
					,isnull([ValueDecimal2], 0) AS ValueDecimal2 
					,[ParentsType],[ParentsId], [Value16]
				FROM [INTL2010].[dbo].[Document] 
				WHERE [Type] in ('MasterBLN', 'HouseBLN') and TypePk=" + TBBHPk + @"
				ORDER BY [Type] DESC, ValueInt0 ASC ;";
			DB_Intl2000.DBCon.Open();
			SqlDataReader RS = DB_Intl2000.SqlCmd.ExecuteReader();

			Current.HouseBL = new List<HouseBLN>();

			var house = new HouseBLN();

			while (RS.Read()) {
				if (RS["Type"] + "" == "MasterBLN") {
					Current.DocumentPk = RS["DocumentPk"] + "";
					Current.FinalDate = RS["Value0"] + "";
					Current.MasterBLNo = RS["Value1"] + "";
					Current.ShipName = RS["Value2"] + "";
					Current.VoyageNo = RS["Value3"] + "";
					Current.MRN = RS["Value4"] + "";
					Current.MSN = RS["Value5"] + "";
					Current.Customs = RS["Value6"] + "";
					Current.Division = RS["Value7"] + "";
					Current.LineCode = RS["Value8"] + "";
					Current.FinalPort = RS["Value9"] + "";
					Current.AssignmentWH = RS["Value10"] + "";
					Current.Container.ContainerNo = RS["Value11"] + "";
					Current.Container.ContainerCode = RS["Value12"] + "";
					Current.Container.SealNo1 = RS["Value13"] + "";
					Current.AssignmentCode = RS["Value14"] + "";
					Current.AssignmentName = RS["Value15"] + "";
					Current.TotalPackedCount = Int32.Parse(RS["ValueInt0"] + "");
					Current.TotalVolume = decimal.Parse(RS["ValueDecimal0"] + "");
					Current.TotalWeight = decimal.Parse(RS["ValueDecimal1"] + "");
					Current.TotalAmount = decimal.Parse(RS["ValueDecimal2"] + "");
					Current.FlagAACO = RS["Value16"] + "";
					if (Current.FlagAACO == "") {
						Current.FlagAACO = "INTL";
					}
				} else {
					house = new HouseBLN();
					house.BLNo = RS["Value0"] + "";
					house.PackedCount = RS["Value1"] + "";
					house.PackingUnit = RS["Value2"] + "";
					house.Weight = RS["Value3"] + "";
					house.Volume = RS["Value4"] + "";
					house.Amount = RS["Value5"] + "";
					house.ShipperName = RS["Value6"] + "";
					house.ConsigneeName = RS["Value7"] + "";
					house.ShipperAddress = RS["Value8"] + "";
					house.ConsigneeAddress = RS["Value9"] + "";
					house.Description = RS["Value10"] + "";
					house.ConsigneeSaupjaNo = RS["Value11"] + "";
					house.HSN = RS["ValueInt0"] + "";
					Current.HouseBL.Add(house);
				}
			}

			RS.Close();
			DB_Intl2000.DBCon.Close();

			return Current;
		}
		public sYuhan_MasterN Make_YuhanMaster(string TBBHPk) {
			sYuhan_MasterN Returnvalue = new sYuhan_MasterN();
			Returnvalue.TBBHPk = TBBHPk;
			DB_Intl2000 = new DBConn();
			DB_Intl2000.SqlCmd.CommandText = @"
SELECT TOP 10 
	BBHead.[TRANSPORT_WAY], BBHead.[VALUE_STRING_0], BBHead.[DATETIME_FROM], BBHead.[DATETIME_TO], BBHead.[AREA_FROM], BBHead.[AREA_TO], BBHead.[VESSELNAME], BBHead.[VOYAGE_NO]
	, BBHead.[VALUE_STRING_1], BBHead.[VALUE_STRING_2], BBHead.[VALUE_STRING_3], BBHead.[TRANSPORT_STATUS], TP.[TYPE], TP.[SIZE], TP.[NO], TP.[SEAL_NO]
FROM [dbo].[TRANSPORT_HEAD] AS BBHead 
LEFT JOIN [dbo].[TRANSPORT_PACKED] AS TP ON BBHead.[TRANSPORT_PK] = TP.[TRANSPORT_HEAD_PK]
WHERE BBHead.[TRANSPORT_PK]=" + TBBHPk + ";";
			DB_Intl2000.DBCon.Open();
			SqlDataReader RS = DB_Intl2000.SqlCmd.ExecuteReader();
			string stepCL = ""; string blno = ""; string loadingdate = ""; string finaldate = ""; string loadingport = ""; string finalport = ""; string precarriage = ""; string vesselname = ""; string containertype = "";
			string precarriagecode = "";
			string containerno = "";
			string sealno = "";
			string voyageno = "";
			if (RS.Read()) {
				string transportcl = RS["TRANSPORT_WAY"] + "";
				stepCL = RS["TRANSPORT_STATUS"] + "";
				if (RS["DATETIME_FROM"] + "" != "... :" && RS["DATETIME_FROM"] + "" != "") {
					string tempFromDateTime = RS["DATETIME_FROM"] + "";
					loadingdate = tempFromDateTime.Substring(0, tempFromDateTime.IndexOf(" ") - 1).Replace(".", "/");
				}
				if (RS["DATETIME_TO"] + "" != "... :" && RS["DATETIME_TO"] + "" != "") {
					string tempToDateTime = RS["DATETIME_TO"] + "";
					finaldate = tempToDateTime.Substring(0, tempToDateTime.IndexOf(" ") - 1).Replace(".", "/");
				}

				blno = RS["VALUE_STRING_0"] + "";
				precarriage = RS["VESSELNAME"] + "";
				loadingport = RS["AREA_FROM"] + "";
				finalport = RS["AREA_TO"] + "";
				vesselname = RS["VESSELNAME"] + "";
				containertype = RS["SIZE"] + "";
				voyageno = RS["VOYAGE_NO"] + "";
				containerno = RS["NO"] + "";
				sealno = RS["SEAL_NO"] + "";

			}
			RS.Dispose();
			Returnvalue.MasterBLNo = blno;
			Returnvalue.ShipName = vesselname;
			Returnvalue.VoyageNo = voyageno;
			Returnvalue.LineCode = precarriagecode;
			Returnvalue.FinalDate = finaldate;

			if (finalport.Substring(0, 4) == "INCH") {
				Returnvalue.FinalPort = "KRINC";
			} else {
				Returnvalue.FinalPort = "KRPTK";
			}

			Returnvalue.Container.ContainerNo = containerno;
			Returnvalue.Container.SealNo1 = sealno;

			if (containertype != "" && containertype.Substring(0, 1) == "2") {
				Returnvalue.Container.ContainerCode = "22GP";
			} else if (containertype == "42GP" && containertype == "40GP") {
				Returnvalue.Container.ContainerCode = "42GP";
			} else {
				Returnvalue.Container.ContainerCode = "44GP";
			}
			Returnvalue.AssignmentCode = "G";
			Returnvalue.AssignmentName = "보세운송";
			Returnvalue.HouseBL = Make_YuhanHouse(TBBHPk, stepCL);
			return Returnvalue;
		}
		private List<HouseBLN> Make_YuhanHouse(string TBBHPk, string stepCL) {
			List<HouseBLN> ReturnValue = new List<HouseBLN>();
			string Table, TempQ;
			if (stepCL == "" || stepCL == "3") {
				Table = " TransportBBHistory ";
				TempQ = "";
			} else {
				Table = " [dbo].[TRANSPORT_BODY] ";
				TempQ = " ";
			}
			DB_Intl2000 = new DBConn();
			DB_Intl2000.SqlCmd.CommandText = @"		
SELECT 
	Storage.[PACKED_COUNT] 
	, R.RequestFormPk, R.ConsigneeCode
	, R.TotalPackedCount, R.PackingUnit, R.TotalGrossWeight, R.TotalVolume
	, RFI.Description AS CICKD 
	, CD.BLNo, CD.Shipper, CD.ShipperAddress, CD.Consignee, CD.ConsigneeAddress
	, Saupja.CompanyNo
FROM  
	" + Table + @" AS Storage left join 
	RequestForm AS R on Storage.[REQUEST_PK]=R.RequestFormPk 
	Left join Company AS CC on R.ConsigneePk=CC.CompanyPk 
	Left join RegionCode AS Departure on R.DepartureRegionCode=Departure.RegionCode 
	Left join RegionCode AS Arrival on R.ArrivalRegionCode=Arrival.RegionCode 
	left join RequestFormItems AS RFI ON R.RequestFormPk=RFI.RequestFormPk 
	left join (SELECT [TABLE_PK], [ACCOUNT_ID], [DESCRIPTION], [REGISTERD] FROM [dbo].[HISTORY] WHERE [TABLE_NAME] = 'RequestForm' AND [CODE]='0') AS RFAI ON R.RequestFormPk=RFAI.[TABLE_PK] 
	left join CommerdialConnectionWithRequest AS CCWR On R.RequestFormPk=CCWR.RequestFormPk 
	left join CommercialDocument AS CD On CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk 
	left join [CompanyInDocument] AS Saupja on R.ConsigneeClearanceNamePk=Saupja.CompanyInDocumentPk
WHERE Storage.[TRANSPORT_HEAD_PK]=" + TBBHPk + TempQ + @" and isnull(R.DocumentStepCL,'')<>4
ORDER BY R.ConsigneeCode ASC, BLNo ASC;";

			//Response.Write(DB.SqlCmd.CommandText);
			DB_Intl2000.DBCon.Open();
			SqlDataReader RS = DB_Intl2000.SqlCmd.ExecuteReader();
			string TempBLNo = "_";
			string TempRequestFormPk = "";
			List<string> ItemSum = new List<string>();
			StringBuilder itemTemp;
			int count = 0;
			int TotalPackedCount = 0;
			decimal TotalGrossWeight = 0;
			decimal TotalVolume = 0;
			HouseBLN Current = new HouseBLN();
			while (RS.Read()) {
				if (TempBLNo == RS["BLNo"] + "") {
					if (TempRequestFormPk != RS["RequestFormPk"] + "") {
						TempRequestFormPk = RS["RequestFormPk"] + "";
						if (RS["PACKED_COUNT"] + "" != "") {
							TotalPackedCount += Int32.Parse(RS["PACKED_COUNT"] + "");
						}
						if (RS["TotalGrossWeight"] + "" != "") {
							TotalGrossWeight += decimal.Parse(RS["TotalGrossWeight"] + "");
						}
						if (RS["TotalVolume"] + "" != "") {
							TotalVolume += decimal.Parse(RS["TotalVolume"] + "");
						}
					}
					string tempItemDescription = (RS["CICKD"] + "").Replace("\t", "").Trim();
					if (tempItemDescription != "") {
						bool isinitemsum = false;
						foreach (string each in ItemSum) {
							if (each == tempItemDescription) {
								isinitemsum = true;
								break;
							}
						}
						if (!isinitemsum) {
							ItemSum.Add(tempItemDescription);
						}
					}
				} else {
					if (TempBLNo != "_") {
						Current.PackedCount = TotalPackedCount + "";
						Current.Weight = TotalGrossWeight + "";
						Current.Volume = TotalVolume + "";

						itemTemp = new StringBuilder();
						for (int i = 0; i < ItemSum.Count; i++) {
							itemTemp.Append(", " + ItemSum[i]);
						}
						if (itemTemp.ToString() != "") {
							Current.Description = itemTemp.ToString().Substring(2);
						}
						ReturnValue.Add(Current);
						Current = new HouseBLN();
						count++;
						TotalPackedCount = 0;
						TotalGrossWeight = 0;
						TotalVolume = 0;
					}

					TempBLNo = RS["BLNo"] + "";
					TempRequestFormPk = RS["RequestFormPk"] + "";
					if (RS["PACKED_COUNT"] + "" != "") {
						TotalPackedCount += Int32.Parse(RS["PACKED_COUNT"] + "");
					}
					if (RS["TotalGrossWeight"] + "" != "") {
						TotalGrossWeight += decimal.Parse(RS["TotalGrossWeight"] + "");
					}
					if (RS["TotalVolume"] + "" != "") {
						TotalVolume += decimal.Parse(RS["TotalVolume"] + "");
					}
					string packingunit = RS["PackingUnit"] + "";
					switch (packingunit) {
						case "15": packingunit = "CT"; break;
						case "16": packingunit = "RL"; break;
						case "17": packingunit = "PT"; break;
						default: packingunit = "GT"; break;
					}
					ItemSum = new List<string>();
					string tempItemDescription = (RS["CICKD"] + "").Replace("\t", "").Trim();
					if (tempItemDescription != "") {
						bool isinitemsum = false;
						foreach (string each in ItemSum) {
							if (each == tempItemDescription) {
								isinitemsum = true;
								break;
							}
						}
						if (!isinitemsum) {
							ItemSum.Add(tempItemDescription);
						}
					}

					Current.HSN = (count + 1).ToString();
					Current.BLNo = TempBLNo;
					Current.ShipperName = (RS["Shipper"] + "").Trim();
					Current.ShipperAddress = RS["ShipperAddress"] + "";

					string tempCompanyName = (RS["Consignee"] + "").Trim();
					string tempSaupja = "";

					if (tempCompanyName != "" && tempCompanyName.Substring(tempCompanyName.Length - 1) == ")") {
						int tempStart = tempCompanyName.LastIndexOf("(") + 1;
						tempSaupja = tempCompanyName.Substring(tempStart, tempCompanyName.Length - 1 - tempStart);
						tempSaupja = tempSaupja.Trim();
						string[] arrSaupja = tempSaupja.Split(new string[] { "-" }, StringSplitOptions.None);
						if (arrSaupja.Length == 3) {
							tempSaupja = arrSaupja[0].Trim() + arrSaupja[1].Trim() + arrSaupja[2].Trim();
						}
						if (tempSaupja.Length == 10) {
							tempCompanyName = tempCompanyName.Substring(0, tempCompanyName.LastIndexOf("("));
						}
					}

					Current.ConsigneeName = tempCompanyName;
					Current.ConsigneeSaupjaNo = tempSaupja + "" == "" ? RS["CompanyNo"] + "" : tempSaupja;
					Current.ConsigneeSaupjaNo = Current.ConsigneeSaupjaNo.Replace("-", "").Trim();
					Current.ConsigneeAddress = RS["ConsigneeAddress"] + "";
					Current.PackingUnit = packingunit;

				}
			}
			if (TotalPackedCount > 0) {
				Current.PackedCount = TotalPackedCount + "";
				Current.Weight = TotalGrossWeight + "";
				Current.Volume = TotalVolume + "";

				itemTemp = new StringBuilder();
				for (int i = 0; i < ItemSum.Count; i++) {
					itemTemp.Append(", " + ItemSum[i]);
				}
				if (itemTemp.ToString() != "") {
					Current.Description = itemTemp.ToString().Substring(2);
				}
				ReturnValue.Add(Current);
			}
			RS.Dispose();
			DB_Intl2000.DBCon.Close();

			return ReturnValue;
		}
		public string Clear_YuhanMaster_Intl2000(string TBBHPk) {
			DB_Intl2000 = new DBConn();
			DB_Intl2000.DBCon.Open();
			DB_Intl2000.SqlCmd.CommandText = @"
DELETE FROM [INTL2010].[dbo].[Document]
		WHERE [DocumentPk] in (
SELECT [DocumentPk]
  FROM [INTL2010].[dbo].[Document]
  WHERE([Type]= 'MasterBLN' or[Type]= 'HouseBLN') and TypePk = " + TBBHPk + @"
  ); ";
			DB_Intl2000.SqlCmd.ExecuteNonQuery();
			DB_Intl2000.DBCon.Close();
			return "1";
		}
		public string SetSave_YuhanMaster_Intl2000(sYuhan_MasterN Current) {
			Clear_YuhanMaster_Intl2000(Current.TBBHPk);
			DB_Intl2000 = new DBConn();
			DB_Intl2000.DBCon.Open();

			DB_Intl2000.SqlCmd.CommandText = Utility.ToDB("[dbo].[Document]", new List<string[]>() {
				new string[] {"DocumentPk",  "", "int" },
				new string[] {"Type",  "MasterBLN" },
				new string[] {"TypePk",  Current.TBBHPk, "int"},
				new string[] {"Status",  "0" , "int"},
				new string[] {"Value0",  Current.FinalDate },
				new string[] {"Value1",  Current.MasterBLNo },
				new string[] {"Value2",  Current.ShipName },
				new string[] {"Value3",  Current.VoyageNo },
				new string[] {"Value4",  Current.MRN },
				new string[] {"Value5",  Current.MSN },
				new string[] {"Value6",  Current.Customs },
				new string[] {"Value7",  Current.Division},
				new string[] {"Value8",  Current.LineCode},
				new string[] {"Value9",  Current.FinalPort},
				new string[] {"Value10",  Current.AssignmentWH},
				new string[] {"Value11",  Current.Container.ContainerNo},
				new string[] {"Value12",  Current.Container.ContainerCode},
				new string[] {"Value13",  Current.Container.SealNo1},
				new string[] {"Value14",  Current.AssignmentCode},
				new string[] {"Value15",  Current.AssignmentName},
				new string[] {"Value16",  Current.FlagAACO},
				new string[] { "ValueInt0",  Current.TotalPackedCount.ToString(), "int" },
				new string[] { "ValueDecimal0",  Current.TotalVolume.ToString(), "decimal(18, 4)" },
				new string[] { "ValueDecimal1",  Current.TotalWeight.ToString(), "decimal(18, 4)" },
				new string[] { "ValueDecimal2",  Current.TotalAmount.ToString(), "decimal(18, 4)" },
			});

			string DocumentPk = DB_Intl2000.SqlCmd.ExecuteScalar() + "";

			StringBuilder query = new StringBuilder();
			for (var i = 0; i < Current.HouseBL.Count; i++) {
				HouseBLN house = Current.HouseBL[i];
				query.Append(Utility.ToDB("[dbo].[Document]", new List<string[]>() {
					new string[] {"DocumentPk",  "", "int" },
					new string[] {"Type",  "HouseBLN" },
					new string[] {"TypePk", Current.TBBHPk, "int"},
					new string[] {"Status",  "0" , "int"},
					new string[] {"Value0",  house.BLNo },
					new string[] {"Value1", house.PackedCount },
					new string[] {"Value2",  house.PackingUnit },
					new string[] {"Value3",  house.Weight },
					new string[] {"Value4", house.Volume },
					new string[] {"Value5",  house.Amount},
					new string[] {"Value6",  house.ShipperName},
					new string[] {"Value7",  house.ConsigneeName},
					new string[] {"Value8",  house.ShipperAddress},
					new string[] {"Value9",  house.ConsigneeAddress},
					new string[] {"Value10",  house.Description},
					new string[] {"Value11",  house.ConsigneeSaupjaNo},
					new string[] { "ValueInt0", house.HSN, "int" },
					new string[] { "ParentsType", "Document" },
					new string[] { "ParentsId", DocumentPk, "int" }
				}));
			}
			DB_Intl2000.SqlCmd.CommandText = query + "";
			DB_Intl2000.SqlCmd.ExecuteNonQuery();
			DB_Intl2000.DBCon.Close();
			return "1";
		}
		public String Search_PreCarriage(string Code) {
			DB_Yuhan = new DBConn("YuhanN");
			DB_Yuhan.SqlCmd.CommandText = string.Format(@"
SELECT  Name 
	 From [INTL_uBase].[dbo].[TSM_CodeDetail] 
	 Where GroupCode ='SIGNLINE'  and Code ='{0}'  and Use_YN = 'Y'  
	 ORDER BY Code ,Name", Code);
			DB_Yuhan.DBCon.Open();
			string ReturnValue = DB_Yuhan.SqlCmd.ExecuteScalar() + "";
			DB_Yuhan.DBCon.Close();
			return ReturnValue;
		}
		public String Search_Port(string Code) {
			DB_Yuhan = new DBConn("YuhanN");
			DB_Yuhan.SqlCmd.CommandText = string.Format(@"
SELECT  Name
From [INTL_uBase].[dbo].[TSM_CodeDetail] 
Where GroupCode ='CITYPORT'  And ETC1 = 'Ocean' And Code='{0}' AND Use_YN = 'Y'  
ORDER BY Code ;", Code);
			DB_Yuhan.DBCon.Open();
			string ReturnValue = DB_Yuhan.SqlCmd.ExecuteScalar() + "";
			DB_Yuhan.DBCon.Close();
			return ReturnValue;
		}
		public String Search_Warehouse(string Code) {
			DB_Yuhan = new DBConn("YuhanN");
			DB_Yuhan.SqlCmd.CommandText = string.Format(@"
SELECT  Name
From [INTL_uBase].[dbo].[TSM_CodeDetail] 
Where GroupCode ='BONDEDWAREHOUSE'  And ETC2 = 'OCEAN' And Code='{0}' AND Use_YN = 'Y'  
ORDER BY Code ;", Code);
			DB_Yuhan.DBCon.Open();
			string ReturnValue = DB_Yuhan.SqlCmd.ExecuteScalar() + "";
			DB_Yuhan.DBCon.Close();
			return ReturnValue;
		}
		public String Search_Assignment(string Code) {
			DB_Yuhan = new DBConn("YuhanN");
			DB_Yuhan.SqlCmd.CommandText = string.Format(@"
SELECT Name
From [INTL_uBase].[dbo].[TSM_CodeDetail] 
Where GroupCode ='EDIASSIGNMENT' And Code='{0}' AND Use_YN = 'Y'  
ORDER BY Code ;", Code);
			DB_Yuhan.DBCon.Open();
			string ReturnValue = DB_Yuhan.SqlCmd.ExecuteScalar() + "";
			DB_Yuhan.DBCon.Close();
			return ReturnValue;
		}
		public String Load_PreCarriageList() {
			DB_Yuhan = new DBConn("YuhanN");
			//			DB_Yuhan.SqlCmd.CommandText = @"
			//Select  Code, Name 
			//	 From [INTL_uBase].[dbo].[TSM_CodeDetail] 
			//	 Where GroupCode ='SIGNLINE'  and Name  Like '%%'  and Use_YN = 'Y'  
			//	 ORDER BY Code ,Name;";
			DB_Yuhan.SqlCmd.CommandText = @"
Select  Code, Name 
	 From [INTL_uBase].[dbo].[TSM_CodeDetail] 
	 Where GroupCode ='SIGNLINE'  and Name  Like '%%'  and Use_YN = 'Y'  
	 and code in (
select LineCode from INTL_uFMS.[dbo].TEDOI_mBL
group by LineCode )
ORDER BY Code ,Name;";
			StringBuilder ReturnValue = new StringBuilder();
			DB_Yuhan.DBCon.Open();
			SqlDataReader RS = DB_Yuhan.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				ReturnValue.Append("<tr><td onclick=\"SetInModal('" + RS[0] + "', '" + RS[1] + "');\">" + RS[0] + "</td><td onclick=\"SetInModal('" + RS[0] + "', '" + RS[1] + "');\">" + RS[1] + "</td></tr>");
			}
			RS.Dispose();
			DB_Yuhan.DBCon.Close();
			return @"
<table class=""table table-hover"">
	<thead>
		<tr>	
			<th>Code</th>
			<th>Title</th>
		</tr>
	</thead>
	<tbody>" + ReturnValue + @"</tbody>
</table>";
		}
		public String Load_PortList() {
			DB_Yuhan = new DBConn("YuhanN");

			DB_Yuhan.SqlCmd.CommandText = @"
SELECT  Code, Name
FROM [INTL_uBase].[dbo].[TSM_CodeDetail] 
Where GroupCode ='CITYPORT'  And ETC1 = 'Ocean' And code  in ('KRINC','KRPTK','KRPUS')  AND Use_YN = 'Y' 
ORDER BY Code 
 ";
			StringBuilder ReturnValue = new StringBuilder();
			DB_Yuhan.DBCon.Open();
			SqlDataReader RS = DB_Yuhan.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				ReturnValue.Append("<tr><td onclick=\"SetInModal('" + RS[0] + "', '" + RS[1] + "');\">" + RS[0] + "</td><td onclick=\"SetInModal('" + RS[0] + "', '" + RS[1] + "');\">" + RS[1] + "</td></tr>");
			}
			RS.Dispose();
			DB_Yuhan.DBCon.Close();
			return @"
<table class=""table table-hover"">
	<thead>
		<tr>	
			<th>Code</th>
			<th>Title</th>
		</tr>
	</thead>
	<tbody>" + ReturnValue + @"</tbody>
</table>";
		}
		public String Load_CustomsList() {
			DB_Yuhan = new DBConn("YuhanN");

			DB_Yuhan.SqlCmd.CommandText = @"
SELECT  Code, Name
FROM [INTL_uBase].[dbo].[TSM_CodeDetail] 
WHERE GroupCode ='SIGNCUSTOMS'  And Name  Like '%%'  AND Use_YN = 'Y'  ORDER BY Code 
 ";
			StringBuilder ReturnValue = new StringBuilder();
			DB_Yuhan.DBCon.Open();
			SqlDataReader RS = DB_Yuhan.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				ReturnValue.Append("<tr><td onclick=\"SetInModal('" + RS[0] + "', '" + RS[1] + "');\">" + RS[0] + "</td><td onclick=\"SetInModal('" + RS[0] + "', '" + RS[1] + "');\">" + RS[1] + "</td></tr>");
			}
			RS.Dispose();
			DB_Yuhan.DBCon.Close();
			return @"
<table class=""table table-hover"">
	<thead>
		<tr>	
			<th>Code</th>
			<th>Title</th>
		</tr>
	</thead>
	<tbody>" + ReturnValue + @"</tbody>
</table>";
		}
		public String Load_DivisionList() {
			DB_Yuhan = new DBConn("YuhanN");

			DB_Yuhan.SqlCmd.CommandText = @"
SELECT  Code, Name
FROM [INTL_uBase].[dbo].[TSM_CodeDetail] 
WHERE GroupCode ='SIGNCUSTOMSDV'  And Name  Like '%%'  AND Use_YN = 'Y'  ORDER BY Code 
 ";
			StringBuilder ReturnValue = new StringBuilder();
			DB_Yuhan.DBCon.Open();
			SqlDataReader RS = DB_Yuhan.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				ReturnValue.Append("<tr><td onclick=\"SetInModal('" + RS[0] + "', '" + RS[1] + "');\">" + RS[0] + "</td><td onclick=\"SetInModal('" + RS[0] + "', '" + RS[1] + "');\">" + RS[1] + "</td></tr>");
			}
			RS.Dispose();
			DB_Yuhan.DBCon.Close();
			return @"
<table class=""table table-hover"">
	<thead>
		<tr>	
			<th>Code</th>
			<th>Title</th>
		</tr>
	</thead>
	<tbody>" + ReturnValue + @"</tbody>
</table>";
		}
		public String Load_Assignment() {
			DB_Yuhan = new DBConn("YuhanN");

			DB_Yuhan.SqlCmd.CommandText = @"
SELECT  Code, Name
FROM [INTL_uBase].[dbo].[TSM_CodeDetail] 
WHERE GroupCode ='EDIASSIGNMENT'  AND Use_YN = 'Y'  
and code in ('G','A','C')
ORDER BY Code 
 ";
			StringBuilder ReturnValue = new StringBuilder();
			DB_Yuhan.DBCon.Open();
			SqlDataReader RS = DB_Yuhan.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				ReturnValue.Append("<tr><td onclick=\"SetInModal('" + RS[0] + "', '" + RS[1] + "');\">" + RS[0] + "</td><td onclick=\"SetInModal('" + RS[0] + "', '" + RS[1] + "');\">" + RS[1] + "</td></tr>");
			}
			RS.Dispose();
			DB_Yuhan.DBCon.Close();
			return @"
<table class=""table table-hover"">
	<thead>
		<tr>	
			<th>Code</th>
			<th>Title</th>
		</tr>
	</thead>
	<tbody>" + ReturnValue + @"</tbody>
</table>";
		}
		public String Load_Warehouse() {
			DB_Yuhan = new DBConn("YuhanN");

			DB_Yuhan.SqlCmd.CommandText = @"
Select  Code, Name
From [INTL_uBase].[dbo].[TSM_CodeDetail]
Where GroupCode ='BONDEDWAREHOUSE' AND Etc2='OCEAN' AND Use_YN = 'Y'
AND Code in (SELECT [AssignmentWH] FROM [INTL_uFMS].[dbo].[TEDOI_HBL]
group by AssignmentWH)
ORDER BY Code;";
			StringBuilder ReturnValue = new StringBuilder();
			DB_Yuhan.DBCon.Open();
			SqlDataReader RS = DB_Yuhan.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				ReturnValue.Append("<tr><td onclick=\"SetInModal('" + RS[0] + "', '" + RS[1] + "');\">" + RS[0] + "</td><td onclick=\"SetInModal('" + RS[0] + "', '" + RS[1] + "');\">" + RS[1] + "</td></tr>");
			}
			RS.Dispose();
			DB_Yuhan.DBCon.Close();
			return @"
<table class=""table table-hover"">
	<thead>
		<tr>	
			<th>Code</th>
			<th>Title</th>
		</tr>
	</thead>
	<tbody>" + ReturnValue + @"</tbody>
</table>";
		}
		public String Load_History_Description(string BLNo) {
			DB_Intl2000 = new DBConn();
			DB_Intl2000.SqlCmd.CommandText = @"
				DECLARE @Consignee nvarchar(300); 
				SELECT @Consignee =[Value7] 
				  FROM [INTL2010].[dbo].[Document] 
				  WHERE [Type]='HouseBLN' and Value0='" + BLNo + @"'

				SELECT CASE D.TypePk WHEN 0 THEN D.Value0 Else MasterBL.Value1 END AS BLNo, MasterBL.Value0 DATE, D.[Value10] Descrition 
				  FROM [INTL2010].[dbo].[Document] AS D 
					left join [INTL2010].[dbo].[Document]  AS MasterBL ON D.ParentsId=MasterBL.DocumentPk 
				  WHERE D.[Type]='HouseBLN' and D.Value7=@Consignee and D.Value0 is not null and  D.Value10 is not null and D.Value0<>'" + BLNo + "' ORDER BY MasterBL.Value1 ASC, D.Value1 DESC ;";
			StringBuilder ReturnValue = new StringBuilder();
			DB_Intl2000.DBCon.Open();
			SqlDataReader RS = DB_Intl2000.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				ReturnValue.Append("<tr style='cursor:pointer;' onclick=\"SetInModal('" + BLNo + "', '" + RS["Descrition"].ToString().ToUpper() + "');\"><td>" + RS["DATE"] + "</td><td>" + RS["BLNo"] + "</td><td>" + RS["Descrition"] + "</td></tr>");
			}
			RS.Dispose();
			DB_Intl2000.DBCon.Close();
			if (ReturnValue.ToString() != "") {
				return @"
<table class=""table table-hover"">
	<thead>
		<tr>	
			<th>날짜</th>
			<th>BLNo</th>
			<th>Description</th>
		</tr>
	</thead>
	<tbody>" + ReturnValue + @"</tbody>
</table>";
			} else {
				return "";
			}
		}

		public String Load_SHPREQNo() {
			DB_Yuhan = new DBConn("YuhanN");
			DB_Yuhan.SqlCmd.CommandText = @"
USE [impsea]; 
SELECT TOP 1 SHPREQNo 
FROM [impsea].[dbo].[SHPREQ]
ORDER BY FirstDate DESC, SHPREQNo DESC; ";
			DB_Yuhan.DBCon.Open();
			string recentNo = DB_Yuhan.SqlCmd.ExecuteScalar() + "";

			int ReturnValue = Int32.Parse(recentNo);
			while (true) {
				ReturnValue++;
				DB_Yuhan.SqlCmd.CommandText = @"
					SELECT COUNT(*)  
					FROM [impsea].[dbo].[SHPREQ]
					WHERE SHPREQNo='" + ReturnValue + "';";
				if ("0" == DB_Yuhan.SqlCmd.ExecuteScalar() + "") {
					break;
				}
			}
			DB_Yuhan.DBCon.Close();
			return ReturnValue.ToString();
		}
		public string SetSave_YuhanMaster_Yuhan(sYuhan_MasterN Current) {
			List<string> Query = new List<string>();
			if (GetQuery_CheckYuhan(Current)) {
				return "0";
			}
			Query.Add(GetQuery_SetMasterBL(Current));
			for (var i = 0; i < Current.HouseBL.Count; i++) {
				Query.Add(GetQuery_SetHouseBL(Current, i));
			}

			DB_Yuhan = new DBConn("YuhanN");
			DB_Yuhan.DBCon.Open();

			foreach (string eachQuery in Query) {
				//ReturnValue.Append("AAAAQQQQ"+eachQuery);
				DB_Yuhan.SqlCmd.CommandText = eachQuery;
				try {

					DB_Yuhan.SqlCmd.ExecuteNonQuery();
				} catch (Exception) {

					return "2";
				}
			}
			DB_Yuhan.DBCon.Close();


			return "1";
		}

		private bool GetQuery_CheckYuhan(sYuhan_MasterN Current) {

			string Check = "";
			DB_Yuhan = new DBConn("YuhanN");
			DB_Yuhan.DBCon.Open();
			if (Current.FlagAACO == "AACO") {
				DB_Yuhan.SqlCmd.CommandText = @"SELECT Count(*) from [AACO_uFMS].[dbo].[TEDOI_MBL] WHERE MBLNo='" + Current.MasterBLNo + "';";
			}
			else {
				DB_Yuhan.SqlCmd.CommandText = @"SELECT Count(*) from [INTL_uFMS].[dbo].[TEDOI_MBL] WHERE MBLNo='" + Current.MasterBLNo + "';";
			}
			Check = DB_Yuhan.SqlCmd.ExecuteScalar() + "";
			DB_Yuhan.DBCon.Close();
			if (Check == "0") { return false; } else { return true; }

		}
		private string LoadHouseRefNo(string HouseBLSum) {
			DB_Yuhan = new DBConn("YuhanN");
			DB_Yuhan.SqlCmd.CommandText = @" 
SELECT RefNo FROM [INTL_uFMS].[dbo].[TEDOI_HBL] WHERE HBLNo in (" + HouseBLSum.ToString().Substring(1) + @")";
			DB_Yuhan.DBCon.Open();
			SqlDataReader RS = DB_Yuhan.SqlCmd.ExecuteReader();
			StringBuilder ReturnValue = new StringBuilder();
			while (RS.Read()) {
				ReturnValue.Append(", '" + RS["RefNo"].ToString() + "'");
			}
			RS.Dispose();
			DB_Yuhan.DBCon.Close();
			return ReturnValue + "";
		}
		private string Make_RefNo(string Division, string House_Count = null) {
			string RefNo_Left = "EDOI" + Division + DateTime.Today.ToString("yy") + DateTime.Today.ToString("MM");
			DB_Yuhan = new DBConn("YuhanN");
			string Query = "";
			if (Division == "M") {
				Query = @"
SELECT TOP 1 RefNo FROM [INTL_uFMS].[dbo].[TEDOI_MBL]
WHERE left(RefNo, 9)='" + RefNo_Left + @"' 
ORDER BY RefNo DESC;";
				DB_Yuhan.SqlCmd.CommandText = Query;
				DB_Yuhan.DBCon.Open();
				decimal TempNo = 0;
				SqlDataReader RS = DB_Yuhan.SqlCmd.ExecuteReader();
				if (RS.Read()) {
					TempNo = decimal.Parse(RS["RefNo"].ToString().Substring(9, 5)) + 1;
				}
				else {
					TempNo = 1;
				}
				DB_Yuhan.DBCon.Close();
				RS.Dispose();
				if (House_Count != "" && House_Count != null) {
					TempNo = TempNo + decimal.Parse(House_Count);
				}
				return RefNo_Left + TempNo.ToString("00000");
			}
			else {
				//H House
				Query = @"
SELECT TOP 1 RefNo FROM [INTL_uFMS].[dbo].[TEDOI_HBL]
WHERE left(RefNo, 9)='" + RefNo_Left + @"' and len(RefNo)=16
ORDER BY RefNo DESC;";
				DB_Yuhan.SqlCmd.CommandText = Query;
				DB_Yuhan.DBCon.Open();
				decimal TempNo = 0;
				SqlDataReader RS = DB_Yuhan.SqlCmd.ExecuteReader();
				if (RS.Read()) {
					TempNo = decimal.Parse(RS["RefNo"].ToString().Substring(9, 7)) + 1;
				}
				else {
					TempNo = 1;
				}
				DB_Yuhan.DBCon.Close();
				RS.Dispose();
				if (House_Count != "" && House_Count != null) {
					TempNo = TempNo + decimal.Parse(House_Count);
				}
				return RefNo_Left + TempNo.ToString("0000000");
			}
		}
		private string Make_RefNo_AACO(string Division, string House_Count = null) {
			string RefNo_Left = "EDOI" + Division + DateTime.Today.ToString("yy") + DateTime.Today.ToString("MM");
			DB_Yuhan = new DBConn("YuhanN");
			string Query = "";
			if (Division == "M") {
				Query = @"
SELECT TOP 1 RefNo FROM [AACO_uFMS].[dbo].[TEDOI_MBL]
WHERE left(RefNo, 9)='" + RefNo_Left + @"' 
ORDER BY RefNo DESC;";
				DB_Yuhan.SqlCmd.CommandText = Query;
				DB_Yuhan.DBCon.Open();
				decimal TempNo = 0;
				SqlDataReader RS = DB_Yuhan.SqlCmd.ExecuteReader();
				if (RS.Read()) {
					TempNo = decimal.Parse(RS["RefNo"].ToString().Substring(9, 5)) + 1;
				}
				else {
					TempNo = 1;
				}
				DB_Yuhan.DBCon.Close();
				RS.Dispose();
				if (House_Count != "" && House_Count != null) {
					TempNo = TempNo + decimal.Parse(House_Count);
				}
				return RefNo_Left + TempNo.ToString("00000");
			}
			else {
				//H House
				Query = @"
SELECT TOP 1 RefNo FROM [AACO_uFMS].[dbo].[TEDOI_HBL]
WHERE left(RefNo, 9)='" + RefNo_Left + @"' and len(RefNo)=16
ORDER BY RefNo DESC;";
				DB_Yuhan.SqlCmd.CommandText = Query;
				DB_Yuhan.DBCon.Open();
				decimal TempNo = 0;
				SqlDataReader RS = DB_Yuhan.SqlCmd.ExecuteReader();
				if (RS.Read()) {
					TempNo = decimal.Parse(RS["RefNo"].ToString().Substring(9, 7)) + 1;
				}
				else {
					TempNo = 1;
				}
				DB_Yuhan.DBCon.Close();
				RS.Dispose();
				if (House_Count != "" && House_Count != null) {
					TempNo = TempNo + decimal.Parse(House_Count);
				}
				return RefNo_Left + TempNo.ToString("0000000");
			}
		}
		private string Make_Customs_RefNo() {
			string RefNo_Left = "SE0001" + DateTime.Today.ToString("yy");
			DB_Yuhan.SqlCmd.CommandText = @"
SELECT TOP 1 Customs_RefNo FROM [INTL_uFMS].[dbo].[TEDOI_MBL]
WHERE left(Customs_RefNo, 8)='" + RefNo_Left + @"'
ORDER BY Customs_RefNo DESC;";
			DB_Yuhan.DBCon.Open();
			decimal TempNo = 0;
			SqlDataReader RS = DB_Yuhan.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				TempNo = decimal.Parse(RS["Customs_RefNo"].ToString().Substring(8, 8)) + 1;
			} else {
				TempNo = 1;
			}
			DB_Yuhan.DBCon.Close();
			RS.Dispose();
			return RefNo_Left + TempNo.ToString("00000000");
		}
		private string Make_Customs_RefNo_AACO() {
			string RefNo_Left = "SE0001" + DateTime.Today.ToString("yy");
			DB_Yuhan.SqlCmd.CommandText = @"
SELECT TOP 1 Customs_RefNo FROM [AACO_uFMS].[dbo].[TEDOI_MBL]
WHERE left(Customs_RefNo, 8)='" + RefNo_Left + @"'
ORDER BY Customs_RefNo DESC;";
			DB_Yuhan.DBCon.Open();
			decimal TempNo = 0;
			SqlDataReader RS = DB_Yuhan.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				TempNo = decimal.Parse(RS["Customs_RefNo"].ToString().Substring(8, 8)) + 1;
			}
			else {
				TempNo = 1;
			}
			DB_Yuhan.DBCon.Close();
			RS.Dispose();
			return RefNo_Left + TempNo.ToString("00000000");
		}
		private String GetQuery_SetMasterBL(sYuhan_MasterN Current) {
			string Query;

			if (Current.FlagAACO == "AACO") {
				MRefNo = Make_RefNo_AACO("M");
				string Customs_RefNo = Make_Customs_RefNo_AACO();
				Query = @"
INSERT INTO [AACO_uFMS].[dbo].[TEDOI_MBL] (
RefNo , MRN , MSN , MBLNo , LineCode ,
LineCustomerID , VesselName , Voyage , LineCallSign , OBDate ,
ARDate , OrigCode , DestCode , Customs , CustomsDepart ,
Verification , VerificationDO , MF_SendType , MF_SendDateTime , MF_SendAttn ,
DO_SendType , DO_SendDateTime , DO_SendAttn , Auto_CreateType , Auto_CreateDateTime ,
Auto_CreateAttn , Customs_CreateType , Customs_CreateDateTime , Customs_CreateAttn , Last_SendType , 
Last_SendDateTime , Puncher , PuncherDate , ModifyUser , ModifyDate , 
EdiHost , JobStatus  , BranchCode  , Customs_RefNo , DeclareDate  ) 
VALUES (  
'" + MRefNo + @"',  '" + Current.MRN + @"',  '" + Current.MSN + @"',  '" + Current.MasterBLNo + @"',  '" + Current.LineCode + @"',
'',  '" + Current.ShipName + @"',  '" + Current.VoyageNo + @"',  '',  '" + Current.FinalDate + @"', 
'" + Current.FinalDate + @"',  '',  '" + Current.FinalPort + @"',  '" + Current.Customs + @"',  '" + Current.Division + @"', 
'',  '',  '',  '',  '', 
'',  '',  '',  'Create',  getdate(),
'IL',  '',  '',  '',  '', 
'',  'IL',  getdate(),  'IL',  getdate() ,
'KTNET'  ,'MI01'  ,'INTL01' , '" + Customs_RefNo + @"' ,getdate());";
			}
			else {
				MRefNo = Make_RefNo("M");
				string Customs_RefNo = Make_Customs_RefNo();
				Query = @"
INSERT INTO [INTL_uFMS].[dbo].[TEDOI_MBL] (
RefNo , MRN , MSN , MBLNo , LineCode ,
LineCustomerID , VesselName , Voyage , LineCallSign , OBDate ,
ARDate , OrigCode , DestCode , Customs , CustomsDepart ,
Verification , VerificationDO , MF_SendType , MF_SendDateTime , MF_SendAttn ,
DO_SendType , DO_SendDateTime , DO_SendAttn , Auto_CreateType , Auto_CreateDateTime ,
Auto_CreateAttn , Customs_CreateType , Customs_CreateDateTime , Customs_CreateAttn , Last_SendType , 
Last_SendDateTime , Puncher , PuncherDate , ModifyUser , ModifyDate , 
EdiHost , JobStatus  , BranchCode  , Customs_RefNo , DeclareDate  ) 
VALUES (  
'" + MRefNo + @"',  '" + Current.MRN + @"',  '" + Current.MSN + @"',  '" + Current.MasterBLNo + @"',  '" + Current.LineCode + @"',
'',  '" + Current.ShipName + @"',  '" + Current.VoyageNo + @"',  '',  '" + Current.FinalDate + @"', 
'" + Current.FinalDate + @"',  '',  '" + Current.FinalPort + @"',  '" + Current.Customs + @"',  '" + Current.Division + @"', 
'',  '',  '',  '',  '', 
'',  '',  '',  'Create',  getdate(),
'IL',  '',  '',  '',  '', 
'',  'IL',  getdate(),  'IL',  getdate() ,
'KTNET'  ,'MI01'  ,'INTL01' , '" + Customs_RefNo + @"' ,getdate());";
			}

			
			return Query;
		}
		private String GetQuery_SetHouseBL(sYuhan_MasterN Current, int Count_HouseBL) {
			string HRefNo = Make_RefNo("H", Count_HouseBL.ToString());
			StringBuilder Query = new StringBuilder();
			if (Current.FlagAACO == "AACO") {
				Query.Append(@"
INSERT INTO [AACO_uFMS].[dbo].[TEDOI_HBL] (
RefNo , HBLNo , MBLNo , MRefNo , BLTypeIE , 
HSN , EDIFlag , Pieces , PiecesUnit , GWT , 
KgUnit , CBM , LoadingType , CommodityItem , AssignmentCode ,
AssignmentName , AssignmentWH , ElectronicYN , CustomsUseType , CurrencyCode ,
AmountTotal , CustomsTypeYN , FWDCode , Remark , ShipperCode , 
ShipperName , ShipperAddress , ShipperAddressA , ShipperAddressB , ShipperAddressC , 
ShipperAddressD , ShipperTel , ShipperFax , ShipperBusinessID , ShipperZipcode , 
ShipperCountryCode , ShipperState , ShipperCity , ConsigneeCode , ConsigneeName , 
ConsigneeAddress , ConsigneeAddressA , ConsigneeAddressB , ConsigneeAddressC , ConsigneeAddressD , 
ConsigneeTel , ConsigneeFax , ConsigneeBusinessID , ConsigneeZipcode ,ConsigneeCountryCode , 
ConsigneeState , ConsigneeCity , NotifyCode , NotifyName ,NotifyAddress , 
NotifyAddressA , NotifyAddressB , NotifyAddressC , NotifyAddressD , NotifyTel ,
NotifyFax , NotifyBusinessID , NotifyZipcode , NotifyCountryCode , NotifyState , 
NotifyCity , ArrivalNoticeYN , TransNoticeYN , DeliveryOrderYN ,CargoProperty ,
TruckingCode , DisChargeType , DischargePlace , LCNo ,SpecialCargo , 
SpecialCargoA , SpecialCargoB , BillOrigCode ,BillDestCode , HSCode ,
MF_SendType , MF_SendDateTime , MF_SendAttn ,DO_SendType , DO_SendDateTime , 
DO_SendAttn , Auto_CreateType , Auto_CreateDateTime , Auto_CreateAttn , Customs_CreateType ,
Customs_CreateDateTime , Customs_CreateAttn , Puncher , PuncherDate , ModifyUser , 
ModifyDate , ElectronicURL , ItemCode , RealPieces , CustomsTypeGW , 
CheckReqYN , Special_ShipperCode  ) 
VALUES (  
'" + HRefNo + @"',  '" + Current.HouseBL[Count_HouseBL].BLNo + @"',  '" + Current.MasterBLNo + @"',  '" + MRefNo + @"',  'I', 
'" + int.Parse(Current.HouseBL[Count_HouseBL].HSN).ToString("0000") + @"','N'," + decimal.Parse(Current.HouseBL[Count_HouseBL].PackedCount).ToString("########0") + @",'" + Current.HouseBL[Count_HouseBL].PackingUnit + @"',  " + decimal.Parse(Current.HouseBL[Count_HouseBL].Weight.Replace(",", "")).ToString("###############0.000") + @", 
'KG',  " + decimal.Parse(Current.HouseBL[Count_HouseBL].Volume.Replace(",", "")).ToString("###########0.000") + @",  '',  '" + Current.HouseBL[Count_HouseBL].Description + @"',  '" + Current.AssignmentCode + @"',
'" + Current.AssignmentName + @"',  '" + Current.AssignmentWH + @"',  '',  '',  'USD', 
1,  'N',  '',  '',  '',
'" + Current.HouseBL[Count_HouseBL].ShipperName + @"',  '" + Current.HouseBL[Count_HouseBL].ShipperAddress + @"',  '" + Current.HouseBL[Count_HouseBL].ShipperAddressA + @"',  '" + Current.HouseBL[Count_HouseBL].ShipperAddressB + @"',  '" + Current.HouseBL[Count_HouseBL].ShipperAddressC + @"', 
'" + Current.HouseBL[Count_HouseBL].ShipperAddressD + @"',  '',  '',  '',  '', 
'',  '',  '',  '',  '" + Current.HouseBL[Count_HouseBL].ConsigneeName + @"', 
'" + Current.HouseBL[Count_HouseBL].ConsigneeAddress + @"',  '" + Current.HouseBL[Count_HouseBL].ConsigneeAddressA + @"',  '" + Current.HouseBL[Count_HouseBL].ConsigneeAddressB + @"',  '" + Current.HouseBL[Count_HouseBL].ConsigneeAddressC + @"',  '" + Current.HouseBL[Count_HouseBL].ConsigneeAddressD + @"',
'',  '',  '" + Current.HouseBL[Count_HouseBL].ConsigneeSaupjaNo + @"',  '',  '',
'',  '',  '',  'SAME AS ABOVE',  '', 
'',  '',  '',  '',  '',
'',  '',  '',  '',  '',
'',  'Y',  'N',  'N',  '', 
'',  '',  '',  '',  '', 
'',  '',  '',  '',  '', 
'',  '',  '',  '',  '',
'',  'Create',  getdate(),  'IL',  '',
'',  '',  'IL',  getdate(),  'IL', 
getdate(),  '',  ''  ,0 ,'0' ,
'N' , ''  ) 
");

				Query.Append(@"
   INSERT INTO [AACO_uFMS].[dbo].[TEDOI_HBL_BaseContainer] (   
RefNo , SeqNo , ContainerNo , ContainerCode , SealNo1 , SealNo2 , SealNo3 , Pieces , PiecesUnit , GWT  , KgUnit  , CBM  ) 
VALUES (  
'" + HRefNo + @"',  '',  '" + Current.Container.ContainerNo + @"',  '" + Current.Container.ContainerCode + @"',  '" + Current.Container.SealNo1 + @"',  '',  '',  '" + Current.HouseBL[Count_HouseBL].PackedCount + @"',  '" + Current.HouseBL[Count_HouseBL].PackingUnit + @"',  " + decimal.Parse(Current.HouseBL[Count_HouseBL].Weight).ToString("###########0.000") + @",  'KG',  " + decimal.Parse(Current.HouseBL[Count_HouseBL].Volume).ToString("###########0.000") + @") 
");
			}
			else {
				Query.Append(@"
INSERT INTO [INTL_uFMS].[dbo].[TEDOI_HBL] (
RefNo , HBLNo , MBLNo , MRefNo , BLTypeIE , 
HSN , EDIFlag , Pieces , PiecesUnit , GWT , 
KgUnit , CBM , LoadingType , CommodityItem , AssignmentCode ,
AssignmentName , AssignmentWH , ElectronicYN , CustomsUseType , CurrencyCode ,
AmountTotal , CustomsTypeYN , FWDCode , Remark , ShipperCode , 
ShipperName , ShipperAddress , ShipperAddressA , ShipperAddressB , ShipperAddressC , 
ShipperAddressD , ShipperTel , ShipperFax , ShipperBusinessID , ShipperZipcode , 
ShipperCountryCode , ShipperState , ShipperCity , ConsigneeCode , ConsigneeName , 
ConsigneeAddress , ConsigneeAddressA , ConsigneeAddressB , ConsigneeAddressC , ConsigneeAddressD , 
ConsigneeTel , ConsigneeFax , ConsigneeBusinessID , ConsigneeZipcode ,ConsigneeCountryCode , 
ConsigneeState , ConsigneeCity , NotifyCode , NotifyName ,NotifyAddress , 
NotifyAddressA , NotifyAddressB , NotifyAddressC , NotifyAddressD , NotifyTel ,
NotifyFax , NotifyBusinessID , NotifyZipcode , NotifyCountryCode , NotifyState , 
NotifyCity , ArrivalNoticeYN , TransNoticeYN , DeliveryOrderYN ,CargoProperty ,
TruckingCode , DisChargeType , DischargePlace , LCNo ,SpecialCargo , 
SpecialCargoA , SpecialCargoB , BillOrigCode ,BillDestCode , HSCode ,
MF_SendType , MF_SendDateTime , MF_SendAttn ,DO_SendType , DO_SendDateTime , 
DO_SendAttn , Auto_CreateType , Auto_CreateDateTime , Auto_CreateAttn , Customs_CreateType ,
Customs_CreateDateTime , Customs_CreateAttn , Puncher , PuncherDate , ModifyUser , 
ModifyDate , ElectronicURL , ItemCode , RealPieces , CustomsTypeGW , 
CheckReqYN , Special_ShipperCode  ) 
VALUES (  
'" + HRefNo + @"',  '" + Current.HouseBL[Count_HouseBL].BLNo + @"',  '" + Current.MasterBLNo + @"',  '" + MRefNo + @"',  'I', 
'" + int.Parse(Current.HouseBL[Count_HouseBL].HSN).ToString("0000") + @"','N'," + decimal.Parse(Current.HouseBL[Count_HouseBL].PackedCount).ToString("########0") + @",'" + Current.HouseBL[Count_HouseBL].PackingUnit + @"',  " + decimal.Parse(Current.HouseBL[Count_HouseBL].Weight.Replace(",", "")).ToString("###############0.000") + @", 
'KG',  " + decimal.Parse(Current.HouseBL[Count_HouseBL].Volume.Replace(",", "")).ToString("###########0.000") + @",  '',  '" + Current.HouseBL[Count_HouseBL].Description + @"',  '" + Current.AssignmentCode + @"',
'" + Current.AssignmentName + @"',  '" + Current.AssignmentWH + @"',  '',  '',  'USD', 
1,  'N',  '',  '',  '',
'" + Current.HouseBL[Count_HouseBL].ShipperName + @"',  '" + Current.HouseBL[Count_HouseBL].ShipperAddress + @"',  '" + Current.HouseBL[Count_HouseBL].ShipperAddressA + @"',  '" + Current.HouseBL[Count_HouseBL].ShipperAddressB + @"',  '" + Current.HouseBL[Count_HouseBL].ShipperAddressC + @"', 
'" + Current.HouseBL[Count_HouseBL].ShipperAddressD + @"',  '',  '',  '',  '', 
'',  '',  '',  '',  '" + Current.HouseBL[Count_HouseBL].ConsigneeName + @"', 
'" + Current.HouseBL[Count_HouseBL].ConsigneeAddress + @"',  '" + Current.HouseBL[Count_HouseBL].ConsigneeAddressA + @"',  '" + Current.HouseBL[Count_HouseBL].ConsigneeAddressB + @"',  '" + Current.HouseBL[Count_HouseBL].ConsigneeAddressC + @"',  '" + Current.HouseBL[Count_HouseBL].ConsigneeAddressD + @"',
'',  '',  '" + Current.HouseBL[Count_HouseBL].ConsigneeSaupjaNo + @"',  '',  '',
'',  '',  '',  'SAME AS ABOVE',  '', 
'',  '',  '',  '',  '',
'',  '',  '',  '',  '',
'',  'Y',  'N',  'N',  '', 
'',  '',  '',  '',  '', 
'',  '',  '',  '',  '', 
'',  '',  '',  '',  '',
'',  'Create',  getdate(),  'IL',  '',
'',  '',  'IL',  getdate(),  'IL', 
getdate(),  '',  ''  ,0 ,'0' ,
'N' , ''  ) 
");

				Query.Append(@"
   INSERT INTO [INTL_uFMS].[dbo].[TEDOI_HBL_BaseContainer] (   
RefNo , SeqNo , ContainerNo , ContainerCode , SealNo1 , SealNo2 , SealNo3 , Pieces , PiecesUnit , GWT  , KgUnit  , CBM  ) 
VALUES (  
'" + HRefNo + @"',  '',  '" + Current.Container.ContainerNo + @"',  '" + Current.Container.ContainerCode + @"',  '" + Current.Container.SealNo1 + @"',  '',  '',  '" + Current.HouseBL[Count_HouseBL].PackedCount + @"',  '" + Current.HouseBL[Count_HouseBL].PackingUnit + @"',  " + decimal.Parse(Current.HouseBL[Count_HouseBL].Weight).ToString("###########0.000") + @",  'KG',  " + decimal.Parse(Current.HouseBL[Count_HouseBL].Volume).ToString("###########0.000") + @") 
");
			}
			return Query.ToString();
		}
	}


	public struct sYuhan_MasterN
	{
		public string DocumentPk;
		public string TBBHPk;
		public int TotalPackedCount;
		public decimal TotalAmount;
		public decimal TotalWeight;
		public decimal TotalVolume;
		public string RefNo;
		public string MRN;
		public string MSN;
		public string MasterBLNo;
		public string LineCode;
		public string ShipName;
		public string VoyageNo;
		public string FinalDate;
		public string FinalPort;
		public string Customs;
		public string Division;
		public string FlagAACO;
		public List<HouseBLN> HouseBL;
		public ContainerN Container;
		public string AssignmentWH;
		public string AssignmentCode;
		public string AssignmentName;
	}

	public struct HouseBLN
	{
		public string RefNo;
		public string BLNo;
		public string HSN;
		public string ShipperName;
		public string ShipperAddress;
		public string ShipperAddressA;
		public string ShipperAddressB;
		public string ShipperAddressC;
		public string ShipperAddressD;
		public string ConsigneeName;
		public string ConsigneeAddress;
		public string ConsigneeAddressA;
		public string ConsigneeAddressB;
		public string ConsigneeAddressC;
		public string ConsigneeAddressD;
		public string ConsigneeSaupjaNo;
		public string NortifyName;
		public string Description;
		public string PackedCount;
		public string PackingUnit;
		public string Weight;
		public string Volume;
		public string Amount;
	}
	public struct ContainerN
	{
		public string RefNo;
		public string SeqNo;
		public string ContainerNo;
		public string ContainerCode;
		public string SealNo1;

	}
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.cClearance
{
	public class Yuhan
	{
		private DBConn DB_Yuhan;
		private DBConn DB_Intl2000;
		public Yuhan() { }

		public sYuhan_Master Load_YuhanMaster_FromIntl2000(string TBBHPk) {
			sYuhan_Master Current = new sYuhan_Master();
			Current.TBBHPk = TBBHPk;
			DB_Intl2000 = new DBConn();
			DB_Intl2000.SqlCmd.CommandText = @"
				SELECT 
					[DocumentPk],[Type],[TypePk],[Status]
					,[Value0],[Value1],[Value2],[Value3],[Value4],[Value5],[Value6],[Value7],[Value8],[Value9]
					,[Value10],[Value11],[Value12],[Value13],[Value14],[Value15],[Value16],[Value17]
					,isnull([ValueInt0], 0) AS ValueInt0
					,isnull([ValueDecimal0], 0) AS ValueDecimal0 
					,[ParentsType],[ParentsId] 
				FROM [INTL2010].[dbo].[Document] 
				WHERE [Type] in ('MasterBL', 'HouseBL') and TypePk=" + TBBHPk + @"
				ORDER BY [Type] DESC, ValueInt0 ASC ;";
			DB_Intl2000.DBCon.Open();
			SqlDataReader RS = DB_Intl2000.SqlCmd.ExecuteReader();

			Current.SHPREQ = new SHPREQ();
			Current.BLADVI = new BLADVI();
			Current.BLADVISettle = new BLADVISettle();
			Current.BLADVIQuantity = new BLADVIQuantity();
			Current.CUSREP = new CUSREP();
			Current.SHPREQAddr = new SHPREQAddr();
			Current.PreCarriage = new Code();
			Current.LoadPort = new Code();
			Current.FinalPort = new Code();
			Current.Partner = new Code();
			Current.ACC = new Code();
			Current.ShipperAddress = new Address();
			Current.ConsigneeAddress = new Address();
			Current.NotifyAddress = new Address();
			Current.CUSMANEquipment = new CUSMANEquipment();
			Current.Warehouse = new Code();
			Current.CUSMAN_Assignment = new Code();
			Current.HouseBL = new List<HouseBL>();

			var house = new HouseBL();

			while (RS.Read()) {
				if (RS["Type"] + "" == "MasterBL") {
					Current.DocumentPk = RS["DocumentPk"] + "";
					Current.SHPREQ.SHPREQNo = RS["Value0"] + "";
					Current.SHPREQ.SHPREQDate = RS["Value1"] + "";
					Current.SHPREQ.BLADVIMaster = RS["Value2"] + "";
					Current.SHPREQ.ShipName = RS["Value3"] + "";
					Current.SHPREQ.VoyageNo = RS["Value4"] + "";
					Current.SHPREQ.SHPCode = RS["Value5"] + "";
					Current.SHPREQ.SHPName = RS["Value6"] + "";
					Current.SHPREQ.CNECode = RS["Value7"] + "";
					Current.SHPREQ.CNEName = RS["Value8"] + "";
					Current.SHPREQ.CNEAcct = RS["Value9"] + "";
					Current.ShipperAddress.ADDR_ENG1 = RS["Value10"] + "";
					Current.ShipperAddress.ADDR_ENG2 = RS["Value11"] + "";
					Current.ShipperAddress.ADDR_ENG3 = RS["Value12"] + "";
					Current.ConsigneeAddress.ADDR_ENG1 = RS["Value13"] + "";
					Current.ConsigneeAddress.ADDR_ENG2 = RS["Value14"] + "";
					Current.ConsigneeAddress.ADDR_ENG3 = RS["Value15"] + "";
					Current.CUSREP.MRN = RS["Value16"] + "";
					Current.CUSREP.MSN = RS["Value17"] + "";
					Current.TotalPackedCount = Int32.Parse(RS["ValueInt0"] + "");
					Current.TotalVolume = decimal.Parse(RS["ValueDecimal0"] + "");
				} else {
					house = new HouseBL();
					house.ShipperAddress = new Address();
					house.ConsigneeAddress = new Address();
					house.BLNo = RS["Value0"] + "";
					house.PackedCount = RS["Value1"] + "";
					house.PackingUnit = RS["Value2"] + "";
					house.Weight = RS["Value3"] + "";
					house.Volume = RS["Value4"] + "";
					house.ShipperCode = RS["Value5"] + "";
					house.ShipperName = RS["Value6"] + "";
					house.ConsigneeCode = RS["Value7"] + "";
					house.ConsigneeName = RS["Value8"] + "";
					house.ConsigneeSaupjaNo = RS["Value9"] + "";
					house.Description = RS["Value10"] + "";
					house.ShipperAddress.ADDR_ENG1 = RS["Value11"] + "";
					house.ShipperAddress.ADDR_ENG2 = RS["Value12"] + "";
					house.ShipperAddress.ADDR_ENG3 = RS["Value13"] + "";
					house.ConsigneeAddress.ADDR_ENG1 = RS["Value14"] + "";
					house.ConsigneeAddress.ADDR_ENG2 = RS["Value15"] + "";
					house.ConsigneeAddress.ADDR_ENG3 = RS["Value16"] + "";
					house.HSN = RS["ValueInt0"] + "";
					Current.HouseBL.Add(house);
				}
			}
			RS.Close();

			DB_Intl2000.SqlCmd.CommandText = @"
				SELECT 
					[DocumentBodyPk],[DocumentPk],[Category]
					,[Value0],[Value1],[Value2],[Value3],[Value4],[Value5],[Value6],[Value7],[Value8],[Value9]
					, isnull([ValueInt0], 0) AS ValueInt0
					, isnull([ValueDecimal0], 0) AS ValueDecimal0
				FROM [INTL2010].[dbo].[DocumentBody] 
				WHERE DocumentPk=" + Current.DocumentPk + @" 
				ORDER BY Category ASC; ";
			RS = DB_Intl2000.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				if (RS["Category"] + "" == "0") {
					Current.PreCarriage.F1 = RS["Value0"] + "";
					Current.PreCarriage.F2 = RS["Value1"] + "";
					Current.BLADVI.LoadgDate = RS["Value2"] + "";
					Current.BLADVI.FinalDate = RS["Value3"] + "";
					Current.LoadPort.F1 = RS["Value4"] + "";
					Current.LoadPort.F2 = RS["Value5"] + "";
					Current.FinalPort.F1 = RS["Value6"] + "";
					Current.FinalPort.F2 = RS["Value7"] + "";
					Current.ACC.F1 = RS["Value8"] + "";
					Current.ACC.F2 = RS["Value9"] + "";
					Current.TotalWeight = decimal.Parse(RS["ValueDecimal0"] + "");
				} else if (RS["Category"] + "" == "1") {
					Current.CUSMANEquipment.CONTNRNo = RS["Value0"] + "";
					Current.CUSMANEquipment.CONTNRSeal1 = RS["Value1"] + "";
					Current.CUSMANEquipment.CONTNRTypes = RS["Value2"] + "";
					Current.Warehouse.F1 = RS["Value3"] + "";
					Current.Warehouse.F2 = RS["Value4"] + "";
					Current.CUSMAN_Assignment.F1 = RS["Value5"] + "";
					Current.CUSMAN_Assignment.F2 = RS["Value6"] + "";
					Current.Partner.F1 = RS["Value7"] + "";
					Current.Partner.F2 = RS["Value8"] + "";
				}
			}
			RS.Close();
			DB_Intl2000.DBCon.Close();

			return Current;
		}
		private List<HouseBL> Make_YuhanHouse(string TBBHPk, string stepCL) {
			List<HouseBL> ReturnValue = new List<HouseBL>();
			string Table, TempQ;
			if (stepCL == "" || stepCL == "3") {
				Table = " TransportBBHistory ";
				TempQ = "";
			} else {
				Table = " [dbo].[TRNASPORT_BODY] ";
				TempQ = " ";
			}
			DB_Intl2000 = new DBConn();
			DB_Intl2000.SqlCmd.CommandText = @"		
SELECT 
	Storage.BoxCount 
	, R.RequestFormPk, R.ConsigneeCode
	, R.TotalPackedCount, R.PackingUnit, R.TotalGrossWeight, R.TotalVolume
	, RFI.Description AS CICKD 
	, CD.BLNo, CD.Shipper, CD.ShipperAddress, CD.Consignee, CD.ConsigneeAddress
FROM  
	" + Table + @" AS Storage left join 
	RequestForm AS R on Storage.RequestFormPk=R.RequestFormPk 
	Left join Company AS CC on R.ConsigneePk=CC.CompanyPk 
	Left join RegionCode AS Departure on R.DepartureRegionCode=Departure.RegionCode 
	Left join RegionCode AS Arrival on R.ArrivalRegionCode=Arrival.RegionCode 
	left join RequestFormItems AS RFI ON R.RequestFormPk=RFI.RequestFormPk 
	left join (SELECT [TABLE_PK], [ACCOUNT_ID], [DESCRIPTION], [REGISTERD] FROM [dbo].[HISTORY] WHERE [TABLE_NAME] = 'RequestForm' AND [CODE]='0') AS RFAI ON R.RequestFormPk=RFAI.[TABLE_PK] 
	left join CommerdialConnectionWithRequest AS CCWR On R.RequestFormPk=CCWR.RequestFormPk 
	left join CommercialDocument AS CD On CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk 
WHERE Storage.TransportBetweenBranchPk=" + TBBHPk + TempQ + @" and isnull(R.DocumentStepCL,'')<>4
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
			HouseBL Current = new HouseBL();
			while (RS.Read()) {
				if (TempBLNo == RS["BLNo"] + "") {
					if (TempRequestFormPk != RS["RequestFormPk"] + "") {
						TempRequestFormPk = RS["RequestFormPk"] + "";
						if (RS["BoxCount"] + "" != "") {
							TotalPackedCount += Int32.Parse(RS["BoxCount"] + "");
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
						Current = new HouseBL();
						count++;
						TotalPackedCount = 0;
						TotalGrossWeight = 0;
						TotalVolume = 0;
					}

					TempBLNo = RS["BLNo"] + "";
					TempRequestFormPk = RS["RequestFormPk"] + "";
					if (RS["BoxCount"] + "" != "") {
						TotalPackedCount += Int32.Parse(RS["BoxCount"] + "");
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
						case "16": packingunit = "RO"; break;
						case "17": packingunit = "PA"; break;
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
					Current.ShipperAddress = new Address();

					string Address = RS["ShipperAddress"] + "";
					Current.ShipperAddress.ADDR_ENG1 = "";
					Current.ShipperAddress.ADDR_ENG2 = "";
					Current.ShipperAddress.ADDR_ENG3 = "";

					if (Address.Length > 40) {
						Current.ShipperAddress.ADDR_ENG1 = Address.Substring(0, 40).Trim();
						if (Address.Length > 80) {
							Current.ShipperAddress.ADDR_ENG2 = Address.Substring(40, 40).Trim();
							if (Address.Length > 120) {
								Current.ShipperAddress.ADDR_ENG3 = Address.Substring(80, 40).Trim();
							} else {
								Current.ShipperAddress.ADDR_ENG3 = Address.Substring(80).Trim();
							}
						} else {
							Current.ShipperAddress.ADDR_ENG2 = Address.Substring(40).Trim();
						}
					} else {
						Current.ShipperAddress.ADDR_ENG1 = Address.Trim();
					}

					Current.ConsigneeCode = RS["ConsigneeCode"] + "";

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
					Current.ConsigneeSaupjaNo = tempSaupja;
					Current.ConsigneeAddress = new Address();

					Address = RS["ConsigneeAddress"] + "";
					Current.ConsigneeAddress.ADDR_ENG1 = "";
					Current.ConsigneeAddress.ADDR_ENG2 = "";
					Current.ConsigneeAddress.ADDR_ENG3 = "";

					if (Address.Length > 40) {
						Current.ConsigneeAddress.ADDR_ENG1 = Address.Substring(0, 40).Trim();
						if (Address.Length > 80) {
							Current.ConsigneeAddress.ADDR_ENG2 = Address.Substring(40, 40).Trim();
							if (Address.Length > 120) {
								Current.ConsigneeAddress.ADDR_ENG3 = Address.Substring(80, 40).Trim();
							} else {
								Current.ConsigneeAddress.ADDR_ENG3 = Address.Substring(80).Trim();
							}
						} else {
							Current.ConsigneeAddress.ADDR_ENG2 = Address.Substring(40).Trim();
						}
					} else {
						Current.ConsigneeAddress.ADDR_ENG1 = Address.Trim();
					}


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

			for (var i = 0; i < ReturnValue.Count; i++) {
				if (ReturnValue[i].ConsigneeSaupjaNo == "") {
					DB_Intl2000.SqlCmd.CommandText = @"
						DECLARE @Name varchar(max);
						DECLARE @Count int; 
						SET @Name='" + ReturnValue[i].ConsigneeName + @"';
						SELECT @Count=count(*)
						  FROM [INTL2010].[dbo].[CompanyInDocument]
						  where GubunCL<>'10' and Name=@Name; 

						  if (@Count=1) 
							BEGIN; 
								SELECT [CompanyNo]
								FROM [INTL2010].[dbo].[CompanyInDocument] 
								where GubunCL<>'10' and Name=@Name; 
							END
						";
					string tempString = DB_Intl2000.SqlCmd.ExecuteScalar() + "";
					if (tempString != "") {
						HouseBL tempHouseBL = ReturnValue[i];
						tempHouseBL.ConsigneeSaupjaNo = tempString.Replace("-", "");
						ReturnValue[i] = tempHouseBL;
					}
				}
			}
			DB_Intl2000.DBCon.Close();


			return ReturnValue;
		}
		public string Clear_YuhanMaster_Intl2000(string TBBHPk) {
			DB_Intl2000 = new DBConn();
			DB_Intl2000.DBCon.Open();
			DB_Intl2000.SqlCmd.CommandText = @"
DELETE FROM[INTL2010].[dbo].[Document]
		WHERE[DocumentPk] in (
SELECT[DocumentPk]
  FROM[INTL2010].[dbo].[Document]
  WHERE([Type]= 'MasterBL' or[Type]= 'HouseBL') and TypePk = " + TBBHPk + @"
  );

DELETE FROM
	[INTL2010].[dbo].[DocumentBody]
		WHERE DocumentBodyPk  in (
    SELECT DB.DocumentBodyPk
    FROM[INTL2010].[dbo].[DocumentBody] AS DB
        left join [INTL2010].[dbo].[Document] AS D ON DB.DocumentPk= D.DocumentPk
    WHERE D.DocumentPk is null 
); ";
			DB_Intl2000.SqlCmd.ExecuteNonQuery();
			DB_Intl2000.DBCon.Close();
			return "1";
		}
		public string SetSave_YuhanMaster_Intl2000(sYuhan_Master Current) {
			Clear_YuhanMaster_Intl2000(Current.TBBHPk);
			DB_Intl2000 = new DBConn();
			DB_Intl2000.DBCon.Open();

			DB_Intl2000.SqlCmd.CommandText = Utility.ToDB("[dbo].[Document]", new List<string[]>() {
				new string[] {"DocumentPk",  "", "int" },
				new string[] {"Type",  "MasterBL" },
				new string[] {"TypePk",  Current.TBBHPk, "int"},
				new string[] {"Status",  "0" , "int"},
				new string[] {"Value0",  Current.SHPREQ.SHPREQNo },
				new string[] {"Value1",  Current.SHPREQ.SHPREQDate },
				new string[] {"Value2",  Current.SHPREQ.BLADVIMaster },
				new string[] {"Value3",  Current.SHPREQ.ShipName },
				new string[] {"Value4",  Current.SHPREQ.VoyageNo },
				new string[] {"Value5",  Current.SHPREQ.SHPCode },
				new string[] {"Value6",  Current.SHPREQ.SHPName },
				new string[] {"Value7",  Current.SHPREQ.CNECode},
				new string[] {"Value8",  Current.SHPREQ.CNEName},
				new string[] {"Value9",  Current.SHPREQ.CNEAcct },
				new string[] {"Value10",  Current.ShipperAddress.ADDR_ENG1 },
				new string[] { "Value11",  Current.ShipperAddress.ADDR_ENG2},
				new string[] { "Value12",  Current.ShipperAddress.ADDR_ENG3 },
				new string[] { "Value13",  Current.ConsigneeAddress.ADDR_ENG1 },
				new string[] { "Value14",  Current.ConsigneeAddress.ADDR_ENG2 },
				new string[] { "Value15",  Current.ConsigneeAddress.ADDR_ENG3 },
				new string[] { "Value16",  Current.CUSREP.MRN },
				new string[] { "Value17",  Current.CUSREP.MSN },
				new string[] { "ValueInt0",  Current.TotalPackedCount.ToString(), "int" },
				new string[] { "ValueDecimal0",  Current.TotalVolume.ToString(), "decimal(18, 4)" },
			});

			string DocumentPk = DB_Intl2000.SqlCmd.ExecuteScalar() + "";



			StringBuilder query = new StringBuilder();

			query.Append(Utility.ToDB("[dbo].[DocumentBody]", new List<string[]>() {
				new string[] {"DocumentBodyPk", "", "int" },
				new string[] {"DocumentPk", DocumentPk, "int" },
				new string[] {"Category", "0", "int" },
				new string[] {"Value0", Current.PreCarriage.F1},
				new string[] {"Value1", Current.PreCarriage.F2},
				new string[] {"Value2", Current.BLADVI.LoadgDate},
				new string[] {"Value3", Current.BLADVI.FinalDate },
				new string[] {"Value4", Current.LoadPort.F1},
				new string[] {"Value5", Current.LoadPort.F2},
				new string[] {"Value6", Current.FinalPort.F1},
				new string[] {"Value7", Current.FinalPort.F2},
				new string[] {"Value8", Current.ACC.F1 },
				new string[] {"Value9", Current.ACC.F2 },
				new string[] { "ValueInt0", "", "int" },
				new string[] { "ValueDecimal0", Current.TotalWeight.ToString(), "decimal" }
			}));
			query.Append(
				Utility.ToDB("[dbo].[DocumentBody]", new List<string[]>() {
				new string[] {"DocumentBodyPk", "", "int" },
				new string[] {"DocumentPk", DocumentPk, "int" },
				new string[] {"Category", "1", "int" },
				new string[] {"Value0", Current.CUSMANEquipment.CONTNRNo},
				new string[] {"Value1", Current.CUSMANEquipment.CONTNRSeal1},
				new string[] {"Value2", Current.CUSMANEquipment.CONTNRTypes},
				new string[] {"Value3", Current.Warehouse.F1 },
				new string[] {"Value4", Current.Warehouse.F2 },
				new string[] {"Value5", Current.CUSMAN_Assignment.F1},
				new string[] {"Value6", Current.CUSMAN_Assignment.F2},
				new string[] {"Value7", Current.Partner.F1},
				new string[] {"Value8", Current.Partner.F2 },
				new string[] {"Value9", "" },
				new string[] { "ValueInt0", "", "int" },
				new string[] { "ValueDecimal0", "", "decimal" }
			}));
			for (var i = 0; i < Current.HouseBL.Count; i++) {
				HouseBL house = Current.HouseBL[i];
				query.Append(Utility.ToDB("[dbo].[Document]", new List<string[]>() {
					new string[] {"DocumentPk",  "", "int" },
					new string[] {"Type",  "HouseBL" },
					new string[] {"TypePk", Current.TBBHPk, "int"},
					new string[] {"Status",  "0" , "int"},
					new string[] {"Value0",  house.BLNo },
					new string[] {"Value1", house.PackedCount },
					new string[] {"Value2",  house.PackingUnit },
					new string[] {"Value3",  house.Weight },
					new string[] {"Value4", house.Volume },
					new string[] {"Value5",  house.ShipperCode },
					new string[] {"Value6",  house.ShipperName},
					new string[] {"Value7",  house.ConsigneeCode},
					new string[] {"Value8",  house.ConsigneeName},
					new string[] {"Value9",  house.ConsigneeSaupjaNo},
					new string[] {"Value10",  house.Description},
					new string[] { "Value11", house.ShipperAddress.ADDR_ENG1},
					new string[] { "Value12", house.ShipperAddress.ADDR_ENG2 },
					new string[] { "Value13", house.ShipperAddress.ADDR_ENG3 },
					new string[] { "Value14", house.ConsigneeAddress.ADDR_ENG1 },
					new string[] { "Value15", house.ConsigneeAddress.ADDR_ENG2 },
					new string[] { "Value16", house.ConsigneeAddress.ADDR_ENG3 },
					new string[] { "Value17",  "" },
					new string[] { "ValueInt0", house.HSN, "int" },
					new string[] { "ValueDecimal0", "", "decimal(18, 4)" },
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
			DB_Yuhan = new DBConn("Yuhan");
			DB_Yuhan.SqlCmd.CommandText = string.Format(@"
USE Cyberinf; 
SELECT Firm 
From uCustomer  
WHERE Alias='{0}';", Code);
			DB_Yuhan.DBCon.Open();
			string ReturnValue = DB_Yuhan.SqlCmd.ExecuteScalar() + "";
			DB_Yuhan.DBCon.Close();
			return ReturnValue;
		}
		public String Search_Port(string Code) {
			DB_Yuhan = new DBConn("Yuhan");
			DB_Yuhan.SqlCmd.CommandText = string.Format(@"
USE Cyberinf; 
SELECT PortName FROM xPort WHERE Port='{0}' AND PortDivision='S';", Code);
			DB_Yuhan.DBCon.Open();
			string ReturnValue = DB_Yuhan.SqlCmd.ExecuteScalar() + "";
			DB_Yuhan.DBCon.Close();
			return ReturnValue;
		}
		public String Search_Company(string Code) {
			DB_Yuhan = new DBConn("Yuhan");
			DB_Yuhan.SqlCmd.CommandText = @"
SELECT 
	C.[Alias]
      ,C.[AccountNumber]
      ,C.[Firm]
      ,C.[Division1st]
      ,CAddr.[ADDR_ENG1]
      ,CAddr.[ADDR_ENG2]
      ,CAddr.[ADDR_ENG3]
      ,CAddr.[ADDR_ENG4]
      ,CAddr.[ADDR_ENG5]
  FROM [Cyberinf].[dbo].[uCustomer] AS C 
	left join [Cyberinf].[dbo].[uCustomerAddr] AS CAddr ON C.[Alias]=CAddr.[Alias]
	WHERE 	C.[Alias] ='" + Code + "';";
			DB_Yuhan.DBCon.Open();
			SqlDataReader RS = DB_Yuhan.SqlCmd.ExecuteReader();

			string ReturnValue = "";
			if (RS.Read()) {
				ReturnValue = RS["Firm"] + ",!" + RS["ADDR_ENG1"] +
					(RS["ADDR_ENG2"].ToString() == "" ? "" : "<br />" + RS["ADDR_ENG2"].ToString()) +
					(RS["ADDR_ENG3"].ToString() == "" ? "" : "<br />" + RS["ADDR_ENG3"].ToString()) +
					(RS["ADDR_ENG4"].ToString() == "" ? "" : "<br />" + RS["ADDR_ENG4"].ToString()) +
					(RS["ADDR_ENG5"].ToString() == "" ? "" : "<br />" + RS["ADDR_ENG5"].ToString()) +
					",!" + RS["AccountNumber"];
			}
			RS.Dispose();
			DB_Yuhan.DBCon.Close();
			return ReturnValue;
		}
		private List<string[]> LoadCompanyCodeInYuhan(string SorC, List<string> SumName) {
			List<string[]> ReturnValue = new List<string[]>();

			DB_Intl2000 = new DBConn();
			DB_Yuhan = new DBConn("Yuhan");
			string Column_intl2000 = "";
			string Column_Yuhan = "";
			if (SorC == "S") {
				Column_intl2000 = "Shipper";
				Column_Yuhan = "SHPCode";
			} else {
				Column_intl2000 = "Consignee";
				Column_Yuhan = "CNECode";
			}

			DB_Intl2000.DBCon.Open();
			DB_Yuhan.DBCon.Open();
			SqlDataReader RS;
			for (var i = 0; i < SumName.Count; i++) {
				if (SumName[i] == "") {
					continue;
				}

				DB_Intl2000.SqlCmd.CommandText = @"
SELECT TOP 30 [BLNo]
FROM [INTL2010].[dbo].[CommercialDocument] 
WHERE [BLNo] is not null and " + Column_intl2000 + "='" + SumName[i] + @"' 
ORDER BY Registerd DESC;";
				RS = DB_Intl2000.SqlCmd.ExecuteReader();
				StringBuilder QueryWhere_BLNo = new StringBuilder();
				while (RS.Read()) {
					QueryWhere_BLNo.Append(",'" + RS[0] + "'");
				}
				RS.Dispose();

				if (QueryWhere_BLNo.ToString() == "") {
					continue;
				}

				DB_Yuhan.SqlCmd.CommandText = @"
SELECT " + Column_Yuhan + @" 
FROM [impsea].[dbo].[BLADVI] 
WHERE BLADVINo in (" + QueryWhere_BLNo.ToString().Substring(1) + @") and isnull(" + Column_Yuhan + @", '')<>''
 GROUP BY " + Column_Yuhan + @" 
 ORDER BY Count(*) DESC; ";
				RS = DB_Yuhan.SqlCmd.ExecuteReader();
				var RSCount = 0;
				string result = "";
				while (RS.Read()) {
					RSCount++;
					result = RS[0] + "";
				}
				RS.Dispose();
				if (RSCount == 1) {
					ReturnValue.Add(new string[] { SumName[i], result });
				}
			}
			DB_Intl2000.DBCon.Close();
			DB_Yuhan.DBCon.Close();
			return ReturnValue;
		}
		public String Load_VesselList() {
			DB_Yuhan = new DBConn("Yuhan");
			DB_Yuhan.SqlCmd.CommandText = @"
SELECT [ShipName]
  FROM [impsea].[dbo].[SHPREQ] 
  WHERE ShipName<> '' and SHPREQDate >'2014' 
 GROUP BY [ShipName]; ";
			StringBuilder ReturnValue = new StringBuilder();
			DB_Yuhan.DBCon.Open();
			SqlDataReader RS = DB_Yuhan.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				ReturnValue.Append("<tr><td onclick=\"SetInModal('" + RS[0] + "', '" + RS[0] + "');\">" + RS[0] + "</td></tr>");
			}
			RS.Dispose();
			DB_Yuhan.DBCon.Close();
			return @"
<table class=""table table-hover"">
	<thead>
		<tr>	
			<th>Title</th>
		</tr>
	</thead>
	<tbody>" + ReturnValue + @"</tbody>
</table>";
		}
		public String Load_PreCarriageList() {
			DB_Yuhan = new DBConn("Yuhan");
			DB_Yuhan.SqlCmd.CommandText = @"
USE Cyberinf; 
SELECT Alias,Firm 
From uCustomer 
WHERE Alias in ( 
	SELECT [PreCarriage] 
	FROM [impsea].[dbo].[SHPREQ] 
	WHERE PreCarriage <> '' and SHPREQDate >'2014' 
	GROUP BY [PreCarriage]
)
ORDER BY Firm ,Alias;
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
		public String Load_PortList() {
			DB_Yuhan = new DBConn("Yuhan");

			DB_Yuhan.SqlCmd.CommandText = @"
USE Cyberinf; 
SELECT 
	Port         AS F1
	,PortName         AS F2
	 FROM xPort WHERE Port in (
		(
		SELECT LoadgPort
		  FROM [impsea].[dbo].[SHPREQ] 
		  WHERE LoadgPort<> '' and SHPREQDate >'2014' 
		 GROUP BY LoadgPort 
		 )UNION(
		SELECT FinalPort
		  FROM [impsea].[dbo].[SHPREQ] 
		  WHERE FinalPort<> '' and SHPREQDate >'2014' 
		 GROUP BY FinalPort
		 )

) AND PortDivision='S' 
ORDER BY F2
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
		public String Load_Company(string Code) {
			DB_Yuhan = new DBConn("Yuhan");

			DB_Yuhan.SqlCmd.CommandText = @"
SELECT 
	C.[Alias]
      ,C.[AccountNumber]
      ,C.[Firm]
      ,C.[Division1st]
      ,CAddr.[ADDR_ENG1]
      ,CAddr.[ADDR_ENG2]
      ,CAddr.[ADDR_ENG3]
      ,CAddr.[ADDR_ENG4]
      ,CAddr.[ADDR_ENG5]
  FROM [Cyberinf].[dbo].[uCustomer] AS C 
	left join [Cyberinf].[dbo].[uCustomerAddr] AS CAddr ON C.[Alias]=CAddr.[Alias]
	WHERE 	C.[Alias] in ((
SELECT [SHPCode]
  FROM [impsea].[dbo].[SHPREQ]
 WHERE [SHPCode]<>'' and SHPREQDate >'2014' 
 GROUP BY [SHPCode] 
 )
 union 
 (
SELECT [CNECode]
  FROM [impsea].[dbo].[SHPREQ]
 WHERE [CNECode]<>'' and SHPREQDate >'2014' 
 GROUP BY [CNECode] 
 )
 union 
 (
SELECT [PTNCode]
  FROM [impsea].[dbo].[SHPREQ]
 WHERE [PTNCode]<>'' and SHPREQDate >'2014' 
 GROUP BY [PTNCode] 
 )
 union 
 (
SELECT [ACCCode]
  FROM [impsea].[dbo].[SHPREQ]
 WHERE [ACCCode]<>'' and SHPREQDate >'2014' 
 GROUP BY [ACCCode] 
)
); ";
			StringBuilder ReturnValue = new StringBuilder();
			DB_Yuhan.DBCon.Open();
			SqlDataReader RS = DB_Yuhan.SqlCmd.ExecuteReader();

			while (RS.Read()) {
				string onclick = " onclick=\"SetInModal('" + RS[0] + "', '" + RS["Firm"] + ",!" + RS["ADDR_ENG1"] +
					(RS["ADDR_ENG2"].ToString() == "" ? "" : "<br />" + RS["ADDR_ENG2"].ToString()) +
					(RS["ADDR_ENG3"].ToString() == "" ? "" : "<br />" + RS["ADDR_ENG3"].ToString()) +
					(RS["ADDR_ENG4"].ToString() == "" ? "" : "<br />" + RS["ADDR_ENG4"].ToString()) +
					(RS["ADDR_ENG5"].ToString() == "" ? "" : "<br />" + RS["ADDR_ENG5"].ToString()) +
					",!" + RS["AccountNumber"] + "');\"";
				ReturnValue.Append("<tr><td " + onclick + ">" + RS["Alias"] + "</td><td " + onclick + ">" + RS["Firm"] + "</td></tr>");
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
			DB_Yuhan = new DBConn("Yuhan");

			DB_Yuhan.SqlCmd.CommandText = @"
use Cyberinf; 

SELECT Warehouse.Location,Warehouse.LocationName 
From xLocation as Warehouse 
	inner join (
		SELECT C.[WareHouse] , Count(*) AS C
		  FROM [impsea].[dbo].[BLADVI] AS B
			left join [impsea].[dbo].[CUSMAN] AS C ON B.[BLADVINo]=C.[BLADVINo]
 		  WHERE C.[WareHouse]<>'' and B.FirstDate >'2014' 
		 GROUP BY C.[WareHouse]
	) AS C ON Warehouse.Location=C.[WareHouse] 
ORDER BY C.C DESC, Warehouse.LocationName ,Warehouse.Location; ";
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
				SELECT @Consignee =[Value8] 
				  FROM [INTL2010].[dbo].[Document] 
				  WHERE [Type]='HouseBL' and Value0='" + BLNo + @"'

				SELECT CASE D.TypePk WHEN 0 THEN D.Value1 Else MasterBL.Value1 END AS Value1, D.Value0 , D.[Value10] 
				  FROM [INTL2010].[dbo].[Document] AS D 
					left join [INTL2010].[dbo].[Document]  AS MasterBL ON D.ParentsId=MasterBL.DocumentPk 
				  WHERE D.[Type]='HouseBL' and D.Value8=@Consignee and D.Value0 is not null and  D.Value10 is not null and D.Value0<>'" + BLNo + "' ORDER BY MasterBL.Value1 ASC, D.Value1 DESC ;";
			StringBuilder ReturnValue = new StringBuilder();
			DB_Intl2000.DBCon.Open();
			SqlDataReader RS = DB_Intl2000.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				ReturnValue.Append("<tr><td style='cursor:pointer;' onclick=\"SetInModal('" + BLNo + "', '" + RS["Value10"].ToString().ToUpper() + "');\">" + RS["Value1"] + "</td><td style='cursor:pointer;' onclick=\"SetInModal('" + BLNo + "', '" + RS["Value10"].ToString().ToUpper() + "');\">" + RS["Value0"] + "</td><td style='cursor:pointer;' onclick=\"SetInModal('" + BLNo + "', '" + RS["Value10"].ToString().ToUpper() + "');\">" + RS["Value10"] + "</td></tr>");
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

		public String Load_AssignmentList() {
			DB_Yuhan = new DBConn("Yuhan");

			DB_Yuhan.SqlCmd.CommandText = @"
SELECT [Assignment]
      ,[AssignmentText]
  FROM [impsea].[dbo].[CUSMAN] 
  WHERE Assignment<>'' 
 GROUP BY [Assignment],[AssignmentText]
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
		public String Load_SHPREQNo() {
			DB_Yuhan = new DBConn("Yuhan");
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
		public string SetSave_YuhanMaster_Yuhan(sYuhan_Master Current) {
			List<string> Query = new List<string>();
			if (Current.SHPREQ.SHPREQNo == "") {
				Current.SHPREQ.SHPREQNo = Load_SHPREQNo();
			}
			Query.Add(GetQuery_ClearYuhan(Current));
			Query.Add(GetQuery_SetMasterBL(Current));
			for (var i = 0; i < Current.HouseBL.Count; i++) {
				Query.Add(GetQuery_SetHouseBL(Current, i));
			}
			DB_Yuhan = new DBConn("Yuhan");
			DB_Yuhan.DBCon.Open();
			foreach (string eachQuery in Query) {
				//ReturnValue.Append("AAAAQQQQ"+eachQuery);
				DB_Yuhan.SqlCmd.CommandText = eachQuery;
				try {
					DB_Yuhan.SqlCmd.ExecuteNonQuery();
				} catch (Exception) {
				}
			}
			DB_Yuhan.DBCon.Close();
			return "1";
		}
		private String GetQuery_ClearYuhan(sYuhan_Master Current) {
			StringBuilder Query = new StringBuilder();
			Query.Append(@"
				DECLARE @BLADVIMaster varchar(20);
				SET @BLADVIMaster='" + Current.SHPREQ.BLADVIMaster + @"';
				use [impsea]; 
				DELETE FROM [BLADVI] WHERE BLADVINo=@BLADVIMaster;
				DELETE FROM [SHPREQ] WHERE BLADVIMaster=@BLADVIMaster;
				DELETE FROM CUSMAN WHERE [BLADVIMaster]=@BLADVIMaster;
				DELETE FROM [CUSMANEquipment] WHERE [BLADVIMaster]=@BLADVIMaster;
				DELETE FROM [BLADVI] WHERE [BLADVIMaster]=@BLADVIMaster;
				");
			StringBuilder HouseBLSum = new StringBuilder();
			for (var i = 0; i < Current.HouseBL.Count; i++) {
				HouseBLSum.Append(", '" + Current.HouseBL[i].BLNo + "'");
			}
			if (HouseBLSum.ToString() != "") {
				Query.Append(@"
				DELETE FROM [impsea].[dbo].[CUSMAN] WHERE [BLADVINo] in (" + HouseBLSum.ToString().Substring(1) + @");
				DELETE FROM [impsea].[dbo].[CUSMANEquipment] WHERE [BLADVINo] in (" + HouseBLSum.ToString().Substring(1) + @");
				DELETE FROM [impsea].[dbo].[BLADVI] WHERE [BLADVINo] in (" + HouseBLSum.ToString().Substring(1) + @");
				DELETE FROM [impsea].[dbo].[BLADVIAddr] WHERE [BLADVINo] in (" + HouseBLSum.ToString().Substring(1) + @");
				DELETE FROM [impsea].[dbo].[BLADVISettle] WHERE [BLADVINo] in (" + HouseBLSum.ToString().Substring(1) + @");
				DELETE FROM [impsea].[dbo].[BLADVIQuantity] WHERE [BLADVINo] in (" + HouseBLSum.ToString().Substring(1) + @");");
			}
			return Query.ToString();
		}
		private String GetQuery_SetMasterBL(sYuhan_Master Current) {
			string YearMonth = DateTime.Now.ToShortDateString().Replace("-", "").Substring(0, 6);
			string Query = @"
DECLARE @TXFILE int; 
DECLARE @YearMonth varchar(6); 
DECLARE @SHPREQNo varchar(16); 
DECLARE @SHPREQDate varchar(10); 
DECLARE @VoyageNo varchar(35);
DECLARE @ShipName varchar(35);
DECLARE @PreCarriage varchar(8);
DECLARE @PTNCode varchar(8);
DECLARE @ACCCode varchar(8);
DECLARE @LoadgPort varchar(5);
DECLARE @FinalPort varchar(5);
DECLARE @SHPCode varchar(8);
DECLARE @SHPName varchar(50);
DECLARE @CNEAcct varchar(17);
DECLARE @CNECode varchar(8);
DECLARE @CNEName varchar(50);
DECLARE @FromDate varchar(10);
DECLARE @ToDate varchar(10);
DECLARE @BLADVIMaster varchar(20);
DECLARE @LoadgPortName varchar(25);
DECLARE @FinalPortName varchar(25);
DECLARE @PackedCount real;
DECLARE @PackingUnit varchar(3);
DECLARE @Weight real;
DECLARE @Volume real;
DECLARE @MRN varchar(11); 
DECLARE @MSN varchar(4);

DECLARE @ShipperAddressEng1 varchar(50); 
DECLARE @ShipperAddressEng2 varchar(50); 
DECLARE @ShipperAddressEng3 varchar(50); 
DECLARE @ShipperAddressEng4 varchar(50); 
DECLARE @ShipperAddressEng5 varchar(50); 
DECLARE @ConsigneeAddressEng1 varchar(50); 
DECLARE @ConsigneeAddressEng2 varchar(50); 
DECLARE @ConsigneeAddressEng3 varchar(50); 
DECLARE @ConsigneeAddressEng4 varchar(50); 
DECLARE @ConsigneeAddressEng5 varchar(50); 

SET @YearMonth ='" + YearMonth + @"';
SET @SHPREQNo='" + Current.SHPREQ.SHPREQNo + @"';
SET @SHPREQDate='" + Current.SHPREQ.SHPREQDate + @"';
SET @VoyageNo='" + Current.SHPREQ.VoyageNo + @"';
SET @PreCarriage='" + Current.PreCarriage.F1 + @"';
SET @ShipName='" + Current.SHPREQ.ShipName + @"';
SET @PTNCode='" + Current.Partner.F1 + @"';
SET @ACCCode='" + Current.ACC.F1 + @"';
SET @LoadgPort='" + Current.LoadPort.F1 + @"';
SET @FinalPort='" + Current.FinalPort.F1 + @"';
SET @SHPCode='" + Current.SHPREQ.SHPCode + @"';
SET @SHPName='" + Current.SHPREQ.SHPName + @"';
SET @CNEAcct='" + Current.SHPREQ.CNEAcct + @"';
SET @CNECode='" + Current.SHPREQ.CNECode + @"';
SET @CNEName='" + Current.SHPREQ.CNEName + @"';
SET @FromDate='" + Current.BLADVI.LoadgDate + @"';
SET @ToDate='" + Current.BLADVI.FinalDate + @"';
SET @BLADVIMaster='" + Current.SHPREQ.BLADVIMaster + @"';
SET @LoadgPortName='" + Current.LoadPort.F2 + @"';
SET @FinalPortName='" + Current.FinalPort.F2 + @"';
SET @PackedCount=" + Current.TotalPackedCount + @";
SET @PackingUnit='" + "CT" + @"';
SET @Weight=" + Current.TotalWeight + @";
SET @Volume=" + Current.TotalVolume + @";
SET @MRN='" + Current.CUSREP.MRN + @"';
SET @MSN='" + Current.CUSREP.MSN + @"';

use Cyberinf; 
SELECT @TXFILE=DocNo1st FROM uDocNumber WHERE DocStatus='TXFILE' AND DocKey1st='SHPREQ' AND DocKey2nd=@YearMonth;
if (@TXFILE is null)
BEGIN 
	SET @TXFILE =1;
	INSERT INTO [dbo].[uDocNumber] ([DocStatus],[DocKey1st],[DocKey2nd],[DocNo1st],[DocNo2nd]) VALUES ('TXFILE', 'SHPREQ', @YearMonth, 1, 0);
END
else 
BEGIN 
	SET @TXFILE=@TXFILE +1;
	UPDATE uDocNumber SET  DocNo1st=@TXFILE+1,DocNo2nd=0 WHERE DocStatus='TXFILE' AND DocKey1st='SHPREQ' AND DocKey2nd=@YearMonth;
END

SELECT 
	@ShipperAddressEng1=[ADDR_ENG1]
	,@ShipperAddressEng2=[ADDR_ENG2]
	,@ShipperAddressEng3=[ADDR_ENG3]
	,@ShipperAddressEng4=[ADDR_ENG4]
	,@ShipperAddressEng5=[ADDR_ENG5] 
  FROM [Cyberinf].[dbo].[uCustomerAddr] 
     WHERE [Alias]=@SHPCode;
SELECT 
	@ConsigneeAddressEng1=[ADDR_ENG1]
	,@ConsigneeAddressEng2=[ADDR_ENG2]
	,@ConsigneeAddressEng3=[ADDR_ENG3]
	,@ConsigneeAddressEng4=[ADDR_ENG4]
	,@ConsigneeAddressEng5=[ADDR_ENG5]
  FROM [Cyberinf].[dbo].[uCustomerAddr] 
     WHERE [Alias]=@CNECode;


use [impsea]; 
DECLARE @AleadyCount int; 
SELECT @AleadyCount=COUNT(*)
FROM SHPREQ WHERE SHPREQNo=@SHPREQNo;

if (@AleadyCount>0)
BEGIN
	SELECT 0; 
END
else 
BEGIN
	SELECT @AleadyCount=COUNT(*) 
	FROM BLADVI WHERE BLADVINo=@BLADVIMaster;

	if (@AleadyCount>0)
	BEGIN
		SELECT 0; 
	END
	else 
	BEGIN
		INSERT INTO SHPREQ 	(
			SHPREQNo,SHPREQDate,BookingNo,VoyageNo,ShipName,
			LoadingType,PreCarriage,DeliveryPlace,PTNCode,ACCCode,
			LoadgPort,DischargePort,FinalPort,UnitWTVALPPD,UnitWTVALCOL,
			SHPCode,SHPName,CNEAcct,CNECode,CNEName,
			NFYCode,NFYName,TextName,FirstDate,BLADVIMaster,
			NFYAcct,LoadgPortName,FinalPortName,DischargePortName,DeliveryPlaceName
		) VALUES (
			@SHPREQNo,@SHPREQDate,'',@VoyageNo,@ShipName,
			'',@PreCarriage,'',@PTNCode,@ACCCode,
			@LoadgPort,'',@FinalPort,'','',
			@SHPCode,@SHPName,@CNEAcct,@CNECode,@CNEName,
			@CNECode,@CNEName,@TXFILE,@FromDate,@BLADVIMaster,
			@CNEAcct,@LoadgPortName,@FinalPortName,'','');

		INSERT INTO BLADVI (
			BLADVIMaster,SHPREQNo,MHType,IEType,NLType
			,DCType,PreCarriage,ReceiptPlace,NFYAcct,VoyageNo
			,ShipName,DischargePort,DeliveryPlace,LoadgPort,BLADVINo
			,FinalDate,ETA,ETD,SHPCode,CNECode
			,NFYCode,SHPName,CNEName,NFYName,CNEAcct
			,IssuePort,IssueDate,IssueSign,UnitWTVALPPD,FinalPort
			,INVOICDate,INVOICNo,INVOICType,FirstDate,TextName
			,Status1st,Status2nd,Status3rd,UnitWTVALCOL,UnitOtherPPD
			,UnitOtherCOL,LoadgDate,BranchCode,INVOICNoUser,ModifyDate
			,ProfitDate,UCRcode
		) VALUES (
			'',@SHPREQNo,'M','I','L'
			,'C',@PreCarriage,'','',@VoyageNo
			,@ShipName,'','',@LoadgPort,@BLADVIMaster
			,@ToDate,@ToDate,@FromDate,@SHPCode,@CNECode
			,@CNECode,@SHPName,@CNEName,@CNEName,@CNEAcct
			,@LoadgPort,@FromDate,'','',@FinalPort
			,'','','',@FromDate,'70'
			,'','','','',''
			,'',@FromDate,'INTL','',@FromDate
			,@ToDate,''
		);

		UPDATE BLADVI SET DOSendFlag						 = '' WHERE DOSendFlag     IS NULL AND BLADVINo=@BLADVIMaster;
		UPDATE BLADVI SET Payment								= '' WHERE Payment        IS NULL AND BLADVINo=@BLADVIMaster;
		UPDATE BLADVI SET DODate								= '' WHERE DODate         IS NULL AND BLADVINo=@BLADVIMaster;
		UPDATE BLADVI SET DOReceiverName		= '' WHERE DOReceiverName IS NULL AND BLADVINo=@BLADVIMaster;
		UPDATE BLADVI SET BLType									= '' WHERE BLType         IS NULL AND BLADVINo=@BLADVIMaster;
		UPDATE BLADVI SET ProfitFlag							= '' WHERE ProfitFlag     IS NULL AND BLADVINo=@BLADVIMaster;
		UPDATE BLADVI SET ACCDate								= '' WHERE ACCDate        IS NULL AND BLADVINo=@BLADVIMaster;
		UPDATE BLADVI SET ModifyTime						= '' WHERE ModifyTime     IS NULL AND BLADVINo=@BLADVIMaster;
		UPDATE BLADVI SET ModifyMan						= '' WHERE ModifyMan      IS NULL AND BLADVINo=@BLADVIMaster;


		DELETE FROM BLADVISettle WHERE BLADVINo=@BLADVIMaster;
		INSERT INTO BLADVISettle (Item,BLADVINo,Area,SalesMan,PunchMan,SealMan,Signer,InvoiceNo,ExDate,ExRate,ExCurrency,PTNCode,ACCCode,COLCode) VALUES ('',@BLADVIMaster,'','','','','','','2011/04/04',0,'USD',@PTNCode,@ACCCode,'');

		DELETE FROM BLADVIQuantity WHERE BLADVINo=@BLADVIMaster AND QtyWork='B';
		INSERT INTO BLADVIQuantity (
			RateClass,LCL,CONT40FTHQ,CONT20FTHQ,CONT40FT,CONT20FT
			,WeightUnit,PiecesUnit,RevenueTon,Volume,Weight,Pieces
			,BLADVINo,QtyWork) VALUES (
			'',@Volume,0,0,0,0
			,'K','CT',0,@Volume,@Weight,@PackedCount
			,@BLADVIMaster,'B');

		DELETE FROM CUSREP WHERE BLADVIMaster=@BLADVIMaster;
		INSERT INTO CUSREP (RFFFileName,ModSerialNo,HouseCount,CustomsDepart,Customs,ReportTime,ReportDate,sFunction,Verification,CallSign,BLADVIMaster,MRN,MSN) VALUES ('','',0,'','','','','9','','',@BLADVIMaster,@MRN,@MSN);


		DELETE FROM SHPREQAddr WHERE SHPREQNo=@SHPREQNo;

		INSERT INTO [dbo].[SHPREQAddr] (
			[SHPREQNo]
			,[SHP_ENG1],[SHP_ENG2],[SHP_ENG3]
			,[SHP_ENG4],[SHP_ENG5]
			,[CNE_ENG1],[CNE_ENG2],[CNE_ENG3]
			,[CNE_ENG4],[CNE_ENG5]
			,[NFY_ENG1],[NFY_ENG2],[NFY_ENG3]
			,[NFY_ENG4],[NFY_ENG5]
		) VALUES (
			@SHPREQNo
			,@ShipperAddressEng1,@ShipperAddressEng2,@ShipperAddressEng3
			,@ShipperAddressEng4,@ShipperAddressEng5
			,@ConsigneeAddressEng1,@ConsigneeAddressEng2,@ConsigneeAddressEng3
			,@ConsigneeAddressEng4,@ConsigneeAddressEng5
			,@ConsigneeAddressEng1,@ConsigneeAddressEng2,@ConsigneeAddressEng3
			,@ConsigneeAddressEng4,@ConsigneeAddressEng5);
		UPDATE BLADVI SET SHPREQNo    ='',BLADVIMaster='' WHERE SHPREQNo=@SHPREQNo AND MHType='H';
		UPDATE CUSMAN SET BLADVIMaster='' WHERE BLADVIMaster=@BLADVIMaster;
		UPDATE CUSMANEquipment SET BLADVIMaster='' WHERE BLADVIMaster=@BLADVIMaster;
		SELECT 1; 
	END 
END
";
			return Query;
		}
		private String GetQuery_SetHouseBL(sYuhan_Master Current, int Count_HouseBL) {
			StringBuilder Query = new StringBuilder();
			Query.Append(@"
DECLARE @BLNO varchar(50);
DECLARE @HSN varchar(3);
DECLARE @Description varchar(200);
DECLARE @Count MONEY;
DECLARE @Weight MONEY;
DECLARE @Volume MONEY;
DECLARE @QuantityUnit varchar(3);
DECLARE @ContainerNo varchar(11);
DECLARE @ContainerSize varchar(4);
DECLARE @SealNo varchar(10);
DECLARE @LoadingDate varchar(10);
DECLARE @ArrivalDate varchar(10);
DECLARE @SCode varchar(8);
DECLARE @CCode varchar(8);
DECLARE @SName varchar(50);
DECLARE @CName varchar(50);
DECLARE @CSaupja varchar(17);
DECLARE @ShipName varchar(50);
DECLARE @ShipCode varchar(8);
DECLARE @WarehouseCode varchar(10);
DECLARE @VoyageNo varchar(35);
DECLARE @LoadgPort varchar(5);
DECLARE @FinalPort varchar(5);
DECLARE @PTNCode varchar(8);
DECLARE @ACCCode varchar(8);
DECLARE @MasterBLNo varchar(20);
DECLARE @ShipperAddressEng1 varchar(50); 
DECLARE @ShipperAddressEng2 varchar(50); 
DECLARE @ShipperAddressEng3 varchar(50); 
DECLARE @ShipperAddressEng4 varchar(50); 
DECLARE @ShipperAddressEng5 varchar(50); 
DECLARE @ConsigneeAddressEng1 varchar(50); 
DECLARE @ConsigneeAddressEng2 varchar(50); 
DECLARE @ConsigneeAddressEng3 varchar(50); 
DECLARE @ConsigneeAddressEng4 varchar(50); 
DECLARE @ConsigneeAddressEng5 varchar(50); 
DECLARE @SHPREQNo varchar(16); 
DECLARE @BLADVIMaster varchar(20);


	SET @BLNo='" + Current.HouseBL[Count_HouseBL].BLNo + "';	" +
   "SET @HSN ='" + Current.HouseBL[Count_HouseBL].HSN + "';	" +
   "SET @Description ='" + Current.HouseBL[Count_HouseBL].Description.Replace("'", "''") + "';	" +
   "SET @Count =" + Current.HouseBL[Count_HouseBL].PackedCount.Replace(",", "") + ";	" +
   "SET @Weight =" + Current.HouseBL[Count_HouseBL].Weight.Replace(",", "") + ";	" +
   "SET @Volume =" + Current.HouseBL[Count_HouseBL].Volume.Replace(",", "") + ";	" +
   "SET @QuantityUnit='" + Current.HouseBL[Count_HouseBL].PackingUnit + "';" +
   "SET @ContainerNo='" + Current.CUSMANEquipment.CONTNRNo + "';" +
   "SET @ContainerSize='" + Current.CUSMANEquipment.CONTNRTypes + "';" +
   "SET @SealNo='" + Current.CUSMANEquipment.CONTNRSeal1 + "';" +
   "SET @LoadingDate='" + Current.BLADVI.LoadgDate + "';" +
   "SET @ArrivalDate='" + Current.BLADVI.FinalDate + "';" +
   "SET @SCode='" + Current.HouseBL[Count_HouseBL].ShipperCode + "';" +
   "SET @CCode='" + Current.HouseBL[Count_HouseBL].ConsigneeCode + "';" +
   "SET @SName='" + Current.HouseBL[Count_HouseBL].ShipperName + "';" +
   "SET @CName='" + Current.HouseBL[Count_HouseBL].ConsigneeName + "';" +
   "SET @CSaupja='" + Current.HouseBL[Count_HouseBL].ConsigneeSaupjaNo + "';" +
   "SET @ShipName='" + Current.SHPREQ.ShipName + "';" +
   "SET @ShipCode='" + Current.PreCarriage.F1 + "';" +
   "SET @WarehouseCode='" + Current.Warehouse.F1 + "';" +
   "SET @VoyageNo='" + Current.SHPREQ.VoyageNo + @"';
	SET @LoadgPort='" + Current.LoadPort.F1 + @"';
	SET @FinalPort='" + Current.FinalPort.F1 + @"';
	SET @PTNCode='" + Current.Partner.F1 + @"';
	SET @ACCCode='" + Current.ACC.F1 + @"';
	SET @SHPREQNo='" + Current.SHPREQ.SHPREQNo + @"';
	SET @BLADVIMaster='" + Current.SHPREQ.BLADVIMaster + @"';

	SET @ShipperAddressEng1='" + Current.HouseBL[Count_HouseBL].ShipperAddress.ADDR_ENG1 + @"';
	SET @ShipperAddressEng2='" + Current.HouseBL[Count_HouseBL].ShipperAddress.ADDR_ENG2 + @"';
	SET @ShipperAddressEng3='" + Current.HouseBL[Count_HouseBL].ShipperAddress.ADDR_ENG3 + @"';
	SET @ShipperAddressEng4='" + Current.HouseBL[Count_HouseBL].ShipperAddress.ADDR_ENG4 + @"';
	SET @ShipperAddressEng5='" + Current.HouseBL[Count_HouseBL].ShipperAddress.ADDR_ENG5 + @"';

	SET @ConsigneeAddressEng1 ='" + Current.HouseBL[Count_HouseBL].ConsigneeAddress.ADDR_ENG1 + @"';
	SET @ConsigneeAddressEng2 ='" + Current.HouseBL[Count_HouseBL].ConsigneeAddress.ADDR_ENG2 + @"';
	SET @ConsigneeAddressEng3 ='" + Current.HouseBL[Count_HouseBL].ConsigneeAddress.ADDR_ENG3 + @"';
	SET @ConsigneeAddressEng4 ='" + Current.HouseBL[Count_HouseBL].ConsigneeAddress.ADDR_ENG4 + @"';
	SET @ConsigneeAddressEng5 ='" + Current.HouseBL[Count_HouseBL].ConsigneeAddress.ADDR_ENG5 + @"';

use [impsea]; 

INSERT INTO CUSMAN 
           ([BLADVINo], [BLADVIMaster],[HSN],[Commodity]
,[SHPPhone],[SHPFax],[SHPZip],[SHPCountry],[SHPState]
,[SHPCity],[CNEPhone],[CNEFax],[CNEZip],[CNECountry]
,[CNEState],[CNECity],[NFYPhone],[NFYFax],[NFYZip]
,[NFYCountry],[NFYState],[NFYCity],[Notice1st]
,[Notice2nd]
,[Notice3rd],[Special1st],[Special2nd],[Special3rd],[DischargeType]
,[DischargePlace],[BillOrigin],[BillDestination],[TruckingCompany]
,[WareHouse],[Assignment],[AssignmentText]
,[LoadingType],[ReferenceNo],[ModType],[ModReason],[AmountUSD],[cProperty])
     VALUES
           (@BLNo, @BLADVIMaster, @HSN, @Description
			, '', '', '', '', ''
			, '', '', '', '', ''
			, '', '', '', '', ''
			, '', '', '', 
			'Y', 'Y'
			, '', '', '', '', ''
			, '', '', '', ''
			, @WarehouseCode, '" + Current.CUSMAN_Assignment.F1 + @"', '" + Current.CUSMAN_Assignment.F2 + @"'
			, '', '', '', '', 0, '');

INSERT INTO [impsea].[dbo].[CUSMANEquipment]
           ([BLADVIMaster],[BLADVINo],[CSN],[CONTNRNo],[CONTNRTypes],[CONTNRSeal1],[CONTNRSeal2],[CONTNRSeal3]
           ,[Pieces],[PiecesUnit],[ModType],[ModReason],[OldCNNo])
     VALUES
           (@BLADVIMaster, @BLNo, '1', @ContainerNo, @ContainerSize, @SealNo, '', '', @Count, @QuantityUnit, '', '', '');

INSERT INTO [impsea].[dbo].[BLADVI]
           ([BLADVINo],[BLADVIMaster],[SHPREQNo],[MHType],[IEType],[NLType],[DCType],[PreCarriage],[ReceiptPlace]
           ,[VoyageNo],[ShipName],[DischargePort],[DeliveryPlace],[LoadgPort],[LoadgDate],[FinalPort],[FinalDate]
           ,[ETA],[ETD],[SHPCode],[CNECode],[NFYCode],[SHPName],[CNEName],[NFYName],[CNEAcct],[NFYAcct],[IssuePort]
           ,[IssueDate],[IssueSign],[UnitWTVALPPD],[UnitWTVALCOL],[UnitOtherPPD],[UnitOtherCOL],[INVOICDate]
           ,[INVOICNo],[INVOICNoUser],[INVOICType],[FirstDate],[TextName],[Status1st],[Status2nd],[Status3rd]
           ,[BranchCode],[ModifyDate],[DOSendFlag],[Payment],[DODate],[DOReceiverName],[BLType],[ProfitFlag],[ACCDate]
           ,[ModifyTime],[ModifyMan],[SendDate],[SendTime],[ProfitDate],[ProfitComputeFlag],[MemoText],[DelayReason]
           ,[WorkType],[FWDCode],[SpecialCode])
     VALUES
           (@BLNo, @BLADVIMaster, @SHPREQNo,'H', 'I', 'L', 'C', @ShipCode, ''
			, @VoyageNo, @ShipName, '', '', @LoadgPort, @LoadingDate, @FinalPort, @ArrivalDate
			, @ArrivalDate, @LoadingDate, @SCode, @CCode,'SAMEAS', @SName,@CName, 'SAME AS ABOVE', @CSaupja,'', @LoadgPort
			,@LoadingDate, '', '', '', '', '', ''
			, '', '', '',@LoadingDate,'119', '', '', ''
			, '',@LoadingDate,'Y', '', '', 'SAME AS ABOVE','S', '', ''
			,'16:52:47','user', '', '', @ArrivalDate, 'N', '', ''
			, '', '', '');

INSERT INTO [impsea].[dbo].[BLADVIAddr]
           ([BLADVINo],[SHP_ENG1],[SHP_ENG2],[SHP_ENG3],[SHP_ENG4],[SHP_ENG5],[CNE_ENG1],[CNE_ENG2],[CNE_ENG3]
           ,[CNE_ENG4],[CNE_ENG5],[NFY_ENG1],[NFY_ENG2],[NFY_ENG3],[NFY_ENG4],[NFY_ENG5])
     VALUES
           (@BLNo, @ShipperAddressEng1, @ShipperAddressEng2, @ShipperAddressEng3, @ShipperAddressEng4, @ShipperAddressEng5
			,@ConsigneeAddressEng1,@ConsigneeAddressEng2, @ConsigneeAddressEng3, @ConsigneeAddressEng4, @ConsigneeAddressEng5, '', '', '', '', '');

INSERT INTO [impsea].[dbo].[BLADVISettle]
           ([BLADVINo],[Item],[Area],[SalesMan],[PunchMan],[SealMan],[Signer],[InvoiceNo],[ExDate],[ExRate],[ExCurrency]
           ,[PTNCode],[ACCCode],[COLCode],[TrustCode],[TrustName],[AgentCode],[AgentName])
     VALUES
           (@BLNo, '', '', '', '', '', '', '','2011/04/04', 0,'USD'
			, @PTNCode,@ACCCode, '', '', '', '', '');

INSERT INTO [impsea].[dbo].[BLADVIQuantity]
           ([BLADVINo],[QtyWork],[Pieces],[Weight],[Volume],[RevenueTon],[PiecesUnit],[WeightUnit],[CONT20FT]
           ,[CONT40FT],[CONT20FTHQ],[CONT40FTHQ],[LCL],[RATEClass])
	VALUES
           (@BLNo, 'B', @Count, @Weight, @Volume,0 ,@QuantityUnit,'K',0,0,0,0,@Volume, '');
");
			return Query.ToString();
		}
	}

	public struct sYuhan_Master
	{
		public string DocumentPk;
		public string TBBHPk;
		public int TotalPackedCount;
		public decimal TotalWeight;
		public decimal TotalVolume;
		public SHPREQ SHPREQ;
		public BLADVI BLADVI;
		public BLADVISettle BLADVISettle;
		public BLADVIQuantity BLADVIQuantity;
		public CUSREP CUSREP;
		public SHPREQAddr SHPREQAddr;
		public Code PreCarriage;
		public Code LoadPort;
		public Code FinalPort;
		public Code Partner;
		public Code ACC;
		public Address ShipperAddress;
		public Address ConsigneeAddress;
		public Address NotifyAddress;
		public CUSMANEquipment CUSMANEquipment;
		public Code Warehouse;
		public Code CUSMAN_Assignment;
		public List<HouseBL> HouseBL;
	}
	public struct SHPREQ
	{
		public string SHPREQNo;
		public string SHPREQDate;
		public string BookingNo;
		public string VoyageNo;
		public string ShipName;
		public string LoadingType;
		public string DeliveryPlace;
		public string PTNCode;
		public string ACCCode;
		public string LoadgPort;
		public string DischargePort;
		public string FinalPort;
		public string UnitWTVALPPD;
		public string UnitWTVALCOL;
		public string SHPCode;
		public string SHPName;
		public string CNEAcct;
		public string CNECode;
		public string CNEName;
		public string NFYCode;
		public string NFYName;
		public string TextName;
		public string FirstDate;
		public string BLADVIMaster;
		public string NFYAcct;
		public string LoadgPortName;
		public string FinalPortName;
		public string DischargePortName;
		public string DeliveryPlaceName;
	}
	public struct BLADVI
	{
		public string IssueDate; public string IssuePort; public string IssueSign; public string UnitWTVALPPD; public string FinalPort; public string INVOICDate; public string INVOICNo; public string INVOICType; public string FirstDate; public string TextName; public string Status1st; public string Status2nd; public string Status3rd; public string UnitWTVALCOL; public string UnitOtherPPD; public string UnitOtherCOL; public string LoadgDate; public string FinalDate; public string BranchCode; public string INVOICNoUser; public string ModifyDate; public string ProfitDate; public string UCRcode;
	}
	public struct BLADVISettle
	{
		public string Item; public string BLADVINo; public string Area; public string SalesMan; public string PunchMan; public string SealMan; public string Signer; public string InvoiceNo; public string ExDate; public string ExRate; public string ExCurrency; public string PTNCode; public string ACCCode; public string COLCode;
	}
	public struct BLADVIQuantity
	{
		public string RateClass; public string LCL; public string CONT40FTHQ; public string CONT20FTHQ; public string CONT40FT; public string CONT20FT; public string WeightUnit; public string PiecesUnit; public string RevenueTon; public string Volume; public string Weight; public string Pieces; public string BLADVINo; public string QtyWork;
	}
	public struct CUSREP
	{
		public string RFFFileName; public string ModSerialNo; public string HouseCount; public string CustomsDepart; public string Customs; public string ReportTime; public string ReportDate; public string sFunction; public string Verification; public string CallSign; public string BLADVIMaster; public string MRN; public string MSN;
	}
	public struct SHPREQAddr
	{
		public string SHP_ENG1; public string SHP_ENG2; public string SHP_ENG3; public string SHP_ENG4; public string SHP_ENG5; public string CNE_ENG1; public string CNE_ENG2; public string CNE_ENG3; public string CNE_ENG4; public string CNE_ENG5; public string NFY_ENG1; public string NFY_ENG2; public string NFY_ENG3; public string NFY_ENG4; public string NFY_ENG5;
	}
	public struct Code
	{
		public string F1;
		public string F2;
	}
	public struct Address
	{
		public string ADDR_ENG1;
		public string ADDR_ENG2;
		public string ADDR_ENG3;
		public string ADDR_ENG4;
		public string ADDR_ENG5;
	}
	public struct CUSMANEquipment
	{
		public string CONTNRNo; public string CONTNRTypes; public string CONTNRSeal1;
	}
	public struct HouseBL
	{
		public string HSN;
		public string BLNo;
		public string PackedCount;
		public string PackingUnit;
		public string Weight;
		public string Volume;
		public string ShipperCode;
		public string ShipperName;
		public Address ShipperAddress;
		public string ConsigneeCode;
		public string ConsigneeName;
		public Address ConsigneeAddress;
		public string ConsigneeSaupjaNo;
		public string Description;
	}
}
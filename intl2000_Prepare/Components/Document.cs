using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
	public class Document
	{
		private DBConn DB;

        private string ToDB(string Value, string DataType) {
            if (Value == "" || Value == null) {
                return "NULL";
            } else if (DataType == "nvarchar" || DataType == "nchar") {
                return "N'" + Value + "'";
            } else if (DataType == "string" || DataType == "varchar" || DataType == "char") {
                return "'" + Value + "'";
            } else if (DataType == "int" || DataType == "decimal" || DataType == "smallint" || DataType == "tinyint" || DataType == "money") {
                return Value.Replace(",", "");
            } else {
                return Value;
            }
        }

        public string Set_Document(sDocument dc, ref DBConn DB) {
			StringBuilder Query = new StringBuilder();
			GetQuery GQ = new GetQuery();
			Query.Append(GQ.Document(dc));
			DB.SqlCmd.CommandText = "" + Query;
			DB.SqlCmd.ExecuteNonQuery();

			return "1";
        }
		/*
        public string Delete_Document(string DocumentPk, ref DBConn DB) {
            string Query = @"DELETE FROM [dbo].[Document] WHERE [DocumentPk] = " + ToDB(DocumentPk, "int") + ";";
            DB.SqlCmd.CommandText = Query;
            DB.SqlCmd.ExecuteNonQuery();
            return "1";
        }
		*/
        public string SetSave(sTranfer transfer) {
			string query = Utility.ToDB("[dbo].[Transfer]",
				new List<string[]>() {
					new string[] { "TransferId", transfer.TransferId, "int" },
					new string[] { "CompanyId_From", transfer.CompanyId_From, "int" },
					new string[] { "CompanyId_To", transfer.CompanyId_To, "int" },
					new string[] { "BranchId_From", transfer.BranchId_From, "int" },
					new string[] { "BranchId_To", transfer.BranchId_To, "int" },
					new string[] { "CompanyInDocumentId_Shipper", transfer.CompanyInDocumentId_Shipper, "int" },
					new string[] { "CompanyBankId_Shipper", transfer.CompanyBankId_Shipper, "int" },
					new string[] { "CompanyBankId_FinalTransfer", transfer.CompanyBankId_FinalTransfer, "int" },
					new string[] { "MonetaryUnit", transfer.MonetaryUnit, "varchar" },
					new string[] { "Amount", transfer.Amount, "decimal" },
					new string[] { "Status", transfer.Status, "tinyint" },
					new string[] { "Date_Send", transfer.Date_Send, "varchar" }
			});
			transfer.TransferId = Utility.ExecuteScalar(query, null);
			return transfer.TransferId;
		}
		public string GetQuery_SetCalc_Document(string TransferId, string Sum_DocumentPk) {
			string query = string.Format(@"
				UPDATE [INTL2010].[dbo].[Document] SET ParentsType=NULL, ParentsId=null  WHERE DocumentPk not in ({1}) and ParentsId={0} and ParentsType='Transfer'
				UPDATE [INTL2010].[dbo].[Document] SET ParentsType='Transfer', ParentsId={0} WHERE DocumentPk in ({1}) 
				DECLARE @TotalAmount decimal(18, 4); 
				SELECT @TotalAmount = SUM([ValueDecimal0])
				  FROM [INTL2010].[dbo].[Document] 
				  WHERE ParentsId={0} and ParentsType='Transfer';
				SELECT @TotalAmount ;
				UPDATE [dbo].[Transfer] SET [Amount] = @TotalAmount WHERE [TransferId]={0};", TransferId, Sum_DocumentPk);
			return query;
		}
		public string SetSave(sDebitCredit debit) {
			decimal TotalAmount = 0;
			for (var i = 0; i < debit.InnerPrice.Count; i++) {
				TotalAmount += debit.InnerPrice[i].ValueDecimal0;
			}

			string query = Utility.ToDB("[dbo].[Document]", new List<string[]>() {
				new string[] {"DocumentPk",  debit.DocumentPk+"", "int" },
				new string[] {"Type",  "DebitCredit" },
				new string[] {"TypePk",  debit.TBBHPk, "int"},
				new string[] {"Status",  "0" , "int"},
				new string[] {"Value0",  debit.ShipperName },
				new string[] {"Value1",  debit.ShipperAddress },
				new string[] {"Value2",  debit.ShipperTEL },
				new string[] {"Value3",  debit.ShipperFAX },
				new string[] {"Value4",  debit.ConsigneeName },
				new string[] {"Value5",  debit.ConsigneeAddress },
				new string[] {"Value6",  debit.ConsigneeTEL },
				new string[] {"Value7",  debit.ConsigneeFAX },
				new string[] {"Value8",  debit.VesselName },
				new string[] {"Value9",  debit.Container },
				new string[] {"Value10",  debit.Quantity },
				new string[] { "Value11",  debit.Weight },
				new string[] { "Value12",  debit.Measurment },
				new string[] { "Value13",  debit.IssueDate },
				new string[] { "Value14",  debit.ETD },
				new string[] { "Value15",  debit.ETA },
				new string[] { "Value16",  debit.POL },
				new string[] { "Value17",  debit.POD },
				new string[] { "ValueDecimal0",  TotalAmount.ToString(), "decimal(18, 4)" },
			});
			DB = new DBConn();
			DB.DBCon.Open();
			debit.DocumentPk = Utility.ExecuteScalar(query, DB);
			StringBuilder Query = new StringBuilder();
			StringBuilder Sum_DocumentBodyId_ForNotDeleted = new StringBuilder();
			for (var i = 0; i < debit.InnerPrice.Count; i++) {
				if (debit.InnerPrice[i].DocumentBodyPk != "") {
					Sum_DocumentBodyId_ForNotDeleted.Append(", " + debit.InnerPrice[i].DocumentBodyPk);
				}
				Query.Append(GetQuery_DocumentBody(debit.InnerPrice[i], debit.DocumentPk, "0"));
			}

			if (Sum_DocumentBodyId_ForNotDeleted.ToString() != "") {
				Utility.Excute("DELETE   FROM [INTL2010].[dbo].[DocumentBody] WHERE DocumentBodyPk not in (" + Sum_DocumentBodyId_ForNotDeleted.ToString().Substring(1) + ") and DocumentPk =" + debit.DocumentPk + " ;", DB);
			}
			Utility.Excute(Query.ToString(), DB);
			DB.DBCon.Close();
			return "1";
		}


		//SettlementEach : 1
		//SettlementRequest : 7
		//SettlementRequestEach: 8

		private string GetQuery_DocumentBody(sDocumentBody Current, string DocumentPk, string Category) {
			string query = Utility.ToDB("[dbo].[DocumentBody]", new List<string[]>() {
				new string[] {"DocumentBodyPk", Current.DocumentBodyPk, "int" },
				new string[] {"DocumentPk", DocumentPk, "int" },
				new string[] {"Category", Category, "int" },
				new string[] {"Value0", Current.Value0 },
				new string[] {"Value1", Current.Value1 },
				new string[] {"Value2", Current.Value2 },
				new string[] {"Value3", Current.Value3 },
				new string[] {"Value4", Current.Value4 },
				new string[] {"Value5", Current.Value5 },
				new string[] {"Value6", Current.Value6 },
				new string[] {"Value7", Current.Value7 },
				new string[] {"Value8", Current.Value8 },
				new string[] {"Value9", Current.Value9 },
				new string[] { "ValueInt0", Current.ValueInt0+"", "int" },
				new string[] { "ValueDecimal0", Current.ValueDecimal0.ToString(), "decimal" }
			});
			return query;
		}
		public sDebitCredit LoadDebitCredit(string TypePk) {
			sDebitCredit ReturnValue = new sDebitCredit();
			string query = @"
			SELECT [DocumentPk],[Type],[TypePk],[Status]
				,[Value0],[Value1],[Value2],[Value3],[Value4],[Value5],[Value6],[Value7],[Value8],[Value9],[Value10],[Value11],[Value12],[Value13],[Value14],[Value15],[Value16],[Value17]
				,[ValueInt0], isnull([ValueDecimal0], 0) AS  ValueDecimal0 
			FROM [INTL2010].[dbo].[Document] 
			WHERE [Type]='DebitCredit' and  TypePk=" + TypePk + @";";
			DB = new DBConn();
			DB.DBCon.Open();
			DB.SqlCmd.CommandText = query;
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				ReturnValue.DocumentPk = RS["DocumentPk"] + "";
				ReturnValue.TBBHPk = RS["TypePk"] + "";
				ReturnValue.Status = RS["Status"] + "";
				ReturnValue.ShipperName = RS["Value0"] + "";
				ReturnValue.ShipperAddress = RS["Value1"] + "";
				ReturnValue.ShipperTEL = RS["Value2"] + "";
				ReturnValue.ShipperFAX = RS["Value3"] + "";
				ReturnValue.ConsigneeName = RS["Value4"] + "";
				ReturnValue.ConsigneeAddress = RS["Value5"] + "";
				ReturnValue.ConsigneeTEL = RS["Value6"] + "";
				ReturnValue.ConsigneeFAX = RS["Value7"] + "";
				ReturnValue.VesselName = RS["Value8"] + "";
				ReturnValue.Container = RS["Value9"] + "";
				ReturnValue.Quantity = RS["Value10"] + "";
				ReturnValue.Weight = RS["Value11"] + "";
				ReturnValue.Measurment = RS["Value12"] + "";
				ReturnValue.IssueDate = RS["Value13"] + "";
				ReturnValue.ETD = RS["Value14"] + "";
				ReturnValue.ETA = RS["Value15"] + "";
				ReturnValue.POL = RS["Value16"] + "";
				ReturnValue.POD = RS["Value17"] + "";
				ReturnValue.TotalPrice = decimal.Parse(RS["ValueDecimal0"] + "");
			}
			RS.Close();
			if (ReturnValue.DocumentPk != null) {
				ReturnValue.InnerPrice = new List<sDocumentBody>();
				query = @"
			SELECT 
				[DocumentBodyPk],[DocumentPk],[Category]
				,[Value0],[Value1],[Value2],[Value3],[Value4],[Value5],[Value6],[Value7],[Value8],[Value9],[ValueInt0], isnull([ValueDecimal0], 0) AS ValueDecimal0
			FROM [INTL2010].[dbo].[DocumentBody] 
			WHERE DocumentPk=" + ReturnValue.DocumentPk + ";";
				DB.SqlCmd.CommandText = query;
				RS = DB.SqlCmd.ExecuteReader();
				while (RS.Read()) {
					ReturnValue.InnerPrice.Add(new sDocumentBody() {
						DocumentBodyPk = RS["DocumentBodyPk"] + "",
						Value0 = RS["Value0"] + "",
						Value1 = RS["Value1"] + "",
						ValueDecimal0 = decimal.Parse(RS["ValueDecimal0"] + "")
					});
				}
				RS.Close();
				DB.DBCon.Close();
				return ReturnValue;
			} else {
				return MakeDebitCredit(TypePk);
			}
		}
		private sDebitCredit MakeDebitCredit(string TransportBBHPk) {
			sDebitCredit ReturnValue = new sDebitCredit();
			ReturnValue.DocumentPk = "";
			ReturnValue.TBBHPk = TransportBBHPk;
			ReturnValue.Status = "0";
			ReturnValue.ShipperName = "";
			ReturnValue.ShipperAddress = "";
			ReturnValue.ShipperTEL = "";
			ReturnValue.ShipperFAX = "";
			ReturnValue.ConsigneeName = "INTERNATIONAL LOGISTICS CO., LTD.";
			ReturnValue.ConsigneeAddress = "IL BLDG., 31-1,SHINHEUNG-DONG 1GA,JUNG-GU, INCHEON, KOREA";
			ReturnValue.ConsigneeTEL = "032-772-8481";
			ReturnValue.ConsigneeFAX = "032-765-8688/9";
			DB = new DBConn();
			DB.DBCon.Open();
			DB.SqlCmd.CommandText = "SELECT [TRANSPORT_STATUS] FROM [dbo].[TRANSPORT_HEAD] WHERE [TRANSPORT_PK]=" + TransportBBHPk + ";";
			//return DB.SqlCmd.CommandText;
			string stepCL = DB.SqlCmd.ExecuteScalar() + "";
			string Table;
			string HeadColumn;
			string RequestCloumn;
			string queryWhere;
			if (stepCL == "" || stepCL == "3") {
				Table = "TransportBBHistory";
				HeadColumn = "[TransportBetweenBranchPk]";
				RequestCloumn = "[RequestFormPk]";
				queryWhere = "";

			} else {
				Table = "[dbo].[TRANSPORT_BODY]";
				HeadColumn = "[TRANSPORT_HEAD_PK]";
				RequestCloumn = "[REQUEST_PK]";
				queryWhere = " ";
			}
			DB.SqlCmd.CommandText = @"		SELECT BBHead.[TRANSPORT_WAY], BBHead.[VALUE_STRING_0], BBHead.[BRANCHPK_FROM], BBHead.[DATETIME_FROM], BBHead.[DATETIME_TO]
																	, BBHead.[AREA_FROM], BBHead.[AREA_TO], BBHead.[VESSELNAME], BBHead.[VOYAGE_NO]
																	, BBHead.[VALUE_STRING_1], BBHead.[VALUE_STRING_2], BBHead.[VALUE_STRING_3], BBHead.[TRANSPORT_STATUS], BBHead.[TRANSPORT_STATUS]
																	, DC.CompanyName AS FromCompany, AC.CompanyName AS ToCompany, TOTAL.CT, TOTAL.GW, TOTAL.VO, BBPacked.NO AS PACKED_NO
																FROM [dbo].[TRANSPORT_HEAD] AS BBHead 
																	left join Company AS DC on DC.CompanyPk=BBHead.[BRANCHPK_FROM] 
																	left join Company AS AC on AC.CompanyPk=BBHead.[BRANCHPK_TO] 	
																	LEFT JOIN [dbo].[TRANSPORT_PACKED] AS BBPacked ON BBHead.[TRANSPORT_PK] = BBPacked.[TRANSPORT_HEAD_PK]
																	left join (
																		SELECT OBS." + HeadColumn + @", SUM(RF.TotalPackedCount) AS CT, SUM(RF.TotalGrossWeight) AS GW, SUM(RF.TotalVolume) AS VO
																		FROM " + Table + @" AS OBS 
																		LEFT JOIN [dbo].[RequestForm] AS RF ON RF.RequestFormPk=OBS." + RequestCloumn + @"
																		WHERE OBS." + HeadColumn + @"=" + TransportBBHPk + queryWhere + @" 
																		GROUP BY OBS." + HeadColumn + @") AS TOTAL ON TOTAL." + HeadColumn + @"=BBHead.[TRANSPORT_PK] 
																WHERE BBHead.[TRANSPORT_PK]=" + TransportBBHPk + ";";
			//return DB.SqlCmd.CommandText;
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			string BLNo = "";
			if (RS.Read()) {
				//qdlo1#@!하북주구-위해 직접발송#@!2011-04-13 19:45:59Z
				//%!$@#qdlo1#@!101#@!2011-04-13 19:47:07Z%!$@#qdlo1#@!101#@!2011-04-13 19:47:09Z%!$@#qdlo1#@!101#@!2011-04-13 19:47:10Z%!$@#qdlo1#@!101#@!2011-04-13 19:47:12Z%!$@#qdlo1#@!101#@!2011-04-13 19:47:13Z%!$@#qdlo1#@!101#@!2011-04-13 19:47:13Z%!$@#qdlo1#@!101#@!2011-04-13 19:47:14Z%!$@#qdlo1#@!101#@!2011-04-13 19:47:14Z%!$@#qdlo1#@!101#@!2011-04-13 19:47:14Z
				BLNo = RS["VALUE_STRING_0"] + "";
				if (RS["DATETIME_FROM"] + "" != "... :" && RS["DATETIME_FROM"] + "" != "") {
					string temp = RS["DATETIME_FROM"] + "";
					ReturnValue.ETD = temp.Substring(0, temp.IndexOf(" ") - 1).Replace(".", "/");
				}
				if (RS["DATETIME_TO"] + "" != "... :" && RS["DATETIME_TO"] + "" != "") {
					string temp = RS["DATETIME_TO"] + "";
					ReturnValue.ETA = temp.Substring(0, temp.IndexOf(" ") - 1).Replace(".", "/");
					ReturnValue.IssueDate = ReturnValue.ETA;
				}
				ReturnValue.Quantity = RS["CT"] + "";
				ReturnValue.Measurment = RS["VO"] + "";
				ReturnValue.Weight = RS["GW"] + "";
				ReturnValue.POL = RS["AREA_FROM"] + "";
				ReturnValue.POD = RS["AREA_TO"] + "";


				switch (RS["TRANSPORT_WAY"] + "") {
					case "Air":
						ReturnValue.TransportMethod = "Air";
						break;
					case "Ship":
						ReturnValue.TransportMethod = "Ship";
						break;
				}

				if (RS["TRANSPORT_WAY"] + "" == "Air") {
					//港???#@!?州新白云??机?#@!??仁川机?#@!港???#@!137-1153-9077#@!
					ReturnValue.VesselName = RS["VOYAGE_NO"] + "";
					//					"Container Type: AIR",
				} else {
					ReturnValue.VesselName = RS["VESSELNAME"] + " (" + RS["VOYAGE_NO"] + ")";
					ReturnValue.Container = RS["PACKED_NO"] + "";
				}
			}
			RS.Close();

			ReturnValue.InnerPrice = new List<sDocumentBody>();
			DB.SqlCmd.CommandText = @"	
														DECLARE @CNYToDollorFrom smallint;
														DECLARE @CNYToDollorTo smallmoney;
														DECLARE @KRWToDollorFrom smallint;
														DECLARE @KRWToDollorTo smallmoney;

														SELECT @CNYToDollorFrom =[ExchangeRateStandard]
															  ,@CNYToDollorTo =[ExchangeRate]
														  FROM [INTL2010].[dbo].[ExchangeRateHistory] 
														  WHERE MonetaryUnitTo=19 and MonetaryUnitFrom=18 and ETCSettingPk=7
														  ORDER BY DateSpan DESC 
														SELECT @KRWToDollorFrom =[ExchangeRateStandard]
															  ,@KRWToDollorTo =[ExchangeRate]
														  FROM [INTL2010].[dbo].[ExchangeRateHistory] 
														  WHERE MonetaryUnitTo=19 and MonetaryUnitFrom=20 and ETCSettingPk=7
														  ORDER BY DateSpan DESC 

														SELECT CD.BLNo, RFCB.Title,  
															CASE RFCB.MonetaryUnit	
																WHEN 20 then RFCB.Price * @KRWToDollorTo / @KRWToDollorFrom
																WHEN 19 then RFCB.Price
																WHEN 18 then RFCB.Price * @CNYToDollorTo / @CNYToDollorFrom
															END
														FROM " + Table + @" AS OBS 
															left join CommerdialConnectionWithRequest AS CCWR On OBS.[REQUEST_PK]=CCWR.RequestFormPk 
															left join CommercialDocument AS CD On CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk 
															left join RequestFormCalculateBody AS RFCB ON RFCB.RequestFormPk=OBS.[REQUEST_PK] 
															left join RequestFormCalculateHead AS RFCH ON RFCH.RequestFormPk =OBS.[REQUEST_PK] 
														WHERE 
															OBS.[TRANSPORT_HEAD_PK]=" + TransportBBHPk + queryWhere + @" 
															and GubunCL>200 
															and left(isnull(RFCB.StandardPriceHeadPkNColumn, '111'), 3) not in ('403', '407', '409', '410', '411', '412', '413', '414', '415', '416', '421', '422', '423', '424') 
															and isnull(RFCB.StandardPriceHeadPkNColumn, '111')<>'D' 
															and  RFCB.Title not in ('DEIVERY ORDER CHARGE', 'HANDLING CHARGE', 'DELIVERY ORDER CHARGE', 'BONDED WAREHOUSE CHARGE', 'WHARFAGE')
															ORDER BY CD.BLNo;";

			RS = DB.SqlCmd.ExecuteReader();
			List<String[]> S = new List<string[]>();
			string sPattern = "[a-zA-Z0-9\\s]$";
			while (RS.Read()) {
				string title = (RS[1] + "").IndexOf("OCEAN FREIGHT") == -1 ? RS[1] + "" : "OCEAN FREIGHT";
				if (!System.Text.RegularExpressions.Regex.IsMatch(title, sPattern)) {
					continue;
				}

				if (ReturnValue.TransportMethod == "LCL" && title == "OCEAN FREIGHT") {
					continue;
				}
				ReturnValue.InnerPrice.Add(new sDocumentBody() {
					Value0 = RS["BLNo"] + "",
					Value1 = title,
					ValueDecimal0 = decimal.Parse(RS[2] + "")
				});
			}

			ReturnValue.InnerPrice.Add(new sDocumentBody() {
				Value0 = BLNo,
				Value1 = "OCEAN FREIGHT",
				ValueDecimal0 = 0
			});
			ReturnValue.InnerPrice.Add(new sDocumentBody() {
				Value0 = BLNo,
				Value1 = "BAF / CAF / EBS",
				ValueDecimal0 = 0
			});

			if (ReturnValue.TransportMethod == "Ship") {
				ReturnValue.InnerPrice.Add(new sDocumentBody() {
					Value0 = BLNo,
					Value1 = "OTHER CHARGE",
					ValueDecimal0 = Decimal.Parse(ReturnValue.Measurment) * 5
				});
			}

			RS.Close();
			DB.DBCon.Close();
			decimal TotalPrice = 0;
			for (var i = 0; i < ReturnValue.InnerPrice.Count; i++) {
				TotalPrice += ReturnValue.InnerPrice[i].ValueDecimal0;
			}
			ReturnValue.TotalPrice = TotalPrice;
			return ReturnValue;
		}
	}

	public struct sTranfer
	{
		public string TransferId;
		public string CompanyId_From;
		public string CompanyId_To;
		public string BranchId_From;
		public string BranchId_To;
		public string CompanyInDocumentId_Shipper;
		public string CompanyBankId_Shipper;
		public string CompanyBankId_FinalTransfer;
		public string MonetaryUnit;
		public string Amount;
		public string Status;
		public string Date_Send;
		public List<sDebitCredit> ListDebit;
	}
	public struct sDebitCredit
	{
		public string DocumentPk;
		public string TBBHPk;
		public string Status;
		public string TransportMethod;
		public string ShipperName;
		public string ShipperAddress;
		public string ShipperTEL;
		public string ShipperFAX;
		public string ConsigneeName;
		public string ConsigneeAddress;
		public string ConsigneeTEL;
		public string ConsigneeFAX;
		public string VesselName;
		public string IssueDate;
		public string Container;
		public string ETD;
		public string ETA;
		public string Quantity;
		public string Weight;
		public string Measurment;
		public string POL;
		public string POD;
		public decimal TotalPrice;
		public List<sDocumentBody> InnerPrice;
		public List<sDocumentBody> Summary;
	}
	public struct sDocument
	{
		public string DocumentPk;
		public string Type;
		public string TypePk;
		public string Status;
		public string Value0;
		public string Value1;
		public string Value2;
		public string Value3;
		public string Value4;
		public string Value5;
		public string Value6;
		public string Value7;
		public string Value8;
        public string Value9;
        public string Value10;
        public string Value11;
        public string Value12;
        public string Value13;
        public string Value14;
        public string Value15;
        public string Value16;
        public string Value17;
        public string Value18;
        public string Value19;
        public string ValueInt0;
        public string ValueDecimal0;
        public string ValueDecimal1;
        public string ValueDecimal2;
        public string ParentsType;
        public string ParentsId;
        public List<sDocumentBody> InnerBody;
	}
	public struct sDocumentBody
	{
		public string DocumentBodyPk;
		public string Value0;
		public string Value1;
		public string Value2;
		public string Value3;
		public string Value4;
		public string Value5;
		public string Value6;
		public string Value7;
		public string Value8;
		public string Value9;
		public int ValueInt0;
		public decimal ValueDecimal0;
	}
}
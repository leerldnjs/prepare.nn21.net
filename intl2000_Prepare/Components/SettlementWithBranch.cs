using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
	public class SettlementWithBranch
	{
		private bool CalcSettlementContainer_ExchangeRate(ref SettlementContainer Current) {
			List<string[]> ExchangeRate = new List<string[]>();
			for (var i = 0; i < Current.ArrEach.Count; i++) {
				if (Current.ArrEach[i].MonetaryUnit != "20") {
					bool isin = false;
					foreach (string[] row in ExchangeRate) {
						if (Current.ArrEach[i].MonetaryUnitCL == row[0]) {
							isin = true;
							break;
						}
					}
					if (!isin) {
						ExchangeRate.Add(new string[] { Current.ArrEach[i].MonetaryUnit, "20", "", "" });
					}
				}
			}
			for (var h = 0; h < Current.ArrRequest.Count; h++) {
				for (var i = 0; i < Current.ArrRequest[h].ArrEach.Count; i++) {
					if (Current.ArrRequest[h].ArrEach[i].MonetaryUnit != "20") {
						bool isin = false;
						foreach (string[] row in ExchangeRate) {
							if (Current.ArrRequest[h].ArrEach[i].MonetaryUnitCL == row[0]) {
								isin = true;
								break;
							}
						}
						if (!isin) {
							ExchangeRate.Add(new string[] { Current.ArrRequest[h].ArrEach[i].MonetaryUnitCL, "20", "", "" });
						}
					}
				}
			}
			DBConn DB = new DBConn();
			DB.DBCon.Open();

			for (var i = 0; i < ExchangeRate.Count; i++) {
				DB.SqlCmd.CommandText = @"
SELECT TOP 1 [DateSpan]
		,[ExchangeRateStandard]
		,[ExchangeRate]
	FROM [INTL2010].[dbo].[ExchangeRateHistory]

	WHERE DateSpan<'" + Current.ExchangeRateDate + @"'  and ETCSettingPk=1 and MonetaryUnitFrom=" + ExchangeRate[i][0] + @" and MonetaryUnitTo=20
	ORDER BY DateSpan DESC ;";
				SqlDataReader RS = DB.SqlCmd.ExecuteReader();
				if (RS.Read()) {
					ExchangeRate[i][2] = "" + RS["ExchangeRateStandard"];
					ExchangeRate[i][3] = "" + RS["ExchangeRate"];
				}
				RS.Close();
			}
			DB.DBCon.Close();


			List<SettlementEach> CurrentArrEach = Current.ArrEach;
			List<SettlementRequest> CurrentArrRequest = Current.ArrRequest;
			Current.ArrEach = new List<SettlementEach>();
			Current.ArrRequest = new List<SettlementRequest>();

			for (var i = 0; i < CurrentArrEach.Count; i++) {
				SettlementEach TempEach = new SettlementEach();
				if (CurrentArrEach[i].MonetaryUnitCL == "20") {
					TempEach = CurrentArrEach[i];
					TempEach.Exchanged_MonetaryUnitCL = "20";
					TempEach.Exchanged_MonetaryUnit = Common.GetMonetaryUnit("20");
					TempEach.Exchanged_Price = CurrentArrEach[i].Price;
				} else if (CurrentArrEach[i].MonetaryUnit != "20") {
					foreach (string[] row in ExchangeRate) {
						if (row[0] == CurrentArrEach[i].MonetaryUnitCL) {
							TempEach = CurrentArrEach[i];
							TempEach.Exchanged_MonetaryUnitCL = "20";
							TempEach.Exchanged_MonetaryUnit = Common.GetMonetaryUnit("20");
							TempEach.Exchanged_Price = CurrentArrEach[i].Price / decimal.Parse(row[2]) * decimal.Parse(row[3]);
							TempEach.Exchanged_Price = Math.Round(TempEach.Exchanged_Price);
							break;
						}
					}
				} else {
					continue;
				}
				Current.ArrEach.Add(TempEach);
			}
			for (var h = 0; h < CurrentArrRequest.Count; h++) {
				SettlementRequest Each = CurrentArrRequest[h];
				Each.ArrEach = new List<SettlementEach>();

				for (var i = 0; i < CurrentArrRequest[h].ArrEach.Count; i++) {
					SettlementEach TempEach = new SettlementEach();
					if (CurrentArrRequest[h].ArrEach[i].MonetaryUnitCL == "20") {
						TempEach = CurrentArrRequest[h].ArrEach[i];
						TempEach.Exchanged_MonetaryUnitCL = "20";
						TempEach.Exchanged_MonetaryUnit = Common.GetMonetaryUnit("20");
						TempEach.Exchanged_Price = CurrentArrRequest[h].ArrEach[i].Price;
					} else if (CurrentArrRequest[h].ArrEach[i].MonetaryUnit != "20") {
						foreach (string[] row in ExchangeRate) {
							if (row[0] == CurrentArrRequest[h].ArrEach[i].MonetaryUnitCL) {
								TempEach = CurrentArrRequest[h].ArrEach[i];
								TempEach.Exchanged_MonetaryUnitCL = "20";
								TempEach.Exchanged_MonetaryUnit = Common.GetMonetaryUnit("20");
								TempEach.Exchanged_Price = CurrentArrRequest[h].ArrEach[i].Price / decimal.Parse(row[2]) * decimal.Parse(row[3]);
								TempEach.Exchanged_Price = Math.Round(TempEach.Exchanged_Price);
								break;
							}
						}
					} else {
						continue;
					}
					Each.ArrEach.Add(TempEach);
				}
				Current.ArrRequest.Add(Each);
			}
			return true;
		}

		public string SetSave(ref SettlementContainer Current) {
			string query;
			DBConn DB = new DBConn();
			DB.DBCon.Open();
			query = Utility.ToDB("[dbo].[Document]", new List<string[]>() {
					new string[] {"DocumentPk",  Current.DocumentPk+"", "int" },
					new string[] {"Type",  "SettlementWithBranch" },
					new string[] {"TypePk",  Current.TBBHPk, "int"},
					new string[] {"Status",  Current.Status , "int"},
					new string[] {"Value0",  Current.TransportWayCL },
					new string[] {"Value1",  Current.ContainerNo},
					new string[] {"Value2",  Current.BLNo},
					new string[] {"Value3",  Current.FromRegion },
					new string[] {"Value4",  Current.ToRegion},
					new string[] {"Value5",  Current.FromDate},
					new string[] {"Value6",  Current.ToDate},
					new string[] {"Value7",  Current.ExchangeRateDate},
					new string[] {"Value8",  Current.LastWarehouse},
					new string[] {"Value9",  Current.MonetaryYW },
					new string[] { "ValueInt0", Current.BranchPk_Own, "int" },
					new string[] { "ValueDecimal0", Current.AmountYW.ToString(), "decimal(18, 4)" },
				});
			if (Current.DocumentPk == "" || Current.DocumentPk == null) {
				Current.DocumentPk = Utility.ExecuteScalar(query, DB);
			} else {
				Utility.ExecuteScalar(query, DB);
			}
			StringBuilder Query = new StringBuilder();

			for (var i = 0; i < Current.ArrEach.Count; i++) {
				query = GetQuery_DocumentBody(Current.ArrEach[i], Current.DocumentPk, "1");
				Query.Append(query);
			}

			for (var i = 0; i < Current.ArrRequest.Count; i++) {
				query = GetQuery_DocumentBody(Current.ArrRequest[i], Current.DocumentPk, "7");
				Query.Append(query);
			}

			if (Current.DocumentPk + "" != "") {
				Utility.Excute("DELETE   FROM [INTL2010].[dbo].[DocumentBody] WHERE DocumentPk =" + Current.DocumentPk + " ;", DB);
			}
			Utility.Excute(Query.ToString(), DB);
			DB.DBCon.Close();
			return "1";
		}

		private string GetQuery_DocumentBody(SettlementEach Current, string DocumentPk, string Category) {
			string query = Utility.ToDB("[dbo].[DocumentBody]", new List<string[]>() {
				new string[] {"DocumentBodyPk", "", "int" },
				new string[] {"DocumentPk", DocumentPk, "int" },
				new string[] {"Category", Category, "int" },
				new string[] {"Value0", Current.Description },
				new string[] {"Value1", Current.MonetaryUnitCL},
				new string[] {"Value2", Current.MonetaryUnit},
				new string[] {"Value3", Current.Price.ToString()},
				new string[] {"Value4", Current.Exchanged_MonetaryUnitCL},
				new string[] {"Value5", Current.Exchanged_MonetaryUnit},
				new string[] {"Value8", Current.Type},
				new string[] {"Value9", Current.TypePk},
				new string[] { "ValueInt0", Current.BranchPk_Own + "", "int" },
				new string[] { "ValueDecimal0", Current.Exchanged_Price.ToString(), "decimal" }
			});
			return query;
		}
		private string GetQuery_DocumentBody(SettlementRequest Current, string DocumentPk, string Category) {
			string query = Utility.ToDB("[dbo].[DocumentBody]", new List<string[]>() {
				new string[] {"DocumentBodyPk", "", "int" },
				new string[] {"DocumentPk", DocumentPk, "int" },
				new string[] {"Category", Category, "int" },
				new string[] {"Value0", Current.FromDate+" - "+Current.ToDate},
				new string[] {"Value1", Current.FromRegion+" ~ "+Current.ToRegion},
				new string[] {"Value2", Current.TransportDescription},
				new string[] {"Value3", Current.ConsigneeCode},
				new string[] {"Value4", Current.CT},
				new string[] {"Value5", Current.PackingUnit},
				new string[] {"Value6", Current.Kg},
				new string[] {"Value7", Current.CBM},
				new string[] {"Value8", "RequestForm"},
				new string[] {"Value9", Current.RequestPk},
				new string[] { "ValueInt0", Current.BranchPk_Own + "", "int" }
			});
			StringBuilder queries = new StringBuilder();
			queries.Append(query);

			foreach (SettlementEach each in Current.ArrEach) {
				query = GetQuery_DocumentBody(each, DocumentPk, "8");
				queries.Append(query);
			}
			return queries.ToString();
		}

		public SettlementContainer LoadSettlementContainer(string DocumentPk) {
			SettlementContainer ReturnValue = new SettlementContainer();
			DBConn DB = new DBConn();
			DB.SqlCmd.CommandText = @"
SELECT [DocumentPk]
      ,[Type]
      ,[TypePk]
      ,[Status]
      ,[Value0]
      ,[Value1]
      ,[Value2]
      ,[Value3]
      ,[Value4]
      ,[Value5]
      ,[Value6]
      ,[Value7]
      ,[Value8]
      ,[Value9]
      ,[Value10]
      ,[Value11]
      ,[Value12]
      ,[Value13]
      ,[Value14]
      ,[Value15]
      ,[Value16]
      ,[Value17]
      ,[Value18]
      ,[Value19]
      ,[ValueInt0]
      ,[ValueDecimal0]
      ,[ValueDecimal1]
      ,[ValueDecimal2]
      ,[ParentsType]
      ,[ParentsId]
  FROM [INTL2010].[dbo].[Document] 
  WHERE DocumentPk=" + DocumentPk + ";";
			DB.DBCon.Open();
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				ReturnValue.DocumentPk = DocumentPk;
				ReturnValue.TBBHPk = RS["TypePk"] + "";
				ReturnValue.Status = RS["Status"] + "";
				ReturnValue.TransportWayCL = RS["Value0"] + "";
				ReturnValue.ContainerNo = RS["Value1"] + "";
				ReturnValue.BLNo = RS["Value2"] + "";
				ReturnValue.FromRegion = RS["Value3"] + "";
				ReturnValue.ToRegion = RS["Value4"] + "";
				ReturnValue.FromDate = RS["Value5"] + "";
				ReturnValue.ToDate = RS["Value6"] + "";
				ReturnValue.ExchangeRateDate = RS["Value7"] + "";
				ReturnValue.LastWarehouse = RS["Value8"] + "";
				ReturnValue.MonetaryYW = RS["Value9"] + "";
				ReturnValue.BranchPk_Own = RS["ValueInt0"] + "";
				ReturnValue.AmountYW = decimal.Parse(RS["ValueDecimal0"] + "");
			}
			RS.Close();

			DB.SqlCmd.CommandText = @"
SELECT 
	[DocumentBodyPk],[DocumentPk],[Category]
	,[Value0],[Value1],[Value2],[Value3],[Value4],[Value5],[Value6],[Value7],[Value8],[Value9]
	,[ValueInt0]
    ,[ValueDecimal0]
FROM [INTL2010].[dbo].[DocumentBody] 
  WHERE DocumentPk=" + DocumentPk + " and Value8='TransportBBCharge'";
			RS = DB.SqlCmd.ExecuteReader();

			ReturnValue.ArrEach = new List<SettlementEach>();
			SettlementEach TempSettlementEach;
			while (RS.Read()) {
				TempSettlementEach = new SettlementEach();
				TempSettlementEach.DocumentBodyPk = RS["DocumentBodyPk"] + "";
				TempSettlementEach.DocumentPk = DocumentPk;
				TempSettlementEach.Description = RS["Value0"] + "";
				TempSettlementEach.MonetaryUnitCL = RS["Value1"] + "";
				TempSettlementEach.MonetaryUnit = RS["Value2"] + "";
				TempSettlementEach.Price = decimal.Parse(RS["Value3"] + "");
				TempSettlementEach.Exchanged_MonetaryUnitCL = RS["Value4"] + "";
				TempSettlementEach.Exchanged_MonetaryUnit = RS["Value5"] + "";
				TempSettlementEach.Type = RS["Value8"] + "";
				TempSettlementEach.TypePk = RS["Value9"] + "";
				TempSettlementEach.BranchPk_Own = RS["ValueInt0"] + "";
				TempSettlementEach.Exchanged_Price = decimal.Parse(RS["ValueDecimal0"] + "");
				ReturnValue.ArrEach.Add(TempSettlementEach);
			}
			RS.Close();

			DB.SqlCmd.CommandText = @"
SELECT 
	[DocumentBodyPk],[DocumentPk],[Category]
	,[Value0],[Value1],[Value2],[Value3],[Value4],[Value5],[Value6],[Value7],[Value8],[Value9]
	,[ValueInt0]
    ,[ValueDecimal0]
FROM [INTL2010].[dbo].[DocumentBody] 
  WHERE DocumentPk=" + DocumentPk + " and Value8='RequestForm'";
			RS = DB.SqlCmd.ExecuteReader();

			ReturnValue.ArrRequest = new List<SettlementRequest>();
			SettlementRequest TempSettlementRequest;
			while (RS.Read()) {
				TempSettlementRequest = new SettlementRequest();

				TempSettlementRequest.DocumentPk = DocumentPk;
				string[] date = RS["Value0"].ToString().Split(new string[] { " - " }, StringSplitOptions.None);
				TempSettlementRequest.FromDate = date[0];
				if (date.Length > 1) {
					TempSettlementRequest.ToDate = date[1];
				}

				string[] region = RS["Value1"].ToString().Split(new string[] { " ~ " }, StringSplitOptions.None);
				TempSettlementRequest.FromRegion = region[0];
				if (region.Length > 1) {
					TempSettlementRequest.ToRegion = region[1];
				}
				TempSettlementRequest.DocumentBodyPk = RS["DocumentBodyPk"] + "";
				TempSettlementRequest.TransportDescription = RS["Value2"] + "";
				TempSettlementRequest.ConsigneeCode = RS["Value3"] + "";
				TempSettlementRequest.CT = RS["Value4"] + "";
				TempSettlementRequest.PackingUnit = RS["Value5"] + "";
				TempSettlementRequest.Kg = RS["Value6"] + "";
				TempSettlementRequest.CBM = RS["Value7"] + "";
				TempSettlementRequest.RequestPk = RS["Value9"] + "";
				TempSettlementRequest.BranchPk_Own = RS["ValueInt0"] + "";
				TempSettlementRequest.ArrEach = new List<SettlementEach>();
				ReturnValue.ArrRequest.Add(TempSettlementRequest);
			}
			RS.Close();

			DB.SqlCmd.CommandText = @"
SELECT 
	[DocumentBodyPk],[DocumentPk],[Category]
	,[Value0],[Value1],[Value2],[Value3],[Value4],[Value5],[Value6],[Value7],[Value8],[Value9]
	,[ValueInt0]
    ,[ValueDecimal0]
FROM [INTL2010].[dbo].[DocumentBody] 
  WHERE DocumentPk=" + DocumentPk + " and Value8 not in ('RequestForm', 'TransportBBCharge') ";
			RS = DB.SqlCmd.ExecuteReader();

			while (RS.Read()) {
				TempSettlementEach = new SettlementEach();
				TempSettlementEach.DocumentPk = DocumentPk;
				TempSettlementEach.DocumentBodyPk = RS["DocumentBodyPk"] + "";
				TempSettlementEach.Description = RS["Value0"] + "";
				TempSettlementEach.MonetaryUnitCL = RS["Value1"] + "";
				TempSettlementEach.MonetaryUnit = RS["Value2"] + "";
				TempSettlementEach.Price = decimal.Parse(RS["Value3"] + "");
				TempSettlementEach.Exchanged_MonetaryUnitCL = RS["Value4"] + "";
				TempSettlementEach.Exchanged_MonetaryUnit = RS["Value5"] + "";
				TempSettlementEach.Type = RS["Value8"] + "";
				TempSettlementEach.TypePk = RS["Value9"] + "";
				TempSettlementEach.BranchPk_Own = RS["ValueInt0"] + "";
				TempSettlementEach.Exchanged_Price = decimal.Parse(RS["ValueDecimal0"] + "");

				for (var i = 0; i < ReturnValue.ArrRequest.Count; i++) {
					if (ReturnValue.ArrRequest[i].RequestPk == TempSettlementEach.TypePk) {
						ReturnValue.ArrRequest[i].ArrEach.Add(TempSettlementEach);
						break;
					}
				}
			}
			RS.Close();

			DB.DBCon.Close();
			return ReturnValue;
		}
	}

	public struct SettlementContainer
	{
		public string DocumentPk;
		public string TBBHPk;
		public string Status;
		public string TransportWayCL;
		public string ContainerNo;
		public string BLNo;
		public string FromRegion;
		public string ToRegion;
		public string FromDate;
		public string ToDate;
		public string ExchangeRateDate;
		public string LastWarehouse;
		public string BranchPk_Own;
		public string MonetaryYW;
		public decimal AmountYW;
		public List<SettlementEach> ArrEach;
		public List<SettlementRequest> ArrRequest;
	}
	public struct SettlementRequest
	{
		public string DocumentBodyPk;
		public string DocumentPk;
		public string RequestPk;
		public string FromDate;
		public string ToDate;
		public string FromRegion;
		public string ToRegion;
		public string TransportDescription;
		public string ConsigneeCode;
		public string CT;
		public string PackingUnit;
		public string Kg;
		public string CBM;
		public string BranchPk_Own;
		public List<SettlementEach> ArrEach;
	}
	public struct SettlementEach
	{
		public string DocumentBodyPk;
		public string DocumentPk;

		public string Type;
		public string TypePk;

		public string Description;
		public string MonetaryUnitCL;
		public string MonetaryUnit;
		public decimal Price;
		public string Exchanged_MonetaryUnitCL;
		public string Exchanged_MonetaryUnit;
		public decimal Exchanged_Price;
		public string BranchPk_Own;
	}
}
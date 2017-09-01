using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using Components;
using System.Data;

namespace Components
{
	public class Ready
	{
		private DBConn DB_Ready;
		private DBConn DB_Intl2000;
		private DBConn DB_Yuhan;
		public Ready() {
			DB_Intl2000 = new DBConn();
			DB_Ready = new DBConn("ReadyKorea");
			DB_Yuhan = new DBConn("Yuhan");
		}
		public string Check_ClearanceStatus_FromReadyKorea() {
			DB_Intl2000.SqlCmd.CommandText = @"
SELECT 
	CD.BLNo, CD.[CommercialDocumentHeadPk], R.RequestFormPk, R.DocumentStepCL 
FROM 
	[INTL2010].[dbo].[OurBranchStorageOut] AS OBS 
	left join RequestForm AS R ON OBS.RequestFormPk=R.RequestFormPk 
	left join CommerdialConnectionWithRequest AS CCWR ON CCWR.RequestFormPk=OBS.RequestFormPk
	left join CommercialDocument AS CD ON CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk 
WHERE 
	OBS.BoxCount>0 and  R.DocumentStepCL in (5, 6, 7, 8, 9) 
	and CD.BLNo is not null 
	and CD.BLNo<>'' 
	and R.DepartureDate>'2015' ; 
";
			DB_Intl2000.DBCon.Open();
			SqlDataReader RS = DB_Intl2000.SqlCmd.ExecuteReader();
			List<string> BLNo = new List<string>();
			List<string> CommercialDocumentHeadPk = new List<string>();
			List<string> RequestFormPk = new List<string>();
			List<string> DocumentStepCL = new List<string>();

			while (RS.Read()) {
				BLNo.Add(RS["BLNo"] + "");
				CommercialDocumentHeadPk.Add(RS["CommercialDocumentHeadPk"] + "");
				RequestFormPk.Add(RS["RequestFormPk"] + "");
				DocumentStepCL.Add(RS["DocumentStepCL"] + "");
			}
			RS.Close();
			DB_Intl2000.DBCon.Close();
			string SumBLNo = string.Join("','", BLNo.ToArray());
			if (SumBLNo != "") {
				DB_Ready.SqlCmd.CommandText = @"
					SELECT BlNo, [Rece] 
					FROM [edicus].[dbo].[CUSDEC929A1] 
					WHERE BlNo in ('" + SumBLNo + @"');";

				DB_Ready.DBCon.Open();
				RS = DB_Ready.SqlCmd.ExecuteReader();

				List<string[]> Updated = new List<string[]>();

				while (RS.Read()) {
					for (var i = 0; i < BLNo.Count; i++) {
						if (BLNo[i] == RS["BlNo"] + "") {
							string DocumentStepCL_Ready = "";
							string Rece = RS["Rece"] + "";
							switch (Rece) {
								case "미결":
									DocumentStepCL_Ready = "7";
									break;
								case "전제":
									DocumentStepCL_Ready = "8";
									break;
								case "서보":
									DocumentStepCL_Ready = "9";
									break;
								case "검사":
									DocumentStepCL_Ready = "9";
									break;
								case "담변":
									DocumentStepCL_Ready = "9";
									break;
								case "수리":
									DocumentStepCL_Ready = "13";
									break;
								case "대기":
									DocumentStepCL_Ready = "13";
									break;
							}

							if (DocumentStepCL[i] != DocumentStepCL_Ready && DocumentStepCL_Ready != "") {
								Updated.Add(new string[] { CommercialDocumentHeadPk[i], DocumentStepCL_Ready });
							}

							BLNo.RemoveAt(i);
							CommercialDocumentHeadPk.RemoveAt(i);
							RequestFormPk.RemoveAt(i);
							DocumentStepCL.RemoveAt(i);
							break;
						}
					}
				}
				RS.Close();
				DB_Ready.DBCon.Close();

				DBConn DB = new DBConn();
				DB.DBCon.Open();

				StringBuilder Query = new StringBuilder();
				for (var i = 0; i < Updated.Count; i++) {
					string CommercialDocumentPk = Updated[i][0];
					string ToValue = Updated[i][1];
					string AccountID = "Program";
					if (ToValue == "") {
						continue;
					}


					DB.SqlCmd.CommandText = "SELECT [RequestFormPk] FROM CommerdialConnectionWithRequest WHERE [CommercialDocumentPk]=" + CommercialDocumentPk + ";";
					RS = DB.SqlCmd.ExecuteReader();

					while (RS.Read()) {
						if (ToValue == "6") {
							Query.Append(new GetQuery().AddRequestHistory(RS["RequestFormPk"] + "", ToValue, AccountID, "") +
												"	UPDATE RequestForm SET StepCL=63, [DocumentStepCL] = " + ToValue + " WHERE RequestFormPk=" + RS["RequestFormPk"] + ";");
						} else if (ToValue == "2") {
							Query.Append(new GetQuery().AddRequestHistory(RS["RequestFormPk"] + "", ToValue, AccountID, "") +
												"	UPDATE RequestForm SET [DocumentStepCL] = " + ToValue + " , GubunCL=null WHERE RequestFormPk=" + RS["RequestFormPk"] + ";");
						} else {
							Query.Append(new GetQuery().AddRequestHistory(RS["RequestFormPk"] + "", ToValue, AccountID, "") +
												"	UPDATE RequestForm SET [DocumentStepCL] = " + ToValue + " WHERE RequestFormPk=" + RS["RequestFormPk"] + ";");
						}
					}
					RS.Dispose();

					switch (ToValue) {
						case "8":
							Query.Append("INSERT INTO Highlighter (GubunCL, GubunPk, Color) VALUES (1, " + CommercialDocumentPk + ", 0);");
							break;

						case "9":
							Query.Append("INSERT INTO Highlighter (GubunCL, GubunPk, Color) VALUES (1, " + CommercialDocumentPk + ", 1);");
							break;

						case "13":
							Query.Append("DELETE FROM Highlighter WHERE [GubunPk]=" + CommercialDocumentPk + " and [GubunCL] in (0, 1);");
							break;

						case "14":
							Query.Append("DELETE FROM Highlighter WHERE [GubunPk]=" + CommercialDocumentPk + " and [GubunCL] in (0, 1);");
							break;

						case "15":
							Query.Append("DELETE FROM Highlighter WHERE [GubunPk]=" + CommercialDocumentPk + " and [GubunCL] in (0, 1);");
							break;

						default:
							break;
					}
				}
				if (Query.ToString() != "") {
					DB.SqlCmd.CommandText = Query + "";
					DB.SqlCmd.ExecuteNonQuery();
				}
				DB.DBCon.Close();
			}
			return "1";
		}

		public string SetClearance_Ready_SS(string HouseBLNo, string AccountID) {
			if (CheckIsAleadyBL(HouseBLNo)) {
				return "0";
			}
			if (CheckIsClearanceDate(HouseBLNo)) {
				return "2";
			}
			Clearance clearance = new Clearance(HouseBLNo);
			clearance.MOD_ID = AccountID + "";
			clearance.REG_ID = AccountID + "";

			string CompanyInDocument_Shipper, CompanyInDocument_Consignee;
			string RequestPk = LoadIntl2000(HouseBLNo, ref clearance, out CompanyInDocument_Shipper, out CompanyInDocument_Consignee);
			if (RequestPk == "-1") {
				return "3";
			}
			Shipper Shipper = new Shipper("");
			Consignee Consignee = new Consignee("");
			LoadCompany(CompanyInDocument_Shipper, CompanyInDocument_Consignee, ref Shipper, ref Consignee);


			clearance.Sup_Firm = Shipper.Firm;
			clearance.Sup_Mark = Shipper.Mark;
			clearance.Sup_St = Shipper.State_Word;
			clearance.Sup_St_Sht = Shipper.State;

			clearance.Imp_Code = Consignee.Co;
			clearance.Imp_Firm = Consignee.Co_Name;
			clearance.Imp_TgNo = Consignee.TgNo;

			if (true) {         //// 전항차 체크
				clearance.Nab_Code = Consignee.Co;
				if (Consignee.Post_No.Length > 3) {
					clearance.Nab_Pa_Mark = Consignee.Post_No.Substring(0, 3);
				} else {
					clearance.Nab_Pa_Mark = "";
				}
				clearance.Nab_Firm = Consignee.Co_Name;
				clearance.Nab_TgNo = Consignee.TgNo;
				clearance.Nab_Name = Consignee.Boss_Name;
				clearance.Nab_SdNo = Consignee.SdNo;
				clearance.Nab_TelNo = Consignee.Ref_Tel;

				clearance.Nab_Addr1 = Consignee.Addr1;
				clearance.Nab_Addr2 = Consignee.Addr2;

				clearance.Nab_EMailID = Consignee.Email;
				clearance.Nab_EMailDoMain = Consignee.EmailDomain;
			}

			//LoadYuhan(HouseBLNo, out clearance.Mrn, out clearance.Msn, out clearance.Hsn, out clearance.Arr_Mark, out clearance.Ship, out clearance.Mas_BlNo, out clearance.Arr_Name);


			DB_Ready.SqlCmd.CommandText = @"
SELECT [Pa_Code]
      ,[Dv_Pa]
      ,[Sec_Mark]
      ,[Cus_Mark]
  FROM [edicus].[dbo].[CDPLACE]
  WHERE [Pa_Code]='" + clearance.Chk_Pa_Mark + "';";
			DB_Ready.DBCon.Open();
			SqlDataReader RS = DB_Ready.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				clearance.Chk_Pa_Name = RS["Dv_Pa"] + "";
				clearance.Sec = RS["Sec_Mark"] + "";//수입과
				clearance.Cus = RS["Cus_Mark"] + "";//Chk_Pa="",          창고번호????????
			}
			RS.Dispose();
			DB_Ready.DBCon.Close();

			List<Item> Item_Intl2000 = LoadItem_Intl2000(HouseBLNo);
			Convert_Item_Intl2000ToReadyItem(Item_Intl2000, ref clearance);
			clearance.Rpt_No = Make_RptNo();
			SetReady(clearance);

			string ReturnValue = Shipper.SN == "" ? "0" : "1";

			if (Consignee.SN == "") {
				ReturnValue += "0";
			} else {
				ReturnValue += "1";
			}

			return ReturnValue;
		}
		public Decimal GetExchangeRated(string From, string To, decimal Value, out string ExchageRate, string SetDateTime) {
			Decimal result;
			//string date = SetDateTime == "" ? String.Empty : " and DateSpan<='" + SetDateTime + "'";
			string QWhereDate = "";
			if (SetDateTime != "") {
				DateTime A = new DateTime(Int32.Parse(SetDateTime.Substring(0, 4)), Int32.Parse(SetDateTime.Substring(4, 2)), Int32.Parse(SetDateTime.Substring(6, 2)));
				string Week = A.DayOfWeek.ToString();
				if (Week == "Sunday") {
					QWhereDate = " and DateSpan in ('" + SetDateTime + "', '" + A.AddDays(-1).ToString("yyyyMMdd") + "' , '" + A.AddDays(-2).ToString("yyyyMMdd") + "')";
				} else if (Week == "Saturday") {
					QWhereDate = " and DateSpan in ('" + SetDateTime + "', '" + A.AddDays(-1).ToString("yyyyMMdd") + "')";
				} else {
					QWhereDate = " and DateSpan='" + SetDateTime + "'";
				}
			}

			DB_Intl2000 = new DBConn();
			DB_Intl2000.SqlCmd.CommandText = "	SELECT TOP 1 ExchangeRatePk, ExchangeRateStandard, ExchangeRate, MonetaryUnitFrom, MonetaryUnitTo, DateSpan " +
														"	FROM ExchangeRateHistory " +
														"	WHERE ETCSettingPk=1 and MonetaryUnitFrom =" + From + " and [MonetaryUnitTo] =" + To + " " + QWhereDate +
														"	ORDER BY DateSpan DESC";
			DB_Intl2000.DBCon.Open();
			SqlDataReader RS = DB_Intl2000.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				ExchageRate = RS["MonetaryUnitFrom"] + "!" + RS["MonetaryUnitTo"] + "!" + RS["DateSpan"] + "!" + RS["ExchangeRate"] + "@@";
				result = Value * Decimal.Parse(RS[2] + "") / decimal.Parse(RS[1] + "");
			} else {
				result = -1;
				ExchageRate = "";
			}
			RS.Dispose();
			DB_Intl2000.DBCon.Close();
			return result;
		}
		public static String GetQuantityUnit(string QuantityUnitCL) {
			switch (QuantityUnitCL) {
				case "40":
					return "PC";   //PC
				case "41":
					return "PR";
				case "42":
					return "SET";
				case "43":
					return "SF";
				case "44":
					return "YD";
				case "45":
					return "M";
				case "46":
					return "KG";
				case "47":
					return "DZ";
				case "48":
					return "L";
				case "49":
					return "CT";
				case "50":
					return "SQ";
				case "51":
					return "M2";
				case "52":
					return "RO";
				default:
					return "";
			}
		}
		public static String GetPackingUnit(string PackingUnitCL) {
			switch (PackingUnitCL) {
				case "15":
					return "CT";
				case "16":
					return "RO";
				case "17":
					return "PA";
				default:
					return "GT";
			}
		}

		private List<Item> LoadItem_Intl2000(string HouseBLNo) {
			DB_Intl2000.SqlCmd.CommandText = @"
				SELECT 
					RI.[Quantity], RI.[QuantityUnit], RI.[PackedCount], RI.[PackingUnit], RI.[GrossWeight]
					,RI.[NetWeight],RI.[Volume],RI.[UnitPrice],RI.[Amount],RI.[RAN]
					,RI.[Label],CIK.[HSCode],CIK.[Description],CIK.[RANTradingDescription],CIK.[RANDescription]
					,CIK.[Material],CIK.[TarriffRate],CIK.[AdditionalTaxRate],CIK.[FCN1] ,CIK.[E1] ,CIK.[C] 
				FROM [INTL2010].[dbo].[RequestFormItems] RI 
					left join ClearanceItemCodeKOR CIK on RI.ItemCode=CIK.ItemCode 
					WHERE RI.RequestFormPk in (
						SELECT CCWR.RequestFormPk 
						FROM [INTL2010].[dbo].[CommercialDocument] AS CD 
							left join [INTL2010].[dbo].[CommerdialConnectionWithRequest] AS CCWR ON CD.CommercialDocumentHeadPk=CCWR.CommercialDocumentPk 
						WHERE BLNo='" + HouseBLNo + @"'
					)
				order by [RAN], RI.RequestFormItemsPk;";
			DB_Intl2000.DBCon.Open();
			SqlDataReader RS = DB_Intl2000.SqlCmd.ExecuteReader();

			List<Item> Intl2000_Items = new List<Item>();

			int RAN = 0;
			string TempRAN = "";
			while (RS.Read()) {
				if (RS["RAN"] + "" != TempRAN) {
					RAN++;
					TempRAN = RS["RAN"] + "";
				}
				Item tempItem = new Item("");
				tempItem.Ran_No = RAN.ToString("000");

				tempItem.Qty = decimal.Parse(RS["Quantity"] + "" == "" ? "0" : RS["Quantity"] + "").ToString("#############0.0000");//수량 numeric(14; 4)
				tempItem.Ut = GetQuantityUnit(RS["QuantityUnit"] + "");//수량단위
				tempItem.Upi = decimal.Parse(RS["UnitPrice"] + "" == "" ? "0" : RS["UnitPrice"] + "").ToString("#################0.000000");//단가 numeric(18; 6)
				tempItem.Amt = decimal.Parse(RS["Amount"] + "" == "" ? "0" : RS["Amount"] + "").ToString("###############0.0000");//금액 numeric(16; 4)
				tempItem.TempSun_Wt = RS["NetWeight"] + "";

				tempItem.TempHS = RS["HSCode"] + "";
				tempItem.TempFCN1Check = RS["FCN1"] + "";
				tempItem.TempE1Check = RS["E1"] + "";
				tempItem.TempCCheck = RS["C"] + "";
				tempItem.TempPackedCount = RS["PackedCount"] + "";
				tempItem.TempGs_Rate = RS["TarriffRate"] + "";
				tempItem.TempAmt = decimal.Parse(RS["Amount"] + "" == "" ? "0" : RS["Amount"] + "").ToString("###############0.0000");
				tempItem.TempStd_GName1 = RS["RANDescription"].ToString();
				tempItem.TempExc_GName1 = RS["RANTradingDescription"] + "" == "" ? RS["Description"] + "" : RS["RANTradingDescription"] + "";

				tempItem.TempPackedUnit = RS["PackingUnit"] + "";
				tempItem.Compoent1 = RS["Material"] + "";
				//란번호                

				if (RS["Description"].ToString().Trim().Length > 30) {
					tempItem.IMP_GName1 = RS["Description"].ToString().Substring(0, 30).Trim();
					tempItem.IMP_GName2 = RS["Description"].ToString().Substring(30).Trim();
					tempItem.IMP_GName3 = GetStringByteCollection(RS["Label"].ToString().Trim(), 30);
				} else {
					tempItem.IMP_GName1 = RS["Description"].ToString().Trim();
					tempItem.IMP_GName2 = GetStringByteCollection(RS["Label"].ToString().Trim(), 30);
					tempItem.IMP_GName3 = "";
				}

				tempItem.TempStd_GName1 = tempItem.TempStd_GName1.ToUpper().Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("^", "").Replace("&", "").Replace("*", "").Replace("~", "");
				tempItem.TempExc_GName1 = tempItem.TempExc_GName1.ToUpper().Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("^", "").Replace("&", "").Replace("*", "").Replace("~", "");
				tempItem.Compoent1 = tempItem.Compoent1.ToUpper().Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("^", "").Replace("&", "").Replace("*", "").Replace("~", "");
				tempItem.IMP_GName1 = tempItem.IMP_GName1.ToUpper().Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("^", "").Replace("&", "").Replace("*", "").Replace("~", "");
				tempItem.IMP_GName2 = tempItem.IMP_GName2.ToUpper().Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("^", "").Replace("&", "").Replace("*", "").Replace("~", "");
				tempItem.IMP_GName3 = tempItem.IMP_GName3.ToUpper().Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("^", "").Replace("&", "").Replace("*", "").Replace("~", "");

				Intl2000_Items.Add(tempItem);
			}
			RS.Dispose();
			DB_Intl2000.DBCon.Close();

			return Intl2000_Items;
		}

		public static string GetStringByteCollection(string StringValue, int ByteCnt) {
			string lsTmpStringValue = string.Empty;
			string lsReturnValue = string.Empty;
			int liByteCnt = 0;
			try {
				for (int liStartPoint = 0; liStartPoint < StringValue.Length; liStartPoint++) {
					lsTmpStringValue = lsTmpStringValue + StringValue.Substring(liStartPoint, 1).Replace("'", "''");
					liByteCnt = Encoding.Default.GetByteCount(lsTmpStringValue);

					if (liByteCnt > ByteCnt) {
						lsReturnValue = lsTmpStringValue.Substring(0, lsTmpStringValue.Length - 1);
						break;
					} else {
						lsReturnValue = lsTmpStringValue.ToString();
					}
				}
				return lsReturnValue.ToString();
			} catch {
				return string.Empty;
			}
		}




		private string Convert_Item_Intl2000ToReadyItem(List<Item> Intl2000_Items, ref Clearance clearance) {
			clearance.Rans = new List<RAN>();
			Decimal[] TempTotalPackingCount = new Decimal[3] { 0, 0, 0 };
			List<RAN> RAN = new List<RAN>();

			int CursorRAN = 0;
			while (true) {
				RAN temp = new RAN("");
				temp.Items = new List<Item>();
				int count_Item = 0;
				CursorRAN++;

				bool CheckAtleastOne = false;
				for (var i = 0; i < Intl2000_Items.Count; i++) {
					Item each = Intl2000_Items[i];
					if (Int32.Parse(each.Ran_No) == CursorRAN) {
						CheckAtleastOne = true;
						temp.Tax_Usg_Usd += decimal.Parse(decimal.Parse(each.TempAmt).ToString("##############0.00"));       //numeric(15, 2)
						temp.Sun_Wt += Math.Round(decimal.Parse(each.TempSun_Wt == "" ? "0" : each.TempSun_Wt), 1);//순중량)       numeric(15, 1)
						temp.Sun_Ut = each.TempSun_Ut;// 순중량 단위
													  //temp.Qty = "0";
													  //temp.Qty_Ut = "";
						temp.Qty += Math.Round(decimal.Parse(each.Qty == "" ? "0" : each.Qty), 0);//수량 소숫점 첫번째 반올림 numeric(10, 0)
						temp.Qty_Ut = each.Ut;                 // 수량 단위
						temp.Hs = each.TempHS.Replace(".", "").Replace("-", "");// HSCOde
						temp.Ref_Wt += decimal.Parse(decimal.Parse(each.Qty == "" ? "0" : each.Qty).ToString("############0.000"));           //환급물량 numeric(13, 3)
						temp.Ref_Ut = each.Ut + "" == "SET" ? "SE" : each.Ut + "";                 // 환급물량 단위
																								   // 품명 1,품명2
						if (each.TempStd_GName1 == "") {
							temp.Std_GName1 = "";
							temp.Std_GName2 = "";
						} else if (each.TempStd_GName1.Length > 25) {
							temp.Std_GName1 = each.TempStd_GName1.ToString().Substring(0, 25).Trim().ToUpper();
							temp.Std_GName2 = each.TempStd_GName1.ToString().Substring(25).Trim().ToUpper();
						} else {
							temp.Std_GName1 = each.TempStd_GName1.ToString().Trim().ToUpper();
							temp.Std_GName2 = "";
						}
						//temp.Exc_GName1 = each.TempExc_GName1; //거래품명
						//temp.Exc_GName2 = "";  //거래품명2
						if (each.TempExc_GName1 == "") {
							temp.Exc_GName1 = "";
							temp.Exc_GName2 = "";
						} else if (each.TempExc_GName1.Length > 25) {
							temp.Exc_GName1 = each.TempExc_GName1.ToString().Substring(0, 25).Trim().ToUpper();
							temp.Exc_GName2 = each.TempExc_GName1.ToString().Substring(25).Trim().ToUpper();
						} else {
							temp.Exc_GName1 = each.TempExc_GName1.ToString().Trim().ToUpper();
							temp.Exc_GName2 = "";
						}

						temp.Gs_Rate = decimal.Parse(each.TempGs_Rate).ToString("#####0.00");    //관세율numeric(6, 2)
						temp.Send_Rate = decimal.Parse(each.TempGs_Rate).ToString("#########0.00");    //관세율numeric(10, 2)
						temp.Ran_No = (each.Ran_No).ToString();

						if (each.TempFCN1Check == "1") {
							temp.Tax_Ki_Divi = "중가";
							temp.Gs_Divi = "FCN1";
						} else if (each.TempE1Check == "1") {
							temp.Tax_Ki_Divi = "아가";
							temp.Gs_Divi = "E1";
						} else if (each.TempCCheck == "1") {
							temp.Tax_Ki_Divi = "가가";
							temp.Gs_Divi = "C";
						} else {
							temp.Tax_Ki_Divi = "기가";
							temp.Gs_Divi = "A";
						}
						//덤가 I

						temp.REG_ID = clearance.REG_ID;
						temp.MOD_ID = clearance.REG_ID;
						temp.Ori_St_Mark1 = clearance.Sup_St;  //원산지 국가코드 
						temp.Ori_St_Sht = clearance.St_Sht; //원산지 국가코드  약어
						temp.Ori_St_Mark2 = "G"; //원산지 표시유무
						temp.Ori_St_Mark3 = "B";//?원산지 방법

						count_Item++;
						each.Sil = count_Item.ToString("00");

						temp.Items.Add(each);
						if (each.TempPackedCount != "") {
							TempTotalPackingCount[(Int32.Parse(each.TempPackedUnit) - 15)] += decimal.Parse(each.TempPackedCount);
						}

						clearance.Con_Tot_Amt += decimal.Parse(each.TempAmt);//총결제금액numeric(15, 2)
						clearance.Con_Amt += decimal.Parse(each.TempAmt);//결제금액numeric(15, 2)
					}
				}

				if (temp.Items.Count > 0) {
					clearance.Rans.Add(temp);
					continue;
				}

				if (!CheckAtleastOne) {
					break;
				}
			}

			for (int i = 0; i < TempTotalPackingCount.Length; i++) {
				if (TempTotalPackingCount[i] != 0) {
					clearance.Tot_Pack_Cnt += TempTotalPackingCount[i] + "";
					clearance.Tot_Pack_Ut += Common.GetPackingUnit((i + 15) + "");
				}
			}
			clearance.Tot_Pack_Cnt = decimal.Parse(clearance.Tot_Pack_Cnt).ToString("#######0");
			if (clearance.Tot_Pack_Ut != "CT") {
				clearance.Tot_Pack_Ut = "GT";
			}
			clearance.Tot_Ran_Cnt = (clearance.Rans.Count).ToString();

			if (clearance.Con_Cur == "USD" && clearance.Con_Tot_Amt > 10000) {
				clearance.Amt_Rpt_Yn = "Y";//   가격신고서 
			} else if (clearance.Con_Cur == "CNY" && clearance.Con_Tot_Amt > 60000) {
				clearance.Amt_Rpt_Yn = "Y";
			}

			return "1";
		}
		private bool CheckIsAleadyBL(string HouseBLNo) {
			DB_Ready = new DBConn("ReadyKorea");
			string Count = "";
			// 중복체크
			DB_Ready.SqlCmd.CommandText = @"
select COUNT(*) count from CUSDEC929A1
where BlNo='" + HouseBLNo + "'";
			DB_Ready.DBCon.Open();
			SqlDataReader RS = DB_Ready.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				Count = RS["count"] + "";
			}
			RS.Dispose();
			DB_Ready.DBCon.Close();
			if (Count == "0") {
				return false;
			} else {
				return true;
			}
		}
		private bool CheckIsClearanceDate(string HouseBLNo) {
			DB_Intl2000 = new DBConn();
			string Check = "";
			// 중복체크
			DB_Intl2000.SqlCmd.CommandText = @"
select ClearanceDate from CommercialDocument
where BLNo='" + HouseBLNo + "'";
			DB_Intl2000.DBCon.Open();
			SqlDataReader RS = DB_Intl2000.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				Check = RS["ClearanceDate"] + "";
			}
			RS.Dispose();
			DB_Intl2000.DBCon.Close();
			if (Check != "") {
				return false;
			} else {
				return true;
			}
		}
		private bool LoadCompany(string CompanyInDocument_Shipper, string CompanyInDocument_Consignee, ref Shipper Shipper, ref Consignee Consignee) {
			DB_Intl2000.SqlCmd.CommandText = @"
(
	SELECT 
		'S',  [CompanyInDocumentPk],[Title]
		,[Name_KOR],[Address_KOR],[Name],[Address],[DefaultConnection],[CompanyNo],[CustomsCode],[PresidentName],[ZipCode1],[ZipCode2] 
	FROM [dbo].[CompanyInDocument] 
	WHERE CompanyInDocumentPk=" + CompanyInDocument_Shipper + @"
) UNION (
	SELECT 
		'C',  [CompanyInDocumentPk],[Title]
		,[Name_KOR],[Address_KOR],[Name],[Address],[DefaultConnection],[CompanyNo],[CustomsCode],[PresidentName],[ZipCode1],[ZipCode2]
	FROM [dbo].[CompanyInDocument] 
	WHERE CompanyInDocumentPk=" + CompanyInDocument_Consignee + @"
); 
";
			DB_Intl2000.DBCon.Open();
			SqlDataReader RS = DB_Intl2000.SqlCmd.ExecuteReader();
			string[] intl_Shipper = new string[] { "" };
			string[] intl_Consignee = new string[] { "" };
			while (RS.Read()) {
				string[] Temp = new string[] { RS["CompanyInDocumentPk"] + "", RS["Title"] + "", RS["Name_KOR"] + "", RS["Address_KOR"] + "", RS["Name"] + "", RS["Address"] + "", RS["DefaultConnection"] + "", RS["CompanyNo"] + "", RS["CustomsCode"] + "", RS["PresidentName"] + "", RS["ZipCode1"] + "", RS["ZipCode2"] + "" };
				if (RS[0].ToString() == "S") {
					intl_Shipper = Temp;
				} else {
					intl_Consignee = Temp;
				}
			}
			RS.Dispose();
			DB_Intl2000.DBCon.Close();

			bool[] UpdateTitle = new bool[] { false, false };
			DB_Ready.DBCon.Open();

			if (intl_Shipper[0] != "" && intl_Shipper[8] == "") {
				DB_Ready.SqlCmd.CommandText = @"
						SELECT [Mark]  
						FROM [edicus].[dbo].[CDFORE] 
						WHERE 
							Firm like '" + (intl_Shipper[4].Length > 20 ? intl_Shipper[4].Substring(0, 20) : intl_Shipper[4]) + @"%' 
							and Addr1 like '" + (intl_Shipper[5].Length > 20 ? intl_Shipper[5].Substring(0, 20) : intl_Shipper[5]) + @"%' 
						GROUP BY Mark;";

				RS = DB_Ready.SqlCmd.ExecuteReader();
				int count = 0;
				string mark = "";
				while (RS.Read()) {
					count++;
					mark = RS[0] + "";
				}
				RS.Dispose();
				if (count == 1) {
					intl_Shipper[8] = mark;
					UpdateTitle[0] = true;
				}
			}

			if (intl_Shipper[8] != "") {
				DB_Ready.SqlCmd.CommandText = @"
Select TOP 1 [SN], [Firm], [Mark], [State_Word], [State], [Addr1], [Addr2], [Addr3]
 From CDFORE
where 1=1  AND  mark ='" + intl_Shipper[8] + @"' 
ORDER BY SN DESC ; ";
				RS = DB_Ready.SqlCmd.ExecuteReader();
				if (RS.Read()) {
					Shipper.SN = RS[0] + "";
					Shipper.Firm = RS[1] + "";
					Shipper.Mark = RS[2] + "";
					Shipper.State_Word = RS[3] + "";
					Shipper.State = RS[4] + "";
					Shipper.Addr1 = RS[5] + "";
					Shipper.Addr2 = RS[6] + "";
					Shipper.Addr3 = RS[7] + "";
				}
				RS.Dispose();
			}




			if (intl_Consignee[0] != "" && intl_Consignee[8] == "") {
				if (intl_Consignee[7] == "" && intl_Consignee[4] != "") {
					string tempCompanyName = intl_Consignee[4].Trim();
					string tempSaupja = "";
					if (tempCompanyName != "") {
						if (tempCompanyName.Substring(tempCompanyName.Length - 1) == ")") {
							int tempStart = tempCompanyName.LastIndexOf("(") + 1;
							tempSaupja = tempCompanyName.Substring(tempStart, tempCompanyName.Length - 1 - tempStart);
							tempSaupja = tempSaupja.Trim();
							string[] arrSaupja = tempSaupja.Split(new string[] { "-" }, StringSplitOptions.None);
							if (arrSaupja.Length == 3) {
								tempSaupja = arrSaupja[0].Trim() + arrSaupja[1].Trim() + arrSaupja[2].Trim();
							}
							if (tempSaupja.Length == 10) {
								tempCompanyName = tempCompanyName.Substring(0, tempCompanyName.LastIndexOf("("));
							} else {
								tempSaupja = "";
							}
						}
					}
					intl_Consignee[4] = tempCompanyName;
					intl_Consignee[7] = tempSaupja;
				}
				if (intl_Consignee[7] != "") {
					DB_Ready.SqlCmd.CommandText = @"
					SELECT [TgNo] 
					FROM [dbo].[CDCUSTOMA1]
					WHERE SdNo='" + intl_Consignee[7].Replace("-", "").Trim() + @"' 
					GROUP BY [TgNo];";

					RS = DB_Ready.SqlCmd.ExecuteReader();
					int count = 0;
					string mark = "";
					while (RS.Read()) {
						count++;
						mark = RS[0] + "";
					}
					RS.Dispose();
					if (count == 1) {
						intl_Consignee[8] = mark;
						UpdateTitle[1] = true;
					}
				}
			}

			if (intl_Consignee[8] != "") {
				DB_Ready.SqlCmd.CommandText = @"
SELECT 
	[SN],[Co],[Co_Name],[Addr1],[Addr2],[Boss_Name],[TgNo],[SdNo],[Post_No],[Mgt_EMail],[Mgt_EMailDoMain] , [Ref_Tel]
FROM [dbo].[CDCUSTOMA1]
WHERE TgNo='" + intl_Consignee[8] + @"' ; ";

				RS = DB_Ready.SqlCmd.ExecuteReader();
				while (RS.Read()) {
					Consignee.SN = RS[0] + "";
					Consignee.Co = RS[1] + "";
					Consignee.Co_Name = RS[2] + "";
					Consignee.Addr1 = RS[3] + "";
					Consignee.Addr2 = RS[4] + "";
					Consignee.Boss_Name = RS[5] + "";
					Consignee.TgNo = RS[6] + "";
					Consignee.SdNo = RS[7] + "";
					Consignee.Post_No = RS["Post_No"] + "";
					Consignee.Email = RS["Mgt_EMail"] + "";
					Consignee.EmailDomain = RS["Mgt_EMailDoMain"] + "";
					Consignee.Ref_Tel = RS["Ref_Tel"] + "";
				}
				RS.Dispose();
			}

			DB_Ready.DBCon.Close();


			StringBuilder query = new StringBuilder();
			if (UpdateTitle[0]) {
				query.Append("UPDATE [dbo].[CompanyInDocument] SET CustomsCode='" + intl_Shipper[8] + "' WHERE [CompanyInDocumentPk]=" + intl_Shipper[0] + ";");
			}
			if (UpdateTitle[1]) {
				query.Append("UPDATE [dbo].[CompanyInDocument] SET CustomsCode='" + intl_Consignee[8] + "' , CompanyNo='" + intl_Consignee[7] + "'  WHERE [CompanyInDocumentPk]=" + intl_Consignee[0] + ";");
			}

			if (query.ToString() != "") {
				DB_Intl2000.DBCon.Open();
				DB_Intl2000.SqlCmd.CommandText = query + "";
				DB_Intl2000.SqlCmd.ExecuteScalar();
				DB_Intl2000.DBCon.Close();
			}


			if (Shipper.SN == "") {
				if (intl_Shipper.Length > 1) {
					Shipper.SN = "";
					Shipper.Firm = intl_Shipper[4];
					Shipper.State_Word = "CN";
					Shipper.State = "PR.CHNA";

					if (intl_Shipper[5].Length < 36) {
						Shipper.Addr1 = intl_Shipper[5];
						Shipper.Addr2 = "";
						Shipper.Addr3 = "";
					} else if (intl_Shipper[5].Length < 71) {
						Shipper.Addr1 = intl_Shipper[5].Substring(0, 35);
						Shipper.Addr2 = intl_Shipper[5].Substring(35);
						Shipper.Addr3 = "";
					} else {
						Shipper.Addr1 = intl_Shipper[5].Substring(0, 35);
						Shipper.Addr2 = intl_Shipper[5].Substring(35, 35);
						Shipper.Addr3 = intl_Shipper[5].Substring(70);
					}
					Shipper.Mark = "";
				}
			}

			if (Consignee.SN == "") {
				Consignee.SN = "";
				Consignee.Co = intl_Consignee[1];
				Consignee.Co_Name = intl_Consignee[2];
				if (intl_Consignee[3].Length < 16) {
					Consignee.Addr1 = intl_Consignee[3];
					Consignee.Addr2 = "";
				} else {
					Consignee.Addr1 = intl_Consignee[3].Substring(0, 15);
					Consignee.Addr2 = intl_Consignee[3].Substring(15);
				}
				Consignee.Boss_Name = intl_Consignee[9];
			}

			return true;
		}
		private string LoadIntl2000(string HouseBLNo, ref Clearance clearance, out string CompanyInDocument_Shipper, out string CompanyInDocument_Consignee) {
			DB_Intl2000.SqlCmd.CommandText = @"
DECLARE @CommercialDocumentPk int; 
DECLARE @RequestPk int; 
DECLARE @CompanyInDocument_Shipper int; 
DECLARE @CompanyInDocument_Consignee int; 
DECLARE @MonetaryUnitCL smallint; 
DECLARE @ExchangeDate varchar(MAX); 
DECLARE @DocumentRequestCL varchar(50);
DECLARE @DepartureDate varchar(8); 

SELECT @CommercialDocumentPk=CommercialDocumentHeadPk 
FROM CommercialDocument 
WHERE BLNo='" + HouseBLNo + @"';

SELECT TOP 1 @RequestPk=[RequestFormPk]
FROM [dbo].[CommerdialConnectionWithRequest] 
WHERE CommercialDocumentPk=@CommercialDocumentPk 
ORDER BY RequestFormPk desc; 

SELECT 
	@CompanyInDocument_Shipper=R.[ShipperClearanceNamePk], 
	@CompanyInDocument_Consignee=R.[ConsigneeClearanceNamePk] 
	,@MonetaryUnitCL=R.[MonetaryUnitCL]
	, @ExchangeDate=R.ExchangeDate 
	, @DocumentRequestCL=R.DocumentRequestCL 
    , @DepartureDate=R.DepartureDate
FROM [dbo].[RequestForm] AS R 
WHERE R.RequestFormPk=@RequestPk; 

SELECT 
	@RequestPk AS RequestPk, @CompanyInDocument_Shipper AS CID_Shipper, @CompanyInDocument_Consignee AS CID_Consignee, @MonetaryUnitCL AS MonetaryUnitCL
	, @ExchangeDate AS ExchangeDate
	, @DocumentRequestCL AS DocumentRequestCL
    , @DepartureDate AS DepartureDate
	, [CommercialDocumentHeadPk],[Shipper],[ShipperAddress],[Consignee],[ConsigneeAddress]
	,[PortOfLoading],[FinalDestination],[Carrier],[SailingOn],[PaymentTerms],[OtherReferences],[FOBorCNF],[VoyageNo],[VoyageCompany]
	,[ContainerNo],[SealNo],[ContainerSize], [ClearanceDate] 
  FROM [dbo].[CommercialDocument] 
  WHERE [CommercialDocumentHeadPk]=@CommercialDocumentPk; 
";

			DB_Intl2000.DBCon.Open();
			SqlDataReader RS = DB_Intl2000.SqlCmd.ExecuteReader();
			string CommercialDocumentPk;
			string RequestPk = "";
			string ClearanceDate = "";
			CompanyInDocument_Shipper = "";
			CompanyInDocument_Consignee = "";

			if (RS.Read()) {
				CommercialDocumentPk = RS["CommercialDocumentHeadPk"] + "";
				RequestPk = RS["RequestPk"] + "";
				ClearanceDate = RS["ClearanceDate"] + "";
				CompanyInDocument_Shipper = RS["CID_Shipper"] + "";
				CompanyInDocument_Consignee = RS["CID_Consignee"] + "";

				if (RS["PaymentTerms"].ToString().Trim() == "T/T" || RS["PaymentTerms"].ToString().Trim() == "T / T") {
					clearance.Con_Ki = "TT";    //결제방법 디폴트 TT
					clearance.Con_Ki_Cot = "단순송금방식";
				} else {
					clearance.Con_Ki = "";  //결제방법 디폴트 TT
					clearance.Con_Ki_Cot = "";
				}

				if (RS["MonetaryUnitCL"] + "" == "18") {
					clearance.Con_Cur = "CNY";
				} else if (RS["MonetaryUnitCL"] + "" == "19") {
					clearance.Con_Cur = "USD";
				} else if (RS["MonetaryUnitCL"] + "" == "21") {
					clearance.Con_Cur = "JPY";
				} else {
					clearance.Con_Cur = "KRW";
				}


				if (RS["FOBorCNF"] + "" != "") {
					string FOBorCNF = RS["FOBorCNF"] + "";
					string[] arrFOBCNF = FOBorCNF.Split(new string[] { "#@!" }, StringSplitOptions.None);

					if (arrFOBCNF[0] == "FOB") {
						clearance.Con_Cod = "FOB";
					} else {
						clearance.Con_Cod = "CFR";
					}

					switch (arrFOBCNF[1]) {
						case "18":
							clearance.Ad_Cur_Ki = "CNY";
							clearance.Sub_Cur_Ki = "CNY";
							break;
						case "19":
							clearance.Ad_Cur_Ki = "USD";
							clearance.Sub_Cur_Ki = "USD";
							break;
						case "21":
							clearance.Ad_Cur_Ki = "JPY";
							clearance.Sub_Cur_Ki = "JPY";
							break;
						default:
							clearance.Ad_Cur_Ki = "KRW";
							clearance.Sub_Cur_Ki = "KRW";

							break;
					}


					decimal tempExchangedAmount = 0;

					if (arrFOBCNF[2] == "") {
						tempExchangedAmount = 0;
					} else {
						if (arrFOBCNF[1] == "20") {
							tempExchangedAmount = decimal.Parse(arrFOBCNF[2]);
						} else {
							string AAAAAAA = "";
							if (RS["ExchangeDate"] + "" != "") {
								string ExchangeDate = RS["ExchangeDate"] + "";
								if (GetExchangeRated(arrFOBCNF[1], "20", decimal.Parse(arrFOBCNF[2]), out AAAAAAA, ExchangeDate) == -1) {
									return "-1";
								}
								tempExchangedAmount = Math.Truncate(GetExchangeRated(arrFOBCNF[1], "20", decimal.Parse(arrFOBCNF[2]), out AAAAAAA, ExchangeDate));
							} else {
								tempExchangedAmount = Math.Truncate(GetExchangeRated(arrFOBCNF[1], "20", decimal.Parse(arrFOBCNF[2]), out AAAAAAA, RS["DepartureDate"].ToString()));
							}
						}
					}
					clearance.Fre_Krw = decimal.Parse(tempExchangedAmount.ToString()).ToString("###########0");         //             "0",// 세액 합계numeric(12, 0)
					clearance.Fre1_Amt = decimal.Parse(tempExchangedAmount.ToString()).ToString("##############0.00");       // "0.00",// 운임 1numeric(15, 2)
				}


				string documentrequestcl = RS["DocumentRequestCL"] + "";
				if (documentrequestcl.IndexOf("31!") > -1) {
					clearance.Ori_St_Prf_Yn = "Y";//원산지 여부
				} else {
					clearance.Ori_St_Prf_Yn = "N";//원산지 여부
				}
			}
			RS.Dispose();

			DB_Intl2000.SqlCmd.CommandText = @"
SELECT ExchangeRate 
FROM ExchangeRateHistory 
WHERE 
	ETCSettingPk=2 and ExchangeRateStandard=1 and MonetaryUnitFrom=19 and MonetaryUnitTo=20
	and left(DateSpan,8)<='" + ClearanceDate + @"' and RIGHT(DateSpan,8)>='" + ClearanceDate + @"' ;";
			if (DB_Intl2000.DBCon.State == ConnectionState.Closed) {
				DB_Intl2000.DBCon.Open();
			}
			string tempStr = DB_Intl2000.SqlCmd.ExecuteScalar() + "";
			clearance.Con_Rate_Usd = decimal.Parse(tempStr).ToString("########0.0000");


			if (clearance.Con_Cur == "USD") {
				clearance.Con_Rate = clearance.Con_Rate_Usd;
			} else if (clearance.Con_Cur == "CNY") {
				DB_Intl2000.SqlCmd.CommandText = @"
					SELECT ExchangeRate 
					FROM ExchangeRateHistory 
					WHERE 
						ETCSettingPk=2 and ExchangeRateStandard=1 and MonetaryUnitFrom=18 and MonetaryUnitTo=20 
						and left(DateSpan,8)<='" + ClearanceDate + @"' and RIGHT(DateSpan,8)>='" + ClearanceDate + @"' ;";
				tempStr = DB_Intl2000.SqlCmd.ExecuteScalar() + "";
				clearance.Con_Rate = decimal.Parse(tempStr).ToString("########0.0000");
			} else if (clearance.Con_Cur == "JPY") {
				DB_Intl2000.SqlCmd.CommandText = @"
					SELECT ExchangeRate 
					FROM ExchangeRateHistory 
					WHERE 
						ETCSettingPk=2 and ExchangeRateStandard=1 and MonetaryUnitFrom=21 and MonetaryUnitTo=20 
						and left(DateSpan,8)<='" + ClearanceDate + @"' and RIGHT(DateSpan,8)>='" + ClearanceDate + @"' ;";
				tempStr = DB_Intl2000.SqlCmd.ExecuteScalar() + "";
				clearance.Con_Rate = decimal.Parse(tempStr).ToString("########0.0000");
			} else {
				clearance.Con_Rate = "1.0000";
			}


			DB_Intl2000.SqlCmd.CommandText = @"
DECLARE @TransportCL int; 
DECLARE @RequestPk int; 
DECLARE @TBBPk int; 
DECLARE @StorageCodePk int; 
DECLARE @StorageCode nvarchar(50); 


SET @RequestPk=" + RequestPk + @"; 

SELECT @TBBPk =[TRANSPORT_HEAD_PK] , @StorageCodePk=[WAREHOUSE_PK]
FROM [dbo].[STORAGE]
WHERE [REQUEST_PK]=@RequestPk; 
  
 if (ISNULL(@TBBPk, 0 )=0) 
BEGIN
	SELECT TOP 1 @TBBPk=[TransportBetweenBranchPk]
  FROM [dbo].[TransportBBHistory]
  WHERE RequestFormPk=@RequestPk 
  ORDER BY [TransportBetweenBranchPk]
END
SELECT @TransportCL=[TRANSPORT_WAY]
  FROM [dbo].[TRANSPORT_HEAD]
WHERE [TRANSPORT_PK]=@TBBPk; 


if (ISNULL(@StorageCodePk, 0 )=0) 
BEGIN 
	SELECT @StorageCodePk=[StorageCode] 
	FROM [dbo].[OurBranchStorageOut] 
	WHERE [RequestFormPk]=@RequestPk;  
END
SELECT @StorageCode=StorageCode FROM OurBranchStorageCode
WHERE OurBranchStoragePk=@StorageCodePk;

SELECT @TransportCL AS TransportCL, @StorageCode AS StorageCode; ";
			RS = DB_Intl2000.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				if (RS["TransportCL"] + "" == "Ship") {
					clearance.Tra_Cta = "FC";
				} else {
					clearance.Tra_Cta = "LC";
				}
				clearance.Chk_Pa_Mark = RS["StorageCode"] + "";//창고코드
			}
			RS.Dispose();
			DB_Intl2000.DBCon.Close();
			return RequestPk;
		}
		public string Make_RptNo() {
			string RptNo_Left = "11829" + DateTime.Today.ToString("yy") + DateTime.Today.ToString("MM");
			DB_Ready.SqlCmd.CommandText = @"
SELECT TOP 1 Rpt_No FROM CUSDEC929A1 
WHERE left(Rpt_No, 9)='" + RptNo_Left + @"'
ORDER BY Rpt_No DESC;";
			DB_Ready.DBCon.Open();
			decimal TempNo = 0;
			SqlDataReader RS = DB_Ready.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				TempNo = decimal.Parse(RS["Rpt_No"].ToString().Substring(9, 4)) + 1;
			} else {
				TempNo = 1;
			}
			DB_Ready.DBCon.Close();
			RS.Dispose();
			return RptNo_Left + TempNo.ToString("0000") + "M";
		}

		private string SetReady(Clearance clearance) {
			StringBuilder Query = new StringBuilder();
			Query.Append(@"
DECLARE @APk numeric(18, 0);
DECLARE @BPk numeric(18, 0);

INSERT INTO [edicus].[dbo].[CUSDEC929A1]
([MEMBER_ID],[DIVISION_ID],[REG_ID],[REG_DATE],[MOD_ID]
,[MOD_DATE],[STATUS],[Rpt_No],[Rpt_Seq],[Rpt_Day]
,[Cus],[Sec],[BlNo],[Bl_Yn],[Mrn]
,[Msn],[Hsn],[Arr_Day],[Inc_Day],[Lev_Form]
,[Rpt_Firm],[Rpt_Name],[Rpt_TelNo],[Rpt_EMailID],[Rpt_EMailDoMain]
,[Imp_Code],[Imp_Firm],[Imp_Mark],[Imp_TgNo],[Imp_Divi]
,[Nab_Code],[Nab_Pa_Mark],[Nab_Addr1],[Nab_Addr2],[Nab_Firm]
,[Nab_Name],[Nab_TgNo],[Nab_SdNo],[Nab_SdNo_Divi],[Sd_Idno_Divi]
,[Nab_TelNo],[Nab_EMailID],[Nab_EMailDoMain],[Off_Firm],[Off_Mark]
,[Sup_Firm],[Sup_St],[Sup_St_Sht],[Sup_Mark],[Tg_Pln_Mark]
,[Tg_Pln_Cot],[Rpt_Divi_Mark],[Rpt_Divi_Cot],[Exc_Divi_Mark],[Exc_Divi_Cot]
,[Imp_Ki_Mark],[Imp_Ki_Cot],[Rpt_Use_Divi_CD],[Rpt_Use_Divi_NM],[Rpt_Use_Day]
,[Ori_St_Prf_Yn],[Amt_Rpt_Yn],[Tot_Wt],[Tot_Ut],[Tot_Pack_Cnt]
,[Tot_Pack_Ut],[Arr_Mark],[Arr_Name],[Tra_Met],[Tra_Cta]
,[Fod_Mark],[Fod_Sht],[Ship],[St_Sht],[St_Code]
,[Mas_BlNo],[Tra_Chf_Mark],[Chk_Pa_Mark],[Chk_Pa],[Chk_Pa_Name]
,[Tot_Ran_Cnt],[Con_Cod],[Con_Cur],[Con_Rate],[Con_Rate_Usd]
,[Con_Tot_Amt],[Con_Ki],[Con_Ki_Cot],[Con_Amt],[Con_Gita_Amt]
,[Tot_Tax_Usd],[Tot_Tax_Krw],[Fre_Krw],[Fre1_Ki],[Fre1_Amt]
,[Fre2_Ki],[Fre2_Amt],[Insu_Krw],[Insu1_Ki],[Insu1_Amt]
,[Insu2_Ki],[Insu2_Amt],[App_Amt],[Insu_Con_Amt],[Ad_Divi]
,[Ad_Cur_Ki],[Ad_Cst_Rate],[Ad_Cur_Ki2],[Ad_Cst_Rate2],[Ad_Amt_Krw]
,[Ad_Cst_Krw],[Sub_Divi],[Sub_Cur_Ki],[Sub_Cst_Rate],[Sub_Cst_Krw]
,[Tot_Gs],[Tot_Ts],[Tot_Hof],[Tot_Gt],[Tot_Ky]
,[Tot_Nt],[Tot_Blank],[Tot_Vat],[Tot_Dly_Tax],[Tot_Add_Tax]
,[Tot_Tax_Sum],[Add_Tax_Gbn],[Nab_No],[Nab_Deli_Day],[Vat_Tax_Ct]
,[Vat_Free_Ct],[Sp_Deli],[Send_Divi],[Res_Form],[Cb_Divi1]
,[Cb_Divi2],[Cb_Divi3],[CB1],[CB2],[CB3]
,[CB4],[CB5],[CB6],[CB7],[CB8]
,[CB9],[Cus1],[Cus2],[Cus3],[Cus4]
,[Cus5],[Cus6],[Cus7],[Cus8],[Cus9]
,[Cus10],[Cus11],[Cus12],[Cus13],[Cus14]
,[Ju_Name],[Ju_Mark],[Rc_Day],[Lis_Day],[File_No]
,[Send],[Rece],[Send_Cnt],[Md_Refe_No],[Usg_Yn]
,[Imp_Req_No],[Imp_Last_Day],[Pay_Rpt_Rsul],[Modi_Rpt_RSul],[Modi_Rpt]
,[Send_Date],[SN_Divi],[BLYnCd],[BLYnTxt],[AmtFixDay]
,[AmtNo_5SM],[FwdMark],[FwdName],[GumSa],[GumSaAmt]
,[ImpTypeCd],[FwdCode],[FwdFirm],[FwdSdNo],[BanChulDay]
,[Bl_Wt],[PDateTime],[UDateTime],[IDateTime],[EDateTime]
,[SDateTime],[GDateTime],[MDateTime],[Re_Exp_Day],[Exp_LisDay]
,[Exp_LisNo],[Ec_Day],[Ref_Dn],[GinGu],[HsUser]
,[Agent],[StnCode],[SaYu_Code],[SaYu],[PrtChk]
,[PrtTime],[ConfirmTime],[ConfirmID],[NabDivi],[BigoCode]
,[Upload],[Bf],[Imp],[ImageSave],[Po_No]
,[GoldAccYn],[Nab_RegNo],[DDP],[Urgent],[Destination]
,[Consignee],[Rece_Result])     
VALUES
('" + clearance.MEMBER_ID + @"','" + clearance.DIVISION_ID + @"','" + clearance.REG_ID + @"'," + clearance.REG_DATE + @",'" + clearance.MOD_ID + @"'
," + clearance.MOD_DATE + @",'" + clearance.STATUS + @"','" + clearance.Rpt_No + @"','" + clearance.Rpt_Seq + @"','" + clearance.Rpt_Day + @"'
,'" + clearance.Cus + @"','" + clearance.Sec + @"','" + clearance.BlNo + @"','" + clearance.Bl_Yn + @"','" + clearance.Mrn + @"'
,'" + clearance.Msn + @"','" + clearance.Hsn + @"','" + clearance.Arr_Day + @"','" + clearance.Inc_Day + @"','" + clearance.Lev_Form + @"'
,'" + clearance.Rpt_Firm + @"','" + clearance.Rpt_Name + @"','" + clearance.Rpt_TelNo + @"','" + clearance.Rpt_EMailID + @"','" + clearance.Rpt_EMailDoMain + @"'
,'" + clearance.Imp_Code + @"','" + clearance.Imp_Firm + @"','" + clearance.Imp_Mark + @"','" + clearance.Imp_TgNo + @"','" + clearance.Imp_Divi + @"'
,'" + clearance.Nab_Code + @"','" + clearance.Nab_Pa_Mark + @"','" + clearance.Nab_Addr1 + @"','" + clearance.Nab_Addr2 + @"','" + clearance.Nab_Firm + @"'
,'" + clearance.Nab_Name + @"','" + clearance.Nab_TgNo + @"','" + clearance.Nab_SdNo + @"','" + clearance.Nab_SdNo_Divi + @"','" + clearance.Sd_Idno_Divi + @"'
,'" + clearance.Nab_TelNo + @"','" + clearance.Nab_EMailID + @"','" + clearance.Nab_EMailDoMain + @"','" + clearance.Off_Firm + @"','" + clearance.Off_Mark + @"'
,'" + clearance.Sup_Firm + @"','" + clearance.Sup_St + @"','" + clearance.Sup_St_Sht + @"','" + clearance.Sup_Mark + @"','" + clearance.Tg_Pln_Mark + @"'
,'" + clearance.Tg_Pln_Cot + @"','" + clearance.Rpt_Divi_Mark + @"','" + clearance.Rpt_Divi_Cot + @"','" + clearance.Exc_Divi_Mark + @"','" + clearance.Exc_Divi_Cot + @"'
,'" + clearance.Imp_Ki_Mark + @"','" + clearance.Imp_Ki_Cot + @"','" + clearance.Rpt_Use_Divi_CD + @"','" + clearance.Rpt_Use_Divi_NM + @"','" + clearance.Rpt_Use_Day + @"'
,'" + clearance.Ori_St_Prf_Yn + @"','" + clearance.Amt_Rpt_Yn + @"'," + clearance.Tot_Wt + @",'" + clearance.Tot_Ut + @"'," + clearance.Tot_Pack_Cnt + @"
,'" + clearance.Tot_Pack_Ut + @"','" + clearance.Arr_Mark + @"','" + clearance.Arr_Name + @"','" + clearance.Tra_Met + @"','" + clearance.Tra_Cta + @"'
,'" + clearance.Fod_Mark + @"','" + clearance.Fod_Sht + @"','" + clearance.Ship + @"','" + clearance.St_Sht + @"','" + clearance.St_Code + @"'
,'" + clearance.Mas_BlNo + @"','" + clearance.Tra_Chf_Mark + @"','" + clearance.Chk_Pa_Mark + @"','" + clearance.Chk_Pa + @"','" + clearance.Chk_Pa_Name + @"'
," + clearance.Tot_Ran_Cnt + @",'" + clearance.Con_Cod + @"','" + clearance.Con_Cur + @"'," + clearance.Con_Rate + @"," + clearance.Con_Rate_Usd + @"
," + clearance.Con_Tot_Amt + @",'" + clearance.Con_Ki + @"','" + clearance.Con_Ki_Cot + @"'," + clearance.Con_Amt + @"," + clearance.Con_Gita_Amt + @"
," + clearance.Tot_Tax_Usd + @"," + clearance.Tot_Tax_Krw + @"," + clearance.Fre_Krw + @",'" + clearance.Fre1_Ki + @"'," + clearance.Fre1_Amt + @"
,'" + clearance.Fre2_Ki + @"'," + clearance.Fre2_Amt + @"," + clearance.Insu_Krw + @",'" + clearance.Insu1_Ki + @"'," + clearance.Insu1_Amt + @"
,'" + clearance.Insu2_Ki + @"'," + clearance.Insu2_Amt + @"," + clearance.App_Amt + @"," + clearance.Insu_Con_Amt + @",'" + clearance.Ad_Divi + @"'
,'" + clearance.Ad_Cur_Ki + @"'," + clearance.Ad_Cst_Rate + @",'" + clearance.Ad_Cur_Ki2 + @"'," + clearance.Ad_Cst_Rate2 + @"," + clearance.Ad_Amt_Krw + @"
," + clearance.Ad_Cst_Krw + @",'" + clearance.Sub_Divi + @"','" + clearance.Sub_Cur_Ki + @"'," + clearance.Sub_Cst_Rate + @"," + clearance.Sub_Cst_Krw + @"
," + clearance.Tot_Gs + @"," + clearance.Tot_Ts + @"," + clearance.Tot_Hof + @"," + clearance.Tot_Gt + @"," + clearance.Tot_Ky + @"
," + clearance.Tot_Nt + @"," + clearance.Tot_Blank + @"," + clearance.Tot_Vat + @"," + clearance.Tot_Dly_Tax + @"," + clearance.Tot_Add_Tax + @"
," + clearance.Tot_Tax_Sum + @",'" + clearance.Add_Tax_Gbn + @"','" + clearance.Nab_No + @"','" + clearance.Nab_Deli_Day + @"'," + clearance.Vat_Tax_Ct + @"
," + clearance.Vat_Free_Ct + @",'" + clearance.Sp_Deli + @"','" + clearance.Send_Divi + @"','" + clearance.Res_Form + @"','" + clearance.Cb_Divi1 + @"'
,'" + clearance.Cb_Divi2 + @"','" + clearance.Cb_Divi3 + @"','" + clearance.CB1 + @"','" + clearance.CB2 + @"','" + clearance.CB3 + @"'
,'" + clearance.CB4 + @"','" + clearance.CB5 + @"','" + clearance.CB6 + @"','" + clearance.CB7 + @"','" + clearance.CB8 + @"'
,'" + clearance.CB9 + @"','" + clearance.Cus1 + @"','" + clearance.Cus2 + @"','" + clearance.Cus3 + @"','" + clearance.Cus4 + @"'
,'" + clearance.Cus5 + @"','" + clearance.Cus6 + @"','" + clearance.Cus7 + @"','" + clearance.Cus8 + @"','" + clearance.Cus9 + @"'
,'" + clearance.Cus10 + @"','" + clearance.Cus11 + @"','" + clearance.Cus12 + @"','" + clearance.Cus13 + @"','" + clearance.Cus14 + @"'
,'" + clearance.Ju_Name + @"','" + clearance.Ju_Mark + @"','" + clearance.Rc_Day + @"' ,'" + clearance.Lis_Day + @"','" + clearance.File_No + @"'
,'" + clearance.Send + @"','" + clearance.Rece + @"'," + clearance.Send_Cnt + @",'" + clearance.Md_Refe_No + @"','" + clearance.Usg_Yn + @"'
,'" + clearance.Imp_Req_No + @"','" + clearance.Imp_Last_Day + @"','" + clearance.Pay_Rpt_Rsul + @"','" + clearance.Modi_Rpt_RSul + @"','" + clearance.Modi_Rpt + @"'
,'" + clearance.Send_Date + @"','" + clearance.SN_Divi + @"','" + clearance.BLYnCd + @"' ,'" + clearance.BLYnTxt + @"','" + clearance.AmtFixDay + @"'
,'" + clearance.AmtNo_5SM + @"','" + clearance.FwdMark + @"','" + clearance.FwdName + @"','" + clearance.GumSa + @"'  ," + clearance.GumSaAmt + @"
,'" + clearance.ImpTypeCd + @"','" + clearance.FwdCode + @"','" + clearance.FwdFirm + @"','" + clearance.FwdSdNo + @"','" + clearance.BanChulDay + @"'
," + clearance.Bl_Wt + @"  ,'" + clearance.PDateTime + @"','" + clearance.UDateTime + @"','" + clearance.IDateTime + @"','" + clearance.EDateTime + @"'  
,'" + clearance.SDateTime + @"'  ,'" + clearance.GDateTime + @"'  ,'" + clearance.MDateTime + @"'  ,'" + clearance.Re_Exp_Day + @"' ,'" + clearance.Exp_LisDay + @"' 
,'" + clearance.Exp_LisNo + @"'  ,'" + clearance.Ec_Day + @"','" + clearance.Ref_Dn + @"','" + clearance.GinGu + @"','" + clearance.HsUser + @"' 
,'" + clearance.Agent + @"','" + clearance.StnCode + @"' ,'" + clearance.SaYu_Code + @"','" + clearance.SaYu + @"','" + clearance.PrtChk + @"'
,'" + clearance.PrtTime + @"','" + clearance.ConfirmTime + @"'    ,'" + clearance.ConfirmID + @"'	,'" + clearance.NabDivi + @"','" + clearance.BigoCode + @"'  
,'" + clearance.Upload + @"','" + clearance.Bf + @"','" + clearance.Imp + @"'  ,'" + clearance.ImageSave + @"'    ,'" + clearance.Po_No + @"'
,'" + clearance.GoldAccYn + @"'    ,'" + clearance.Nab_RegNo + @"'    ,'" + clearance.DDP + @"'  ,'" + clearance.Urgent + @"','" + clearance.Destination + @"' 
,'" + clearance.Consignee + @"','" + clearance.Rece_Result + @"');
SELECT @APk=@@IDENTITY;");

			foreach (RAN each in clearance.Rans) {
				Query.Append(@"		
INSERT INTO [edicus].[dbo].[CUSDEC929B1]
([SNA1],[MEMBER_ID],[DIVISION_ID],[REG_ID],[REG_DATE]
,[MOD_ID],[MOD_DATE],[STATUS],[Rpt_No],[Rpt_Seq]
,[Ran_No],[Hs],[Std_GName1],[Std_GName2],[Exc_GName1]
,[Exc_GName2],[Std_Code],[Model_GName1],[Model_GName2],[Att_Yn]
,[sub_krw],[fre_rmv_divi],[fre_cur],[fre_amt],[fre_krw]
,[insu_cur],[insu_amt],[insu_krw],[ad_cur],[ad_amt]
,[ad_krw],[Ad_Cst],[sub_cur],[sub_amt],[Sub_Cst]
,[Tax_Krw],[Tax_Usd],[Tax_Usg_Krw],[Tax_Usg_Usd],[Sun_Wt]
,[Sun_Ut],[Qty],[Qty_Ut],[Ref_Wt],[Ref_Ut]
,[Cs_Chk_Mark],[Cs_Chk_Cot],[Chk_Met_Mark],[Chk_Met_Cot],[Aft_Chk_Chf1]
,[Aft_Chk_Chf2],[Aft_Chk_Chf3],[Ori_St_Mark1],[Ori_St_Sht],[Ori_St_Mark2]
,[Ori_St_Mark3],[Ori_St_Mark4],[Ori_St_Exempt_CD],[Ori_St_Exempt_NM],[Ori_St_Day]
,[Ori_St_No],[Ori_St_Chf],[Ori_St_iso],[Sp_Tax_Basis],[Tax_Ki_Divi]
,[Gs_Divi],[Gs_Rate],[Upi_Tax],[Gs_Rmv_Rate],[Gs]
,[Gs_Rmv],[Gs_Rmv_Mark],[Gs_Rmv_Divi],[Send_Rate],[Ng_Divi]
,[Ng_Ki_Divi],[Ng_Mark],[Ng_Rate],[Ng_Rmv_Amt],[Ng]
,[Hof_Free_Mark],[Ts_Free_Mark],[Ts_Cet_Sub],[Ts],[Gt]
,[Hof],[Ky_Divi],[Ky_Ki_Divi],[Ky_Rmv_Amt],[Ky_Rate]
,[Ky],[Nt_Divi],[Nt_Ki_Divi],[Nt],[Nt_Rate]
,[Vat_Rate],[Vat_Divi],[Vat_Ki_Divi],[Vat],[Vat_Rmv_Mark]
,[Vat_Rmv],[Vat_Rmv_Rate],[Gs_Cet],[Vat_Tax_Ct],[Vat_Free_ct]
,[Sp_Deli],[Roy_Rate],[Roy_Divi],[Roy_Amt],[Chk_Met_Ch]
,[Dam_Amt],[Tot_Size_Cnt],[Vit_Chk_Cnt],[Tot_Exp_Cnt],[YongDo_No]
,[Ori_St_StnCd],[Ori_St_Stn],[Ori_St_Name],[Ori_St_Mark5],[Ori_St_Mark6]
,[Ori_St_TotQty],[Ori_St_Qty],[Ori_St_TotWt],[Ori_St_Wt],[Ori_St_PartYn]
,[Ori_St_PartDoc],[ItemCd],[ItemCdSend],[ModiRan],[CRan]
,[Tot_C4_Cnt],[Mg_Divi],[Mg_RanNo])   
VALUES
(@APk  ,'" + each.MEMBER_ID + @"','" + each.DIVISION_ID + @"','" + each.REG_ID + @"'," + each.REG_DATE + @"
,'" + each.MOD_ID + @"'," + each.MOD_DATE + @",'" + each.STATUS + @"','" + each.Rpt_No + @"','" + each.Rpt_Seq + @"'
,'" + each.Ran_No + @"','" + each.Hs + @"','" + each.Std_GName1 + @"','" + each.Std_GName2 + @"','" + each.Exc_GName1 + @"'
,'" + each.Exc_GName2 + @"','" + each.Std_Code + @"','" + each.Model_GName1 + @"','" + each.Model_GName2 + @"','" + each.Att_Yn + @"'
," + each.sub_krw + @",'" + each.fre_rmv_divi + @"','" + each.fre_cur + @"'," + each.fre_amt + @"," + each.fre_krw + @"
,'" + each.insu_cur + @"'," + each.insu_amt + @"," + each.insu_krw + @",'" + each.ad_cur + @"'," + each.ad_amt + @"
," + each.ad_krw + @"," + each.Ad_Cst + @",'" + each.sub_cur + @"'," + each.sub_amt + @"," + each.Sub_Cst + @"
," + each.Tax_Krw + @"," + each.Tax_Usd + @"," + each.Tax_Usg_Krw + @"," + each.Tax_Usg_Usd + @"," + each.Sun_Wt + @"
,'" + each.Sun_Ut + @"'," + each.Qty + @",''," + each.Ref_Wt + @",'" + each.Ref_Ut + @"'
,'" + each.Cs_Chk_Mark + @"','" + each.Cs_Chk_Cot + @"','" + each.Chk_Met_Mark + @"','" + each.Chk_Met_Cot + @"','" + each.Aft_Chk_Chf1 + @"'
,'" + each.Aft_Chk_Chf2 + @"','" + each.Aft_Chk_Chf3 + @"','" + each.Ori_St_Mark1 + @"','" + each.Ori_St_Sht + @"','" + each.Ori_St_Mark2 + @"'
,'" + each.Ori_St_Mark3 + @"','" + each.Ori_St_Mark4 + @"','" + each.Ori_St_Exempt_CD + @"','" + each.Ori_St_Exempt_NM + @"','" + each.Ori_St_Day + @"'
,'" + each.Ori_St_No + @"','" + each.Ori_St_Chf + @"','" + each.Ori_St_iso + @"'," + each.Sp_Tax_Basis + @",'" + each.Tax_Ki_Divi + @"'
,'" + each.Gs_Divi + @"'," + each.Gs_Rate + @"," + each.Upi_Tax + @"," + each.Gs_Rmv_Rate + @"," + each.Gs + @"
," + each.Gs_Rmv + @",'" + each.Gs_Rmv_Mark + @"','" + each.Gs_Rmv_Divi + @"'," + each.Send_Rate + @",'" + each.Ng_Divi + @"'
,'" + each.Ng_Ki_Divi + @"','" + each.Ng_Mark + @"'," + each.Ng_Rate + @"," + each.Ng_Rmv_Amt + @"," + each.Ng + @"
,'" + each.Hof_Free_Mark + @"','" + each.Ts_Free_Mark + @"'," + each.Ts_Cet_Sub + @"," + each.Ts + @"," + each.Gt + @"
," + each.Hof + @",'" + each.Ky_Divi + @"','" + each.Ky_Ki_Divi + @"'," + each.Ky_Rmv_Amt + @"," + each.Ky_Rate + @"
," + each.Ky + @",'" + each.Nt_Divi + @"','" + each.Nt_Ki_Divi + @"'," + each.Nt + @"," + each.Nt_Rate + @"
," + each.Vat_Rate + @",'" + each.Vat_Divi + @"','" + each.Vat_Ki_Divi + @"'," + each.Vat + @",'" + each.Vat_Rmv_Mark + @"'
," + each.Vat_Rmv + @"," + each.Vat_Rmv_Rate + @",'" + each.Gs_Cet + @"'," + each.Vat_Tax_Ct + @"," + each.Vat_Free_ct + @"
,'" + each.Sp_Deli + @"'," + each.Roy_Rate + @",'" + each.Roy_Divi + @"'," + each.Roy_Amt + @",'" + each.Chk_Met_Ch + @"'
," + each.Dam_Amt + @"," + each.Tot_Size_Cnt + @",'" + each.Vit_Chk_Cnt + @"'," + each.Tot_Exp_Cnt + @",'" + each.YongDo_No + @"'
,'" + each.Ori_St_StnCd + @"','" + each.Ori_St_Stn + @"','" + each.Ori_St_Name + @"','" + each.Ori_St_Mark5 + @"','" + each.Ori_St_Mark6 + @"'
," + each.Ori_St_TotQty + @"," + each.Ori_St_Qty + @"," + each.Ori_St_TotWt + @"," + each.Ori_St_Wt + @",'" + each.Ori_St_PartYn + @"'
,'" + each.Ori_St_PartDoc + @"','" + each.ItemCd + @"','" + each.ItemCdSend + @"','" + each.ModiRan + @"','" + each.CRan + @"'
," + each.Tot_C4_Cnt + @",'" + each.Mg_Divi + @"','" + each.Mg_RanNo + @"');
SELECT @BPk=@@IDENTITY;");

				foreach (Item eachItem in each.Items) {
					Query.Append(@"
						INSERT INTO [edicus].[dbo].[CUSDEC929C1]
							([SNA1] ,[SNB1] ,[MEMBER_ID] ,[DIVISION_ID] ,[REG_ID] 
							,[REG_DATE] ,[MOD_ID] ,[MOD_DATE] ,[STATUS] ,[Rpt_No] 
							,[Rpt_Seq] ,[Ran_No] ,[Sil] ,[Rg_Code] ,[IMP_GName1] 
							,[IMP_GName2] ,[IMP_GName3] ,[Compoent1] ,[Compoent2] ,[Qty] 
							,[Ut] ,[Upi] ,[Amt] ,[ModiRan]
						) VALUES(
							@APk, @BPk, '" + eachItem.MEMBER_ID + @"','" + eachItem.DIVISION_ID + @"','" + eachItem.REG_ID + @"'
							," + eachItem.REG_DATE + @",'" + eachItem.MOD_ID + @"'," + eachItem.MOD_DATE + @",'" + eachItem.STATUS + @"','" + eachItem.Rpt_No + @"'
							,'" + eachItem.Rpt_Seq + @"','" + eachItem.Ran_No + @"','" + eachItem.Sil + @"','" + eachItem.Rg_Code + @"','" + eachItem.IMP_GName1 + @"'
							,'" + eachItem.IMP_GName2 + @"','" + eachItem.IMP_GName3 + @"','" + eachItem.Compoent1 + @"','" + eachItem.Compoent2 + @"'," + eachItem.Qty + @"
							,'" + eachItem.Ut + @"'," + eachItem.Upi + @"," + eachItem.Amt + @",'" + eachItem.ModiRan + @"');");
				}
			}
			//return Query.ToString();
			DB_Ready.SqlCmd.CommandText = Query + "";
			DB_Ready.DBCon.Open();
			DB_Ready.SqlCmd.ExecuteNonQuery();
			DB_Ready.DBCon.Close();

			return "1";
		}
	}
}
public struct Shipper
{
    public string SN;
    public string Firm;
    public string Mark;
    public string State_Word;
    public string State;
    public string Addr1;
    public string Addr2;
    public string Addr3;
    public Shipper(string NULL)
    {
        SN = "";
        Firm = "";
        Mark = "";
        State_Word = "";
        State = "";
        Addr1 = "";
        Addr2 = "";
        Addr3 = "";
    }
}
public struct Consignee
{
	public string SN;
	public string Co;
	public string Co_Name;
	public string Addr1;
	public string Addr2;
	public string Boss_Name;
	public string TgNo;
	public string SdNo;
	public string Post_No;
	public string Email;
	public string EmailDomain;
	public string Ref_Tel;
	public Consignee(string NULL) {
		SN = "";
		Co = "";
		Co_Name = "";
		Addr1 = "";
		Addr2 = "";
		Boss_Name = "";
		TgNo = "";
		SdNo = "";
		Post_No = "";
		Email = "";
		EmailDomain = "";
		Ref_Tel = "";
	}
}
public struct Clearance
{
	public string MEMBER_ID;
	public string DIVISION_ID;
	public string REG_ID;
	public string REG_DATE;
	public string MOD_ID;
	public string MOD_DATE;
	public string STATUS;
	public string Rpt_No;
	public string Rpt_Seq;
	public string Rpt_Day;
	public string Cus;
	public string Sec;
	public string BlNo;
	public string Bl_Yn;
	public string Mrn;
	public string Msn;
	public string Hsn;
	public string Arr_Day;
	public string Inc_Day;
	public string Lev_Form;
	public string Rpt_Firm;
	public string Rpt_Name;
	public string Rpt_TelNo;
	public string Rpt_EMailID;
	public string Rpt_EMailDoMain;
	public string Imp_Code;
	public string Imp_Firm;
	public string Imp_Mark;
	public string Imp_TgNo;
	public string Imp_Divi;
	public string Nab_Code;
	public string Nab_Pa_Mark;
	public string Nab_Addr1;
	public string Nab_Addr2;
	public string Nab_Firm;
	public string Nab_Name;
	public string Nab_TgNo;
	public string Nab_SdNo;
	public string Nab_SdNo_Divi;
	public string Sd_Idno_Divi;
	public string Nab_TelNo;
	public string Nab_EMailID;
	public string Nab_EMailDoMain;
	public string Off_Firm;
	public string Off_Mark;
	public string Sup_Firm;
	public string Sup_St;
	public string Sup_St_Sht;
	public string Sup_Mark;
	public string Tg_Pln_Mark;
	public string Tg_Pln_Cot;
	public string Rpt_Divi_Mark;
	public string Rpt_Divi_Cot;
	public string Exc_Divi_Mark;
	public string Exc_Divi_Cot;
	public string Imp_Ki_Mark;
	public string Imp_Ki_Cot;
	public string Rpt_Use_Divi_CD;
	public string Rpt_Use_Divi_NM;
	public string Rpt_Use_Day;
	public string Ori_St_Prf_Yn;
	public string Amt_Rpt_Yn;
	public string Tot_Wt;
	public string Tot_Ut;
	public string Tot_Pack_Cnt;
	public string Tot_Pack_Ut;
	public string Arr_Mark;
	public string Arr_Name;
	public string Tra_Met;
	public string Tra_Cta;
	public string Fod_Mark;
	public string Fod_Sht;
	public string Ship;
	public string St_Sht;
	public string St_Code;
	public string Mas_BlNo;
	public string Tra_Chf_Mark;
	public string Chk_Pa_Mark;
	public string Chk_Pa;
	public string Chk_Pa_Name;
	public string Tot_Ran_Cnt;
	public string Con_Cod;
	public string Con_Cur;
	public string Con_Rate;
	public string Con_Rate_Usd;
	public Decimal Con_Tot_Amt;
	public string Con_Ki;
	public string Con_Ki_Cot;
	public Decimal Con_Amt;
	public string Con_Gita_Amt;
	public string Tot_Tax_Usd;
	public string Tot_Tax_Krw;
	public string Fre_Krw;
	public string Fre1_Ki;
	public string Fre1_Amt;
	public string Fre2_Ki;
	public string Fre2_Amt;
	public string Insu_Krw;
	public string Insu1_Ki;
	public string Insu1_Amt;
	public string Insu2_Ki;
	public string Insu2_Amt;
	public string App_Amt;
	public string Insu_Con_Amt;
	public string Ad_Divi;
	public string Ad_Cur_Ki;
	public string Ad_Cst_Rate;
	public string Ad_Cur_Ki2;
	public string Ad_Cst_Rate2;
	public string Ad_Amt_Krw;
	public string Ad_Cst_Krw;
	public string Sub_Divi;
	public string Sub_Cur_Ki;
	public string Sub_Cst_Rate;
	public string Sub_Cst_Krw;
	public string Tot_Gs;
	public string Tot_Ts;
	public string Tot_Hof;
	public string Tot_Gt;
	public string Tot_Ky;
	public string Tot_Nt;
	public string Tot_Blank;
	public string Tot_Vat;
	public string Tot_Dly_Tax;
	public string Tot_Add_Tax;
	public string Tot_Tax_Sum;
	public string Add_Tax_Gbn;
	public string Nab_No;
	public string Nab_Deli_Day;
	public string Vat_Tax_Ct;
	public string Vat_Free_Ct;
	public string Sp_Deli;
	public string Send_Divi;
	public string Res_Form;
	public string Cb_Divi1;
	public string Cb_Divi2;
	public string Cb_Divi3;
	public string CB1;
	public string CB2;
	public string CB3;
	public string CB4;
	public string CB5;
	public string CB6;
	public string CB7;
	public string CB8;
	public string CB9;
	public string Cus1;
	public string Cus2;
	public string Cus3;
	public string Cus4;
	public string Cus5;
	public string Cus6;
	public string Cus7;
	public string Cus8;
	public string Cus9;
	public string Cus10;
	public string Cus11;
	public string Cus12;
	public string Cus13;
	public string Cus14;
	public string Ju_Name;
	public string Ju_Mark;
	public string Rc_Day;
	public string Lis_Day;
	public string File_No;
	public string Send;
	public string Rece;
	public string Send_Cnt;
	public string Md_Refe_No;
	public string Usg_Yn;
	public string Imp_Req_No;
	public string Imp_Last_Day;
	public string Pay_Rpt_Rsul;
	public string Modi_Rpt_RSul;
	public string Modi_Rpt;
	public string Send_Date;
	public string SN_Divi;
	public string BLYnCd;
	public string BLYnTxt;
	public string AmtFixDay;
	public string AmtNo_5SM;
	public string FwdMark;
	public string FwdName;
	public string GumSa;
	public string GumSaAmt;
	public string ImpTypeCd;
	public string FwdCode;
	public string FwdFirm;
	public string FwdSdNo;
	public string BanChulDay;
	public string Bl_Wt;
	public string PDateTime;
	public string UDateTime;
	public string IDateTime;
	public string EDateTime;
	public string SDateTime;
	public string GDateTime;
	public string MDateTime;
	public string Re_Exp_Day;
	public string Exp_LisDay;
	public string Exp_LisNo;
	public string Ec_Day;
	public string Ref_Dn;
	public string GinGu;
	public string HsUser;
	public string Agent;
	public string StnCode;
	public string SaYu_Code;
	public string SaYu;
	public string PrtChk;
	public string PrtTime;
	public string ConfirmTime;
	public string ConfirmID;
	public string NabDivi;
	public string BigoCode;
	public string Upload;
	public string Bf;
	public string Imp;
	public string ImageSave;
	public string Po_No;
	public string GoldAccYn;
	public string Nab_RegNo;
	public string DDP;
	public string Urgent;
	public string Destination;
	public string Consignee;
	public string Rece_Result;
	public List<RAN> Rans;
	public Clearance(string HouseBL) {
		Rans = new List<RAN>();
		PDateTime = "";
		Modi_Rpt_RSul = "";
		Sp_Deli = "";
		Nab_Deli_Day = "";
		Insu2_Ki = "";
		Fre2_Ki = "";
		Con_Rate = "";
		Con_Cod = "";
		Chk_Pa = "";
		MOD_ID = "";
		REG_ID = "";
		Rpt_No = "";
		Mrn = "";
		Msn = "";
		Hsn = "";
		Arr_Mark = "";
		Ship = "";
		Mas_BlNo = "";
		Arr_Name = "";
		Sup_Firm = "";
		Sup_Mark = "";
		Sup_St = "";
		Sup_St_Sht = "";
		Imp_Code = "";
		Imp_Firm = "";
		Imp_TgNo = "";
		Nab_Code = "";
		Nab_Pa_Mark = "";
		Nab_Firm = "";
		Nab_TgNo = "";
		Nab_Name = "";
		Nab_SdNo = "";
		Nab_TelNo = "";
		Nab_Addr1 = "";
		Nab_Addr2 = "";
		Nab_EMailID = "";
		Nab_EMailDoMain = "";
		Con_Ki = "";
		Con_Ki_Cot = "";
		Con_Cur = "";
		Ad_Cur_Ki = "";
		Sub_Cur_Ki = "";
		Fre_Krw = "";
		Fre1_Amt = "";
		Ori_St_Prf_Yn = "";
		Con_Rate_Usd = "";
		Tra_Cta = "";
		Chk_Pa_Mark = "";
		Chk_Pa_Name = "";
		Sec = "";
		Cus = "";
		Imp_Mark = "";
		BlNo = HouseBL;
		Rpt_Day = DateTime.Today.ToString("yyyyMMdd") + "";
		Amt_Rpt_Yn = "N";//   가격신고서 
		File_No = "";
		Rpt_Seq = "00";
		Rpt_Divi_Mark = "A";// 신고구분				
		Rpt_Divi_Cot = "일반P/L신고";
		Arr_Day = "";//입항일
		Inc_Day = "";//반입일
		MEMBER_ID = "";
		DIVISION_ID = "";
		REG_DATE = "getdate()";
		MOD_DATE = "getdate()";
		STATUS = "";
		Bl_Yn = "N";
		Lev_Form = "11";//징수형태										/////// 이게 전항확인해서 전껄로 가자. 그리고 ALERT 띄우자
		Rpt_Firm = "인평관세사무소";
		Rpt_Name = "이득룡";
		Rpt_TelNo = "032-772-8480";
		Rpt_EMailID = "ldr1945";
		Rpt_EMailDoMain = "hanmail.net";
		Imp_Divi = "A";//수입자구분 :납세의무자와 동일(A)			//////////////전항차 확인해서 ALERT 띄우기
		Nab_SdNo_Divi = "04";
		Sd_Idno_Divi = "16";
		Off_Firm = "";
		Off_Mark = "";
		Tg_Pln_Mark = "C";// 통관계획										//////////////전항차 확인해서 ALERT 띄우기
		Tg_Pln_Cot = "보세구역도착전";//통관계획 텍스트								/////////////////////TEXT
		Exc_Divi_Mark = "11";//거래구분 11디폴트											
		Exc_Divi_Cot = "일반형태수입";//거래구분 텍스트 
		Imp_Ki_Mark = "21";// 수입종류  K디폴트
		Imp_Ki_Cot = "일반수입(내수용)";// 수입종류 텍스트
		Rpt_Use_Divi_CD = "";
		Rpt_Use_Divi_NM = "";
		Rpt_Use_Day = "";
		Tot_Ut = "KG";//총중량단위
		Tot_Pack_Ut = "";//총 포장 단위
		Tra_Met = "10";//운송수단									40AIR
		Fod_Mark = "CN";//적출국 
		Fod_Sht = "PR.CHNA";
		St_Sht = "";//선박회사 국적 어찌 하질 못함
		St_Code = "";//선박회사 국적 약어
		Tra_Chf_Mark = "";
		Fre1_Ki = "KRW";// 운임1 단위
		Insu1_Ki = "KRW";// 보험료 단위
		Ad_Cur_Ki2 = "";
		Add_Tax_Gbn = "N";  //미신고 
		Nab_No = "";//납세 번호  납부고지번호 아마 세관에서 
		Vat_Tax_Ct = "0";// 과세 numeric
		Vat_Free_Ct = "0";    // 면세 numeric
		Send_Divi = "9";
		Res_Form = "AB";
		Cb_Divi1 = "A";//기재구분 1
		Cb_Divi2 = "D";//기재구분 2
		Cb_Divi3 = "A";//기재구분 3
		CB1 = "";
		CB2 = "";
		CB3 = "";
		CB4 = "";
		CB5 = "";
		CB6 = "";
		CB7 = "";
		CB8 = "";
		CB9 = "";
		Cus1 = "";
		Cus2 = "";
		Cus3 = "";
		Cus4 = "";
		Cus5 = "";
		Cus6 = "";
		Cus7 = "";
		Cus8 = "";
		Cus9 = "";
		Cus10 = "";
		Cus11 = "";
		Cus12 = "";
		Cus13 = "";
		Cus14 = "";
		Ju_Name = "";// 심사자
		Ju_Mark = "";// 심사자번호
		Rc_Day = "";// 접수일시
		Lis_Day = "";// 수리일자
		Send = "";     //상태값(전완)
		Rece = "";//상태값(수리)
		Md_Refe_No = "";
		Usg_Yn = "";
		Imp_Req_No = "";
		Pay_Rpt_Rsul = "";
		Modi_Rpt = "수입";
		Send_Date = "";
		SN_Divi = "";
		BLYnCd = "";
		BLYnTxt = "";
		AmtFixDay = "";
		AmtNo_5SM = "";
		FwdMark = "INTL";//  운송주선 코드
		FwdName = "(주)아이엘";
		GumSa = "";
		GumSaAmt = "null";
		ImpTypeCd = "";
		FwdCode = "";
		FwdFirm = "";
		FwdSdNo = "";
		BanChulDay = "";
		EDateTime = "";
		SDateTime = "";
		GDateTime = "";
		MDateTime = "";
		Re_Exp_Day = "";
		Exp_LisDay = "";
		Exp_LisNo = "";
		Ec_Day = "";
		Ref_Dn = "";
		GinGu = "";
		HsUser = "";
		Agent = "";
		StnCode = "";
		SaYu_Code = "";
		SaYu = "";
		PrtChk = "";
		PrtTime = "";
		ConfirmTime = "";
		ConfirmID = "";
		NabDivi = "";
		BigoCode = "";
		Upload = "";
		Bf = "";
		Imp = "";
		ImageSave = "";
		Po_No = "";
		GoldAccYn = "";
		Nab_RegNo = "";
		DDP = "";
		Urgent = "";
		Destination = "";
		Consignee = "";
		Rece_Result = "";
		Ad_Divi = "FOB금";       // 가신금액 조건
		Sub_Divi = "FOB금";
		Bl_Wt = "0.00";//numeric(12; 2)
		Send_Cnt = "0";//numeric(2; 0)
		Tot_Dly_Tax = "0";//numeric(12; 0)
		Tot_Add_Tax = "0";//numeric(12; 0)
		Fre2_Amt = "0.00";//numeric(15; 2)
		Insu_Krw = "0";// 보험원화 numeric(12; 0)
		Con_Gita_Amt = "0.00";//기타 금액numeric(15; 2)
		Insu1_Amt = "0.00";//보험료 가격numeric(15; 2)
		Insu2_Amt = "0.00";//numeric(15; 2)
		App_Amt = "0.00";//numeric(15; 2)
		Insu_Con_Amt = "0.00";//numeric(15; 2)
		Ad_Cst_Rate = "0.0000";//numeric(16; 4)
		Ad_Cst_Rate2 = "0.0000";//numeric(16; 4)
		Ad_Amt_Krw = "0";//가산금액 합계numeric(12; 0)
		Ad_Cst_Krw = "0";//numeric(12; 0)
		Sub_Cst_Rate = "0.0000";//numeric(16; 4)
		Sub_Cst_Krw = "0";//numeric(12; 0)
		Tot_Ts = "0";//numeric(12; 0)
		Tot_Hof = "0";//numeric(12; 0)
		Tot_Gt = "0";//numeric(12; 0)
		Tot_Ky = "0";//numeric(12; 0)
		Tot_Nt = "0";//numeric(12; 0)
		Tot_Blank = "0";//numeric(12; 0)
		Tot_Wt = "0.0";// 총중량            numeric(15; 1)
		Tot_Pack_Cnt = "0";//--총포장수  numeric(8; 0)
		Tot_Ran_Cnt = "0";//numeric(3; 0)
		Con_Tot_Amt = decimal.Parse("0.00");//총결제금액numeric(15; 2)
		Con_Amt = decimal.Parse("0.00");//결제금액numeric(15; 2)
		Tot_Tax_Usd = "0";//과세가격 달러numeric(10; 0)
		Tot_Tax_Krw = "0";//과세가격 원화numeric(12; 0)
		Tot_Vat = "0";//부가세 합계//numeric(12; 0)
		Tot_Tax_Sum = "0";//총세액//numeric(12; 0)
		Tot_Gs = "0";// 관세 합계//numeric(12; 0)
		IDateTime = DateTime.Today.ToString("yyyymmddhhmm") + "";
		Imp_Last_Day = DateTime.Today.ToString("yyyymmddhhmm") + "";//????
		UDateTime = "";//시간???				
	}
}
public struct RAN
{
    public string SN;
    public string SNA1;
    public string MEMBER_ID;
    public string DIVISION_ID;
    public string REG_ID;
    public string REG_DATE;
    public string MOD_ID;
    public string MOD_DATE;
    public string STATUS;
    public string Rpt_No;
    public string Rpt_Seq;
    public string Ran_No;
    public string Hs;
    public string Std_GName1;
    public string Std_GName2;
    public string Exc_GName1;
    public string Exc_GName2;
    public string Std_Code;
    public string Model_GName1;
    public string Model_GName2;
    public string Att_Yn;
    public string sub_krw;
    public string fre_rmv_divi;
    public string fre_cur;
    public string fre_amt;
    public string fre_krw;
    public string insu_cur;
    public string insu_amt;
    public string insu_krw;
    public string ad_cur;
    public string ad_amt;
    public string ad_krw;
    public string Ad_Cst;
    public string sub_cur;
    public string sub_amt;
    public string Sub_Cst;
    public string Tax_Krw;
    public string Tax_Usd;
    public string Tax_Usg_Krw;
    public Decimal Tax_Usg_Usd;
    public Decimal Sun_Wt;
    public string Sun_Ut;
    public Decimal Qty;
    public string Qty_Ut;
    public Decimal Ref_Wt;
    public string Ref_Ut;
    public string Cs_Chk_Mark;
    public string Cs_Chk_Cot;
    public string Chk_Met_Mark;
    public string Chk_Met_Cot;
    public string Aft_Chk_Chf1;
    public string Aft_Chk_Chf2;
    public string Aft_Chk_Chf3;
    public string Ori_St_Mark1;
    public string Ori_St_Sht;
    public string Ori_St_Mark2;
    public string Ori_St_Mark3;
    public string Ori_St_Mark4;
    public string Ori_St_Exempt_CD;
    public string Ori_St_Exempt_NM;
    public string Ori_St_Day;
    public string Ori_St_No;
    public string Ori_St_Chf;
    public string Ori_St_iso;
    public string Sp_Tax_Basis;
    public string Tax_Ki_Divi;
    public string Gs_Divi;
    public string Gs_Rate;
    public string Upi_Tax;
    public string Gs_Rmv_Rate;
    public string Gs;
    public string Gs_Rmv;
    public string Gs_Rmv_Mark;
    public string Gs_Rmv_Divi;
    public string Send_Rate;
    public string Ng_Divi;
    public string Ng_Ki_Divi;
    public string Ng_Mark;
    public string Ng_Rate;
    public string Ng_Rmv_Amt;
    public string Ng;
    public string Hof_Free_Mark;
    public string Ts_Free_Mark;
    public string Ts_Cet_Sub;
    public string Ts;
    public string Gt;
    public string Hof;
    public string Ky_Divi;
    public string Ky_Ki_Divi;
    public string Ky_Rmv_Amt;
    public string Ky_Rate;
    public string Ky;
    public string Nt_Divi;
    public string Nt_Ki_Divi;
    public string Nt;
    public string Nt_Rate;
    public string Vat_Rate;
    public string Vat_Divi;
    public string Vat_Ki_Divi;
    public string Vat;
    public string Vat_Rmv_Mark;
    public string Vat_Rmv;
    public string Vat_Rmv_Rate;
    public string Gs_Cet;
    public string Vat_Tax_Ct;
    public string Vat_Free_ct;
    public string Sp_Deli;
    public string Roy_Rate;
    public string Roy_Divi;
    public string Roy_Amt;
    public string Chk_Met_Ch;
    public string Dam_Amt;
    public string Tot_Size_Cnt;
    public string Vit_Chk_Cnt;
    public string Tot_Exp_Cnt;
    public string YongDo_No;
    public string Ori_St_StnCd;
    public string Ori_St_Stn;
    public string Ori_St_Name;
    public string Ori_St_Mark5;
    public string Ori_St_Mark6;
    public string Ori_St_TotQty;
    public string Ori_St_Qty;
    public string Ori_St_TotWt;
    public string Ori_St_Wt;
    public string Ori_St_PartYn;
    public string Ori_St_PartDoc;
    public string ItemCd;
    public string ItemCdSend;
    public string ModiRan;
    public string CRan;
    public string Tot_C4_Cnt;
    public string Mg_Divi;
    public string Mg_RanNo;
    public List<Item> Items;
    public RAN(string Null = null)
    {
        Items = new List<Item>();
        Tax_Usg_Usd = 0m;
        Sun_Wt = 0m;
        Sun_Ut = "";// 순중량 단위
        //temp.Qty = "0";
        //temp.Qty_Ut = "";
        Qty = 0m;//수량 소숫점 첫번째 반올림 numeric(10, 0)
        Qty_Ut = "";                 // 수량 단위
        Hs = "";// HSCOde
        Ref_Wt = 0m;           //환급물량 numeric(13, 3)
        // 품명 1,품명2
        Std_GName1 = "";
        Std_GName2 = "";
        //Exc_GName1 = each.TempExc_GName1; //거래품명
        //Exc_GName2 = "";  //거래품명2
        Exc_GName1 = "";
        Exc_GName2 = "";
        Gs_Rate = "";//관세율numeric(6, 2)
        Send_Rate = "";    //관세율numeric(10, 2)
        Ran_No = "";
        Tax_Ki_Divi = "";
        Gs_Divi = "";
        SN = "";
        SNA1 = "";
        MEMBER_ID = "";
        DIVISION_ID = "";
        REG_ID = "";               //등록아이디
        REG_DATE = "getdate()";//등록시간
        MOD_ID = "";
        MOD_DATE = "getdate()";
        STATUS = "";
        Rpt_No = "";          //신고번호
        Rpt_Seq = "";
        Std_Code = "XXXX";//상표코드 
        Model_GName1 = "NO";     // 상표코드 텍
        Model_GName2 = "";
        Att_Yn = "N"; // 첨부 여부 디폴트 N
        sub_krw = "0";            // 0numeric(12, 0)
        fre_rmv_divi = "N";       //  면세적용 N
        fre_cur = "";
        fre_amt = "0.00";    //감면율 numeric(14, 2)
        fre_krw = "0";     // 감면액 0numeric(12, 0)
        insu_cur = "";
        insu_amt = "0.00";   // 내국세율 0.00numeric(14, 2)
        insu_krw = "0"; // 내국세 0numeric(12, 0)
        ad_cur = "";
        ad_amt = "0.00";     // 0.00numeric(14, 2)
        ad_krw = "0";   // 0numeric(12, 0)
        Ad_Cst = "0";//0numeric(12, 0)
        sub_cur = "";
        sub_amt = "0.00"; //0.00numeric(14, 2)
        Sub_Cst = "0";   //0numeric(12, 0)
        Tax_Usg_Krw = "0.00";    //0.00numeric(15, 2)
        Chk_Met_Mark = "";
        Chk_Met_Cot = "";
        Aft_Chk_Chf1 = "";
        Aft_Chk_Chf2 = "";
        Aft_Chk_Chf3 = "";
        Ori_St_Mark1 = ""; //원산지 국가코드 
        Ori_St_Sht = "";//원산지 국가코드  약어
        Ori_St_Mark2 = ""; //원산지 표시유무
        Ori_St_Mark3 = "";//?원산지 방법
        Ori_St_Mark4 = "";//?원산지 세부코드 조사 필요
        Ori_St_Exempt_CD = "";   // 면제사유
        Ori_St_Exempt_NM = "";   // 면제사유 텍스트
        Ori_St_Day = ""; //원산지 발행일자
        Ori_St_No = "";     // 원산지발행번호
        Ori_St_Chf = "";       // 발행기관
        Ori_St_iso = "";        // 발행국가
        Sp_Tax_Basis = "0.00";  //0.00 numeric(14, 2)
        Upi_Tax = "0.00";             //0.00numeric(10, 2)
        Gs_Rmv_Rate = "0.00";   //0.00numeric(7, 2)
        Gs_Rmv = "0";     // 0numeric(12, 0)
        Gs_Rmv_Mark = "";
        Gs_Rmv_Divi = "";
        Ng_Divi = "";
        Ng_Ki_Divi = "";
        Ng_Mark = "";
        Ng_Rate = "0.00";//0.00numeric(7, 2)
        Ng_Rmv_Amt = "0";//0numeric(12, 0)
        Ng = "0";//0numeric(12, 0)
        Hof_Free_Mark = "";
        Ts_Free_Mark = "";
        Ts_Cet_Sub = "0";     //0numeric(15, 0)
        Ts = "0";//0numeric(12, 0)
        Gt = "0";//0numeric(12, 0)
        Hof = "0";//0numeric(12, 0)
        Ky_Divi = "";
        Ky_Ki_Divi = "";
        Ky_Rmv_Amt = "0";//0numeric(12, 0)
        Ky_Rate = "0";//0numeric(6, 2)
        Ky = "0";//0numeric(12, 0)
        Nt_Divi = "";
        Nt_Ki_Divi = "";
        Nt = "0";//0numeric(12, 0)
        Nt_Rate = "0";//0numeric(6, 2)
        Vat_Rate = "10";//10 Vat_Rate
        Vat_Divi = "A";   //A   부가세구분
        Vat_Ki_Divi = "과";//과   부가세구분텍스트
        Vat_Rmv_Mark = "";
        Vat_Rmv = "0";   // 면세 과표 numeric(12, 0)
        Vat_Rmv_Rate = "0.00";   // 0.00  numeric(6, 2)
        Gs_Cet = "1";      //1 인데 몬지 모르겟음 확인 해야대   
        Vat_Tax_Ct = "0";    // 과세과표 Vat_Tax_Ct numeric
        Sp_Deli = "";
        Roy_Rate = "0.00";//0.00 numeric(6, 2)
        Roy_Divi = "";
        Roy_Amt = "0.00";//0.00 numeric(12, 0)
        Chk_Met_Ch = "";
        Dam_Amt = "0.00";//0.00 numeric(12, 0)
        Vit_Chk_Cnt = "";
        YongDo_No = "";
        Ori_St_StnCd = ""; //원산지증명세부사항?
        Ori_St_Stn = "";//원산지증명세부사항?
        Ori_St_Name = "";//원산지증명세부사항?
        Ori_St_Mark5 = "A";//원산지 결정기준
        Ori_St_Mark6 = "";//원산지증명세부사항?
        Ori_St_PartYn = "";//원산지증명세부사항?
        Ori_St_PartDoc = "";//원산지증명세부사항?
        ModiRan = "";
        CRan = "";
        Mg_Divi = "";
        Mg_RanNo = "";
        Ref_Ut = "";              //단위
        Cs_Chk_Mark = "";            // 구분부호 
        Cs_Chk_Cot = "";              // 청 CS 검사생략
        ItemCd = "";// 품명코드
        ItemCdSend = "";//품명 텍스트
        Tax_Krw = "0";//과세가격;numeric(12, 0)
        Tax_Usd = "0";//신고가격numeric(12, 0)
        Gs = "0";       //  관세액numeric(12, 0)
        Tot_Size_Cnt = "0";//규격수 numeric(2, 0)
        Tot_Exp_Cnt = "0";// numeric(3, 0)
        Ori_St_TotQty = "0";//원산지증명세부사항? numeric(10, 0)
        Ori_St_Qty = "0";//원산지증명세부사항?numeric(10, 0)
        Ori_St_TotWt = "0.0";//원산지증명세부사항?numeric(14, 1)
        Ori_St_Wt = "0.0";//원산지증명세부사항?numeric(14, 1)
        Tot_C4_Cnt = "0"; //numeric(2, 0)
        Vat = "0";              // 부가세금액 numeric(12, 0)
        Vat_Free_ct = "0";       //면세과표 Vat_Free_ct numeric(12, 0)
    }
}
public struct Item
{
	public string SN;
	public string SNA1;
	public string SNB1;
	public string MEMBER_ID;
	public string DIVISION_ID;
	public string REG_ID;
	public string REG_DATE;
	public string MOD_ID;
	public string MOD_DATE;
	public string STATUS;
	public string Rpt_No;
	public string Rpt_Seq;
	public string Ran_No;
	public string Sil;
	public string Rg_Code;
	public string IMP_GName1;
	public string IMP_GName2;
	public string IMP_GName3;
	public string Compoent1;
	public string Compoent2;
	public string Qty;
	public string Ut;
	public string Upi;
	public string Amt;
	public string ModiRan;
	//nn21.net 에서 데이터 변환 과정 추가
	public string TempSun_Wt;
	public string TempFCN1Check;
	public string TempE1Check;
	public string TempCCheck;
	public string TempStd_GName1;
	public string TempExc_GName1;
	public string TempPackedCount;
	public string TempSun_Ut;
	public string TempGs_Rate;
	public string TempAmt;
	public string TempHS;
	public string TempPackedUnit;
	public Item(string AccountId) {

		SN = "";
		SNA1 = "";
		SNB1 = "";
		MEMBER_ID = "";
		DIVISION_ID = "";
		REG_ID = AccountId; //등록ID
		REG_DATE = "getdate()";// 등록 시간
		MOD_ID = AccountId;
		MOD_DATE = "getdate()";
		STATUS = "";
		Rg_Code = "";
		Compoent2 = "";
		ModiRan = "";
		Rpt_No = "";
		Rpt_Seq = "";
		Sil = "";// 규격번호
		IMP_GName2 = "";//모델규격2
		IMP_GName3 = "";//모델규격3
		Compoent1 = "";

		Qty = "";
		Ut = "";
		Upi = "";
		Amt = "";
		TempSun_Wt = "";
		TempHS = "";
		TempFCN1Check = "";
		TempE1Check = "";
		TempCCheck = "";
		TempPackedCount = "";
		TempSun_Ut = "KG";
		TempGs_Rate = "";
		TempAmt = "";
		TempStd_GName1 = "";
		TempExc_GName1 = "";
		TempPackedUnit = "";
		Ran_No = "";
		IMP_GName1 = "";
		IMP_GName2 = "";
		IMP_GName3 = "";
	}
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
	public class ChargeC {

		public ChargeC() {
			
		}

		public List<sStandardPriceList> LoadList_StandardPriceList(string ShipperPk, string TransportWayCL, string Role, ref DBConn DB) {
			List<sStandardPriceList> ReturnValue = new List<sStandardPriceList>();
			sStandardPriceList Temp;
			string QueryWhere = "";
			if (Role == "M") {
				QueryWhere = " AND [TRANSPORT_WAY_CL] = " + TransportWayCL;
			}

			DB.SqlCmd.CommandText = @"SELECT 
				[STANDARDPRICE_PK]
				,[OURBRANCH_PK]
				,[TITLE]
				,[TRANSPORT_WAY_CL]
				,[STANDARD_GUIDE]
				,[LIMIT_OVERWEIGHT]
				,[ROLE]
			FROM [dbo].[STANDARDPRICE_LIST]
			--WHERE [OURBRANCH_PK] = " + ShipperPk + @"
			--AND [ROLE] = '" + Role + @"'" + QueryWhere;

			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				Temp = new sStandardPriceList();

				Temp.StandardPrice_Pk = RS["STANDARDPRICE_PK"] + "";
				Temp.OurBranch_Pk = RS["OURBRANCH_PK"] + "";
				Temp.Title = RS["TITLE"] + "";
				Temp.Transport_Way_CL = RS["TRANSPORT_WAY_CL"] + "";
				Temp.Standard_Guide = RS["STANDARD_GUIDE"] + "";
				Temp.Limit_OverWeight = RS["LIMIT_OVERWEIGHT"] + "";

				ReturnValue.Add(Temp);
			}
			RS.Close();
			return ReturnValue;
		}

		public sStandardPriceList Load_StandardPriceItem(string StandardPricePk, ref DBConn DB) {
			sStandardPriceList ReturnValue = new sStandardPriceList();
			ReturnValue.arrItem = new List<sStandardPriceItem>();
			sStandardPriceItem Item;

			DB.SqlCmd.CommandText = @"SELECT 
				LIST.[STANDARDPRICE_PK]
				, LIST.[OURBRANCH_PK]
				, LIST.[TITLE] AS LIST_TITLE
				, LIST.[TRANSPORT_WAY_CL]
				, LIST.[STANDARD_GUIDE]
				, LIST.[LIMIT_OVERWEIGHT]
				, ITEM.[STANDARDPRICE_ITEM_PK]
				, ITEM.[SETTLEMENT_BRANCH_PK]
				, ITEM.[TITLE] AS ITEM_TITLE
				, ITEM.[MONETARY_UNIT]
				, ITEM.[EXW]
				, ITEM.[DDP]
				, ITEM.[CNF]
				, ITEM.[FOB]
				, ITEM.[OVERWEIGHT_FLAG]
			FROM [dbo].[STANDARDPRICE_LIST] AS LIST
			LEFT JOIN [dbo].[STANDARDPRICE_ITEM] AS ITEM ON LIST.[STANDARDPRICE_PK] = ITEM.[STANDARDPRICE_PK] 
			WHERE LIST.[STANDARDPRICE_PK] = " + StandardPricePk;

			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			string Current = "";
			while (RS.Read()) {
				if (Current != RS["STANDARDPRICE_PK"] + "") {
					ReturnValue.StandardPrice_Pk = RS["STANDARDPRICE_PK"] + "";
					ReturnValue.OurBranch_Pk = RS["OURBRANCH_PK"] + "";
					ReturnValue.Title = RS["LIST_TITLE"] + "";
					ReturnValue.Transport_Way_CL = RS["TRANSPORT_WAY_CL"] + "";
					ReturnValue.Standard_Guide = RS["STANDARD_GUIDE"] + "";
					ReturnValue.Limit_OverWeight = RS["LIMIT_OVERWEIGHT"] + "";

					Current = RS["STANDARDPRICE_PK"] + "";
				}
				Item = new sStandardPriceItem();
				Item.StandardPrice_Item_Pk = RS["STANDARDPRICE_ITEM_PK"] + "";
				Item.Settlement_Branch_Pk = RS["SETTLEMENT_BRANCH_PK"] + "";
				Item.Title = RS["ITEM_TITLE"] + "";
				Item.Monetary_Unit = RS["MONETARY_UNIT"] + "";
				Item.EXW = RS["EXW"] + "";
				Item.DDP = RS["DDP"] + "";
				Item.CNF = RS["CNF"] + "";
				Item.FOB = RS["FOB"] + "";
				Item.OverWeight_Flag = RS["OVERWEIGHT_FLAG"] + "";

				ReturnValue.arrItem.Add(Item);
			}
			RS.Close();
			return ReturnValue;
		}

		public List<sStandardPriceValue> Load_StandardPriceValue(string StandardPricePk, string Criterion, ref DBConn DB) {
			List<sStandardPriceValue> ReturnValue = new List<sStandardPriceValue>();
			sStandardPriceValue Temp;

			DB.SqlCmd.CommandText = @"SELECT 
				ITEM.STANDARDPRICE_ITEM_PK
				, MIN(VALUE.STANDARDPRICE_VALUE_PK) AS STANDARDPRICE_VALUE_PK
				, MIN(VALUE.CRITERION) AS CRITERION
				, ISNULL(MIN(VALUE.PRICE), 0) AS PRICE
			FROM [dbo].[STANDARDPRICE_ITEM] AS ITEM 
			LEFT JOIN [dbo].[STANDARDPRICE_VALUE] AS VALUE ON ITEM.STANDARDPRICE_ITEM_PK = VALUE.STANDARDPRICE_ITEM_PK
			WHERE ITEM.[STANDARDPRICE_PK] = " + StandardPricePk + @"
			AND ISNULL(VALUE.CRITERION, 1000) >= " + Criterion + @"
			GROUP BY ITEM.STANDARDPRICE_ITEM_PK
			HAVING MIN(ISNULL(VALUE.CRITERION, 10000)) >= " + Criterion + @"
			ORDER BY ITEM.STANDARDPRICE_ITEM_PK";

			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				Temp = new sStandardPriceValue();
				Temp.StandardPrice_Value_Pk = RS["STANDARDPRICE_VALUE_PK"] + "";
				Temp.StandardPrice_Item_Pk = RS["STANDARDPRICE_ITEM_PK"] + "";
				Temp.Criterion = RS["CRITERION"] + "";
				Temp.Price = RS["PRICE"] + "";

				ReturnValue.Add(Temp);
			}
			RS.Close();
			return ReturnValue;
		}

		public List<sStandardPriceValue> LoadList_StandardPriceValue(string StandardPriceItemPk, ref DBConn DB) {
			List<sStandardPriceValue> ReturnValue = new List<sStandardPriceValue>();
			sStandardPriceValue Temp;

			DB.SqlCmd.CommandText = @"SELECT 
				[STANDARDPRICE_VALUE_PK]
				,[STANDARDPRICE_ITEM_PK]
				,[CRITERION]
				,[CRITERION_TEXT]
				,[PRICE]
			FROM [dbo].[STANDARDPRICE_VALUE]
			WHERE [STANDARDPRICE_ITEM_PK] = " + StandardPriceItemPk;

			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				Temp = new sStandardPriceValue();
				Temp.StandardPrice_Value_Pk = RS["STANDARDPRICE_VALUE_PK"] + "";
				Temp.StandardPrice_Item_Pk = RS["STANDARDPRICE_ITEM_PK"] + "";
				Temp.Criterion = RS["CRITERION"] + "";
				Temp.Criterion_Text = RS["CRITERION_TEXT"] + "";
				Temp.Price = RS["PRICE"] + "";

				ReturnValue.Add(Temp);
			}
			RS.Close();
			return ReturnValue;
		}

		public sRequestFormCalculateInfo Load_Calculated(string Table_Name, string Table_Pk, string ChargeNo, ref DBConn DB) {
			sRequestFormCalculateInfo ReturnValue = new sRequestFormCalculateInfo();
			ReturnValue.arrHead = new List<sRequestFormCalculateHead>();
			sRequestFormCalculateHead HTemp = new sRequestFormCalculateHead();
			HTemp.arrBody = new List<sRequestFormCalculateBody>();
			sRequestFormCalculateBody BTemp = new sRequestFormCalculateBody();
			string QueryWhere = "";
			
			if (ChargeNo != "") {
				QueryWhere = "AND HEAD.[CHARGE_NO] = " + ChargeNo;
			}

			if (Table_Name == "RequestForm") {
				DB.SqlCmd.CommandText = @"SELECT
					RF.[RequestFormPk]
					, RF.[StandardPricePk]
					, RF.[TotalPackedCount]
					, RF.[PackingUnit]
					, RF.[TotalGrossWeight]
					, RF.[TotalVolume]
					, RF.[CriterionValue]
					, RF.[OverWeightValue]
					, RF.[PaymentWhoCL]
					, RF.[ExchangeDate]
					, HEAD.[REQUESTFORMCALCULATE_HEAD_PK]
					, HEAD.[BRANCH_COMPANY_PK]
					, BRANCH.[CompanyCode] AS BRANCH_CODE
					, HEAD.[BRANCH_BANK_PK]
					, HEAD.[CUSTOMER_COMPANY_PK]
					, CUSTOMER.[CompanyCode] AS CUSTOMER_CODE
					, HEAD.[CUSTOMER_BANK_PK]
					, HEAD.[MONETARY_UNIT] AS TOTAL_MONETARY_UNIT
					, HEAD.[TOTAL_PRICE]
					, BODY.[REQUESTFORMCALCULATE_BODY_PK]
					, BODY.[REQUESTFORMCALCULATE_HEAD_PK]
					, BODY.[RELATION_PK]
					, BODY.[SETTLEMENT_COMPANY_PK]
					, BODY.[STANDARDPRICE_ITEM_PK]
					, BODY.[CATEGORY]
					, BODY.[PRICE_CODE]
					, BODY.[TITLE]
					, BODY.[ORIGINAL_MONETARY_UNIT]
					, BODY.[EXCHANGED_MONETARY_UNIT]
					, BODY.[FROM_EXCHANGE_RATE]
					, BODY.[TO_EXCHANGE_RATE]
					, BODY.[ORIGINAL_PRICE]
					, BODY.[EXCHANGED_PRICE]
				FROM [dbo].[RequestForm] AS RF
				LEFT JOIN [dbo].[REQUESTFORMCALCULATE_HEAD] AS HEAD ON RF.[RequestFormPk] = HEAD.[TABLE_PK]
				LEFT JOIN [dbo].[REQUESTFORMCALCULATE_BODY] AS BODY ON HEAD.[REQUESTFORMCALCULATE_HEAD_PK] = BODY.[REQUESTFORMCALCULATE_HEAD_PK]
				LEFT JOIN [dbo].[Company] AS BRANCH ON HEAD.[BRANCH_COMPANY_PK] = BRANCH.[CompanyPk]
				LEFT JOIN [dbo].[Company] AS CUSTOMER ON HEAD.[CUSTOMER_COMPANY_PK] = CUSTOMER.[CompanyPk]
				WHERE HEAD.[TABLE_NAME] = '" + Table_Name + @"'
				AND RF.[RequestFormPk] = " + Table_Pk + QueryWhere;
			}
			else {
				DB.SqlCmd.CommandText = @"SELECT 
					TH.[TRANSPORT_PK]
					, TH.[VESSELNAME]
					, HEAD.[REQUESTFORMCALCULATE_HEAD_PK]
					, HEAD.[BRANCH_COMPANY_PK]
					, BRANCH.[CompanyCode] AS BRANCH_CODE
					, HEAD.[BRANCH_BANK_PK]
					, HEAD.[CUSTOMER_COMPANY_PK]
					, CUSTOMER.[CompanyCode] AS CUSTOMER_CODE
					, HEAD.[CUSTOMER_BANK_PK]
					, HEAD.[MONETARY_UNIT] AS TOTAL_MONETARY_UNIT
					, HEAD.[TOTAL_PRICE]
					, BODY.[REQUESTFORMCALCULATE_BODY_PK]
					, BODY.[REQUESTFORMCALCULATE_HEAD_PK]
					, BODY.[RELATION_PK]
					, BODY.[SETTLEMENT_COMPANY_PK]
					, BODY.[STANDARDPRICE_ITEM_PK]
					, BODY.[CATEGORY]
					, BODY.[PRICE_CODE]
					, BODY.[TITLE]
					, BODY.[ORIGINAL_MONETARY_UNIT]
					, BODY.[EXCHANGED_MONETARY_UNIT]
					, BODY.[FROM_EXCHANGE_RATE]
					, BODY.[TO_EXCHANGE_RATE]
					, BODY.[ORIGINAL_PRICE]
					, BODY.[EXCHANGED_PRICE]
				FROM [dbo].[TRANSPORT_HEAD] AS TH
				LEFT JOIN [dbo].[REQUESTFORMCALCULATE_HEAD] AS HEAD ON TH.[TRANSPORT_PK] = HEAD.[TABLE_PK]
				LEFT JOIN [dbo].[REQUESTFORMCALCULATE_BODY] AS BODY ON HEAD.[REQUESTFORMCALCULATE_HEAD_PK] = BODY.[REQUESTFORMCALCULATE_HEAD_PK]
				LEFT JOIN [dbo].[Company] AS BRANCH ON HEAD.[BRANCH_COMPANY_PK] = BRANCH.[CompanyPk]
				LEFT JOIN [dbo].[Company] AS CUSTOMER ON HEAD.[CUSTOMER_COMPANY_PK] = CUSTOMER.[CompanyPk]
				WHERE HEAD.[TABLE_NAME] = '" + Table_Name + @"'
				AND TH.[TRANSPORT_PK] = " + Table_Pk + QueryWhere;
			}

			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			string CurrReqPk = "";
			string CurrHeadPk = "";
			while (RS.Read()) {
				if (Table_Name == "RequestForm") {
					if (RS["RequestFormPk"] + "" != CurrReqPk) {
						ReturnValue.RequestPk = RS["RequestFormPk"] + "";
						ReturnValue.StandardPricePk = RS["StandardPricePk"] + "";
						ReturnValue.TotalPackedCount = RS["TotalPackedCount"] + "";
						ReturnValue.PackingUnit = RS["PackingUnit"] + "";
						ReturnValue.TotalGrossWeight = RS["TotalGrossWeight"] + "";
						ReturnValue.TotalVolume = RS["TotalVolume"] + "";
						ReturnValue.CriterionValue = RS["CriterionValue"] + "";
						ReturnValue.OverWeightValue = RS["OverWeightValue"] + "";
						ReturnValue.PaymentWay = RS["PaymentWhoCL"] + "";
						ReturnValue.ExchangeDate = RS["ExchangeDate"] + "";
					}
				}

				if (CurrHeadPk != "" && RS["REQUESTFORMCALCULATE_HEAD_PK"] + "" != CurrHeadPk) {
		
				}
				if (RS["REQUESTFORMCALCULATE_HEAD_PK"] + "" != CurrHeadPk) {
					HTemp = new sRequestFormCalculateHead();
					HTemp.arrBody = new List<sRequestFormCalculateBody>();
					HTemp.RequestFormCalculate_Head_Pk = RS["REQUESTFORMCALCULATE_HEAD_PK"] + "";
					HTemp.Branch_Company_Pk = RS["BRANCH_COMPANY_PK"] + "";
					HTemp.Branch_Code = RS["BRANCH_CODE"] + "";
					HTemp.Branch_Bank_Pk = RS["BRANCH_BANK_PK"] + "";
					HTemp.Customer_Company_Pk = RS["CUSTOMER_COMPANY_PK"] + "";
					HTemp.Customer_Code = RS["CUSTOMER_CODE"] + "";
					HTemp.Customer_Bank_Pk = RS["CUSTOMER_BANK_PK"] + "";
					HTemp.Monetary_Unit = RS["TOTAL_MONETARY_UNIT"] + "";
					HTemp.Total_Price = RS["TOTAL_PRICE"] + "";

					ReturnValue.arrHead.Add(HTemp);
				}
				BTemp = new sRequestFormCalculateBody();
				BTemp.RequestFormCalculate_Body_Pk = RS["REQUESTFORMCALCULATE_BODY_PK"] + "";
				BTemp.RequestFormCalculate_Head_Pk = RS["REQUESTFORMCALCULATE_HEAD_PK"] + "";
				BTemp.Relation_Pk = RS["RELATION_PK"] + "";
				BTemp.Settlement_Company_Pk = RS["SETTLEMENT_COMPANY_PK"] + "";
				BTemp.StandardPrice_Item_Pk = RS["STANDARDPRICE_ITEM_PK"] + "";
				BTemp.Category = RS["CATEGORY"] + "";
				BTemp.Price_Code = RS["PRICE_CODE"] + "";
				BTemp.Title = RS["TITLE"] + "";
				BTemp.Original_Monetary_Unit = RS["ORIGINAL_MONETARY_UNIT"] + "";
				BTemp.Exchanged_Monetary_Unit = RS["EXCHANGED_MONETARY_UNIT"] + "";
				BTemp.Original_Price = RS["ORIGINAL_PRICE"] + "";
				BTemp.Exchanged_Price = RS["EXCHANGED_PRICE"] + "";
				HTemp.arrBody.Add(BTemp);

				if (Table_Name == "RequestForm") {
					CurrReqPk = RS["RequestFormPk"] + "";
				}
				CurrHeadPk = RS["REQUESTFORMCALCULATE_HEAD_PK"] + "";
			}

			RS.Close();
			return ReturnValue;
		}

		public List<sBankDeposit> Load_BankDeposit(string BankPk, string RetrieveDate, ref DBConn DB) {
			List<sBankDeposit> ReturnValue = new List<sBankDeposit>();
			sBankDeposit Temp;
			DB.SqlCmd.CommandText = @"SELECT 
				[BANK_DEPOSIT_PK]
				,[BANK_PK]
				,[CATEGORY]
				,[TYPE]
				,[TYPE_PK]
				,[DESCRIPTION]
				,[MONETARY_UNIT]
				,[PRICE]
				,[PRICE_REMAIN]
				,[DATETIME]
				,[REGISTERD]
			FROM [dbo].[BANK_DEPOSIT] 
			WHERE [BANK_PK] = " + BankPk + @"
			AND LEFT([DATETIME], 10) = '" + RetrieveDate + @"' 
			ORDER BY [DATETIME] ASC, [REGISTERD] ASC ";
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();

			while (RS.Read()) {
				Temp = new sBankDeposit();
				Temp.Bank_Deposit_Pk = RS["BANK_DEPOSIT_PK"] + "";
				Temp.Bank_Pk = RS["BANK_PK"] + "";
				Temp.Category = RS["CATEGORY"] + "";
				Temp.Type = RS["TYPE"] + "";
				Temp.Type_Pk = RS["TYPE_PK"] + "";
				Temp.Description = RS["DESCRIPTION"] + "";
				Temp.Monetary_Unit = RS["MONETARY_UNIT"] + "";
				Temp.Price = RS["PRICE"] + "";
				Temp.Price_Remain = RS["PRICE_REMAIN"] + "";
				Temp.DateTime = RS["DATETIME"] + "";
				Temp.Registerd = RS["REGISTERD"] + "";

				ReturnValue.Add(Temp);
			}

			return ReturnValue;
		}

		public string Set_CalculatedInfo(sRequestFormCalculateInfo CalculatedInfo, ref DBConn DB) {
			Setting ST = new Setting();
			DB.SqlCmd.CommandText = @"UPDATE [dbo].[RequestForm] SET 
			[StandardPricePk] = " + ST.ToDB(CalculatedInfo.StandardPricePk, "int") + @"
			,[CriterionValue] = " + ST.ToDB(CalculatedInfo.CriterionValue, "varchar") + @"
			,[OverWeightValue] = " + ST.ToDB(CalculatedInfo.OverWeightValue, "varchar") + @"
			,[PaymentWhoCL] = " + ST.ToDB(CalculatedInfo.PaymentWay, "int") + @"
			,[ExchangeDate] = " + ST.ToDB(CalculatedInfo.ExchangeDate, "datetime") + @"
			,[StepCL] = " + ST.ToDB(CalculatedInfo.StepCL, "int") + @"
			WHERE [RequestFormPk] = " + CalculatedInfo.RequestPk;

			DB.SqlCmd.ExecuteNonQuery();
			return "1";
		}

		public sRequestFormCalculateHead Load_RequestFormCalculateHead(string Table_Name, string Table_Pk, string CalcHeadPk, ref DBConn DB) {
			sRequestFormCalculateHead ReturnValue = new sRequestFormCalculateHead();
			ReturnValue.arrBody = new List<sRequestFormCalculateBody>();
			sRequestFormCalculateBody Temp;

			DB.SqlCmd.CommandText = @"SELECT 
				HEAD.[REQUESTFORMCALCULATE_HEAD_PK]
				,HEAD.[TABLE_NAME]
				,HEAD.[TABLE_PK]
				,HEAD.[TYPE]
				,HEAD.[BRANCH_COMPANY_PK]
				,HEAD.[BRANCH_BANK_PK]
				,BBANK.[BankName]
				,BBANK.[AccountNo]
				,HEAD.[CUSTOMER_COMPANY_PK]
				,HEAD.[CUSTOMER_BANK_PK]
				,CUSTOMER.[CompanyCode]
				,CUSTOMER.[CompanyName]
				,HEAD.[CHARGE_DATE]
				,HEAD.[MONETARY_UNIT]
				,HEAD.[TOTAL_PRICE]
				,ISNULL(HEAD.[DEPOSITED_PRICE], 0) AS DEPOSITED_PRICE
				,HEAD.[LAST_DEPOSITED_DATE]
				,RF.[TotalPackedCount]
				,BODY.[REQUESTFORMCALCULATE_BODY_PK]
				,BODY.[SETTLEMENT_COMPANY_PK]
				,BODY.[STANDARDPRICE_ITEM_PK]
				,BODY.[TITLE]
				,BODY.[ORIGINAL_MONETARY_UNIT]
				,BODY.[EXCHANGED_MONETARY_UNIT]
				,BODY.[FROM_EXCHANGE_RATE]
				,BODY.[TO_EXCHANGE_RATE]
				,BODY.[ORIGINAL_PRICE]
				,BODY.[EXCHANGED_PRICE]
			FROM [dbo].[REQUESTFORMCALCULATE_HEAD] AS HEAD
			LEFT JOIN [dbo].[REQUESTFORMCALCULATE_BODY] AS BODY ON HEAD.REQUESTFORMCALCULATE_HEAD_PK = BODY.REQUESTFORMCALCULATE_HEAD_PK
			LEFT JOIN [dbo].[CompanyBank] AS BBANK ON HEAD.BRANCH_BANK_PK = BBANK.CompanyBankPk
			LEFT JOIN [dbo].[Company] AS CUSTOMER ON HEAD.[CUSTOMER_COMPANY_PK] = CUSTOMER.[CompanyPk]
			LEFT JOIN [dbo].[RequestForm] AS RF ON HEAD.[TABLE_PK] = RF.[RequestFormPk]
			WHERE HEAD.[TABLE_NAME]  = '" + Table_Name + @"'
			AND HEAD.[TABLE_PK] = " + Table_Pk + @"
			AND HEAD.[REQUESTFORMCALCULATE_HEAD_PK] = " + CalcHeadPk;

			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			string CurrHead = "";
			while (RS.Read()) {
				if (CurrHead != RS["REQUESTFORMCALCULATE_HEAD_PK"] + "") {
					ReturnValue.RequestFormCalculate_Head_Pk = RS["REQUESTFORMCALCULATE_HEAD_PK"] + "";
					ReturnValue.Table_Name = RS["TABLE_NAME"] + "";
					ReturnValue.Table_Pk = RS["TABLE_PK"] + "";
					ReturnValue.Type = RS["TYPE"] + "";
					ReturnValue.Branch_Company_Pk = RS["BRANCH_COMPANY_PK"] + "";
					ReturnValue.Branch_Bank_Pk = RS["BRANCH_BANK_PK"] + "";
					ReturnValue.Bank_Branch_BankName = RS["BankName"] + "";
					ReturnValue.Bank_Branch_BankNo = RS["AccountNo"] + "";
					ReturnValue.Customer_Company_Pk = RS["CUSTOMER_COMPANY_PK"] + "";
					ReturnValue.Customer_Code = RS["CompanyCode"] + "";
					ReturnValue.Customer_Name = RS["CompanyName"] + "";
					ReturnValue.Customer_Bank_Pk = RS["CUSTOMER_BANK_PK"] + "";
					ReturnValue.Charge_Date = RS["CHARGE_DATE"] + "";
					ReturnValue.Monetary_Unit = RS["MONETARY_UNIT"] + "";
					ReturnValue.Total_Price = RS["TOTAL_PRICE"] + "";
					ReturnValue.Deposited_Price = RS["DEPOSITED_PRICE"] + "";
					ReturnValue.Last_Deposited_Date = RS["LAST_DEPOSITED_DATE"] + "";
					ReturnValue.TotalPackedCount = RS["TotalPackedCount"] + "";
				}
				Temp = new sRequestFormCalculateBody();
				Temp.RequestFormCalculate_Body_Pk = RS["REQUESTFORMCALCULATE_BODY_PK"] + "";
				Temp.Settlement_Company_Pk = RS["SETTLEMENT_COMPANY_PK"] + "";
				Temp.StandardPrice_Item_Pk = RS["STANDARDPRICE_ITEM_PK"] + "";
				Temp.Title = RS["TITLE"] + "";
				Temp.Original_Monetary_Unit = RS["ORIGINAL_MONETARY_UNIT"] + "";
				Temp.Exchanged_Monetary_Unit = RS["EXCHANGED_MONETARY_UNIT"] + "";
				Temp.From_Exchange_Rate = RS["FROM_EXCHANGE_RATE"] + "";
				Temp.To_Exchange_Rate = RS["TO_EXCHANGE_RATE"] + "";
				Temp.Original_Price = RS["ORIGINAL_PRICE"] + "";
				Temp.Exchanged_Price = RS["EXCHANGED_PRICE"] + "";

				ReturnValue.arrBody.Add(Temp);
				CurrHead = RS["REQUESTFORMCALCULATE_HEAD_PK"] + "";
			}
			RS.Close();

			return ReturnValue;
		}

		public string Set_CalculatedHead(sRequestFormCalculateHead CalculatedHead, ref DBConn DB) {
			Setting ST = new Setting();
			string ReturnValue = "";
			if (CalculatedHead.RequestFormCalculate_Head_Pk == null || CalculatedHead.RequestFormCalculate_Head_Pk == "") {
				DB.SqlCmd.CommandText = @"INSERT INTO [dbo].[REQUESTFORMCALCULATE_HEAD]
				([TABLE_NAME]
				,[TABLE_PK]
				,[TYPE]
				,[CHARGE_NO]
				,[BRANCH_COMPANY_PK]
				,[BRANCH_BANK_PK]
				,[CUSTOMER_COMPANY_PK]
				,[CUSTOMER_BANK_PK]
				,[STATUS]
				,[CHARGE_DATE]
				,[MONETARY_UNIT]
				,[TOTAL_PRICE])
				VALUES
				(" + ST.ToDB(CalculatedHead.Table_Name, "varchar") + @"
				," + ST.ToDB(CalculatedHead.Table_Pk, "int") + @"
				," + ST.ToDB(CalculatedHead.Type, "varchar") + @"
				," + ST.ToDB(CalculatedHead.Charge_No, "int") + @"
				," + ST.ToDB(CalculatedHead.Branch_Company_Pk, "int") + @"
				," + ST.ToDB(CalculatedHead.Branch_Bank_Pk, "int") + @"
				," + ST.ToDB(CalculatedHead.Customer_Company_Pk, "int") + @"
				," + ST.ToDB(CalculatedHead.Customer_Bank_Pk, "int") + @"
				," + ST.ToDB(CalculatedHead.Status, "varchar") + @"
				, getdate()
				," + ST.ToDB(CalculatedHead.Monetary_Unit, "int") + @"
				," + ST.ToDB(CalculatedHead.Total_Price, "money") + @"); SELECT @@IDENTITY;";
				ReturnValue = DB.SqlCmd.ExecuteScalar() + "";
			}
			else {
				DB.SqlCmd.CommandText = @"UPDATE [dbo].[REQUESTFORMCALCULATE_HEAD] SET 
				[TABLE_NAME] = " + ST.ToDB(CalculatedHead.Table_Name, "varchar") + @"
				,[TABLE_PK] = " + ST.ToDB(CalculatedHead.Table_Pk, "int") + @"
				,[TYPE] = " + ST.ToDB(CalculatedHead.Type, "varchar") + @"
				,[BRANCH_COMPANY_PK] = " + ST.ToDB(CalculatedHead.Branch_Company_Pk, "int") + @"
				,[BRANCH_BANK_PK] = " + ST.ToDB(CalculatedHead.Branch_Bank_Pk, "int") + @"
				,[CUSTOMER_COMPANY_PK] = " + ST.ToDB(CalculatedHead.Customer_Company_Pk, "int") + @"
				,[CUSTOMER_BANK_PK] = " + ST.ToDB(CalculatedHead.Customer_Bank_Pk, "int") + @"
				,[STATUS] = " + ST.ToDB(CalculatedHead.Status, "varchar") + @"
				,[MONETARY_UNIT] = " + ST.ToDB(CalculatedHead.Monetary_Unit, "int") + @"
				,[TOTAL_PRICE] = " + ST.ToDB(CalculatedHead.Total_Price, "money") + @"
				WHERE [REQUESTFORMCALCULATE_HEAD_PK] = " + CalculatedHead.RequestFormCalculate_Head_Pk;
				DB.SqlCmd.ExecuteNonQuery();
				ReturnValue = CalculatedHead.RequestFormCalculate_Head_Pk;
			}

			return ReturnValue;
		}

		public string Set_CalculatedBody(sRequestFormCalculateBody CalculatedBody, ref DBConn DB) {
			Setting ST = new Setting();
			if (CalculatedBody.RequestFormCalculate_Body_Pk == null || CalculatedBody.RequestFormCalculate_Body_Pk == "") {
				DB.SqlCmd.CommandText = @"INSERT INTO [dbo].[REQUESTFORMCALCULATE_BODY]
				([REQUESTFORMCALCULATE_HEAD_PK]
				,[SETTLEMENT_COMPANY_PK]
				,[STANDARDPRICE_ITEM_PK]
				,[CATEGORY]
				,[TITLE]
				,[ORIGINAL_MONETARY_UNIT]
				,[EXCHANGED_MONETARY_UNIT]
				,[FROM_EXCHANGE_RATE]
				,[TO_EXCHANGE_RATE]
				,[ORIGINAL_PRICE]
				,[EXCHANGED_PRICE])
				VALUES
				(" + ST.ToDB(CalculatedBody.RequestFormCalculate_Head_Pk, "int") + @"
				," + ST.ToDB(CalculatedBody.Settlement_Company_Pk, "int") + @"
				," + ST.ToDB(CalculatedBody.StandardPrice_Item_Pk, "int") + @"
				," + ST.ToDB(CalculatedBody.Category, "varchar") + @"
				," + ST.ToDB(CalculatedBody.Title, "nvarchar") + @"
				," + ST.ToDB(CalculatedBody.Original_Monetary_Unit, "int") + @"
				," + ST.ToDB(CalculatedBody.Exchanged_Monetary_Unit, "int") + @"
				," + ST.ToDB(CalculatedBody.From_Exchange_Rate, "money") + @"
				," + ST.ToDB(CalculatedBody.To_Exchange_Rate, "money") + @"
				," + ST.ToDB(CalculatedBody.Original_Price, "money") + @"
				," + ST.ToDB(CalculatedBody.Exchanged_Price, "money") + @"); ";
			}
			else {
				DB.SqlCmd.CommandText = @"UPDATE [dbo].[REQUESTFORMCALCULATE_BODY] SET 
				[REQUESTFORMCALCULATE_HEAD_PK] = " + ST.ToDB(CalculatedBody.RequestFormCalculate_Head_Pk, "int") + @"
				,[SETTLEMENT_COMPANY_PK] = " + ST.ToDB(CalculatedBody.Settlement_Company_Pk, "int") + @"
				,[STANDARDPRICE_ITEM_PK] = " + ST.ToDB(CalculatedBody.StandardPrice_Item_Pk, "int") + @"
				,[CATEGORY] = " + ST.ToDB(CalculatedBody.Category, "varchar") + @"
				,[TITLE] = " + ST.ToDB(CalculatedBody.Title, "nvarchar") + @"
				,[ORIGINAL_MONETARY_UNIT] = " + ST.ToDB(CalculatedBody.Original_Monetary_Unit, "int") + @"
				,[EXCHANGED_MONETARY_UNIT] = " + ST.ToDB(CalculatedBody.Exchanged_Monetary_Unit, "int") + @"
				,[FROM_EXCHANGE_RATE] = " + ST.ToDB(CalculatedBody.From_Exchange_Rate, "money") + @"
				,[TO_EXCHANGE_RATE] = " + ST.ToDB(CalculatedBody.To_Exchange_Rate, "money") + @"
				,[ORIGINAL_PRICE] = " + ST.ToDB(CalculatedBody.Original_Price, "money") + @"
				,[EXCHANGED_PRICE] = " + ST.ToDB(CalculatedBody.Exchanged_Price, "money") + @"
				WHERE [REQUESTFORMCALCULATE_BODY_PK] = " + CalculatedBody.RequestFormCalculate_Body_Pk;
			}
			DB.SqlCmd.ExecuteNonQuery();

			return "1";
		}

		public static string Get_MonetaryUnit(string Value) {
			StringBuilder ReturnValue = new StringBuilder();
			string[] Choosed = new string[] { "", "", "", "", "", "" };
			switch (Value) {
				case "18":
					Choosed[0] = "selected=\"selected\"";
					break;
				case "19":
					Choosed[1] = "selected=\"selected\"";
					break;
				case "20":
					Choosed[2] = "selected=\"selected\"";
					break;
				case "21":
					Choosed[3] = "selected=\"selected\"";
					break;
				case "22":
					Choosed[4] = "selected=\"selected\"";
					break;
				case "23":
					Choosed[5] = "selected=\"selected\"";
					break;
				default:
					break;
			}
			ReturnValue.Append("<option value=\"18\" " + Choosed[0] + ">￥</option>");
			ReturnValue.Append("<option value=\"19\" " + Choosed[1] + ">$</option>");
			ReturnValue.Append("<option value=\"20\" " + Choosed[2] + ">￦</option>");
			ReturnValue.Append("<option value=\"21\" " + Choosed[3] + ">Y</option>");
			ReturnValue.Append("<option value=\"22\" " + Choosed[4] + ">HK$</option>");
			ReturnValue.Append("<option value=\"23\" " + Choosed[5] + ">€</option>");

			return ReturnValue + "";
		}

		public static string Get_OurBranch(string Value) {
			StringBuilder ReturnValue = new StringBuilder();

			string[] Choosed = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
			switch (Value) {
				case "3157":
					Choosed[0] = "selected=\"selected\"";
					break;
				case "3095":
					Choosed[1] = "selected=\"selected\"";
					break;
				case "3843":
					Choosed[2] = "selected=\"selected\"";
					break;
				case "2886":
					Choosed[3] = "selected=\"selected\"";
					break;
				case "2887":
					Choosed[4] = "selected=\"selected\"";
					break;
				case "2888":
					Choosed[5] = "selected=\"selected\"";
					break;
				case "3388":
					Choosed[6] = "selected=\"selected\"";
					break;
				case "11456":
					Choosed[7] = "selected=\"selected\"";
					break;
				case "7898":
					Choosed[8] = "selected=\"selected\"";
					break;
				case "12437":
					Choosed[9] = "selected=\"selected\"";
					break;
				case "12438":
					Choosed[10] = "selected=\"selected\"";
					break;
				case "12527":
					Choosed[11] = "selected=\"selected\"";
					break;
				case "12464":
					Choosed[12] = "selected=\"selected\"";
					break;
				case "3798":
					Choosed[13] = "selected=\"selected\"";
					break;
				default:
					break;
			}
			ReturnValue.Append("<option value=\"3157\" " + Choosed[0] + ">KRIC 인천</option>");																			
			ReturnValue.Append("<option value=\"3095\" " + Choosed[1] + ">JPOK Osaka</option>");
			ReturnValue.Append("<option value=\"3843\" " + Choosed[2] + ">CNGZ 广州</option>");
			ReturnValue.Append("<option value=\"2886\" " + Choosed[3] + ">CNYT 烟台</option>");
			ReturnValue.Append("<option value=\"2887\" " + Choosed[4] + ">CNSY 瀋陽</option>");
			ReturnValue.Append("<option value=\"2888\" " + Choosed[5] + ">CNYW 义乌</option>");
			ReturnValue.Append("<option value=\"3388\" " + Choosed[6] + ">CNQD 青岛</option>");
			ReturnValue.Append("<option value=\"11456\" " + Choosed[7] + ">CNHZ 杭州</option>");
			ReturnValue.Append("<option value=\"7898\" " + Choosed[8] + ">CNSX 绍兴</option>");
			ReturnValue.Append("<option value=\"12437\" " + Choosed[9] + ">VTHM Hochimin</option>");
			ReturnValue.Append("<option value=\"12438\" " + Choosed[10] + ">VTHN Hanoi</option>");
			ReturnValue.Append("<option value=\"12527\" " + Choosed[11] + ">VTDN Danang</option>");
			ReturnValue.Append("<option value=\"12464\" " + Choosed[12] + ">MMYG</option>");
			ReturnValue.Append("<option value=\"3798\" " + Choosed[13] + ">OtherLocation</option>");

			return ReturnValue + "";
		}

		public static string Get_StandardPriceItem_Column(string StandardPriceItemPk, string Column, ref DBConn DB) {
			if (StandardPriceItemPk == "" || StandardPriceItemPk == null) {
				StandardPriceItemPk = "null";
			}
			DB.SqlCmd.CommandText = @"SELECT " + Column + @" FROM [dbo].[STANDARDPRICE_ITEM] WHERE [STANDARDPRICE_ITEM_PK] = " + StandardPriceItemPk;

			return DB.SqlCmd.ExecuteScalar() + "";
		}

	}



	public struct sOld_StandardPriceHead
	{
		public string StandardPriceHeadPk;
		public string Title;
		public string Length;
		public string OurBranchCode;
		public string A;
		public string B;
		public string C;
		public string D;
		public string E;
		public string F;
		public string G;
		public string H;
		public string I;
		public string J;
		public List<sOld_StandardPriceBody> arrBody;
	}

	public struct sOld_StandardPriceBody
	{
		public string StandardPriceHeadPk;
		public string A;
		public string B;
		public string C;
		public string D;
		public string E;
		public string F;
		public string G;
		public string H;
		public string I;
		public string J;
	}


	public struct sStandardPriceList
	{
		public string StandardPrice_Pk;
		public string Title;
		public string Transport_Way_CL;
		public string OurBranch_Pk;
		public string Standard_Guide;
		public string Limit_OverWeight;
		public string Role;
		public List<sStandardPriceItem> arrItem;
	}

	public struct sStandardPriceItem
	{
		public string StandardPrice_Item_Pk;
		public string StandardPrice_Pk;
		public string Settlement_Branch_Pk;
		public string Title;
		public string Monetary_Unit;
		public string EXW;
		public string DDP;
		public string CNF;
		public string FOB;
		public string OverWeight_Flag;
		public List<sStandardPriceValue> arrValue;
	}

	public struct sStandardPriceValue
	{
		public string StandardPrice_Value_Pk;
		public string StandardPrice_Item_Pk;
		public string Criterion;
		public string Criterion_Text;
		public string Price;
		public string Item_OverWeight_Flag;
	}

	public struct sRequestFormCalculateInfo
	{
		public string RequestPk;
		public string TotalPackedCount;
		public string PackingUnit;
		public string TotalGrossWeight;
		public string TotalVolume;
		public string StandardPricePk;
		public string CriterionValue;
		public string OverWeightValue;
		public string PaymentWay;
		public string ExchangeDate;
		public string StepCL;
		public string VesselName;
		public List<sRequestFormCalculateHead> arrHead;
	}

	public struct sRequestFormCalculateHead
	{
		public string RequestFormCalculate_Head_Pk;
		public string Table_Name;
		public string Table_Pk;
		public string Type;
		public string Charge_No;
		public string Branch_Company_Pk;
		public string Branch_Code;
		public string Branch_Bank_Pk;
		public string Customer_Company_Pk;
		public string Customer_Code;
		public string Customer_Name;
		public string Customer_Bank_Pk;
		public string Status;
		public string Charge_Date;
		public string Monetary_Unit;
		public string Total_Price;
		public string Deposited_Price;
		public string Last_Deposited_Date;
		public string TotalPackedCount;
		public string Bank_Branch_BankName;
		public string Bank_Branch_BankNo;
		public string VesselName;
		public string Transport_From;
		public string Transport_To;
		public string[] Temp_ConverExchangedData;
		public List<sRequestFormCalculateBody> arrBody;
	}

	public struct sRequestFormCalculateBody
	{
		public string RequestFormCalculate_Body_Pk;
		public string RequestFormCalculate_Head_Pk;
		public string Relation_Pk;
		public string Settlement_Company_Pk;
		public string StandardPrice_Item_Pk;
		public string Category;
		public string Price_Code;
		public string Title;
		public string Original_Monetary_Unit;
		public string Exchanged_Monetary_Unit;
		public string From_Exchange_Rate;
		public string To_Exchange_Rate;
		public string Original_Price;
		public string Exchanged_Price;
	}

	public struct sBankDeposit
	{
		public string Bank_Deposit_Pk;
		public string Bank_Pk;
		public string Category;
		public string Type;
		public string Type_Pk;
		public string Description;
		public string Monetary_Unit;
		public string Price;
		public string Price_Remain;
		public string DateTime;
		public string Registerd;
	}


}

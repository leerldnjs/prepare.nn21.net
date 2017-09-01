using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Components
{
    public class GetQuery
    {
		public String TransportHead(sTransportHead THead) {
			Dictionary<string, string> Data = new Dictionary<string, string>();
			Data.Add("TRANSPORT_PK", THead.Transport_Head_Pk);
			Data.Add("TRANSPORT_WAY", THead.Transport_Way);
			Data.Add("TRANSPORT_STATUS", THead.Transport_Status);
			Data.Add("BRANCHPK_FROM", THead.BranchPk_From);
			Data.Add("BRANCHPK_TO", THead.BranchPk_To);
			Data.Add("WAREHOUSE_PK_ARRIVAL", THead.Warehouse_Pk_Arrival);
			Data.Add("AREA_FROM", THead.Area_From);
			Data.Add("AREA_TO", THead.Area_To);
			Data.Add("DATETIME_FROM", THead.DateTime_From);
			Data.Add("DATETIME_TO", THead.DateTime_To);
			Data.Add("TITLE", THead.Title);
			Data.Add("VESSELNAME", THead.VesselName);
			Data.Add("VOYAGE_NO", THead.Voyage_No);
			Data.Add("VALUE_STRING_0", THead.Value_String_0);
			Data.Add("VALUE_STRING_1", THead.Value_String_1);
			Data.Add("VALUE_STRING_2", THead.Value_String_2);
			Data.Add("VALUE_STRING_3", THead.Value_String_3);
			Data.Add("VALUE_STRING_4", THead.Value_String_4);
			Data.Add("VALUE_STRING_5", THead.Value_String_5);
			string query = new Utility().GetQuery("TRANSPORT_HEAD", Data);
			return query;
		}

		public String TransportPacked(sTransportPacked TPacked) {
			Dictionary<string, string> Data = new Dictionary<string, string>();
			Data.Add("TRANSPORT_PACKED_PK", TPacked.Transport_Packed_Pk);
			Data.Add("SEQ", TPacked.Seq);
			Data.Add("WAREHOUSE_PK", TPacked.WareHouse_Pk);
			Data.Add("TRANSPORT_HEAD_PK", TPacked.Transport_Head_Pk);
			Data.Add("COMPANY_PK_OWNER", TPacked.Company_Pk_Owner);
			Data.Add("CONTAINER_COMPANY", TPacked.Container_Company);
			Data.Add("TYPE", TPacked.Type);
			Data.Add("NO", TPacked.No);
			Data.Add("SIZE", TPacked.Size);
			Data.Add("SEAL_NO", TPacked.Seal_No);
			Data.Add("REALPACKED_FLAG", TPacked.RealPacked_Flag);
			string query = new Utility().GetQuery("TRANSPORT_PACKED", Data);
			return query;
		}

		public String Comment(sComment Comment) {
			Dictionary<string, string> Data = new Dictionary<string, string>();
			Data.Add("COMMENT_PK", Comment.Comment_Pk);
			Data.Add("TABLE_NAME", Comment.Table_Name);
			Data.Add("TABLE_PK", Comment.Table_Pk);
			Data.Add("CATEGORY", Comment.Category);
			Data.Add("CONTENTS", Comment.Contents);
			Data.Add("ACCOUNT_PK", Comment.Account_Pk);
			Data.Add("ACCOUNT_ID", Comment.Account_Id);
			Data.Add("ACCOUNT_NAME", Comment.Account_Name);
			string query = new Utility().GetQuery("COMMENT", Data);
			return query;
		}

		public String Document(sDocument Document) {
			Dictionary<string, string> Data = new Dictionary<string, string>();
			Data.Add("DocumentPk", Document.DocumentPk);
			Data.Add("Type", Document.Type);
			Data.Add("TypePk", Document.TypePk);
			Data.Add("Status", Document.Status);
			Data.Add("Value0", Document.Value0);
			Data.Add("Value1", Document.Value1);
			Data.Add("Value2", Document.Value2);
			Data.Add("Value3", Document.Value3);
			Data.Add("Value4", Document.Value4);
			Data.Add("Value5", Document.Value5);
			Data.Add("Value6", Document.Value6);
			Data.Add("Value7", Document.Value7);
			Data.Add("Value8", Document.Value8);
			Data.Add("Value9", Document.Value9);
			Data.Add("Value10", Document.Value10);
			Data.Add("Value11", Document.Value11);
			Data.Add("Value12", Document.Value12);
			Data.Add("Value13", Document.Value13);
			Data.Add("Value14", Document.Value14);
			Data.Add("Value15", Document.Value15);
			Data.Add("Value16", Document.Value16);
			Data.Add("Value17", Document.Value17);
			Data.Add("Value18", Document.Value18);
			Data.Add("Value19", Document.Value19);
			Data.Add("ValueInt0", Document.ValueInt0);
			Data.Add("ValueDecimal0", Document.ValueDecimal0);
			Data.Add("ValueDecimal1", Document.ValueDecimal1);
			Data.Add("ValueDecimal2", Document.ValueDecimal2);
			Data.Add("ParentsType", Document.ParentsType);
			Data.Add("ParentsId", Document.ParentsId);
			string query = new Utility().GetQuery("Document", Data);
			return query;
		}


		public String DbSetTransportHead(string Gubun, sTransportHead THead)
		{
			if (Gubun == "INSERT")
			{
				return "INSERT INTO [dbo].[TRANSPORT_HEAD] ([TRANSPORT_WAY] ,[TRANSPORT_STATUS] ,[BRANCHPK_FROM] ,[BRANCHPK_TO] ,[WAREHOUSE_PK_ARRIVAL] ,[AREA_FROM] ,[AREA_TO] ,[DATETIME_FROM] ,[DATETIME_TO] ,[TITLE] ,[VESSELNAME] ,[VOYAGE_NO] ,[VALUE_STRING_0] ,[VALUE_STRING_1] ,[VALUE_STRING_2] ,[VALUE_STRING_3] ,[VALUE_STRING_4] ,[VALUE_STRING_5]) VALUES (" +
					Common.StringToDB(THead.Transport_Way, true, false) + ", " +
					Common.StringToDB(THead.Transport_Status, false, false) + ", " +
					Common.StringToDB(THead.BranchPk_From, false, false) + ", " +
					Common.StringToDB(THead.BranchPk_To, false, false) + ", " +
					Common.StringToDB(THead.Warehouse_Pk_Arrival, false, false) + ", " +
					Common.StringToDB(THead.Area_From, true, true) + ", " +
					Common.StringToDB(THead.Area_To, true, true) + ", " +
					Common.StringToDB(THead.DateTime_From, true, false) + ", " +
					Common.StringToDB(THead.DateTime_To, true, false) + ", " +
					Common.StringToDB(THead.Title, true, true) + ", " +
					Common.StringToDB(THead.VesselName, true, true) + ", " +
					Common.StringToDB(THead.Voyage_No, true, false) + ", " +
					Common.StringToDB(THead.Value_String_0, true, true) + ", " +
					Common.StringToDB(THead.Value_String_1, true, true) + ", " +
					Common.StringToDB(THead.Value_String_2, true, true) + ", " +
					Common.StringToDB(THead.Value_String_3, true, true) + ", " +
					Common.StringToDB(THead.Value_String_4, true, true) + ", " +
					Common.StringToDB(THead.Value_String_5, true, true) + ");";
			}
			else
			{
				return "UPDATE [dbo].[TRANSPORT_HEAD] SET [TRANSPORT_HEAD_PK] = " + Common.StringToDB(THead.Transport_Head_Pk, false, false) +
					"[TRANSPORT_WAY] = " + Common.StringToDB(THead.Transport_Way, true, false) +
					"[TRANSPORT_STATUS] = " + Common.StringToDB(THead.Transport_Status, false, false) + ", " +
					"[BRANCHPK_FROM] = " + Common.StringToDB(THead.BranchPk_From, false, false) + ", " +
					"[BRANCHPK_TO] = " + Common.StringToDB(THead.BranchPk_To, false, false) + ", " +
					"[WAREHOUSE_PK_ARRIVAL] = " + Common.StringToDB(THead.Warehouse_Pk_Arrival, false, false) + ", " +
					"[AREA_FROM] = " + Common.StringToDB(THead.Area_From, true, true) + ", " +
					"[AREA_TO] = " + Common.StringToDB(THead.Area_To, true, true) + ", " +
					"[DATETIME_FROM] = " + Common.StringToDB(THead.DateTime_From, true, false) + ", " +
					"[DATETIME_TO] = " + Common.StringToDB(THead.DateTime_To, true, false) + ", " +
					"[TITLE] = " + ", " + Common.StringToDB(THead.Title, true, true) + ", " +
					"[VESSELNAME] = " + Common.StringToDB(THead.VesselName, true, true) + ", " +
					"[VOYAGE_NO] = " + Common.StringToDB(THead.Voyage_No, true, false) + ", " +
					"[VALUE_STRING_0] = " + Common.StringToDB(THead.Value_String_0, true, true) + ", " +
					"[VALUE_STRING_1] = " + Common.StringToDB(THead.Value_String_1, true, true) + ", " +
					"[VALUE_STRING_2] = " + Common.StringToDB(THead.Value_String_2, true, true) + ", " +
					"[VALUE_STRING_3] = " + Common.StringToDB(THead.Value_String_3, true, true) + ", " +
					"[VALUE_STRING_4] = " + Common.StringToDB(THead.Value_String_4, true, true) + ", " +
					"[VALUE_STRING_5] = " + Common.StringToDB(THead.Value_String_5, true, true) + ";";
			}
		}
        public String DbSetTransportPacked(string Gubun, sTransportPacked TPacked) {
            if (Gubun == "INSERT") {
                return "INSERT INTO [dbo].[TRANSPORT_PACKED] ([TRANSPORT_HEAD_PK], [BRANCH_PK_OWNER], [TYPE], [NO], [SIZE], [SEAL_NO]) VALUES (" +
                    Common.StringToDB(TPacked.Transport_Head_Pk, false, false) + ", " +
                    Common.StringToDB(TPacked.Company_Pk_Owner, false, false) + ", " +
					Common.StringToDB(TPacked.Container_Company, true, false) + ", " +
					Common.StringToDB(TPacked.Type, true, false) + ", " +
                    Common.StringToDB(TPacked.No, true, false) + ", " +
                    Common.StringToDB(TPacked.Size, true, false) + ", " +
                    Common.StringToDB(TPacked.Seal_No, true, false) + ");";
            } else {
                return "UPDATE [dbo].[TRANSPORT_PACKED] SET [TRANSPORT_PACKED_PK] = " + Common.StringToDB(TPacked.Transport_Packed_Pk, false, false) + 
                    "[TRANSPORT_HEAD_PK] = " + Common.StringToDB(TPacked.Transport_Head_Pk, false, false) + ", " +
                    "[BRANCH_PK_OWNER] = " + Common.StringToDB(TPacked.Company_Pk_Owner, false, false) + ", " +
					"[CONTAINER_COMPANY] = " + Common.StringToDB(TPacked.Type, true, false) + ", " +
					"[TYPE] = " + Common.StringToDB(TPacked.Type, true, false) + ", " +
                    "[NO] = " + Common.StringToDB(TPacked.No, true, false) + ", " +
                    "[SIZE] = " + Common.StringToDB(TPacked.Size, true, false) + ", " +
                    "[SEAL_NO] = " + Common.StringToDB(TPacked.Seal_No, true, false) + ";";
            }
                
        }
        public String InsertCompanyAdditional(string CompanyPk, string Title, string Value) {
			return "INSERT INTO CompanyAdditionalInfomation ([CompanyPk], [Title], [Value]) VALUES (" + CompanyPk + ", " + Title + ", N'" + Value + "');";
		}

		public String InsertRequestForm(string ShipperPk, string AccountID, string ShipperCode, string MonetaryUnitCL, string StepCL) {
			Common C = new Common();
			ShipperCode = C.CheckNull(ShipperCode, true, false);
			return "INSERT INTO RequestForm ([ShipperPk], [AccountID], [ShipperCode], [MonetaryUnitCL], [StepCL], [RequestDate]) VALUES " +
																"(" + ShipperPk + ", '" + AccountID + "', " + ShipperCode + ", " + MonetaryUnitCL + " , " + StepCL + ", getDate()); " +
						"SELECT @@IDENTITY;";
		}
		public String UpdateOfferFormMonetaryUnit(string RequestFormPk, string MonetaryUnitCL) {
			Common C = new Common();
			return "UPDATE RequestForm SET [MonetaryUnitCL] = " + MonetaryUnitCL + " WHERE RequestFormPk=" + RequestFormPk + ";";
		}
		public String SelectItemLibrary(string CompanyPk) {
			return "SELECT IL.ItemLibraryPk, IL.Description, IL.StyleNo, IL.Material, IL.MonetaryUnitCL, IL.UnitCost, IL.NetWeight, IL.Width, IL.Depth, IL.Height, IL.AdditionalValue1, IL.AdditionalValue2, IL.Memo, " +
									" S.FileName " +
						"FROM ItemLibrary as IL LEFT OUTER JOIN  ItemSajin as S " +
									" ON IL.ItemLibraryPk=S.ItemLibraryPk " +
						"WHERE IL.CompanyPk=" + CompanyPk + " and IL.IsAlive=1 and (S.IsAlive=1 or isnull(S.IsAlive, 1)=1)" +
						"ORDER BY IL.Description ASC, IL.Registerd DESC";
		}
		public String InsertItemLibrary(string CompanyPk, string Description, string StyleNo, string Material, string UnitCost, string NetWeight, string Width, string Depth, string Height, string AdditionalValue1, string AdditionalValue2, string Memo, string MonetaryUnit) {
			Common C = new Common();
			return " INSERT INTO ItemLibrary (" +
							" [CompanyPk], [Description], [StyleNo], [Material], [MonetaryUnitCL], [UnitCost], [NetWeight], [Width], [Depth], [Height], " +
							"[AdditionalValue1], [AdditionalValue2], [Memo], [Registerd], [IsAlive])" +
					  " VALUES" +
							" (" + C.CheckNull(CompanyPk, false, false) + ", " + C.CheckNull(Description, true, true) + ", " + C.CheckNull(StyleNo, true, true) + ", " + C.CheckNull(Material, true, true) + ", " +
							C.CheckNull(MonetaryUnit, false, false) + ", " + C.CheckNull(UnitCost, false, false) + ", " + C.CheckNull(NetWeight, false, false) + ", " + C.CheckNull(Width, false, false) + ", " +
							C.CheckNull(Depth, false, false) + ", " + C.CheckNull(Height, false, false) + ", " + C.CheckNull(AdditionalValue1, true, true) + ", " + C.CheckNull(AdditionalValue2, true, true) + ", " +
							C.CheckNull(Memo, true, true) +
							", getDate(), 1);" +
					  " SELECT @@IDENTITY;";
		}
		public String LoadDocuments(string RequestFormPk, char Type) {
			switch (Type) {
				default:
					return "EXECUTE SP_SelectCommercialInvoice @RequestFormPk=" + RequestFormPk;
			}
		}
		public String UpdateRequestFormForOffer(string Gubun, string RequestFormPk, string MonetaryUnit, string StepCL, string CustomerListPk, string ConsigneeCode, string ConsigneePk, string SailingOn, string PortOfLanding, string FilnalDestination, string paymentWho, string NotifyParty, string ShipperName, string NotifyPartyAddress, string DateNNoofInvoice, string BuyerNOtherReferences) {
			MonetaryUnit = Gubun == "C" ? ", [MonetaryUnitCL] = " + Common.StringToDB(MonetaryUnit, false, false) : string.Empty;

			return "UPDATE RequestForm SET [ConsigneePk] = " + Common.StringToDB(ConsigneePk, false, false) +
															  ", [ConsigneeCCLPk] = " + Common.StringToDB(CustomerListPk, false, false) +
															  ", [CompanyInDocumentPk] = " + Common.StringToDB(ShipperName, false, false) +
															  ", [ConsigneeCode] = " + Common.StringToDB(ConsigneeCode, true, false) +
															  ", [DepartureDate] = " + Common.StringToDB(SailingOn, true, false) +
															  ", [DepartureRegionCode] = " + Common.StringToDB(PortOfLanding, true, false) +
															  ", [ArrivalRegionCode] = " + Common.StringToDB(FilnalDestination, true, false) +
															  ", [ShipperStaffName] = " + Common.StringToDB(DateNNoofInvoice, true, true) +
															  ", [PaymentWayCL] = " + Common.StringToDB(paymentWho, false, false) +
															  MonetaryUnit +
															  ", [StepCL] = " + Common.StringToDB(StepCL, false, false) +
															  ", [NotifyPartyName] = " + Common.StringToDB(NotifyParty, true, true) +
															  ", [NotifyPartyAddress] = " + Common.StringToDB(NotifyPartyAddress, true, true) +
															  ", [Memo] = " + Common.StringToDB(BuyerNOtherReferences, true, true) +
														" WHERE RequestFormPk=" + RequestFormPk + ";";
		}
		public String InsertRequestFormForOffer(string Gubun, string SessionPk, string ShipperCode, string accountID, string MonetaryUnit, string StepCL, string CustomerListPk, string ConsigneeCode, string ConsigneePk, string SailingOn, string PortOfLanding, string FilnalDestination, string paymentWho, string NotifyParty, string ShipperName, string NotifyPartyAddress, string DateNNoofInvoice, string BuyerNOtherReferences) {
			return " declare @return int " +
					   " EXECUTE @return=SP_InsertRequestForm  @ShipperPk=" + Common.StringToDB(SessionPk, false, false) +
																		", @ConsigneePk=" + Common.StringToDB(ConsigneePk, false, false) +
																		", @AccountID=" + Common.StringToDB(accountID, true, true) +
																		", @ConsigneeCCLPk=" + Common.StringToDB(CustomerListPk, false, false) +
																		", @ShipperCode=" + Common.StringToDB(ShipperCode, true, false) +
																		", @CompanyInDocumentPk=" + Common.StringToDB(ShipperName, false, false) +
																		", @ConsigneeCode=" + Common.StringToDB(ConsigneeCode, true, false) +
																		", @DepartureDate=" + Common.StringToDB(SailingOn, true, false) +
																		", @DepartureRegionCode=" + Common.StringToDB(PortOfLanding, true, false) +
																		", @ArrivalRegionCode=" + Common.StringToDB(FilnalDestination, true, false) +
																		", @DepartureAreaBranchCode=" + Common.StringToDB(PortOfLanding, true, false) +
																		", @ShipperStaffName=" + Common.StringToDB(DateNNoofInvoice, true, true) +
																		", @PaymentWayCL=" + Common.StringToDB(paymentWho, false, false) +
																		", @MonetaryUnitCL=" + Common.StringToDB(MonetaryUnit, false, false) +
																		", @StepCL=" + Common.StringToDB(StepCL, false, false) +
																		", @NotifyPartyName=" + Common.StringToDB(NotifyParty, true, true) +
																		", @NotifyPartyAddress=" + Common.StringToDB(NotifyPartyAddress, true, true) +
																		", @Memo=" + Common.StringToDB(BuyerNOtherReferences, true, true) +
						" select @return ;";
		}

		public String InsertEmailSend(string GubunPk, string GubunCL, string SenderID, string ReceiverPk, string EmailFrom, string EmailTo, string Title, string Contents) {
			return "INSERT INTO EmailHistory ([GubunPk], [GubunCL], [SenderID], [ReceiverPk], [EmailFrom], [EmailTo], [Title], [Contents], [SendTime]) " +
													" VALUES (" + GubunPk + ", " + GubunCL + ", '" + SenderID + "', " + new Common().CheckNull(ReceiverPk, false, false) + ", '" + EmailFrom + "', '" + EmailTo + "', N'" + Title + "', N'" + Contents + "', Getdate());";
		}
		public String LoadOfferDocumentItems(string Gubun, string RequestFormPk) {
			switch (Gubun) {
				case "C":
					return "SELECT RequestFormItemsPk, ItemCode, MarkNNumber, Description, Label, Quantity, QuantityUnit, UnitPrice, Amount " +
								" FROM RequestFormItems " +
								" where RequestFormPk=" + RequestFormPk;
				default:
					return "1";
			}
		}
		public String UpdateOrInsertRequestFormItem(string Gubun, string RequestFormItemsPk, string ItemCode, string ItemName, string Brand, string Matarial, string Quantity, string QuantityUnit, string UnitCost, string Price) {
			Common C = new Common();
			if (Gubun == "Insert") {
				return "EXECUTE SP_InsertRequestFormItems @RequestFormPk=" + RequestFormItemsPk +
																				  ", @ItemCode=" + C.CheckNull(ItemCode, false, false) +
																				  ", @Description=" + C.CheckNull(ItemName, true, true) +
																				  ", @Label=" + C.CheckNull(Brand, true, true) +
																				  ", @Material=" + C.CheckNull(Matarial, true, true) +
																				  ", @Quantity=" + C.CheckNull(Quantity, false, false) +
																				  ", @QuantityUnit=" + C.CheckNull(QuantityUnit, false, false) +
																				  ", @UnitPrice=" + C.CheckNull(UnitCost, false, false) +
																				  ", @Amount=" + C.CheckNull(Price, false, false) + ";";
			} else {
				return "UPDATE RequestFormItems SET [ItemCode] =" + C.CheckNull(ItemCode, false, false) +
																		 ", [Description] =" + C.CheckNull(ItemName, true, true) +
																		 ", [Label] =" + C.CheckNull(Brand, true, true) +
																		 ", [Material] =" + C.CheckNull(Matarial, true, true) +
																		 ", [Quantity] =" + C.CheckNull(Quantity, false, false) +
																		 ", [QuantityUnit] =" + C.CheckNull(QuantityUnit, false, false) +
																		 ", [UnitPrice] =" + C.CheckNull(UnitCost, false, false) +
																		 ", [Amount] =" + C.CheckNull(Price, false, false) +
																	" WHERE [RequestFormItemsPk]=" + RequestFormItemsPk + ";";
			}
		}   //Commercial Invoice
		public String UpdateOrInsertRequestFormItem(string Gubun, string RequestFormItemsPk, string ItemCode, string BoxNo, string ItemName, string Brand, string Matarial, string Quantity, string QuantityUnit, string PackedCount, string PackingUnit, string Weight, string Volume) {
			Common C = new Common();
			if (Gubun == "Insert") {
				return "EXECUTE SP_InsertRequestFormItems @RequestFormPk=" + RequestFormItemsPk +
																				  ", @ItemCode=" + C.CheckNull(ItemCode, false, false) +
																				  ", @MarkNNumber=" + C.CheckNull(BoxNo, true, false) +
																				  ", @Description=" + C.CheckNull(ItemName, true, true) +
																				  ", @Label=" + C.CheckNull(Brand, true, true) +
																				  ", @Material=" + C.CheckNull(Matarial, true, true) +
																				  ", @Quantity=" + C.CheckNull(Quantity, false, false) +
																				  ", @QuantityUnit=" + C.CheckNull(QuantityUnit, false, false) +
																				  ", @PackedCount=" + C.CheckNull(PackedCount, false, false) +
																				  ", @PackingUnit=" + C.CheckNull(PackingUnit, false, false) +
																				  ", @GrossWeight=" + C.CheckNull(Weight, false, false) +
																				  ", @Volume=" + C.CheckNull(Volume, false, false) + ";";
			} else {
				return "UPDATE RequestFormItems SET [ItemCode] =" + C.CheckNull(ItemCode, false, false) +
																		 ", [MarkNNumber] =" + C.CheckNull(BoxNo, true, false) +
																		 ", [Description] =" + C.CheckNull(ItemName, true, true) +
																		 ", [Label] =" + C.CheckNull(Brand, true, true) +
																		 ", [Material] =" + C.CheckNull(Matarial, true, true) +
																		 ", [Quantity] =" + C.CheckNull(Quantity, false, false) +
																		 ", [QuantityUnit] =" + C.CheckNull(QuantityUnit, false, false) +
																		 ", [PackedCount] =" + C.CheckNull(PackedCount, false, false) +
																		 ", [PackingUnit]=" + C.CheckNull(PackingUnit, false, false) +
																		 ", [GrossWeight] =" + C.CheckNull(Weight, false, false) +
																		 ", [Volume] =" + C.CheckNull(Volume, false, false) +
																	" WHERE [RequestFormItemsPk]=" + RequestFormItemsPk + ";";
			}
		}
		public String UpdateOrInsertRequestFormItem(string Gubun, string Pk, string ItemCode, string BoxNo, string ItemName, string Brand, string Matarial, string Quantity, string QuantityUnit, string PackedCount, string PackingUnit, string Weight, string Volume, string UnitPrice, string Amount) {
			if (Gubun == "Insert") {
				return "EXECUTE SP_InsertRequestFormItems @RequestFormPk=" + Pk +
																				  ", @ItemCode=" + Common.StringToDB(ItemCode, false, false) +
																				  ", @MarkNNumber=" + Common.StringToDB(BoxNo, true, false) +
																				  ", @Description=" + Common.StringToDB(ItemName, true, true) +
																				  ", @Label=" + Common.StringToDB(Brand, true, true) +
																				  ", @Material=" + Common.StringToDB(Matarial, true, true) +
																				  ", @Quantity=" + Common.StringToDB(Quantity, false, false) +
																				  ", @QuantityUnit=" + Common.StringToDB(QuantityUnit, false, false) +
																				  ", @PackedCount=" + Common.StringToDB(PackedCount, false, false) +
																				  ", @PackingUnit=" + Common.StringToDB(PackingUnit, false, false) +
																				  ", @GrossWeight=" + Common.StringToDB(Weight, false, false) +
																				  ", @Volume=" + Common.StringToDB(Volume, false, false) +
																				  ", @UnitPrice=" + Common.StringToDB(UnitPrice, false, false) +
																				  ", @Amount=" + Common.StringToDB(Amount, false, false) + ";";
			} else {
				return "UPDATE RequestFormItems SET ItemCode =" + Common.StringToDB(ItemCode, false, false) +
																		 ", MarkNNumber =" + Common.StringToDB(BoxNo, true, false) +
																		 ", Description =" + Common.StringToDB(ItemName, true, true) +
																		 ", Label =" + Common.StringToDB(Brand, true, true) +
																		 ", Material =" + Common.StringToDB(Matarial, true, true) +
																		 ", Quantity =" + Common.StringToDB(Quantity, false, false) +
																		 ", QuantityUnit =" + Common.StringToDB(QuantityUnit, false, false) +
																		 ", PackedCount =" + Common.StringToDB(PackedCount, false, false) +
																		 ", PackingUnit=" + Common.StringToDB(PackingUnit, false, false) +
																		 ", GrossWeight =" + Common.StringToDB(Weight, false, false) +
																		 ", Volume =" + Common.StringToDB(Volume, false, false) +
																		 ", UnitPrice=" + Common.StringToDB(UnitPrice, false, false) +
																		 ", Amount=" + Common.StringToDB(Amount, false, false) +
																	" WHERE [RequestFormItemsPk]=" + Pk + ";";
			}
		}
		public String AddRequestHistory(string RequestFormPk, string GubunCL, string AccountID, string Comment) {
			return "INSERT INTO [dbo].[HISTORY] ([TABLE_NAME], [TABLE_PK], [CODE], [ACCOUNT_ID], [DESCRIPTION]) VALUES ('RequestForm', " + RequestFormPk + ", '" + GubunCL + "', '" + AccountID + "', " + Common.StringToDB(Comment, true, true) + ");";
		}
		public String DeleteInsertRequestFormAdditionalInfo(string RequestFromPk, string GubunCL, string ActID, string Value) {
			return string.Format(@"DELETE FROM [dbo].[HISTORY] WHERE [TABLE_NAME] = 'RequestForm' AND [TABLE_PK]={0} AND [GubunCL]='{1}'
                                   INSERT INTO [dbo].[HISTORY] ([TABLE_NAME], [TABLE_PK],[CODE],[ACCOUNT_ID],[DESCRIPTION])
                                                                          VALUES('RequestForm', {0},{1},'{2}',N'{3}')", RequestFromPk, GubunCL, ActID, Value);
		}
		public String AddTransportBCHistory(string TransportBetweenCompanyPk, string GubunCL, string AccountID, string Comment) {

			if (Comment == "") {
				return "INSERT INTO TransportBCHistory (TransportBetweenCompanyPk, GubunCL, ActID, Registerd) VALUES (" + TransportBetweenCompanyPk + ", " + GubunCL + ", '" + AccountID + "', getDate());";
			} else {
				return "INSERT INTO TransportBCHistory (TransportBetweenCompanyPk, GubunCL, ActID, Comment, Registerd) VALUES (" + TransportBetweenCompanyPk + ", " + GubunCL + ", '" + AccountID + "', " + Common.StringToDB(Comment, true, true) + ", getDate());";
			}

		}

		public String LoadStandardPriceHead(string StandardPriceHeadPk) {
			// 이걸 where in 으로 해도 될까? 그게 순서대로 나와줄까? ㄷㄷ
			return "SELECT [Title], [Length], [B], [C], [D], [E], [F], [G], [H], [I], [J] " +
					  " FROM StandardPriceHead " +
					  " where StandardPriceHeadPk=" + StandardPriceHeadPk + ";";
		}
		public String LoadStandardPriceBody(string StandardPriceHeadPk, string CriterionValue) {
			return "SELECT [StandardPriceBodyPk], [A], [B], [C], [D], [E], [F], [G], [H], [I], [J] " +
					  "FROM StandardPriceBody " +
					  "where StandardPriceHeadPk=" + StandardPriceHeadPk + " and A>" + CriterionValue +
					  " order by [A];";
		}
		public String CountRequestFormItem(string RequestFormPk) {
			return "SELECT count(*) FROM RequestFormItems where RequestFormPk=" + RequestFormPk + ";";
		}
		public String DeleteRequestFormItem(string ItemsCount, string RequestFormItemsPk) {
			switch (ItemsCount) {
				case "0":
					return "DELETE FROM RequestFormItems WHERE RequestFormPk=" + RequestFormItemsPk + ";";

				case "1":
					return "DELETE FROM RequestForm WHERE RequestFormPk=" + RequestFormItemsPk + ";" +
								"DELETE FROM RequestFormItems WHERE RequestFormPk=" + RequestFormItemsPk + ";" +
								"DELETE FROM [dbo].[HISTORY] WHERE [TABLE_NAME] = 'RequestForm' AND [TABLE_PK]=" + RequestFormItemsPk + ";";
				default:
					return "DELETE FROM RequestFormItems WHERE RequestFormItemsPk=" + RequestFormItemsPk + ";";
			}
		}
		//화주문서페이지작업
		public String InsertDocumentForm(string ShipperPk, string AccountID, string ShipperCode, string MonetaryUnitCL, string StepCL, string TitleName, string Memo) {
			Common C = new Common();
			ShipperCode = C.CheckNull(ShipperCode, true, false);
			return "INSERT INTO DocumentForm ([ShipperPk], [AccountID], [ShipperCode], [MonetaryUnitCL], [StepCL], [DocumentDate], [TitleName], [Memo]) VALUES (" + ShipperPk + ", '" + AccountID + "', " + ShipperCode + ", " + MonetaryUnitCL + " , " + StepCL + ", getDate(), " + Common.StringToDB(TitleName, true, true) + "," + Common.StringToDB(Memo, true, true) + "); " +
						"SELECT @@IDENTITY;";
		}
		//화주문서페이지작업
		public String InsertDocumentFormForDocument(string Gubun, string SessionPk, string ShipperCode, string accountID, string MonetaryUnit, string StepCL, string CustomerListPk, string ConsigneeCode, string ConsigneePk, string SailingOn, string PortOfLanding, string FilnalDestination, string paymentWho, string NotifyParty, string ShipperName, string NotifyPartyAddress, string DateNNoofInvoice, string BuyerNOtherReferences) {
			return " declare @return int " +
					   " EXECUTE @return=SP_InsertDocumentForm  @ShipperPk=" + Common.StringToDB(SessionPk, false, false) +
															", @ConsigneePk=" + Common.StringToDB(ConsigneePk, false, false) +
															", @AccountID=" + Common.StringToDB(accountID, true, true) +
															", @ConsigneeCCLPk=" + Common.StringToDB(CustomerListPk, false, false) +
															", @ShipperCode=" + Common.StringToDB(ShipperCode, true, false) +
															", @CompanyClearancePk=" + Common.StringToDB(ShipperName, false, false) +
															", @ConsigneeCode=" + Common.StringToDB(ConsigneeCode, true, false) +
															", @DepartureDate=" + Common.StringToDB(SailingOn, true, false) +
															", @DepartureRegionCode=" + Common.StringToDB(PortOfLanding, true, false) +
															", @ArrivalRegionCode=" + Common.StringToDB(FilnalDestination, true, false) +
															", @DepartureAreaBranchCode=" + Common.StringToDB(PortOfLanding, true, false) +
															", @ShipperStaffName=" + Common.StringToDB(DateNNoofInvoice, true, true) +
															", @PaymentWayCL=" + Common.StringToDB(paymentWho, false, false) +
															", @MonetaryUnitCL=" + Common.StringToDB(MonetaryUnit, false, false) +
															", @StepCL=" + Common.StringToDB(StepCL, false, false) +
															", @NotifyPartyName=" + Common.StringToDB(NotifyParty, true, true) +
															", @NotifyPartyAddress=" + Common.StringToDB(NotifyPartyAddress, true, true) +
															", @Memo=" + Common.StringToDB(BuyerNOtherReferences, true, true) +
						" select @return ;";
		}
		//화주문서페이지작업
		public String UpdateOrInsertDocumentFormItem(string Gubun, string DocumentFormItemsPk, string ItemCode, string ItemName, string Brand, string Matarial, string Quantity, string QuantityUnit, string UnitCost, string Price) {
			Common C = new Common();
			if (Gubun == "Insert") {
				return "EXECUTE SP_InsertDocumentFormItems @DocumentFormPk=" + DocumentFormItemsPk +
													   ", @ItemCode=" + C.CheckNull(ItemCode, false, false) +
													   ", @Description=" + C.CheckNull(ItemName, true, true) +
													   ", @Label=" + C.CheckNull(Brand, true, true) +
													   ", @Material=" + C.CheckNull(Matarial, true, true) +
													   ", @Quantity=" + C.CheckNull(Quantity, false, false) +
													   ", @QuantityUnit=" + C.CheckNull(QuantityUnit, false, false) +
													   ", @UnitPrice=" + C.CheckNull(UnitCost, false, false) +
													   ", @Amount=" + C.CheckNull(Price, true, true) + ";";
			} else {
				return "UPDATE DocumentFormItems SET [ItemCode] =" + C.CheckNull(ItemCode, false, false) +
												 ", [Description] =" + C.CheckNull(ItemName, true, true) +
												 ", [Label] =" + C.CheckNull(Brand, true, true) +
												 ", [Material] =" + C.CheckNull(Matarial, true, true) +
												 ", [Quantity] =" + C.CheckNull(Quantity, false, false) +
												 ", [QuantityUnit] =" + C.CheckNull(QuantityUnit, false, false) +
												 ", [UnitPrice] =" + C.CheckNull(UnitCost, false, false) +
												 ", [Amount] =" + C.CheckNull(Price, true, true) +
											" WHERE [DocumentFormItemsPk]=" + DocumentFormItemsPk + ";";
			}
		}   //Commercial Invoice
			//화주문서페이지작업
		public String UpdateOrInsertDocumentFormItem(string Gubun, string DocumentFormItemsPk, string ItemCode, string BoxNo, string ItemName, string Brand, string Matarial, string Quantity, string QuantityUnit, string PackedCount, string PackingUnit, string Weight, string Volume) {
			Common C = new Common();
			if (Gubun == "Insert") {
				return "EXECUTE SP_InsertDocumentFormItems @DocumentFormPk=" + DocumentFormItemsPk +
													   ", @ItemCode=" + C.CheckNull(ItemCode, false, false) +
													   ", @MarkNNumber=" + C.CheckNull(BoxNo, true, false) +
													   ", @Description=" + C.CheckNull(ItemName, true, true) +
													   ", @Label=" + C.CheckNull(Brand, true, true) +
													   ", @Material=" + C.CheckNull(Matarial, true, true) +
													   ", @Quantity=" + C.CheckNull(Quantity, false, false) +
													   ", @QuantityUnit=" + C.CheckNull(QuantityUnit, false, false) +
													   ", @PackedCount=" + C.CheckNull(PackedCount, false, false) +
													   ", @PackingUnit=" + C.CheckNull(PackingUnit, false, false) +
													   ", @GrossWeight=" + C.CheckNull(Weight, false, false) +
													   ", @Volume=" + C.CheckNull(Volume, false, false) + ";";
			} else {
				return "UPDATE DocumentFormItems SET [ItemCode] =" + C.CheckNull(ItemCode, false, false) +
												 ", [MarkNNumber] =" + C.CheckNull(BoxNo, true, false) +
												 ", [Description] =" + C.CheckNull(ItemName, true, true) +
												 ", [Label] =" + C.CheckNull(Brand, true, true) +
												 ", [Material] =" + C.CheckNull(Matarial, true, true) +
												 ", [Quantity] =" + C.CheckNull(Quantity, false, false) +
												 ", [QuantityUnit] =" + C.CheckNull(QuantityUnit, false, false) +
												 ", [PackedCount] =" + C.CheckNull(PackedCount, false, false) +
												 ", [PackingUnit]=" + C.CheckNull(PackingUnit, false, false) +
												 ", [GrossWeight] =" + C.CheckNull(Weight, false, false) +
												 ", [Volume] =" + C.CheckNull(Volume, false, false) +
											" WHERE [DocumentFormItemsPk]=" + DocumentFormItemsPk + ";";
			}
		}
		//화주문서페이지작업
		public String UpdateOrInsertDocumentFormItem(string Gubun, string Pk, string ItemCode, string BoxNo, string ItemName, string Brand, string Matarial, string Quantity, string QuantityUnit, string PackedCount, string PackingUnit, string Weight, string Volume, string UnitPrice, string Amount) {
			if (Gubun == "Insert") {
				return "EXECUTE SP_InsertDocumentFormItems @DocumentFormPk=" + Pk +
													   ", @ItemCode=" + Common.StringToDB(ItemCode, false, false) +
													   ", @MarkNNumber=" + Common.StringToDB(BoxNo, true, false) +
													   ", @Description=" + Common.StringToDB(ItemName, true, true) +
													   ", @Label=" + Common.StringToDB(Brand, true, true) +
													   ", @Material=" + Common.StringToDB(Matarial, true, true) +
													   ", @Quantity=" + Common.StringToDB(Quantity, false, false) +
													   ", @QuantityUnit=" + Common.StringToDB(QuantityUnit, false, false) +
													   ", @PackedCount=" + Common.StringToDB(PackedCount, false, false) +
													   ", @PackingUnit=" + Common.StringToDB(PackingUnit, false, false) +
													   ", @GrossWeight=" + Common.StringToDB(Weight, false, false) +
													   ", @Volume=" + Common.StringToDB(Volume, false, false) +
													   ", @UnitPrice=" + Common.StringToDB(UnitPrice, false, false) +
													   ", @Amount=" + Common.StringToDB(Amount, true, true) + ";";
			} else {
				return "UPDATE DocumentFormItems SET ItemCode =" + Common.StringToDB(ItemCode, false, false) +
												 ", MarkNNumber =" + Common.StringToDB(BoxNo, true, false) +
												 ", Description =" + Common.StringToDB(ItemName, true, true) +
												 ", Label =" + Common.StringToDB(Brand, true, true) +
												 ", Material =" + Common.StringToDB(Matarial, true, true) +
												 ", Quantity =" + Common.StringToDB(Quantity, false, false) +
												 ", QuantityUnit =" + Common.StringToDB(QuantityUnit, false, false) +
												 ", PackedCount =" + Common.StringToDB(PackedCount, false, false) +
												 ", PackingUnit=" + Common.StringToDB(PackingUnit, false, false) +
												 ", GrossWeight =" + Common.StringToDB(Weight, false, false) +
												 ", Volume =" + Common.StringToDB(Volume, false, false) +
												 ", UnitPrice=" + Common.StringToDB(UnitPrice, false, false) +
												 ", Amount=" + Common.StringToDB(Amount, true, true) +
										 " WHERE [DocumentFormItemsPk]=" + Pk + ";";
			}
		}
		//화주문서페이지작업
		public String UpdateOrInsertTradingScheduleItems(string Gubun, string TradingScheduleHeadPk, string Date, string Description, string Volume, string Quantity, string Price, string Amount, string Tax) {
			if (Gubun == "Insert") {
				return "EXECUTE SP_InsertTradingScheduleItems @TradingScheduleHeadPk=" + Common.StringToDB(TradingScheduleHeadPk, false, false) +
																						 ",@Date=" + Common.StringToDB(Date, true, true) +
																						 ",@Description=" + Common.StringToDB(Description, true, true) +
																						 ",@Volume=" + Common.StringToDB(Volume, true, true) +
																						 ",@Quantity=" + Common.StringToDB(Quantity, true, true) +
																						 ",@Price=" + Common.StringToDB(Price, true, true) +
																						 ",@Amount=" + Common.StringToDB(Amount, true, true) +
																						 ",@Tax=" + Common.StringToDB(Tax, true, true) + ";";
			} else {
				return "UPDATE SP_InsertTradingScheduleItems SET Date=" + Common.StringToDB(Date, true, true) +
																				",Description=" + Common.StringToDB(Description, true, true) +
																				",Volume=" + Common.StringToDB(Volume, true, true) +
																				",Quantity=" + Common.StringToDB(Quantity, true, true) +
																				",Price=" + Common.StringToDB(Price, true, true) +
																				",Amount=" + Common.StringToDB(Amount, true, true) +
																				",Tax=" + Common.StringToDB(Tax, true, true) +
										 " WHERE [TradingScheduleItemsPk]=" + TradingScheduleHeadPk + ";";
			}
		}


	}
}
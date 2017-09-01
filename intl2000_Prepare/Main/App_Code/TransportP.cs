using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

/// <summary>
/// TransportP의 요약 설명입니다.
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
[System.Web.Script.Services.ScriptService]
public class TransportP : System.Web.Services.WebService
{
    public TransportP() {
			
        //디자인된 구성 요소를 사용하는 경우 다음 줄의 주석 처리를 제거합니다. 
        //InitializeComponent(); 
    }

	[WebMethod]
	public string TransportToHead(string HeadPk, string FromType, string FromTypePk_s, string Count_s) {
		string[] arrFromTypePk = FromTypePk_s.Split(new string[] { ",!" }, StringSplitOptions.None);
		string[] arrCount = Count_s.Split(new string[] { ",!" }, StringSplitOptions.None);
		string BodyPk;
		string TempPackedPk;
		StringBuilder Query = new StringBuilder();

		TransportC TransC = new TransportC();
		DBConn DB = new DBConn();
		DB.DBCon.Open();

		if (FromType == "Packed") {
			for (int i = 0; i < arrFromTypePk.Length; i++) {
				Query.Append("UPDATE [dbo].[TRANSPORT_PACKED] SET [TRANSPORT_HEAD_PK] = " + HeadPk + " WHERE [TRANSPORT_PACKED_PK] = " + arrFromTypePk[i] + " ;");
				Query.Append("UPDATE [dbo].[TRANSPORT_BODY] SET [TRANSPORT_HEAD_PK] = " + HeadPk + " WHERE [TRANSPORT_PACKED_PK] = " + arrFromTypePk[i] + " ;");
				DB.SqlCmd.CommandText = Query + "";
				DB.SqlCmd.ExecuteNonQuery();
			}
		}
		else if (FromType == "Storage") {
			for (int i = 0; i < arrFromTypePk.Length; i++) {
				int Count = Int32.Parse(arrCount[i]);
				BodyPk = TransC.Find_BodyPk(arrFromTypePk[i], "Head", HeadPk, ref DB);
				TransC.StorageAddCount(arrFromTypePk[i], Count * -1, ref DB);
				TransC.BodyAddCount(BodyPk, Count, ref DB);
				TempPackedPk = TransC.Find_PackedPk(HeadPk, BodyPk, ref DB);
				Query.Append("UPDATE [dbo].[TRANSPORT_PACKED] SET [TRANSPORT_HEAD_PK] = " + HeadPk + " WHERE [TRANSPORT_PACKED_PK] = " + TempPackedPk + " ;");
				Query.Append("UPDATE [dbo].[TRANSPORT_BODY] SET [TRANSPORT_HEAD_PK] = " + HeadPk + ", [TRANSPORT_PACKED_PK] = " + TempPackedPk + " WHERE [TRANSPORT_BODY_PK] = " + BodyPk + " ;");
				DB.SqlCmd.CommandText = Query + "";
				DB.SqlCmd.ExecuteNonQuery();
			}
		}

		
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public string TransportToPacked(string PackedPk, string FromType, string FromTypePk_s, string Count_s) {
		string[] arrFromTypePk = FromTypePk_s.Split(new string[] { ",!" }, StringSplitOptions.None);
		string[] arrCount = Count_s.Split(new string[] { ",!" }, StringSplitOptions.None);
		string BodyPk;
		StringBuilder Query = new StringBuilder();

		TransportC TransC = new TransportC();
		DBConn DB = new DBConn();
		DB.DBCon.Open();

		if (FromType == "Storage") {
			for (var i = 0; i < arrFromTypePk.Length; i++) {
				int count = Int32.Parse(arrCount[i]);
				BodyPk = TransC.Find_BodyPk(arrFromTypePk[i], "Packed", PackedPk, ref DB);
				TransC.StorageAddCount(arrFromTypePk[i], count * -1, ref DB);
				TransC.BodyAddCount(BodyPk, count, ref DB);
				Query.Append("UPDATE [dbo].[TRANSPORT_BODY] SET [TRANSPORT_PACKED_PK] = " + PackedPk + " WHERE [TRANSPORT_BODY_PK] = " + BodyPk + " ;");
			}
		}
		else if(FromType == "Packed") {// Head.Packed -> Wait.Packed
			for (var i = 0; i < arrFromTypePk.Length; i++) {
				Query.Append("UPDATE [dbo].[TRANSPORT_PACKED] SET [TRANSPORT_HEAD_PK] = NULL " + " WHERE [TRANSPORT_PACKED_PK] = " + arrFromTypePk[i] + " ;");
				Query.Append("UPDATE [dbo].[TRANSPORT_BODY] SET [TRANSPORT_HEAD_PK] = NULL " + " WHERE [TRANSPORT_PACKED_PK] = " + arrFromTypePk[i] + " ;");
			}
				
		}

		DB.SqlCmd.CommandText = Query + "";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public string TransportToStorage(string WarehousePk, string FromType, string FromTypePk_s, string Count_s) {
		string[] arrFromTypePk = FromTypePk_s.Split(new string[] { ",!" }, StringSplitOptions.None);
		string[] arrCount = Count_s.Split(new string[] { ",!" }, StringSplitOptions.None);
		string StoragePk;
		string TempPackedPk = "";
		StringBuilder Query = new StringBuilder();

		TransportC TransC = new TransportC();
		DBConn DB = new DBConn();
		DB.DBCon.Open();

		if (FromType == "Head") {
			for (int i = 0; i < arrFromTypePk.Length; i++) {
				int Count = Int32.Parse(arrCount[i]);
				StoragePk = TransC.Find_StoragePk(arrFromTypePk[i], "Warehouse", WarehousePk, ref DB);
				TransC.BodyAddCount(arrFromTypePk[i], Count * -1, ref DB);
				TransC.StorageAddCount(StoragePk, Count, ref DB);
				Query.Append("UPDATE [dbo].[STORAGE] SET [WAREHOUSE_PK] = " + WarehousePk + " WHERE [STORAGE_PK] = " + StoragePk + " ;");
			}
		} else if (FromType == "Packed") {
			for (int i = 0; i < arrFromTypePk.Length; i++) {
				if (TempPackedPk == "") {
					TempPackedPk = TransC.Check_TempPacked(arrFromTypePk[i], ref DB);
				}
				int Count = Int32.Parse(arrCount[i]);
				StoragePk = TransC.Find_StoragePk(arrFromTypePk[i], "Warehouse", WarehousePk, ref DB);
				TransC.BodyAddCount(arrFromTypePk[i], Count * -1, ref DB);
				TransC.StorageAddCount(StoragePk, Count, ref DB);
				Query.Append("UPDATE [dbo].[STORAGE] SET [WAREHOUSE_PK] = " + WarehousePk + " WHERE [STORAGE_PK] = " + StoragePk + " ;");
				Query.Append("DELETE FROM [dbo].[TRANSPORT_PACKED] WHERE [TRANSPORT_PACKED_PK] = " + TempPackedPk + ";");

			}
		} else if (FromType == "Storage") { //Storage Item의 창고이동
			for (int i = 0; i < arrFromTypePk.Length; i++) {
				int Count = Int32.Parse(arrCount[i]);
				StoragePk = TransC.Find_StoragePk(arrFromTypePk[i], "WarehouseToWarehouse", WarehousePk, ref DB);
				TransC.StorageAddCount(arrFromTypePk[i], Count * -1, ref DB);   //이동전 기존의 Warehouse StoragePk -
				TransC.StorageAddCount(StoragePk, Count, ref DB);			//이동후 새로운 Warehouse StoragePk +
				Query.Append("UPDATE [dbo].[STORAGE] SET [WAREHOUSE_PK] = " + WarehousePk + " WHERE [STORAGE_PK] = " + StoragePk + " ;");
			}
		} 

		DB.SqlCmd.CommandText = Query + "";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public string TransportEditStorage(string StoragePk, string EditCount) {
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"UPDATE [dbo].[STORAGE] SET [PACKED_COUNT] = " + EditCount + @" WHERE [STORAGE_PK] = " + StoragePk;
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
    public string MakeHtml_TransportHistory(string BranchPk, string Category) {
        string RetVal = "";
        var Add_Value0 = "";
        var Add_Value1 = "";
        var Add_Value2 = "";
		var Add_Value3 = "";
		var Add_Value4 = "";
		var StartCommVar = "<table class=\"table table-hover\"><thead>";
        var HtmlData = "";
        var Code = "";
        int Add_Count = 0;

        switch (Category) {
            case "Air":
                Code = "Air_VesselName";
                break;
            case "Car":
                Code = "Car_VesselName";
                break;
            case "Ship":
                Code = "Ship_VesselName";
                break;
            case "Sub":
                Code = "Sub_VesselName";
                break;
            case "FromReg":
                Code = "Area_From";
                break;
            case "ToReg":
                Code = "Area_To";
                break;
        }	

        DBConn DB = new DBConn();
        DB.DBCon.Open();
        Setting ls = new Setting();
        SqlDataReader SDR = ls.Load_Saved(BranchPk, Code, ref DB);
        List<string[]> RS = new List<string[]>();

        switch (Category) {
            case "Air":
                Add_Value0 = "항공명";
                Add_Count = 1;
                while (SDR.Read()) {
                    RS.Add(new string[] { SDR["SAVED_PK"] + "", SDR["VALUE_0"] +"" });
                }
                RetVal += StartCommVar + "<tr><th>" + Add_Value0 + "</th></tr></thead><tbody>";
                break;
            case "Car":
				Add_Value0 = "차량회사";
				Add_Value1 = "기사명";
                Add_Value2 = "기사전화번호";
                Add_Value3 = "차번호";
                Add_Value4 = "차사이즈";
                Add_Count = 5;
                while (SDR.Read()) {
                    RS.Add(new string[] { SDR["SAVED_PK"] + "", SDR["VALUE_0"] + "", SDR["VALUE_1"] + "", SDR["VALUE_2"] + "", SDR["VALUE_3"] + "", SDR["VALUE_4"] + "" });
                }
                RetVal += StartCommVar + "<tr><th>" + Add_Value0 + "</th><th>" + Add_Value1 + "</th><th>" + Add_Value2 + "</th>><th>" + Add_Value3 + "</th><th>" + Add_Value4 + "</th></tr></thead><tbody>";
                break;
            case "Ship":
				Add_Value0 = "선박회사";
				Add_Value1 = "배이름";
				Add_Count = 2;
                while (SDR.Read()) {
                    RS.Add(new string[] { SDR["SAVED_PK"] + "", SDR["VALUE_0"] + "", SDR["VALUE_1"] + "" });
                }
                RetVal += StartCommVar + "<tr><th>" + Add_Value0 + "</th><th>" + Add_Value1 + "</th></thead><tbody>";
                break;
            case "Sub":
                Add_Value0 = "업체명";
                Add_Value1 = "출발지연락처";
                Add_Value2 = "도착지연락처";
                Add_Count = 3;
                while (SDR.Read()) {
                    RS.Add(new string[] { SDR["SAVED_PK"] + "", SDR["VALUE_0"] + "", SDR["VALUE_1"] + "", SDR["VALUE_2"] + "" });
                }
                RetVal += StartCommVar + "<tr><th>" + Add_Value0 + "</th><th>" + Add_Value1 + "</th>><th>" + Add_Value2 + "</th></tr></thead><tbody>";
                break;
            case "FromReg":
                Add_Value0 = "출발지";
                Add_Count = 1;
                while (SDR.Read()) {
                    RS.Add(new string[] { SDR["SAVED_PK"] + "", SDR["VALUE_0"] + "" });
                }
                RetVal += StartCommVar + "<tr><th>" + Add_Value0 + "</th></tr></thead><tbody>";
                break;
            case "ToReg":
                Add_Value0 = "도착지";
                Add_Count = 1;
                while (SDR.Read()) {
                    RS.Add(new string[] { SDR["SAVED_PK"] + "", SDR["VALUE_0"] + "" });
                }
                RetVal += StartCommVar + "<tr><th>" + Add_Value0 + "</th></tr></thead><tbody>";
                break;
        }
        int j = 0 ;
        foreach (string[] each in RS) {
            HtmlData += "<tr onclick=\"GetHistory('" + string.Join(",!", each) + "', " + "'" +  Category + "')\";>";
            for (j = 1; j < each.Length; j++) {
                HtmlData += "<td style=\"text-align:center; padding:10px;\" >" + each[j] + "</td>";
            }
            HtmlData += "</tr>"; 
        };
        HtmlData += "<table style=\"vertical-align:middle;\"><tr>";
        for (int x = 0; x < Add_Count; x++) {
            HtmlData += "<td style=\"vertical-align:middle; padding:2px;\" ><input type=\"text\" class=\"form-control\" style=\"width:95px; vertical-align:middle;\" id =\"DirectSet" + x + "\"></td>";
        }
        HtmlData += "</tr></ table>";
        RetVal += HtmlData + "</tbody></table>" + "<div class=\"form-group\"><div class=\"col-xs-4 col-xs-offset-4\"><button class=\"btn btn-primary btn-md btn-block\" id=\"BTN_DirectSubmit\"type=\"button\" onclick=\"GetHistory('" + "Direct" + "', " + "'" + Category + "')\";>확인</button></div></div>";

        SDR.Close();
        DB.DBCon.Close();

        return RetVal;
    }
    [WebMethod]
    public string Set_TransportHead(string Transport_Head_Pk, string Transport_Way, string Transport_Status, string BranchPk_From, string BranchPk_To, string Area_From, string Area_To, string DateTime_From, string DateTime_To, string Title, string VesselName, string Voyage_No, string Value_String_0, string Value_String_1, string Value_String_2, string Value_String_3, string Value_String_4, string Value_String_5, string SavedPk_VesselName, string SavedPk_Area_From, string SavedPk_Area_To) {
        sTransportHead th = new sTransportHead();
        th.Transport_Head_Pk = Transport_Head_Pk;
        th.Transport_Way = Transport_Way;
        th.Transport_Status = Transport_Status;
        th.BranchPk_From = BranchPk_From;
        th.BranchPk_To = BranchPk_To;
        th.Area_From = Area_From;
        th.Area_To = Area_To;
        th.DateTime_From = DateTime_From;
        th.DateTime_To = DateTime_To;
        th.Title = Title;
        th.VesselName = VesselName;
        th.Voyage_No = Voyage_No;
        th.Value_String_0 = Value_String_0;
        th.Value_String_1 = Value_String_1;
        th.Value_String_2 = Value_String_2;
        th.Value_String_3 = Value_String_3;
        th.Value_String_4 = Value_String_4;
        th.Value_String_5 = Value_String_5;

        TransportC setth = new TransportC();
        DBConn DB = new DBConn();
        DB.DBCon.Open();
		if (Transport_Head_Pk != "") {
			HistoryC HisC = new HistoryC();
			sHistory History = new sHistory();
			History.Table_Name = "TRANSPORT_HEAD";
			History.Table_Pk = Transport_Head_Pk;
			History.Code = "TransportHead";
			History.Account_Id = "ilic32";
			History.Description = HisC.ComputeChanges_Head(th, ref DB);

			HisC.Set_History(History, ref DB);
		}
		setth.Set_TransportHead(th, ref DB);
        Setting setsv = new Setting();
        
        if (SavedPk_VesselName == "") {
            if (Transport_Way == "Air") {
                setsv.Set_Saved(th.BranchPk_From, "Air_VesselName", th.Title, "", "", "", "", "", ref DB);
            } 
            else if (Transport_Way == "Car") {
                setsv.Set_Saved(th.BranchPk_From, "Car_VesselName", th.Title, th.VesselName, Voyage_No, th.Value_String_1, th.Value_String_3, "", ref DB);
            }
            else if (Transport_Way == "Ship") {
                setsv.Set_Saved(th.BranchPk_From, "Ship_VesselName", th.Title, th.VesselName, "", "", "", "", ref DB);
            }
            else if (Transport_Way == "Sub") {
                setsv.Set_Saved(th.BranchPk_From, "Sub_VesselName", th.Title, th.Value_String_1, th.Value_String_2, "", "", "", ref DB);
            }
        }

        if (SavedPk_Area_From == "") {
            setsv.Set_Saved(th.BranchPk_From, "Area_From", th.Area_From, "", "", "", "", "", ref DB);
        }
        if (SavedPk_Area_To == "") {
            setsv.Set_Saved(th.BranchPk_From, "Area_To", th.Area_To, "", "", "", "", "", ref DB);
        }
		
        DB.DBCon.Close();
 
        return "1";
    }

	[WebMethod]
	public String LoadBranchStorage(string CompanyPk) {
		StringBuilder ReturnValue = new StringBuilder();
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT [OurBranchStoragePk], [StorageName] FROM OurBranchStorageCode WHERE OurBranchCode=" + CompanyPk + " and IsUse is null order by [StorageName] ;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		int RSCount = 0;
		while (RS.Read()) {
			ReturnValue.Append("<option value=\"" + RS[0] + "\">" + RS[1] + "</option>");
			RSCount++;
		}
		RS.Close();
		DB.DBCon.Close();
		if (RSCount == 1) {
			return "<select id=\"St_Storage\" class=\"form-control\" style=\"visibility:hidden;\">" + ReturnValue + "</select>";
		} else {
			return "<select id=\"St_Storage\" class=\"form-control\"><option value=\"0\">WAREHOUSE</option>" + ReturnValue + "</select>";
		}
	}

	/*
    [WebMethod]
    public string Set_TrarnsportBody(string Transport_Head_Pk, string Transport_Packed_Pk, string Owner_Company_Pk, string Owner_Company_Name, string Count, string Count_Unit, string Packed_Count, string Packing_Unit, string Description) {
        sTransportBody tb = new sTransportBody();
        tb.Transport_Body_Pk = "";
        tb.Transport_Head_Pk = Transport_Head_Pk;
        tb.Transport_Packed_Pk = Transport_Packed_Pk;
        tb.Owner_Company_Pk = Owner_Company_Pk;
        tb.Owner_Company_Name = Owner_Company_Name;
        tb.Count = Count;
        tb.Count_Unit = Count_Unit;
        tb.Packed_Count = Packed_Count;
        tb.Packing_Unit = Packing_Unit;
        tb.Description = Description;

        TransportC settb = new TransportC();
        DBConn DB = new DBConn();
        DB.DBCon.Open();
        settb.Set_TransportBody(tb, ref DB);
        DB.DBCon.Close();

        return "1";
    }
	*/
	[WebMethod]
    public string Set_TransportPacked(string Transport_Packed_Pk, string Seq, string WareHouse_Pk, string Transport_Head_Pk, string ContainerBranch_Own, string ContainerType, string ContainerNo, string ContainerSize, string SealNo, string RealPacked_Flag) {
        sTransportPacked ct = new sTransportPacked();
        ct.Transport_Packed_Pk = Transport_Packed_Pk;
		ct.Seq = Seq;
		ct.WareHouse_Pk = WareHouse_Pk;
		ct.Transport_Head_Pk = Transport_Head_Pk;
        ct.Company_Pk_Owner = ContainerBranch_Own;
        ct.Type = ContainerType;
        ct.No = ContainerNo;
        ct.Size = ContainerSize;
        ct.Seal_No = SealNo;
		ct.RealPacked_Flag = RealPacked_Flag;

		TransportC setct = new TransportC();
        Setting setsv = new Setting();
        DBConn DB = new DBConn();
        DB.DBCon.Open();
		
		if (Transport_Head_Pk != "") {
			HistoryC HisC = new HistoryC();
			sHistory History = new sHistory();
			History.Table_Name = "TRANSPORT_PACKED";
			History.Table_Pk = Transport_Packed_Pk;
			History.Code = "Packed";
			History.Account_Id = "ilic32";
			History.Description = HisC.ComputeChanges_Packed(ct, ref DB);

			HisC.Set_History(History, ref DB);
		}
		
		setct.Set_TransportPacked(ct, ref DB);

        DB.DBCon.Close();

        return "1";
    }


    /*
    [WebMethod]
    public sTransportHead Load_TrarnsportHead(string Transport_Head_Pk) {
        TransportC loadth = new TransportC();
        DBConn DB = new DBConn();
        DB.DBCon.Open();
        sTransportHead th = loadth.Load_TransportHead(Transport_Head_Pk, ref DB);
        DB.DBCon.Close();

        return th;
    }
    */
    [WebMethod]
    public List<sTransportBody> Load_TrarnsportBody(string Type, string TypePk) {
        TransportC TransC = new TransportC();
        DBConn DB = new DBConn();
        DB.DBCon.Open();
        List<sTransportBody> TBody = TransC.LoadList_TransportBody(Type, TypePk, ref DB);

		DB.DBCon.Close();

        return TBody;
    }
    [WebMethod]
    public List<sTransportPacked> Load_TrarnsportPacked(string Transport_Packed_Pk) {
        TransportC loadtp = new TransportC();
        DBConn DB = new DBConn();
        DB.DBCon.Open();
        List<sTransportPacked> tp = loadtp.Load_TransportPacked(Transport_Packed_Pk, ref DB);
        DB.DBCon.Close();
        return tp;
    }

    [WebMethod]
    public string Delete_TrarnsportHead(string Transport_Head_Pk) {
        TransportC delth = new TransportC();
        DBConn DB = new DBConn();
        DB.DBCon.Open();
        delth.Delete_TransportHead(Transport_Head_Pk, ref DB);
        DB.DBCon.Close();
        return "1";
    }
    [WebMethod]
    public string Delete_TrarnsportBody(string Transport_Body_Pk) {
        TransportC deltb = new TransportC();
        DBConn DB = new DBConn();
        DB.DBCon.Open();
        deltb.Delete_TransportBody(Transport_Body_Pk, ref DB);
        DB.DBCon.Close();
        return "1";
    }
    [WebMethod]
    public string Delete_TrarnsportPacked(string Transport_Packed_Pk) {
        DBConn DB = new DBConn();
        DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"SELECT COUNT(*) FROM [dbo].[TRANSPORT_BODY] WHERE [TRANSPORT_PACKED_PK] = " + Transport_Packed_Pk;
		string Count = DB.SqlCmd.ExecuteScalar() + "";
		if (Count != "0") {
			DB.DBCon.Close();
			return "-1";
		}
		else {
			DB.SqlCmd.CommandText = @"DELETE FROM [dbo].[TRANSPORT_PACKED] WHERE [TRANSPORT_PACKED_PK] = " + Transport_Packed_Pk;
			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
			return "1";
		}
    }

	[WebMethod]
	public string Set_TransportDelivery(string TransportHeadPk, string TransportStatus, string BranchPk_From, string CompanyPk_To, string WarehousePk_Arrival, string Area_From, string Area_To, string Datetime_From, string Datetime_To, string Title, string VesselName, string Voyage_No, string Value_String_1, string Value_String_2, string Value_String_3, string StoragePk, string DeliveryHeadPk, string DeliveryBodyPk, string DeliveryPrice, string PurchaseHeadPk, string PurchaseBodyPk, string PurchasePrice) {
		TransportC TC = new TransportC();
		sTransportHead THead = new sTransportHead();
		string DB_TransportHeadPk;

		THead.Transport_Head_Pk = TransportHeadPk;
		THead.Transport_Way = "Delivery";
		THead.Transport_Status = TransportStatus;
		THead.BranchPk_From = BranchPk_From;
		THead.BranchPk_To = CompanyPk_To;
		THead.Warehouse_Pk_Arrival = WarehousePk_Arrival;
		THead.Area_From = Area_From;
		THead.Area_To = Area_To;
		THead.DateTime_From = Datetime_From;
		THead.DateTime_To = Datetime_To;
		THead.Title = Title;
		THead.VesselName = VesselName;
		THead.Voyage_No = Voyage_No;
		THead.Value_String_1 = Value_String_1;
		THead.Value_String_2 = Value_String_2;
		THead.Value_String_3 = Value_String_3;

		DBConn DB = new DBConn();
		DB.DBCon.Open();

		DB_TransportHeadPk = TC.Set_TransportHead(THead, ref DB);

		DB.SqlCmd.CommandText = @"UPDATE [dbo].[STORAGE] SET [TRANSPORT_HEAD_PK] = " + DB_TransportHeadPk + @"WHERE[STORAGE_PK] = " + StoragePk;
		DB.SqlCmd.ExecuteNonQuery();


		DB.DBCon.Close();

		return "1";
	}

}
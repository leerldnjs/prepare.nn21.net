using CKEditor.NET;
using Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

/// <summary>
/// DocumentP의 요약 설명입니다.
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
[System.Web.Script.Services.ScriptService]
public class DocumentP : System.Web.Services.WebService {

    public DocumentP() {

    }

    [WebMethod]
    public string SetDelete_Document(string DocumentPk) {
        string query = string.Format(@"
			DELETE FROM [INTL2010].[dbo].[Document] WHERE [DocumentPk]={0}; ", DocumentPk);
        Utility.Excute(query);
        return "1";
    }
    [WebMethod]
    public string MakeHtml_DebitCreditSCHistory(string SorC) {
        DBConn DB = new DBConn();
        DateTime datetime = DateTime.Now.AddMonths(-2);

        if (SorC == "S") {
            DB.SqlCmd.CommandText = @"
    SELECT [Value0],[Value1],[Value2],[Value3]
    FROM [INTL2010].[dbo].[Document] 
    WHERE [Type]='DebitCredit' and Value13>'" + datetime.Year + "/" + datetime.ToString("MM") + @"'
    GROUP BY [Value0],[Value1],[Value2],[Value3] 
    ORDER BY [Value0] DESC ;";
        } else {
            DB.SqlCmd.CommandText = @"
    SELECT [Value4],[Value5],[Value6],[Value7]
    FROM [INTL2010].[dbo].[Document] 
    WHERE [Type]='DebitCredit' and Value13>'" + datetime.Year + "/" + datetime.ToString("MM") + @"'
    GROUP BY [Value4],[Value5],[Value6],[Value7] 
    ORDER BY [Value4] DESC ;";
        }
        DB.DBCon.Open();
        SqlDataReader RS = DB.SqlCmd.ExecuteReader();
        StringBuilder InnerHtml = new StringBuilder();
        while (RS.Read()) {
            InnerHtml.Append(@"
    <tr onclick=""ChooseCompany('" + SorC + @"', '" + RS[0] + @"', '" + RS[1] + @"', '" + RS[2] + @"', '" + RS[3] + @"');"">
     <td style = 'text-align:left; cursor:pointer;' >" + RS[0] + @"</td>
     <td style = 'text-align:left; cursor:pointer;' >" + RS[1] + @"</td >
    </tr >");
        }

        return @"
    <table class=""table table-hover"">
      <thead >
                          <th >Title</th >
                          <th >Address</th >
                      </thead > " + InnerHtml + "</table > ";
    }
    [WebMethod]
    public string SetSave_Transfer(string IssueDate, string SumDocumentPk) {
        sTranfer transfer = new sTranfer() {
            BranchId_From = "3157",
            BranchId_To = "2886",
            CompanyId_From = "3157",
            CompanyId_To = "2886",
            MonetaryUnit = "$",
            CompanyBankId_FinalTransfer = "",
            CompanyBankId_Shipper = "",
            CompanyInDocumentId_Shipper = "",
            Status = "0",
            Date_Send = IssueDate
        };

        Document D = new Document();
        string TransferId = D.SetSave(transfer);
        string query = D.GetQuery_SetCalc_Document(TransferId, SumDocumentPk);
        Utility.Excute(query);
        return TransferId;
    }
    [WebMethod]
    public string SetCancel_Transfer(string TransferId) {
        string query = string.Format(@"
			DELETE FROM [INTL2010].[dbo].[Transfer] WHERE [TransferId]={0}; 
			UPDATE [INTL2010].[dbo].[Document]  SET ParentsType=NULL, [ParentsId]=null 
			WHERE [Type]='DebitCredit' and [ParentsType]='Transfer' and [ParentsId]={0};", TransferId);
        Utility.Excute(query);
        return "1";
    }
    [WebMethod]
    public string SetSave_Document(string ValueSum) {
        Dictionary<string, string> Form = new Utility().ConvertGetParam(ValueSum);
        sDebitCredit Debit = new sDebitCredit();

        Debit.DocumentPk = Form["DocumentPk"];
        Debit.TBBHPk = Form["TBBHPk"];
        Debit.Status = Form["Status"];
        Debit.ShipperName = Form["ShipperName"];
        Debit.ShipperTEL = Form["ShipperTEL"];
        Debit.ShipperFAX = Form["ShipperFAX"];
        Debit.ShipperAddress = Form["ShipperAddress"];
        Debit.ConsigneeName = Form["ConsigneeName"];
        Debit.ConsigneeAddress = Form["ConsigneeAddress"];
        Debit.ConsigneeTEL = Form["ConsigneeTEL"];
        Debit.ConsigneeFAX = Form["ConsigneeFAX"];
        Debit.VesselName = Form["VesselName"];
        Debit.IssueDate = Form["IssueDate"];
        Debit.Container = Form["Container"];
        Debit.ETD = Form["ETD"];
        Debit.ETA = Form["ETA"];
        Debit.Weight = Form["Weight"];
        Debit.POL = Form["POL"];
        Debit.POD = Form["POD"];
        Debit.Measurment = Form["Measurment"];
        Debit.Quantity = Form["Quantity"];

        Debit.InnerPrice = new List<sDocumentBody>();
        var i = 0;
        string BLNo = "";
        while (Form.ContainsKey("InnerPrice_" + i + "_BLNo")) {
            if (Form["InnerPrice_" + i + "_Title"] != "") {
                if (Form["InnerPrice_" + i + "_BLNo"] != "") {
                    BLNo = Form["InnerPrice_" + i + "_BLNo"];
                }
                Debit.InnerPrice.Add(new sDocumentBody() {
                    DocumentBodyPk = Form["InnerPrice_" + i + "_DocumentBodyPk"],
                    Value0 = BLNo,
                    Value1 = Form["InnerPrice_" + i + "_Title"],
                    ValueDecimal0 = Form["InnerPrice_" + i + "_Collect"] == "" ? 0 : decimal.Parse(Form["InnerPrice_" + i + "_Collect"])
                });
            }
            i++;
        }
        Document D = new Document();
        return D.SetSave(Debit);
    }

    [WebMethod]
    public string Set_Document(string DocumentPk, string Type, string TypePk, string Status, string Value0, string Value1, string Value2, string Value3, string Value4, string Value5, string Value6, string Value7, string Value8, string Value9, string Value10, string Value11, string Value12, string Value13, string Value14, string Value15, string Value16, string Value17, string Value18, string Value19) {

        sDocument dc = new sDocument();
		dc.DocumentPk = DocumentPk;
        dc.Type = Type;
        dc.TypePk = TypePk;
        dc.Status = Status;
        dc.Value0 = Value0;
        dc.Value1 = Value1;
        dc.Value2 = Value2;
        dc.Value3 = Value3;
        dc.Value4 = Value4;
        dc.Value5 = Value5;
        dc.Value6 = Value6;
        dc.Value7 = Value7;
        dc.Value8 = Value8;
        dc.Value9 = Value9;
        dc.Value10 = Value10;
        dc.Value11 = Value11;
        dc.Value12 = Value12;
        dc.Value13 = Value13;
        dc.Value14 = Value14;
        dc.Value15 = Value15;
        dc.Value16 = Value16;
        dc.Value17 = Value17;
        dc.Value18 = Value18;
        dc.Value19 = Value19;

        Document setdc = new Document();
        DBConn DB = new DBConn();
        DB.DBCon.Open();
        setdc.Set_Document(dc, ref DB);

        DB.DBCon.Close();

        return "1";
    }

	[WebMethod]
    public string Delete_Document(string DocumentPk) {
        DBConn DB = new DBConn();
        DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"DELETE FROM [dbo].[Document] WHERE [DocumentPk] = " + DocumentPk;
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();

        return "1";
    }

    [WebMethod]
    public string MakeHtml_ModalSaved(string BranchPk, string Code, string Value) {
        StringBuilder ReturnValue = new StringBuilder();

        string queryWhere = "";
        string RowFormat = "";
        switch (Code) {
            case "nn21com_Route_Continent":
                queryWhere = "and [VALUE_4] is null ";
				RowFormat = "<tr><td onclick=\"ChooseSaved('{0}');\">{2}</td><td onclick=\"Delete_Saved('{0}')\" style=\"color:red;\">X</td></tr>";
				break;
            case "nn21com_Route_Country":
                queryWhere = "and [VALUE_4] = N'" + Value + "'";
				RowFormat = "<tr><td onclick=\"ChooseSaved('{0}');\">{2}</td><td onclick=\"Delete_Saved('{0}')\" style=\"color:red;\" style=\"color:red;\">X</td></tr>";
				break;
            case "nn21com_Route_Area":
                queryWhere = "and [VALUE_4] = N'" + Value + "'";
				RowFormat = "<tr><td onclick=\"ChooseSaved('{0}');\">{2}</td><td onclick=\"Delete_Saved('{0}')\" style=\"color:red;\" style=\"color:red;\">X</td></tr>";
				break;
            case "nn21com_Route_Branch":
                queryWhere = "and [VALUE_4] = N'" + Value + "'";
				RowFormat = "<tr><td onclick=\"ChooseSaved('{0}');\">{2}</td><td onclick=\"Delete_Saved('{0}')\" style=\"color:red;\" style=\"color:red;\">X</td></tr>";
				break;
            case "nn21com_Route_VesselName":
                RowFormat = "<tr><td onclick=\"ChooseSaved('{0}');\">{2}</td><td onclick=\"Delete_Saved('{0}')\" style=\"color:red;\" style=\"color:red;\">X</td></tr>";
                break;
			case "nn21com_Route_ShipWay":
				queryWhere = "and [VALUE_4] = N'" + Value + "'";
				RowFormat = "<tr><td onclick=\"ChooseSaved('{0}');\">{2}</td><td onclick=\"Delete_Saved('{0}')\" style=\"color:red;\" style=\"color:red;\">X</td></tr>";
				break;
			case "nn21com_Route_Container":
				queryWhere = "and [VALUE_4] = N'" + Value + "'";
				RowFormat = "<tr><td onclick=\"ChooseSaved('{0}');\">{2}</td><td onclick=\"Delete_Saved('{0}')\" style=\"color:red;\" style=\"color:red;\">X</td></tr>";
				break;
		}

        DBConn DB = new DBConn();
        Setting SETTING = new Setting();
        string Query = @"SELECT [SAVED_PK]
            ,[VALUE_0]
            ,[VALUE_1]
            ,[VALUE_2]
            ,[VALUE_3]
            ,[VALUE_4]
            ,[VALUE_INT_0]
        FROM [dbo].[SAVED]
            WHERE [BRANCH_PK] = " + SETTING.ToDB(BranchPk, "int") + @"
            AND [CODE] = " + SETTING.ToDB(Code, "varchar") + " " + queryWhere;

        DB.SqlCmd.CommandText = Query;
        DB.DBCon.Open();
        SqlDataReader RS = DB.SqlCmd.ExecuteReader();
        StringBuilder InnerHtml = new StringBuilder();
        while (RS.Read()) {
            string[] temp = new string[] {
                RS[0]+",!"+RS[1]+",!"+RS[2]+",!"+RS[3]+",!"+RS[4]+",!"+RS[5]+",!"+RS[6],
                RS[0]+"",
                RS[1]+"",
                RS[2]+"",
                RS[3]+"",
                RS[4]+"",
                RS[5]+"",
                RS[6]+""};
            InnerHtml.AppendFormat(RowFormat, temp);
        }

        RS.Close();

        DB.DBCon.Close();

        if (InnerHtml.ToString() != "") {
            switch (Code) {
                case "nn21com_Route_Continent":
                    ReturnValue.Append("<table class=\"table table-hover\"><thead><tr><th>대륙</th><th></th></tr></thead><tbody>" + InnerHtml + "</tbody></table>");
                    break;
                case "nn21com_Route_Country":
                    ReturnValue.Append("<table class=\"table table-hover\"><thead><tr><th>국가</th><th></th></tr></thead><tbody>" + InnerHtml + "</tbody></table>");
                    break;
                case "nn21com_Route_Area":
                    ReturnValue.Append("<table class=\"table table-hover\"><thead><tr><th>지역</th><th></th></tr></thead><tbody>" + InnerHtml + "</tbody></table>");
                    break;
                case "nn21com_Route_Branch":
                    ReturnValue.Append("<table class=\"table table-hover\"><thead><tr><th>지점</th><th></th></tr></thead><tbody>" + InnerHtml + "</tbody></table>");
                    break;
				case "nn21com_Route_VesselName":
					ReturnValue.Append("<table class=\"table table-hover\"><thead><tr><th>선사</th><th></th></tr></thead><tbody>" + InnerHtml + "</tbody></table>");
					break;
				case "nn21com_Route_ShipWay":
					ReturnValue.Append("<table class=\"table table-hover\"><thead><tr><th>운송</th><th></th></tr></thead><tbody>" + InnerHtml + "</tbody></table>");
					break;
				case "nn21com_Route_Container":
					ReturnValue.Append("<table class=\"table table-hover\"><thead><tr><th>항목</th><th></th></tr></thead><tbody>" + InnerHtml + "</tbody></table>");
					break;
			}
        }
        return ReturnValue.ToString();
    }

    [WebMethod]
    public string Set_Saved(string ValueSum) {
        Dictionary<string, string> Form = new Utility().ConvertGetParam(ValueSum);
        Setting SETTING = new Setting();
        DBConn DB = new DBConn();
        DB.DBCon.Open();
        SETTING.Set_Saved(Form["BranchPk"], Form["Code"], Form["Value_0"], Form["Value_1"], Form["Value_2"], Form["Value_3"], Form["Value_4"], Form["Value_Int_0"], ref DB);
        DB.DBCon.Close();

        return "1";
    }
	[WebMethod]
	public string Delete_Saved(string SavedPk) {
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"DELETE FROM [dbo].[SAVED] WHERE [SAVED_PK] = " + SavedPk;
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();

		return "1";
	}
}


/*
[WebMethod]
public string MakeHtml_Document(string BranchPk, string Type, string Flag) {
    StringBuilder RetVal = new StringBuilder();

    DBConn DB = new DBConn();
    DB.DBCon.Open();
    Setting ld = new Setting();
    SqlDataReader RS1 = ld.Load_Saved(BranchPk, Type, ref DB);
    List<string[]> RS2 = new List<string[]>();

    RetVal.Append("<table class=\"table table-hover\"><thead>");
    RetVal.Append("<tr><th>대륙</th><th>국가</th><th>지역</th><th>지점</th></tr></thead><tbody>");

    while (RS1.Read()) {
        RS2.Add(new string[] { RS1["SAVED_PK"] + "", RS1["VALUE_0"] + "", RS1["VALUE_1"] + "", RS1["VALUE_2"] + "", RS1["VALUE_3"] + "" });
    }

    foreach (string [] each in RS2) {
        RetVal.Append("<tr onclick=\"Get_Saved('" + string.Join(",!", each) + "'" + ", '" + Flag + "')\">");
        RetVal.Append("<td style=\"display:none; text-align:center; padding:10px;\">" + each[0] + "</td>");
        for (int j = 1; j < 5; j++) {
            RetVal.Append("<td>" + each[j] + "</td>");
        }
        RetVal.Append("</tr>");
    }

    DB.DBCon.Close();

    return RetVal + "</tbody></table>";
}
*/

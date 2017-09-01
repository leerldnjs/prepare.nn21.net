using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CustomClearance_ForCapture_HouseBL_YT : System.Web.UI.Page
{
    protected String Shipper;
    protected String ShipperAddress;
    protected String Consignee;
    protected String ConsigneeAddress;
    protected String NotifyParty;
    protected String NotifyPartyAddress;
    protected String PortOfLanding;
    protected String Carrier;
    protected String FinalDestination;
    protected String SailingOnOrAbout;
    protected String PaymentTerms;
    protected StringBuilder ItemList;
    protected String TotalQuantity;
    protected String LastTotalQuantity;
    protected String TotalGrossWeight;
    protected String TotalNetWeight;
    protected String TotalVolume;
    protected String[] MemberInfo;
    protected String BLNo;
    protected String Buyer;
    protected String Memo;
    protected String FOBNCNF;
    protected String gubun;
    protected String VoyageNo;
    protected String VoyageCompany;
    protected String ContainerNo;
    protected String SealNo;
    protected String ContainerSize;
    protected String NM;
    protected void Page_Load(object sender, EventArgs e)
    {        
        gubun = Request.Params["G"] + "" == "" ? "View" : Request.Params["G"] + "";

        if (Request.Params["S"] + "" != "")
        {
            LoadCommercialInvoice(Request.Params["S"]);
            LoadCommercialInvoiceItems(Request.Params["S"]);
            NM = "N/M";
        }

    }
    private string ToCardinal0To99(int number)
    {
        string cardinal = "";
        switch (number)
        {
            case 0:
                cardinal = "";
                break;
            case 1:
                cardinal = "one";
                break;
            case 2:
                cardinal = "two";
                break;
            case 3:
                cardinal = "three";
                break;
            case 4:
                cardinal = "four";
                break;
            case 5:
                cardinal = "five";
                break;
            case 6:
                cardinal = "six";
                break;
            case 7:
                cardinal = "seven";
                break;
            case 8:
                cardinal = "eight";
                break;
            case 9:
                cardinal = "nine";
                break;
            case 10:
                cardinal = "ten";
                break;
            case 11:
                cardinal = "eleven";
                break;
            case 12:
                cardinal = "twelve";
                break;
            case 13:
                cardinal = "thirteen";
                break;
            case 14:
                cardinal = "fourteen";
                break;
            case 15:
                cardinal = "fifteen";
                break;
            case 16:
                cardinal = "sixteen";
                break;
            case 17:
                cardinal = "seventeen";
                break;
            case 18:
                cardinal = "eighteen";
                break;
            case 19:
                cardinal = "nineteen";
                break;
            case 20:
                cardinal = "twenty";
                break;
            case 21:
                cardinal = "twenty-one";
                break;
            case 22:
                cardinal = "twenty-two";
                break;
            case 23:
                cardinal = "twenty-three";
                break;
            case 24:
                cardinal = "twenty-four";
                break;
            case 25:
                cardinal = "twenty-five";
                break;
            case 26:
                cardinal = "twenty-six";
                break;
            case 27:
                cardinal = "twenty-seven";
                break;
            case 28:
                cardinal = "twenty-eight";
                break;
            case 29:
                cardinal = "twenty-nine";
                break;
            case 30:
                cardinal = "thirty";
                break;
            case 31:
                cardinal = "thirty-one";
                break;
            case 32:
                cardinal = "thirty-two";
                break;
            case 33:
                cardinal = "thirty-three";
                break;
            case 34:
                cardinal = "thirty-four";
                break;
            case 35:
                cardinal = "thirty-five";
                break;
            case 36:
                cardinal = "thirty-six";
                break;
            case 37:
                cardinal = "thirty-seven";
                break;
            case 38:
                cardinal = "thirty-eight";
                break;
            case 39:
                cardinal = "thirty-nine";
                break;
            case 40:
                cardinal = "forty";
                break;
            case 41:
                cardinal = "forty-one";
                break;
            case 42:
                cardinal = "forty-two";
                break;
            case 43:
                cardinal = "forty-three";
                break;
            case 44:
                cardinal = "forty-four";
                break;
            case 45:
                cardinal = "forty-five";
                break;
            case 46:
                cardinal = "forty-six";
                break;
            case 47:
                cardinal = "forty-seven";
                break;
            case 48:
                cardinal = "forty-eight";
                break;
            case 49:
                cardinal = "forty-nine";
                break;
            case 50:
                cardinal = "fifty";
                break;
            case 51:
                cardinal = "fifty-one";
                break;
            case 52:
                cardinal = "fifty-two";
                break;
            case 53:
                cardinal = "fifty-three";
                break;
            case 54:
                cardinal = "fifty-four";
                break;
            case 55:
                cardinal = "fifty-five";
                break;
            case 56:
                cardinal = "fifty-six";
                break;
            case 57:
                cardinal = "fifty-seven";
                break;
            case 58:
                cardinal = "fifty-eight";
                break;
            case 59:
                cardinal = "fifty-nine";
                break;
            case 60:
                cardinal = "sixty";
                break;
            case 61:
                cardinal = "sixty-one";
                break;
            case 62:
                cardinal = "sixty-two";
                break;
            case 63:
                cardinal = "sixty-three";
                break;
            case 64:
                cardinal = "sixty-four";
                break;
            case 65:
                cardinal = "sixty-five";
                break;
            case 66:
                cardinal = "sixty-six";
                break;
            case 67:
                cardinal = "sixty-seven";
                break;
            case 68:
                cardinal = "sixty-eight";
                break;
            case 69:
                cardinal = "sixty-nine";
                break;
            case 70:
                cardinal = "seventy";
                break;
            case 71:
                cardinal = "seventy-one";
                break;
            case 72:
                cardinal = "seventy-two";
                break;
            case 73:
                cardinal = "seventy-three";
                break;
            case 74:
                cardinal = "seventy-four";
                break;
            case 75:
                cardinal = "seventy-five";
                break;
            case 76:
                cardinal = "seventy-six";
                break;
            case 77:
                cardinal = "seventy-seven";
                break;
            case 78:
                cardinal = "seventy-eight";
                break;
            case 79:
                cardinal = "seventy-nine";
                break;
            case 80:
                cardinal = "eighty";
                break;
            case 81:
                cardinal = "eighty-one";
                break;
            case 82:
                cardinal = "eighty-two";
                break;
            case 83:
                cardinal = "eighty-three";
                break;
            case 84:
                cardinal = "eighty-four";
                break;
            case 85:
                cardinal = "eighty-five";
                break;
            case 86:
                cardinal = "eighty-six";
                break;
            case 87:
                cardinal = "eighty-seven";
                break;
            case 88:
                cardinal = "eighty-eight";
                break;
            case 89:
                cardinal = "eighty-nine";
                break;
            case 90:
                cardinal = "ninety";
                break;
            case 91:
                cardinal = "ninety-one";
                break;
            case 92:
                cardinal = "ninety-two";
                break;
            case 93:
                cardinal = "ninety-three";
                break;
            case 94:
                cardinal = "ninety-four";
                break;
            case 95:
                cardinal = "ninety-five";
                break;
            case 96:
                cardinal = "ninety-six";
                break;
            case 97:
                cardinal = "ninety-seven";
                break;
            case 98:
                cardinal = "ninety-eight";
                break;
            case 99:
                cardinal = "ninety-nine";
                break;
            default:
                cardinal = "";
                break;
        }
        return cardinal;
    }
    private string ToCardinal0To999(int Number)
    {
        int Over100 = ((Number / 100) * 100) / 100;
        int Under100 = Number - Over100 * 100;
        string ReturnValue = "";
        string strOver100 = "";
        string strUnder100 = "";

        if (Over100 < 10 && Over100 > 0)
        {
            strOver100 = ToCardinal0To99(Over100) + " hundred";
        }
        if (Under100 > 0)
        {
            strUnder100 = ToCardinal0To99(Under100);
        }
        ReturnValue = strOver100 + " " + strUnder100;
        return ReturnValue.Trim();
    }
    public string int32ToCardinalNumber(int Number)
    {
        string strNumber = Common.NumberFormat(Number.ToString());
        string[] arrNumber = strNumber.Split(new string[] { "," }, StringSplitOptions.None);
        string ReturnValue = "";

        int Count = 0;
        for (var i = arrNumber.Length - 1; i > -1; i--)
        {
            string each = ToCardinal0To999(Int32.Parse(arrNumber[i]));
            switch (Count)
            {
                case 0:
                    ReturnValue = each;
                    break;
                case 1:
                    ReturnValue = each + " THOUSAND " + ReturnValue;
                    break;
                case 2:
                    ReturnValue = each + " MILLION " + ReturnValue;
                    break;
                case 3:
                    ReturnValue = each + " BILLION " + ReturnValue;
                    break;
            }
            Count++;
        }
        return ReturnValue.ToUpper().Trim();
    }
    private void LoadCommercialInvoice(string CommercialDocumentHeadPk)
    {
        DBConn DB = new DBConn();

        string TransportBetweenBranchPk = "";
        DB.SqlCmd.CommandText = @"
SELECT CD.[BLNo], [InvoiceNo], [Shipper], [ShipperAddress], [Consignee], [ConsigneeAddress], [NotifyParty], CD.[NotifyPartyAddress], [PortOfLoading], [FinalDestination], [Carrier], [SailingOn]
	 , [PaymentTerms] ,[OtherReferences], [StampImg], [FOBorCNF], [VoyageNo], [VoyageCompany], [ContainerNo], [SealNo], [ContainerSize], CD.[Registerd], [ClearanceDate], CD.[StepCL] 
     ,tbbhead.[TRANSPORT_PK] ,tbbhead.[BRANCHPK_TO]
FROM CommercialDocument CD
	left join CommerdialConnectionWithRequest CCWR on CD.CommercialDocumentHeadPk=CCWR.CommercialDocumentPk
	left join RequestForm R on CCWR.RequestFormPk= R.RequestFormPk
		left join (select TransportBetweenBranchPk,RequestFormpk from TransportBBHistory
				union all
			   select [TRANSPORT_HEAD_PK],[REQUEST_PK] from [dbo].[STORAGE])	as tbbpk on r.RequestFormPk=tbbpk.RequestFormPk	
    left join [dbo].[TRANSPORT_HEAD] as tbbhead on tbbpk.TransportBetweenBranchPk=tbbhead.[TRANSPORT_PK]
WHERE CommercialDocumentHeadPk=" + CommercialDocumentHeadPk + ";";
        DB.DBCon.Open();
        SqlDataReader RS = DB.SqlCmd.ExecuteReader();
        string tempFOBNCNF = "";
        while (RS.Read())
        {
            if (RS["BRANCHPK_TO"] + "" == "3157")
            {
                TransportBetweenBranchPk = RS["TRANSPORT_PK"] + "";
                Shipper = "" + RS["Shipper"];
                ShipperAddress = "" + RS["ShipperAddress"];
                Consignee = "" + RS["Consignee"];
                ConsigneeAddress = "" + RS["ConsigneeAddress"];
                NotifyParty = "" + RS["NotifyParty"];
                NotifyPartyAddress = "" + RS["NotifyPartyAddress"];
                PortOfLanding = "" + RS["PortOfLoading"];
                FinalDestination = "" + RS["FinalDestination"];
                Carrier = "" + RS["Carrier"];
                SailingOnOrAbout = "" + RS["SailingOn"];
                PaymentTerms = "" + RS["PaymentTerms"];
                Memo = "" + RS["OtherReferences"];
                VoyageNo = "" + RS["VoyageNo"];
                VoyageCompany = "" + RS["VoyageCompany"];
                ContainerNo = "" + RS["ContainerNo"];
                SealNo = "" + RS["SealNo"];
                ContainerSize = "" + RS["ContainerSize"];
                tempFOBNCNF = RS["FOBorCNF"] + "" == "" ? "" : (RS["FOBorCNF"] + "").Substring(0, 3);
                FOBNCNF = tempFOBNCNF + "" == "FOB" ? "FREIGHT COLLECT" : "FREIGHT PREPAID";
            }
        }

        RS.Dispose();
        DB.DBCon.Close();
        DB.SqlCmd.CommandText = @"
SELECT [VALUE_STRING_0],[TRANSPORT_STATUS] FROM [dbo].[TRANSPORT_HEAD] Head
WHERE Head.[TRANSPORT_PK]=" + TransportBetweenBranchPk + ";";
        DB.DBCon.Open();
        RS = DB.SqlCmd.ExecuteReader();
        string TableGubun = "";
        while (RS.Read())
        {
            BLNo = RS["VALUE_STRING_0"] + "";
            TableGubun = RS["TRANSPORT_STATUS"] + "";
        }
        RS.Dispose();

        int TempCount = 0;
        if (TableGubun == "3" || TableGubun == "")
        {
            DB.SqlCmd.CommandText = @"
SELECT count(*)
from TransportBBHistory AS Storage 
left join (select * from RequestForm where DocumentRequestCL like '%33%') AS R on Storage.RequestFormPk=R.RequestFormPk 
left join CommerdialConnectionWithRequest AS CCWR ON R.RequestFormPk=CCWR.RequestFormPk 
left join CommercialDocument AS CD ON CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk 
WHERE Storage.TransportBetweenBranchPk = " + TransportBetweenBranchPk + @" and R.DocumentStepCL<>4";
		}
        else
        {
            DB.SqlCmd.CommandText = @"
SELECT count(*)
from [dbo].[STORAGE] AS Storage
left join (select * from RequestForm where DocumentRequestCL like '%33%') AS R on Storage.[REQUEST_PK]=R.RequestFormPk 
left join CommerdialConnectionWithRequest AS CCWR ON R.RequestFormPk=CCWR.RequestFormPk 
left join CommercialDocument AS CD ON CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk 
WHERE Storage.[TRANSPORT_HEAD_PK] = " + TransportBetweenBranchPk + @" and R.DocumentStepCL<>4";
		}
        RS = DB.SqlCmd.ExecuteReader();
        if (RS.Read())
        {
            TempCount = Int32.Parse(RS[0] + "");
        }
        RS.Dispose();

        if (TableGubun == "3" || TableGubun == "")
        {
            DB.SqlCmd.CommandText = @"select CD.CommercialDocumentHeadPk, R.DocumentRequestCL
from TransportBBHistory AS Storage 
left join RequestForm AS R on Storage.RequestFormPk=R.RequestFormPk 
left join CommerdialConnectionWithRequest AS CCWR ON R.RequestFormPk=CCWR.RequestFormPk 
left join CommercialDocument AS CD ON CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk 
WHERE Storage.TransportBetweenBranchPk = " + TransportBetweenBranchPk + @" and R.DocumentStepCL<>4
ORDER BY R.ConsigneeCode, R.RequestFormPk, CD.BLNo, R.ArrivalRegionCode;";
        }
        else
        {
            DB.SqlCmd.CommandText = @"select CD.CommercialDocumentHeadPk, R.DocumentRequestCL
from [dbo].[STORAGE] AS Storage
left join RequestForm AS R on Storage.[REQUEST_PK]=R.RequestFormPk 
left join CommerdialConnectionWithRequest AS CCWR ON R.RequestFormPk=CCWR.RequestFormPk 
left join CommercialDocument AS CD ON CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk 
WHERE Storage.[TRANSPORT_HEAD_PK] = " + TransportBetweenBranchPk + @" and R.DocumentStepCL<>4
ORDER BY R.ConsigneeCode, R.RequestFormPk, CD.BLNo, R.ArrivalRegionCode;";
		}


        RS = DB.SqlCmd.ExecuteReader();
        string[] Pool = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        int cursor = -1;

        bool if33 = false;
        string[] tempDocumentRequestCL;
        while (RS.Read())
        {
            tempDocumentRequestCL = RS["DocumentRequestCL"].ToString().Split(Common.Splite11, StringSplitOptions.None);

            foreach (string e in tempDocumentRequestCL)
            {
                switch (e)
                {
                    case "33":
                        cursor++;
                        break;
                }
            }
            if (RS[0] + "" == CommercialDocumentHeadPk)
            {
                foreach (string e in tempDocumentRequestCL)
                {
                    switch (e)
                    {
                        case "33":
                            if33 = true;
                            break;
                    }
                }
                break;
            }

        }
        if (if33)
        {
            BLNo = BLNo + Pool[cursor];
        }
        else
        {
			BLNo = BLNo + Pool[TempCount];
        }
        RS.Dispose();
        DB.DBCon.Close();

    }
    private void LoadCommercialInvoiceItems(string CommercialDocumentHeadPk)
    {

        DBConn DB = new DBConn();
        DB.SqlCmd.CommandText = @"	SELECT CICK.Description, CICK.Material 
															FROM CommerdialConnectionWithRequest as CCWR 
																left join RequestFormItems AS RFI ON CCWR.RequestFormPk =RFI.RequestFormPk  
																left join ClearanceItemCodeKOR AS CICK ON RFI.ItemCode=CICK.ItemCode  
															WHERE CCWR.CommercialDocumentPk=" + CommercialDocumentHeadPk + ";";
        DB.DBCon.Open();
        SqlDataReader RS = DB.SqlCmd.ExecuteReader();
        List<string> TempItemSum = new List<string>();
        while (RS.Read())
        {
            bool CheckIsIn = false;
            for (int i = 0; i < TempItemSum.Count; i++) { if (TempItemSum[i] == (RS["Description"] + "").Trim()) { CheckIsIn = true; } }
            if (!CheckIsIn)
            {
                TempItemSum.Add((RS["Description"] + "").Trim());
            }
        }
        RS.Dispose();

        DB.SqlCmd.CommandText = @"	SELECT RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume 
															FROM CommerdialConnectionWithRequest AS CCWR
																left join [dbo].[RequestForm] AS RF ON CCWR.RequestFormPk=RF.RequestFormPk	
															WHERE CCWR.CommercialDocumentPk=" + CommercialDocumentHeadPk + ";";
        RS = DB.SqlCmd.ExecuteReader();
        string BeforePacking = "";
        Decimal TempTotalGweight = 0;
        Decimal TempTotalVolume = 0;
        Decimal TempTotalPackingCount = 0;

        while (RS.Read())
        {
            if (RS[0] + "" != "") { TempTotalPackingCount += Decimal.Parse(RS[0] + ""); }
            if (RS[1] + "" != "")
            {
                if (BeforePacking == "")
                {
                    BeforePacking = Common.GetPackingUnit(RS[1] + "");
                }
                else if (BeforePacking != Common.GetPackingUnit(RS[1] + ""))
                {
                    BeforePacking = "GT";
                }
            }
            if (RS[2] + "" != "") { TempTotalGweight += Decimal.Parse(RS[2] + ""); }
            if (RS[3] + "" != "") { TempTotalVolume += Decimal.Parse(RS[3] + ""); }
        }

        RS.Dispose();
        DB.DBCon.Close();
        ItemList = new StringBuilder();
        for (int i = 0; i < TempItemSum.Count; i++)
        {
            if (i == 0)
            {
                ItemList.Append(TempItemSum[i]);
            }
            else
            {
                if (TempItemSum[(i - 1)].Length > 50)
                {
                    ItemList.Append(", <br /> " + TempItemSum[i]);
                }
                else
                {
                    ItemList.Append(", " + TempItemSum[i]);
                }
            }
        }
        TotalQuantity = string.Empty;

        TotalGrossWeight = TempTotalGweight == 0 ? "" : Common.NumberFormat(TempTotalGweight + "") + " KGS";
        TotalVolume = TempTotalVolume == 0 ? "" : Common.NumberFormat(TempTotalVolume + "") + " CBM";

        if (BeforePacking + "" == "GT")
        {
            BeforePacking = "GT";
        }
        else if (BeforePacking + "" == "CT")
        {
            BeforePacking = "CARTON";
        }
        else if (BeforePacking + "" == "RO")
        {
            BeforePacking = "ROLL";
        }
        else if (BeforePacking + "" == "PA")
        {
            BeforePacking = "PALLET";
        }

        TotalQuantity = TempTotalPackingCount + " " + BeforePacking;
        LastTotalQuantity = "SAY:" + int32ToCardinalNumber(Int32.Parse(TempTotalPackingCount.ToString())) + " " + BeforePacking + " ONLY";
    }


}
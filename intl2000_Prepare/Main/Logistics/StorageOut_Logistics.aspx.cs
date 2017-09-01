using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Text;
using System.Data.SqlClient;

public partial class Logistics_StorageOut_Logistics : System.Web.UI.Page
{
    protected String[] MEMBERINFO;
    protected String Gubun;
    protected Int32 PageNo;
    protected String HTMLDriver;
    private DBConn DB;
    protected Int32 ChkCount = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null)
        {
            Response.Redirect("../Default.aspx");
        }
        try
        {
            if (Request["Language"].Length == 2)
            {
                Session["Language"] = Request["Language"];
            }
        }
        catch (Exception)
        {
        }
        switch (Session["Language"] + "")
        {
            case "en":
                Page.UICulture = "en";
                break;
            case "ko":
                Page.UICulture = "ko";
                break;
            case "zh":
                Page.UICulture = "zh-cn";
                break;
        }
        MEMBERINFO = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
        DB = new DBConn();
        LoadAleadySetDriver(MEMBERINFO[1]);
    }

    private Boolean LoadAleadySetDriver(string BranchPk)
    {
        string InnerTableRowFormat = "<tr>" +
            "<td class='TBody1' >{0}</td>" +
            "<td class='TBody1' >{1}</td>" +
            "<td class='TBody1' style=\"text-align:left; padding-left:10px; \" >{2}</td>" +
            "<td class='TBody1' >{5}</td>" +
            "<td class='TBody1' >{3}</td>" +
            "<td class='TBody1' >{4}</td>" +
            "<td class='TBody1' >{8}</td>" +
            "<td class='TBody1' >{9}</td>" +
            "<td class='TBody1' >{10}</td>" +
            "<td class='TBody1' >{11}</td>" +
            "<td class='TBody1' >{6}</td>" +
            "<td class='TBody1' >{7}</td>" +
            "<td class='TBody1' >{12}</td></tr>";
        string InnerTableFormat = "<fieldset style=\"width:950px; margin-left:10px; margin-right:10px; \">" +
                    "<legend><strong>{0}</strong>&nbsp;&nbsp;&nbsp;<input type=\"button\" value=\"List Down\" onclick=\"DeliveryListExcelDown('{0}', '" + BranchPk + "');\"  /></legend>" +
                    "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"padding:3px; width:950px; \"><tr>" +
                    "	<td style=\"width:80px; font-size:1px; \">&nbsp;</td>" +
                    "	<td style=\"width:80px; font-size:1px; \">&nbsp;</td>" +
                    "	<td style=\"font-size:1px; \">&nbsp;</td>" +
                    "	<td style=\"width:90px; font-size:1px; \">&nbsp;</td>" +
                    "	<td style=\"width:50px; font-size:1px; \">&nbsp;</td>" +
                    "	<td style=\"width:70px; font-size:1px; \">&nbsp;</td>" +
                    "	<td style=\"width:70px; font-size:1px; \">&nbsp;</td>" +
                    "	<td style=\"width:50px; font-size:1px; \">&nbsp;</td>" +
                    "	<td style=\"width:90px; font-size:1px; \">&nbsp;</td>" +
                    "	<td style=\"width:50px; font-size:1px; \">&nbsp;</td>" +
                    "	<td style=\"width:70px; font-size:1px; \">&nbsp;</td>" +
                    "	<td style=\"width:50px; font-size:1px; \">&nbsp;</td>" +
                    "	<td style=\"width:50px; font-size:1px; \">&nbsp;</td></tr>{1}</table></fieldset>";
        //자체통관 출고, 화주님직출
        DB.SqlCmd.CommandText = @"	
DECLARE @BranchCode int;
SET @BranchCode=" + BranchPk + @";
SELECT 
	OBS.OurBranchStorageOutPk, OBS.RequestFormPk, OBS.BoxCount, OBS.StatusCL
	, OBSC.StorageName 
    , TBH.[DATETIME_TO]
	, Consignee.CompanyName AS ConsigneeName, Consignee.CompanyNamee AS ConsigneeNamee
	, R.ShipperPk, R.ConsigneePk, R.ConsigneeCode, R.StepCL, R.DocumentStepCL
	, RF.TotalPackedCount, RF.PackingUnit, RFCH.[ShipperMonetaryUnit], RFCH.[ExchangeRate], RFCH.[ShipperCharge], (isnull(RFCH.[ShipperDeposited], 0)-RFCH.[ShipperCharge]-isnull(RFCB.[Price], 0)) AS Shipperleft, RFCH.[ConsigneeMonetaryUnit], RFCH.[ConsigneeCharge], (isnull(RFCH.[ConsigneeDeposited], 0)-RFCH.[ConsigneeCharge]-isnull(RFCB.[Price], 0)) AS Consigneeleft, RFCH.[ConsigneeBankAccountPk], RFCH.[WillPayTariff]
    , RF.TotalGrossWeight,RF.TotalVolume
	, RFCB.Price AS DCPrice, RFCB.StandardPriceHeadPkNColumn
    , Tariff.TSum
	, CD.CommercialDocumentHeadPk
	, TBC.TransportBetweenCompanyPk, TBC.Type, TBC.Title, TBC.DriverName, TBC.FromDate, TBC.PackedCount, TBC.PackingUnit, TBC.Memo , TBC.Price TBCPrice, TBC.DeliveryPrice TBCDeliveryPrice,TBC.[DepositWhere]
    , TBCHistory.Name TBCHistoryName
FROM OurBranchStorageOut AS OBS 
	left join OurBranchStorageCode AS OBSC ON OBS.StorageCode=OBSC.OurBranchStoragePk
	left join [dbo].[TRANSPORT_HEAD] AS TBH ON TBH.[TRANSPORT_PK]=OBS.TransportBetweenBranchPk
	left join RequestForm AS R ON OBS.RequestFormPk=R.RequestFormPk 
	left join Company AS Consignee ON Consignee.CompanyPk=R.ConsigneePk 
	left join (SELECT [RequestFormPk], [Price], [StandardPriceHeadPkNColumn] FROM [RequestFormCalculateBody] where [StandardPriceHeadPkNColumn]='D') AS RFCB ON RFCB.RequestFormPk=OBS.RequestFormPk 
	left join RequestFormCalculateHead AS RFCH ON RFCH.RequestFormPk=OBS.RequestFormPk 
	left join (SELECT [GubunPk], SUM([Value]) AS TSum FROM [CommercialDocumentTariff] Group By [GubunPk]) AS Tariff ON Tariff.GubunPk=OBS.RequestFormPk 
	left join CommerdialConnectionWithRequest AS CCWR ON CCWR.RequestFormPk=OBS.RequestFormPk
	left join CommercialDocument AS CD ON CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk 
	left join TransportBC AS TBC ON TBC.TransportBetweenCompanyPk=OBS.TransportBetweenCompanyPk 
    left join (SELECT t.TransportBetweenCompanyPk,a.Name ,t.Registerd
	FROM [dbo].[TransportBCHistory] t left join Account_ a on t.ActID=a.AccountID 
	where t.GubunCL=0) TBCHistory on OBS.TransportBetweenCompanyPk =TBCHistory.TransportBetweenCompanyPk
WHERE OBSC.OurBranchCode=@BranchCode  and OBS.BoxCount>0 
and ( R.DocumentStepCL in (5, 6, 7, 8, 9, 10, 11, 12) or isnull(OBS.StatusCL, 0)<6 ) 
and	(isnull(type,'') like '%온세상%' or isnull(TBC.title,'') like '%온세상%')
ORDER BY  TBCHistory.Registerd desc, StaTusCL ASC;";

        DB.DBCon.Open();
        SqlDataReader RS = DB.SqlCmd.ExecuteReader();
        StringBuilder returnvalue = new StringBuilder();
        StringBuilder innertable = new StringBuilder();
        string[] innertableData;
        string TempType = "#@!";
        string TempTitle = "#@!";

        while (RS.Read())
        {

            if (TempType == "#@!")
            {
                TempType = RS["Type"] + "";
            }
            else if (TempType != RS["Type"] + "")
            {
                returnvalue.Append(string.Format(InnerTableFormat, TempType, innertable));
                innertable = new StringBuilder();
                TempType = RS["Type"] + "";
            }

            if (TempTitle != RS["Title"] + "")
            {
                TempTitle = RS["Title"] + "";
                innertable.Append("<tr style=\"height:30px;\"><td class='THead1 style=\"font-weight:bold; padding:5px; \"'>Date</td><td class='THead1 style=\" font-weight:bold; padding:5px; \"'>창고</td><td class='THead1 style=\" font-weight:bold; padding:5px; \"'>회사명</td><td class='THead1 style=\" font-weight:bold; padding:5px; \"'>CT</td><td class='THead1 style=\" font-weight:bold; padding:5px; \"'>중량</td><td class='THead1 style=\" font-weight:bold; padding:5px; \"'>체적</td><td class='THead1 style=\" font-weight:bold; padding:5px; \"'>기사</td><td class='THead1 style=\" font-weight:bold; padding:5px; \"'>IL</td><td class='THead1 style=\" font-weight:bold; padding:5px; \"'>화주금액</td><td class='THead1 style=\" font-weight:bold; padding:5px; \"'>IL금액</td><td class='THead1 style=\"font-weight:bold; padding:5px; \"'>상태</td><td class='THead1 style=\" font-weight:bold; padding:5px; \"'>출고</td><td class='THead1 style=\" font-weight:bold; padding:5px; \"'>출력</td></tr>");
            }

            innertableData = new string[13];
            innertableData[0] = "<span style=\"font-weight:bold;\">" + (RS["DATETIME_TO"] + "").Substring(5, 5) + " - " + (RS["FromDate"] + "").Substring(6, 2) + "</span>";
            innertableData[1] = RS["StorageName"] + "";
            innertableData[2] = "<span style=\"font-weight:bold; \">" + RS["ConsigneeCode"] + "</span> " + RS["ConsigneeName"] + "";
            innertableData[3] = "<span style=\"font-weight:bold; \">" + Common.NumberFormat(RS["TotalGrossWeight"] + "") + "</span>KG";
            innertableData[4] = "<span style=\"font-weight:bold; \">" + Common.NumberFormat(RS["TotalVolume"] + "") + "</span>CBM";
            innertableData[5] = "<span style=\"cursor:hand; font-weight:bold;\" onclick=\"DeliveryModify('" + RS["OurBranchStorageOutPk"] + "','" + RS["RequestFormPk"] + "' , '" + RS["ConsigneePk"] + "', '" + MEMBERINFO[1] + "');\" >  " +
                        RS["BoxCount"] + " / " + RS["TotalPackedCount"] + "</span>" + Common.GetPackingUnit(RS["PackingUnit"] + "");

            string tempdocument;
            switch (RS["DocumentStepCL"] + "")
            {
                case "1":
                    tempdocument = "준비중";
                    break;
                case "2":
                    tempdocument = "준비중";
                    break;
                case "3":
                    tempdocument = "<span style=\"color:green;\">자가통관</span>";
                    break;
                case "4":
                    tempdocument = "<span style=\"color:green;\">Sample</span>";
                    break;
                case "5":
                    tempdocument = "준비중";
                    break;
                case "6":
                    tempdocument = "통관지시";
                    break;
                case "7":
                    tempdocument = "통관지시";
                    break;
                case "8":
                    tempdocument = "통관지시";
                    break;
                case "9":
                    tempdocument = "통관지시";
                    break;
                case "10":
                    tempdocument = "<span style=\"color:green;\">세납</span>";
                    break;
                case "11":
                    tempdocument = "<span style=\"color:green;\">세납</span>";
                    break;
                case "12":
                    tempdocument = "<span style=\"color:green;\">세납</span>";
                    break;
                case "13":
                    tempdocument = "<span style=\"color:red;\">면허완료</span>";
                    break;
                case "14":
                    tempdocument = "<span style=\"color:red;\">면허완료</span>";
                    break;
                case "15":
                    tempdocument = "<span style=\"color:red;\">면허완료</span>";
                    break;
                default:
                    tempdocument = "미확정";
                    break;
            }
            innertableData[6] = tempdocument;
            innertableData[7] = RS["StatusCL"] + "" == "6" ?
                "&nbsp;<span style=\"color:green;\">출고끝</span>" :
                "<input type=\"button\" value=\"出\" style=\"padding:0px; width:30px; \" onclick=\"GoDeliveryOrder('" + RS["TransportBetweenCompanyPk"] + "' , '" + RS["RequestFormPk"] + "', '" + RS["ConsigneePk"] + "', '" + MEMBERINFO[1] + "');\" />";
            innertableData[8] = RS["DriverName"] + "";
            innertableData[9] = RS["TBCHistoryName"] + "";
            innertableData[10] = (RS["DepositWhere"] + "" == "1" ? "착불" : "현불") + "/" + Common.NumberFormat(RS["TBCPrice"] + "");
            innertableData[11] = Common.NumberFormat(RS["TBCDeliveryPrice"] + "");
            innertableData[12] = "<input type=\"button\" value=\"P\" style=\"padding:0px; width:30px;\" onclick=\"DeliveryPrint(" + RS["TransportBetweenCompanyPk"] + ");\" />";
            innertable.Append(String.Format(InnerTableRowFormat, innertableData));
            if (RS["Memo"] + "" != "")
            {
                string styleMemo = RS["Memo"].ToString();
                if (RS["Memo"].ToString().Trim().Length > 3)
                {
                    if (RS["Memo"].ToString().Trim().Substring(0, 4) == "*수정*")
                    {
                        styleMemo = "<span style=\"color:Red;\" >" + RS["Memo"].ToString() + "</span>";
                    }
                }

                innertable.Append("<tr>" +
            "<td class='TBody1' colspan='2' >&nbsp;</td>" +
            "<td class='TBody1' style=\"text-align:left; padding-left:10px; color:darkblue;\" colspan='11' >" + styleMemo + "</td><td>&nbsp;</td></tr>");
            }
        }
        if (innertable + "" != "")
        {
            returnvalue.Append(string.Format(InnerTableFormat, TempType, innertable));
        }
        RS.Dispose();
        DB.DBCon.Close();
        HTMLDriver = returnvalue + "";
        return true;
    }


}
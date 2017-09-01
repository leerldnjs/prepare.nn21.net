using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Text;
using System.Data.SqlClient;

public partial class RequestForm_ItemModify : System.Web.UI.Page
{
    protected String StepCL;
	protected StringBuilder ItemInformationLoad;
	protected String MonetaryUnitCL;
	protected String PriceSum;
	protected String QuantitySum;
	protected String PackedCountSum;
	protected String WeightSum;
	protected String VolumeSum;
	protected String MonetaryUnit;
	protected String MemberGubun;
	protected void Page_Load(object sender, EventArgs e)
	{
		MemberGubun = (Session["MemberInfo"] + "").Split(Common.Splite11, StringSplitOptions.None)[0];
		if (Request.Params["S"] + "" != "") {
			SetItemInfomation(Request.Params["S"], MemberGubun);
		}
		Response.Expires = 0; 
		Response.Cache.SetNoStore(); 
		Response.AppendHeader("Pragma", "no-cache"); 
	}
	private void SetItemInfomation(string pk, string MemberGubun)
	{
		string disableIfNotAdmin = "";
		if (MemberGubun != "OurBranch")
		{
			disableIfNotAdmin = "disabled=\"disabled\"";
		}
		DBConn DB = new DBConn();

		Decimal TPriceSum = 0;
		Decimal TQuantitySum = 0;
		int TPackedCountSum = 0;
		Decimal TWeightSum = 0;
		Decimal TVolumeSum = 0;
		ItemInformationLoad = new StringBuilder();
		DB.SqlCmd.CommandText = "EXEC SP_SelectItemWithRequestFormPk @RequestFormPk=" + pk;
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		
		int RowCount = 0;
		while (RS.Read())
		{
			MonetaryUnitCL = RS["MonetaryUnitCL"] + "";
			MonetaryUnit = Common.GetMonetaryUnit(MonetaryUnitCL);

			if (RS["RequestFormItemsPk"] + "" != "")
			{
				string Temp = RS["MarkNNumber"] + "#@!" + (RS["Description"] + "").Replace("\"", "##%%$$^^") + "#@!" + RS["Label"] + "#@!" + RS["Material"] + "#@!" + Common.NumberFormat("" + RS["Quantity"]) + "#@!" + RS["QuantityUnit"] + "#@!" + RS["PackedCount"] + "#@!" + RS["PackingUnit"] + "#@!" + Common.NumberFormat("" + RS["GrossWeight"]) + "#@!" + Common.NumberFormat("" + RS["Volume"]) + "#@!" + Common.NumberFormat("" + RS["UnitPrice"]) + "#@!" + Common.NumberFormat("" + RS["Amount"]) + "#@!" + RS["RequestFormItemsPk"];
				if (RS["Amount"] + "" != "") { TPriceSum += Decimal.Parse(RS["Amount"] + ""); }
				if (RS["Quantity"] + "" != "") { TQuantitySum += Decimal.Parse(RS["Quantity"] + ""); }
				if (RS["PackedCount"] + "" != "") { TPackedCountSum += int.Parse(RS["PackedCount"] + ""); }
				if (RS["GrossWeight"] + "" != "") { TWeightSum += Decimal.Parse(RS["GrossWeight"] + ""); }
				if (RS["Volume"] + "" != "") { TVolumeSum += Decimal.Parse(RS["Volume"] + ""); }

                string[] QuantityUnit = new string[13] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
				switch (RS["QuantityUnit"] + "")
				{
					case "40": QuantityUnit[0] = "selected=\"selected\""; break;
					case "41": QuantityUnit[1] = "selected=\"selected\""; break;
					case "42": QuantityUnit[2] = "selected=\"selected\""; break;
					case "43": QuantityUnit[3] = "selected=\"selected\""; break;
					case "44": QuantityUnit[4] = "selected=\"selected\""; break;
					case "45": QuantityUnit[5] = "selected=\"selected\""; break;
					case "46": QuantityUnit[6] = "selected=\"selected\""; break;
					case "47": QuantityUnit[7] = "selected=\"selected\""; break;
					case "48": QuantityUnit[8] = "selected=\"selected\""; break;
					case "49": QuantityUnit[9] = "selected=\"selected\""; break;
					case "50": QuantityUnit[10] = "selected=\"selected\""; break;
                    case "51": QuantityUnit[11] = "selected=\"selected\""; break;
					case "52": QuantityUnit[12] = "selected=\"selected\""; break;
				}
				
				string[] PackedUnit = new string[3] { string.Empty, string.Empty, string.Empty };
				switch (RS["PackingUnit"] + "")
				{
					case "15": PackedUnit[0] = "selected=\"selected\""; break;
					case "16": PackedUnit[1] = "selected=\"selected\""; break;
					case "17": PackedUnit[2] = "selected=\"selected\""; break;
				}


				string BTNDelete = "<a onclick=\"DelThisRow('" + RS["RequestFormItemsPk"] + "', '" + (RowCount + 1) + "');\"><span style=\"color:red; cursor:hand;\">X</span></a>";
				if (MemberGubun != "OurBranch") {
					if (RS["PackedCount"] + "" != "" || RS["GrossWeight"] + "" != "" || RS["Volume"] + "" != "")
					{
						BTNDelete = "";
					}
				}

				ItemInformationLoad.Append("<tr>" +
					"<td align=\"center\" ><input type=\"text\" id=\"Item[0][" + RowCount + "][boxNo]\"  onkeyup=\"CountBox('0','" + RowCount + "',this)\" size=\"3\" value='" + RS["MarkNNumber"] + "' /></td>" +
					"<td align=\"center\" >" +
						"<input type=\"hidden\" id=\"Item[0][" + RowCount + "][BeforeItem]\" value=\"" + Temp + "\" />" +
						"<input type=\"text\" id=\"Item[0][" + RowCount + "][Brand]\" size=\"7\" value='" + RS["Label"] + "' /> / " +
						"<input type=\"text\" id=\"Item[0][" + RowCount + "][ItemName]\" size=\"12\" value='" + RS["Description"] + "' /> / " +
						"<input type=\"text\" id=\"Item[0][" + RowCount + "][Matarial]\" size=\"12\" value='" + RS["Material"] + "' />" +
					"</td >" +
					"<td align=\"center\" >" +
						"<input type=\"text\" id=\"Item[0][" + RowCount + "][Quantity]\" onkeyup=\"QuantityXUnitCost(0," + RowCount + "); \" size=\"4\" value='" + Common.NumberFormat(RS["Quantity"] + "") + "' />" +
						"<select id=\"Item[0][" + RowCount + "][QuantityUnit]\">" +
							"<option value=\"40\" " + QuantityUnit[0] + " >PCS</option>" +
							"<option value=\"41\" " + QuantityUnit[1] + " >PRS</option>" +
							"<option value=\"42\" " + QuantityUnit[2] + " >SET</option>" +
							"<option value=\"43\" " + QuantityUnit[3] + " >S/F</option>" +
							"<option value=\"44\" " + QuantityUnit[4] + " >YDS</option>" +
							"<option value=\"45\" " + QuantityUnit[5] + " >M</option>" +
							"<option value=\"46\" " + QuantityUnit[6] + " >KG</option>" +
							"<option value=\"47\" " + QuantityUnit[7] + " >DZ</option>" +
							"<option value=\"48\" " + QuantityUnit[8] + " >L</option>" +
							"<option value=\"49\" " + QuantityUnit[9] + " >BOX</option>" +
							"<option value=\"50\" " + QuantityUnit[10] + " >SQM</option>" +
                            "<option value=\"51\" " + QuantityUnit[11] + " >M2</option>" +
							"<option value=\"52\" " + QuantityUnit[12] + " >RO</option>" +
						"</select>" +
					"</td><td align=\"center\" >" +
						"<input type=\"text\" style=\"border:0;\" id=\"Item[0][" + RowCount + "][MonetaryUnit][0]\" size=\"1\" value='" + MonetaryUnit + "' />" +
						"<input type=\"text\" id=\"Item[0][" + RowCount + "][UnitCost]\" onkeyup=\"QuantityXUnitCost(0," + RowCount + "); \" size=\"2\" value='" + Common.NumberFormat(RS["UnitPrice"] + "") + "' />" +
					"</td><td align=\"center\" >" +
						"<input type=\"text\" style=\"border:0;\" id=\"Item[0][" + RowCount + "][MonetaryUnit][1]\" size=\"1\" value='" + MonetaryUnit + "' />" +
						"<input type=\"text\" id=\"Item[0][" + RowCount + "][Price]\" size=\"8\"value='" + Common.NumberFormat(RS["Amount"] + "") + "' readonly />" +
					"</td><td align=\"center\" >" +
						"<input type=\"text\" id=\"Item[0][" + RowCount + "][PackedCount]\" " + disableIfNotAdmin + " size=\"2\" onkeyup=\"GetTotal('PackedCount');\" value='" + RS["PackedCount"] + "' />" +
						"<select id=\"Item[0][" + RowCount + "][PackingUnit]\" size=\"1\"  >" +
							"<option value=\"15\" " + PackedUnit[0] + ">CT</option>" +
							"<option value=\"16\" " + PackedUnit[1] + ">RO</option>" +
							"<option value=\"17\" " + PackedUnit[2] + ">PA</option>" +
						"</select>" +
					"</td><td align=\"center\" >" +
						"<input type=\"text\" id=\"Item[0][" + RowCount + "][Weight]\" onkeyup=\"GetTotal('Weight');\" " + disableIfNotAdmin + " size=\"6\" value='" + Common.NumberFormat(RS["GrossWeight"] + "") + "' />" +
					"</td><td align=\"center\" ><input type=\"text\" id=\"Item[0][" + RowCount + "][Volume]\" onkeyup=\"GetTotal('Volume');\" " + disableIfNotAdmin + " size=\"6\" value='" + Common.NumberFormat(RS["Volume"] + "") + "' /></td>" +
					"</td><td align=\"center\" >"+BTNDelete+"</td></tr>");
			}
			RowCount++;
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = @"SELECT R.StepCL, R.TotalPackedCount, R.PackingUnit, R.TotalGrossWeight, R.TotalVolume 
		FROM RequestForm AS R 
		WHERE R.RequestFormPk=" + pk + ";";
		RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read())
		{
			StepCL = RS[0] + "";
			PackedCountSum = Common.NumberFormat(RS["TotalPackedCount"] + "");
			VolumeSum = Common.NumberFormat(RS["TotalVolume"] + "");
			WeightSum = Common.NumberFormat(RS["TotalGrossWeight"] + "");
		}
		else
		{
			if (TWeightSum != 0) { WeightSum = Common.NumberFormat(TWeightSum + ""); }
			if (TVolumeSum != 0) { VolumeSum = Common.NumberFormat(TVolumeSum + ""); }
			if (TPackedCountSum != 0) { PackedCountSum = Common.NumberFormat(TPackedCountSum + ""); }
		}
		DB.DBCon.Close();
	}
}

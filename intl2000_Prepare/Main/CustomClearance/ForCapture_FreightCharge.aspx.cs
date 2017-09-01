using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class CustomClearance_ForCapture_FreightCharge : System.Web.UI.Page
{
	protected String[] MEMBERINFO;
	protected String Title;
	protected String Header;
	protected String Item;
	protected String Freight;
	protected String BankInfo;
	protected String OnlyAdmin;
	private DBConn DB;
	private String SorC;
	protected String BTN_FAX;
	private String CompanyPk;
	private String CompanyTEL;
	private String PresidentEmail;
	private String OurBranchPk;
	protected void Page_Load(object sender, EventArgs e) {
		BTN_FAX = "<input type=\"button\" class=\"InputButton\" value=\"프린터 선택\" onclick=\"RunFAX()\" />";

		string gubun = Request.Params["T"] + "" == "" ? "" : "FreightInvoice";
		DB = new DBConn("PoolingNo");
		DB.DBCon.Open();
		SorCLoad(Request.Params["S"], Request.Params["G"]);
		Title = TitleLoad(Request.Params["S"]);

		if (gubun == "FreightInvoice") {
			Header = RequestLoadFI(Request.Params["S"]);
			Item = "";
			Freight = "";
		} else {
			Header = RequestLoad(Request.Params["S"]);
			Item = ItemLoad(Request.Params["S"]);
			Freight = FreightLoad(Request.Params["S"], SorC);
		}

		DB.DBCon.Close();
	}
	private void SorCLoad(string RequestFormPk, string RegionCode) {
		string regionTemp = "";
		SorC = RegionCode;

		if (SorC == "S" || SorC == "C") {
			asAdmin asAdmin = new asAdmin();
			OnlyAdmin = asAdmin.Get_SelectEmail(RequestFormPk, RegionCode);
		} else {
			DB.SqlCmd.CommandText = "SELECT [ShipperPk], [ConsigneePk], [DepartureRegionCode], [ArrivalRegionCode] FROM RequestForm WHERE RequestFormPk=" + RequestFormPk;
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();

			if (RS.Read()) {
				//Response.Write(DB.SqlCmd.CommandText + " / " + SorC + " / " + RS[0] + " / " + RS[1]);

				if (RS[0] + "" == SorC) {
					SorC = "S";
					CompanyPk = RS[0] + "";
					regionTemp = RS[2] + "";
				} else {
					SorC = "C";
					CompanyPk = RS[1] + "";
					regionTemp = RS[3] + "";
					//관세적용 안되는 부분때문에  아래의 if (RS[1] + "" == SorC) 적용안함
					//} else if (RS[1] + "" == SorC) {
					//	SorC = "C";
					//	CompanyPk = RS[1] + "";
					//	regionTemp = RS[3] + "";
				}
			}
			RS.Dispose();
		}
	}
	private String RequestLoad(string RequestFormPk) {
		StringBuilder Html = new StringBuilder();
		DB.SqlCmd.CommandText = "EXEC SP_FreightChargeView_RequestLoad @RequstFormPk=" + RequestFormPk + ";";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			if (SorC == "S") {
				Html.Append("<div class=\"Title\">" +
"<div style=\"border:1px black solid; padding:10px;\"><strong>" + RS["ShipperCompanyName"] + "(" + RS["ShipperCode"] + ")</strong>&nbsp;&nbsp;&nbsp; &nbsp;TEL : " + RS["ShipperComapnyTEL"] + "&nbsp;&nbsp;FAX : " + RS["ShipperCompanyFAX"] + "</div><br />" +
"<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:695px; height:130px; \">" +
"	<tr><td colspan=\"2\">Consignee. " + RS["ConsigneeCompanyName"] + "(" + RS["ConsigneeCode"] + ")</td><td>TEL : " + RS["ConsigneeCompanyTEL"] + "</td><td>FAX : " + RS["ConsigneeCompanyFAX"] + "</td></tr>" +
"	<tr><td>B / L NO</td><td>" + RS["BLNo"] + "</td><td>운송일정</td><td>" + RS["DepartureName"] + " - " + RS["ArrivalName"] + " " + Common.GetTransportWay(RS["TransportWayCL"] + "") + "</td></tr>" +
"	<tr><td>B / L TERMS</td><td>FREIGHT COLLECT</td><td>접수일자</td><td>" + RS["DepartureDate"] + "</td></tr>" +
"	<tr><td>Q'TY</td><td>" + RS["TotalPackedCount"] + " " + Common.GetPackingUnit(RS["PackingUnit"] + "") + "</td><td>출고예정</td><td>" + RS["ArrivalDate"] + "</td></tr>" +
"	<tr><td>WEIGHT</td><td>" + Common.NumberFormat("" + RS["TotalGrossWeight"]) + " KG</td><td>MEASUREMENT</td><td>" + Common.NumberFormat(RS["TotalVolume"] + "") + " CBM</td></tr>" +
"	</table></div>");
			} else {
				Html.Append("<div class=\"Title\">" +
"<div style=\"border:1px black solid; padding:10px;\"><strong>" + RS["ConsigneeCompanyName"] + "(" + RS["ConsigneeCode"] + ")</strong>&nbsp;&nbsp;&nbsp; &nbsp;TEL : " + RS["ConsigneeCompanyTEL"] + "&nbsp;&nbsp;FAX : " + RS["ConsigneeCompanyFAX"] + "</div><br />" +
"<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:695px; height:70px; \">" +
"	<tr><td colspan=\"2\">Shipper. " + RS["ShipperCompanyName"] + " (" + RS["ShipperCode"] + ")</td><td>TEL : " + RS["ShipperComapnyTEL"] + "</td><td>FAX : " + RS["ShipperCompanyFAX"] + "</td></tr>" +
"	<tr><td>B / L NO</td><td>" + RS["BLNo"] + "</td><td>운송일정</td><td>" + RS["DepartureName"] + " - " + RS["ArrivalName"] + " " + Common.GetTransportWay(RS["TransportWayCL"] + "") + "</td></tr>" +
"	<tr><td>Q'TY</td><td>" + RS["TotalPackedCount"] + " " + Common.GetPackingUnit(RS["PackingUnit"] + "") + "</td><td>&nbsp;</td><td>" +
		(RS["DepartureDate"] + "" == "" ? "" : string.Format("{0}.{1}.{2}", ("" + RS["DepartureDate"]).Substring(0, 4), ("" + RS["DepartureDate"]).Substring(4, 2), ("" + RS["DepartureDate"]).Substring(6, 2))) + " - " +
		(RS["ArrivalDate"] + "" == "" ? "" : string.Format("{0}.{1}.{2}", ("" + RS["ArrivalDate"]).Substring(0, 4), ("" + RS["ArrivalDate"]).Substring(4, 2), ("" + RS["ArrivalDate"]).Substring(6, 2))) + "</td></tr>" +
"	<tr><td>WEIGHT</td><td>" + Common.NumberFormat("" + RS["TotalGrossWeight"]) + " KG</td><td>MEASURMENT</td><td>" + Common.NumberFormat(RS["TotalVolume"] + "") + " CBM</td></tr>" +
"	</table></div>");
			}
		}
		RS.Dispose();
		return Html + "";
	}
	private decimal[] CalcForFI(decimal Value) {
		List<decimal[]> Data = new List<decimal[]>();
		Data.Add(new decimal[] { 1500, 200, 2000, 250, 19000, 10000, 5350 });
		Data.Add(new decimal[] { 1800, 220, 2800, 300, 19000, 10000, 6420 });
		Data.Add(new decimal[] { 2100, 240, 3600, 350, 19000, 10000, 7490 });
		Data.Add(new decimal[] { 2400, 260, 4400, 400, 19000, 10000, 8560 });
		Data.Add(new decimal[] { 2700, 280, 5200, 450, 19000, 10000, 9630 });
		Data.Add(new decimal[] { 3000, 300, 6000, 500, 19000, 10000, 10700 });
		Data.Add(new decimal[] { 3300, 320, 6800, 550, 19000, 12000, 11770 });
		Data.Add(new decimal[] { 3600, 340, 7600, 600, 19000, 14000, 12840 });
		Data.Add(new decimal[] { 3900, 360, 8400, 650, 19000, 16000, 13910 });
		Data.Add(new decimal[] { 4200, 380, 9200, 700, 19000, 18000, 14980 });
		Data.Add(new decimal[] { 4500, 400, 10000, 750, 19000, 20000, 16050 });
		Data.Add(new decimal[] { 4950, 420, 10650, 800, 19000, 20000, 17620 });
		Data.Add(new decimal[] { 5400, 440, 11300, 850, 19000, 20000, 19190 });
		Data.Add(new decimal[] { 5850, 460, 11950, 900, 19000, 20000, 20760 });
		Data.Add(new decimal[] { 6300, 480, 12600, 950, 19000, 20000, 22330 });
		Data.Add(new decimal[] { 6750, 500, 13250, 1000, 19000, 20000, 23900 });
		Data.Add(new decimal[] { 7200, 520, 13900, 1050, 19000, 22000, 25460 });
		Data.Add(new decimal[] { 7650, 540, 14550, 1100, 19000, 24000, 27020 });
		Data.Add(new decimal[] { 8100, 560, 15200, 1150, 19000, 26000, 28580 });
		Data.Add(new decimal[] { 8550, 580, 15850, 1200, 19000, 28000, 30140 });
		Data.Add(new decimal[] { 9000, 600, 16500, 1250, 19000, 30000, 31700 });
		Data.Add(new decimal[] { 9600, 620, 17050, 1300, 19000, 30000, 32968 });
		Data.Add(new decimal[] { 10200, 640, 17600, 1350, 19000, 30000, 34236 });
		Data.Add(new decimal[] { 10800, 660, 18150, 1400, 19000, 30000, 35504 });
		Data.Add(new decimal[] { 11400, 680, 18700, 1450, 19000, 30000, 36772 });
		Data.Add(new decimal[] { 12000, 700, 19250, 1500, 19000, 30000, 38040 });
		Data.Add(new decimal[] { 12600, 720, 19800, 1550, 19000, 31200, 39308 });
		Data.Add(new decimal[] { 13200, 740, 20350, 1600, 19000, 32400, 40576 });
		Data.Add(new decimal[] { 13800, 760, 20900, 1650, 19000, 33600, 41844 });
		Data.Add(new decimal[] { 14400, 780, 21450, 1700, 19000, 34800, 43112 });
		Data.Add(new decimal[] { 15000, 800, 22000, 1750, 19000, 36000, 44380 });
		Data.Add(new decimal[] { 15600, 820, 22550, 1800, 19000, 36000, 45648 });
		Data.Add(new decimal[] { 16200, 840, 23100, 1850, 19000, 36000, 46916 });
		Data.Add(new decimal[] { 16800, 860, 23650, 1900, 19000, 36000, 48184 });
		Data.Add(new decimal[] { 17400, 880, 24200, 1950, 19000, 36000, 49452 });
		Data.Add(new decimal[] { 18000, 900, 24750, 2000, 19000, 36000, 50720 });
		Data.Add(new decimal[] { 18600, 920, 25300, 2050, 19000, 36000, 51988 });
		Data.Add(new decimal[] { 19200, 940, 25850, 2100, 19000, 36000, 53256 });
		Data.Add(new decimal[] { 19800, 960, 26400, 2150, 19000, 36000, 54524 });
		Data.Add(new decimal[] { 20400, 980, 26950, 2200, 19000, 36000, 55792 });
		Data.Add(new decimal[] { 21000, 1000, 27500, 2250, 19000, 36000, 57060 });
		Data.Add(new decimal[] { 21600, 1020, 28050, 2300, 19000, 36000, 58328 });
		Data.Add(new decimal[] { 22200, 1040, 28600, 2350, 19000, 36000, 59596 });
		Data.Add(new decimal[] { 22800, 1060, 29150, 2400, 19000, 36000, 60864 });
		Data.Add(new decimal[] { 23400, 1080, 29700, 2450, 19000, 36000, 62132 });
		Data.Add(new decimal[] { 24000, 1100, 30250, 2500, 19000, 36000, 63400 });
		Data.Add(new decimal[] { 24600, 1120, 30800, 2550, 19000, 36000, 64668 });
		Data.Add(new decimal[] { 25200, 1140, 31350, 2600, 19000, 36000, 65936 });
		Data.Add(new decimal[] { 25800, 1160, 31900, 2650, 19000, 36000, 67204 });
		Data.Add(new decimal[] { 26400, 1180, 32450, 2700, 19000, 36000, 68472 });
		Data.Add(new decimal[] { 27000, 1200, 33000, 2750, 19000, 36000, 69740 });
		Data.Add(new decimal[] { 27600, 1220, 33550, 2800, 19000, 36000, 71008 });
		Data.Add(new decimal[] { 28200, 1240, 34100, 2850, 19000, 36000, 72276 });
		Data.Add(new decimal[] { 28800, 1260, 34650, 2900, 19000, 36000, 73544 });
		Data.Add(new decimal[] { 29400, 1280, 35200, 2950, 19000, 36000, 74812 });
		Data.Add(new decimal[] { 30000, 1300, 35750, 3000, 19000, 36000, 76080 });
		Data.Add(new decimal[] { 30600, 1320, 36300, 3050, 19000, 36000, 77348 });
		Data.Add(new decimal[] { 31200, 1340, 36850, 3100, 19000, 36000, 78616 });
		Data.Add(new decimal[] { 31800, 1360, 37400, 3150, 19000, 36000, 79884 });
		Data.Add(new decimal[] { 32400, 1380, 37950, 3200, 19000, 36000, 81152 });
		Data.Add(new decimal[] { 33000, 1400, 38500, 3250, 19000, 36000, 82420 });
		Data.Add(new decimal[] { 33600, 1420, 39050, 3300, 19000, 36000, 83688 });
		Data.Add(new decimal[] { 34200, 1440, 39600, 3350, 19000, 36000, 84956 });
		Data.Add(new decimal[] { 34800, 1460, 40150, 3400, 19000, 36000, 86224 });
		Data.Add(new decimal[] { 35400, 1480, 40700, 3450, 19000, 36000, 87492 });
		Data.Add(new decimal[] { 36000, 1500, 41250, 3500, 19000, 36000, 88760 });
		Data.Add(new decimal[] { 36600, 1520, 41800, 3550, 19000, 36000, 90028 });
		Data.Add(new decimal[] { 37200, 1540, 42350, 3600, 19000, 36000, 91296 });
		Data.Add(new decimal[] { 37800, 1560, 42900, 3650, 19000, 36000, 92564 });
		Data.Add(new decimal[] { 38400, 1580, 43450, 3700, 19000, 36000, 93832 });
		Data.Add(new decimal[] { 39000, 1600, 44000, 3750, 19000, 36000, 95100 });
		Data.Add(new decimal[] { 39600, 1620, 44550, 3800, 19000, 36000, 96368 });
		Data.Add(new decimal[] { 40200, 1640, 45100, 3850, 19000, 36000, 97636 });
		Data.Add(new decimal[] { 40800, 1660, 45650, 3900, 19000, 36000, 98904 });
		Data.Add(new decimal[] { 41400, 1680, 46200, 3950, 19000, 36000, 100172 });
		Data.Add(new decimal[] { 42000, 1700, 46750, 4000, 19000, 36000, 101440 });
		Data.Add(new decimal[] { 42600, 1720, 47300, 4050, 19000, 36000, 102708 });
		Data.Add(new decimal[] { 43200, 1740, 47850, 4100, 19000, 36000, 103976 });
		Data.Add(new decimal[] { 43800, 1760, 48400, 4150, 19000, 36000, 105244 });
		Data.Add(new decimal[] { 44400, 1780, 48950, 4200, 19000, 36000, 106512 });
		Data.Add(new decimal[] { 45000, 1800, 49500, 4250, 19000, 36000, 107780 });
		Data.Add(new decimal[] { 45600, 1820, 50050, 4300, 19000, 36000, 109048 });
		Data.Add(new decimal[] { 46200, 1840, 50600, 4350, 19000, 36000, 110316 });
		Data.Add(new decimal[] { 46800, 1860, 51150, 4400, 19000, 36000, 111584 });
		Data.Add(new decimal[] { 47400, 1880, 51700, 4450, 19000, 36000, 112852 });
		Data.Add(new decimal[] { 48000, 1900, 52250, 4500, 19000, 36000, 114120 });
		Data.Add(new decimal[] { 48600, 1920, 52800, 4550, 19000, 36000, 115388 });
		Data.Add(new decimal[] { 49200, 1940, 53350, 4600, 19000, 36000, 116656 });
		Data.Add(new decimal[] { 49800, 1960, 53900, 4650, 19000, 36000, 117924 });
		Data.Add(new decimal[] { 50400, 1980, 54450, 4700, 19000, 36000, 119192 });
		Data.Add(new decimal[] { 51000, 2000, 55000, 4750, 19000, 36000, 120460 });
		Data.Add(new decimal[] { 51600, 2020, 55550, 4800, 19000, 36000, 121728 });
		Data.Add(new decimal[] { 52200, 2040, 56100, 4850, 19000, 36000, 122996 });
		Data.Add(new decimal[] { 52800, 2060, 56650, 4900, 19000, 36000, 124264 });
		Data.Add(new decimal[] { 53400, 2080, 57200, 4950, 19000, 36000, 125532 });
		Data.Add(new decimal[] { 54000, 2100, 57750, 5000, 19000, 36000, 126800 });
		Data.Add(new decimal[] { 54600, 2120, 58300, 5050, 19000, 36000, 128068 });
		Data.Add(new decimal[] { 55200, 2140, 58850, 5100, 19000, 36000, 129336 });
		Data.Add(new decimal[] { 55800, 2160, 59400, 5150, 19000, 36000, 130604 });
		Data.Add(new decimal[] { 56400, 2180, 59950, 5200, 19000, 36000, 131872 });
		Data.Add(new decimal[] { 57000, 2200, 60500, 5250, 19000, 36000, 133140 });
		Data.Add(new decimal[] { 57600, 2220, 61050, 5300, 19000, 36000, 134408 });
		Data.Add(new decimal[] { 58200, 2240, 61600, 5350, 19000, 36000, 135676 });
		Data.Add(new decimal[] { 58800, 2260, 62150, 5400, 19000, 36000, 136944 });
		Data.Add(new decimal[] { 59400, 2280, 62700, 5450, 19000, 36000, 138212 });
		Data.Add(new decimal[] { 60000, 2300, 63250, 5500, 19000, 36000, 139480 });
		Data.Add(new decimal[] { 60600, 2320, 63800, 5550, 19000, 36000, 140748 });
		Data.Add(new decimal[] { 61200, 2340, 64350, 5600, 19000, 36000, 142016 });
		Data.Add(new decimal[] { 61800, 2360, 64900, 5650, 19000, 36000, 143284 });
		Data.Add(new decimal[] { 62400, 2380, 65450, 5700, 19000, 36000, 144552 });
		Data.Add(new decimal[] { 63000, 2400, 66000, 5750, 19000, 36000, 145820 });
		Data.Add(new decimal[] { 63600, 2420, 66550, 5800, 19000, 36000, 147088 });
		Data.Add(new decimal[] { 64200, 2440, 67100, 5850, 19000, 36000, 148356 });
		Data.Add(new decimal[] { 64800, 2460, 67650, 5900, 19000, 36000, 149624 });
		Data.Add(new decimal[] { 65400, 2480, 68200, 5950, 19000, 36000, 150892 });
		Data.Add(new decimal[] { 66000, 2500, 68750, 6000, 19000, 36000, 152160 });
		Data.Add(new decimal[] { 66600, 2520, 69300, 6050, 19000, 36000, 153428 });
		Data.Add(new decimal[] { 67200, 2540, 69850, 6100, 19000, 36000, 154696 });
		Data.Add(new decimal[] { 67800, 2560, 70400, 6150, 19000, 36000, 155964 });
		Data.Add(new decimal[] { 68400, 2580, 70950, 6200, 19000, 36000, 157232 });
		Data.Add(new decimal[] { 69000, 2600, 71500, 6250, 19000, 36000, 158500 });
		Data.Add(new decimal[] { 69600, 2620, 72050, 6300, 19000, 36000, 159768 });
		Data.Add(new decimal[] { 70200, 2640, 72600, 6350, 19000, 36000, 161036 });
		Data.Add(new decimal[] { 70800, 2660, 73150, 6400, 19000, 36000, 162304 });
		Data.Add(new decimal[] { 71400, 2680, 73700, 6450, 19000, 36000, 163572 });
		Data.Add(new decimal[] { 72000, 2700, 74250, 6500, 19000, 36000, 164840 });
		Data.Add(new decimal[] { 72600, 2720, 74800, 6550, 19000, 36000, 166108 });
		Data.Add(new decimal[] { 73200, 2740, 75350, 6600, 19000, 36000, 167376 });
		Data.Add(new decimal[] { 73800, 2760, 75900, 6650, 19000, 36000, 168644 });
		Data.Add(new decimal[] { 74400, 2780, 76450, 6700, 19000, 36000, 169912 });
		Data.Add(new decimal[] { 75000, 2800, 77000, 6750, 19000, 36000, 171180 });
		Data.Add(new decimal[] { 75600, 2820, 77550, 6800, 19000, 36000, 172448 });
		Data.Add(new decimal[] { 76200, 2840, 78100, 6850, 19000, 36000, 173716 });
		Data.Add(new decimal[] { 76800, 2860, 78650, 6900, 19000, 36000, 174984 });
		Data.Add(new decimal[] { 77400, 2880, 79200, 6950, 19000, 36000, 176252 });
		Data.Add(new decimal[] { 78000, 2900, 79750, 7000, 19000, 36000, 177520 });
		Data.Add(new decimal[] { 78600, 2920, 80300, 7050, 19000, 36000, 178788 });
		Data.Add(new decimal[] { 79200, 2940, 80850, 7100, 19000, 36000, 180056 });
		Data.Add(new decimal[] { 79800, 2960, 81400, 7150, 19000, 36000, 181324 });
		Data.Add(new decimal[] { 80400, 2980, 81950, 7200, 19000, 36000, 182592 });
		Data.Add(new decimal[] { 81000, 3000, 82500, 7250, 19000, 36000, 183860 });
		Data.Add(new decimal[] { 81600, 3020, 83050, 7300, 19000, 36000, 185128 });
		Data.Add(new decimal[] { 82200, 3040, 83600, 7350, 19000, 36000, 186396 });
		Data.Add(new decimal[] { 82800, 3060, 84150, 7400, 19000, 36000, 187664 });
		Data.Add(new decimal[] { 83400, 3080, 84700, 7450, 19000, 36000, 188932 });
		Data.Add(new decimal[] { 84000, 3100, 85250, 7500, 19000, 36000, 190200 });
		Data.Add(new decimal[] { 84600, 3120, 85800, 7550, 19000, 36000, 191468 });
		Data.Add(new decimal[] { 85200, 3140, 86350, 7600, 19000, 36000, 192736 });
		Data.Add(new decimal[] { 85800, 3160, 86900, 7650, 19000, 36000, 194004 });
		Data.Add(new decimal[] { 86400, 3180, 87450, 7700, 19000, 36000, 195272 });
		Data.Add(new decimal[] { 87000, 3200, 88000, 7750, 19000, 36000, 196540 });
		Data.Add(new decimal[] { 87600, 3220, 88550, 7800, 19000, 36000, 197808 });
		Data.Add(new decimal[] { 88200, 3240, 89100, 7850, 19000, 36000, 199076 });
		Data.Add(new decimal[] { 88800, 3260, 89650, 7900, 19000, 36000, 200344 });
		Data.Add(new decimal[] { 89400, 3280, 90200, 7950, 19000, 36000, 201612 });
		Data.Add(new decimal[] { 90000, 3300, 90750, 8000, 19000, 36000, 202880 });
		Data.Add(new decimal[] { 90600, 3320, 91300, 8050, 19000, 36000, 204148 });
		Data.Add(new decimal[] { 91200, 3340, 91850, 8100, 19000, 36000, 205416 });
		Data.Add(new decimal[] { 91800, 3360, 92400, 8150, 19000, 36000, 206684 });
		Data.Add(new decimal[] { 92400, 3380, 92950, 8200, 19000, 36000, 207952 });
		Data.Add(new decimal[] { 93000, 3400, 93500, 8250, 19000, 36000, 209220 });
		Data.Add(new decimal[] { 93600, 3420, 94050, 8300, 19000, 36000, 210488 });
		Data.Add(new decimal[] { 94200, 3440, 94600, 8350, 19000, 36000, 211756 });
		Data.Add(new decimal[] { 94800, 3460, 95150, 8400, 19000, 36000, 213024 });
		Data.Add(new decimal[] { 95400, 3480, 95700, 8450, 19000, 36000, 214292 });
		Data.Add(new decimal[] { 96000, 3500, 96250, 8500, 19000, 36000, 215560 });
		Data.Add(new decimal[] { 96600, 3520, 96800, 8550, 19000, 36000, 216828 });
		Data.Add(new decimal[] { 97200, 3540, 97350, 8600, 19000, 36000, 218096 });
		Data.Add(new decimal[] { 97800, 3560, 97900, 8650, 19000, 36000, 219364 });
		Data.Add(new decimal[] { 98400, 3580, 98450, 8700, 19000, 36000, 220632 });
		Data.Add(new decimal[] { 99000, 3600, 99000, 8750, 19000, 36000, 221900 });
		Data.Add(new decimal[] { 99400, 3620, 99550, 8800, 19000, 36000, 223168 });
		Data.Add(new decimal[] { 99800, 3640, 100100, 8850, 19000, 36000, 224436 });
		Data.Add(new decimal[] { 100200, 3660, 100650, 8900, 19000, 36000, 225704 });
		Data.Add(new decimal[] { 100600, 3680, 101200, 8950, 19000, 36000, 226972 });
		Data.Add(new decimal[] { 101000, 3700, 101750, 9000, 19000, 36000, 228240 });
		Data.Add(new decimal[] { 101000, 3720, 102300, 9050, 19000, 36000, 229508 });
		Data.Add(new decimal[] { 101000, 3740, 102850, 9100, 19000, 36000, 230776 });
		Data.Add(new decimal[] { 101000, 3760, 103400, 9150, 19000, 36000, 232044 });
		Data.Add(new decimal[] { 101000, 3780, 103950, 9200, 19000, 36000, 233312 });
		Data.Add(new decimal[] { 101000, 3800, 104500, 9250, 19000, 36000, 234580 });
		Data.Add(new decimal[] { 101000, 3820, 105050, 9300, 19000, 36000, 235848 });
		Data.Add(new decimal[] { 101000, 3840, 105600, 9350, 19000, 36000, 237116 });
		Data.Add(new decimal[] { 101000, 3860, 106150, 9400, 19000, 36000, 238384 });
		Data.Add(new decimal[] { 101000, 3880, 106700, 9450, 19000, 36000, 239652 });
		Data.Add(new decimal[] { 101000, 3900, 107250, 9500, 19000, 36000, 240920 });
		Data.Add(new decimal[] { 101000, 3920, 107800, 9550, 19000, 36000, 242188 });
		Data.Add(new decimal[] { 101000, 3940, 108350, 9600, 19000, 36000, 243456 });
		Data.Add(new decimal[] { 101000, 3960, 108900, 9650, 19000, 36000, 244724 });
		Data.Add(new decimal[] { 101000, 3980, 109450, 9700, 19000, 36000, 245992 });
		Data.Add(new decimal[] { 101000, 4000, 110000, 9750, 19000, 36000, 247260 });
		Data.Add(new decimal[] { 101000, 4020, 110550, 9800, 19000, 36000, 248528 });
		Data.Add(new decimal[] { 101000, 4040, 111100, 9850, 19000, 36000, 249796 });
		Data.Add(new decimal[] { 101000, 4060, 111650, 9900, 19000, 36000, 251064 });
		Data.Add(new decimal[] { 101000, 4080, 112200, 9950, 19000, 36000, 252332 });
		Data.Add(new decimal[] { 101000, 4100, 112750, 10000, 19000, 36000, 253600 });
		Data.Add(new decimal[] { 101000, 4120, 113300, 10050, 19000, 36000, 254868 });
		Data.Add(new decimal[] { 101000, 4140, 113850, 10100, 19000, 36000, 256136 });
		Data.Add(new decimal[] { 101000, 4160, 114400, 10150, 19000, 36000, 257404 });
		Data.Add(new decimal[] { 101000, 4180, 114950, 10200, 19000, 36000, 258672 });
		Data.Add(new decimal[] { 101000, 4200, 115500, 10250, 19000, 36000, 259940 });
		Data.Add(new decimal[] { 101000, 4200, 116050, 10300, 19000, 36000, 261208 });
		Data.Add(new decimal[] { 101000, 4200, 116600, 10350, 19000, 36000, 262476 });
		Data.Add(new decimal[] { 101000, 4200, 117150, 10400, 19000, 36000, 263744 });
		Data.Add(new decimal[] { 101000, 4200, 117700, 10450, 19000, 36000, 265012 });
		Data.Add(new decimal[] { 101000, 4200, 118250, 10500, 19000, 36000, 266280 });
		Data.Add(new decimal[] { 101000, 4200, 118800, 10550, 19000, 36000, 267548 });
		Data.Add(new decimal[] { 101000, 4200, 119350, 10600, 19000, 36000, 268816 });
		Data.Add(new decimal[] { 101000, 4200, 119900, 10650, 19000, 36000, 270084 });
		Data.Add(new decimal[] { 101000, 4200, 120450, 10700, 19000, 36000, 271352 });
		Data.Add(new decimal[] { 101000, 4200, 121000, 10750, 19000, 36000, 272620 });
		Data.Add(new decimal[] { 101000, 4200, 121550, 10800, 19000, 36000, 273888 });
		Data.Add(new decimal[] { 101000, 4200, 122100, 10850, 19000, 36000, 275156 });
		Data.Add(new decimal[] { 101000, 4200, 122650, 10900, 19000, 36000, 276424 });
		Data.Add(new decimal[] { 101000, 4200, 123200, 10950, 19000, 36000, 277692 });
		Data.Add(new decimal[] { 101000, 4200, 123750, 11000, 19000, 36000, 278960 });
		Data.Add(new decimal[] { 101000, 4200, 124300, 11050, 19000, 36000, 280228 });
		Data.Add(new decimal[] { 101000, 4200, 124850, 11100, 19000, 36000, 281496 });
		Data.Add(new decimal[] { 101000, 4200, 125400, 11150, 19000, 36000, 282764 });
		Data.Add(new decimal[] { 101000, 4200, 125950, 11200, 19000, 36000, 284032 });
		Data.Add(new decimal[] { 101000, 4200, 126500, 11250, 19000, 36000, 285300 });
		Data.Add(new decimal[] { 101000, 4200, 127050, 11300, 19000, 36000, 286568 });
		Data.Add(new decimal[] { 101000, 4200, 127600, 11350, 19000, 36000, 287836 });
		Data.Add(new decimal[] { 101000, 4200, 128150, 11400, 19000, 36000, 289104 });
		Data.Add(new decimal[] { 101000, 4200, 128700, 11450, 19000, 36000, 290372 });
		Data.Add(new decimal[] { 101000, 4200, 129250, 11500, 19000, 36000, 291640 });
		Data.Add(new decimal[] { 101000, 4200, 129800, 11550, 19000, 36000, 292908 });
		Data.Add(new decimal[] { 101000, 4200, 130350, 11600, 19000, 36000, 294176 });
		Data.Add(new decimal[] { 101000, 4200, 130900, 11650, 19000, 36000, 295444 });
		Data.Add(new decimal[] { 101000, 4200, 131450, 11700, 19000, 36000, 296712 });
		Data.Add(new decimal[] { 101000, 4200, 132000, 11750, 19000, 36000, 297980 });
		Data.Add(new decimal[] { 101000, 4200, 132550, 11800, 19000, 36000, 299248 });
		Data.Add(new decimal[] { 101000, 4200, 133100, 11850, 19000, 36000, 300516 });
		Data.Add(new decimal[] { 101000, 4200, 133650, 11900, 19000, 36000, 301784 });
		Data.Add(new decimal[] { 101000, 4200, 134200, 11950, 19000, 36000, 303052 });
		Data.Add(new decimal[] { 101000, 4200, 134750, 12000, 19000, 36000, 304320 });
		Data.Add(new decimal[] { 101000, 4200, 135300, 12050, 19000, 36000, 305588 });
		Data.Add(new decimal[] { 101000, 4200, 135850, 12100, 19000, 36000, 306856 });
		Data.Add(new decimal[] { 101000, 4200, 136400, 12150, 19000, 36000, 308124 });
		Data.Add(new decimal[] { 101000, 4200, 136950, 12200, 19000, 36000, 309392 });
		Data.Add(new decimal[] { 101000, 4200, 137500, 12250, 19000, 36000, 310660 });
		Data.Add(new decimal[] { 101000, 4200, 138050, 12300, 19000, 36000, 311928 });
		Data.Add(new decimal[] { 101000, 4200, 138600, 12350, 19000, 36000, 313196 });
		Data.Add(new decimal[] { 101000, 4200, 139150, 12400, 19000, 36000, 314464 });
		Data.Add(new decimal[] { 101000, 4200, 139700, 12450, 19000, 36000, 315732 });
		Data.Add(new decimal[] { 101000, 4200, 140250, 12500, 19000, 36000, 317000 });
		Data.Add(new decimal[] { 101000, 4200, 140800, 12550, 19000, 36000, 318268 });
		Data.Add(new decimal[] { 101000, 4200, 141350, 12600, 19000, 36000, 319536 });
		Data.Add(new decimal[] { 101000, 4200, 141900, 12650, 19000, 36000, 320804 });
		Data.Add(new decimal[] { 101000, 4200, 142450, 12700, 19000, 36000, 322072 });
		Data.Add(new decimal[] { 101000, 4200, 143000, 12750, 19000, 36000, 323340 });
		Data.Add(new decimal[] { 101000, 4200, 143550, 12800, 19000, 36000, 324608 });
		Data.Add(new decimal[] { 101000, 4200, 144100, 12850, 19000, 36000, 325876 });
		Data.Add(new decimal[] { 101000, 4200, 144650, 12900, 19000, 36000, 327144 });
		Data.Add(new decimal[] { 101000, 4200, 145200, 12950, 19000, 36000, 328412 });
		Data.Add(new decimal[] { 101000, 4200, 145750, 13000, 19000, 36000, 329680 });
		Data.Add(new decimal[] { 101000, 4200, 145860, 13050, 19000, 36000, 330948 });
		Data.Add(new decimal[] { 101000, 4200, 145970, 13100, 19000, 36000, 332216 });
		Data.Add(new decimal[] { 101000, 4200, 146080, 13150, 19000, 36000, 333484 });
		Data.Add(new decimal[] { 101000, 4200, 146190, 13200, 19000, 36000, 334752 });
		Data.Add(new decimal[] { 101000, 4200, 146300, 13250, 19000, 36000, 336020 });
		Data.Add(new decimal[] { 101000, 4220, 146300, 13300, 19000, 36000, 337288 });
		Data.Add(new decimal[] { 101000, 4240, 146300, 13350, 19000, 36000, 338556 });
		Data.Add(new decimal[] { 101000, 4260, 146300, 13400, 19000, 36000, 339824 });
		Data.Add(new decimal[] { 101000, 4280, 146300, 13450, 19000, 36000, 341092 });
		Data.Add(new decimal[] { 101000, 4300, 146300, 13500, 19000, 36000, 342360 });
		Data.Add(new decimal[] { 101000, 4320, 146300, 13550, 19000, 36000, 343628 });
		Data.Add(new decimal[] { 101000, 4340, 146300, 13600, 19000, 36000, 344896 });
		Data.Add(new decimal[] { 101000, 4360, 146300, 13650, 19000, 36000, 346164 });
		Data.Add(new decimal[] { 101000, 4380, 146300, 13700, 19000, 36000, 347432 });
		Data.Add(new decimal[] { 101000, 4400, 146300, 13750, 19000, 36000, 348700 });
		Data.Add(new decimal[] { 101000, 4420, 146300, 13800, 19000, 36000, 349968 });
		Data.Add(new decimal[] { 101000, 4440, 146300, 13850, 19000, 36000, 351236 });
		Data.Add(new decimal[] { 101000, 4460, 146300, 13900, 19000, 36000, 352504 });
		Data.Add(new decimal[] { 101000, 4480, 146300, 13950, 19000, 36000, 353772 });
		Data.Add(new decimal[] { 101000, 4500, 146300, 14000, 19000, 36000, 355040 });
		Data.Add(new decimal[] { 101600, 4520, 146600, 14050, 19000, 36000, 356308 });
		Data.Add(new decimal[] { 102200, 4540, 146900, 14100, 19000, 36000, 357576 });
		Data.Add(new decimal[] { 102800, 4560, 147200, 14150, 19000, 36000, 358844 });
		Data.Add(new decimal[] { 103400, 4580, 147500, 14200, 19000, 36000, 360112 });
		Data.Add(new decimal[] { 104000, 4600, 147800, 14250, 19000, 36000, 361380 });
		Data.Add(new decimal[] { 104600, 4620, 148100, 14300, 19000, 36000, 362648 });
		Data.Add(new decimal[] { 105200, 4640, 148400, 14350, 19000, 36000, 363916 });
		Data.Add(new decimal[] { 105800, 4660, 148700, 14400, 19000, 36000, 365184 });
		Data.Add(new decimal[] { 106400, 4680, 149000, 14450, 19000, 36000, 366452 });
		Data.Add(new decimal[] { 107000, 4700, 149300, 14500, 19000, 36000, 367720 });
		Data.Add(new decimal[] { 107600, 4720, 149600, 14550, 19000, 36000, 368988 });
		Data.Add(new decimal[] { 108200, 4740, 149900, 14600, 19000, 36000, 370256 });
		Data.Add(new decimal[] { 108800, 4760, 150200, 14650, 19000, 36000, 371524 });
		Data.Add(new decimal[] { 109400, 4780, 150500, 14700, 19000, 36000, 372792 });
		Data.Add(new decimal[] { 110000, 4800, 150800, 14750, 19000, 36000, 374060 });
		Data.Add(new decimal[] { 110600, 4820, 151100, 14800, 19000, 36000, 375328 });
		Data.Add(new decimal[] { 111200, 4840, 151400, 14850, 19000, 36000, 376596 });
		Data.Add(new decimal[] { 111800, 4860, 151700, 14900, 19000, 36000, 377864 });
		Data.Add(new decimal[] { 112400, 4880, 152000, 14950, 19000, 36000, 379132 });
		Data.Add(new decimal[] { 113000, 4900, 152300, 15000, 19000, 36000, 380400 });
		Data.Add(new decimal[] { 113600, 4920, 152600, 15050, 19000, 36000, 381668 });
		Data.Add(new decimal[] { 114200, 4940, 152900, 15100, 19000, 36000, 382936 });
		Data.Add(new decimal[] { 114800, 4960, 153200, 15150, 19000, 36000, 384204 });
		Data.Add(new decimal[] { 115400, 4980, 153500, 15200, 19000, 36000, 385472 });
		Data.Add(new decimal[] { 116000, 5000, 153800, 15250, 19000, 36000, 386740 });
		Data.Add(new decimal[] { 116600, 5020, 154100, 15300, 19000, 36000, 388008 });
		Data.Add(new decimal[] { 117200, 5040, 154400, 15350, 19000, 36000, 389276 });
		Data.Add(new decimal[] { 117800, 5060, 154700, 15400, 19000, 36000, 390544 });
		Data.Add(new decimal[] { 118400, 5080, 155000, 15450, 19000, 36000, 391812 });
		Data.Add(new decimal[] { 119000, 5100, 155300, 15500, 19000, 36000, 393080 });
		Data.Add(new decimal[] { 119600, 5120, 155600, 15550, 19000, 36000, 394348 });
		Data.Add(new decimal[] { 120200, 5140, 155900, 15600, 19000, 36000, 395616 });
		Data.Add(new decimal[] { 120800, 5160, 156200, 15650, 19000, 36000, 396884 });
		Data.Add(new decimal[] { 121400, 5180, 156500, 15700, 19000, 36000, 398152 });
		Data.Add(new decimal[] { 122000, 5200, 156800, 15750, 19000, 36000, 399420 });
		Data.Add(new decimal[] { 122600, 5220, 157100, 15800, 19000, 36000, 400688 });
		Data.Add(new decimal[] { 123200, 5240, 157400, 15850, 19000, 36000, 401956 });
		Data.Add(new decimal[] { 123800, 5260, 157700, 15900, 19000, 36000, 403224 });
		Data.Add(new decimal[] { 124400, 5280, 158000, 15950, 19000, 36000, 404492 });
		Data.Add(new decimal[] { 125000, 5300, 158300, 16000, 19000, 36000, 405760 });
		Data.Add(new decimal[] { 125600, 5320, 158600, 16050, 19000, 36000, 407028 });
		Data.Add(new decimal[] { 126200, 5340, 158900, 16100, 19000, 36000, 408296 });
		Data.Add(new decimal[] { 126800, 5360, 159200, 16150, 19000, 36000, 409564 });
		Data.Add(new decimal[] { 127400, 5380, 159500, 16200, 19000, 36000, 410832 });
		Data.Add(new decimal[] { 128000, 5400, 159800, 16250, 19000, 36000, 412100 });
		Data.Add(new decimal[] { 128600, 5420, 160100, 16300, 19000, 36000, 413368 });
		Data.Add(new decimal[] { 129200, 5440, 160400, 16350, 19000, 36000, 414636 });
		Data.Add(new decimal[] { 129800, 5460, 160700, 16400, 19000, 36000, 415904 });
		Data.Add(new decimal[] { 130400, 5480, 161000, 16450, 19000, 36000, 417172 });
		Data.Add(new decimal[] { 131000, 5500, 161300, 16500, 19000, 36000, 418440 });
		Data.Add(new decimal[] { 131600, 5520, 161600, 16550, 19000, 36000, 419708 });
		Data.Add(new decimal[] { 132200, 5540, 161900, 16600, 19000, 36000, 420976 });
		Data.Add(new decimal[] { 132800, 5560, 162200, 16650, 19000, 36000, 422244 });
		Data.Add(new decimal[] { 133400, 5580, 162500, 16700, 19000, 36000, 423512 });
		Data.Add(new decimal[] { 134000, 5600, 162800, 16750, 19000, 36000, 424780 });
		Data.Add(new decimal[] { 134600, 5620, 162800, 16800, 19000, 36000, 426048 });
		Data.Add(new decimal[] { 135200, 5640, 162800, 16850, 19000, 36000, 427316 });
		Data.Add(new decimal[] { 135800, 5660, 162800, 16900, 19000, 36000, 428584 });
		Data.Add(new decimal[] { 136400, 5680, 162800, 16950, 19000, 36000, 429852 });
		Data.Add(new decimal[] { 137000, 5700, 162800, 17000, 19000, 36000, 431120 });
		Data.Add(new decimal[] { 137000, 5720, 162800, 17050, 19000, 36000, 432388 });
		Data.Add(new decimal[] { 137000, 5740, 162800, 17100, 19000, 36000, 433656 });
		Data.Add(new decimal[] { 137000, 5760, 162800, 17150, 19000, 36000, 434924 });
		Data.Add(new decimal[] { 137000, 5780, 162800, 17200, 19000, 36000, 436192 });
		Data.Add(new decimal[] { 137000, 5800, 162800, 17250, 19000, 36000, 437460 });
		Data.Add(new decimal[] { 137000, 5820, 162800, 17300, 19000, 36000, 438728 });
		Data.Add(new decimal[] { 137000, 5840, 162800, 17350, 19000, 36000, 439996 });
		Data.Add(new decimal[] { 137000, 5860, 162800, 17400, 19000, 36000, 441264 });
		Data.Add(new decimal[] { 137000, 5880, 162800, 17450, 19000, 36000, 442532 });
		Data.Add(new decimal[] { 137000, 5900, 162800, 17500, 19000, 36000, 443800 });
		Data.Add(new decimal[] { 137000, 5920, 162800, 17550, 19000, 36000, 445068 });
		Data.Add(new decimal[] { 137000, 5940, 162800, 17600, 19000, 36000, 446336 });
		Data.Add(new decimal[] { 137000, 5960, 162800, 17650, 19000, 36000, 447604 });
		Data.Add(new decimal[] { 137000, 5980, 162800, 17700, 19000, 36000, 448872 });
		Data.Add(new decimal[] { 137000, 6000, 162800, 17750, 19000, 36000, 450140 });
		Data.Add(new decimal[] { 137000, 6020, 162800, 17800, 19000, 36000, 451408 });
		Data.Add(new decimal[] { 137000, 6040, 162800, 17850, 19000, 36000, 452676 });
		Data.Add(new decimal[] { 137000, 6060, 162800, 17900, 19000, 36000, 453944 });
		Data.Add(new decimal[] { 137000, 6080, 162800, 17950, 19000, 36000, 455212 });
		Data.Add(new decimal[] { 137000, 6100, 162800, 18000, 19000, 36000, 456480 });
		Data.Add(new decimal[] { 137000, 6120, 162800, 18050, 19000, 36000, 457748 });
		Data.Add(new decimal[] { 137000, 6140, 162800, 18100, 19000, 36000, 459016 });
		Data.Add(new decimal[] { 137000, 6160, 162800, 18150, 19000, 36000, 460284 });
		Data.Add(new decimal[] { 137000, 6180, 162800, 18200, 19000, 36000, 461552 });
		Data.Add(new decimal[] { 137000, 6200, 162800, 18250, 19000, 36000, 462820 });
		Data.Add(new decimal[] { 137000, 6220, 162800, 18300, 19000, 36000, 464088 });
		Data.Add(new decimal[] { 137000, 6240, 162800, 18350, 19000, 36000, 465356 });
		Data.Add(new decimal[] { 137000, 6260, 162800, 18400, 19000, 36000, 466624 });
		Data.Add(new decimal[] { 137000, 6280, 162800, 18450, 19000, 36000, 467892 });
		Data.Add(new decimal[] { 137000, 6300, 162800, 18500, 19000, 36000, 469160 });
		Data.Add(new decimal[] { 137000, 6320, 162800, 18550, 19000, 36000, 470428 });
		Data.Add(new decimal[] { 137000, 6340, 162800, 18600, 19000, 36000, 471696 });
		Data.Add(new decimal[] { 137000, 6360, 162800, 18650, 19000, 36000, 472964 });
		Data.Add(new decimal[] { 137000, 6380, 162800, 18700, 19000, 36000, 474232 });
		Data.Add(new decimal[] { 137000, 6400, 162800, 18750, 19000, 36000, 475500 });
		Data.Add(new decimal[] { 137000, 6420, 162800, 18800, 19000, 36000, 476768 });
		Data.Add(new decimal[] { 137000, 6440, 162800, 18850, 19000, 36000, 478036 });
		Data.Add(new decimal[] { 137000, 6460, 162800, 18900, 19000, 36000, 479304 });
		Data.Add(new decimal[] { 137000, 6480, 162800, 18950, 19000, 36000, 480572 });
		Data.Add(new decimal[] { 137000, 6500, 162800, 19000, 19000, 36000, 481840 });
		Data.Add(new decimal[] { 137000, 6520, 162800, 19050, 19000, 36000, 483108 });
		Data.Add(new decimal[] { 137000, 6540, 162800, 19100, 19000, 36000, 484376 });
		Data.Add(new decimal[] { 137000, 6560, 162800, 19150, 19000, 36000, 485644 });
		Data.Add(new decimal[] { 137000, 6580, 162800, 19200, 19000, 36000, 486912 });
		Data.Add(new decimal[] { 137000, 6600, 162800, 19250, 19000, 36000, 488180 });
		Data.Add(new decimal[] { 137000, 6620, 162800, 19300, 19000, 36000, 489448 });
		Data.Add(new decimal[] { 137000, 6640, 162800, 19350, 19000, 36000, 490716 });
		Data.Add(new decimal[] { 137000, 6660, 162800, 19400, 19000, 36000, 491984 });
		Data.Add(new decimal[] { 137000, 6680, 162800, 19450, 19000, 36000, 493252 });
		Data.Add(new decimal[] { 137000, 6700, 162800, 19500, 19000, 36000, 494520 });
		Data.Add(new decimal[] { 137000, 6720, 162800, 19550, 19000, 36000, 495788 });
		Data.Add(new decimal[] { 137000, 6740, 162800, 19600, 19000, 36000, 497056 });
		Data.Add(new decimal[] { 137000, 6760, 162800, 19650, 19000, 36000, 498324 });
		Data.Add(new decimal[] { 137000, 6780, 162800, 19700, 19000, 36000, 499592 });
		Data.Add(new decimal[] { 137000, 6800, 162800, 19750, 19000, 36000, 500860 });
		Data.Add(new decimal[] { 137000, 6820, 162800, 19800, 19000, 36000, 502128 });
		Data.Add(new decimal[] { 137000, 6840, 162800, 19850, 19000, 36000, 503396 });
		Data.Add(new decimal[] { 137000, 6860, 162800, 19900, 19000, 36000, 504664 });
		Data.Add(new decimal[] { 137000, 6880, 162800, 19950, 19000, 36000, 505932 });
		Data.Add(new decimal[] { 137000, 6900, 162800, 20000, 19000, 36000, 507200 });
		Data.Add(new decimal[] { 137000, 6920, 162800, 20000, 19000, 36000, 508468 });
		Data.Add(new decimal[] { 137000, 6940, 162800, 20000, 19000, 36000, 509736 });
		Data.Add(new decimal[] { 137000, 6960, 162800, 20000, 19000, 36000, 511004 });
		Data.Add(new decimal[] { 137000, 6980, 162800, 20000, 19000, 36000, 512272 });
		Data.Add(new decimal[] { 137000, 7000, 162800, 20000, 19000, 36000, 513540 });
		Data.Add(new decimal[] { 137000, 7020, 162800, 20000, 19000, 36000, 514808 });
		Data.Add(new decimal[] { 137000, 7040, 162800, 20000, 19000, 36000, 516076 });
		Data.Add(new decimal[] { 137000, 7060, 162800, 20000, 19000, 36000, 517344 });
		Data.Add(new decimal[] { 137000, 7080, 162800, 20000, 19000, 36000, 518612 });
		Data.Add(new decimal[] { 137000, 7100, 162800, 20000, 19000, 36000, 519880 });
		Data.Add(new decimal[] { 137000, 7120, 162800, 20000, 19000, 36000, 521148 });
		Data.Add(new decimal[] { 137000, 7140, 162800, 20000, 19000, 36000, 522416 });
		Data.Add(new decimal[] { 137000, 7160, 162800, 20000, 19000, 36000, 523684 });
		Data.Add(new decimal[] { 137000, 7180, 162800, 20000, 19000, 36000, 524952 });
		Data.Add(new decimal[] { 137000, 7200, 162800, 20000, 19000, 36000, 526220 });
		Data.Add(new decimal[] { 137000, 7220, 162800, 20000, 19000, 36000, 527176 });
		Data.Add(new decimal[] { 137000, 7240, 162800, 20000, 19000, 36000, 528132 });
		Data.Add(new decimal[] { 137000, 7260, 162800, 20000, 19000, 36000, 529088 });
		Data.Add(new decimal[] { 137000, 7280, 162800, 20000, 19000, 36000, 530044 });
		Data.Add(new decimal[] { 137000, 7300, 162800, 20000, 19000, 36000, 531000 });
		Data.Add(new decimal[] { 137000, 7320, 162800, 20000, 19000, 36000, 531070 });
		Data.Add(new decimal[] { 137000, 7340, 162800, 20000, 19000, 36000, 531140 });
		Data.Add(new decimal[] { 137000, 7360, 162800, 20000, 19000, 36000, 531210 });
		Data.Add(new decimal[] { 137000, 7380, 162800, 20000, 19000, 36000, 531280 });
		Data.Add(new decimal[] { 137000, 7400, 162800, 20000, 19000, 36000, 531350 });
		Data.Add(new decimal[] { 137000, 7420, 162800, 20000, 19000, 36000, 531420 });
		Data.Add(new decimal[] { 137000, 7440, 162800, 20000, 19000, 36000, 531490 });
		Data.Add(new decimal[] { 137000, 7460, 162800, 20000, 19000, 36000, 531560 });
		Data.Add(new decimal[] { 137000, 7480, 162800, 20000, 19000, 36000, 531630 });
		Data.Add(new decimal[] { 137000, 7500, 162800, 20000, 19000, 36000, 531700 });
		Data.Add(new decimal[] { 137000, 7520, 162800, 20000, 19000, 36000, 531770 });
		Data.Add(new decimal[] { 137000, 7540, 162800, 20000, 19000, 36000, 531840 });
		Data.Add(new decimal[] { 137000, 7560, 162800, 20000, 19000, 36000, 531910 });
		Data.Add(new decimal[] { 137000, 7580, 162800, 20000, 19000, 36000, 531980 });
		Data.Add(new decimal[] { 137000, 7600, 162800, 20000, 19000, 36000, 532050 });
		Data.Add(new decimal[] { 137000, 7620, 162800, 20000, 19000, 36000, 532120 });
		Data.Add(new decimal[] { 137000, 7640, 162800, 20000, 19000, 36000, 532190 });
		Data.Add(new decimal[] { 137000, 7660, 162800, 20000, 19000, 36000, 532260 });
		Data.Add(new decimal[] { 137000, 7680, 162800, 20000, 19000, 36000, 532330 });
		Data.Add(new decimal[] { 137000, 7700, 162800, 20000, 19000, 36000, 532400 });
		Data.Add(new decimal[] { 137000, 7720, 162800, 20000, 19000, 36000, 532470 });
		Data.Add(new decimal[] { 137000, 7740, 162800, 20000, 19000, 36000, 532540 });
		Data.Add(new decimal[] { 137000, 7760, 162800, 20000, 19000, 36000, 532610 });
		Data.Add(new decimal[] { 137000, 7780, 162800, 20000, 19000, 36000, 532680 });
		Data.Add(new decimal[] { 137000, 7800, 162800, 20000, 19000, 36000, 532750 });
		Data.Add(new decimal[] { 137000, 7820, 162800, 20000, 19000, 36000, 532820 });
		Data.Add(new decimal[] { 137000, 7840, 162800, 20000, 19000, 36000, 532890 });
		Data.Add(new decimal[] { 137000, 7860, 162800, 20000, 19000, 36000, 532960 });
		Data.Add(new decimal[] { 137000, 7880, 162800, 20000, 19000, 36000, 533030 });
		Data.Add(new decimal[] { 137000, 7900, 162800, 20000, 19000, 36000, 533100 });
		Data.Add(new decimal[] { 137000, 7920, 162800, 20000, 19000, 36000, 533170 });
		Data.Add(new decimal[] { 137000, 7940, 162800, 20000, 19000, 36000, 533240 });
		Data.Add(new decimal[] { 137000, 7960, 162800, 20000, 19000, 36000, 533310 });
		Data.Add(new decimal[] { 137000, 7980, 162800, 20000, 19000, 36000, 533380 });
		Data.Add(new decimal[] { 137000, 8000, 162800, 20000, 19000, 36000, 533450 });
		Data.Add(new decimal[] { 137000, 8020, 162800, 20000, 19000, 36000, 533520 });
		Data.Add(new decimal[] { 137000, 8040, 162800, 20000, 19000, 36000, 533590 });
		Data.Add(new decimal[] { 137000, 8060, 162800, 20000, 19000, 36000, 533660 });
		Data.Add(new decimal[] { 137000, 8080, 162800, 20000, 19000, 36000, 533730 });
		Data.Add(new decimal[] { 137000, 8100, 162800, 20000, 19000, 36000, 533800 });
		Data.Add(new decimal[] { 137000, 8120, 162800, 20000, 19000, 36000, 533870 });
		Data.Add(new decimal[] { 137000, 8140, 162800, 20000, 19000, 36000, 533940 });
		Data.Add(new decimal[] { 137000, 8160, 162800, 20000, 19000, 36000, 534010 });
		Data.Add(new decimal[] { 137000, 8180, 162800, 20000, 19000, 36000, 534080 });
		Data.Add(new decimal[] { 137000, 8200, 162800, 20000, 19000, 36000, 534150 });
		Data.Add(new decimal[] { 137000, 8220, 162800, 20000, 19000, 36000, 534220 });
		Data.Add(new decimal[] { 137000, 8240, 162800, 20000, 19000, 36000, 534290 });
		Data.Add(new decimal[] { 137000, 8260, 162800, 20000, 19000, 36000, 534360 });
		Data.Add(new decimal[] { 137000, 8280, 162800, 20000, 19000, 36000, 534430 });
		Data.Add(new decimal[] { 137000, 8300, 162800, 20000, 19000, 36000, 534500 });
		Data.Add(new decimal[] { 137000, 8320, 162800, 20000, 19000, 36000, 534570 });
		Data.Add(new decimal[] { 137000, 8340, 162800, 20000, 19000, 36000, 534640 });
		Data.Add(new decimal[] { 137000, 8360, 162800, 20000, 19000, 36000, 534710 });
		Data.Add(new decimal[] { 137000, 8380, 162800, 20000, 19000, 36000, 534780 });
		Data.Add(new decimal[] { 137000, 8400, 162800, 20000, 19000, 36000, 534850 });
		Data.Add(new decimal[] { 137000, 8420, 162800, 20000, 19000, 36000, 534920 });
		Data.Add(new decimal[] { 137000, 8440, 162800, 20000, 19000, 36000, 534990 });
		Data.Add(new decimal[] { 137000, 8460, 162800, 20000, 19000, 36000, 535060 });
		Data.Add(new decimal[] { 137000, 8480, 162800, 20000, 19000, 36000, 535130 });
		Data.Add(new decimal[] { 137000, 8500, 162800, 20000, 19000, 36000, 535200 });
		Data.Add(new decimal[] { 137000, 8520, 162800, 20000, 19000, 36000, 535270 });
		Data.Add(new decimal[] { 137000, 8540, 162800, 20000, 19000, 36000, 535340 });
		Data.Add(new decimal[] { 137000, 8560, 162800, 20000, 19000, 36000, 535410 });
		Data.Add(new decimal[] { 137000, 8580, 162800, 20000, 19000, 36000, 535480 });
		Data.Add(new decimal[] { 137000, 8600, 162800, 20000, 19000, 36000, 535550 });
		Data.Add(new decimal[] { 137000, 8620, 162800, 20000, 19000, 36000, 535620 });
		Data.Add(new decimal[] { 137000, 8640, 162800, 20000, 19000, 36000, 535690 });
		Data.Add(new decimal[] { 137000, 8660, 162800, 20000, 19000, 36000, 535760 });
		Data.Add(new decimal[] { 137000, 8680, 162800, 20000, 19000, 36000, 535830 });
		Data.Add(new decimal[] { 137000, 8700, 162800, 20000, 19000, 36000, 535900 });
		Data.Add(new decimal[] { 137000, 8720, 162800, 20000, 19000, 36000, 535970 });
		Data.Add(new decimal[] { 137000, 8740, 162800, 20000, 19000, 36000, 536040 });
		Data.Add(new decimal[] { 137000, 8760, 162800, 20000, 19000, 36000, 536110 });
		Data.Add(new decimal[] { 137000, 8780, 162800, 20000, 19000, 36000, 536180 });
		Data.Add(new decimal[] { 137000, 8800, 162800, 20000, 19000, 36000, 536250 });
		Data.Add(new decimal[] { 137000, 8820, 162800, 20000, 19000, 36000, 536320 });
		Data.Add(new decimal[] { 137000, 8840, 162800, 20000, 19000, 36000, 536390 });
		Data.Add(new decimal[] { 137000, 8860, 162800, 20000, 19000, 36000, 536460 });
		Data.Add(new decimal[] { 137000, 8880, 162800, 20000, 19000, 36000, 536530 });
		Data.Add(new decimal[] { 137000, 8900, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 8920, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 8940, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 8960, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 8980, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9000, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9020, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9040, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9060, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9080, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9100, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9120, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9140, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9160, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9180, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9200, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9220, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9240, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9260, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9280, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9300, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9320, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9340, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9360, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9380, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });
		Data.Add(new decimal[] { 137000, 9400, 162800, 20000, 19000, 36000, 536600 });

		int pointer = 0;
		if (Value > 0.5m) {
			pointer = Int32.Parse(Math.Ceiling(Value * 10 - 5).ToString());
		}
		if (Data.Count <= pointer) {
			return Data[Data.Count - 1];
		} else {
			return Data[pointer];
		}
	}
	private String RequestLoadFI(string RequestFormPk) {
		decimal CBM = 0;
		string FOBAmount = "";
		string FOBMonetaryUnit = "";
		string TotalAmount = "";
		StringBuilder Html = new StringBuilder();
		DB.SqlCmd.CommandText = @"
SELECT TBBH.[DATETIME_TO]
   FROM [INTL2010].[dbo].[TransportBBHistory] AS TBBHistory
	 left join OurBranchStorageCode AS OBSC ON OBSC.[OurBranchStoragePk]=TBBHistory.StorageCode 
	left join [dbo].[TRANSPORT_HEAD] AS TBBH ON TBBHistory.[TransportBetweenBranchPk]=TBBH.[TRANSPORT_PK]
 WHERE OBSC.[OurBranchCode]=3157 and RequestFormPk=" + RequestFormPk + ";";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string todatetime;
		if (RS.Read()) {
			todatetime = RS[0] + "";
		} else {
			todatetime = "";
		}
		RS.Dispose();
		string temparrivalDate = "";

		string PackedCount = "";
		string Weight = "";
		string Volume = "";

		DB.SqlCmd.CommandText = string.Format(@"
DECLARE @CommercialDocumentPk int; 

SELECT  @CommercialDocumentPk=[CommercialDocumentPk]
  FROM [dbo].[CommerdialConnectionWithRequest]
   WHERE RequestFormPk={0}; 
   
SELECT Sum(PackedCount),  Sum(GrossWeight), Sum(Volume)
  FROM [dbo].[RequestFormItems] 
   WHERE RequestFormPk in (SELECT  RequestFormPk
  FROM [dbo].[CommerdialConnectionWithRequest]
   WHERE [CommercialDocumentPk]=@CommercialDocumentPk)   
", RequestFormPk);
		RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			PackedCount = RS[0] + "";
			Weight = RS[1] + "";
			Volume = RS[2] + "";
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = @"
SELECT R.ArrivalDate, R.ShipperPk, R.ConsigneePk, R.ShipperCode, R.ConsigneeCode
	, R.MonetaryUnitCL, R.RequestDate, R.StepCL
	, CD.BLNo, CD.[InvoiceNo], CD.Shipper, CD.[PortOfLoading], CD.[FinalDestination],  CD.[Consignee], CD.Registerd AS InvoiceDate, CD.[Carrier], CD.[FOBorCNF] 
	, RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume, RFCH.[ConsigneeCharge] 
FROM RequestForm AS R 
	left join CommerdialConnectionWithRequest AS CCWR ON R.RequestFormPk=CCWR.RequestFormPk 
	left join CommercialDocument AS CD ON CD.CommercialDocumentHeadPk=CCWR.CommercialDocumentPk 
	left join RequestFormCalculateHead AS RFCH ON R.RequestFormPk=RFCH.RequestFormPk 
WHERE R.RequestFormPk=" + RequestFormPk + ";";

		RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			temparrivalDate = RS["ArrivalDate"] + "";
			string tempFOBNCNF = RS["FOBorCNF"] + "";
			string TempBLTERMS = "";
			if (tempFOBNCNF != "") {
				if (tempFOBNCNF.Substring(0, 3) == "CNF") {
					TempBLTERMS = "FREIGHT PREPAID";
				} else {
					TempBLTERMS = "FREIGHT COLLECT";
				}
				FOBAmount = tempFOBNCNF.Split(Common.Splite321, StringSplitOptions.None)[2];
				FOBMonetaryUnit = tempFOBNCNF.Split(Common.Splite321, StringSplitOptions.None)[1];
				TotalAmount = RS["ConsigneeCharge"] + "";
			}

			try {
				CBM = decimal.Parse(Volume);

			} catch (Exception) {
				CBM = 0;
			}
			Html.Append("<div class=\"Title\">" +
   "<div style=\"border-bottom:2px black solid; padding-left:10px; padding-bottom:5px; font-weight:bold; font-size:18px;\"> To.&nbsp;&nbsp;&nbsp;" + RS["Consignee"] + "</div><br />" +
   "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:695px; height:140px; \">" +
   "	<tr><td>INVOICE DATE</td><td>: " + ((RS["InvoiceDate"] + "").IndexOf(" ") > 0 ? (RS["InvoiceDate"] + "").Substring(0, (RS["InvoiceDate"] + "").IndexOf(" ")) : RS["InvoiceDate"] + "") + "</td><td>B / L NO</td><td>: " + RS["BLNo"] + "</td></tr>" +
   "	<tr><td>CONTAINER TYPE</td><td>: LCL</td><td>입항일자</td><td>: " + todatetime + "</td></tr>" +
   "	<tr><td>Q'TY</td><td>: " + PackedCount + " " + Common.GetPackingUnit(RS["PackingUnit"] + "") + "</td><td>VESSLEL NAME</td><td>: " + RS["Carrier"] + "</td></tr>" +
   "	<tr><td>WEIGHT</td><td>: " + Common.NumberFormat(Weight) + " KG</td><td>LOADING PORT</td><td>: " + RS["PortOfLoading"] + "</td></tr>" +
   "	<tr><td>MEASURMENT</td><td>: " + Common.NumberFormat(Volume) + " CBM</td><td>DESTINATION PORT</td><td>: " + RS["FinalDestination"] + "</td></tr>" +
   "	<tr><td>B/L TERMS</td><td colspan='3'>: " + TempBLTERMS + " </td></tr>" +
   "	<tr><td>SHIPPER NAME</td><td colspan='3'>: " + RS["Shipper"] + "</td></tr>" +
   "	</table></div>");
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = "EXEC SP_SelectCalculatedHead @RequestFormPk=" + RequestFormPk + ";";
		string ShipperMonetaryUnit, ShipperCharge, ConsigneeMonetaryUnit, ConsigneeCharge, SBankName, SBankOwnerName, SBankAccount, CBankName, CBankOwnerName, CBankAccount;
		string[] ExchangeRate;

		RS = DB.SqlCmd.ExecuteReader();
		string monetary = "";
		if (RS.Read()) {
			ExchangeRate = (RS["ExchangeRate"] + "") == "" ? new string[] { "N" } : (RS["ExchangeRate"] + "").Split(Common.Splite22, StringSplitOptions.RemoveEmptyEntries);
			ShipperMonetaryUnit = RS["ShipperMonetaryUnit"] + "";
			ShipperCharge = RS["ShipperCharge"] + "";
			ConsigneeMonetaryUnit = RS["ConsigneeMonetaryUnit"] + "";
			ConsigneeCharge = RS["ConsigneeCharge"] + "";
			SBankName = RS["SBankName"] + "";
			SBankOwnerName = RS["SBankOwnerName"] + "";
			SBankAccount = RS["SBankAccountNo"] + "";
			CBankName = RS["CBankName"] + "";
			CBankOwnerName = RS["CBankOwnerName"] + "";
			CBankAccount = RS["CBankAccountNo"] + "";
			if (SorC == "S") {
				monetary = Common.GetMonetaryUnit(ShipperMonetaryUnit);
			} else {
				monetary = Common.GetMonetaryUnit(ConsigneeMonetaryUnit);
			}
		} else {
			RS.Dispose();
			DB.DBCon.Close();
			return "0";
		}
		RS.Dispose();
		DB.DBCon.Close();

		string mainmonetaryunit = ConsigneeMonetaryUnit;
		string maincharge = ConsigneeCharge;

		////////////////////////여기부터 	
		StringBuilder tempstringbuilder = new StringBuilder();
		tempstringbuilder.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:695px; \"	><tr><td colspan='7' style=\"font-size:15px; font-weight:bold; border-bottom:solid 1px black;\" >&nbsp;</td></tr>" +
										 "<tr><td style=\"height:25px; text-align:center;border-left:solid 1px black;border-right:solid 1px black;border-bottom:solid 1px black;\" >내역</td>" +
										 "<td style=\"width:40px; text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\" >CUR</td>" +
										 "<td style=\"width:60px; text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\" >ExRate</td>" +
										 "<td style=\"width:100px; text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\" >AMOUNT</td>" +
										 "<td style=\"width:100px; text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\" >AMOUNT[￦]</td>" +
										 "<td style=\"width:100px; text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\" >AMOUNT[VAT]</td></tr>");
		string TableRowElse = "<tr><td style=\"height:22px; text-align:center; border-bottom:solid 1px black; black;border-left:solid 1px black;border-right:solid 1px black;\" >{0}" +
												  "</td><td style=\"text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\">{1}" +
												  "</td><td style=\"text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\">{2}" +
												  "</td><td style=\"text-align:right;padding-right:10px; border-bottom:solid 1px black;border-right:solid 1px black;\">{3}</td>" +
												  "<td style=\"text-align:right;padding-right:10px; border-bottom:solid 1px black;border-right:solid 1px black;\">{4}</td>" +
												  "<td style=\"text-align:right;padding-right:10px; border-bottom:solid 1px black;border-right:solid 1px black;\">&nbsp;</td></tr>";
		string tempExchangedDate = "";
		string exchangerate = "";
		string fobexchanged = "";
		decimal tempExchangedAmount = 0;
		decimal SumAmount = 0;
		if (ExchangeRate[0] == "N") {
			tempExchangedDate = temparrivalDate;
		} else {
			for (int j = 0; j < ExchangeRate.Length; j++) {
				if (j == 0) {
					tempExchangedDate = ExchangeRate[j].Substring(6, 8);
					continue;
				}
				if (Int32.Parse(ExchangeRate[j].Substring(6, 8)) > Int32.Parse(tempExchangedDate)) {
					tempExchangedDate = ExchangeRate[j].Substring(6, 8);
				}
			}
		}
		string temp = "";
		if (FOBAmount == "") {
			tempExchangedAmount = 0;
		} else {
			if (FOBMonetaryUnit == "20") {
				tempExchangedAmount = decimal.Parse(FOBAmount);
			} else {
				tempExchangedAmount = Math.Truncate(new Admin().GetExchangeRated(FOBMonetaryUnit, "20", decimal.Parse(FOBAmount), out temp, tempExchangedDate));
			}
		}
		if (temp != "") {
			string[] temparray = temp.Split(Common.Splite11, StringSplitOptions.None);
			exchangerate = Common.NumberFormat(temparray[3].Replace("@@", ""));
			fobexchanged = Common.GetMonetaryUnit(mainmonetaryunit) + " " + Common.NumberFormat(tempExchangedAmount + "");
		}

		decimal templeft = decimal.Parse(TotalAmount) - tempExchangedAmount;
		SumAmount += tempExchangedAmount;

		//Response.Write(exchangerate);

		if (FOBAmount != "") {
			if (FOBMonetaryUnit == "20") {
				tempstringbuilder.Append(string.Format(TableRowElse, "OCEAN FREIGHT CHARGE", "&nbsp", "&nbsp", "&nbsp", "￦ " + FOBAmount));
			} else {
				tempstringbuilder.Append(string.Format(TableRowElse, "OCEAN FREIGHT CHARGE", "USD", exchangerate, "$" + FOBAmount, fobexchanged));
			}
		}


		string[] Arr_Title = new string[] { "TERMINAL HANDLING CHARGE[LCL]", "WHARFAGE", "TRUCKING CHARGE[LCL]", "CONTAINER CLEANING CHARE[LCL]", "DOCUMENT FEE[LCL]", "HANDLING CHARE[LCL]", "WAREHOUSE CHARE[LCL]" };
		decimal[] Arr_Value = CalcForFI(CBM);
		for (var i = 0; i < Arr_Title.Length; i++) {
			if (templeft < Arr_Value[i] || i == Arr_Title.Length - 1) {
				tempstringbuilder.AppendFormat(TableRowElse, Arr_Title[i], "&nbsp;", "&nbsp;", "&nbsp;", "￦ " + Common.NumberFormat(templeft.ToString()));
				break;
			} else {
				templeft -= Arr_Value[i];
				tempstringbuilder.AppendFormat(TableRowElse, Arr_Title[i], "&nbsp;", "&nbsp;", "&nbsp;", "￦ " + Common.NumberFormat(Arr_Value[i].ToString()));
			}
			//			SumAmount += Arr_Value[i];

		}

		/*
		for (var i = 0; i < Arr_Title.Length; i++) {
		   decimal currentValue = Calc(Arr_Title[i], CBM);
		   if (i == Arr_Title.Length - 1) {
			  currentValue = templeft;
			  templeft -= currentValue;
		   } else {
			  templeft -= currentValue;
		   }
		   if (currentValue < 0) {
			  Response.Write("<h1>ERROR : 전산실 문의바랍니다.</h1>");
		   }

		   tempstringbuilder.AppendFormat(TableRowElse, Arr_Title[i], "&nbsp;", "&nbsp;", "&nbsp;", "￦ " + Common.NumberFormat(currentValue.ToString()));
		}
		*/

		if (SumAmount == 0) {
			tempstringbuilder.Append("<tr><td colspan='7' style=\"height:30px; font-weight:bold; text-align:center; border-left:solid 1px black;border-right:solid 1px black;border-bottom:solid 1px black;\" >청구내역이 없습니다. </tr>");
		} else {
			tempstringbuilder.Append("	<tr  style=\"height:30px;\">" +
   "<td colspan='4' style=\"height:30px; font-weight:bold; text-align:center; border-left:solid 1px black;border-right:solid 1px black;border-bottom:solid 1px black;\" >합계</td>" +
   "<td colspan='3' style=\"text-align:center;padding-right:10px; font-weight:bold; border-bottom:solid 1px black;border-right:solid 1px black;\">" + monetary + " " + Common.NumberFormat(TotalAmount) + "</td></tr>" +
										  "	<tr style=\"height:30px;\">" +
   "<td colspan='1' style=\"height:30px; font-weight:bold; text-align:center; \" >&nbsp;</td>" +
   "<td colspan='6' style=\"text-align:right; font-size:20px; padding-top:20px; padding-right:20px; padding-right:10px; font-weight:bold; border-bottom:double 5px black;\">" +
	  "청구합계금액 : [KRW]&nbsp;&nbsp;&nbsp;&nbsp;" + monetary + "&nbsp;&nbsp;&nbsp;&nbsp;" + Common.NumberFormat(TotalAmount) + "</td></tr></table>");
		}
		tempstringbuilder.Append("</table>");
		string bankaccount = CBankName + " : " + CBankAccount + " " + CBankOwnerName;

		BankInfo = "	<div style=\"border-top:1px dotted black; margin-top:30px; padding:10px; line-height:10px;\">" +
		   "<p><strong>* 계좌 번호 안내 *</strong></p>" +
		   "<p><strong>" + bankaccount + "</strong></p>" +
		   "</div>";
		return Html.Append(tempstringbuilder + "").ToString();
	}
	private String ItemLoad(string RequestFormPk) {
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:695px; \"><tr><td colspan='8' style=\"font-size:15px; font-weight:bold; border-bottom:solid 1px black;\" >접수명세</td></tr><tr>" +
										"<td style=\"height:25px; text-align:center; border-left:solid 1px black; border-right:solid 1px black; border-bottom:solid 1px black;\" >품명 (재질)</td>" +
										"<td style=\"width:85px; text-align:center; border-right:solid 1px black; border-bottom:solid 1px black;\" >수량</td>" +
										"<td style=\"width:60px; text-align:center; border-right:solid 1px black; border-bottom:solid 1px black;\" >단가</td>" +
										"<td style=\"width:85px; text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >금액</td>" +
										"<td style=\"width:50px; text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >포장수량</td>" +
										"<td style=\"width:60px; text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >중량</td>" +
										"<td style=\"width:65px; text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >체적</td></tr>");
		DB.SqlCmd.CommandText = "EXEC	SP_SelectItemWithRequestFormPk @RequestFormPk = " + RequestFormPk + ";";
		string EachRow = "<tr><td style=\"height:22px; text-align:center; border-left:solid 1px black;border-right:solid 1px black;border-bottom:solid 1px black; padding:2px; \" >{0}</td>" +
										"<td style=\"text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >{1}</td>" +
										"<td style=\"text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >{2}</td>" +
										"<td style=\"text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >{3}</td>" +
										"<td style=\"text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >{4}</td>" +
										"<td style=\"text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >{5}</td>" +
										"<td style=\"text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >{6}</td></tr>";

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		decimal quantity = 0;
		decimal amount = 0;
		string monetaryunit = "";
		string quantityunit = "";
		while (RS.Read()) {
			string squantity = "&nbsp;";
			string samount = "&nbsp;";

			if (RS["Quantity"] + "" != "") {
				quantityunit = Common.GetQuantityUnit(RS["QuantityUnit"] + "");
				squantity = Common.NumberFormat(RS["Quantity"] + "") + " " + Common.GetQuantityUnit(RS["QuantityUnit"] + "");
				quantity += Decimal.Parse(RS["Quantity"] + "");
			}
			if (RS["Amount"] + "" != "") {
				monetaryunit = Common.GetMonetaryUnit(RS["MonetaryUnitCL"] + "");
				samount = Common.GetMonetaryUnit(RS["MonetaryUnitCL"] + "") + " " + Common.NumberFormat("" + RS["Amount"]);
				amount += Decimal.Parse("" + RS["Amount"]);
			}

			string description;
			if (RS["Description"] + "" != "" && RS["Label"] + "" != "") {
				description = RS["Label"] + " : " + RS["Description"];
			} else {
				description = RS["Label"] + "" + RS["Description"];
			}

			string[] RowData = new string[]{
				description + ((RS["Material"] + "") == "" ? "" : "(" + RS["Material"] + ")") ,
				((RS["Quantity"] + "") == "" ? "&nbsp;" : Common.NumberFormat(RS["Quantity"] + "") + " " + Common.GetQuantityUnit(RS["QuantityUnit"] + "")) ,
				((RS["UnitPrice"] + "") == "" ? "&nbsp;" : Common.GetMonetaryUnit(RS["MonetaryUnitCL"] + "") + " " + Common.NumberFormat("" + RS["UnitPrice"])) ,
				((RS["Amount"] + "") == "" ? "&nbsp;" : Common.GetMonetaryUnit(RS["MonetaryUnitCL"] + "") + " " + Common.NumberFormat("" + RS["Amount"])),
				((RS["PackedCount"] + "") == "" ? "&nbsp;" : Common.NumberFormat(RS["PackedCount"] + "") + " " + Common.GetPackingUnit(RS["PackingUnit"] + "")) ,
				((RS["GrossWeight"] + "") == "" ? "&nbsp;" : Common.NumberFormat(RS["GrossWeight"] + "") + " Kg") ,
				((RS["Volume"] + "") == "" ? "&nbsp;" : Common.NumberFormat(RS["Volume"] + "") + " CBM")
			};

			ReturnValue.Append(String.Format(EachRow, RowData));
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = @"SELECT	RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume 
															FROM	[dbo].[RequestForm] AS RF 
															WHERE	RF.RequestFormPk=" + RequestFormPk + ";";
		RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ReturnValue.Append(String.Format(EachRow, "Total",
				Common.NumberFormat(quantity.ToString()) + " " + quantityunit,
				"&nbsp;",
				monetaryunit + " " + Common.NumberFormat(amount.ToString()),
				((RS["TotalPackedCount"] + "") == "" ? "" : Common.NumberFormat(RS["TotalPackedCount"] + "") + " " + Common.GetPackingUnit(RS["PackingUnit"] + "")),
				((RS["TotalGrossWeight"] + "") == "" ? "" : Common.NumberFormat(RS["TotalGrossWeight"] + "") + " Kg"),
				((RS["TotalVolume"] + "") == "" ? "" : Common.NumberFormat(RS["TotalVolume"] + "") + " CBM")));
		}
		RS.Dispose();
		ReturnValue.Append("</table>");
		return ReturnValue + "";
	}
	private String TitleLoad(string RequestFormPk) {
		string ReturnValue = "";
		if (SorC == "S") {
			DB.SqlCmd.CommandText = "SELECT C.CompanyPk, C.CompanyName, C.CompanyAddress, C.CompanyTEL, C.CompanyFAX, C.PresidentEmail " +
														"	FROM RequestForm AS RF " +
														"		left join RegionCode AS R ON  RF.DepartureRegionCode=R.RegionCode " +
														"		left join Company AS C on R.OurBranchCode=C.CompanyPk WHERE RF.RequestFormPk=" + RequestFormPk;
		} else {
			DB.SqlCmd.CommandText = "SELECT C.CompanyPk, C.CompanyName, C.CompanyAddress, C.CompanyTEL, C.CompanyFAX, C.PresidentEmail " +
														"	FROM RequestForm AS RF " +
														"		left join RegionCode AS R ON  RF.ArrivalRegionCode=R.RegionCode " +
														"		left join Company AS C on R.OurBranchCode=C.CompanyPk WHERE RF.RequestFormPk=" + RequestFormPk;
		}
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		if (RS.Read()) {
			OurBranchPk = RS["CompanyPk"] + "";
			CompanyTEL = RS["CompanyTEL"] + "";
			PresidentEmail = RS["PresidentEmail"] + "";

			ReturnValue = "<div style=\"border-bottom:solid 2px black; width:692px; height:75px; text-align:center; padding-top:3px; \">" +
				"<div style=\"font-size:18px; font-weight:bold; text-align:center; letter-spacing:3px;\">" + RS["CompanyName"] + "</div>" +
				"<div style=\"font-size:12px; text-align:center; padding-top:15px; letter-spacing:1px;   \">" + RS["CompanyAddress"] + "</div>" +
				"<div style=\"font-size:12px; text-align:center; padding-top:5px; letter-spacing:1px;   \">TEL : " + RS["CompanyTEL"] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;FAX : " + RS["CompanyFAX"] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;E-mail : " + RS["PresidentEmail"] + "</div>" +
			"</div>";
		}
		RS.Dispose();
		return ReturnValue;
	}
	private String FreightLoad(string RequestFormPk, string SorC) {
		DB.SqlCmd.CommandText = "EXEC SP_SelectCalculatedHead @RequestFormPk=" + RequestFormPk + ";";
		string ShipperMonetaryUnit, ShipperCharge, ConsigneeMonetaryUnit, ConsigneeCharge, SBankName, SBankOwnerName, SBankAccount, CBankName, CBankOwnerName, CBankAccount, WillPayTariff;
		string[] ExchangeRate;

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string monetary = "";
		if (RS.Read()) {
			WillPayTariff = RS["WillPayTariff"] + "";
			ExchangeRate = (RS["ExchangeRate"] + "") == "" ? new string[] { "N" } : (RS["ExchangeRate"] + "").Split(Common.Splite22, StringSplitOptions.RemoveEmptyEntries);
			ShipperMonetaryUnit = RS["ShipperMonetaryUnit"] + "";
			ShipperCharge = RS["ShipperCharge"] + "";
			ConsigneeMonetaryUnit = RS["ConsigneeMonetaryUnit"] + "";
			ConsigneeCharge = RS["ConsigneeCharge"] + "";
			SBankName = RS["SBankName"] + "";
			SBankOwnerName = RS["SBankOwnerName"] + "";
			SBankAccount = RS["SBankAccountNo"] + "";
			CBankName = RS["CBankName"] + "";
			CBankOwnerName = RS["CBankOwnerName"] + "";
			CBankAccount = RS["CBankAccountNo"] + "";
			if (SorC == "S") {
				monetary = Common.GetMonetaryUnit(ShipperMonetaryUnit);
			} else {
				monetary = Common.GetMonetaryUnit(ConsigneeMonetaryUnit);
			}
		} else {
			RS.Dispose();
			DB.DBCon.Close();
			return "0";
		}
		RS.Dispose();
		string where = SorC == "S" ? " and GubunCL<=200" : " and GubunCL>200 ";
		string mainmonetaryunit = SorC == "S" ? ShipperMonetaryUnit : ConsigneeMonetaryUnit;
		string maincharge = SorC == "S" ? ShipperCharge : ConsigneeCharge;
		DB.SqlCmd.CommandText = "SELECT Count(*) FROM RequestFormCalculateBody WHERE RequestFormPk=" + RequestFormPk + " and isnull(StandardPriceHeadPkNColumn, '0')<>'D' " + where + " ;";
		string MulryubiCount = DB.SqlCmd.ExecuteScalar() + "";

		DB.SqlCmd.CommandText = "SELECT Title, Price, MonetaryUnit FROM RequestFormCalculateBody WHERE RequestFormPk=" + RequestFormPk + " and isnull(StandardPriceHeadPkNColumn, '0')<>'D' " + where + " ;";

		RS = DB.SqlCmd.ExecuteReader();
		decimal TotalPayment = 0;
		string TotalString = "";

		List<string[]> ItemRowSource = new List<string[]>();
		////////////////////////여기부터 
		int ItemCount = 0;
		while (RS.Read()) {
			string CUR, ExRate, AmountBefore, AmountAfter;
			if (mainmonetaryunit != RS["MonetaryUnit"] + "") {
				string temp = RS["MonetaryUnit"] + "";
				switch (temp) {
					case "18":
						CUR = "CNY";
						break;
					case "19":
						CUR = "USD";
						break;
					case "20":
						CUR = "KRW";
						break;
					case "21":
						CUR = "JPY";
						break;
					case "22":
						CUR = "HKD";
						break;
					case "23":
						CUR = "EUR";
						break;
					default:
						CUR = "?";
						break;
				}

				ExRate = "N";
				AmountBefore = Common.GetMonetaryUnit(RS["MonetaryUnit"] + "") + " " + Common.NumberFormat(RS["Price"] + "");
				AmountAfter = "N";
			} else {
				CUR = "&nbsp;";
				ExRate = "&nbsp;";
				AmountBefore = "&nbsp;";
				AmountAfter = Common.GetMonetaryUnit(RS["MonetaryUnit"] + "") + " " + Common.NumberFormat(RS["Price"] + "");
			}

			ItemRowSource.Add(new string[11] { "", "", "", "", "", "", "", "", "", "", "" });
			ItemRowSource[ItemCount][0] = MulryubiCount;
			ItemRowSource[ItemCount][1] = "물류비";
			ItemRowSource[ItemCount][2] = RS["Title"] + "";
			ItemRowSource[ItemCount][3] = CUR;
			ItemRowSource[ItemCount][4] = ExRate;
			ItemRowSource[ItemCount][5] = AmountBefore;
			ItemRowSource[ItemCount][6] = AmountAfter;
			ItemRowSource[ItemCount][7] = Common.GetMonetaryUnit(mainmonetaryunit) + " " + Common.NumberFormat(maincharge);
			ItemRowSource[ItemCount][8] = RS["MonetaryUnit"] + "";
			ItemRowSource[ItemCount][9] = mainmonetaryunit;
			ItemRowSource[ItemCount][10] = RS["Price"] + "";

			ItemCount++;
		}
		RS.Dispose();
		DB.DBCon.Close();

		StringBuilder tempstringbuilder = new StringBuilder();
		tempstringbuilder.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:695px; \"	><tr><td colspan='7' style=\"font-size:15px; font-weight:bold; padding-top:20px; border-bottom:solid 1px black;\" >청구내역</td></tr>" +
													"<tr><td style=\"height:25px; text-align:center;border-left:solid 1px black;border-right:solid 1px black;border-bottom:solid 1px black;\" >TYPE</td>" +
													"<td style=\"height:25px; text-align:center;border-bottom:solid 1px black;border-right:solid 1px black;\" >ITEM</td>" +
													"<td style=\"width:40px; text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\" >CUR</td>" +
													"<td style=\"width:60px; text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\" >ExRate</td>" +
													"<td style=\"width:100px; text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\" >AMOUNT</td>" +
													"<td style=\"width:100px; text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\" >AMOUNT[" + Common.GetMonetaryUnit(mainmonetaryunit) + "]</td>" +
													"<td style=\"width:100px; text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\" >TOTAL</td></tr>");
		string TableRowFirst = "<tr><td style=\"height:22px; text-align:center; font-weight:bold; border-bottom:solid 1px black;border-right:solid 1px black;border-left:solid 1px black;\" rowspan=\"{0}\" >{1}" +
																"</td><td style=\"height:22px; text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\" >{2}" +
																"</td><td style=\"text-align:center;border-bottom:solid 1px black;border-right:solid 1px black;\">{3}" +
																"</td><td style=\"text-align:center;border-bottom:solid 1px black;border-right:solid 1px black;\">{4}" +
																"</td><td style=\"text-align:right;padding-right:10px; border-bottom:solid 1px black;border-right:solid 1px black;\">{5}</td>" +
																"<td style=\"text-align:right;padding-right:10px; border-bottom:solid 1px black;border-right:solid 1px black;\">{6}</td>" +
																"<td rowspan=\"{0}\" style=\"text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\">{7}</td></tr>";
		string TableRowElse = "<tr><td style=\"height:22px; text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\" >{0}" +
																"</td><td style=\"text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\">{1}" +
																"</td><td style=\"text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\">{2}" +
																"</td><td style=\"text-align:right;padding-right:10px; border-bottom:solid 1px black;border-right:solid 1px black;\">{3}</td>" +
																"<td style=\"text-align:right;padding-right:10px; border-bottom:solid 1px black;border-right:solid 1px black;\">{4}</td></tr>";
		for (int i = 0; i < ItemRowSource.Count; i++) {
			if (ItemRowSource[i][4] == "N") {
				string tempExchangedDate = "";
				decimal tempExchangedAmount = 0;

				for (int j = 0; j < ExchangeRate.Length; j++) {
					if (tempExchangedDate == "") {
						tempExchangedDate = ExchangeRate[j].Substring(6, 8);
						continue;
					}
					if (Int32.Parse(ExchangeRate[j].Substring(6, 8)) > Int32.Parse(tempExchangedDate)) {
						tempExchangedDate = ExchangeRate[j].Substring(6, 8);
					}
				}
				string temp = "";
				tempExchangedAmount = Math.Truncate(new Admin().GetExchangeRated(ItemRowSource[i][8], ItemRowSource[i][9], decimal.Parse(ItemRowSource[i][10]), out temp, tempExchangedDate));
				if (temp != "") {
					string[] temparray = temp.Split(Common.Splite11, StringSplitOptions.None);
					ItemRowSource[i][4] = Common.NumberFormat(temparray[3].Replace("@@", ""));
					ItemRowSource[i][6] = Common.GetMonetaryUnit(mainmonetaryunit) + " " + Common.NumberFormat(tempExchangedAmount + "");
				}
			}

			if (i == 0) {
				tempstringbuilder.Append(string.Format(TableRowFirst,
					ItemRowSource[i][0],
					ItemRowSource[i][1],
					ItemRowSource[i][2],
					ItemRowSource[i][3],
					ItemRowSource[i][4],
					ItemRowSource[i][5],
					ItemRowSource[i][6],
					ItemRowSource[i][7]));
				TotalString += "물류비";
				TotalPayment += decimal.Parse(maincharge);
			} else {
				tempstringbuilder.Append(string.Format(TableRowElse,
					ItemRowSource[i][2],
					ItemRowSource[i][3],
					ItemRowSource[i][4],
					ItemRowSource[i][5],
					ItemRowSource[i][6]));
			}
		}


		DB.DBCon.Open();
		bool isFirst;
		//Response.Write("WillPayTariff : " + WillPayTariff + " / SorC :" + SorC);
		if (WillPayTariff == SorC) {
			DB.SqlCmd.CommandText = @"	SELECT CDT.CommercialDocumentHeadPk, CDT.Title, CDT.MonetaryUnitCL, CDT.Value , Total.TariffSUM , Total.TariffRow
																FROM CommercialDocumentTariff AS CDT
																	left join (SELECT [CommercialDocumentHeadPk], sum([Value]) AS TariffSUM, count(*) AS TariffRow FROM CommercialDocumentTariff WHERE GubunPk=" + RequestFormPk + @" and GubunCL=0 and Value>0 group by [CommercialDocumentHeadPk] ) AS Total ON CDT.CommercialDocumentHeadPk=Total.CommercialDocumentHeadPk 
																WHERE CDT.GubunPk=" + RequestFormPk + " and CDT.GubunCL=0 and CDT.Value>0 ;";
			//Response.Write(DB.SqlCmd.CommandText);
			RS = DB.SqlCmd.ExecuteReader();
			decimal TarriffSum = 0;
			isFirst = true;
			while (RS.Read()) {
				if (isFirst) {
					string tempTariffTotal;
					if (mainmonetaryunit == RS["MonetaryUnitCL"] + "") {
						TarriffSum = decimal.Parse("" + RS["TariffSUM"]);
						tempTariffTotal = Common.GetMonetaryUnit(RS["MonetaryUnitCL"] + "") + " " + Common.NumberFormat("" + RS["TariffSUM"]);
					} else {
						string tempExchangedDate = "";
						for (int i = 0; i < ExchangeRate.Length; i++) {
							if (tempExchangedDate == "") {
								tempExchangedDate = ExchangeRate[i].Substring(6, 8);
								continue;
							}
							if (Int32.Parse(ExchangeRate[i].Substring(6, 8)) > Int32.Parse(tempExchangedDate)) {
								tempExchangedDate = ExchangeRate[i].Substring(6, 8);
							}
						}
						string temp = "";
						decimal tempTariffSUM = new Admin().GetExchangeRated(RS["MonetaryUnitCL"] + "", mainmonetaryunit, decimal.Parse("" + RS["TariffSUM"]), out temp, tempExchangedDate);
						switch (mainmonetaryunit) {
							case "18":
								TarriffSum = Math.Round(tempTariffSUM, 1, MidpointRounding.AwayFromZero);
								break;
							case "19":
								TarriffSum = Math.Round(tempTariffSUM, 2, MidpointRounding.AwayFromZero);
								break;
							case "20":
								TarriffSum = Math.Round(tempTariffSUM, 0, MidpointRounding.AwayFromZero);
								break;
						}
						tempTariffTotal = Common.GetMonetaryUnit(mainmonetaryunit) + " " + Common.NumberFormat(TarriffSum + "");
					}
					TotalPayment += TarriffSum;
					tempstringbuilder.Append("	<tr><td rowspan='" + RS["TariffRow"] + "' style=\"font-weight:bold; text-align:center; border-left:solid 1px black;border-right:solid 1px black;border-bottom:solid 1px black;\" >통관비</td>" +
																		"<td colspan='3' style=\"height:22px; text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\"  >" + RS["Title"] + "</td>" +
																		"<td colspan='2' style=\"text-align:center;padding-right:10px; border-bottom:solid 1px black;border-right:solid 1px black;\">" + Common.GetMonetaryUnit(RS["MonetaryUnitCL"] + "") + " " + Common.NumberFormat(RS["Value"] + "") + "</td>" +
																		"<td rowspan='" + RS["TariffRow"] + "' style=\"text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\" >" + tempTariffTotal + "</td></tr>");

					if (TotalString != "") {
						TotalString += " + 통관비";
					} else {
						TotalString += "통관비";
					}
					isFirst = false;
				} else {
					tempstringbuilder.Append("	<tr><td colspan='3' style=\"height:22px; text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\" >" + RS["Title"] + "</td>" +
												"					<td colspan='2' style=\"text-align:center;padding-right:10px;border-bottom:solid 1px black;border-right:solid 1px black; \">" + Common.GetMonetaryUnit(RS["MonetaryUnitCL"] + "") + " " + Common.NumberFormat(RS["Value"] + "") + "</td></tr>");
				}
			}
			RS.Dispose();
		}
		DB.SqlCmd.CommandText = "SELECT Title, Price, MonetaryUnit FROM RequestFormCalculateBody WHERE RequestFormPk=" + RequestFormPk + " and StandardPriceHeadPkNColumn='D' " + where + " ;";
		RS = DB.SqlCmd.ExecuteReader();

		if (RS.Read()) {
			tempstringbuilder.Append("	<tr><td style=\"height:30px; font-weight:bold; text-align:center; border-left:solid 1px black;border-right:solid 1px black;border-bottom:solid 1px black;\" >배달비</td>" +
																		"<td colspan='3' style=\"height:30px; text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\"  >" + RS["Title"] + "</td>" +
																		"<td colspan='2' style=\"text-align:center;padding-right:10px; border-bottom:solid 1px black;border-right:solid 1px black;\">" + Common.GetMonetaryUnit(RS["MonetaryUnit"] + "") + " " + Common.NumberFormat(RS["Price"] + "") + "</td>" +
																		"<td style=\"height:30px; text-align:center; border-bottom:solid 1px black;border-right:solid 1px black; \" >" + Common.GetMonetaryUnit(RS["MonetaryUnit"] + "") + " " + Common.NumberFormat("" + RS["Price"]) + "</td></tr>");
			TotalPayment += decimal.Parse(RS["Price"] + "");
			if (TotalString != "") {
				TotalString += " + 배달비";
			} else {
				TotalString += "배달비";
			}
		}
		RS.Dispose();
		if (TotalPayment == 0) {
			tempstringbuilder.Append("<tr><td colspan='7' style=\"height:30px; font-weight:bold; text-align:center; border-left:solid 1px black;border-right:solid 1px black;border-bottom:solid 1px black;\" >청구내역이 없습니다. </tr>");
		} else {
			tempstringbuilder.Append("<tr  style=\"height:30px;\"><td colspan='4' style=\"height:30px; font-weight:bold; text-align:center; border-left:solid 1px black;border-right:solid 1px black;border-bottom:solid 1px black;\" >입금하실 금액 (" + TotalString + ")</td>" +
														"	<td colspan='3' style=\"text-align:center;padding-right:10px; font-weight:bold; border-bottom:solid 1px black;border-right:solid 1px black;\">" + monetary + " " + Common.NumberFormat(TotalPayment + "") + "</td></tr></table>");
		}

		tempstringbuilder.Append("</table>");
		string bankaccount;
		if (SorC == "S") {
			bankaccount = SBankName + " : " + SBankAccount + " " + SBankOwnerName;
		} else {
			bankaccount = CBankName + " : " + CBankAccount + " " + CBankOwnerName;
		}

		if (SorC == "S") {
			DB.SqlCmd.CommandText = @"SELECT A.Duties, A.Name, A.TEL, A.Mobile, A.Email , RFAI.[DESCRIPTION]
														FROM Company AS C
															left join Account_ AS A on C.ResponsibleStaff=A.AccountID
															left join RequestForm AS R ON R.ShipperPk=C.CompanyPk
															left join (SELECT [TABLE_PK], [DESCRIPTION] FROM [dbo].[HISTORY] WHERE [TABLE_NAME] = 'RequestForm' AND [CODE]='5') AS RFAI ON RFAI.[TABLE_PK]=R.RequestFormPk 
														WHERE R.RequestFormPk=" + RequestFormPk + ";";
		} else {
			DB.SqlCmd.CommandText = @"SELECT A.Duties, A.Name, A.TEL, A.Mobile, A.Email, RFAI.[DESCRIPTION] 
														FROM Company AS C
															left join Account_ AS A on C.ResponsibleStaff=A.AccountID
															left join RequestForm AS R ON R.ConsigneePk=C.CompanyPk 
															left join (SELECT [TABLE_PK], [DESCRIPTION] FROM [dbo].[HISTORY] WHERE [TABLE_NAME] = 'RequestForm' AND [CODE]='6') AS RFAI ON RFAI.[TABLE_PK]=R.RequestFormPk 
														WHERE R.RequestFormPk=" + RequestFormPk + ";";
		}
		//Response.Write(DB.SqlCmd.CommandText);
		RS = DB.SqlCmd.ExecuteReader();
		string staff = "";
		string comment = "";
		if (RS.Read()) {
			if (RS["Value"] + "" != "") {
				comment = "<p>" + RS["Value"] + "</p>청구금액의 입금기한은 출고일 PM 4:00전까지, 토요일은11:00전까지 입니다. <br />마감시간내 미입금 및 이후 입금시 익일 업무진행됩니다. 양해바랍니다.";
				/*
				comment += "<p>위와 같이 운임/통관 부대비용을 청구하오니<br>" +
                     "상기 계좌번호로 <br>" +
                      "</p>";
				 */
			}
			staff = RS["Duties"] + " " + RS["Name"] + " ( TEL : " + CompanyTEL + "  E-mail : " + PresidentEmail + ")";
		}
		RS.Dispose();
		string Html_BankInfo = "";
		MakeHtml_BankInfo(OurBranchPk, out Html_BankInfo);

		if (OurBranchPk == "3157") {
			bankaccount = @"KEB 하나은행(구.외환은행) (주)국제종합물류 630-004796-321 ";

			BankInfo = "	<div style=\"border-top:1px dotted black; border-bottom:1px dotted black; margin-top:10px; padding-top:10px; line-height:20px; text-align:right;\">" +
"<div>" + staff + "&nbsp;&nbsp;&nbsp;<strong><br/>" + bankaccount + "</strong></div>" +
							comment.Replace("\r", "<br/>") +
						"</div>";
		} else {
			BankInfo = "	<div style=\"border-top:1px dotted black; border-bottom:1px dotted black; margin-top:10px; padding-top:10px; line-height:20px; text-align:right;\">" +
				   "<div>" + staff + "<strong><br/>" + Html_BankInfo + "</strong></div>" +
												comment.Replace("\r", "<br/>") +
											"</div>";
		}

		return tempstringbuilder + "";
	}
	private string MakeHtml_BankInfo(string OurBranchPk, out string Html_BankInfo) {
		DB.SqlCmd.CommandText = string.Format(@"
select BankName,OwnerName,AccountNo from CompanyBank
where isdel = 0 and GubunPk={0}", OurBranchPk);
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		Html_BankInfo = "";
		while (RS.Read()) {
			Html_BankInfo += RS["BankName"].ToString() + " " + RS["OwnerName"].ToString() + " " + RS["AccountNo"].ToString() + "<br />";
		}
		RS.Dispose();

		return "1";

	}
}
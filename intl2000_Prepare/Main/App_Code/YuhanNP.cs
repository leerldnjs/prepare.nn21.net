using Components;
using Components.cClearance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// YuhanNP의 요약 설명입니다.
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
[System.Web.Script.Services.ScriptService]
public class YuhanNP : System.Web.Services.WebService
{
	[WebMethod]
	public string SearchCode(string Type, string Code) {
		YuhanN Y = new YuhanN();
		switch (Type) {
			case "PreCarriage":
				return Y.Search_PreCarriage(Code);
			case "FinalPort":
				return Y.Search_Port(Code);
			case "Warehouse":
				return Y.Search_Warehouse(Code);
			case "Assignment":
				return Y.Search_Assignment(Code);
		}
		return "1";
	}

	[WebMethod]
	public String SetCalc(string ValueSum) {
		Dictionary<string, string> Form = new Utility().ConvertGetParam(ValueSum);
		var Count_HouseBL = 0;
		decimal TotalPackedCount = 0;
		decimal TotalVolume = 0;
		decimal TotalWeight = 0;
		while (Form.ContainsKey("House_" + Count_HouseBL + "_BLNo")) {
			if (Form["House_" + Count_HouseBL + "_BLNo"] == "") {
				Count_HouseBL++;
				continue;
			}
			string Count = Form["House_" + Count_HouseBL + "_PackedCount"];
			string Volume = Form["House_" + Count_HouseBL + "_Volume"];
			string Weight = Form["House_" + Count_HouseBL + "_Weight"];

			TotalPackedCount += decimal.Parse(Count);
			TotalVolume += decimal.Parse(Volume);
			TotalWeight += decimal.Parse(Weight);
			Count_HouseBL++;
		}
		return Common.NumberFormat(TotalPackedCount.ToString()) + ",!" + Common.NumberFormat(TotalVolume.ToString()) + ",!" + Common.NumberFormat(TotalWeight.ToString()) + ",!";
	}

	[WebMethod]
	public String Load_SavedList(string Type) {
		YuhanN Y = new YuhanN();
		switch (Type) {
			case "PreCarriage":
				return Y.Load_PreCarriageList();
			case "FinalPort":
				return Y.Load_PortList();
			case "Customs":
				return Y.Load_CustomsList();
			case "Division":
				return Y.Load_DivisionList();
			case "Warehouse":
				return Y.Load_Warehouse();
			case "Assignment":
				return Y.Load_Assignment();
		}
		return "1";
	}

	[WebMethod]
	public string SetDeleteFromIntl2000(string TBBHPk) {
		return new YuhanN().Clear_YuhanMaster_Intl2000(TBBHPk);
	}
	[WebMethod]
	public string LoadHistory_Description(string HouseBLNo) {
		return new YuhanN().Load_History_Description(HouseBLNo);
	}
	[WebMethod]
	public String SetMasterBL(string ValueSum) {
		Dictionary<string, string> Form = new Utility().ConvertGetParam(ValueSum);
		sYuhan_MasterN Current = new sYuhan_MasterN();
		Current.TBBHPk = Form["TBBHPk"].ToString().Trim();
		Current.MasterBLNo = Form["MasterBLNo"].ToString().Trim();
		Current.ShipName = Form["VesselName"].ToString().Trim();
		Current.VoyageNo = Form["VoyageNo"].ToString().Trim();
		Current.MRN = Form["MRN"].ToString().Trim();
		Current.MSN = Form["MSN"].ToString().Trim();
		Current.LineCode = Form["PreCarriageCode"].ToString().Trim();
		Current.FinalDate = Form["FinalDate"].ToString().Trim();
		Current.FinalPort = Form["FinalPortCode"].ToString().Trim();
		Current.Container.ContainerNo = Form["ContainerNo"].ToString().Trim();
		Current.Container.SealNo1 = Form["SealNo"].ToString().Trim();
		Current.Container.ContainerCode = Form["ContainerType"].ToString().Trim();
		Current.AssignmentWH = Form["WarehouseCode"].ToString().Trim();
		Current.Customs = Form["Customs"].ToString().Trim();
		Current.Division = Form["Division"].ToString().Trim();
		Current.AssignmentCode = Form["AssignmentCode"].ToString().Trim();
		Current.AssignmentName = Form["AssignmentName"].ToString().Trim();
		Current.FlagAACO = Form["FlagAACO"].ToString().Trim();

		Current.TotalPackedCount = Int32.Parse(Form["TotalPackedCount"].ToString().Replace(",", ""));
		Current.TotalVolume = decimal.Parse(Form["TotalVolume"].ToString().Replace(",", ""));
		Current.TotalWeight = decimal.Parse(Form["TotalWeight"].ToString().Replace(",", ""));

		Current.HouseBL = new List<HouseBLN>();
		var Count_HouseBL = 0;
		while (Form.ContainsKey("House_" + Count_HouseBL + "_BLNo")) {
			if (Form["IsSendYuhan"] == "Y" && Form["House_" + Count_HouseBL + "_BLNo"] == "") {
				Count_HouseBL++;
				continue;
			}
			HouseBLN house = new HouseBLN();
			house.BLNo = Form["House_" + Count_HouseBL + "_BLNo"].ToString().Trim();
			house.HSN = Form["House_" + Count_HouseBL + "_HSN"].ToString().Trim();
			if (house.HSN == "" || house.HSN == "0") {
				house.HSN = "99";
			}
			house.PackedCount = Form["House_" + Count_HouseBL + "_PackedCount"].ToString().Trim();

			switch (Form["House_" + Count_HouseBL + "_PackingUnit"].ToString().Trim()) {
				case "CT":
					house.PackingUnit = "CT";
					break;
				case "PT":
					house.PackingUnit = "PT";
					break;
				case "RL":
					house.PackingUnit = "RL";
					break;
				default:
					house.PackingUnit = "GT";
					break;
			}

			house.Volume = Form["House_" + Count_HouseBL + "_Volume"].ToString().Trim();
			house.Weight = Form["House_" + Count_HouseBL + "_Weight"].ToString().Trim();
			house.Description = Form["House_" + Count_HouseBL + "_Description"].ToString().Trim();
			house.ShipperName = Form["House_" + Count_HouseBL + "_ShipperName"].ToString().Trim();
			house.ConsigneeName = Form["House_" + Count_HouseBL + "_ConsigneeName"].ToString().Trim();
			house.ConsigneeSaupjaNo = Form["House_" + Count_HouseBL + "_ConsigneeSaupjaNo"].ToString().Trim();

			if (Form["IsSendYuhan"] == "Y") {
				if (Form["House_" + Count_HouseBL + "_ShipperAddress"].Length > 60) {
					house.ShipperAddress = Form["House_" + Count_HouseBL + "_ShipperAddress"].Substring(0, 60).Trim();
					if (Form["House_" + Count_HouseBL + "_ShipperAddress"].Length > 120) {
						house.ShipperAddressA = Form["House_" + Count_HouseBL + "_ShipperAddress"].Substring(60, 60).Trim();
						if (Form["House_" + Count_HouseBL + "_ShipperAddress"].Length > 180) {
							house.ShipperAddressB = Form["House_" + Count_HouseBL + "_ShipperAddress"].Substring(120, 60).Trim();
							if (Form["House_" + Count_HouseBL + "_ShipperAddress"].Length > 240) {
								house.ShipperAddressC = Form["House_" + Count_HouseBL + "_ShipperAddress"].Substring(180, 60).Trim();
								if (Form["House_" + Count_HouseBL + "_ShipperAddress"].Length > 300) {
									house.ShipperAddressD = Form["House_" + Count_HouseBL + "_ShipperAddress"].Substring(240, 60).Trim();
								} else {
									house.ShipperAddressD = Form["House_" + Count_HouseBL + "_ShipperAddress"].Substring(240).Trim();
								}
							} else {
								house.ShipperAddressC = Form["House_" + Count_HouseBL + "_ShipperAddress"].Substring(180).Trim();
							}
						} else { house.ShipperAddressB = Form["House_" + Count_HouseBL + "_ShipperAddress"].Substring(120).Trim(); }
					} else {
						house.ShipperAddressA = Form["House_" + Count_HouseBL + "_ShipperAddress"].Substring(60).Trim();
					}
				} else {
					house.ShipperAddress = Form["House_" + Count_HouseBL + "_ShipperAddress"].Trim();
				}

				if (Form["House_" + Count_HouseBL + "_ConsigneeAddress"].Length > 60) {
					house.ConsigneeAddress = Form["House_" + Count_HouseBL + "_ConsigneeAddress"].Substring(0, 60).Trim();
					if (Form["House_" + Count_HouseBL + "_ConsigneeAddress"].Length > 120) {
						house.ConsigneeAddressA = Form["House_" + Count_HouseBL + "_ConsigneeAddress"].Substring(60, 60).Trim();
						if (Form["House_" + Count_HouseBL + "_ConsigneeAddress"].Length > 180) {
							house.ConsigneeAddressB = Form["House_" + Count_HouseBL + "_ConsigneeAddress"].Substring(120, 60).Trim();
							if (Form["House_" + Count_HouseBL + "_ConsigneeAddress"].Length > 240) {
								house.ConsigneeAddressC = Form["House_" + Count_HouseBL + "_ConsigneeAddress"].Substring(180, 60).Trim();
								if (Form["House_" + Count_HouseBL + "_ConsigneeAddress"].Length > 300) {
									house.ConsigneeAddressD = Form["House_" + Count_HouseBL + "_ConsigneeAddress"].Substring(240, 60).Trim();
								} else {
									house.ConsigneeAddressD = Form["House_" + Count_HouseBL + "_ConsigneeAddress"].Substring(240).Trim();
								}
							} else {
								house.ConsigneeAddressC = Form["House_" + Count_HouseBL + "_ConsigneeAddress"].Substring(180).Trim();
							}
						} else { house.ConsigneeAddressB = Form["House_" + Count_HouseBL + "_ConsigneeAddress"].Substring(120).Trim(); }
					} else {
						house.ConsigneeAddressA = Form["House_" + Count_HouseBL + "_ConsigneeAddress"].Substring(60).Trim();
					}
				} else {
					house.ConsigneeAddress = Form["House_" + Count_HouseBL + "_ConsigneeAddress"].Trim();
				}
			} else {
				house.ShipperAddress = Form["House_" + Count_HouseBL + "_ShipperAddress"];
				house.ConsigneeAddress = Form["House_" + Count_HouseBL + "_ConsigneeAddress"];
			}

			Current.HouseBL.Add(house);
			Count_HouseBL++;
		}
		YuhanN Y = new YuhanN();
		if (Form["IsSendYuhan"] == "Y") {
			return Y.SetSave_YuhanMaster_Yuhan(Current);
		} else {
			return Y.SetSave_YuhanMaster_Intl2000(Current);
		}
	}
}
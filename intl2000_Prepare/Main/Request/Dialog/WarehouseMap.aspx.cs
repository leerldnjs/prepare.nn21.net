using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Request_Dialog_WarehouseMap : System.Web.UI.Page
{
	protected String Title, TEL, FAX, Address; 
    protected void Page_Load(object sender, EventArgs e)
    {
		string WarehouseCode = Request.Params["S"] + "";
		
		switch (WarehouseCode) {
			case "2":
				Title = "동방창고";
				TEL = "880-6014";
				FAX = "880-6098";
				Address = "인천 중구 항동 7가 82-7";
				break;
			case "58":
				Title = "구내1 신창고";
				TEL = "772-6009";
				FAX = "777-8397";
				Address = "인천중구항동7가 1-22";
				break;
			case "61":
				Title = "구내4";
				TEL = "887-2818";
				FAX = "887-2819";
				Address = "인천시 중구 항동7가 42";
				break;
			case "65":
				Title = "극동";
				TEL = "885-0367(박혜진)";
				FAX = "885-0364";
				Address = "인천시 중구 항동 7가 27-3번지";
				break;
			case "69":
				Title = "흥아창고";
				TEL = "885-6601 내선2";
				FAX = "885-5408";
				Address = "인천시 중구 항동 7가 31-4";
				break;
			case "71":
				Title = "선광창고";
				TEL = "885-0071";
				FAX = "885-0081";
				Address = "인천 중구 항동7가 82-7";
				break;
			case "76":
				Title = "한진국제창고";
				TEL = "032-887-8142";
				FAX = "881-3538";
				Address = "인천시 중구 항동 56-10";
				break;
			case "77":
				Title = "브이지로지스틱스창고";
				TEL = "889-6611";
				FAX = "889-6613";
				Address = "인천 중구 항동 7가 82-1";
				break;
			case "79":
				Title = "그린창고";
				TEL = "888-5325";
				FAX = "888-5328";
				Address = "인천 중구 항동 7가 82-7";
				break;
			case "125":
				Title = "인천콘테이너(본사)";
				TEL = "887-8600내선2";
				FAX = "887-9051";
				Address = "인천시 중구 항동 7가 82-1";
				break;
				/*
			case "125":
				Title = "대아 (인천컨테이너보세)";
				TEL = "887-7756~7";
				FAX = "070-7159-1637";
				Address = "중구 항동 7가 82-1번지";
				break;
				 */
			case "130":
				Title = "세계로물류 보세창고";
				TEL = "032-887-0281";
				FAX = "032-887-0280";
				Address = "인천시 중구 항동7가 57-6,10";
				break;
			case "132":
				Title = "희창물산㈜인천물류지점";
				TEL = "032-885-8151";
				FAX = "032-885-8157";
				Address = "인천시 중구 서해대로 209번길 76 (항동 7가)";
				break;
			case "133":
				Title = "대우로지스틱 보세창고";
				TEL = "032-890-9873";
				FAX = "032-881-9818";
				Address = "인천 중구 신흥동3가 76번지";
				break;
			case "153":
				Title = "대호보세창고";
				TEL = "032-887-2206";
				FAX = "032-887-2209";
				Address = "인천 중구 항동 7가 91-1";
				break;
			case "156":
				Title = "대흥창고";
				TEL = "032-887-3678";
				FAX = "";
				Address = "인천 중구 신흥동 3가 56";
				break;
			case "157":
				Title = "대보창고";
				TEL = "032-766-8301";
				FAX = "032-766-8305";
				Address = "인천 중구 신흥동 3가 56";
				break;
		}

    }
}
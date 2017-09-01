
function RunPrint() {
   var initBody;
   var beforePrint = function () {
      initBody = document.body.innerHTML;
      document.body.innerHTML = document.getElementById("ForPrint").innerHTML;
   };
   var afterPrint = function () {
      document.body.innerHTML = initBody;
   };

   if (window.matchMedia) {
      var mediaQueryList = window.matchMedia('print');
      mediaQueryList.addListener(function (mql) {
         if (mql.matches) {
            beforePrint();
         } else {
            afterPrint();
         }
      });
   }

   window.onbeforeprint = beforePrint;
   window.onafterprint = afterPrint;
   /* 익스 전용 호환성을 위해 수정함
   var initBody;
	window.onbeforeprint = function () {
		initBody = document.body.innerHTML;
		document.body.innerHTML = document.getElementById("ForPrint").innerHTML;
	};
	window.onafterprint = function () {
		document.body.innerHTML = initBody;
	};
   */
	window.print();
	return false;
}
function quick_menu() {
	/* quick menu initialization */
	var quick_menu = $('#quick_menu');
	var quick_top = 80;
	quick_menu.css('top', $(window).height());
	quick_menu.animate({ "top": $(document).scrollTop() + quick_top + "px" }, 500);
	$(window).scroll(function () {
		quick_menu.stop();
		quick_menu.animate({ "top": $(document).scrollTop() + quick_top + "px" }, 1000);
	});
}
function AddValidation(id) {
	var ReturnValue = true;
	var Allnull = true;
	var rowcount = 0;
	var child = "";
	var child2 = "";
	var Type = $(id).get(0).tagName;
	switch (Type) {
		case "TABLE": 
			child = ">tbody";
			child2 = ">tr";
			rowcount = $("" + Type + id + child + child2 + "").size();
			break;
		case "DIV":
			break;
	}
	for (i = 0; i < rowcount; i++) {
		$("" + Type + id + child + child2 + ":eq(" + [i] + ")").find('input[type=text]').each(function () {
			if ($(this).val() + "" != "") {
				Allnull = false;
			}
		});
		if (Allnull != true) {
			$("" + Type + id + child + child2 + ":eq(" + [i] + ")").find('input[type=text]').each(function () {
				if ($(this).data("addrequired") == "1") {
					if ($(this).val() == "") {
						alert($(this).data("addrequiredmsg"));
						$(this).select();
						ReturnValue = false;
						return ReturnValue;

					}
				}
				return ReturnValue;
			});
		}
		if (ReturnValue== false) {
			return ReturnValue;
		}
		Allnull = true;
	}
	return ReturnValue;
}

function Validation(PnId) {
	var ReturnValue = true;
	$(PnId).find('input[type=text]').each(function () {
		if ($(this).data("required") == "1") {
			if ($(this).val() == "") {
				alert($(this).data("requiredmsg"));
				$(this).select();
				ReturnValue = false;
				return false;
			}
		}
		return ReturnValue;
	});
	return ReturnValue;
}

function PopupClose() {
	window.opener.location.reload();
	window.close();
}

function LanguageSet(lang) {
	var URI = location.href;
	var UriSpliteByQ = URI.split('?');
	var MainRoot = UriSpliteByQ[0].split('/');
	var QueryString = "";
	if (UriSpliteByQ[1] != undefined) {
		var EachGet = UriSpliteByQ[1].split('&');
		for (var i = 0; i < EachGet.length; i++) {
			if (EachGet[i].split('=')[0] != 'Language' && EachGet[i] != "") { QueryString += EachGet[i] + "&"; }
		}
	}
	QueryString = "?Language=" + lang + "&" + QueryString;
	parent.location.href = MainRoot[MainRoot.length - 1] + QueryString;
}
function SMSSend() {
	$("#form_SMS").attr('action', '/Tool/SMSSend.aspx');
	$("#form_SMS").attr('target', 'Modify');
	$("#form_SMS").submit();
}
function EmailSend() {
	$("#form_Email").attr('action', '/Tool/EmailSend.aspx');
	$("#form_Email").attr('target', 'Modify');
	$("#form_Email").submit();
}
function MoveNextByEnter(to) {
	if (window.event.keyCode == 13) {
		document.getElementById(to).focus();
	}
}
function onlyEmail(obj) {
	if (obj.value.length > 0) {
		re = /^[0-9a-zA-Z]([-_\.]?[0-9a-zA-Z])*@[0-9a-zA-Z]([-_\.]?[0-9a-zA-Z])*.[a-zA-Z]{2,3}$/i;
		if (obj.value.length < 6 || !re.test(obj.value)) {
			alert("메일형식이 맞지 않습니다.\n"); obj.select(); obj.focus(); return false;
		}
	}
}

function OnlyNum(obj) {
	val = obj.value;
	re = /[^0-9]/gi;
	if (re.test(val)) {
		alert("숫자만 입력가능합니다");
		obj.select();
		obj.focus();
	}
}

function LoadOption(id, Which, SavedValue) {
	$.ajax({
		type: "POST",
		url: "/Process/SettingP.asmx/LoadOption",
		data: "{Which:'" + Which + "', SelectedValue:''}",
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (result) {
			$(id).html(result.d);
			if (SavedValue != "") {
				if ($(SavedValue).length) {
					$(id).val($(SavedValue).val());
				} else {
					$(id).val(SavedValue);
				}
			}
		},
		error: function (msg) {
			alert('failure : ' + msg);
		}
	});
}

function LoadOptionAppend(id, Which, SavedValue) {
	$.ajax({
		type: "POST",
		url: "/Process/SettingP.asmx/LoadOption",
		data: "{Which:'" + Which + "', SelectedValue:''}",
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (result) {
			$(id).append(result.d);
			if (SavedValue != "") {
				if ($(SavedValue).length) {
					$(id).val($(SavedValue).val());
				} else {
					$(id).val(SavedValue);
				}
			}
		},
		error: function (msg) {
			alert('failure : ' + msg);
		}
	});
}

function NumberFormat(num) {
	var len, point, str;
	var plusorminus = "";

	if (num.toString().substr(0, 1) == "+" || num.toString().substr(0, 1) == "-") {
		plusorminus = num.toString().substr(0, 1);
		num = num.toString().substr(1);
	}

	if (num.toString().indexOf('.') < 0) {
		num = num + ".";
	}
	num = num.split(".");

	point = num[0].length % 3
	len = num[0].length;

	str = num[0].toString().substr(0, point);
	while (point < len) {
		if (str != "") str += ",";
		str += num[0].toString().substr(point, point + 3);
		point += 3;
	}
	if (num.length > 1) {
		if (num[1] != "" || parseInt(num[1]) != 0) {
			temp = "";
			i = 0;

			while (true) {
				if (num[1].substr(num[1].length - 1, 1) == "0") {
					num[1] = num[1].substr(0, num[1].length - 1);
					continue;
				}
				break;
			}
			if (num[1] != "") {
				str += "." + num[1];
			}
		}
	}
	return plusorminus + str;
}

function CheckAll(event) {
	var isAllChecked = true;
	$(event.data.TableId).find("input[type=checkbox]").each(function () {
		if (!$(this).is(":checked")) {
			isAllChecked = false;
			return false;
		}
	});
	if (isAllChecked) {
		$(event.data.TableId).find("input[type=checkbox]").prop("checked", false);
	} else {
		$(event.data.TableId).find("input[type=checkbox]").prop("checked", true);
	}
}
function CheckAll_JustTarget(WapperId, dataTarget, dataValue) {
	var isAllChecked = true;
	$("#" + WapperId).find("input[type=checkbox]").each(function () {
		 if ($(this).data(dataTarget) == dataValue) {
			if (!$(this).is(":checked")) {
				isAllChecked = false;
				return false;
			}
		}
	});
	if (isAllChecked) {
		isAllChecked = false;
	} else {
		isAllChecked = true;
	}
	$("#" + WapperId).find("input[type=checkbox]").each(function () {
		if ($(this).data(dataTarget) == dataValue) {
			$(this).prop("checked", isAllChecked);
		}
	});
}
function GetToday() {
	var newDate = new Date();
	var yy = newDate.getFullYear();
	var mm = newDate.getMonth() + 1;
	if (mm < 10) {
		mm = "0" + mm;
	}
	var dd = newDate.getDate();
	if (dd < 10) {
		dd = "0" + dd;
	}
	var toDay = yy + "-" + mm + "-" + dd;
	return toDay;
}
function Ajax_SelectFinanceCode(S_CompanyPK, ResultHtmlId, S_DutiesCL) {
	$.ajax({
		type: "POST",
		url: "/Process/Admin/FinanceCodeP.asmx/MakeHtml_SelectFinanceCode",
		data: "{S_CompanyPk:'" + S_CompanyPK + "',S_DutiesCL:'" + S_DutiesCL + "'}",
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (result) {
			$(ResultHtmlId).html(result.d);
		},
		error: function (result) {
			console.log(result);
			alert('failure : ' + result);
		}
	});
}

function SearchCompany(event) {
	var SearchValue = $(event.data.SearchValue).val();
	var BranchPk = event.data.BranchPk;
	var DesignType = event.data.DesignType;
	var ResultId = event.data.Result;
	SearchCompanyCore(SearchValue, BranchPk, DesignType, ResultId);
}
function SearchCompanyCore(SearchValue, BranchPk, DesignType, ResultId) {
	$.ajax({
		type: "POST",
		url: "/Process/Work/CommonP.asmx/SearchCompany",
		data: "{BranchPk:'" + BranchPk + "', Value:'" + SearchValue + "', DesignType:'" + DesignType + "'}",
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (result) {
			$(ResultId).html(result.d);
		},
		error: function (result) {
			console.log(result);
			alert('failure : ' + result);
		}
	});
}
function LoadAccountList(CompanyPk, Type, DesignType, ResultHtmlId) {
	$.ajax({
		type: "POST",
		url: "/Process/Work/CommonP.asmx/LoadAccountList",
		data: "{CompanyPk:'" + CompanyPk + "', Type:'" + Type + "', DesignType:'" + DesignType + "'}",
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (result) {
			$(ResultHtmlId).html(result.d);
		},
		error: function (result) {
			console.log(result);
			alert('failure : ' + result);
		}
	});
}

function MakeHtml_RelatedCompanyList(RelationType, BranchPk, DesignType, ResultHtmlId) {
	$.ajax({
		type: "POST",
		url: "/Process/Work/CommonP.asmx/MakeHtml_RelatedCompanyList",
		data: "{RelationType:'" + RelationType + "', BranchPk:'" + BranchPk + "', DesignType:'" + DesignType + "'}",
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (result) {
			$(ResultHtmlId).append(result.d);
		},
		error: function (result) {
			console.log(result);
			alert('failure : ' + result);
		}
	});
}

function LoadRegionCode(RegionCode, ResultFinalCode, ResultFinalText, ResultBranchCode) {
	$.ajax({
		type: "POST",
		url: "/Process/SettingP.asmx/LoadRegionCode",
		data: "{ParentsRegionCode:'" + RegionCode + "'}",
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (result) {
			var RS = result.d;
			var Row = RS.split("%!$@#");
			if (Row[0] == "-1") {
				$(ResultFinalCode).val(Row[1]);
				$(ResultFinalText).val(Row[2]);
				$(ResultBranchCode).val(Row[3]);
			} else {
				AccessDeny();
			}
		},
		error: function (result) {
			alert('failure : ' + result);
		}
	});
}
function LoadRegionCodeFinalInfo(RegionCode, ResultFinalCode, ResultFinalText, ResultBranchCode) {
	$.ajax({
		type: "POST",
		url: "/Process/SettingP.asmx/LoadRegionCodeInfo",
		data: "{RegionCode:'" + RegionCode + "'}",
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (result) {
			var RS = result.d;
			var Row = RS.split("%!$@#");
			$(ResultFinalCode).val(Row[1]);
			$(ResultFinalText).val(Row[2]);
			$(ResultBranchCode).val(Row[3]);
		},
		error: function (result) {
			alert('failure : ' + result);
		}
	});
}
function LoadHtml_RegionCode(ParentsRegionCode, ResultHtmlId, ResultFinalCode, ResultFinalText, ResultBranchCode) {
	$.ajax({
		type: "POST",
		url: "/Process/SettingP.asmx/LoadRegionCode",
		data: "{ParentsRegionCode:'" + ParentsRegionCode + "'}",
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (result) {
			var RS = result.d;
			var Row = RS.split("%!$@#");
			if (Row[0] == "-1") {
				$(ResultFinalCode).val(Row[1]);
				$(ResultFinalText).val(Row[2]);
				$(ResultBranchCode).val(Row[3]);
				
				$('#SelectRegionCode').modal('hide');
			} else {
				var ReturnValue = "";
				var TempCategory = "-1";
				var CategoryArr = Array();
				var CategoryInner = Array();
				for (var i = 0; i < Row.length; i++) {
					var Each = Row[i].split("@#$");
					if (TempCategory != Each[4]) {
						TempCategory = Each[4];
						CategoryArr[CategoryArr.length] = Each[4];
						CategoryInner[(CategoryArr.length - 1)] = "";
					}
					CategoryInner[(CategoryArr.length - 1)] += "<span class='col-sm-4' style='cursor:pointer;' onclick=\"LoadHtml_RegionCode('" + Each[0] + "', '" + ResultHtmlId + "', '" + ResultFinalCode + "', '" + ResultFinalText + "', '" + ResultBranchCode + "')\">" + Each[1] + "[" + Each[2] + "]</span>";
				}
				for (var i = 0; i < CategoryArr.length; i++) {
					ReturnValue += "<tr><td>" + CategoryArr[i] + "</td><td>" + CategoryInner[i] + "</td></tr>";
				}

				$(ResultHtmlId).html(ReturnValue);
			}
		},
		error: function (result) {
			alert('failure : ' + result);
		}
	});
}
function AccessDeny() {
	alert("잘못된 접근입니다.");
	return false;
}
function CommentAdd() {
	$.ajax({
		type: "POST",
		url: "/Process/Work/CommonP.asmx/AddComment",
		data: "{Type:'" + $("#CommentType").val() + "', TypePk:'" + $("#CommentTypePk").val() + "', AccountPk:'" + $("#CommentAccountPk").val() + "', Comment:'" + $("#CommentComment").val() + "'}",
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (result) {
			if (result.d != "") {
				location.reload();
			} else {
				console.log(result);
			}
		},
		error: function (msg) {
			alert('failure : ' + msg);
		}
	});
}


function Goto_Event(e) {
	Goto(e.data.Type, e.data.TypePk);
}
function Goto(Type, TypePk) {
	switch (Type) {
		//case "back": history.back(); break;
		case "FileDown":
			location.href = "/Process/Tool/FileDownLoad.aspx?S=" + TypePk;
			break;
		case "CompanyBusinessNoCompany":
			location.href = "/Member/MyCompanyBusinessNo.aspx?P=" + TypePk;
			break;
	   case "CompanyBusinessNoBranch":
	      location.href = "/Admin/BranchBusinessNoList.aspx";
			break;
		case "CompanyInfo":
			location.href = "/Company/CompanyInfo.aspx?M=View&K=" + TypePk;
			break;
		case "CompanyView":
			location.href = "/Company/CompanyView.aspx?P=" + TypePk;
			break;
		case "RequestView":
			location.href = "/Work/Request/View.aspx?S=" + TypePk;
			break;
		case "RequestView_ByRootPK":
			location.href = "/Work/Request/View.aspx?R=" + TypePk;
			break;
		case "RequestDetail":
			location.href = "/Work/Request/Detail.aspx";
			break;
		case "ItemLibrary":
			location.href = "/Member/ItemLibrary.aspx?S=" + TypePk;
			break;
		case "CompanyNoticeList":
			location.href = "/Company/Notice/List.aspx?HC=" + TypePk;
			break;
		case "ConveyView":
			location.href = "/Work/Convey/View.aspx?S=" + TypePk;
			break;
		case "ApplyList":
			location.href = "/Admin/ApplyList.aspx?P=" + TypePk;
			break;
		case "RequestList":
			location.href = "/Work/Request/List_C.aspx?P=" + TypePk;
			break;
	}
}

function CheckID(f, count) {
	if ($(f).attr('readonly')) {
		return false;
	}
	regA1 = new RegExp(/^[0-9a-zA-Z]+$/);
	if (f != "") {
		if (!regA1.test(f.value)) {
			alert("ID able by Alphabet & Number");
			f.value = "";
			return false;
		}
		if (f.length < 5) {
			alert("Id should be more than 4 letters");
			f.value = "";
			return false;
		}
		$.ajax({
			type: "POST",
			url: "/Process/Member/MemberP.asmx/UniqueCheck",
			data: "{Column:'AccountID',Data:'" + f + "'}",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function (result) {
				if (result.d != "0") {
					alert("ID have been used");
					$("#TBStaffID" + count + "").select();
					$("#TBStaffID" + count + "").focus();
				} else {
					alert("Success");
					$("#CheckID").val("1");
				}
			},
			error: function (msg) {
				console.log(msg);
				alert('failure : ' + msg);
			}
		});
	}
}
function IDPWSetting(Type, CompanyPk) {
	if (Type == "Master") {
		$.ajax({
			type: "POST",
			url: "/Process/Company/CompanyP.asmx/CheckAccountID",
			data: "{CompanyPk:'" + CompanyPk + "'}",
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			success: function (result) {
				if (result.d == "0") {
					$("#TargetCompanyPK").val(CompanyPk);
					$("#Type").val(Type);
					$("#form1_MasterID").attr('action', '../Company/Dialog/IDPWSetting.aspx');
					$("#form1_MasterID").attr('target', 'Modify');
					$("#form1_MasterID").submit();
				} else {
					alert("이미사용중인 아이디가 있습니다");
					location.reload();
				}
			},
			error: function (msg) {
				alert('failure : ' + msg);
			}
		});
	}
	else {
		$("#TargetCompanyPK").val(CompanyPk);
		$("#Type").val(Type);
		$("#form1_IDPWSetting").attr('action', '../Company/Dialog/IDPWSetting.aspx');
		$("#form1_IDPWSetting").attr('target', 'Modify');
		$("#form1_IDPWSetting").submit();
	}
}
function validation_forBusinessNo(Type) {
	var Count = $("#TabBusinessNo > tr").length;
	if (Count == 0) {
		Count = 0;
	} else {
		Count = Count / 2;
	}
	var ReturnValue = true;
	if(Type=="Only"){
		var nullCheck = true;
	}
	for (i = 0; i < Count; i++) {
		if ($("#TBBusinessNoCompanyName" + i + "").val() + "" == "" && $("#TBBusinessNoAddress" + i + "").val() + "" == "" && $("#TBBusinessNo" + i + "").val() + "" == "" && $("#TBBusinessNoPresident" + i + "").val() + "" == "" && $("#TBBusinessNoBusinessType" + i + "").val() + "" == "" && $("#TBBusinessNoBusinessCategory" + i + "").val() + "" == "" && $("#TBBusinessNoEmail" + i + "").val() + ""=="") {

		} else {
			var nullCheck = false;
			if ($("#TBBusinessNoRegionCode" + i + "").val() + "" == "" || $("#TBBusinessNoRegionCode" + i + "").val() + "" == "Select") {
				alert("Country is not select");
				$("#TBBusinessNoRegionCode" + i + "").focus();
				$("#TBBusinessNoRegionCode" + i + "").select();
				ReturnValue = true;
				return false;
			} if ($("#TBBusinessNoCompanyName").val() + "" == "") {
				alert("TBBusinessNoCompanyName is not Write");
				$("#TBBusinessNoCompanyName" + i + "").focus();
				$("#TBBusinessNoCompanyName" + i + "").select();
				ReturnValue = true;
				return false;
			}
		}
		if (Type == "Only") {
			if (nullCheck) {
				alert("저장할 데이터가 없습니다");
				ReturnValue = true;
				return false;
			}
		}
	}
	return ReturnValue;
}
function CommentDelete(CommentPk) {
   if (confirm("This Comment will be delete.")) {
      $.ajax({
         type: "POST",
         url: "/Process/Work/CommonP.asmx/DeleteComment",
         data: "{CommentPk:'" + CommentPk + "'}",
         contentType: "application/json; charset=utf-8",
         dataType: "json",
         success: function (result) {
            if (result.d == "1") {
               location.reload();
            } else {
               console.log(result);
            }
         },
         error: function (msg) {
            alert('failure : ' + msg);
         }
      });
   }
}
function FileDelete(UploadedFilesPk) {
   if (confirm("Delete?")) {
      $.ajax({
         type: "POST",
         url: "/Process/Work/CommonP.asmx/FileDelete",
         data: "{UploadedFilesPk:'" + UploadedFilesPk + "'}",
         contentType: "application/json; charset=utf-8",
         dataType: "json",
         success: function (result) {
            if (result.d == "1") {
               alert("SUCCESS");
               location.reload();
            } else {
               alert("Failed");
               return false;
            }
         },
         error: function (msg) {
            alert('failure : ' + msg);
         }
      });
   }
}
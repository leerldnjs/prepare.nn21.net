function moveNext(from, to, length) {
	if (from.value.length == length) {
		document.getElementById(to).select();
		document.getElementById(to).focus();
	}
}

function MoveNextByEnter(to) {
    if (window.event.keyCode == 13) {
        document.getElementById(to).focus();
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


function OnlyEnglishNum(obj) {
    if (obj.value.length > 0) {
        regA1 = new RegExp(/^[0-9a-zA-Z,.-\/\ ]+$/);
        if (!regA1.test(obj.value)) {
            alert("영문과 숫자만 입력가능합니다.");
            obj.select();
            obj.focus();
        }
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

function AddNewCustomer() {
	//var SetValue = form1.HAccountID.value + "!" + document.getElementById("HCCLPk").value + "!" + document.getElementById('ConsigneeCodeFirst4').value + "!" + document.getElementById('ConsigneeCodeLastNum').value;
	var DialogResult = form1.logedAccountID.value + "!!!";
	var retVal = window.showModalDialog('../Admin/Dialog/CustomerAdd.aspx', DialogResult, 'dialogWidth=900px;dialogHeight=750px;resizable=0;status=0;scroll=1;help=0;');
	if (retVal != undefined) {
		location.href = "./CompanyInfo.aspx?M=View&S=" + retVal;
	}
}

function GotoForShippingBranch(Type, TypePk) {
	switch (Type) {
		case "NewCompany":
			window.open('/Admin/Dialog/CustomerAdd.aspx?Mode=ShippingBranch&OwnCPk=' + TypePk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=yes, top=20px; left=200px; height=700px; width=870px;');
			break;
	}
}
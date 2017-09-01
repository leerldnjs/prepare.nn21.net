function SpotAction(ActionType, TypeId) {
	$.ajax({
		type: "POST",
		url: "/Process/Management.asmx/SpotAction",
		data: "{ActionType:'" + ActionType + "', TypeId:'" + TypeId + "'}",
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (result) {
			if (result.d == "1") {
				if (ActionType == "SetCharge_Delete") {
					alert("완료");
					opener.location.reload();
					window.close();
				} else {
					alert("완료");
					location.reload();
				}
			}
		},
		error: function (msg) {
			alert('failure : ' + msg);
		}
	});
}
function SpotAction_ConfirmText(ActionType, TypeId, ConfirmText) {
	if (ConfirmText != "") {
		if (confirm(ConfirmText)) {
			SpotAction(ActionType, TypeId);
		}
	} else {
		SpotAction(ActionType, TypeId);
	}
}
function CheckAll(WapperId, dataTarget, dataValue) {
	var isAllChecked = true;
	$("#" + WapperId).find("input[type=checkbox]").each(function () {
		if (dataTarget != "") {
			if ($(this).data(dataTarget) == dataValue) {
				if (!$(this).is(":checked")) {
					isAllChecked = false;
					return false;
				}
			}
		} else {
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
		if (dataTarget != "") {
			if ($(this).data(dataTarget) == dataValue) {
				$(this).prop("checked", isAllChecked);
			}
		} else {
			$(this).prop("checked", isAllChecked);
		}
	});
}

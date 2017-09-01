var ForModal = (function () {
	var ModalStatus;
	var Init = function (Size, ModalId) {
		$("body").append('																				\
			<div class="modal" id="' + ModalId + '" tabindex="-1" role="dialog">			\
				<div class="modal-dialog modal-' + Size + '">								\
					<div class="modal-content">												\
						<div class="modal-header">											\
							<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>			\
							<h4 class="modal-title" id="Pn' + ModalId + '_Title"></h4>			\
						</div>																			\
						<div class="modal-body" id="Pn' + ModalId + '_Body">					\
						</div>																			\
						<div class="modal-footer" id="Pn' + ModalId + '_Footer">					\
							<button type="button" class="btn btn-warning" data-dismiss="modal" aria-hidden="true" >Close</button>\
						</div>																			\
					</div>																				\
				</div>																					\
			</div>');
	}
	var Open = function (ModalId, Value) {
		switch (ModalId) {
			case "ModalChoose_DebitCredit":
				ModalStatus = ModalId;
				if (Value == "S") {
					$("#Pn" + ModalId + "_Title").html("Shipper 선택");
				} else {
					$("#Pn" + ModalId + "_Title").html("Consignee 선택");
				}
				$.ajax({
					type: "POST",
					url: "/Process/DocumentP.asmx/MakeHtml_DebitCreditSCHistory",
					data: "{SorC:'" + Value + "'}",
					contentType: "application/json; charset=utf-8",
					dataType: "json",
					success: function (result) {
						$("#Pn" + ModalId + "_Body").html(result.d);
					},
					error: function (msg) {
						alert('failure : ' + msg);
						console.log(msg);
					}
				});
				break;
		}
		$('#' + ModalId).modal('show');
	}
	return {
		Init: Init,
		Open: Open
	};
}());
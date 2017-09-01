var ForDialog = (function () {
    initPopup = function () {
        $("body").append('																				\
    <form id="ForDialog" >                                                                              \
        <input type="hidden" id="ForDialog_Type" name="Type" />                                         \
        <input type="hidden" id="ForDialog_TypePk" name="TypePk" />                                       \
        <input type="hidden" id="ForDialog_CompanyPk" name="CompanyPk" />                                       \
        <input type="hidden" id="ForDialog_AccountPk" name="AccountPk" />                                       \
    </form>');

    }
    var ModalStatus;
    var Init = function (Size, ModalId) {
        $("body").append('																				\
			<div class="modal" id="' + ModalId + '" tabindex="-1" role="dialog" style="z-index:1111">			\
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

	var InitZ = function (Size, ModalId) {
		$("body").append('																				\
			<div class="modal" id="' + ModalId + '" tabindex="-1" role="dialog" style="z-index:1100;">			\
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

    return {
        initPopup: initPopup,
		Init: Init,
		InitZ: InitZ 
    };
}());
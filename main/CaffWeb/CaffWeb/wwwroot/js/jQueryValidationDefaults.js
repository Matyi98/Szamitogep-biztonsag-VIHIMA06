jQuery.validator.setDefaults({
	onfocusout: function (e) {
		this.element(e);
	},
	//onkeyup: false,
	highlight: function (element) {
		jQuery(element).closest('.form-control').addClass('is-invalid');
	},
	unhighlight: function (element) {
		jQuery(element).closest('.form-control').removeClass('is-invalid');
		jQuery(element).closest('.form-control').addClass('is-valid');
	},
	errorElement: 'div',
	errorClass: 'invalid-feedback',
	errorPlacement: function (error, element) {
		if (element.parent('.input-group-prepend').length) {
			$(element).siblings(".invalid-feedback").append(error);
			//error.insertAfter(element.parent());
		} else {
			error.insertAfter(element);
		}
	},
});

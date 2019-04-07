function ToastrDefaultOptions(position) {
    var options = {
        "closeButton": true,
        "debug": false,
        "positionClass": position === null ? "toast-bottom-right" : position,
        "onclick": null,
        "showDuration": "1000",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    return options;
}

/**
 * Shows Taostr Growl
 * @param {string} title Title of the toastr
 * @param {string} msg Message to Toast can be html.
 * @param {string} position position for toast ["toast-top-right","toast-top-full-width","toast-top-center", "toast-top-right","toast-bottom-right","toast-bottom-left","toast-bottom-full-width","toast-bottom-center" ]
 * @param {string} type toast type ["success", "info", "warning", "error"]
 */
function ShowToast(title, msg, position, type) {
    toastr[type](msg, title, ToastrDefaultOptions(position));
}


/**
 * Shows a Gritter Growl Message
 * @param {string} title Title for Growl
 * @param {string} msg Message can be an HTML string
 * @param {string} imageUrl Optional
 * @param {object} callbacks a callbacks object, {beforeOpen : function(), afterOpen : function(e), beforeClose : function(e, manuelclose), afterClose : function() }
 */
function ShowGritter(title, msg, imageUrl, callbacks) {
    if (callbacks === undefined)
        callbacks = {};

    if (imageUrl === undefined)
        imageUrl = '';

    if ($.gritter) {
        $.gritter.add({            
            title: title,            
            text: msg,
            // (string | optional) the image to display on the left
            image: imageUrl,
            sticky: false,
            time: 8000,
            // (function | optional) function called before it opens
            before_open: callbacks.beforeOpen,
            // (function | optional) function called after it opens
            after_open: callbacks.afterOpen,
            // (function | optional) function called before it closes
            before_close: callbacks.beforeClose,
            // (function | optional) function called after it closes
            after_close: callbacks.afterClose
        });
    }
}

/**
 * Shows Sweet Popup
 * @param {string} title Title for Popup
 * @param {string} msg Message for Popup
 * @param {string} type popup type ["success", "info", "warning", "error"]
 * @param {object} buttons buttons { okText: "OK", cancelText: "Cancel" }
 * @param {object} callbacks a callbacks object, {confirm : function(), cancel : function()}
 * @param {string} imageUrl Optional url for showing image in icon
 * @param {boolean} showLoaderOnConfirm Optional, shows a loading animations on Confirm button if set to true
 */
function ShowSweetPopup(title, msg, type, buttons, callbacks, imageUrl, showLoaderOnConfirm) {
    var okButtonColor = "btn-success";

    if (type === "error") { okButtonColor = "btn-danger"; }
    else if (type === "warning") {okButtonColor = "btn-warning";}
    else if (type === "info") { okButtonColor = "btn-info"; }
    else if (type === "success") { okButtonColor = "btn-success"; }
    else { okButtonColor = "btn-primary";}

    if (buttons === undefined)
        buttons = {};

    if (callbacks === undefined) {
        Swal.fire({            
            title: title,
            text: msg,
            type: type,
            confirmButtonText: buttons.okText !== undefined ? buttons.okText : "OK",
            confirmButtonClass: okButtonColor,
            imageUrl: imageUrl,
            allowOutsideClick: true,
            allowEscapeKey: true            
        });
    }
    else {
        Swal.fire({
            title: title,
            text: msg,
            type: type,
            allowOutsideClick: false,
            allowEscapeKey: false,
            showCancelButton: buttons.cancelText !== undefined,
            showCloseButton : buttons.cancelText === undefined,
            confirmButtonClass: okButtonColor,
            confirmButtonText: buttons.okText !== undefined ? buttons.okText : "Yes",
            cancelButtonText: buttons.cancelText !== undefined ? buttons.cancelText : "No",
            showLoaderOnConfirm: showLoaderOnConfirm
        }).then((result) => {
            if (result.value) {
                callbacks.confirm();
            } else if (
                // Read more about handling dismissals
                result.dismiss === Swal.DismissReason.cancel) {

                if (callbacks.cancel !== undefined)
                        callbacks.cancel();
            }
        });       
    }
}               
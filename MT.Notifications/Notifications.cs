﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace MT.Notifications
{
    public enum MessageType
    {
        [Description("info")]
        Info,
        [Description("error")]
        Error,
        [Description("warning")]
        Warning,
        [Description("success")]
        Success,
        [Description("message")]
        Message,
        [Description("question")]
        Question
    }

    public enum ToastrPosition
    {
        [Description("toast-top-right")]
        TopRight,

        [Description("toast-bottom-right")]
        BottomRight,

        [Description("toast-bottom-left")]
        BottomLeft,

        [Description("toast-top-left")]
        TopLeft,

        [Description("toast-top-full-width")]
        TopFullWidth,

        [Description("toast-bottom-full-width")]
        BottomFullWidth,

        [Description("toast-top-center")]
        TopCenter,

        [Description("toast-bottom-center")]
        BottomCenter
    }

    public class GritterCallback
    {
        public string BeforeOpen { get; set; }
        public string BeforeClose { get; set; }
        public string AfterOpen { get; set; }
        public string AfterClose { get; set; }
    }

    public class SweetCallBack
    {
        public SweetCallBack(string confirmCallbackFunction)
        {
            Debug.Assert(!string.IsNullOrEmpty(confirmCallbackFunction), "Confirm Callback cannot be null or empty string");

            if (string.IsNullOrEmpty(confirmCallbackFunction))
                throw new ArgumentNullException(nameof(confirmCallbackFunction), "Confirm Callback cannot be null or empty string");

            Confirm = confirmCallbackFunction;
        }

        public SweetCallBack(string confirmCallbackFunction, string cancelCallbachFunction)
        {
            Debug.Assert(!string.IsNullOrEmpty(confirmCallbackFunction), "Confirm Callback cannot be null or empty string");

            if (string.IsNullOrEmpty(confirmCallbackFunction))
                throw new ArgumentNullException(nameof(confirmCallbackFunction), "Confirm Callback cannot be null or empty string");

            Confirm = confirmCallbackFunction;
            Cancel = cancelCallbachFunction;
        }


        public string Confirm { get; }
        public string Cancel { get; set; }
    }

    public class Notifications
    {
        public static string OkText { get; set; } = "OK";
        public static string CancelText { get; set; } = "Cancel";

        #region Popup
        /// <summary>
        /// This function return the script string of a popup box
        /// To use this function, the return value of this must be placed in TempData["Script"] 
        /// in order to insert into ViewData.Script and send it to the view to show.
        /// </summary>
        /// <param name="msg">message to show</param>
        /// <param name="type">Type of popup, message will not show any icon</param>
        /// <param name="title">title for Popup box</param>
        /// <param name="okText">you can set it Globally via OkText Property of Notifications Class (default: OK)</param>
        /// <param name="cancelText">you can set it Globally via OkText Property of Notifications Class (default: Cancel)</param>
        /// <param name="imageUrl">Adds a custom image in place of icon</param>
        /// <param name="callbacks">Used when you need a confirm dialog box, you sould write a java script function name or call for every callback you want</param>
        /// <param name="showLoaderOnConfirm">optional, shows Loader if user clicked confirm button, used when you run an ajax request in confirm callback.</param>
        /// <param name="addOnDocumentReady">optional,set true to add jquery Document ready to script</param>
        /// <returns>script string</returns>
        public static string ShowPopup(string msg, MessageType type, string title, string okText, string cancelText, string imageUrl,
            SweetCallBack callbacks, bool showLoaderOnConfirm = false, bool addOnDocumentReady = false, bool addScript = true)
        {
            //ShowSweetPopup(title, msg, type, buttons, callbacks, imageUrl, showLoaderOnConfirm)           
            var str = new StringBuilder();
            
            if(addScript)
                str.Append("<script>");

            if (addOnDocumentReady)
                str.Append("$(function() {");

            title = System.Net.WebUtility.HtmlEncode(title);
            msg = System.Net.WebUtility.HtmlEncode(msg);

            str.Append($"ShowSweetPopup('{title}',`{msg}`");

            // add Type parameter
            if (type != MessageType.Message)
                str.Append($",'{type.GetDescription()}'");
            else
                str.Append($", 'info'");

            // Add buttons parameter
            var oktext = string.IsNullOrEmpty(okText) ? OkText : okText;
            var canceltext = string.IsNullOrEmpty(cancelText) ? CancelText : cancelText;

            str.Append($", {{okText : '{oktext}', cancelText: '{canceltext}'}}");

            // Add Callbacks
            if (callbacks != null)
            {
                str.Append($", {{confirm: {AddFunctionPhrase(callbacks.Confirm)}");

                if (callbacks.Cancel != null)
                    str.Append($", cancel: {AddFunctionPhrase(callbacks.Cancel)}");

                str.Append($"}}");
            }
            else
                str.Append($", undefined");

            // Add ImageUrl
            str.Append($", '{imageUrl}'");

            // Add showLoaderOnConfirm
            str.Append($", {showLoaderOnConfirm.ToString().ToLower()}");
            str.Append(");");

            if (addOnDocumentReady)
                str.Append("});");

            if(addScript)
                str.Append("</script>");

            return str.ToString();
        }

        /// <summary>
        /// This function return the script string of a popup box, 
        /// uses default texts for Ok and Cancel buttons
        /// </summary>
        /// <param name="msg">message to show</param>
        /// <param name="type">Type of popup, message will not show any icon</param>
        /// <returns></returns>
        public static string ShowPopup(string msg, MessageType type, bool addScript = true)
        {
            return ShowPopup(msg, type, string.Empty, string.Empty, string.Empty, string.Empty, null, addScript: addScript);
        }

        /// <summary>
        /// This function return the script string of a popup box, 
        /// uses default texts for Ok and Cancel buttons
        /// </summary>
        /// <param name="title">title for Popup box</param>
        /// <param name="msg">message to show</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ShowPopup(string msg, MessageType type, string title, bool addScript = true)
        {
            return ShowPopup(msg, type, title, string.Empty, string.Empty, string.Empty, null, addScript: addScript);
        }

        /// <summary>
        /// This function return the script string of a popup box,        
        /// </summary>
        /// <param name="title">title for Popup box</param>
        /// <param name="msg">message to show</param>
        /// <param name="type">Type of popup, message will not show any icon</param>
        /// <param name="okText">you can set it Globally via OkText Property of Notifications Class (default: OK)</param>
        /// <param name="cancelText">you can set it Globally via OkText Property of Notifications Class (default: Cancel)</param>
        /// <returns></returns>
        public static string ShowPopup(string msg, MessageType type, string title, string okText, string cancelText, bool addScript = true)
        {
            return ShowPopup(msg, type, title, okText, cancelText, string.Empty, null, addScript: addScript);
        }

        /// <summary>
        /// This function return the script string of a popup box with just Ok Button
        /// To use this function, the return value of this must be placed in TempData["Script"]
        /// in order to insert into ViewData.Script and send it to the view to show.
        /// </summary>
        /// <param name="msg">message to show</param>
        /// <param name="type">Type of popup, message will not show any icon</param>
        /// <param name="title">title for Popup box</param>
        /// <param name="okText">you can set it Globally via OkText Property of Notifications Class (default: OK)</param>                
        /// <param name="callbacks">Used when you need a confirm dialog box, you sould write a java script function name or call for every callback you want</param>
        /// <param name="closeOnConfirm">optional, close when Confirm button clicked or not.</param>
        /// <param name="addOnDocumentReady">optional,set true to add jquery Document ready to script</param>
        /// <returns>script string</returns>
        public static string ShowPopup(string msg, MessageType type, string title, string okText,
            SweetCallBack callbacks, bool addOnDocumentReady = false, bool addScript = true)
        {
            //Swal.fire({
            //  title: 'Are you sure?',
            //  text: "You won't be able to revert this!",
            //  type: 'warning',
            //  showCancelButton: true,
            //  confirmButtonText: 'Yes, delete it!',
            //  cancelButtonText: 'No, cancel!',
            //  reverseButtons: true
            //}).then((result) => {
            //  if (result.value) {
            //    Swal.fire(
            //      'Deleted!',
            //      'Your file has been deleted.',
            //      'success'
            //    )
            //  } else if (
            //    // Read more about handling dismissals
            //    result.dismiss === Swal.DismissReason.cancel
            //  ) {
            //    Swal.fire(
            //      'Cancelled',
            //      'Your imaginary file is safe :)',
            //      'error'
            //    )
            //  }
            //})

            var str = new StringBuilder();

            if (addScript)
                str.Append("<script>");

            if (addOnDocumentReady)
                str.Append("$(function() {");

            var typeStr = "null";
            // add Type parameter
            if (type != MessageType.Message)
                typeStr = type.GetDescription();
            else
                typeStr = "info";

            var buttonColor = "btn-success";
            if (type == MessageType.Error) { buttonColor = "btn-danger"; }
            else if (type == MessageType.Warning) { buttonColor = "btn-warning"; }
            else if (type == MessageType.Info) { buttonColor = "btn-info"; }
            else if (type == MessageType.Success) { buttonColor = "btn-success"; }
            else { buttonColor = "btn-primary"; }

            title = System.Net.WebUtility.HtmlEncode(title);
            msg = System.Net.WebUtility.HtmlEncode(msg);

            var scriptStr =
$@"Swal.fire({{
    title : '{title}',
    text : `{msg}`,
    type : '{typeStr}',
    showCancelButton : false,
    confirmButtonClass : '{buttonColor}',
    confirmButtonText : '{(string.IsNullOrEmpty(okText) ? OkText : okText)}',   
    allowEscapeKey : false,
    allowOutsideClick: false
    }})";

            if (callbacks != null)
            {
                scriptStr += $@".then((result) => {{ if (result.value) {{ {callbacks.Confirm} }} ";

                if (callbacks.Cancel != null)
                {
                    scriptStr += $@"else if (
                                    // Read more about handling dismissals
                                    result.dismiss === Swal.DismissReason.cancel
                                ) {{  {callbacks.Cancel} }}";
                }

                scriptStr += $"}} )";
            }

            scriptStr += ";";

            str.Append(scriptStr);

            if (addOnDocumentReady)
                str.Append("});");

            if(addScript)
                str.Append("</script>");

            return str.ToString();

        }

        public static string ShowConfirm(string msg, SweetCallBack callbacks, string title = "", string okText = "", string cancelText = "", MessageType type = MessageType.Question, bool showLoaderOnConfirm = false, bool addScript = true)
        {
            return ShowPopup(msg, type, title, okText, cancelText, string.Empty, callbacks, showLoaderOnConfirm, addScript: addScript);
        }

        /// <summary>
        /// This function return the script string of a error popup box,
        /// </summary>
        /// <param name="title">title for Popup box</param>
        /// <param name="msg">message to show</param>
        /// <returns></returns>
        public static string ShowErrorPopup(string title, string msg, bool addScript = true)
        {
            return ShowPopup(msg, MessageType.Error, title, addScript: addScript);
        }

        /// <summary>
        /// This function return the script string of a error popup box,
        /// </summary>
        /// <param name="msg">message to show</param>
        /// <returns></returns>
        public static string ShowErrorPopup(string msg, bool addScript = true)
        {
            return ShowPopup(msg, MessageType.Error, addScript: addScript);
        }
        #endregion

        /// <summary>
        /// This function generates an script string to show a message growl
        /// </summary>
        /// <param name="msg">Content of the message</param>
        /// <param name="type">Type of the message, can be one of {success, warning, info, danger}</param>
        /// <param name="title">Header message</param>
        /// <param name="position">The position for Toastr</param>
        /// <param name="addOnDocumentReady">set true to add jquery Document ready to script</param>
        /// <returns>string contains script needed to show a message growl</returns>
        public static string ShowToast(string title, string msg, MessageType type, ToastrPosition position = ToastrPosition.BottomRight, bool addOnDocumentReady = false, bool addScript = true)
        {
            // ShowToast(title, msg, position, type)
            
            var str = new StringBuilder();

            if (addScript)
                str.Append("<script>");

            if (addOnDocumentReady)
                str.Append("$(function() {");

            if (type == MessageType.Message || type == MessageType.Question)
                type = MessageType.Info;            

            str.Append($"ShowToast('{title}', `{msg}`, '{position.GetDescription()}', '{type.GetDescription()}');");

            if (addOnDocumentReady)
                str.Append("});");

            if(addScript)
                str.Append("</script>");

            return str.ToString();
        }

        /// <summary>
        /// Shows a toastr message in default position
        /// </summary>
        /// <param name="msg">Message of Toast</param>
        /// <param name="type">Type of Toast Message</param>
        /// <returns></returns>
        public static string ShowToast(string msg, MessageType type, bool addScript = true)
        {
            return ShowToast(string.Empty, msg, type, addScript: addScript);
        }

        /// <summary>
        /// Shows a toastr error message in default position
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string ShowErrorToast(string title, string msg, bool addScript = true)
        {
            return ShowToast(title, msg, MessageType.Error, addScript: addScript);
        }

        public static string ShowGritter(string msg, string title = "", string imageUrl = "", GritterCallback callbacks = null, bool addOnDocumentReady = false, bool addScript = true)
        {
            // ShowGritter(title, msg, imageUrl, callbacks)

            var str = new StringBuilder();

            if (addScript)
                str.Append("<script>");

            if (addOnDocumentReady)
                str.Append("$(function() {");

            str.Append($"ShowGritter('{title}', `{msg}`, '{imageUrl}'");

            if (callbacks != null)
            {
                str.Append($", {{");

                if (!string.IsNullOrEmpty(callbacks.BeforeOpen))
                    str.Append($"beforeOpen: {AddFunctionPhrase(callbacks.BeforeOpen)}, ");
                if (!string.IsNullOrEmpty(callbacks.AfterOpen))
                    str.Append($"afterOpen:  {AddFunctionPhrase(callbacks.AfterOpen)}, ");
                if (!string.IsNullOrEmpty(callbacks.BeforeClose))
                    str.Append($"beforeClose:{AddFunctionPhrase(callbacks.BeforeClose)}, ");
                if (!string.IsNullOrEmpty(callbacks.AfterClose))
                    str.Append($"afterClose: {AddFunctionPhrase(callbacks.AfterClose)}");

                str.Append($"}}");
            }
            else
                str.Append($", undefined");

            str.Append($");");

            if (addOnDocumentReady)
                str.Append("});");

            if(addScript)                  
                str.Append("</script>");

            return str.ToString();
        }

        /// <summary>
        /// Helper function to add function() phrase if not exist.
        /// </summary>
        /// <param name="callbackFunction">a string that represents a javascript function</param>
        /// <returns>string with function() phrase added if not have.</returns>
        private static string AddFunctionPhrase(string callbackFunction)
        {
            if (string.IsNullOrEmpty(callbackFunction))
                return callbackFunction;

            return callbackFunction.StartsWith("function()") ? callbackFunction : $"function() {{ {callbackFunction }}}";
        }
    }
}

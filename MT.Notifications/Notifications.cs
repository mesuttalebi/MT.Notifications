using System.Text;

namespace MT.Notifications.Net
{
    public enum MessageType
    {   
        Info,
        Error,
        Warning,
        Success,
        Message
    }    

    public enum JsLibrary : byte
    {
        Toastr = 0,
        Gritter = 1,
        SweetAlert = 2        
    }  
    
    public class SweetCallBack
    {
        public string Confirm { get; set; }
        public string Cancel { get; set; }
    }
   

    public static class Notifications
    {
        public static string OkText { get; set; } = "OK";
        public static string CancelText { get; set; } = "Cancel";

        /// <summary>
        /// This function return the script string of a popup box
        /// To use this function, the return value of this must be placed in TempData["Script"] 
        /// in order to insert into ViewData.Script and send it to the view to show.
        /// </summary>
        /// <param name="msg">message to show</param>
        /// <param name="okText">text to use in Ok button</param>
        /// <param name="type">Type of popup, error, alert or info</param>
        /// <param name="addOnDocumentReady">set true to add jquery Document ready to script</param>
        /// <returns>script string</returns>
        public static string ShowPopup(string msg, string okText, MessageType type, SweetCallBack callbacks = null, bool addOnDocumentReady = false, string title = "")
        {
            //ShowSweetPopup(title, msg, type, buttons, callbacks, imageUrl)           
            var str = new StringBuilder();
            str.Append("<script>");
            if (addOnDocumentReady)
                str.Append("$(function() {");
            str.Append($"ShowSweetPopup('{title}','{msg}'");

            if (type != MessageType.Message)
                str.Append($",'{type.ToString().ToLower()}'");

            if (!string.IsNullOrEmpty(okText))
                str.Append($", {{okText : {OkText}, cancelText: {CancelText}}}");

            if (callbacks != null)
                str.Append($", {{confirm: {callbacks.Confirm}, cancel: {callbacks.Cancel}}}");

            str.Append(");");

            if (addOnDocumentReady)
                str.Append("});");
            str.Append("</script>");

            return str.ToString();
        }

        /// <summary>
        /// Belirtilen tipte popup acılır.Default olarak toastr 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <param name="btnOk"></param>
        /// <param name="type"></param>
        /// <param name="addOnDocumentReady"></param>
        /// <param name="jsLibrary"></param>
        /// <returns></returns>
        public static string ShowPopup(MessageType type, string title, string msg, bool addOnDocumentReady = false, JsLibrary jsLibrary = JsLibrary.Toastr)
        {
            StringBuilder str = new StringBuilder();
            str.Append("<script>");
            if (addOnDocumentReady)
                str.Append("$(function() {");
            str.Append($"ShowPopupBox({(byte)type},'{title}','{msg}',{(byte)jsLibrary});");
            if (addOnDocumentReady)
                str.Append("});");
            str.Append("</script>");

            return str.ToString();
        }

        /// <summary>
        /// This function return the script string of a popup box that when you click Ok button it will refresh parent window of the modal
        /// This function is for modal windows.
        /// To use this function, the return value of this must be placed in TempData["Script"] 
        /// in order to insert into ViewData.Script and send it to the view to show.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="btnOk"></param>
        /// <param name="type"></param>
        /// <param name="addOnDocumentReady"></param>
        /// <returns></returns>
        public static string ShowPopupAndReloadParent(string msg, string btnOk, MessageType type, bool addOnDocumentReady = false)
        {
            StringBuilder str = new StringBuilder();
            str.Append("<script>");
            if (addOnDocumentReady)
                str.Append("$(function() {");
            str.Append($"ShowPopupAndReloadParent('{msg}','{btnOk}', '{type}');");
            if (addOnDocumentReady)
                str.Append("});");
            str.Append("</script>");

            return str.ToString();
        }


        /// <summary>
        /// This function generates an script string to show a message growl
        /// </summary>
        /// <param name="header">Header message</param>
        /// <param name="msg">Content of the message</param>
        /// <param name="type">Type of the message, can be one of {success, warning, info, danger}</param>
        /// <param name="addOnDocumentReady">set true to add jquery Document ready to script</param>
        /// <returns>string contains script needed to show a message growl</returns>
        public static string ShowGrowl(string header, string msg, MessageType type, bool addOnDocumentReady = false)
        {
            var messageType = type.ToString();
            if (messageType == "danger") messageType = "error";

            var str = new StringBuilder();
            str.Append("<script>");
            if (addOnDocumentReady)
                str.Append("$(function() {");
            str.Append($"ShowGrowl('{header}','{msg}','{messageType}');");
            if (addOnDocumentReady)
                str.Append("});");
            str.Append("</script>");

            return str.ToString();
        }        
    }
}
using System;
using System.Web;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Orak.Identity;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using SmartRegistry.Web.Services;
using System.Configuration;
using SmartRegistry.Web.Models;
using SmartRegistry.Domain.Services;
using SmartRegistry.Domain.Entities;
using SmartRegistry.Web.AuthHelp.Models;
using SmartRegistry.Web.AuthHelp;
using System.Threading.Tasks;
using SmartRegistry.Web.AuthHelp.Enums;
using SmartRegistry.Domain.Common;
using System.Collections.Generic;
using System.IO;

namespace SmartRegistry.Web.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private readonly EauthXmlService _xmlService;

        public AuthenticationController()
        {
            _xmlService = new EauthXmlService();
        }

        public AuthenticationController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _xmlService = new EauthXmlService();
        }



        // GET: Authentication
        public ActionResult Index()
        {
                        
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        public ActionResult Verify()
        {
            if (Request.QueryString.Count != 0)
            {     
                var resetCode = Request.QueryString[0];

                var userToReset = SmartContext.DbContext.GetUsersDao().GetByResetCode(resetCode.ToString());
                if (userToReset == null) {
                    ViewBag.ErrorMsg = "Линкът за потвърждение е използван вече!";
                    return View();
                }

                var newcookie = new HttpCookie(SmartRegistryConstants.RESET_CODE, resetCode.ToString());
                newcookie.Expires = DateTime.Now.AddDays(1);
                newcookie.SameSite = SameSiteMode.Lax;
                HttpContext.Response.Cookies.Add(newcookie);                
            }
           
            return View();
        }

        public ActionResult Redirect(bool login = true,string returnUrl = null)
        {
            #region old
            //var urlbase = Request.MapPath("~/Authentication/Result");
            //var resultUrl = this.Url.Action("Result", "Authentication", null, Request.Url.Scheme);
            //string SSODestination = ConfigurationManager.AppSettings["SSODestination"];
            //var reqXML = _xmlService.Create(resultUrl);

            //string formID = "PostForm";

            //byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(reqXML.OuterXml);
            //var testXml = System.Convert.ToBase64String(toEncodeAsBytes);



            //StringBuilder strForm = new StringBuilder();
            //strForm.Append("<form id=\"" + formID + "\" name=\"" + formID + "\" action=\"" + SSODestination + "\" method=\"POST\">");
            //strForm.AppendFormat("<input type ='hidden' id ='{0}' name ='{0}' value ='{1}'/>", "SAMLRequest", testXml);
            ////  strForm.AppendFormat("<input type ='hidden' id ='SAMLRequest' name ='SAMLRequest' value = " + testXml + "/>");
            //strForm.Append("<input type = 'hidden' name = 'RelayState' value = 'token' />");
            //strForm.Append("</form>");

            //////Build the JavaScript which will do the Posting operation.
            //StringBuilder strScript = new StringBuilder();
            //strScript.Append("<script language='javascript'>");
            //strScript.Append("var v" + formID + " = document." + formID + ";");
            //strScript.Append("v" + formID + ".submit();");
            //strScript.Append("</script>");

            ////Return the form and the script concatenated. (The order is important, Form then JavaScript)
            //var test = strForm.ToString() + strScript.ToString();

            ////Page.Controls.Add(new LiteralControl(test));
            //Response.Write(test);
            //return PartialView();
            #endregion

            CertAuthViewModel model = new CertAuthViewModel();

            //X509Certificate2 cert = SAML2.Config.Saml2Section
            //    .GetConfig()
            //    .ServiceProvider
            //    .SigningCertificate
            //    .GetCertificate();

            System.Xml.XmlDocument authnRequest =
                SamlHelper.GenerateKEPAuthnRequest(model);

            // string signedXml = SamlHelper.SignXmlDocument(authnRequest, cert);
            var signedXml = authnRequest.OuterXml;
            string encodedStr = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(signedXml));

            model.EncodedRequest = encodedStr;

            string relayState = login
                ? CertAuthViewModel.RelayStateLogin
                : string.Empty;

            if (!string.IsNullOrEmpty(returnUrl))
            {
                relayState += ";" + returnUrl;
            }

            model.EncodedRelayState = !string.IsNullOrEmpty(relayState)
                ? Convert.ToBase64String(Encoding.UTF8.GetBytes(relayState))
                : string.Empty;

            return View("CertificateAuth", model);


        }

        public ActionResult TestLogin(char? id)
        {
            var identNumber = "1111111111";
            if (id != null)
            {
                identNumber = new string((char)id, 10);
            }
            EGovAuthUser egovUser = new EGovAuthUser();
            egovUser.PersonName = "Тестов профил";
            egovUser.PersonalIndentifier = "PNOBG-"+ identNumber;

            var usrSvc = SmartContext.UsersService;
            var usr = usrSvc.CheckForUserByPersonalIdentificator(egovUser);

            if (usr != null)
            {

                var user = new ApplicationUser()
                {
                    Id = usr.Id.ToString(),
                    UserName = usr.Name
                };

                SignInManager.SignIn(user, true, true);

              SmartContext.SystemLog.LogEvent(SmartContext, SystemLogEventTypeEnum.LoginSuccess, "Успешен логин на: " + usr.Name, usr.Id);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Result = "Не е намерен тестов акаунт!";
                return View();

            }
        }

        public ActionResult Result()
        {
            string rawSamlData = Request["SAMLResponse"];
            string SamlResponse = string.Empty;
            SamlResponse = TempData["samlResponce"].ToString();

           

            string decodedStr = Encoding.UTF8.GetString(
                Convert.FromBase64String(SamlResponse));

            logResult(decodedStr);

            CertificateAuthResponse response = new CertificateAuthResponse();

            switch (SamlHelper.SamlConfiguration.SamlVersion)
            {
                case 1:
                    response = SamlHelper.ParseSaml2CertificateResult(decodedStr);
                    break;
                case 2:
                    response = SamlHelper.ParseSaml2CertificateResultV2(decodedStr);
                    break;
            }

            string relayState = string.Empty;
            string returnUrl = string.Empty;


            switch (response.ResponseStatus)
            {
                case eCertResponseStatus.Success:
                    return LoginInternalKEP(response, returnUrl);
                case eCertResponseStatus.InvalidSignature:
                case eCertResponseStatus.InvalidResponseXML:
                case eCertResponseStatus.MissingEGN:
                case eCertResponseStatus.CanceledByUser:
                    return RedirectToAction(
                       nameof(AuthenticationController.Index));
                case eCertResponseStatus.AuthenticationFailed:
                default:
                    return RedirectToAction(
                        nameof(AuthenticationController.Index));
            }


        }

        private void logResult(string data)
        {

            var fileName = AppDomain.CurrentDomain.BaseDirectory + "samlData.txt";

            FileInfo fi = new FileInfo(fileName);

            try
            {
                // Check if file already exists. If yes, write to it.     
                if (fi.Exists)
                {
                    using (var tw = new StreamWriter(fileName, true))
                    {
                        tw.WriteLine(data);
                        tw.WriteLine(Environment.NewLine);
                    }

                }
                else {
                    // Create a new file     
                    using (StreamWriter sw = fi.CreateText())
                    {
                        sw.WriteLine(data);
                        sw.WriteLine(Environment.NewLine);
                    }

                }

            
            }
            catch (Exception Ex)
            {
               // Console.WriteLine(Ex.ToString());
                var fileName2 = AppDomain.CurrentDomain.BaseDirectory + "samlData" +DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".txt";

                FileInfo fi2 = new FileInfo(fileName2);
                using (StreamWriter sw = fi2.CreateText())
                {
                    sw.WriteLine(Environment.NewLine);
                    sw.WriteLine(Ex.Message);
                    sw.WriteLine(Environment.NewLine);
                    sw.WriteLine(data);
                    sw.WriteLine(Environment.NewLine);
                }
            }
        }

        /// <summary>
        /// Used for user signout
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Authentication");
        }



        [AllowAnonymous]
        [HttpPost]
        public ActionResult AuthenticateCertificate(
            string SamlResponse,
            string RelayState)
        {

            TempData["samlResponce"] = SamlResponse;
            return RedirectToAction("Result");

        }

        private ActionResult LoginInternalKEP(
            CertificateAuthResponse response,
            string returnUrl = null)
        {
            try
            {
                string error = LoginInternal(response);

                if (!string.IsNullOrEmpty(error))
                {
                    TempData["ErrorMessage"] = error;

                    return RedirectToAction(
                        nameof(AuthenticationController.Index));
                }

                return RedirectToAction(
                      nameof(HomeController.Index));
            }
            catch (Exception ex)
            {               
                return RedirectToAction(
                    nameof(AuthenticationController.Index));
            }
        }
     

        private string LoginInternal(CertificateAuthResponse logindata)
        {

            EGovAuthUser egovUser = new EGovAuthUser();

            egovUser.PersonalIndentifier = logindata.EGN;
            egovUser.LatinName = logindata.LatinNames;
            egovUser.Phone = logindata.PhoneNumber;
            egovUser.Email = logindata.Email;
            egovUser.DateOfBirth = logindata.DateOfBirth.ToShortDateString() ;

            var usrSvc = SmartContext.UsersService;
            var usr = usrSvc.CheckForUserByEGN(egovUser);

            string cookieVal = string.Empty;
         
            var cookie = Request.Cookies[SmartRegistryConstants.RESET_CODE];
            if (cookie != null)
            {
                cookieVal = cookie.Value;              
            }

            if (cookieVal.Length!=0 && usr == null)
            {
             
                usr = usrSvc.GetByResetCode(cookieVal);
                if (usr != null)
                {
                    var identType = DbContext.GetUserIdentificatorTypesDao().GetByIdentificatorType("PNO");

                    UserIdentificator uid = new UserIdentificator();
                    uid.Identificator = egovUser.PersonalIndentifier;
                    uid.IdentificatorType = identType;
                    uid.User = usr;

                    var dbIdent = DbContext.GetUserIdentificatorDao();
                    dbIdent.Save(uid);

                    //usr.Identificators = new List<UserIdentificator>();
                    //usr.Identificators.Add(new UserIdentificator()
                    //{
                    //    Identificator = egovUser.CivilIdentificationNumber,
                    //    IdentificatorType = identType,
                    //    User = usr
                    //});
                    //usrSvc.Save(usr);

                    HttpCookie currentUserCookie = HttpContext.Request.Cookies[SmartRegistryConstants.RESET_CODE];
                    HttpContext.Response.Cookies.Remove(SmartRegistryConstants.RESET_CODE);
                    currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                    currentUserCookie.Value = null;
                    HttpContext.Response.SetCookie(currentUserCookie);

                    usr.ResetCode = string.Empty;
                    var userDao = DbContext.GetUsersDao(); 
                    userDao.Save(usr);


                }
            }

            if (usr != null)
            {

                var user = new ApplicationUser()
                {
                    Id = usr.Id.ToString(),
                    UserName = usr.Name
                };


               SmartContext.SystemLog.LogEvent(SmartContext,SystemLogEventTypeEnum.LoginSuccess, "Успешен логин на: " + usr.Name, usr.Id);
                

                SignInManager.SignIn(user, false, false);
                return string.Empty;
                // return RedirectToAction("Index", "Home");

            }
            else
            {
                SmartContext.SystemLog.LogEvent(SmartContext, SystemLogEventTypeEnum.UnauthorizedLogin, "Не успешен логин на: " + egovUser.LatinName, 0);
                ViewBag.Result = "Вашият профил не е активен! Обърнете се към локалният администратор на ПАД!";
                return "Вашият профил не е активен! Обърнете се към локалният администратор на ПАД!";
                // return View();
            }
        }
    }
}
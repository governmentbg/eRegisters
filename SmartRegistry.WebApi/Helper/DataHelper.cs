using SmartRegistry.DataAccess;
using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Interfaces;
using SmartRegistry.Domain.QueryFilters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Xml;

namespace SmartRegistry.WebApi.Helper
{
    public class DataHelper
    {


        private IDbContext _dbContext;
        public IDbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                {
                    _dbContext = (NHibernateDbContext)HttpContext.Current.Items[ServiceConstants.DbContextRequestKey];
                }
                if (_dbContext == null)
                {
                    _dbContext = new NHibernateDbContext();
                    HttpContext.Current.Items[ServiceConstants.DbContextRequestKey] = _dbContext;
                }
                return _dbContext;
            }
        }

        public DataHelper()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SessionFactoryManager.Initialize(connectionString);
        }



        public ServiceResultISCIPR ProccessData(RequestDataISCIPR requestData, WebServiceType serviceType)
        {
            var registerRecDao = DbContext.RegisterRecordsDao;
            ServiceResultISCIPR result = new ServiceResultISCIPR();
            List<RegisterRecord> recordList = new List<RegisterRecord>();

            var operation = requestData.Operation;
            string[] listOperation = operation.Split('.');
            var namespaceAdminBody = listOperation[0];
            var namesapaceRegister = listOperation[1];
            var namespaceService = listOperation[2];

            logRequest(requestData.CallContext);

            WebServiceISCIPR webservice = null;

            User usr = null;
            try
            {
                webservice = DbContext.WebServicesDao.GetByServiceKey(namespaceService);
                usr = getServiceUser();

                if (webservice != null)
                {
                    var listAttributes = webservice.AttributeHead.Attributes;
                    var argumentData = requestData.Argument;
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(argumentData.OuterXml);
                    var elementList = doc.GetElementsByTagName(namesapaceRegister + "Record");

                    foreach (XmlNode baseNode in elementList)
                    {
                        var childList = baseNode.ChildNodes;
                        var xmlElement = baseNode as XmlElement;

                        RegisterRecord regRec = new RegisterRecord();
                        var atrb = webservice.AttributeHead;
                        regRec.AttributeHead = atrb;
                        regRec.Version++;
                        regRec.Register = webservice.Register;
                        regRec.CreatedBy = usr;
                        regRec.IsActive = true;

                        if (serviceType == WebServiceType.CreateRecord)
                        {
                            var externalIdNode = xmlElement.GetElementsByTagName("ExternalId");
                            if (externalIdNode.Count != 0)
                            {
                                var externalId = externalIdNode[0].InnerText; ;
                                regRec.ExternalId = externalId;
                            }
                        }

                        if (serviceType == WebServiceType.ChangeRecord)
                        {

                            var uriNode = xmlElement.GetElementsByTagName("URI");
                            if (uriNode.Count != 0)
                            {
                                var uri = uriNode[0].InnerText;
                                RegisterRecordFilter recFilter = new RegisterRecordFilter();
                                recFilter.URI = uri;
                                regRec = DbContext.RegisterRecordsDao.GetRecords(recFilter).Results.FirstOrDefault();
                                regRec.ModifiedBy = usr;
                            }

                            var externalIdNode = xmlElement.GetElementsByTagName("ExternalId");
                            if (externalIdNode.Count != 0)
                            {
                                var externalId = externalIdNode[0].InnerText; ;
                                RegisterRecordFilter recFilter = new RegisterRecordFilter();
                                recFilter.ExternalId = externalId;
                                regRec = DbContext.RegisterRecordsDao.GetRecords(recFilter).Results.FirstOrDefault();
                                regRec.ModifiedBy = usr;
                            }

                            if (externalIdNode.Count == 0 && uriNode.Count == 0)
                            {
                                result.HasError = true;
                                result.ErrorMessage = "Възникна грешка ! Липсва URI/ExternalId елемент в XML!";
                                result.ErrorCode = "Липсва URI елемент в XML!";
                                return result;
                            }

                            if (regRec == null)
                            {
                                result.HasError = true;
                                result.ErrorMessage = "Възникна грешка ! Не са намерени данните за редактиране!";
                                result.ErrorCode = "Данните за редактиране не са намерени!";
                                return result;
                            }

                        }

                        foreach (XmlNode childNode in childList)
                        {


                            var nodeName = childNode.Name;
                            var nodeValue = childNode.InnerXml;

                            var selectedAttr = listAttributes.Where(x => x.ApiName == nodeName).FirstOrDefault();
                            if (selectedAttr == null)
                            {
                                selectedAttr = listAttributes.Where(x => x.UnifiedData.NamespaceApi == nodeName).FirstOrDefault();
                            }

                            if (selectedAttr != null)
                            {

                                var attr = DbContext.GetRegisterAttributeDao().GetById(selectedAttr.Id);
                                RegisterRecordValueBase attrValue = null;

                                switch (attr.UnifiedData.DataType)
                                {
                                    case UnifiedDataTypeEnum.Text:
                                        attrValue = SetTextAttrValue(regRec, attr, nodeValue);
                                        break;
                                    case UnifiedDataTypeEnum.String:
                                        attrValue = SetStringAttrValue(regRec, attr, nodeValue);
                                        break;
                                    case UnifiedDataTypeEnum.Date:
                                        attrValue = SetDateAttrValue(regRec, attr, nodeValue);
                                        break;
                                    case UnifiedDataTypeEnum.Integer:
                                        attrValue = SetIntAttrValue(regRec, attr, nodeValue);
                                        break;
                                    case UnifiedDataTypeEnum.Decimal:
                                        attrValue = SetDecimalAttrValue(regRec, attr, nodeValue);
                                        break;
                                }
                                if (attrValue != null)
                                {
                                    attrValue.Version = regRec.Version;
                                    attrValue.IsActive = true;
                                }
                            }

                        }
                        registerRecDao.Save(regRec);
                        if (regRec.URI == null)
                        {
                            regRec.URI = generateURI(regRec);
                            registerRecDao.Save(regRec);
                        }
                        recordList.Add(regRec);
                    }

                }
            }
            catch (Exception e)
            {
                result.HasError = true;
                result.ErrorMessage = "Възникна грешка при обработването на данни.";
                result.ErrorCode = e.Message;
                return result;

            }

            try
            {
                XmlDocument xDoc = new XmlDocument();
                XmlElement span = xDoc.CreateElement(namesapaceRegister + "Request" + "Result");

                span.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                span.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                span.SetAttribute("xmlns", "http://iscipr.egov.bg/SmartRegistry/" + namespaceService + "Record" + "Result");

                foreach (RegisterRecord rc in recordList)
                {
                    XmlElement uri = xDoc.CreateElement("URI");
                    uri.InnerText = rc.URI;
                    span.AppendChild(uri);
                }

                xDoc.AppendChild(span);
                result.Data = xDoc.DocumentElement;
                result.HasError = false;

            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.ErrorMessage = "Възникна грешка при връщането на xml.";
                result.ErrorCode = ex.Message;
                return result;
            }

            return result;
        }

        public enum EntryIdType : int
        {
            URI = 1,
            ExternalId = 2,
        }

        public ServiceResultISCIPR DeleteData(RequestDataISCIPR requestData, WebServiceType serviceType)
        {
            var registerRecDao = DbContext.RegisterRecordsDao;
            ServiceResultISCIPR result = new ServiceResultISCIPR();
            List<RegisterRecord> recordList = new List<RegisterRecord>();

            var operation = requestData.Operation;
            string[] listOperation = operation.Split('.');
            var namespaceAdminBody = listOperation[0];
            var namesapaceRegister = listOperation[1];
            var namespaceService = listOperation[2];

            logRequest(requestData.CallContext);

            WebServiceISCIPR webservice = null;
            RegisterRecord regRec = null;

            User usr = null;
            try
            {

                webservice = DbContext.WebServicesDao.GetByServiceKey(namespaceService);
                usr = getServiceUser();

                if (webservice != null)
                {
                    var argumentData = requestData.Argument;

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(argumentData.OuterXml);
                    var uriNode = doc.GetElementsByTagName("URI");
                    foreach (XmlNode node in uriNode)
                    {

                        var uri = node.InnerText;
                        RegisterRecordFilter recFilter = new RegisterRecordFilter();
                        recFilter.URI = uri;                       
                        regRec = DbContext.RegisterRecordsDao.GetRecords(recFilter).Results.FirstOrDefault();

                        if (regRec != null)
                        {
                            regRec.IsActive = false;
                            regRec.ModifiedBy = usr;
                            registerRecDao.Save(regRec);
                            recordList.Add(regRec);
                        }
                        else
                        {
                            result.HasError = true;
                            result.ErrorMessage = "Възникна грешка ! Не са намерени данните за изтриване!";
                            result.ErrorCode = "Данните за изтриване не са намерени!";
                            return result;
                        }
                    }

                    var externalNodes = doc.GetElementsByTagName("ExternalId");
                    foreach (XmlNode node in externalNodes)
                    {

                        var uri = node.InnerText;
                        RegisterRecordFilter recFilter = new RegisterRecordFilter();                     
                        recFilter.ExternalId = uri;                        
                        regRec = DbContext.RegisterRecordsDao.GetRecords(recFilter).Results.FirstOrDefault();

                        if (regRec != null)
                        {
                            regRec.IsActive = false;
                            regRec.ModifiedBy = usr;
                            registerRecDao.Save(regRec);
                            recordList.Add(regRec);
                        }
                        else
                        {
                            result.HasError = true;
                            result.ErrorMessage = "Възникна грешка ! Не са намерени данните за изтриване!";
                            result.ErrorCode = "Данните за изтриване не са намерени!";
                            return result;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result.HasError = true;
                result.ErrorMessage = "Възникна грешка при обработването на данни.";
                result.ErrorCode = e.Message;
                return result;

            }

            try
            {
                XmlDocument xDoc = new XmlDocument();
                XmlElement span = xDoc.CreateElement(namespaceService + "Record" + "Result");

                span.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                span.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                span.SetAttribute("xmlns", "http://iscipr.egov.bg/SmartRegistry/" + namespaceService + "Record" + "Result");

                foreach (RegisterRecord rc in recordList)
                {
                    XmlElement uri = xDoc.CreateElement("URI");
                    uri.InnerText = rc.URI;
                    span.AppendChild(uri);
                }

                xDoc.AppendChild(span);
                result.Data = xDoc.DocumentElement;
                result.HasError = false;

            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.ErrorMessage = "Възникна грешка при връщането на xml.";
                result.ErrorCode = ex.Message;
                return result;
            }

            return result;
        }

        private void logRequest(CallContext callContext)
        {
            //todo add logging data
            ServiceLog log = new ServiceLog();
            log.InsDateTime = DateTime.Now;
            log.ServiceType = Int32.Parse(callContext.ServiceType);
            log.ServiceUri = callContext.ServiceURI;
            log.EmployeeIdentificator = callContext.EmployeeIdentifier;
            log.EmployeeNames = callContext.EmployeeNames;

            DbContext.GetServiceLogDao().Save(log);

        }

        private User getServiceUser()
        {
            UserFilter usrFilter = new UserFilter();
            usrFilter.Name = ServiceConstants.ServiceUser;

            var rslt = DbContext.GetUsersDao().GetAllUsers(usrFilter);
            if (rslt.RowCount != 0)
            {
                return rslt.Results[0];
            }
            else
            {
                User usr = new User();
                usr.Name = ServiceConstants.ServiceUser;
                usr.IsActive = true;
                DbContext.GetUsersDao().Save(usr);
                return usr;

            }



        }

        public string generateURI(RegisterRecord reg)
        {
            // (<ИД на адм орган>-<ИД на регистър>-<ИД на запис>)
            string uri = string.Empty;
            var admnId = reg.Register.AdministrativeBody.Id.ToString();
            var regId = reg.Register.Id.ToString();
            uri = admnId + "-" + regId + "-" + reg.Id.ToString();
            return uri;
        }

        public RegisterRecordValueNVarchar SetStringAttrValue(RegisterRecord regRec, RegisterAttribute attr, object val)
        {
            string strVal = (string)val;

            var attrValue = regRec.GetVarcharValue(attr);
            if (attrValue == null)
            {
                attrValue = new RegisterRecordValueNVarchar()
                {
                    Attribute = attr,
                    RegisterRecord = regRec,
                };

                regRec.VarcharValues.Add(attrValue);
            }
            attrValue.Value = strVal;
            return attrValue;
        }

        public RegisterRecordValueInt SetIntAttrValue(RegisterRecord regRec, RegisterAttribute attr, object val)
        {
            int strVal = Int32.Parse(val.ToString());

            var attrValue = regRec.GetIntValue(attr);
            if (attrValue == null)
            {
                attrValue = new RegisterRecordValueInt()
                {
                    Attribute = attr,
                    RegisterRecord = regRec,
                };

                regRec.IntValues.Add(attrValue);
            }
            attrValue.Value = strVal;
            return attrValue;
        }


        public RegisterRecordValueNumeric SetDecimalAttrValue(RegisterRecord regRec, RegisterAttribute attr, object val)
        {
            decimal strVal = Decimal.Parse(val.ToString());

            var attrValue = regRec.GetDecimalValue(attr);
            if (attrValue == null)
            {
                attrValue = new RegisterRecordValueNumeric()
                {
                    Attribute = attr,
                    RegisterRecord = regRec,
                };

                regRec.DecimalValues.Add(attrValue);
            }
            attrValue.Value = strVal;
            return attrValue;
        }

        public RegisterRecordValueText SetTextAttrValue(RegisterRecord regRec, RegisterAttribute attr, object val)
        {
            string strVal = (string)val;

            var attrValue = regRec.GetTextValue(attr);
            if (attrValue == null)
            {
                attrValue = new RegisterRecordValueText()
                {
                    Attribute = attr,
                    RegisterRecord = regRec,
                };

                regRec.TextValues.Add(attrValue);
            }
            attrValue.Value = strVal;
            return attrValue;
        }

        public RegisterRecordValueDateTime SetDateAttrValue(RegisterRecord regRec, RegisterAttribute attr, object val)
        {
            // DateTime? dtVal = (DateTime?)val;
            DateTime? result = DateTime.ParseExact((string)val, "yyyy-MM-dd", CultureInfo.InvariantCulture);


            var attrValue = regRec.GetDateTimeValue(attr);
            if (attrValue == null)
            {
                attrValue = new RegisterRecordValueDateTime()
                {
                    Attribute = attr,
                    RegisterRecord = regRec,
                };

                regRec.DateTimeValues.Add(attrValue);
            }

            attrValue.Value = result;
            return attrValue;
        }



    }
}
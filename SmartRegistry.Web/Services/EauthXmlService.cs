using SmartRegistry.Domain.Entities;
using SmartRegistry.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SmartRegistry.Web.Services
{
    public class EauthXmlService
    {
        public  XmlDocument Create(String resultUrl)
        {
            var time = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            var id = GenerateXMLId();

            string SSODestination = ConfigurationManager.AppSettings["SSODestination"];
            string SSOIssuer = ConfigurationManager.AppSettings["SSOIssuer"];
           
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);
            XmlElement element1 = doc.CreateElement("saml2p", "AuthnRequest", "urn:oasis:names:tc:SAML:2.0:protocol");

            element1.SetAttribute("AssertionConsumerServiceURL", resultUrl );
            element1.SetAttribute("Destination", SSODestination);
            element1.SetAttribute("ForceAuthn", "false");
            element1.SetAttribute("ID", id);
            element1.SetAttribute("IsPassive", "false");
            element1.SetAttribute("IssueInstant", time);
            element1.SetAttribute("ProtocolBinding", "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST");
            element1.SetAttribute("Version", "2.0");
            element1.SetAttribute("RelayState", resultUrl );
            element1.IsEmpty = false;

            XmlElement chld1 = doc.CreateElement("saml2", "Issuer", "urn:oasis:names:tc:SAML:2.0:assertion");
            chld1.InnerText = SSOIssuer;
            chld1.IsEmpty = false;
            element1.AppendChild(chld1);

            XmlElement extchld = doc.CreateElement("saml2p", "Extensions", "urn:oasis:names:tc:SAML:2.0:protocol");
            extchld.IsEmpty = false;


            XmlElement requestedService = doc.CreateElement("egovbga", "RequestedService", "urn:bg:egov:eauth:2.0:saml:ext");
            requestedService.IsEmpty = false;

            XmlElement chldService = doc.CreateElement("egovbga", "Service", "urn:bg:egov:eauth:2.0:saml:ext");
            chldService.InnerText = "2.16.100.1.1.1.1.13.1.1.2";
            chldService.IsEmpty = false;

            XmlElement chldProvider = doc.CreateElement("egovbga", "Provider", "urn:bg:egov:eauth:2.0:saml:ext");
            chldProvider.InnerText = "2.16.100.1.1.1.1.13";
            chldProvider.IsEmpty = false;

            XmlElement chldLevelOfAssurance = doc.CreateElement("egovbga", "LevelOfAssurance", "urn:bg:egov:eauth:2.0:saml:ext");
            chldLevelOfAssurance.InnerText = "LOW";
            chldLevelOfAssurance.IsEmpty = false;



            XmlElement requestedAttributes = doc.CreateElement("egovbga", "RequestedAttributes", "urn:bg:egov:eauth:2.0:saml:ext");
            requestedAttributes.IsEmpty = false;

            XmlElement requestedAttribute1 = doc.CreateElement("egovbga", "RequestedAttribute", " ");
            requestedAttribute1.IsEmpty = false;
            requestedAttribute1.SetAttribute("FriendlyName", "latinName");
            requestedAttribute1.SetAttribute("Name", "urn:egov:bg:eauth:2.0:attributes:latinName");
            requestedAttribute1.SetAttribute("NameFormat", "urn:oasis:names:tc:saml2:2.0:attrname-format:uri");
            requestedAttribute1.SetAttribute("isRequired", "false");
            XmlElement val = doc.CreateElement("egovbga", "AttributeValue", " ");
            val.InnerText = "urn:egov:bg:eauth:2.0:attributes:latinName";
            requestedAttribute1.AppendChild(val);


            XmlElement requestedAttribute2 = doc.CreateElement("egovbga", "RequestedAttribute", " ");
            requestedAttribute2.IsEmpty = false;
            requestedAttribute2.SetAttribute("FriendlyName", "birthName");
            requestedAttribute2.SetAttribute("Name", "urn:egov:bg:eauth:2.0:attributes:birthName");
            requestedAttribute2.SetAttribute("NameFormat", "urn:oasis:names:tc:saml2:2.0:attrname-format:uri");
            requestedAttribute2.SetAttribute("isRequired", "false");
            XmlElement val2 = doc.CreateElement("egovbga", "AttributeValue", " ");
            val2.InnerText = "urn:egov:bg:eauth:2.0:attributes:birthName";
            requestedAttribute2.AppendChild(val2);

            XmlElement requestedAttribute3 = doc.CreateElement("egovbga", "RequestedAttribute", " ");
            requestedAttribute3.IsEmpty = false;
            requestedAttribute3.SetAttribute("FriendlyName", "email");
            requestedAttribute3.SetAttribute("Name", "urn:egov:bg:eauth:2.0:attributes:email");
            requestedAttribute3.SetAttribute("NameFormat", "urn:oasis:names:tc:saml2:2.0:attrname-format:uri");
            requestedAttribute3.SetAttribute("isRequired", "false");
            XmlElement val3 = doc.CreateElement("egovbga", "AttributeValue", " ");
            val3.InnerText = "urn:egov:bg:eauth:2.0:attributes:email";
            requestedAttribute3.AppendChild(val3);

            XmlElement requestedAttribute4 = doc.CreateElement("egovbga", "RequestedAttribute", " ");
            requestedAttribute4.IsEmpty = false;
            requestedAttribute4.SetAttribute("FriendlyName", "canonicalResidenceAddress");
            requestedAttribute4.SetAttribute("Name", "urn:egov:bg:eauth:2.0:attributes:canonicalResidenceAddress");
            requestedAttribute4.SetAttribute("NameFormat", "urn:oasis:names:tc:saml2:2.0:attrname-format:uri");
            requestedAttribute4.SetAttribute("isRequired", "false");
            XmlElement val4 = doc.CreateElement("egovbga", "AttributeValue", " ");
            val4.InnerText = "urn:egov:bg:eauth:2.0:attributes:canonicalResidenceAddress";
            requestedAttribute4.AppendChild(val4);



            requestedAttributes.AppendChild(requestedAttribute1);
            requestedAttributes.AppendChild(requestedAttribute2);
            requestedAttributes.AppendChild(requestedAttribute3);
            requestedAttributes.AppendChild(requestedAttribute4);

            requestedService.AppendChild(chldService);
            requestedService.AppendChild(chldProvider);
            requestedService.AppendChild(chldLevelOfAssurance);

            extchld.AppendChild(requestedService);
            extchld.AppendChild(requestedAttributes);
            element1.AppendChild(extchld);

            doc.AppendChild(element1);

            return doc;
        }

        internal EGovAuthUser GetResultData(string rawSamlData)
        {

            //TODO: проверка на резултата с техния сертификат
            //var signatureElement2 = xmlDoc2.GetElementsByTagName("ds:Signature");
            //var signedXml12 = new SignedXml(xmlDoc2);
            //signedXml12.LoadXml((XmlElement)signatureElement2[0]);
            //bool isValid1 = signedXml12.CheckSignature(certagain.GetRSAPublicKey());



            byte[] data = Convert.FromBase64String(rawSamlData);
            string decodedString = Encoding.UTF8.GetString(data);
           
            XmlDocument xml = new XmlDocument();
             xml.LoadXml(decodedString);
          
            XmlElement root = xml.DocumentElement;
            var nodeList = xml.GetElementsByTagName("saml2:AttributeValue");

            EGovAuthUser user = new EGovAuthUser();

            foreach (XmlNode selectedNode in nodeList)
            {

                var attribute = selectedNode.ParentNode.Attributes.GetNamedItem("Name");
                var attributeValue = selectedNode.InnerText;

                switch (attribute.Value) {
                    case "urn:egov:bg:eauth:2.0:attributes:personIdentifier":                     
                        user.PersonalIndentifier = attributeValue;
                        break;
                    case "urn:egov:bg:eauth:2.0:attributes:personName":                       
                        user.PersonName = attributeValue;
                        break;
                    case "urn:egov:bg:eauth:2.0:attributes:latinName":
                        user.LatinName = attributeValue;
                        break;
                    case "urn:egov:bg:eauth:2.0:attributes:birthName":
                        user.BirthName = attributeValue;
                        break;
                    case "urn:egov:bg:eauth:2.0:attributes:email":
                        user.Email = attributeValue;
                        break;
                    case "urn:egov:bg:eauth:2.0:attributes:phone":
                        user.Phone = attributeValue;
                        break;
                    case "urn:egov:bg:eauth:2.0:attributes:gender":
                        user.Gender = attributeValue;
                        break;
                    case "urn:egov:bg:eauth:2.0:attributes:dateOfBirth":
                        user.DateOfBirth = attributeValue;
                        break;
                    case "urn:egov:bg:eauth:2.0:attributes:placeOfBirth":
                        user.PlaceOfBirth = attributeValue;
                        break;
                    case "urn:egov:bg:eauth:2.0:attributes:canonicalResidenceAddress":
                        user.CanonicalResidenceAddress = attributeValue;
                        break;
                        
                    default:
                        break;
                }
                
                
            }

            return user;
        }

        private string GenerateXMLId()
        {
            char[] charMapping = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p' };
            Random random = new Random();

            byte[] bytes = new byte[20]; // 160 bits
            random.NextBytes(bytes);

            char[] chars = new char[40];

            for (int i = 0; i < bytes.Length; i++)
            {
                int left = (bytes[i] >> 4) & 0x0f;
                int right = bytes[i] & 0x0f;
                chars[i * 2] = charMapping[left];
                chars[i * 2 + 1] = charMapping[right];
            }

            string s = new string(chars);

            return s;
        }
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="RequestDataISCIPR", Namespace="http://iscipr.egov.bg")]
    [System.SerializableAttribute()]
    public partial class RequestDataISCIPR : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ArgumentField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.CallContext CallContextField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CitizenEGNField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmployeeEGNField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string OperationField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Argument {
            get {
                return this.ArgumentField;
            }
            set {
                if ((object.ReferenceEquals(this.ArgumentField, value) != true)) {
                    this.ArgumentField = value;
                    this.RaisePropertyChanged("Argument");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.CallContext CallContext {
            get {
                return this.CallContextField;
            }
            set {
                if ((object.ReferenceEquals(this.CallContextField, value) != true)) {
                    this.CallContextField = value;
                    this.RaisePropertyChanged("CallContext");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CitizenEGN {
            get {
                return this.CitizenEGNField;
            }
            set {
                if ((object.ReferenceEquals(this.CitizenEGNField, value) != true)) {
                    this.CitizenEGNField = value;
                    this.RaisePropertyChanged("CitizenEGN");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EmployeeEGN {
            get {
                return this.EmployeeEGNField;
            }
            set {
                if ((object.ReferenceEquals(this.EmployeeEGNField, value) != true)) {
                    this.EmployeeEGNField = value;
                    this.RaisePropertyChanged("EmployeeEGN");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Operation {
            get {
                return this.OperationField;
            }
            set {
                if ((object.ReferenceEquals(this.OperationField, value) != true)) {
                    this.OperationField = value;
                    this.RaisePropertyChanged("Operation");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CallContext", Namespace="http://schemas.datacontract.org/2004/07/SmartRegistry.WebApi")]
    [System.SerializableAttribute()]
    public partial class CallContext : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AdministrationNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AdministrationOIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmployeeAdditionalIdentifierField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmployeeIdentifierField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmployeeNamesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmployeePositionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LawReasonField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RemarkField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ServiceTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ServiceURIField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AdministrationName {
            get {
                return this.AdministrationNameField;
            }
            set {
                if ((object.ReferenceEquals(this.AdministrationNameField, value) != true)) {
                    this.AdministrationNameField = value;
                    this.RaisePropertyChanged("AdministrationName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AdministrationOId {
            get {
                return this.AdministrationOIdField;
            }
            set {
                if ((object.ReferenceEquals(this.AdministrationOIdField, value) != true)) {
                    this.AdministrationOIdField = value;
                    this.RaisePropertyChanged("AdministrationOId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EmployeeAdditionalIdentifier {
            get {
                return this.EmployeeAdditionalIdentifierField;
            }
            set {
                if ((object.ReferenceEquals(this.EmployeeAdditionalIdentifierField, value) != true)) {
                    this.EmployeeAdditionalIdentifierField = value;
                    this.RaisePropertyChanged("EmployeeAdditionalIdentifier");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EmployeeIdentifier {
            get {
                return this.EmployeeIdentifierField;
            }
            set {
                if ((object.ReferenceEquals(this.EmployeeIdentifierField, value) != true)) {
                    this.EmployeeIdentifierField = value;
                    this.RaisePropertyChanged("EmployeeIdentifier");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EmployeeNames {
            get {
                return this.EmployeeNamesField;
            }
            set {
                if ((object.ReferenceEquals(this.EmployeeNamesField, value) != true)) {
                    this.EmployeeNamesField = value;
                    this.RaisePropertyChanged("EmployeeNames");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EmployeePosition {
            get {
                return this.EmployeePositionField;
            }
            set {
                if ((object.ReferenceEquals(this.EmployeePositionField, value) != true)) {
                    this.EmployeePositionField = value;
                    this.RaisePropertyChanged("EmployeePosition");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LawReason {
            get {
                return this.LawReasonField;
            }
            set {
                if ((object.ReferenceEquals(this.LawReasonField, value) != true)) {
                    this.LawReasonField = value;
                    this.RaisePropertyChanged("LawReason");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Remark {
            get {
                return this.RemarkField;
            }
            set {
                if ((object.ReferenceEquals(this.RemarkField, value) != true)) {
                    this.RemarkField = value;
                    this.RaisePropertyChanged("Remark");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ServiceType {
            get {
                return this.ServiceTypeField;
            }
            set {
                if ((object.ReferenceEquals(this.ServiceTypeField, value) != true)) {
                    this.ServiceTypeField = value;
                    this.RaisePropertyChanged("ServiceType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ServiceURI {
            get {
                return this.ServiceURIField;
            }
            set {
                if ((object.ReferenceEquals(this.ServiceURIField, value) != true)) {
                    this.ServiceURIField = value;
                    this.RaisePropertyChanged("ServiceURI");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceResultISCIPR", Namespace="http://schemas.datacontract.org/2004/07/SmartRegistry.WebApi")]
    [System.SerializableAttribute()]
    public partial class ServiceResultISCIPR : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ErrorCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ErrorMessageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool HasErrorField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Data {
            get {
                return this.DataField;
            }
            set {
                if ((object.ReferenceEquals(this.DataField, value) != true)) {
                    this.DataField = value;
                    this.RaisePropertyChanged("Data");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ErrorCode {
            get {
                return this.ErrorCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.ErrorCodeField, value) != true)) {
                    this.ErrorCodeField = value;
                    this.RaisePropertyChanged("ErrorCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ErrorMessage {
            get {
                return this.ErrorMessageField;
            }
            set {
                if ((object.ReferenceEquals(this.ErrorMessageField, value) != true)) {
                    this.ErrorMessageField = value;
                    this.RaisePropertyChanged("ErrorMessage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool HasError {
            get {
                return this.HasErrorField;
            }
            set {
                if ((this.HasErrorField.Equals(value) != true)) {
                    this.HasErrorField = value;
                    this.RaisePropertyChanged("HasError");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://iscipr.egov.bg/", ConfigurationName="RegisterManagementServiceReference.IRegisterManagementService")]
    public interface IRegisterManagementService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://iscipr.egov.bg/IRegisterManagementService/RegisterRecordEntry", ReplyAction="http://iscipr.egov.bg/IRegisterManagementService/RegisterRecordEntryResponse")]
        RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.ServiceResultISCIPR RegisterRecordEntry(RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.RequestDataISCIPR requestData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://iscipr.egov.bg/IRegisterManagementService/RegisterRecordEntry", ReplyAction="http://iscipr.egov.bg/IRegisterManagementService/RegisterRecordEntryResponse")]
        System.Threading.Tasks.Task<RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.ServiceResultISCIPR> RegisterRecordEntryAsync(RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.RequestDataISCIPR requestData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://iscipr.egov.bg/IRegisterManagementService/RegisterRecordChange", ReplyAction="http://iscipr.egov.bg/IRegisterManagementService/RegisterRecordChangeResponse")]
        RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.ServiceResultISCIPR RegisterRecordChange(RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.RequestDataISCIPR requestData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://iscipr.egov.bg/IRegisterManagementService/RegisterRecordChange", ReplyAction="http://iscipr.egov.bg/IRegisterManagementService/RegisterRecordChangeResponse")]
        System.Threading.Tasks.Task<RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.ServiceResultISCIPR> RegisterRecordChangeAsync(RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.RequestDataISCIPR requestData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://iscipr.egov.bg/IRegisterManagementService/RegisterRecordRemove", ReplyAction="http://iscipr.egov.bg/IRegisterManagementService/RegisterRecordRemoveResponse")]
        RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.ServiceResultISCIPR RegisterRecordRemove(RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.RequestDataISCIPR requestData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://iscipr.egov.bg/IRegisterManagementService/RegisterRecordRemove", ReplyAction="http://iscipr.egov.bg/IRegisterManagementService/RegisterRecordRemoveResponse")]
        System.Threading.Tasks.Task<RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.ServiceResultISCIPR> RegisterRecordRemoveAsync(RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.RequestDataISCIPR requestData);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IRegisterManagementServiceChannel : RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.IRegisterManagementService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RegisterManagementServiceClient : System.ServiceModel.ClientBase<RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.IRegisterManagementService>, RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.IRegisterManagementService {
        
        public RegisterManagementServiceClient() {
        }
        
        public RegisterManagementServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public RegisterManagementServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RegisterManagementServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RegisterManagementServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.ServiceResultISCIPR RegisterRecordEntry(RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.RequestDataISCIPR requestData) {
            return base.Channel.RegisterRecordEntry(requestData);
        }
        
        public System.Threading.Tasks.Task<RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.ServiceResultISCIPR> RegisterRecordEntryAsync(RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.RequestDataISCIPR requestData) {
            return base.Channel.RegisterRecordEntryAsync(requestData);
        }
        
        public RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.ServiceResultISCIPR RegisterRecordChange(RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.RequestDataISCIPR requestData) {
            return base.Channel.RegisterRecordChange(requestData);
        }
        
        public System.Threading.Tasks.Task<RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.ServiceResultISCIPR> RegisterRecordChangeAsync(RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.RequestDataISCIPR requestData) {
            return base.Channel.RegisterRecordChangeAsync(requestData);
        }
        
        public RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.ServiceResultISCIPR RegisterRecordRemove(RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.RequestDataISCIPR requestData) {
            return base.Channel.RegisterRecordRemove(requestData);
        }
        
        public System.Threading.Tasks.Task<RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.ServiceResultISCIPR> RegisterRecordRemoveAsync(RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference.RequestDataISCIPR requestData) {
            return base.Channel.RegisterRecordRemoveAsync(requestData);
        }
    }
}

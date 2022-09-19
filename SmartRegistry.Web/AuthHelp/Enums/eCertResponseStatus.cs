namespace SmartRegistry.Web.AuthHelp.Enums
{
    public enum eCertResponseStatus
    {
        InvalidResponseXML,
        InvalidSignature,
        AuthenticationFailed,
        Success,
        MissingEGN,
        CanceledByUser,
    }
}
namespace SmartRegistry.Web.Models.ControlModels
{
    public class PageControl
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public bool Required { get; set; }
    }
}

namespace HttpServApp.Models
{
  internal class TagTemplate
  {
    public long IdTag { get; set; }
    public string TagSource { get; set; } = string.Empty;
    public string HtmlSource { get; set; } = string.Empty;
  }
}

namespace SmartLocation.Security;

public class ApiKeyOptions
{
    public string HeaderName { get; set; } = "X-API-KEY";
    public string Value { get; set; } = "";
}
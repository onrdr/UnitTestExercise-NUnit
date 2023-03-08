namespace JobApplicationLibrary.Services.Abstract
{
    public interface IIdentityValidator
    {
        string Country { get; set; }
        bool IsValid(string identityNumber);
    }
}
using JobApplicationLibrary.Services.Abstract;

namespace JobApplicationLibrary.Services.Concrete;

public class IdentityValidator : IIdentityValidator
{
    public string Country { get; set; }
    public bool IsValid(string identityNumber)
    {
        return true;
    }
}

using JobApplicationLibrary.Models;
using JobApplicationLibrary.Services.Abstract;
using JobApplicationLibrary.Services.Concrete;

namespace JobApplicationLibrary;

public class ApplicationEvaluator
{
    private const int minAge = 18;
    private const int autoAcceptedYearOfExperience = 15;
    private readonly List<string> techStackList = new() { "C#", "RabbitMQ", "Microservice", "Visual Studio" };
    private readonly IIdentityValidator _identityValidator;

    public ApplicationEvaluator(IIdentityValidator validator)
    {
        _identityValidator = validator;
    } 

    public ApplicationResult Evaluate(JobApplication form)
    {
        if (form.Applicant.Age < minAge)
            return ApplicationResult.AutoRejected;

        var validIdentity = _identityValidator.IsValid(form.Applicant.IdentityNumber);

        if(_identityValidator.Country != "Turkey")
            return ApplicationResult.TransferredToCTO;

        if(!validIdentity)
            return ApplicationResult.TransferredToHR;

        var similarityRate = GetTechStackSimilarityRate(form.TechStackList);
        if(similarityRate < 75)
            return ApplicationResult.AutoRejected;

        if(similarityRate >= 75 && form.YearsOfExperience > autoAcceptedYearOfExperience)
            return ApplicationResult.AutoAccepted;

        return ApplicationResult.AutoAccepted;
    }

    private int GetTechStackSimilarityRate(List<string> techStacks)
    {
        var matchedCount = techStacks
            .Where(x => techStackList.Contains(x, StringComparer.OrdinalIgnoreCase))
            .Count();
        var divisionResult = (double)matchedCount / techStackList.Count;
        var rateResult =  (int)(divisionResult * 100);
        return rateResult;
    }
}

public enum ApplicationResult
{
    AutoRejected,
    TransferredToHR,
    TransferredToLead,
    TransferredToCTO,
    AutoAccepted
}

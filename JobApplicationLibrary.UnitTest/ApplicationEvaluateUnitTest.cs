using JobApplicationLibrary.Models;
using JobApplicationLibrary.Services.Abstract;
using Moq;

namespace JobApplicationLibrary.UnitTest;

public class ApplicationEvaluateUnitTest
{
    // Naming Convention => UnitOfWork_Condition_ExpectedResult
    [Test]
    public void Application_WithUnderAge_TransferredToAutoRejected()
    {
        // Arrange
        var evaluator = new ApplicationEvaluator(null);
        var form = new JobApplication()
        {
            Applicant = new Applicant() { Age = 17 }
        };

        // Action
        var appResult = evaluator.Evaluate(form);

        // Assert
        Assert.That(appResult, Is.EqualTo(ApplicationResult.AutoRejected));
    }

    [Test]
    public void Application_WithNoTechStack_TransferredToAutoRejected()
    {
        // Arrange
        var mockValidator = new Mock<IIdentityValidator>();
        mockValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(true);

        var evaluator = new ApplicationEvaluator(mockValidator.Object);
        var form = new JobApplication()
        {
            Applicant = new Applicant() { Age = 19},
            TechStackList = new() { "" }
        };

        // Action
        var appResult = evaluator.Evaluate(form);

        // Assert
        Assert.That(appResult, Is.EqualTo(ApplicationResult.AutoRejected));
    }

    [Test]
    public void Application_WithTechStackOver75Percent_TransferredToAutoAccepted()
    {
        // Arrange
        var mockValidator = new Mock<IIdentityValidator>();
        mockValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(true);

        var evaluator = new ApplicationEvaluator(mockValidator.Object); 
        var form = new JobApplication()
        {
            Applicant = new Applicant() { Age = 19 },
            TechStackList = new() { "C#", "RabbitMQ", "Microservice", "Visual Studio" },
            YearsOfExperience = 16
        };

        // Action
        var appResult = evaluator.Evaluate(form);

        // Assert
        Assert.That(appResult, Is.EqualTo(ApplicationResult.AutoAccepted));
    }

    [Test]
    public void Application_WithInvalidIdentityNumber_TransferredToHR()
    {
        // Arrange
        var mockValidator = new Mock<IIdentityValidator>();
        mockValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(false);

        var evaluator = new ApplicationEvaluator(mockValidator.Object);
        var form = new JobApplication()
        {
            Applicant = new Applicant() { Age = 19 }
        };

        // Action
        var appResult = evaluator.Evaluate(form);

        // Assert
        Assert.That(appResult, Is.EqualTo(ApplicationResult.TransferredToHR));
    }

    [Test]
    public void Application_WithOfficeLocation_TransferredToCTO()
    {
        // Arrange
        var mockValidator = new Mock<IIdentityValidator>();
        mockValidator.Setup(i => i.Country).Returns("Vilnius");

        var evaluator = new ApplicationEvaluator(mockValidator.Object);
        var form = new JobApplication()
        {
            Applicant = new Applicant() { Age = 19 },
            OfficeLocation = "Vilnius"
        };

        // Action
        var appResult = evaluator.Evaluate(form);

        // Assert
        Assert.That(appResult, Is.EqualTo(ApplicationResult.TransferredToCTO));
    }
}
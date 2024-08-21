using JobApplicationLibrary.Model;
using NUnit.Framework;
using Moq;
using JobApplicationLibrary.Services;
using System.Net.Http.Headers;
using FluentAssertions;
using System;
namespace JobApplicationLibrary.UnitTest
{
    public class ApplicationEvaluateUnitTest
    {

        //unitOfWork_Condition_ExceptedResult
        //Condition_Result


        [Test]
        public void Application_WithUnderAge_TransferredToAutoRejected()
        {
            //Arrange
            var evaluator = new ApplicationEvaluator(null);
            var form = new JobApplication()
            {
                Applicant = new Applicant()
                {
                    Age = 17
                }
            };
            //Action
            var appResult=evaluator.Evaluate(form);


            //Assert
            //Assert.AreEqual(ApplicationResult.AutoRejected, appResult);
            appResult.Should().Be(ApplicationResult.AutoRejected);


        }


        [Test]
        public void Application_WithNoTechStack_TransferredToAutoRejected()
        {


            //Arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.DefaultValue = DefaultValue.Mock;

            mockValidator.DefaultValue = DefaultValue.Mock;
            mockValidator.Setup(i => i.CountryDataProvider.ContryData.Country).Returns("TURKEY");

            mockValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(true);


            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age=19},
                TechStackList=new System.Collections.Generic.List<string>() { "" }
                    
            };
            //Action
            var appResult = evaluator.Evaluate(form);


            //Assert
            //Assert.AreEqual( ApplicationResult.AutoRejected,appResult);
            appResult.Should().Be(ApplicationResult.AutoRejected);


        }



        [Test]
        public void Application_WithTechStackOver75_TransferredToAutoAccepted()
        {
            //Arrange

            var moqValidator = new Mock<IIdentityValidator>();
            moqValidator.DefaultValue = DefaultValue.Mock;
            moqValidator.Setup(i => i.CountryDataProvider.ContryData.Country).Returns("TURKEY");
            moqValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(true);

            var evaluator = new ApplicationEvaluator(moqValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age=19},
                TechStackList = new System.Collections.Generic.List<string>() { "C#", "Visual Studio", "Microservice", "Asp.net" },
                YearsOfExperience = 17
                

            };
            //Action
            var appResult = evaluator.Evaluate(form);


            //Assert
            //Assert.AreEqual(ApplicationResult.AutoAccepted, appResult);
            appResult.Should().Be(ApplicationResult.AutoAccepted);


        }


        [Test]
        public void Application_WithInValidIdentityNumber_TransferredToHR()
        {
            //Arrange

            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.DefaultValue = DefaultValue.Mock;
            mockValidator.Setup(i => i.CountryDataProvider.ContryData.Country).Returns("TURKEY");


            mockValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(false);

            //var moqValidato1r = new Mock<IIdentityValidator>(MockBehavior.Strict);
            //moqValidato1r.Setup(i => i.IsValid(It.IsAny<string>())).Returns(false);
            //moqValidato1r.Setup(i => i.CheckConnectionToRemoteServer()).Returns(false);



            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 19 }
            };
            //Action
            var appResult = evaluator.Evaluate(form);


            //Assert
            //Assert.AreEqual( ApplicationResult.TransferredToHR,appResult);
            appResult.Should().Be(ApplicationResult.TransferredToHR);




        }




        [Test]
        public void Application_WithOfficeLocation_TransferredToCTO()
        {
            //Arrange

            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.DefaultValue = DefaultValue.Mock;

            mockValidator.Setup(i => i.CountryDataProvider.ContryData.Country).Returns("SPAIN");

            //var mockCountryData = new Mock<ICountryData>();
            //mockCountryData.Setup(i => i.Country).Returns("SPAIN");
            //var mockProvider =new Mock<ICountryDataProvider>();
            //mockProvider.Setup(i=>i.ContryData).Returns(mockCountryData.Object);
            //mockValidator.Setup(i => i.CountryDataProvider).Returns(mockProvider.Object);


            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 19 }
            };
            //Action
            var appResult = evaluator.Evaluate(form);


            //Assert
            //Assert.AreEqual(ApplicationResult.TransferredToCTO, appResult);
            appResult.Should().Be(ApplicationResult.TransferredToCTO);

        }

        [Test]
        public void Application_WithOver50_ValidationToDetailed()
        {
            //Arrange

            var mockValidator = new Mock<IIdentityValidator>();

            //mockValidator.SetupProperty(i => i.ValidationMode);
            mockValidator.SetupAllProperties();
            mockValidator.Setup(i => i.CountryDataProvider.ContryData.Country).Returns("SPAIN");

            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 51 } 
            };
            //Action
            var appResult = evaluator.Evaluate(form);

            //Assert
            //Assert.AreEqual(ValidationMode.Detailed, mockValidator.Object.ValidationMode);
            mockValidator.Object.ValidationMode.Should().Be(ValidationMode.Detailed);

        }
        [Test]

        public void Application_WithNullApplication_ThrowsArgumentNullException()
        {
            //Arrange

            var mockValidator = new Mock<IIdentityValidator>();


            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication();

            //Action
            Action appResult = () => evaluator.Evaluate(form).Should() ;

            //Assert

            appResult.Should().Throw<ArgumentNullException>();
            //mockValidator.Object.ValidationMode.Should().Be(ValidationMode.Detailed);
        }

        [Test]

        public void Application_WithDefaultValue_IsValidCalled()
        {

            //Arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.DefaultValue = DefaultValue.Mock;

            mockValidator.Setup(i => i.CountryDataProvider.ContryData.Country).Returns("TURKEY");



            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 19 ,IdentityNumber="1234"}, 

            };
            //Action
            var appResult = evaluator.Evaluate(form);


            //Assert
            mockValidator.Verify(i=>i.IsValid(It.IsAny<string>()), Times.Exactly(1));
        }

        [Test]

        public void Application_WithYoungAge_IsValidNeverCalled()
        {

            //Arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.DefaultValue = DefaultValue.Mock;

            mockValidator.Setup(i => i.CountryDataProvider.ContryData.Country).Returns("TURKEY");



            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 15},

            };
            //Action
            var appResult = evaluator.Evaluate(form);


            //Assert
            mockValidator.Verify(i => i.IsValid("123"), Times.Never);
        }
    }
}

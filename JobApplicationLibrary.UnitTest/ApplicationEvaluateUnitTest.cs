using JobApplicationLibrary.Model;
using NUnit.Framework;
using Moq;
using JobApplicationLibrary.Services;
using System.Net.Http.Headers;

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
            Assert.AreEqual(appResult, ApplicationResult.AutoRejected);
          
        }



        [Test]
        public void Application_WithNoTechStack_TransferredToAutoRejected()
        {


            //Arrange
            var moqValidator = new Mock<IIdentityValidator>();
            moqValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(true);

            var evaluator = new ApplicationEvaluator(moqValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age=19},
                TechStackList=new System.Collections.Generic.List<string>() { "" }
                    
            };
            //Action
            var appResult = evaluator.Evaluate(form);


            //Assert
            Assert.AreEqual(appResult, ApplicationResult.AutoRejected);

        }

      

         [Test]
        public void Application_WithTechStackOver75_TransferredToAutoAccepted()
        {
            //Arrange

            var moqValidator = new Mock<IIdentityValidator>();
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
            Assert.AreEqual(appResult, ApplicationResult.AutoAccepted);

        }


        [Test]
        public void Application_WithInValidIdentityNumber_TransferredToHR()
        {
            //Arrange

            var moqValidator = new Mock<IIdentityValidator>(MockBehavior.Loose);
             moqValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(false);

            //var moqValidato1r = new Mock<IIdentityValidator>(MockBehavior.Strict);
            //moqValidato1r.Setup(i => i.IsValid(It.IsAny<string>())).Returns(false);
            //moqValidato1r.Setup(i => i.CheckConnectionToRemoteServer()).Returns(false);



            var evaluator = new ApplicationEvaluator(moqValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age = 19 }
            };
            //Action
            var appResult = evaluator.Evaluate(form);


            //Assert
            Assert.AreEqual(appResult, ApplicationResult.TransferredToHR);

        }
    }
}

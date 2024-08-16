using JobApplicationLibrary.Model;
using NUnit.Framework;

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
            var evaluator = new ApplicationEvaluator();
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
            var evaluator = new ApplicationEvaluator();
            var form = new JobApplication()
            {
                Applicant = new Applicant() { Age=19},
                TechStackList=new System.Collections.Generic.List<string>() { ""}
                    
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
            var evaluator = new ApplicationEvaluator();
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
    }
}

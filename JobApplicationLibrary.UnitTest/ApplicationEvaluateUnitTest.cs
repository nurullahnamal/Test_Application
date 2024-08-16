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
    }
}
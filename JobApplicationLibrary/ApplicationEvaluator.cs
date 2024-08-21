using JobApplicationLibrary.Model;
using JobApplicationLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplicationLibrary
{
    public class ApplicationEvaluator
    {
        private const int minAge = 18;
        private const int AutoAcceptedYearsOfExperience = 11;
        private readonly IIdentityValidator identityValidator;
        private List<string> techStackList = new() { "C#", "Visual Studio", "Microservice", "Asp.net" };

        public ApplicationEvaluator(IIdentityValidator identityValidator)
        {
            this.identityValidator = identityValidator;
        }


        public ApplicationResult Evaluate (JobApplication form)
        {
            if (form.Applicant is null)
                throw new ArgumentNullException();
             
            if (form.Applicant.Age<minAge)
                return ApplicationResult.AutoRejected;

            identityValidator.ValidationMode =form.Applicant.Age>50 ? ValidationMode.Detailed : ValidationMode.Quick;
           
            if (identityValidator.CountryDataProvider.ContryData.Country !="TURKEY")
                return ApplicationResult.TransferredToCTO;

            var validIdentity =  identityValidator.IsValid(form.Applicant.IdentityNumber);


            if (!validIdentity) 
                return ApplicationResult.TransferredToHR;
            
            var sr = GetTechStackSimilarityRate(form.TechStackList);

            if (sr<25)
                return ApplicationResult.AutoRejected;
      
            if (sr < 75 && form.YearsOfExperience>AutoAcceptedYearsOfExperience)
                return ApplicationResult.AutoRejected;

            return ApplicationResult.AutoAccepted;
        }



        private int GetTechStackSimilarityRate(List<string> techStacks)
        {
            var matchedCount =
                techStacks
                .Where(i => techStackList.Contains(i, StringComparer.OrdinalIgnoreCase))
                .Count();
            return (int) (double)matchedCount/techStackList.Count()*100;
        }
    }



    public enum ApplicationResult
    {
        AutoRejected,
        TransferredToHR,
        TransferredTolead,
        TransferredToCTO,
        AutoAccepted
    }
}

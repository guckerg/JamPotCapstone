using Microsoft.AspNetCore.Mvc;
using JampotCapstone.Models;

namespace JampotCapstone.Data
{
    public interface IApplicationRepository
    {
        //Applications

        public IQueryable<Application> GetApplicationsQuery();

        public Task<Application> GetApplicationByIdAsync(int id);

        public Task AddApplicationAsync(Application model);

        //Might not be a reason to ever intentionally modify a user's application
        //public Task UpdateApplicationAsync(Application model);

        public int DeleteApplication(int ApplicationID);
    }
}

using JampotCapstone.Models;

namespace JampotCapstone.Data.Interfaces
{
    public interface IApplicationRepository
    {
        //Applications

        public IQueryable<Application> GetApplicationsQuery();

        public Task<List<Application>> GetAllApplicationsAsync();

        public Task<Application> GetApplicationByIdAsync(int id);

        public Task AddApplicationAsync(Application model);

        public int DeleteApplication(int ApplicationID);
    }
}

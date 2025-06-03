
namespace JampotUnitTests
{
    class FakeApplicationRepository : IApplicationRepository
    {
        public readonly List<Application> applications = [];

        public Task AddApplicationAsync(Application model)
        {
            applications.Add(model);
            return Task.CompletedTask;
        }

        public int DeleteApplication(int ApplicationID)
        {
            var application = applications.FirstOrDefault(a => a.ApplicationID == ApplicationID);
            if (application != null)
            {
                applications.Remove(application);
                return 1;
            }

            return 0; //0 is fail, 1 is success
        }

        public Task<Application> GetApplicationByIdAsync(int id)
        {
            var application = applications.FirstOrDefault(a => a.ApplicationID == id);
            return Task.FromResult(application);
        }


        public IQueryable<Application> GetApplicationsQuery()
        {
            return applications.AsQueryable();
        }

        Task<List<Application>> IApplicationRepository.GetAllApplicationsAsync()
        {
            throw new NotImplementedException();
        }
    }
}

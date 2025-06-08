using JampotCapstone.Data.Interfaces;
using JampotCapstone.Models;
using Microsoft.EntityFrameworkCore;


namespace JampotCapstone.Data
{
    public class ApplicationRepository: IApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationRepository(ApplicationDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public IQueryable<Application> GetApplicationsQuery()
        {
            return _context.Applications.Include(application => application.Name);
        }

        public async Task<List<Application>> GetAllApplicationsAsync()
        {
            List<Application> applications = await _context.Applications
                .Include(a => a.JobTitle).ToListAsync();
            return applications;
        }

        public async Task<Application> GetApplicationByIdAsync(int id)
        {
            var application = await _context.Applications.Include(application => application.Name)
                .Where(application => application.ApplicationID == id).SingleOrDefaultAsync();

            if (application == null)
            {
                throw new Exception($"Application with ID: {id} not found");
            }

            return application;
        }

        public async Task AddApplicationAsync(Application model)
        {
            _context.Applications.Add(model);
            await _context.SaveChangesAsync();
        }

        public int DeleteApplication(int ApplicationID)
        {
            var targetApplication = _context.Applications.Find(ApplicationID);
            _context.Applications.Remove(targetApplication);
            return _context.SaveChanges();
        }
    }
}

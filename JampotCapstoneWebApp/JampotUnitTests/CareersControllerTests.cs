using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using JampotCapstone.Controllers;
using JampotCapstone.Data.Interfaces;
using JampotCapstone.Models;
using JampotCapstone.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace JampotUnitTests
{

    public class CareersControllerTests
    {
        private CareersController CreateController(string databaseName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            var context = new ApplicationDbContext(options);

            if (!context.JobTitles.Any())
            {
                context.JobTitles.Add(new JobTitle { JobTitleID = 1, JobTitleName = "Developer" });
                context.SaveChanges();
            }

            var fakeAppRepo = new FakeApplicationRepository();
            var fakeTextRepo = new FakeTextElementRepository();
            var controller = new CareersController(fakeAppRepo, context, fakeTextRepo);

            //outside helper to handle temp data in testing environment
            var httpContext = new DefaultHttpContext();
            httpContext.RequestServices = new ServiceCollection().BuildServiceProvider();
            controller.ControllerContext = new ControllerContext{ HttpContext = httpContext };
            controller.TempData = new TempDataDictionary( httpContext, new FakeTempDataProvider());

            controller.ModelState.Clear();
            return controller;

        }

        [Fact]
        public async Task CreateApplication_InvalidFileExtension()
        {
            var controller = CreateController("Test_InvalidExtension");

            // Create a dummy file with an invalid extension (.jpg).
            var fileContent = "dummy content";
            var bytes = Encoding.UTF8.GetBytes(fileContent);
            var stream = new MemoryStream(bytes);
            var invalidFile = new FormFile(stream, 0, bytes.Length, "ResumeUpload", "test.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            // Build the minimal view model.
            var viewModel = new CareersViewModel
            {
                Application = new Application
                {
                    Name = "Test User",
                    Email = "test@example.com",
                    JobTitleID = 1
                },
                ResumeUpload = invalidFile,
                // The Positions dropdown is built using the seeded JobTitle.
                Positions = new SelectList(new[] { new JobTitle { JobTitleID = 1, JobTitleName = "Developer" } }, "JobTitleID", "JobTitleName")
            };

            // Act: call the CreateApplication action.
            var result = await controller.CreateApplication(viewModel);

            // Assert: ensure the action returns the "Index" view and the ModelState contains the right error.
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", viewResult.ViewName);
            Assert.False(controller.ModelState.IsValid);
        }

        [Fact]
        public async Task CreateApplication_InvalidModelState()
        {
            var controller = CreateController("Test_InvalidModelState");

            controller.ModelState.AddModelError("Application.Name", "Name is required.");

            var fileContent = "dummy content";
            var bytes = Encoding.UTF8.GetBytes(fileContent);
            var stream = new MemoryStream(bytes);
            var validFile = new FormFile(stream, 0, bytes.Length, "ResumeUpload", "resume.pdf")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };

            // Build the view model with invalid data: Application.Name is empty.
            var viewModel = new CareersViewModel
            {
                Application = new Application
                {
                    Name = "",  // Missing required field.
                    Email = "test@example.com",
                    JobTitleID = 1
                },
                ResumeUpload = validFile,
                Positions = new SelectList(new[] { new JobTitle { JobTitleID = 1, 
                    JobTitleName = "Developer" } }, "JobTitleID", "JobTitleName")
            };

            var result = await controller.CreateApplication(viewModel);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", viewResult.ViewName);
            Assert.False(controller.ModelState.IsValid);
            Assert.True(controller.ModelState["Application.Name"].Errors.Any(),
                "Expected an error message for Application.Name when it is empty.");
        }

        [Fact]
        public async Task CreateApplication_ValidSubmission()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("Test_ValidSubmission")
                .Options;
            var context = new ApplicationDbContext(options);

            if (!context.JobTitles.Any())
            {
                context.JobTitles.Add(new JobTitle { JobTitleID = 1, JobTitleName = "Developer" });
                context.SaveChanges();
            }

            var fakeAppRepo = new FakeApplicationRepository();
            var fakeTextRepo = new FakeTextElementRepository();

            var controller = new CareersController(fakeAppRepo, context, fakeTextRepo);
            controller.ModelState.Clear();

            // Create a valid FormFile with an allowed extension (".pdf").
            var fileContent = "dummy pdf content";
            var bytes = Encoding.UTF8.GetBytes(fileContent);
            var stream = new MemoryStream(bytes);
            var validFile = new FormFile(stream, 0, bytes.Length, "ResumeUpload", "resume.pdf")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };

            // Build the view model with valid information.
            var viewModel = new CareersViewModel
            {
                Application = new Application
                {
                    Name = "Jane Doe",
                    Email = "jane.doe@example.com",
                    JobTitleID = 1
                },
                ResumeUpload = validFile,
                // Use the seeded job titles to build the dropdown.
                Positions = new SelectList(context.JobTitles, "JobTitleID", "JobTitleName", 1)
            };

            var result = await controller.CreateApplication(viewModel);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Single(fakeAppRepo.applications);
            var addedApplication = fakeAppRepo.applications.First();
            Assert.Equal("Jane Doe", addedApplication.Name);
            Assert.Equal("jane.doe@example.com", addedApplication.Email);
        }

        [Fact]
        public void DeleteApplication_Success()
        {
            var controller = CreateController("Test_DeleteApplication");

            var fakeRepo = controller.repo as FakeApplicationRepository;
            Assert.NotNull(fakeRepo);

            var application = new Application
            {
                Name = "Delete Test",
                Email = "delete@example.com",
                JobTitleID = 1
            };

            fakeRepo.AddApplicationAsync(application).Wait();
            Assert.Single(fakeRepo.applications);

            var result = controller.DeleteApplication(application.ApplicationID);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            Assert.Empty(fakeRepo.applications);

        }

        
    }
}

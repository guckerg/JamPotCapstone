using Microsoft.AspNetCore.Mvc.Rendering;

namespace JampotCapstone.Models.ViewModels
{
    public class CareersViewModel
    {
        //public string Title { get; set; } = "Careers";
        public SelectList Positions { get; set; } = new SelectList(Enumerable.Empty<string>(), "Id", "JobTitleName");

        public Application Application { get; set; } = new Application();
    }
}

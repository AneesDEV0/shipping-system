using BlazorApp.Models;

namespace BlazorApp.Pages
{
    public partial class Students
    {

        public List<Student> StudentList { get; set; } = new();
        public List<Depatment> DepartmentList { get; set; } = new();
        public int DepartId { get; set; }

        protected override void OnInitialized()
        {
            SeedData seedData = new SeedData();
            StudentList = seedData.GetStudents();
            DepartmentList = seedData.GetDepatments();
        }

        private void GetStudent()
        {
            if (DepartId == 0)
                StudentList = new SeedData().GetStudents();
            else
                StudentList = new SeedData().GetStudents().Where(e => e.DepartmentId == DepartId).ToList();
        }
    }
}

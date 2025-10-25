namespace BlazorApp.Models
{
    public class SeedData
    {
        private static List<Depatment> departments;
        private static List<Student> students;
        public SeedData()
        {
            departments = new List<Depatment>
        {
            new Depatment { Id = 1, Name = "Computer Science" },
            new Depatment { Id = 2, Name = "Mathematics" },
            new Depatment { Id = 3, Name = "Physics" }
        };

            students = new List<Student>
        {
            new Student { Id = 1, Name = "Alice", Address = "123 Main St", DepartmentId = 1 },
            new Student {Id = 4 ,Name = "David", Address = "321 Maple St", DepartmentId = 1 },

            new Student { Id = 2, Name = "Bob", Address = "456 Oak St", DepartmentId = 2 },
            new Student {Id = 5, Name = "Eva", Address = "654 Cedar St", DepartmentId = 2 },

            new Student {Id = 6, Name = "Frank", Address = "987 Birch St", DepartmentId = 3 },
            new Student {Id = 7, Name = "Grace", Address = "789 Spruce St", DepartmentId = 3 },
            new Student { Id = 3, Name = "Charlie", Address = "789 Pine St", DepartmentId = 3 }
        };
        }

        public  List<Depatment> GetDepatments() => departments;
        public  List<Student> GetStudents() => students;
    }
}

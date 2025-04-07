using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectPersonToAnswer.Services
{
    public class StudentService
    {
        
        private static readonly string _classesDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Classes");

        
        public List<ClassGroup> Classes { get; set; } = new List<ClassGroup>();

        public StudentService()
        {
            
            if (!Directory.Exists(_classesDirectory))
            {
                Directory.CreateDirectory(_classesDirectory);
            }
        }

        
        public static async Task<List<ClassGroup>> LoadClassesAsync()
        {
            
            var classFiles = Directory.GetFiles(_classesDirectory, "*.txt");
            var classGroups = new List<ClassGroup>();

            foreach (var file in classFiles)
            {
                var content = await File.ReadAllTextAsync(file);
                var classGroup = DeserializeClasses(content).FirstOrDefault();
                if (classGroup != null)
                {
                    classGroups.Add(classGroup);
                }
            }

            return classGroups;
        }

        
        public async Task SaveClassAsync(ClassGroup classGroup)
        {
            var filePath = Path.Combine(_classesDirectory, $"Class_{classGroup.Name}.txt");
            var content = SerializeClasses(new List<ClassGroup> { classGroup });
            await File.WriteAllTextAsync(filePath, content);
        }

        
        public static Student SelectRandomStudent(ClassGroup classGroup)
        {
            var availableStudents = classGroup.Students.ToList();
            if (availableStudents.Count == 0)
                return null;

            var random = new Random();
            return availableStudents[random.Next(availableStudents.Count)];
        }

        
        private string SerializeClasses(List<ClassGroup> classes)
        {
            var sb = new StringBuilder();
            foreach (var classGroup in classes)
            {
                sb.AppendLine($"Class: {classGroup.Name}");
                foreach (var student in classGroup.Students)
                {
                    sb.AppendLine($"  {student.Name}");
                }
            }
            return sb.ToString();
        }


        
        public static List<ClassGroup> DeserializeClasses(string content)
        {
            var classGroups = new List<ClassGroup>();
            var lines = content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            ClassGroup currentClass = null;

            foreach (var line in lines)
            {
                if (line.StartsWith("Class:"))
                {
                    if (currentClass != null)
                        classGroups.Add(currentClass);

                    currentClass = new ClassGroup(line.Substring(7).Trim());
                }
                else if (line.StartsWith("  "))
                {
                    if (currentClass != null)
                    {
                        var student = new Student(line.Substring(2).Trim());
                        currentClass.Students.Add(student);
                    }
                }
            }

            if (currentClass != null)
                classGroups.Add(currentClass);

            return classGroups;
        }

        
        public void AddClassFromFile(string filePath)
        {
            var content = File.ReadAllText(filePath);
            var loadedClasses = DeserializeClasses(content);

            foreach (var loadedClass in loadedClasses)
            {
                Classes.Add(loadedClass);
                
                SaveClassAsync(loadedClass);
            }
        }

        
        public void DeleteClass(ClassGroup classGroup)
        {
            var filePath = Path.Combine(_classesDirectory, $"Class_{classGroup.Name}.txt");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            
            Classes.Remove(classGroup);
        }
    }
}

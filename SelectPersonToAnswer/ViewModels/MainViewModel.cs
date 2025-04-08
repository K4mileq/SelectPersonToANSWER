using SelectPersonToAnswer.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SelectPersonToAnswer.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ObservableCollection<Student> NewClassStudents { get; set; } = new ObservableCollection<Student>();


        public ObservableCollection<ClassGroup> Classes { get; set; } = new();
        public ClassGroup? SelectedClass { get; set; }
        public Student? DrawnStudent { get; set; }

        public ICommand LoadCommand => new Command(async () => await LoadAsync());
        public ICommand SaveCommand => new Command(async () => await SaveAsync());
        public ICommand DrawCommand => new Command(DrawStudent);

        private async Task LoadAsync()
        {
            Classes.Clear();
            var loaded = await StudentService.LoadClassesAsync();
            foreach (var group in loaded)
                Classes.Add(group);
        }

        public async Task SaveAsync()
        {
            var studentService = new StudentService();

            var classesToSave = Classes.ToList();

            foreach (var classGroup in classesToSave)
            {
                await studentService.SaveClassAsync(classGroup);
            }
        }


        private void DrawStudent()
        {
            if (SelectedClass != null)
            {
                DrawnStudent = StudentService.SelectRandomStudent(SelectedClass);
                OnPropertyChanged(nameof(DrawnStudent));
            }
        }



        public bool IsClassNameUnique(string className)
        {
            return !Classes.Any(c => c.Name.Equals(className, StringComparison.OrdinalIgnoreCase));
        }

    }

}
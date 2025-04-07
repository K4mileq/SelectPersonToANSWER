using SelectPersonToAnswer.ViewModels;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace SelectPersonToAnswer.Views
{
    public partial class AddClassPage : ContentPage
    {
        private MainViewModel viewModel;

        public AddClassPage(MainViewModel vm)
        {
            InitializeComponent();
            viewModel = vm;
            BindingContext = viewModel;
        }

        private void OnAddStudentClicked(object sender, EventArgs e)
        {
            var studentName = newStudentEntry.Text?.Trim();

            if (!string.IsNullOrWhiteSpace(studentName))
            {
                if (!viewModel.NewClassStudents.Any(s => s.Name.Equals(studentName, StringComparison.OrdinalIgnoreCase)))
                {
                    
                    viewModel.NewClassStudents.Add(new Student(studentName));
                    newStudentEntry.Text = string.Empty; 
                }
                else
                {
                    DisplayAlert("B³¹d", "Uczeñ o takim imieniu ju¿ istnieje.", "OK");
                }
            }
        }

        private void OnDeleteStudentClicked(object sender, EventArgs e)
        {
            if ((sender as Button)?.BindingContext is Student student)
            {
                viewModel.NewClassStudents.Remove(student);
            }
        }

        private async void OnSaveClassClicked(object sender, EventArgs e)
        {
            var className = classNameEntry.Text?.Trim();
            if (!string.IsNullOrWhiteSpace(className))
            {
                if (viewModel.IsClassNameUnique(className))
                {
                    var newClass = new ClassGroup(className)
                    {
                        Students = new ObservableCollection<Student>(viewModel.NewClassStudents)
                    };

                    viewModel.Classes.Add(newClass);

                    
                    await viewModel.SaveAsync();  
                    await Navigation.PopAsync();
                }
                else
                {
                    errorLabel.Text = "Ta klasa ju¿ istnieje!";
                    errorLabel.IsVisible = true;
                }
            }
            else
            {
                errorLabel.Text = "WprowadŸ nazwê klasy.";
                errorLabel.IsVisible = true;
            }
        }


    }
}

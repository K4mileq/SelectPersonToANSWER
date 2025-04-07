using SelectPersonToAnswer.ViewModels;
using Microsoft.Maui.Controls;

namespace SelectPersonToAnswer.Views
{
    public partial class EditClassPage : ContentPage
    {
        private MainViewModel viewModel;

        public EditClassPage(MainViewModel vm)
        {
            InitializeComponent();
            viewModel = vm;
            BindingContext = viewModel;
            classNameEntry.Text = viewModel.SelectedClass?.Name;
        }

        private void OnAddClicked(object sender, EventArgs e)
        {
            var newName = newStudentEntry.Text?.Trim();

            if (!string.IsNullOrWhiteSpace(newName))
            {
                if (!viewModel.SelectedClass!.Students.Any(s => s.Name.Equals(newName, StringComparison.OrdinalIgnoreCase)))
                {
                    viewModel.SelectedClass.Students.Add(new Student(newName));
                    newStudentEntry.Text = string.Empty; 
                }
                else
                {
                    DisplayAlert("B³¹d", "Uczeñ o takim imieniu ju¿ istnieje.", "OK");
                }
            }
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            if ((sender as Button)?.BindingContext is Student student)
            {
                viewModel.SelectedClass?.Students.Remove(student);
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            viewModel.SelectedClass!.Name = classNameEntry.Text?.Trim() ?? string.Empty; // Zmieniliœmy 'ClassName' na 'Name'
            viewModel.SaveCommand.Execute(null);  // Zapisz zmiany
            await Navigation.PopAsync();  // Powróæ do poprzedniej strony
        }

    }
}

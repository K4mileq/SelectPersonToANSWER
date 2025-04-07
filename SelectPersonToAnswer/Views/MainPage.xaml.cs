using SelectPersonToAnswer.Services;
using Microsoft.Maui.Controls;
using SelectPersonToAnswer.ViewModels;

namespace SelectPersonToAnswer.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly Random random = new();
        MainViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new MainViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadCommand.Execute(null);
        }

        private async void OnEditClassClicked(object sender, EventArgs e)
        {
            if (viewModel.SelectedClass != null)
            {
                await Navigation.PushAsync(new EditClassPage(viewModel));
            }
        }

        private async void OnAddClassClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddClassPage(viewModel));
        }

        private async void OnDrawStudentClicked(object sender, EventArgs e)
        {
            var students = viewModel.SelectedClass?.Students?.ToList();

            if (students == null || students.Count == 0)
            {
                await DisplayAlert("B³¹d", "Brak uczniów do wylosowania.", "OK");
                return;
            }

            resultLabel.Text = "Losujê...";

            
            for (int i = 0; i < 15; i++)
            {
                var index = random.Next(students.Count);
                var tempName = students[index].Name;

                
                resultLabel.Text = $"Losujê: {tempName}";
                await Task.Delay(100 + i * 10);
            }

            
            var selectedStudent = students[random.Next(students.Count)];
            resultLabel.Text = $"Wylosowano: {selectedStudent.Name}";
        }
    }
}

using SelectPersonToAnswer.Views;

namespace SelectPersonToAnswer
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("AddClassPage", typeof(AddClassPage));
        }
    }
}

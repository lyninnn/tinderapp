using TinderApp.Views;
namespace TinderApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));

        }
    }
}

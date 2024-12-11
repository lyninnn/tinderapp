
using TinderApp.ViewModel;
using TinderApp.Views;

namespace TinderApp
{
    public partial class App : Application
    {
        
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

    }
}
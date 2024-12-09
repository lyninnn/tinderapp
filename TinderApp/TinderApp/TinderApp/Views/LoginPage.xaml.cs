
using TinderApp.ViewModel;
using TinderApp.Model;

namespace TinderApp.Views

{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(TinderDB db)
        {
            InitializeComponent();
            BindingContext = new LoginViewModel(db);
        }

    }
}

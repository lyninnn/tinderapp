
using TinderApp.ViewModel;
using TinderApp.Model;

namespace TinderApp.Views

{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginViewModel loginViewModel)
        {
            InitializeComponent();
           
            BindingContext = loginViewModel;
        }

    }
}

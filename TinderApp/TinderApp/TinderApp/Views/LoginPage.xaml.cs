
using TinderApp.ViewModel;
namespace TinderApp.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginViewModel loginViewModel)
        {
            InitializeComponent();
            BindingContext = loginViewModel;
        }

        private void OnLoginFailed(string errorMessage)
        {
            // Mostrar mensaje de error en caso de que la validación falle
            ErrorLabel.Text = errorMessage;
            ErrorLabel.IsVisible = true;
        }
    }
}

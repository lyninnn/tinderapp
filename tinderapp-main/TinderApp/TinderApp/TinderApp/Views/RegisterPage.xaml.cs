using TinderApp.ViewModel;

namespace TinderApp.Views
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage(RegisterViewModel registerViewModel)
        {
            InitializeComponent();
            BindingContext = registerViewModel;
        }
    }
}

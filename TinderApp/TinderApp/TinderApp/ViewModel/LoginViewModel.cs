using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using TinderApp.Model;

namespace TinderApp.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _nombre;

        [ObservableProperty]
        private TinderDB tinderdb;

        [ObservableProperty]
        private int _edad;

        [ObservableProperty]
        private string _errorMessage;

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLogin);
        }

        private async void OnLogin()
        {
            if (_edad < 18)
            {
                _errorMessage = "Debes ser mayor de 18 años para registrarte.";
                return;
            }


            tinderdb.InsertarUsuario();
            var usuario = new Usuario
            {
                Nombre = Nombre,
                Edad = Edad,
                Genero = Genero,
                Ubicacion = Ubicacion,
                Preferencias = Preferencias,
                Foto = Foto
            };

            // Lógica de inicio de sesión (puedes agregar validaciones adicionales aquí)

            // Si el login es exitoso, navegar a la página principal
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}

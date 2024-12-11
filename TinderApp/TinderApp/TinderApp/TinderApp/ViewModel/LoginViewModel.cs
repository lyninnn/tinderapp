using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TinderApp.Model;
using System.Threading.Tasks;

namespace TinderApp.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly TinderDB database;

        [ObservableProperty]
        private string? nombre;

        [ObservableProperty]
        private string? contraseña;

        [ObservableProperty]
        private Usuario loggedInUser;

        public LoginViewModel(TinderDB tinderdb)
        {
            database = tinderdb;
        }

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Contraseña))
            {
                // Validación básica: asegurarse de que ambos campos no estén vacíos.
                await Shell.Current.DisplayAlert("Error", "Por favor ingresa nombre y contraseña.", "OK");
                return;
            }

            try
            {
                // Verificar las credenciales en la base de datos
                var user = await database.VerificarCredencial(Nombre, Contraseña);

                if (user != null)
                {
                    // Asignar el usuario logueado
                    loggedInUser = user;

                    // Navegar a la página de usuario (o la página principal)
                    await Shell.Current.GoToAsync("UsuarioPage");
                }
                else
                {
                    // Si no se encuentra el usuario
                    await Shell.Current.DisplayAlert("Error", "Usuario o contraseña incorrectos.", "OK");
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones: captura cualquier error y muestra un mensaje al usuario
                await Shell.Current.DisplayAlert("Error", $"Hubo un problema al iniciar sesión: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task Registrar()
        {
            // Navegar a la página de registro
            await Shell.Current.GoToAsync("RegisterPage");
        }
    }
}

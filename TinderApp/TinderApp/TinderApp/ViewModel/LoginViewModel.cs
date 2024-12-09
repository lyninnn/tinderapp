using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TinderApp.Model;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Maui.Storage;

public partial class LoginViewModel : ObservableObject
{
    private readonly TinderDB database;

    [ObservableProperty]
    private string nombre;

    [ObservableProperty]
    private int edad;

    [ObservableProperty]
    private string genero;

    [ObservableProperty]
    private string ubicacion;

    [ObservableProperty]
    private string preferencias;

    [ObservableProperty]
    private string foto;

    [ObservableProperty]
    private string contraseña;


    public LoginViewModel(TinderDB db)
    {
        database = db;
    }

    [RelayCommand]
    public async Task IniciarSesion()
    {
        if (await database.VerificarCredencial(Nombre, Contraseña))
        {
            //await Shell.Current.DisplayAlert("Éxito", "Todo bien!", "OK");
            await Shell.Current.GoToAsync("//UsuariosPage");
            // Redirige al usuario a otra página o realiza la acción correspondiente
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", "usuario no encontrado!", "OK");
        }
    }

    [RelayCommand]
    public async Task RegistrarUsuarioAsync()
    {
       
        try
            { 
            var usuario = new Usuario
            {
                Nombre = Nombre,
                Edad = Edad,
                Genero = Genero,
                Ubicacion = Ubicacion,
                Preferencias = Preferencias,
                Foto = Foto
            };

            await database.InsertarUsuario(usuario);
            await Shell.Current.DisplayAlert("Éxito", "Todo bien!", "OK");
        }
        catch
        {
            await Shell.Current.DisplayAlert("Erro", "Quiza ya existe", "OK");
        }
    }
}

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TinderApp.Model;

namespace TinderApp.ViewModel
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly TinderDB tinderDB;

        public RegisterViewModel(TinderDB tinderdb)
        {
            tinderDB = tinderdb; // Instancia de la base de datos
            Generos = new ObservableCollection<string> { "Masculino", "Femenino", "Otro" };
        }

        // Propiedades para enlazar con la vista
        [ObservableProperty]
        private string? nombre;

        [ObservableProperty]
        private int edad;

        [ObservableProperty]
        private string? genero;

        [ObservableProperty]
        private string? ubicacion;

        [ObservableProperty]
        private string? preferencias;

        [ObservableProperty]
        private string? foto;

        [ObservableProperty]
        private string contraseña;

        public ObservableCollection<string> Generos { get; }

        // Comando para seleccionar foto
        [RelayCommand]
        public async Task SeleccionarFoto()
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                Foto = result.FullPath; // Ruta completa de la foto seleccionada
            }
        }

        // Comando para registrar al usuario
        [RelayCommand]
        public async Task Registrar()
        {
            if (string.IsNullOrWhiteSpace(Nombre) ||
                edad <= 0 ||
                string.IsNullOrWhiteSpace(Genero) ||
                string.IsNullOrWhiteSpace(Ubicacion) ||
                string.IsNullOrWhiteSpace(Preferencias) ||
                string.IsNullOrWhiteSpace(contraseña))
            {
                await Shell.Current.DisplayAlert("Error", "Por favor, complete todos los campos.", "OK");
                return;
            }

            var usuario = new Usuario
            {
                Nombre = Nombre,
                Edad = Edad,
                Genero = Genero,
                Ubicacion = Ubicacion,
                Preferencias = Preferencias,
                Foto = Foto,
                Contraseña = Contraseña
            };

            // Lógica para guardar en la base de datos
            await tinderDB.InsertarUsuario(usuario);

            await Shell.Current.DisplayAlert("Éxito", "Usuario registrado correctamente.", "OK");
            await Shell.Current.GoToAsync(".."); // Regresar a la página anterior
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudKit;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TinderApp.DTOs;
using TinderApp.Model;

namespace TinderApp.ViewModel
{
    [QueryProperty(nameof(UsuarioId), "id")]
    public partial class UsuarioViewModel : ObservableObject
    {

        private readonly TinderDB tinderdb;

        [ObservableProperty]
        private List<Usuario> usuarios;

        [ObservableProperty]
        private UsuariosDTO usuariosDTO;

        [ObservableProperty]
        private string titulo;

        [ObservableProperty]
        private bool loading;

        [ObservableProperty]
        private int usuarioId;

        public UsuarioViewModel(TinderDB tinderDB)
        {
            this.tinderdb = tinderDB;

            usuariosDTO = new UsuariosDTO();
            titulo = "Inicio";
            _ = CargarUsuarios();

        }
        private async Task CargarUsuarios()
        {
            Usuarios = await tinderdb.VerUsuario();
        }
        [RelayCommand]
        public async Task DarLikeAsync(UsuariosDTO usuarioLikeado)
        {
            // Insertar un "like" del usuario actual al usuario objetivo
            await tinderdb.InsertarMatch(new Match
            {
                Usuario1Id = UsuariosDTO.User_id,
                Usuario2Id = usuarioLikeado.User_id,
                FechaMatch = DateTime.Now
            });

            // Obtener todos los matches existentes para comprobar si hay un match mutuo
            var matches = await tinderdb.VerMatch();
            var esMatch = matches.Any(m =>
                m.Usuario1Id == usuarioLikeado.User_id &&
                m.Usuario2Id == UsuariosDTO.User_id
            );

            if (esMatch)
            {
                // Notificar que se ha creado un match mutuo
                await Application.Current.MainPage.DisplayAlert(
                    "¡Match!",
                    $"¡Tienes un match con {usuarioLikeado.Nombre}!",
                    "OK"
                );
            }
            else
            {
                // Confirmación de que el "like" fue registrado
                await Application.Current.MainPage.DisplayAlert(
                    "¡Like enviado!",
                    $"Has dado like a {usuarioLikeado.Nombre}.",
                    "OK"
                );
            }
            
        }
        //[RelayCommand]
        //public async Task GuardarUsuario()
        //{
        //    if (string.IsNullOrEmpty(usuariosDTO.Nombre) || string.IsNullOrEmpty(usuariosDTO.Genero) || string.IsNullOrEmpty(usuariosDTO.Ubicacion) || usuariosDTO.Edad <= 0)
        //    {
        //        await Shell.Current.DisplayAlert("Error", "No has introducido los valores correctamente", "OK");
        //    }

        //    loading = true;

        //    bool success;

        //    Usuario usuario = new Usuario
        //    {
        //        Nombre = usuariosDTO.Nombre,
        //        Genero = usuariosDTO.Genero,
        //        Edad = usuariosDTO.Edad,
        //        Ubicacion = usuariosDTO.Ubicacion,
        //        Preferencias = usuariosDTO.Preferencias
        //    };

        //    if (usuariosDTO.User_id == 0)
        //    {
        //        int idFalse = await tinderdb.InsertarUsuario(usuario);
        //        usuariosDTO.User_id = idFalse;
        //        success = usuariosDTO.User_id > 0;
        //    }
        //    else
        //    {
        //        usuario.UsuarioId = UsuarioId;
        //        await tinderdb.ActualizarUsuario(usuario);
        //        success = true;
        //    }

        //    if (success) 
        //    {
        //        await Shell.Current.DisplayAlert("Éxito", "Todo Bien", "Ok");
        //        await Shell.Current.GoToAsync("..");
        //    }
        //    else
        //    {
        //        await Shell.Current.DisplayAlert("Error", "No se ha podido guardar nada!", "OK");
        //    }

        //    loading = false;

        //}

        partial void OnUsuarioIdChanged(int value)
        {
            if (value != 0)
            {
                CargarUsuario(value);
            }
        }

        public async void CargarUsuario(int value)
        {
            List<Usuario> listado = await tinderdb.VerUsuario();

            if (listado != null)
            {
                Usuario u = listado.Find((x) => x.UsuarioId == value);

                UsuariosDTO usu = new UsuariosDTO
                {
                    User_id = value,
                    Nombre = u.Nombre,
                    Edad = u.Edad,
                    Ubicacion = u.Ubicacion,
                    Genero = u.Genero,
                    Preferencias = u.Preferencias
                };

                usuariosDTO = usu;

                titulo = "Editar Usuario";

            }
        }


    }
}

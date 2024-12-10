using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
        private ObservableCollection<UsuariosDTO> usuariosDTO;

        [ObservableProperty]
        private UsuariosDTO usuarioDTO;

        [ObservableProperty]
        private string titulo;

        [ObservableProperty]
        private bool loading;

        [ObservableProperty]
        private int usuarioId;

        public UsuarioViewModel(TinderDB tinderDB)
        {
            this.tinderdb = tinderDB;
            titulo = "Inicio";
            _ = CargarUsuariosAsync(); // Llamar a CargarUsuariosAsync al crear el ViewModel
        }

        [RelayCommand]
        private async Task CargarUsuariosAsync()
        {
            try
            {
                // Obtener todos los usuarios desde la base de datos
                var usuarios = await tinderdb.VerUsuario();

                // Convertir la lista de usuarios a una colección de DTOs
                var listaUsuariosDTO = usuarios
                    .Select(u => new UsuariosDTO
                    {
                        User_id = u.UsuarioId,
                        Nombre = u.Nombre,
                        Genero = u.Genero,
                        Edad = u.Edad,
                        Ubicacion = u.Ubicacion,
                        Preferencias = u.Preferencias,
                        Foto = u.Foto,
                        Contraseña = u.Contraseña
                    })
                    .ToList();

                usuariosDTO = new ObservableCollection<UsuariosDTO>(listaUsuariosDTO);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Hubo un error al cargar los usuarios: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        public async Task DarLikeAsync(UsuariosDTO usuarioLikeado)
        {
            try
            {
                // Insertar un "like" del usuario actual al usuario objetivo
                await tinderdb.InsertarMatch(new Match
                {
                    Usuario1Id = UsuarioDTO.User_id,
                    Usuario2Id = usuarioLikeado.User_id,
                    FechaMatch = DateTime.Now
                });

                // Comprobar si existe un match mutuo
                var matches = await tinderdb.VerMatch();
                var esMatch = matches.Any(m =>
                    (m.Usuario1Id == usuarioLikeado.User_id && m.Usuario2Id == UsuarioDTO.User_id) ||
                    (m.Usuario2Id == usuarioLikeado.User_id && m.Usuario1Id == UsuarioDTO.User_id)
                );

                if (esMatch)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "¡Match!",
                        $"¡Tienes un match con {usuarioLikeado.Nombre}!",
                        "OK"
                    );
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "¡Like enviado!",
                        $"Has dado like a {usuarioLikeado.Nombre}.",
                        "OK"
                    );
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Hubo un error al dar like: {ex.Message}", "OK");
            }
        }

        partial void OnUsuarioIdChanged(int value)
        {
            if (value != 0)
            {
                CargarUsuario(value);
            }
        }

        public async void CargarUsuario(int value)
        {
            try
            {
                var usuarios = await tinderdb.VerUsuario();
                var usuarioSeleccionado = usuarios.FirstOrDefault(x => x.UsuarioId == value);

                if (usuarioSeleccionado != null)
                {
                    usuarioDTO = new UsuariosDTO
                    {
                        User_id = usuarioSeleccionado.UsuarioId,
                        Nombre = usuarioSeleccionado.Nombre,
                        Edad = usuarioSeleccionado.Edad,
                        Ubicacion = usuarioSeleccionado.Ubicacion,
                        Genero = usuarioSeleccionado.Genero,
                        Preferencias = usuarioSeleccionado.Preferencias,
                    };

                    titulo = "Editar Usuario";
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Hubo un error al cargar el usuario: {ex.Message}", "OK");
            }
        }
    }
}

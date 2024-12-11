using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
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
            usuariosDTO = new ObservableCollection<UsuariosDTO>();
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
                //var usuarios = await tinderdb.VerUsuario();

                // Convertir la lista de usuarios a una colección de DTOs
                List<Usuario> list = await tinderdb.VerUsuario();
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    UsuariosDTO.Clear();
                    foreach (Usuario usuario in list)
                    {
                        UsuariosDTO.Add(new UsuariosDTO
                        {
                            User_id = usuario.UsuarioId,
                            Nombre = usuario.Nombre,
                            Edad = usuario.Edad,
                            Genero = usuario.Genero,
                            Ubicacion = usuario.Ubicacion,
                            Preferencias = usuario.Preferencias,
                            Foto = usuario.Foto
                        });
                    }
                });
            }
            //    var listaUsuariosDTO = usuarios
            //        .Select(u => new UsuariosDTO
            //        {
            //            User_id = u.UsuarioId,
            //            Nombre = u.Nombre,
            //            Genero = u.Genero,
            //            Edad = u.Edad,
            //            Ubicacion = u.Ubicacion,
            //            Preferencias = u.Preferencias,
            //            Foto = u.Foto,
            //            Contraseña = u.Contraseña
            //        })
            //        .ToList();

            //    usuariosDTO = new ObservableCollection<UsuariosDTO>(listaUsuariosDTO);
            //}
            catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Hubo un error al cargar los usuarios: {ex.Message}", "OK");
                }
            }

        [RelayCommand]
        public async Task DarLikeAsync(UsuariosDTO usuarioLikeado)
        {
            if (usuarioLikeado == null)
            {
                await Application.Current.MainPage.DisplayAlert("Advertencia", "El usuario no es válido.", "OK");
                return;
            }

            try
            {
                // Verificar si ya existe un "like" previo para evitar duplicados
                var existeLikePrevio = await VerificarLike(UsuarioDTO.User_id, usuarioLikeado.User_id);

                if (existeLikePrevio)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Like ya enviado",
                        $"Ya has dado like a {usuarioLikeado.Nombre}.",
                        "OK"
                    );
                    return;
                }

                // Insertar un "like" en la base de datos
                await tinderdb.InsertarMatch(new Match
                {
                    Usuario1Id = UsuarioDTO.User_id,
                    Usuario2Id = usuarioLikeado.User_id,
                    FechaMatch = DateTime.Now
                });

                // Comprobar si existe un match mutuo
                var esMatch = await EsMatchMutuo(UsuarioDTO.User_id, usuarioLikeado.User_id);

                if (esMatch)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "¡Match!",
                        $"¡Tienes un match con {usuarioLikeado.Nombre}! Ahora pueden empezar a chatear.",
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
                // Mostrar un mensaje de error si algo sale mal
                await Application.Current.MainPage.DisplayAlert("Error", $"Hubo un error al dar like: {ex.Message}", "OK");
            }
        }
        public async Task<bool> VerificarLike(int usuario1Id, int usuario2Id)
        {
            var likes = await tinderdb.VerMatch(); // Obtén todos los matches de la base de datos
            return likes.Any(m => m.Usuario1Id == usuario1Id && m.Usuario2Id == usuario2Id);
        }
        public async Task<bool> EsMatchMutuo(int usuario1Id, int usuario2Id)
        {
            var matches = await tinderdb.VerMatch(); // Obtén todos los matches de la base de datos
            return matches.Any(m =>
                (m.Usuario1Id == usuario1Id && m.Usuario2Id == usuario2Id) ||
                (m.Usuario2Id == usuario1Id && m.Usuario1Id == usuario2Id)
            );
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
            List<Usuario> listado = await tinderdb.VerUsuario();
               if (listado!=null)
                {        
                Usuario usuarioSeleccionado = listado.Find((e) => e.UsuarioId == UsuarioId);          
                
                    UsuariosDTO usuarioDTO = new UsuariosDTO
                    {
                        User_id = usuarioSeleccionado.UsuarioId,
                        Nombre = usuarioSeleccionado.Nombre,
                        Edad = usuarioSeleccionado.Edad,
                        Ubicacion = usuarioSeleccionado.Ubicacion,
                        Genero = usuarioSeleccionado.Genero,
                        Preferencias = usuarioSeleccionado.Preferencias,
                        Foto = usuarioSeleccionado.Foto,
                    };
                    UsuarioDTO= usuarioDTO;
                    Titulo = "Editar Usuario";
                
            }
            
           
        }
        [RelayCommand]
        private async Task Eliminar(UsuariosDTO usuario)
        {
            if (usuario == null)
            {
                return;
            }

            bool respuesta = await Shell.Current.DisplayAlert("Confirmar eliminación", "Estás seguro que quieres eliminarlo", "Aceptar", "Cancelar");

            if (respuesta)
            {
                usuariosDTO.Remove(usuario); //Para evitar recargar de la bd todos los eventos, simplemente se elimina del listado
                await tinderdb.EliminarUsuario(usuario.User_id);

            }

        }
        [RelayCommand]
        private async Task Editar(UsuariosDTO usuario)
        {
            if (usuario == null)
            {
                return;
            }
            await Shell.Current.GoToAsync($"RegisterPage?id={usuario.User_id}"); //Paso de parámetros de una pag a otra
        }
    }
}

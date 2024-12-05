using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TinderApp.DTOs;
using TinderApp.Model;

namespace TinderApp.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {

        private readonly TinderDB tinderDB;

        [ObservableProperty]
        private ObservableCollection<UsuariosDTO> listaUsuarios;

        [ObservableProperty]
        private ObservableCollection<MatchsDTO> listaMatchs;

        [ObservableProperty]
        private bool isRefreshing;

        [ObservableProperty]
        private bool isBusy;

        

        public MainViewModel(TinderDB tinderDB)
        {
            this.tinderDB = tinderDB;

            listaUsuarios = new ObservableCollection<UsuariosDTO>();
            listaMatchs = new ObservableCollection<MatchsDTO>();
            _ = CargarListaUsuarios();
            _ = CargarListaMatch();

        }


        /* USUARIOS */

        [RelayCommand]
        public async Task CargarListaUsuarios() 
        {
            if (!isBusy)
            {
                return;
            }

            isBusy = true;
            isRefreshing = true;

            List<Usuario> listUsuarios = await tinderDB.VerUsuario();
            MainThread.BeginInvokeOnMainThread(() =>
            {

                listaUsuarios.Clear();
                foreach (Usuario usuario in listUsuarios)
                {
                    listaUsuarios.Add(new UsuariosDTO
                    {
                        User_id = usuario.UsuarioId,
                        Nombre = usuario.Nombre,
                        Genero = usuario.Genero,
                        Edad = usuario.Edad,
                        Ubicacion = usuario.Ubicacion,
                        Preferencias = usuario.Preferencias
                    });
                }

            });

            isBusy = false;
            isRefreshing = false;

        }

        [RelayCommand]
        private async Task CrearUsuario()
        {
            await Shell.Current.GoToAsync("UsuarioPage");
        }

        [RelayCommand]
        private async Task EditarUsuario(UsuariosDTO usuario)
        {
            if (usuario == null)
            {
                return;
            }

            await Shell.Current.GoToAsync($"UsuarioPage?id={usuario.User_id}");

        }

        [RelayCommand]
        private async Task EliminarUsuario(UsuariosDTO usuario)
        {
            if (usuario == null)
            {
                return;
            }

            bool respuesta = await Shell.Current.DisplayAlert("Confirmacion", "Estas seguro que quieres eliminar el usuario", "Aceptar", "Cancelar");

            if (respuesta) 
            {
                listaUsuarios.Remove(usuario);
                await tinderDB.EliminarUsuario(usuario.User_id);
            }

        }

        /* MATCHS */

        [RelayCommand]
        public async Task CargarListaMatch()
        {
            if (!isBusy)
            {
                return;
            }

            isBusy = true;
            isRefreshing = true;

            List<Match> listMatch = await tinderDB.VerMatch();
            MainThread.BeginInvokeOnMainThread(() =>
            {

                listaMatchs.Clear();
                foreach (Match match in listMatch)
                {
                    listaMatchs.Add(new MatchsDTO
                    {
                        MatchId = match.MatchId,
                        Usuario1Id = match.Usuario1Id,
                        Usuario2Id = match.Usuario2Id,
                        FechaMatch = match.FechaMatch
                        
                    });
                }

            });

            isBusy = false;
            isRefreshing = false;

        }

        [RelayCommand]
        private async Task CrearMatch()
        {
            await Shell.Current.GoToAsync("MatchPage");
        }

        [RelayCommand]
        private async Task EditarMathc(MatchsDTO match)
        {
            if (match == null)
            {
                return;
            }

            await Shell.Current.GoToAsync($"MatchPage?id={match.MatchId}");

        }

        [RelayCommand]
        private async Task EliminarMatch(MatchsDTO match)
        {
            if (match == null)
            {
                return;
            }

            bool respuesta = await Shell.Current.DisplayAlert("Confirmacion", "Estas seguro que quieres eliminar el Match", "Aceptar", "Cancelar");

            if (respuesta)
            {
                listaMatchs.Remove(match);
                await tinderDB.EliminarMatch(match.MatchId);
            }

        }

    }
}

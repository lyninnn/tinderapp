using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TinderApp.DTOs;
using TinderApp.Model;

namespace TinderApp.ViewModel
{
    public partial class MatchsViewModel : ObservableObject
    {
        private readonly TinderDB _database;

        [ObservableProperty]
        private MatchsDTO nuevoMatch = new MatchsDTO();

        public ObservableCollection<MatchsDTO> Matchs { get; set; } = new ObservableCollection<MatchsDTO>();

        public ICommand AgregarMatchCommand { get; }
        public ICommand EliminarMatchCommand { get; }

        public MatchsViewModel()
        {
            _database = new TinderDB();

            // Cargar matchs al inicio
            _ = CargarMatchs();

            // Comandos
            AgregarMatchCommand = new Command(AgregarMatch);
            EliminarMatchCommand = new Command<int>(EliminarMatch);
        }

        private async Task CargarMatchs()
        {
            Matchs.Clear();
            var matchs = await _database.VerMatch();
            foreach (var match in matchs)
            {
                Matchs.Add(new MatchsDTO
                {
                    MatchId = match.MatchId,
                    Usuario1Id = match.Usuario1Id,
                    Usuario2Id = match.Usuario2Id,
                    FechaMatch = match.FechaMatch
                });
            }
        }

        private async void AgregarMatch()
        {
            // Convertir DTO a modelo
            var match = new Match
            {
                Usuario1Id = NuevoMatch.Usuario1Id,
                Usuario2Id = NuevoMatch.Usuario2Id,
                FechaMatch = NuevoMatch.FechaMatch
            };

            await _database.InsertarMatch(match);
            CargarMatchs();
            NuevoMatch = new MatchsDTO(); // Reiniciar formulario
        }

        private async void EliminarMatch(int matchId)
        {
            await _database.EliminarMatch(matchId);
            CargarMatchs();
        }
    }
}

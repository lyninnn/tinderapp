﻿using CommunityToolkit.Mvvm.ComponentModel;
using TinderApp.Model;
using TinderApp.DTOs;

public partial class MatchesViewModel : ObservableObject
{
    private readonly TinderDB _database;

    [ObservableProperty]
    private List<Usuario> matches;

    [ObservableProperty]
    private UsuariosDTO usuarioActual;

    public MatchesViewModel(TinderDB database, UsuariosDTO usuario)
    {
        _database = database;
        UsuarioActual = usuario;
        _ = CargarMatchesAsync();
    }

    private async Task CargarMatchesAsync()
    {
        var allMatches = await _database.VerMatch();
        var matchedIds = allMatches
            .Where(m => m.Usuario1Id == UsuarioActual.User_id || m.Usuario2Id == UsuarioActual.User_id)
            .Select(m => m.Usuario1Id == UsuarioActual.User_id ? m.Usuario2Id : m.Usuario1Id)
            .ToList();

        Matches = (await _database.VerUsuario())
            .Where(u => matchedIds.Contains(u.UsuarioId))
            .ToList();
    }
}

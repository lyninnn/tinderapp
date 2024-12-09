﻿
using Microsoft.Extensions.Logging;
using TinderApp.Model;
using TinderApp.Views;
using TinderApp.ViewModel;

namespace TinderApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
            //Se registran las dependencias de SQLite
            SQLitePCL.Batteries.Init();

            //Se indica que se utilizará Singleton para el acceso a la BBDD
            builder.Services.AddSingleton<TinderDB>();


            //Hay que registrar todas las vistas y sus viewmodels asociados para que sean visibles en la aplicación
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<UsuarioPage>();
            builder.Services.AddTransient<UsuarioViewModel>();
            builder.Services.AddTransient<MatchPage>();
            builder.Services.AddTransient<MatchesViewModel>();



            //También hay que registrar las diferentes vistas "nuevas" añadidas al programa,
            // MainPage no es una nueva, ya que es la principal de la app (por eso no se añade).
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(UsuarioPage), typeof(UsuarioPage));
            Routing.RegisterRoute(nameof(MatchPage), typeof(MatchPage));
#endif

            return builder.Build();
        }
    }
}

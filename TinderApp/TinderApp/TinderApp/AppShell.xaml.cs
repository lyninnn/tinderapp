using TinderApp.Views;
namespace TinderApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(UsuarioPage), typeof(UsuarioPage));
            Routing.RegisterRoute(nameof(MatchPage), typeof(MatchPage));
        }
    }
}

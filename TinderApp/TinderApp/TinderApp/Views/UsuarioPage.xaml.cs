namespace TinderApp.Views;
using TinderApp.ViewModel;

public partial class UsuarioPage : ContentPage
{
	public UsuarioPage(UsuarioViewModel usuarioViewModel)
	{
		InitializeComponent();
        BindingContext = usuarioViewModel;
    }
}
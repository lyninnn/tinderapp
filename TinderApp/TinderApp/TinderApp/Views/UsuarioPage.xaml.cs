namespace TinderApp.Views;

public partial class UsuarioPage : ContentPage
{
	public UsuarioPage(UsuarioViewModel usuarioViewModel)
	{
		InitializeComponent();
        BindingContext = usuarioViewModel;
    }
}
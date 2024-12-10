
using TinderApp.ViewModel;
namespace TinderApp.Views;
public partial class MatchPage : ContentPage
{
	public MatchPage(MatchViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
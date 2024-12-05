using TinderApp.ViewModel;

namespace TinderApp
{
    public partial class MainPage : ContentPage
    {

        public MainPage(MainViewModel mainViewModel)
        {
            InitializeComponent();
            BindingContext = mainViewModel;
        }

     
    }

}

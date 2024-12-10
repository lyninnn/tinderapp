using TinderApp.Model;
using TinderApp.ViewModel;
using TinderApp.Views;

namespace TinderApp
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

            TinderDB db = new TinderDB();

            BindingContext = new MainViewModel(db);


        }

     
    }

}

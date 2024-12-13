using EFFlightsWpfApp.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EFFlightsWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCitiesView_Click(object sender, RoutedEventArgs e)
        {
            CitiesView citiesView = new CitiesView();
            citiesView.Show();
        }

        private void btnAirlinesView_Click(object sender, RoutedEventArgs e)
        {
            AirlinesView airlinesView = new AirlinesView();
            airlinesView.Show();
        }
    }
}
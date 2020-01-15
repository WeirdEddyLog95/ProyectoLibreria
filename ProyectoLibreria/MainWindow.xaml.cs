//using ProyectoLibreria.Models;
using ProyectoLibreria.Views;
using ProyectoLibreria.Views.CatalogosView;
using ProyectoLibreria.Views.PrestamosView;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProyectoLibreria
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            CatalogosMenuView generoForm = new CatalogosMenuView();
            generoForm.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            generoForm.Show();
        }

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            ListaPrestamosView librosForm = new ListaPrestamosView();
            librosForm.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            librosForm.Show();
        }
    }
}

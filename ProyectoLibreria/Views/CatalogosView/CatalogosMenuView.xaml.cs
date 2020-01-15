using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ProyectoLibreria.Views.CatalogosView
{
    /// <summary>
    /// Lógica de interacción para CatalogosMenuView.xaml
    /// </summary>
    public partial class CatalogosMenuView : Window
    {
        public CatalogosMenuView()
        {
            InitializeComponent();
        }

        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            GeneroInfoView catalogoGeneros = new GeneroInfoView();
            catalogoGeneros.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            catalogoGeneros.ShowDialog();
        }

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            PasilloInfoView corredorPasillos = new PasilloInfoView();
            corredorPasillos.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            corredorPasillos.ShowDialog();
        }

        private void Btn3_Click(object sender, RoutedEventArgs e)
        {
            LibrosInfoView catalogoLibros = new LibrosInfoView();
            catalogoLibros.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            catalogoLibros.ShowDialog();
        }

        private void Btn4_Click(object sender, RoutedEventArgs e)
        {
            UsuariosInfoView listaUsuarios = new UsuariosInfoView();
            listaUsuarios.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            listaUsuarios.ShowDialog();
        }
    }
}

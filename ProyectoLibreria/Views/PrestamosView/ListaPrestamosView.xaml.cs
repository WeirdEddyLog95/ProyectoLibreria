using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace ProyectoLibreria.Views.PrestamosView
{
    /// <summary>
    /// Lógica de interacción para ListaPrestamosView.xaml
    /// </summary>
    public partial class ListaPrestamosView : Window
    {
        public ListaPrestamosView()
        {
            InitializeComponent();
            //
            RefrescarPrestamos();
        }

        #region Metodos usados dentro de esta Interfaz
        /// <summary>
        /// Este metodo, sirve para aplicar una consulta de todos los prestamos que se han generado y mostrar
        /// quien tiene el libro y que libro se llama
        /// </summary>
        private void RefrescarPrestamos()
        {
            //Se crea una nueva conexion
            SqlConnection connection = CrearConexion();
            try
            {
                //Se abre la conexion
                connection.Open();
                //Se crea esta consulta enlazada con datos de la tabla del prestamo, junto con los nombres del usuarios
                //de la tabla de Usuarios y titulos de libros de la tabla de Libros
                string queryTest1 = "SELECT lp.Folio_Credencial, usr.Nombre_Usuario, lp.Clave_Libro, lib.Titulo_Libro FROM LibrosPrestamos lp " +
                "INNER JOIN UsuariosLibreria usr on lp.Folio_Credencial = usr.Folio_Credencial " +
                "INNER JOIN Libros lib on lp.Clave_Libro = lib.Clave_Libro";
                //Se hace un comando con la consulta
                SqlCommand cmd = new SqlCommand(queryTest1, connection);
                //Se crea un adaptador de datos que sera usado con el comando
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                //Se crea un nuevo conjunto de datos
                DataSet ds = new DataSet();
                sda.Fill(ds);
                //Se revisa si hay datos
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //Se pasa la informacion en el DataGrid
                    dataGrid1.ItemsSource = ds.Tables[0].DefaultView;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
        #endregion

        #region Region para Conexion SQL
        //Este metodo sirve para conectar la base de datos del sistema.
        public static SqlConnection CrearConexion()
        {
            //Usamos un constructor de conexiones de string de SQL Server
            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder();
            connectionString.DataSource = "(localdb)\\MSSQLLocalDB"; //Usamos una base de datos local
            //Conectamos un archivo de base de datos que esta dentro del proyecto
            connectionString.AttachDBFilename = @"C:\Users\DELL\Source\Repos\ProyectoLibreria\ProyectoLibreria\LibreriaBD.mdf";
            connectionString.IntegratedSecurity = true;
            //Usamos un string para revisar conexiones y le ponemos al final
            //una conexion SQL Server para ser usado en algun momento dentro de las funciones
            //del sistema
            string connectString = connectionString.ConnectionString;
            SqlConnection connection = new SqlConnection(connectString);
            return connection;
        }
        #endregion

        #region Acciones de la Interfaz
        /// <summary>
        /// Este metodo sirve para abrir la interfaz que genera los prestamos de libros
        /// </summary>
        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            PrestamosView prestamosForm = new PrestamosView();
            prestamosForm.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            prestamosForm.Closed += PrestamosForm_Closed;
            prestamosForm.ShowDialog();
        }

        /// <summary>
        /// Este metodo sirve para actualizar la lista de prestamos de la interfaz principal, 
        /// si se cierra la interfaz de prestamos de libros
        /// </summary>
        private void PrestamosForm_Closed(object sender, EventArgs e)
        {
            RefrescarPrestamos();
        }

        /// <summary>
        /// Este metodo sirve para abrir la interfaz que aplica el retorno de libros
        /// </summary>
        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            RetornosView retornosForm = new RetornosView();
            retornosForm.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            retornosForm.Closed += RetornosForm_Closed; ;
            retornosForm.ShowDialog();
        }

        /// <summary>
        /// Este metodo sirve para actualizar la lista de prestamos de la interfaz principal,
        /// si se cierra la interfaz de retorno de libros
        /// </summary>
        private void RetornosForm_Closed(object sender, EventArgs e)
        {
            RefrescarPrestamos();
        }
        #endregion
    }
}

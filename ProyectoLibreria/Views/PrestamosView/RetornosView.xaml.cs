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
    /// Lógica de interacción para RetornosView.xaml
    /// </summary>
    public partial class RetornosView : Window
    {
        public RetornosView()
        {
            InitializeComponent();
            iniciarPresentacion();
        }

        /// <summary>
        /// Este metodo sirve para inicializar la interfaz de retornos de libros
        /// </summary>
        private void iniciarPresentacion()
        {
            //Aqui se limpian los campos y se bloquean algunos, solo para anotar el Folio de la Credencial
            //y la Clave del Libro
            Folio_Credencial.Text = string.Empty;
            Folio_Credencial.IsEnabled = true;
            Nombre_Usuario.Text = string.Empty;
            Nombre_Usuario.IsReadOnly = true;
            Clave_Libro.Text = string.Empty;
            Clave_Libro.IsReadOnly = true;
            //Se usa el metodo para mostrar todos los libros que esten en prestamo
            RefrescarPrestamos();
        }

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

        #region Metodos del Retorno de Libros
        /// <summary>
        /// Este metodo sirve para buscar todos los libros que se han registrado en la base de datos que se encuentran en prestamo y los
        /// muestra con en el DataGrid
        /// </summary>
        private void RefrescarPrestamos()
        {
            SqlConnection connection = CrearConexion();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT Clave_Libro, Titulo_Libro, Autor_Libro, Estado FROM Libros Where Estado = 'Prestamo'", connection);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dataGrid1.ItemsSource = ds.Tables[0].DefaultView;
                }
                else
                {
                    dataGrid1.ItemsSource = null;
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

        /// <summary>
        /// Este metodo sirve para remover prestamos de libros de los usuarios de la biblioteca, usando solamente
        /// el folio de la credencial y la clave del libro
        /// </summary>
        private void retornarLibros(LibrosPrestamos prestamoCapturado)
        {
            SqlConnection conexiones = CrearConexion();
            try
            {
                conexiones.Open();
                string revisionPrestamos = string.Format("SELECT COUNT(*) FROM LibrosPrestamos Where Folio_Credencial = '{0}' and Clave_Libro = '{1}'", prestamoCapturado.Folio_Credencial, prestamoCapturado.Clave_Libro);
                SqlCommand comm = new SqlCommand(revisionPrestamos, conexiones);
                Int32 conteo = Convert.ToInt32(comm.ExecuteScalar());
                if (conteo != 0)
                {
                    //Para registrar
                    string queryEliminarPrestamo = string.Format("DELETE FROM LibrosPrestamos WHERE Folio_Credencial = '{0}' AND Clave_Libro = '{1}'", prestamoCapturado.Folio_Credencial, prestamoCapturado.Clave_Libro);
                    comm.CommandText = queryEliminarPrestamo;
                    comm.ExecuteNonQuery();
                    MessageBox.Show("Se ha terminado el prestamo del libro");
                    string queryActualizarEstado = string.Format("UPDATE Libros SET Estado = 'Disponible' WHERE Clave_Libro = '{0}'", prestamoCapturado.Clave_Libro);
                    comm.CommandText = queryActualizarEstado;
                    comm.ExecuteNonQuery();
                    MessageBox.Show("Se ha actualizado el estado del libro");
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
                conexiones.Close();
            }
        }

        /// <summary>
        /// Este metodo sirve para registrar nuevos prestamos de libros para usuarios de la biblioteca, usando solamente
        /// el folio de la credencial y la clave del libro
        /// </summary>
        private string buscarNombreUsuario(string folioCredencial)
        {
            string nombreUsuario = string.Empty;
            using (SqlConnection conexion = CrearConexion())
            {
                conexion.Open();
                string query = string.Format("SELECT Nombre_Usuario FROM UsuariosLibreria Where Folio_Credencial = '{0}'", folioCredencial);
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            nombreUsuario = reader.GetString(0);
                        }
                    }
                }
            }
            return nombreUsuario;
        }
        #endregion

        #region Acciones de la Interfaz
        /// <summary>
        /// Este evento, sucede cuando se pierde el enfoque en el cuadro de texto del Folio de Credencial
        /// una vez que sucede eso, se aplica un metodo para buscar el nombre del usuario usando el folio
        /// de la credencial
        /// </summary>
        private void Folio_Credencial_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Folio_Credencial.Text))
            {
                string folioBuscar = Folio_Credencial.Text;
                string nombreUsuario = buscarNombreUsuario(folioBuscar);
                Nombre_Usuario.Text = nombreUsuario;
            }
        }

        /// <summary>
        /// Esta accion, se usa para capturar la clave del libro del DataGrid y pasarlo al cuadro de texto 
        /// necesario para generar un prestamos
        /// </summary>
        private void DataGrid1_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            var itemsSource = dataGrid1.Items;
            if (itemsSource != null)
            {
                DataRowView dataRow = (DataRowView)dataGrid1.SelectedItem;
                int index = dataGrid1.CurrentCell.Column.DisplayIndex;
                string claveLibro = dataRow.Row.ItemArray[0].ToString();
                //
                Clave_Libro.Text = claveLibro;
            }
        }

        /// <summary>
        /// Esta accion, se usa para generar un nuevo prestamo usando el folio de la credencial y la clave del libro
        /// para crear un nuevo prestamo que se registrara en la base de datos
        /// </summary>
        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            //Aqui se captura los valores del Folio y la Clave como variables
            string folioCredencial = string.Empty;
            folioCredencial = Folio_Credencial.Text;
            string claveLibro = string.Empty;
            claveLibro = Clave_Libro.Text;
            //Generamos un modelo de Prestamo con las variables
            LibrosPrestamos prestamoRetirar = new LibrosPrestamos();
            prestamoRetirar.Folio_Credencial = folioCredencial;
            prestamoRetirar.Clave_Libro = claveLibro;
            //Al tener el modelo, se usa el metodo para eliminar el prestamo del libro, porque ya se retorno a la
            //biblioteca
            retornarLibros(prestamoRetirar);
            //Al finalizar, se actualiza el DataGrid y debe de mostrarse libros que siguen en prestamo
            iniciarPresentacion();
        }

        /// <summary>
        /// Este accion, sirve nomas para salir de la ventana
        /// </summary>
        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

    }
}

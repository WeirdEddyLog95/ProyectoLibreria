using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ProyectoLibreria.Views.CatalogosView
{
    /// <summary>
    /// Lógica de interacción para GeneroInfoView.xaml
    /// </summary>
    public partial class GeneroInfoView : Window
    {
        public GeneroInfoView()
        {
            InitializeComponent();
            //
            iniciarPresentacion();
        }

        /// <summary>
        /// Este metodo sirve para iniciar con la interfaz del Catalogo de Generos y hacer las siguientes 
        /// acciones 
        /// </summary>
        private void iniciarPresentacion()
        {
            //En esta accion se comienza activando y desactivando botones
            btn1.IsEnabled = true; //Se activa el boton para agregar un nuevo Genero
            btn2.IsEnabled = false; //Se desactiva el boton para capturar el Genero
            btn3.IsEnabled = false; //Se desactiva el boton para editar el Genero selecto
            btn4.IsEnabled = false; //Se desactiva el boton para borrar el Genero selecto
            btn5.IsEnabled = true; //Se activa el boton para refrescar el listado de Generos y reiniciar el presentacion
            
            //En esta seccion, se limpia los datos que puede contener en los textbox para el nombre
            //y clave del genero y se quedan los campos de texto desactivados, hasta que desee generar un nuevo genero
            //o modificar el genero existente
            Nombre_Genero.Text = string.Empty;
            Nombre_Genero.IsEnabled = false;
            Clave_Genero.Text = string.Empty;
            Clave_Genero.IsEnabled = false;
            //Al final se aplica el metodo para mostrar los generos que se han registrado al DataGrid
            RefrescarGeneros();
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

        #region Metodos de la Interfaz de Captura de Generos
        /// <summary>
        /// Este metodo sirve consultar y poner los generos que se han capturado en la base de datos
        /// en un conjunto de datos que se aplicara en el datagrid de la interfaz
        /// </summary>
        private void RefrescarGeneros()
        {
            //Establecemos las conexiones
            SqlConnection connection = CrearConexion();
            try
            {
                //Abrimos conexiones
                connection.Open();
                //Creamos un comando y se escribe que busque todo de la tabla de Generos
                SqlCommand cmd = new SqlCommand("SELECT * FROM Genero", connection);
                //Se usa un adaptador de datos y se aplica el comando que creamos
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                //Generamos un nuevo conjunto de datos
                DataSet ds = new DataSet();
                //Del adaptador, se pasan los datos al conjunto de datos
                sda.Fill(ds);
                //Revisamos si hay alguna fila antes de comenzar la siguiente accion
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //Si hay alguna fila, se llegara a poner la vista personalizada a la coleccion de datos del Data Grid
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

        /// <summary>
        /// Este metodo sirve para realizar dos operaciones, agregar datos de un nuevo genero y/o actualizar datos del genero
        /// que ya existe en la base de datos. Para realizar este metodo se captura el modelo de Genero
        /// </summary>
        private void RegistrarGeneros(Genero generoCapturado)
        {
            try
            {
                //Usamos una conexion SQL para realizar lo siguiente
                using (SqlConnection conexiones = CrearConexion())
                {
                    //Abrimos la conexion a la base de datos
                    conexiones.Open();
                    //Aplicamos una variable de transaccion SQL, para asegurarnos de que las operaciones
                    //que hagan en esta seccion, los datos que se registren se queden dentro de la base de
                    //datos
                    using (var trans = conexiones.BeginTransaction())
                    {
                        try
                        {
                            //Creamos una variable de comando SQL Server
                            using (var cmd = conexiones.CreateCommand())
                            {
                                //ponemos la transaccion en el comando
                                cmd.Transaction = trans;
                                //Hacemos la consulta para revisar generos con la clave
                                string revisionGenero = string.Format("SELECT COUNT(*) FROM Genero Where Clave_Genero = '{0}'", generoCapturado.Clave_Genero);
                                //SqlCommand comm = new SqlCommand(revisionGenero, conexiones);
                                //Ponemos la consulta en el comando y lo ejecutamos
                                cmd.CommandText = revisionGenero;
                                Int32 conteo = Convert.ToInt32(cmd.ExecuteScalar());
                                //Al final de la ejecucion, revisamos si la clave del genero existe o no existe en la base de datos
                                if (conteo != 0)
                                {
                                    //Si existe la clave, hacemos una consulta para actualizar el nombre del genero con la clave
                                    //de genero
                                    string queryActualizar = string.Format("UPDATE Genero SET Nombre_Genero = '{0}' " +
                                    "WHERE Clave_Genero = '{1}'", generoCapturado.Nombre_Genero, generoCapturado.Clave_Genero);
                                    cmd.CommandText = queryActualizar;
                                    //Ejecutamos la consulta
                                    cmd.ExecuteNonQuery();
                                    //Al final, avisamos que se actualizado el genero
                                    MessageBox.Show("Se ha actualizado el genero existente genero");
                                }
                                else
                                {
                                    //Si no existe la clave en la base, hacemos la siguiente consulta para insertar el nuevo genero
                                    //en la base de datos
                                    string queryRegistrar = string.Format("INSERT INTO Genero (Nombre_Genero, Clave_Genero) " +
                                    "VALUES ('{0}', '{1}')", generoCapturado.Nombre_Genero, generoCapturado.Clave_Genero);
                                    cmd.CommandText = queryRegistrar;
                                    //Ejecutamos
                                    cmd.ExecuteNonQuery();
                                    //Al terminar, avisamos al usuario que se logro guardar el nuevo genero
                                    MessageBox.Show("Se ha guardado un nuevo genero");
                                }
                            }
                            //Se cometen la transacccion a la base de datos
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Este metodo sirve para realizar la eliminacion de generos para la base de datos, se requiere
        /// tener el genero seleccionado en la interfaz para aplicar este metodo
        /// </summary>
        private void BorrarGeneros(Genero generoCapturado)
        {
            try
            {
                //Usamos una conexion SQL para realizar lo siguiente
                using (SqlConnection conexiones = CrearConexion())
                {
                    //Abrimos la conexion a la base de datos
                    conexiones.Open();
                    //Aplicamos una variable de transaccion SQL, para asegurarnos de que las operaciones
                    //que hagan en esta seccion, los datos que se registren se queden dentro de la base de
                    //datos
                    using (var trans = conexiones.BeginTransaction())
                    {
                        try
                        {
                            //Creamos una variable de comando SQL Server
                            using (var cmd = conexiones.CreateCommand())
                            {
                                //ponemos la transaccion en el comando
                                cmd.Transaction = trans;
                                //Hacemos la consulta para revisar generos con la clave
                                string revisionGenero = string.Format("SELECT COUNT(*) FROM Genero Where Clave_Genero = '{0}'", generoCapturado.Clave_Genero);
                                cmd.CommandText = revisionGenero;
                                Int32 conteo = Convert.ToInt32(cmd.ExecuteScalar());
                                //Se revisa el conteo para confirmar si este genero existe en la base de datos
                                if (conteo != 0)
                                {
                                    //Si existe este genero, se puede borrar de la base de datos
                                    string queryBorrar = string.Format("DELETE FROM Genero WHERE Nombre_Genero = '{0}' " +
                                    "AND Clave_Genero = '{1}'", generoCapturado.Nombre_Genero, generoCapturado.Clave_Genero);
                                    cmd.CommandText = queryBorrar;
                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Se ha borrado el genero de la base de datos");
                                }
                            }
                            //Se aplican la transaccion dentro de la base de datos
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Acciones de la interfaz
        //Esta accion sirve para iniciar a crear un nuevo Genero
        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            //Se limpia los campos de nombre y clave de genero y se activan para comenzar a escribir
            Nombre_Genero.IsEnabled = true;
            Nombre_Genero.Text = string.Empty;
            Clave_Genero.IsEnabled = true;
            Clave_Genero.Text = string.Empty;
            //Se aplican los siguientes cambios en los estados de los botones
            btn1.IsEnabled = false; //Se desactiva el boton para agregar un nuevo Genero
            btn2.IsEnabled = true; //Se activa el boton para guardar o actualizar el Genero
            btn3.IsEnabled = false; //Se desactiva el boton para sobreescribir el Genero
            btn4.IsEnabled = false; //Se desactiva el boton para eliminar el genero
            btn5.IsEnabled = true; //Se activa el boton para refrescar la lista de Generos y reiniciar la presentacion
        }

        //Para guardar y actualizar los generos
        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            //Se captura el nombre del Genero
            string nombreGenero = string.Empty;
            nombreGenero = Nombre_Genero.Text;
            //Se revisa el nombre que capturamos para asegurarnos que no este en blanco, si no se regresa
            if (string.IsNullOrEmpty(nombreGenero))
            {
                MessageBox.Show("Error, falta anotar el Nombre del Genero necesario para guardar el Genero");
                return;
            }
            //Se captura la clave del Genero
            string claveGenero = string.Empty;
            claveGenero = Clave_Genero.Text;
            //Se revisa la clave que capturamos para asegurarnos que no este en blanco, si no se regresa
            if (string.IsNullOrEmpty(claveGenero))
            {
                MessageBox.Show("Error, falta anotar la Clave del Genero necesario para guardar el Genero");
                return;
            }
            //De los valores que capturamos, se ponen en un modelo de genero
            Genero generoRegistrar = new Genero();
            generoRegistrar.Nombre_Genero = nombreGenero;
            generoRegistrar.Clave_Genero = claveGenero;
            //Del modelo, le ponemos el metodo para registrar Generos
            RegistrarGeneros(generoRegistrar);
            //Al finalizar, se aplica el evento para reiniciar campos de texto y los estados de los botones
            //a como estuvo al abrir la interfaz
            iniciarPresentacion();
        }

        /// <summary>
        /// Este metodo sirve para seleccionar alguna fila del DataGrid donde se muestra los generos
        /// que se han registrado en la base de datos y los campos del grid que se selecciono,
        /// se ponen en los campos de texto
        /// </summary>
        private void DataGrid1_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            //Se captura el itemSource del DataGrid
            var itemsSource = dataGrid1.Items;
            if (itemsSource != null)
            {
                //Se captura la fila de datos que se selecciono del Data Grid
                DataRowView dataRow = (DataRowView)dataGrid1.SelectedItem;
                //Sacamos el indice de la columna que tiene la celda selecta
                int index = dataGrid1.CurrentCell.Column.DisplayIndex;
                //Separamos los valores de la fila selecta en variables para nombre de genero y clave de genero
                string nombreGenero = dataRow.Row.ItemArray[0].ToString();
                string claveGenero = dataRow.Row.ItemArray[1].ToString();
                //Las variables terminan siendo asignadas en los cuadros de texto
                Nombre_Genero.Text = nombreGenero;
                Clave_Genero.Text = claveGenero;
                //Para aplicar los cambios de los estados de los botones
                btn1.IsEnabled = false; //Se desactiva el boton para agregar un nuevo Genero
                btn2.IsEnabled = false; //Se desactiva el boton para guardar y/o actualizar el Genero
                btn3.IsEnabled = true; //Se activa el boton para sobreescribir el Genero selecto
                btn4.IsEnabled = true; //Se activa el boton para eliminar el Genero selecto
                btn5.IsEnabled = true; //Se activa el boton para refrescar la lista de Generos y reiniciar la interfaz
            }
        }

        /// <summary>
        /// Este metodo sirve para modificar el Genero seleccionado
        /// </summary>
        private void Btn3_Click(object sender, RoutedEventArgs e)
        {
            //Se activan los campos del Genero en la Interfaz
            Nombre_Genero.IsEnabled = true;
            Clave_Genero.IsEnabled = true;

            //Para aplicar los cambios de los estados de los botones
            btn1.IsEnabled = false; //Se desactiva el boton para agregar un nuevo Genero
            btn2.IsEnabled = true; //Se activa el boton para guardar y/o actualizar Genero
            btn3.IsEnabled = false; //Se desactiva el boton para modificar el Genero
            btn4.IsEnabled = false; //Se desactiva el boton para borrar el Genero
            btn5.IsEnabled = true; //Se activa el boton para actualizar el Genero
        }

        /// <summary>
        /// Este metodo sirve para borrar el Genero seleccionado
        /// </summary>
        private void Btn4_Click(object sender, RoutedEventArgs e)
        {
            //Se capturan los datos del cuadro de texto en variables de texto
            string nombreGenero = string.Empty;
            nombreGenero = Nombre_Genero.Text;
            string claveGenero = string.Empty;
            claveGenero = Clave_Genero.Text;
            //Los ponemos en un modelo de Genero
            Genero generoRegistrar = new Genero();
            generoRegistrar.Nombre_Genero = nombreGenero;
            generoRegistrar.Clave_Genero = claveGenero;
            //Aplicamos el metodo para borrar el genero de la base de datos
            BorrarGeneros(generoRegistrar);
            //Al finalizar se reinicia la presentacion de la interfaz
            iniciarPresentacion();
        }

        /// <summary>
        /// Este metodo sirve para refrescar el listado de generos registrados en la base de datos
        /// </summary>
        private void Btn5_Click(object sender, RoutedEventArgs e)
        {
            //Se aplica el metodo para reinicar la presentacion de la interfaz
            iniciarPresentacion();
        }
        #endregion
    }
}

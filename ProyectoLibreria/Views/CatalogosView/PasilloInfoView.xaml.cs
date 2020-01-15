using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Lógica de interacción para PasilloInfoView.xaml
    /// </summary>
    public partial class PasilloInfoView : Window
    {
        public PasilloInfoView()
        {
            InitializeComponent();
            //
            iniciarPresentacion();
        }

        /// <summary>
        /// Este metodo sirve para inicializar la interfaz de captura de pasillos y sus acciones
        /// </summary>
        private void iniciarPresentacion()
        {
            //Se inicia desde arriba, con los botones, que se activan y desactivan
            btn1.IsEnabled = true; // Se activa el boton para generar un nuevo pasillo
            btn2.IsEnabled = false; //Se desactivan los botones de para capturar el nuevo pasillo
            btn4.IsEnabled = false; //y el boton para borrar el pasillo
            btn5.IsEnabled = true; //Se activa el boton para refrescar el listado de pasillos del DataGrid y reiniciar el presentacion
            //En esta parte, se limpia el campo de texto del pasillo y esta desactivado, hasta que desee
            //crear un nuevo pasillo.
            Id_Pasillo.Text = string.Empty;
            Id_Pasillo.IsEnabled = false;
            //Al final se usa el metodo para mostrar los pasillos que se han capturado en la base de datos
            //hacia el DataGrid
            RefrescarPasillos();
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

        #region Metodos para la generar nuevos pasillos
        /// <summary>
        /// Este metodo sirve para consultar y mostrar todos los pasillos que se han 
        /// registrado de la base de datos hacia el datagrid de esta interfaz
        /// </summary>
        private void RefrescarPasillos()
        {
            //Se establece la conexion a la base de datos
            SqlConnection connection = CrearConexion();
            try
            {
                //Se abre la conexion
                connection.Open();
                //Creamos el comando para buscar todos los pasillos, de forma ordenada
                SqlCommand cmd = new SqlCommand("SELECT * FROM Pasillos ORDER BY Id_Pasillo", connection);
                //Pasamos el comando a un adaptador
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                //Creamos un nuevo conjunto de datos limpio
                DataSet ds = new DataSet();
                //Llenamos el conjunto de datos con informacion del adaptador de comandos de SQL
                sda.Fill(ds);
                //Revisamos si hay filas en el conjunto de datos 
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //Si existe alguna fila del conjunto, enviamos esa informacion a la fuente de items del DataGrid
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
        /// Este metodo sirve para capturar el modelo del Pasillo como dato de la tabla de Pasillos
        /// de la base de datos
        /// </summary>
        private void RegistrarPasillos(Pasillos pasilloCapturado)
        {
            try
            {
                //Usamos una conexion de SQL
                using (SqlConnection conexiones = CrearConexion())
                {
                    //Abrimos la conexion de la base de datos
                    conexiones.Open();
                    //Mientras tengamos la conexion, usamos una transaccion SQL que nos servira al finalizar
                    //la insercion del pasillo 
                    using (var trans = conexiones.BeginTransaction())
                    {
                        try
                        {
                            //Creamos un comando con la conexion abierta
                            using (var cmd = conexiones.CreateCommand())
                            {
                                //Mientras tengamos el comando, ponemos la transaccion dentro del comando
                                cmd.Transaction = trans;
                                //Hacemos nuestra consulta
                                string revisionPasillos = string.Format("SELECT COUNT(*) FROM Pasillos Where Id_Pasillo = '{0}'", pasilloCapturado.Id_Pasillo);
                                //Hacemos insertamos la consulta en el comando
                                cmd.CommandText = revisionPasillos;
                                //Ejecutamos la consulta para revisar si existe el pasillo que capturamos en la base de datos
                                Int32 conteo = Convert.ToInt32(cmd.ExecuteScalar());
                                if (conteo == 0)
                                {
                                    //Si no existe el pasillo, hacemos otra consulta para insertar el nuevo pasillo en la base de datos
                                    string queryRegistrar = string.Format("INSERT INTO Pasillos (Id_Pasillo) " +
                                    "VALUES ('{0}')", pasilloCapturado.Id_Pasillo);
                                    cmd.CommandText = queryRegistrar;
                                    //Lo ejecutamos
                                    cmd.ExecuteNonQuery();
                                    //Y avisamos que ya se logro registrar este pasillo nuevo en la base de datos
                                    MessageBox.Show("Se ha generado un nuevo Pasillo");
                                }
                                else
                                {
                                    //Si ya existe el pasillo, nomas enviamos un mensaje de regreso, no hacemos otra operacion
                                    //hasta ahi
                                    MessageBox.Show("Error, no se puede registrar este pasillo que ya existe.");
                                }
                            }
                            //Cometemos los cambios de la transaccion en la base de datos
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
        /// Este metodo sirve para borrar un pasillo de la base de datos,
        /// usando un dato seleccionado de la interfaz
        /// </summary>
        private void BorrarPasillos(Pasillos pasilloCapturado)
        {
            try
            {
                //Usamos una conexion SQL para realizar lo siguiente
                using (SqlConnection conexiones = CrearConexion())
                {
                    //Abrimos la conexion a la base de datos
                    conexiones.Open();
                    //Aplicamos una variable de transaccion SQL con la conexion abierta
                    using (var trans = conexiones.BeginTransaction())
                    {
                        try
                        {
                            //Creamos un comando SQL con la conexion
                            using (var cmd = conexiones.CreateCommand())
                            {
                                //Metemos la transaccion en el comando
                                cmd.Transaction = trans;
                                //Se crea una consulta para revisar si el pasillo existe en la base de datos
                                string revisionPasillos = string.Format("SELECT COUNT(*) FROM Pasillos Where Id_Pasillo = '{0}'", pasilloCapturado.Id_Pasillo);
                                //Insertamos la consulta en el comando
                                cmd.CommandText = revisionPasillos;
                                //Ejecutamos la consulta
                                Int32 conteo = Convert.ToInt32(cmd.ExecuteScalar());
                                if (conteo != 0)
                                {
                                    //Si existe el pasillo, se hace otra consulta para borrar el pasillo de la base de datos
                                    string queryBorrar = string.Format("DELETE FROM Pasillos Where Id_Pasillo = '{0}'", pasilloCapturado.Id_Pasillo);
                                    cmd.CommandText = queryBorrar;
                                    //Se ejecuta la consulta
                                    cmd.ExecuteNonQuery();
                                    //Avisamos que si se logro borrar el pasillo de la base de datos
                                    MessageBox.Show("Se ha borrado el Pasillos de la base de datos");
                                }
                            }
                            //Cometemos los cambios de la transaccion
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

        #region Las acciones de la interfaz del Pasillo
        //Esta accion sirve para iniciar a crear un nuevo Pasillo
        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            //Se limpia el campo del pasillo y se activa para escribir
            Id_Pasillo.Text = string.Empty;
            Id_Pasillo.IsEnabled = true;
            //Se aplican los siguientes cambios en los estados de los botones
            btn1.IsEnabled = false; //Se desactiva el boton para agregar un nuevo Pasillo
            btn2.IsEnabled = true; //Se activa el boton para guardar o actualizar el Pasillo
            btn4.IsEnabled = false; //Se desactiva el boton para eliminar el Pasillo selecto
            btn5.IsEnabled = true; //Se activa el boton para refrescar Pasillos y reiniciar la presentacion
        }

        //Esta accion sirve para comenzar a guardar el Pasillo en la base de datos
        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            //Se captura el campo de texto que contiene el pasillo
            string descPasillo = string.Empty;
            descPasillo = Id_Pasillo.Text;

            //Se revisa si no esta en blanco el texto
            if (!string.IsNullOrEmpty(descPasillo))
            {
                //Hacemos uso de una expresion regular, para revisar el texto que hemos puesto
                //es alfanumerico
                Regex r = new Regex("^[a-zA-Z0-9]*$");
                //Comparamos si encaja la expresion
                if (r.IsMatch(descPasillo))
                {
                    //Si encaja, se genera un nuevo modelo de Pasillo y poner el valor
                    //dentro del nuevo modelo
                    Pasillos pasilloCapturado = new Pasillos();
                    pasilloCapturado.Id_Pasillo = descPasillo;
                    //Al generar el nuevo modelo, se pasa al metodo para guardarlo en la base de datos
                    RegistrarPasillos(pasilloCapturado);
                    //Al finalizar el guardado de pasillo, se reinicia la presentacion de la interfaz
                    iniciarPresentacion();
                }
                else
                {
                    //Si no encaja, se hace saber al usuario con un mensaje y se detiene las operaciones de este metodo
                    MessageBox.Show("El pasillo requiere ser alfanumerico");
                    return;
                }
            }
            else
            {
                //Si el texto esta en blanco, se avisa el usuario con este mensaje y se detiene las operaciones de este metodo
                MessageBox.Show("No puede tener pasillos en blanco.");
                return;
            }
        }

        //Esta accion, permite tener una fila del Grid seleccionada y de los valores que contiene
        //se pasan al campo de texto
        private void DataGrid1_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            var itemsSource = dataGrid1.Items;
            if (itemsSource != null)
            {
                //Se captura la fila seleccionada del Data Grid 
                DataRowView dataRow = (DataRowView)dataGrid1.SelectedItem;
                //Se saca el indice
                int index = dataGrid1.CurrentCell.Column.DisplayIndex;
                //Se saca el valor de la fila de datos, y se convierte en variable
                string descPasillo = dataRow.Row.ItemArray[0].ToString();
                //Y la variable se pone en el campo de texto
                Id_Pasillo.Text = descPasillo;
                //Se cambian los estados de los botones del Pasillo
                btn1.IsEnabled = false; //Se desactiva el boton para agregar un nuevo Pasillo
                btn2.IsEnabled = false; //Se desactiva el boton para guardar y/o actualizar el Pasillo
                btn4.IsEnabled = true; //Se activa el boton para eliminar el Pasillo de la base de datos
                btn5.IsEnabled = true; //Se activa el boton para refrescar Pasillo y reiniciar la interfaz
            }
        }

        //Esta accion, sirve para borrar el Pasillo seleccionado
        private void Btn4_Click(object sender, RoutedEventArgs e)
        {
            //Se captura el dato del pasillo
            string descPasillo = string.Empty;
            descPasillo = Id_Pasillo.Text;
            //Se pasa a un modelo de Pasillo
            Pasillos pasilloCapturado = new Pasillos();
            pasilloCapturado.Id_Pasillo = descPasillo;
            //El modelo es usado en un metodo para borrar el pasillo en la base de datos
            BorrarPasillos(pasilloCapturado);
            //Al finalizar la eliminacion, se reinicia la interfaz
            iniciarPresentacion();
        }

        //Este metodo sirve para refrescar el listado de pasillos y reiniciar esta interfaz
        private void Btn5_Click(object sender, RoutedEventArgs e)
        {
            //Se aplica el metodo para reiniciar la presentacion de esta interfaz
            iniciarPresentacion();
        }
        #endregion

    }
}

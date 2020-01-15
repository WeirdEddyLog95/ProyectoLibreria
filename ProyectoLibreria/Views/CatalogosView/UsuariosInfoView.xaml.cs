using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
    /// Lógica de interacción para UsuariosInfoView.xaml
    /// </summary>
    public partial class UsuariosInfoView : Window
    {
        LibreriaBDEntities dataEntities = new LibreriaBDEntities();
        public UsuariosInfoView()
        {
            InitializeComponent();
            //
            iniciarPresentacion();
        }

        /// <summary>
        /// Este metodo sirve para iniciar la interfaz del Catalogo de Usuarios y hacer estas acciones
        /// </summary>
        private void iniciarPresentacion()
        {
            //En esta accion se comienza activando y desactivando botones
            btn1.IsEnabled = true; //Se activa el boton para agregar un nuevo Usuario
            btn2.IsEnabled = false; //Se desactiva el boton para capturar el Usuario
            btn3.IsEnabled = false; //Se desactiva el boton para editar el Usuario selecto
            btn4.IsEnabled = false; //Se desactiva el boton para borrar el Usuario selecto
            btn5.IsEnabled = true; //Se activa el boton para refrescar el listado de Usuarios y reiniciar el presentacion
            
            //En esta seccion, se limpian los campos, dejando Nombre del Usuario en blanco por ejemplo, y agregando
            //la fecha y año actual en los campos de Fecha de Registro y Año para la Credencial
            Nombre_Usuario.Text = string.Empty;
            Nombre_Usuario.IsEnabled = false;
            Año_Credencial.Text = DateTime.Today.Year.ToString();
            Año_Credencial.IsEnabled = false;
            Folio_Secuencia.Text = string.Empty;
            Folio_Secuencia.IsEnabled = false;
            Fecha_Registro.Text = DateTime.Today.ToShortDateString();
            Fecha_Registro.IsEnabled = false;
            //Al final de limpiar los campos, se aplica el metodo para mostrar todos los usuarios registrados
            RefrescarUsuarios();
        }

        #region Region para Conexion SQL
        public static SqlConnection CrearConexion()
        {
            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder();
            connectionString.DataSource = "(localdb)\\MSSQLLocalDB";
            connectionString.AttachDBFilename = @"C:\Users\DELL\Source\Repos\ProyectoLibreria\ProyectoLibreria\LibreriaBD.mdf";
            //"|DataDirectory|\\LibreriaBD.mdf";
            connectionString.IntegratedSecurity = true;
            string connectString = connectionString.ConnectionString;
            SqlConnection connection = new SqlConnection(connectString);
            return connection;
        }
        #endregion

        #region Metodos de la Interfaz de Usuarios de la Libreria
        /// <summary>
        /// Este metodo sirve buscar todos los usuarios registrados dentro de la base de datos
        /// que seran aplicados dentro del DataGrid
        /// </summary>
        private void RefrescarUsuarios()
        {
            //Generamos la conexion
            SqlConnection connection = CrearConexion();
            try
            {
                //Abrimos la conexion
                connection.Open();
                //Creamos un comando con la siguiente consulta
                SqlCommand cmd = new SqlCommand("SELECT Nombre_Usuario, Folio_Credencial, FORMAT (Fecha_Registro, 'dd/MM/yyyy') as Fecha_Registro FROM UsuariosLibreria", connection);
                //Creamos un adaptador de datos para usar el comando
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                //Generamos un nuevo conjunto de datos
                DataSet ds = new DataSet();
                //Se pasan los datos del adaptador hacia el conjunto de datos
                sda.Fill(ds);
                //Revisamos si hay fila en el conjunto de datos
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //Si hay datos, se pone la informacion el DataGrid
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
        /// Este metodo sirve para dos operaciones, registrar un nuevo usuario y actualizar un usuario existente
        /// usando un modelo de Usuario generado con los datos de la interfaz
        /// </summary>
        private void RegistrarUsuarios(UsuariosLibreria usuariosCapturado)
        {
            try
            {
                //Usamos una nueva conexion a la base de datos
                using (SqlConnection conexiones = CrearConexion())
                {
                    //Abrimos la conexion
                    conexiones.Open();
                    //Creamos una nueva transaccion que usara al finalizar la operacion
                    using (var trans = conexiones.BeginTransaction())
                    {
                        try
                        {
                            //Se crea un comando con la conexion
                            using (var cmd = conexiones.CreateCommand())
                            {
                                //Insertamos la transaccion en el comando
                                cmd.Transaction = trans;
                                //Creamos la consulta
                                string revisionUsuarios = string.Format("SELECT COUNT(*) FROM UsuariosLibreria Where Folio_Credencial = '{0}'", usuariosCapturado.Folio_Credencial);
                                //Ponemos la consulta en el comando
                                cmd.CommandText = revisionUsuarios;
                                //Ejecutamos
                                Int32 conteo = Convert.ToInt32(cmd.ExecuteScalar());
                                //Revisamos si existe el usuario
                                if (conteo != 0)
                                {
                                    //Si existe el usuario, hacemos la consulta para actualizar y ejecutarla
                                    string queryActualizar = string.Format("UPDATE UsuariosLibreria SET Nombre_Usuario = '{0}', Fecha_Registro = '{1}' " +
                                    "WHERE Folio_Credencial = '{2}'", usuariosCapturado.Nombre_Usuario, usuariosCapturado.Fecha_Registro.ToString("yyyy-MM-dd"), usuariosCapturado.Folio_Credencial);
                                    cmd.CommandText = queryActualizar;
                                    cmd.ExecuteNonQuery();
                                    //Enviamos este mensaje al usuario
                                    MessageBox.Show("Se ha actualizado los datos del Usuario existente");
                                }
                                else
                                {
                                    //Si no existe el usuario, hacemos la consulta para insertar el nuevo usuario y ejecutarla
                                    string queryRegistrar = string.Format("INSERT INTO UsuariosLibreria (Nombre_Usuario, Fecha_Registro, Folio_Credencial) " +
                                    "VALUES ('{0}', '{1}', '{2}')", usuariosCapturado.Nombre_Usuario, usuariosCapturado.Fecha_Registro.ToString("yyyy-MM-dd"), usuariosCapturado.Folio_Credencial);
                                    cmd.CommandText = queryRegistrar;
                                    cmd.ExecuteNonQuery();
                                    //Enviamos este mensaje al usuario
                                    MessageBox.Show("Se ha generado un nuevo Usuario");
                                }
                            }
                            //Cometemos los cambios en la base de datos
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
        /// Este metodo sirve para eliminar un usuario seleccionado de la base de datos, con un modelo de Usuario
        /// generado de los datos de la interfaz
        /// </summary>
        private void BorrarUsuarios(UsuariosLibreria usuarioCapturado)
        {
            try
            {
                //Se usa una nueva conexion de la base de datos
                using (SqlConnection conexiones = CrearConexion())
                {
                    //Abrimos
                    conexiones.Open();
                    //Usamos una transaccion para aplicar cambios en la base de datos
                    using (var trans = conexiones.BeginTransaction())
                    {
                        try
                        {
                            //Creamos un comando con la conexion
                            using (var cmd = conexiones.CreateCommand())
                            {
                                //Colocamos la transaccion dentro del comando
                                cmd.Transaction = trans;
                                //Creamos la consulta
                                string revisionUsuarios = string.Format("SELECT COUNT(*) FROM UsuariosLibreria Where Folio_Credencial = '{0}'", usuarioCapturado.Folio_Credencial);
                                //La consulta se pone el comando
                                cmd.CommandText = revisionUsuarios;
                                //Se ejecuta la consulta
                                Int32 conteo = Convert.ToInt32(cmd.ExecuteScalar());
                                //Se revisa la consulta
                                if (conteo != 0)
                                {
                                    //Si existe este usuario, se hace la siguiente consulta para eliminar el usuario de la base de datos y se ejecuta
                                    string queryBorrar = string.Format("DELETE FROM UsuariosLibreria Where Folio_Credencial = '{0}'", usuarioCapturado.Folio_Credencial);
                                    cmd.CommandText = queryBorrar;
                                    cmd.ExecuteNonQuery();
                                    //Al final enviamos este mensaje al mensaje
                                    MessageBox.Show("Se ha borrado el Usuarios de la base de datos");
                                }
                            }
                            //Se comete los cambios dentro de la base de datos
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

        #region Acciones de los Botones de la interfaz de usuarios
        /// <summary>
        /// Esta accion sirve para iniciarlizar los campos para crear un nuevo Usuario
        /// </summary>
        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            //Se limpia y se activan los campos para crear un nuevo Usuario en la interfaz
            Nombre_Usuario.Text = string.Empty;
            Nombre_Usuario.IsEnabled = true;
            Año_Credencial.Text = DateTime.Today.Year.ToString();
            Año_Credencial.IsEnabled = true;
            Folio_Secuencia.Text = string.Empty;
            Folio_Secuencia.IsEnabled = true;
            Fecha_Registro.Text = DateTime.Today.ToShortDateString();
            Fecha_Registro.IsEnabled = true;
            //Se aplican los siguientes cambios en los estados de los botones
            btn1.IsEnabled = false; //Se desactiva el boton para agregar un nuevo Usuario
            btn2.IsEnabled = true; //Se activa el boton para guardar o actualizar el Usuario
            btn3.IsEnabled = false; //Se desactiva el boton para sobreescribir el Usuario
            btn4.IsEnabled = false; //Se desactiva el boton para eliminar el Usuario
            btn5.IsEnabled = true; //Se activa el boton para refrescar la lista de Usuarios y reiniciar la presentacion
        }

        /// <summary>
        /// Esta accion sirve para registrar o actualizar Usuarios de la base de datos
        /// </summary>
        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            //Se capturan los datos de los campos de texto y fechas en la interfaz y se vuelven en variables
            string nombreUsuario = string.Empty;
            nombreUsuario = Nombre_Usuario.Text;
            string añoCredencial = string.Empty;
            añoCredencial = Año_Credencial.Text;
            Regex revision = new Regex(@"\d{4}");
            //Comparamos si encaja la expresion
            if (revision.IsMatch(Año_Credencial.Text))
            {
                añoCredencial = Año_Credencial.Text;
            }
            else
            {
                MessageBox.Show("Error, este campo es para anotar años solamente");
                return;
            }
            int folioSecuencia = new int();
            if(int.TryParse(Folio_Secuencia.Text, out int resultado))
            {
                folioSecuencia = resultado;
            }
            else
            {
                MessageBox.Show("Favor de poner solo numeros en el folio de la secuencia");
                return;
            }
            string folioCredencial = string.Empty;
            folioCredencial = string.Format("{0}-{1}", añoCredencial, folioSecuencia);
            //
            DateTime fechaRegistro = new DateTime();
            if (DateTime.TryParseExact(Fecha_Registro.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultFecha))
            {
                fechaRegistro = resultFecha;
            }
            else
            {
                MessageBox.Show("Error, el formato de la fecha de publicacion es incorrecto.\nEl formato para la fecha es la siguiente 'dd/MM/yyyy'");
                return;
            }
            //Se juntan las variables dentro de un modelo de Usuario
            UsuariosLibreria usuarioCapturado = new UsuariosLibreria();
            usuarioCapturado.Nombre_Usuario = nombreUsuario;
            usuarioCapturado.Fecha_Registro = fechaRegistro;
            usuarioCapturado.Folio_Credencial = folioCredencial;
            //Se registra el modelo con el metodo para guardar Usuarios en la base de datos
            RegistrarUsuarios(usuarioCapturado);
            //Al finalizar, se reinicia la interfaz
            iniciarPresentacion();
        }

        /// <summary>
        /// Esta accion sirve para seleccionar un Usuario en el DataGrid, para mostrarlo en los campos
        /// de llenado del Usuario, para modificar o borrar
        /// </summary>
        private void DataGrid1_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            //Se captura el itemSource del DataGrid
            var itemsSource = dataGrid1.Items;
            if (itemsSource != null)
            {
                //Se captura la fila del DataGrid
                DataRowView dataRow = (DataRowView)dataGrid1.SelectedItem;
                //Sacamos el indice
                int index = dataGrid1.CurrentCell.Column.DisplayIndex;
                //Sacamos las variables de la fila
                string nombreUsuario = dataRow.Row.ItemArray[0].ToString();
                string folioCredencial = dataRow.Row.ItemArray[1].ToString();
                string fechaRegistro = dataRow.Row.ItemArray[2].ToString();
                //Los pasamos de vuelta a la interfaz
                Nombre_Usuario.Text = nombreUsuario;
                string[] separacion = folioCredencial.Split('-');
                Año_Credencial.Text = separacion[0];
                Folio_Secuencia.Text = separacion[1];
                Fecha_Registro.Text = fechaRegistro;
                //Para aplicar los cambios de los estados de los botones
                btn1.IsEnabled = false; //Se desactiva el boton para agregar un nuevo Usuario
                btn2.IsEnabled = false; //Se desactiva el boton para guardar y/o actualizar el Usuario
                btn3.IsEnabled = true; //Se activa el boton para sobreescribir el Usuario selecto
                btn4.IsEnabled = true; //Se activa el boton para eliminar el Usuario selecto
                btn5.IsEnabled = true; //Se activa el boton para refrescar la lista de Usuario y reiniciar la interfaz
            }
        }

        /// <summary>
        /// Esta accion sirve para modificar el Usuario seleccionado
        /// </summary>
        private void Btn3_Click(object sender, RoutedEventArgs e)
        {
            //Se activan estos campos de la interfaz
            Nombre_Usuario.IsEnabled = true;
            Año_Credencial.IsEnabled = true;
            Folio_Secuencia.IsEnabled = true;
            Fecha_Registro.IsEnabled = true;

            //Para aplicar los cambios de los estados de los botones
            btn1.IsEnabled = false; //Se desactiva el boton para agregar un nuevo Usuario
            btn2.IsEnabled = true; //Se activa el boton para guardar y/o actualizar Usuario
            btn3.IsEnabled = false; //Se desactiva el boton para modificar el Usuario
            btn4.IsEnabled = false; //Se desactiva el boton para borrar el Usuario
            btn5.IsEnabled = true; //Se activa el boton para actualizar el Usuario
        }

        /// <summary>
        /// Esta accion sirve para eliminar el Usuario seleccionado
        /// </summary>
        private void Btn4_Click(object sender, RoutedEventArgs e)
        {
            //Se capturan en variables, los datos en los campos de texto y datepicker de la interfaz
            string nombreUsuario = string.Empty;
            nombreUsuario = Nombre_Usuario.Text;
            string añoCredencial = string.Empty;
            añoCredencial = Año_Credencial.Text;
            Regex revision = new Regex(@"\d{4}");
            //Comparamos si encaja la expresion
            if (revision.IsMatch(Año_Credencial.Text))
            {
                añoCredencial = Año_Credencial.Text;
            }
            else
            {
                MessageBox.Show("Error, este campo es para anotar años solamente");
                return;
            }
            int folioSecuencia = new int();
            if (int.TryParse(Folio_Secuencia.Text, out int resultado))
            {
                folioSecuencia = resultado;
            }
            else
            {
                MessageBox.Show("Favor de poner solo numeros en el folio de la secuencia");
                return;
            }
            string folioCredencial = string.Empty;
            folioCredencial = string.Format("{0}-{1}", añoCredencial, folioSecuencia);
            //
            DateTime fechaRegistro = new DateTime();
            if (DateTime.TryParseExact(Fecha_Registro.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultFecha))
            {
                fechaRegistro = resultFecha;
            }
            else
            {
                MessageBox.Show("Error, el formato de la fecha de publicacion es incorrecto.\nEl formato para la fecha es la siguiente 'dd/MM/yyyy'");
                return;
            }
            //De esas variables, se genera un modelo del Usuario
            UsuariosLibreria usuarioCapturado = new UsuariosLibreria();
            usuarioCapturado.Nombre_Usuario = nombreUsuario;
            usuarioCapturado.Fecha_Registro = fechaRegistro;
            usuarioCapturado.Folio_Credencial = folioCredencial;
            //Del modelo, se usa el metodo para borrar el usuario de la base de datos
            BorrarUsuarios(usuarioCapturado);
            //Al final, se reinicia la interfaz
            iniciarPresentacion();
        }

        //Para refrescar el listado de usuarios registrados
        private void Btn5_Click(object sender, RoutedEventArgs e)
        {
            //Se reinicia la interfaz y se muestran los usuarios de la base de datos
            iniciarPresentacion();
        }
        #endregion
    }
}

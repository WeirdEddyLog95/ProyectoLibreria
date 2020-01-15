using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
    /// Lógica de interacción para LibrosInfoView.xaml
    /// </summary>
    public partial class LibrosInfoView : Window
    {
        public LibrosInfoView()
        {
            InitializeComponent();
            inicarPresentacion();
        }

        /// <summary>
        /// Este metodo sirve para iniciar con la interfaz del Catalogo de Libros y hacer las siguientes 
        /// acciones 
        /// </summary>
        private void inicarPresentacion()
        {
            //Se comienza con activar y desactivar botones
            btn1.IsEnabled = true; //Se activa el boton para agregar un nuevo Libro
            btn2.IsEnabled = false; //Se desactiva el boton para capturar el Libro
            btn3.IsEnabled = false; //Se desactiva el boton para editar el Libro
            btn4.IsEnabled = false; //Se desactiva el boton para borrar el Libro
            btn5.IsEnabled = true; //Se activa el boton para refrescar el listado de Libros y reiniciar el presentacion

            //En esta seccion, se limpian los campos y se quedan desactivados, hasta que el usuario desee generar un
            //nuevo libro
            Titulo_Libro.Text = string.Empty;
            Titulo_Libro.IsEnabled = false;
            Autor_Libro.Text = string.Empty;
            Autor_Libro.IsEnabled = false;
            Fecha_Publicacion.Text = DateTime.Today.ToShortDateString();
            Fecha_Publicacion.IsEnabled = false;
            Clave_Genero.ItemsSource = null;
            Clave_Genero.IsEnabled = false;
            Clave_Pasillo.ItemsSource = null;
            Clave_Pasillo.IsEnabled = false;
            Clave_Libro.Text = string.Empty;
            Clave_Libro.IsEnabled = false;
            Estado.SelectedItem = "Disponible";
            Estado.IsEnabled = false;
            //Se presentan los libros que se han registrado en la base de datos en el DataGrid
            RefrescarLibros();
            //Se presentan los generos que se han registrado en un listview para el campo de Clave de Genero
            mostrarGeneros();
            //Se presentan todos los pasillos ordenados, que se han registrado en un listview en el campo de Clave_Pasillo
            mostrarPasillos();
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

        #region Metodos de la Interfaz para Capturar Libros
        ///<summary>
        ///Este metodo sirve para consultar y mostrar todos los libros que han registrado 
        ///en la base de datos y se llevan los datos al DataGrid
        ///</summary>
        private void RefrescarLibros()
        {
            //Se crea una conexion a la base de datos
            SqlConnection connection = CrearConexion();
            try
            {
                //Se abre la conexion
                connection.Open();
                //Generamos un comando con una consulta para buscar todos los libros de la base de datos
                SqlCommand cmd = new SqlCommand("SELECT Titulo_Libro, Autor_Libro, FORMAT (Fecha_Publicacion, 'dd/MM/yyyy') as Fecha_Publicacion, " +
                "Clave_Genero, Clave_Pasillo, Clave_Libro, Estado FROM Libros", connection);
                //Se crea un adaptador de datos para que se use el comando
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                //Se crea un nuevo conjunto de datos
                DataSet ds = new DataSet();
                //Llenamos el conjunto de datos con informacion del adaptador de datos
                sda.Fill(ds);
                //Revisamos si existen filas en la tabla del conjunto de datos
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //Pasamos la informacion del conjunto hacia el DataGrid de la interfaz
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

        ///<summary>
        ///Este metodo sirve para realizar dos operaciones, para guardar nuevos libros y actualizar libros 
        ///ya registrados en la base de datos con un modelo de Libro
        ///</summary>
        private void RegistrarLibros(Libros libroCapturado)
        {
            try
            {
                //Usamos una conexion de la base de datos para lo siguiente
                using (SqlConnection conexiones = CrearConexion())
                {
                    //Abrimos la conexion
                    conexiones.Open();
                    //Generamos una transaccion, para poner cambios a la base de datos
                    using (var trans = conexiones.BeginTransaction())
                    {
                        try
                        {
                            //Creamos un comando para las siguientes consultas
                            using (var cmd = conexiones.CreateCommand())
                            {
                                //Antes de las consultas, metemos la transaccion dentro del comando
                                cmd.Transaction = trans;
                                //Hacemos una consulta para revisar todos los libros y asegurarnos que el libro que vamos a introducir existe
                                string revisionLibro = string.Format("SELECT COUNT(*) FROM Libros Where Clave_Libro = '{0}'", libroCapturado.Clave_Libro);
                                //Ponemos la consulta para ejecutar
                                cmd.CommandText = revisionLibro;
                                Int32 conteo = Convert.ToInt32(cmd.ExecuteScalar());
                                if (conteo != 0)
                                {
                                    //Si existe el libro, solo se actualiza la informacion excepto la clave, que sera el identificador
                                    string queryActualizar = string.Format("UPDATE Libros SET Clave_Genero = '{0}', Clave_Pasillo = '{1}', Autor_Libro = '{2}', Titulo_Libro = '{3}', Fecha_Publicacion = '{4}', Estado = '{5}' " +
                                    "WHERE Clave_Libro = '{6}'", libroCapturado.Clave_Genero, libroCapturado.Clave_Pasillo, libroCapturado.Autor_Libro, libroCapturado.Titulo_Libro, libroCapturado.Fecha_Publicacion.ToString("yyyy-MM-dd"), libroCapturado.Estado, libroCapturado.Clave_Libro);
                                    cmd.CommandText = queryActualizar;
                                    cmd.ExecuteNonQuery();
                                    //Se hace enterar al usuario que el libro fue actualizado
                                    MessageBox.Show("Se ha actualizado los datos del libro");
                                }
                                else
                                {
                                    //Si no existe, solo se guardar el nuevo libro en la base de datos
                                    string queryRegistrar = string.Format("INSERT INTO Libros (Clave_Genero, Clave_Pasillo, Clave_Libro, Autor_Libro, Titulo_Libro, Fecha_Publicacion, Estado) " +
                                    "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')", libroCapturado.Clave_Genero, libroCapturado.Clave_Pasillo, libroCapturado.Clave_Libro, libroCapturado.Autor_Libro, libroCapturado.Titulo_Libro, libroCapturado.Fecha_Publicacion.ToString("yyyy-MM-dd"), libroCapturado.Estado);
                                    cmd.CommandText = queryRegistrar;
                                    cmd.ExecuteNonQuery();
                                    //Se hace enterar al usuario que el libro fue insertado en la base de datos
                                    MessageBox.Show("Se ha generado un nuevo libro");
                                }
                            }
                            //Al final se cometen los cambios dentro de la base de datos
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

        ///<summary>
        ///Este metodo sirve para realizar la eliminacion de libro seleccionado en la base de datos
        ///</summary>
        private void BorrarLibros(Libros libroCapturado)
        {
            try
            {
                //Usamos una conexion de la base de datos para lo siguiente
                using (SqlConnection conexiones = CrearConexion())
                {
                    //Abrimos la conexion
                    conexiones.Open();
                    //Generamos una transaccion, para poner este cambio en la base de datos
                    using (var trans = conexiones.BeginTransaction())
                    {
                        try
                        {
                            //Creamos un comando para la
                            using (var cmd = conexiones.CreateCommand())
                            {
                                //Antes de las consultas, metemos la transaccion dentro del comando
                                cmd.Transaction = trans;
                                //Hacemos una consulta para revisar todos los libros y asegurarnos que el libro que vamos a introducir existe
                                string revisionGenero = string.Format("SELECT COUNT(*) FROM Libros Where Clave_Libro = '{0}'", libroCapturado.Clave_Libro);
                                //Ponemos la consulta para ejecutar
                                cmd.CommandText = revisionGenero;
                                Int32 conteo = Convert.ToInt32(cmd.ExecuteScalar());
                                if (conteo != 0)
                                {
                                    //Si existe el libro, sera borrado de la base de datos
                                    string queryBorrar = string.Format("DELETE FROM Libros Where Clave_Libro = '{0}'", libroCapturado.Clave_Libro);
                                    cmd.CommandText = queryBorrar;
                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Se ha borrado el genero de la base de datos");
                                }
                            }

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

        ///<summary>
        ///Este metodo sirve para presentar la lista de clave de genero dentro de su respectivo listview
        ///</summary>
        private void mostrarGeneros()
        {
            List<string> items = new List<string>();
            items = presentarGeneros();
            Clave_Genero.ItemsSource = items;
        }

        ///<summary>
        ///Este metodo sirve para generar una lista con valores del clave de genero
        ///</summary>
        private List<string> presentarGeneros()
        {
            //Se crea una nueva lista para capturar la clave de genero
            List<string> listadoGeneros = new List<string>();

            //Se usa una nueva conexion a la base de datos
            using (SqlConnection conexion = CrearConexion())
            {
                //Se abre la conexion
                conexion.Open();
                //Se genera una consulta para las claves de genero
                string query = "SELECT Clave_Genero FROM Genero";
                //Se usa un comando con la consulta
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    //Se aplica un lector de datos para leer los valores de la consulta
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //mientras lea los datos lo agregamos a la lista
                        while (reader.Read())
                        {
                            listadoGeneros.Add(reader.GetString(0));
                        }
                    }
                }
            }
            //Se regresa el listado con las clavess o valores nulos al final
            return listadoGeneros;
        }

        ///<summary>
        ///Este metodo sirve para presentar la lista de pasillos dentro de su respectivo listview
        ///</summary>
        private void mostrarPasillos()
        {
            List<string> items = new List<string>();
            items = presentarPasillos();
            Clave_Pasillo.ItemsSource = items;
        }

        ///<summary>
        ///Este metodo sirve para generar una lista con valores del pasillo, de forma ordenada
        ///</summary>
        private List<string> presentarPasillos()
        {
            //Se crea una nueva lista para capturar los pasillos
            List<string> listadoPasillos = new List<string>();

            //Se usa una nueva conexion a la base de datos
            using (SqlConnection conexion = CrearConexion())
            {
                //Se abre la conexion
                conexion.Open();
                //Se usa una consulta para leer todos los pasillos, de forma ordenada
                string query = "SELECT * FROM Pasillos ORDER BY Id_Pasillo";
                //Ponemos la consulta en un comando SQL
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    //Leemos sus datos usando lector de datos 
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //Mientras lea datos, agregamos los pasillos a la lista
                        while (reader.Read())
                        {
                            listadoPasillos.Add(reader.GetString(0));
                        }
                    }
                }
            }
            //Regresamos la lista con datos o valores nulos al final
            return listadoPasillos;
        }

        ///<summary>
        ///Este metodo sirve para contar la cantidad de libros, y generar el siguiente numero
        ///para la clave del libro
        ///</summary>
        private string numeroLibro()
        {
            //Se crea un integer que servira para capturar el conteo de libros en la base de datos
            int numeroLibro = new int();
            //Usamos una nueva conexion a la base de datos
            using (SqlConnection conexion = CrearConexion())
            {
                //Abrimos la conexion
                conexion.Open();
                //Se hace una consulta
                string query = "SELECT COUNT(*) FROM Libros";
                //Ponemos la consulta en un comando
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    //Ejecutamos el comando
                    int conteo = Convert.ToInt32(command.ExecuteScalar());
                    //Se revisa si existe numeros en el conteo
                    if(conteo != 0)
                    {
                        //Si no tiene 0, se saca el valor y ponemos el siguiente numero de la clave del libro
                        int siguienteNo = conteo + 1;
                        numeroLibro = siguienteNo;
                    }
                    else
                    {
                        //Si tiene 0, se pone el valor inicial del numero de la clave de libro
                        int iniciarNo = 1;
                        numeroLibro = iniciarNo;
                    }
                }
            }
            //Al final hacemos el formato para tener 4 digitos en el numero del libro
            int longitud = 4;
            //Se regresa el siguiente formato
            return numeroLibro.ToString("D" + longitud);
        }
        #endregion

        #region Acciones de la Interfaz de Captura de Libros

        /// <summary>
        /// Esta accion sirve para iniciar a escribir datos para un nuevo Libro
        /// </summary>
        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            //Iniciamos limpiando los campos de datos del libro de la interfaz para comenzar a escribir un nuevo libro
            Titulo_Libro.Text = string.Empty;
            Titulo_Libro.IsEnabled = true;
            Autor_Libro.Text = string.Empty;
            Autor_Libro.IsEnabled = true;
            Fecha_Publicacion.Text = DateTime.Today.ToShortDateString();
            Fecha_Publicacion.IsEnabled = true;
            Clave_Genero.IsEnabled = true;
            Clave_Pasillo.IsEnabled = true;
            string siguienteNo = numeroLibro();
            Clave_Libro.Text = siguienteNo;
            Clave_Libro.IsReadOnly = true;
            Estado.SelectedItem = "Disponible";
            Estado.IsEnabled = true;
            //Se aplica el cambio de estados de los botones
            btn1.IsEnabled = false; //Se desactiva el boton para agregar un nuevo Libro
            btn2.IsEnabled = true; //Se activa el boton para guardar o actualizar el Libro
            btn3.IsEnabled = false; //Se desactiva el boton para sobreescribir el Libro
            btn4.IsEnabled = false; //Se desactiva el boton para eliminar el Libro
            btn5.IsEnabled = true; //Se activa el boton para refrescar la lista de Libros y reiniciar la presentacion
        }

        /// <summary>
        /// Esta accion sirve para capturar el libro y agregarlo a la base de datos
        /// </summary>
        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            //Se capturan los datos de libro de la interfaz y se ponen en variables
            string tituloLibro = string.Empty;
            tituloLibro = Titulo_Libro.Text;
            string autorLibro = string.Empty;
            autorLibro = Autor_Libro.Text;
            //Aqui se revisa el formato de la fecha de publicacion
            //string fechaPubLibro = string.Empty;
            DateTime revisionFecha = new DateTime();
            if (DateTime.TryParseExact(Fecha_Publicacion.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultFecha))
            {
                revisionFecha = resultFecha;
            }
            else
            {
                MessageBox.Show("Error, el formato de la fecha de publicacion es incorrecto.\nEl formato para la fecha es la siguiente 'dd/MM/yyyy'");
                return;
            }
            //
            string generoLibro = string.Empty;
            generoLibro = (string)Clave_Genero.SelectedItem;
            string valorcapturado = generoLibro;
            generoLibro = valorcapturado;
            string pasilloLibro = string.Empty;
            pasilloLibro = Clave_Pasillo.SelectedItem.ToString();
            string codigoLibro = string.Empty;
            codigoLibro = Clave_Libro.Text;
            string claveLibro = string.Empty;
            claveLibro = string.Format("{0}-{1}", generoLibro, codigoLibro);
            string estadoLibro = string.Empty;
            var listItem = (ListViewItem)Estado.SelectedItem;
            estadoLibro = (string)listItem.Content;
            //Despues de generar las variables, se colocan en sus respectivos lugares en el modelo de Libro
            Libros libroCapturado = new Libros();
            libroCapturado.Clave_Genero = generoLibro;
            libroCapturado.Clave_Pasillo = pasilloLibro;
            libroCapturado.Clave_Libro = claveLibro;
            libroCapturado.Autor_Libro = autorLibro;
            libroCapturado.Titulo_Libro = tituloLibro;
            libroCapturado.Fecha_Publicacion = revisionFecha;
            libroCapturado.Estado = estadoLibro;
            //Al tener generado un modelo, se aplica el metodo para guardar libros en la base de datos
            RegistrarLibros(libroCapturado);
            //Al finalizar el registro del libro, se reinicia la interfaz
            inicarPresentacion();
        }

        /// <summary>
        /// Esta accion sirve para seleccionar un libro en el DataGrid, para mostrarlo en los campos
        /// de llenado del libro, para modificar o borrar
        /// </summary>
        private void DataGrid1_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            //Se captura el itemSource del DataGrid
            var itemsSource = dataGrid1.Items;
            if (itemsSource != null)
            {
                //Se captura la fila de datos que se selecciono del Data Grid
                DataRowView dataRow = (DataRowView)dataGrid1.SelectedItem;
                //Sacamos el indice de la fila
                int index = dataGrid1.CurrentCell.Column.DisplayIndex;
                //Separamos los valores de la fila selecta en variables para los siete campos que usa el libro en la interfaz
                string tituloLibro = dataRow.Row.ItemArray[0].ToString();
                string autorLibro = dataRow.Row.ItemArray[1].ToString();
                string fechaPublicacion = dataRow.Row.ItemArray[2].ToString();
                string claveGenero = dataRow.Row.ItemArray[3].ToString();
                string clavePasillo = dataRow.Row.ItemArray[4].ToString();
                string claveLibro = dataRow.Row.ItemArray[5].ToString();
                string estadoLibro = dataRow.Row.ItemArray[6].ToString();
                //Los valores se colocan en sus propios campos de la interfaz
                Titulo_Libro.Text = tituloLibro;
                Autor_Libro.Text = autorLibro;
                Fecha_Publicacion.Text = fechaPublicacion.Replace('-', '/');
                Clave_Genero.SelectedItem = claveGenero;
                Clave_Pasillo.SelectedItem = clavePasillo;
                string[] separacionClaveLib = claveLibro.Split('-');
                string cLibro = separacionClaveLib[1];
                Clave_Libro.Text = cLibro;
                Estado.SelectedItem = estadoLibro;
                //Se aplica el cambio de estados de los botones
                btn1.IsEnabled = false; //Se desactiva el boton para agregar un nuevo Libro
                btn2.IsEnabled = false; //Se desactiva el boton para guardar y/o actualizar el Libro
                btn3.IsEnabled = true; //Se activa el boton para sobreescribir el Genero Libro
                btn4.IsEnabled = true; //Se activa el boton para eliminar el Genero Libro
                btn5.IsEnabled = true; //Se activa el boton para refrescar la lista de Libros y reiniciar la interfaz
            }
        }

        /// <summary>
        /// Esta accion sirve para modificar el libro seleccionado
        /// </summary>
        private void Btn3_Click(object sender, RoutedEventArgs e)
        {
            //Se activan los campos de Libro en la Interfaz
            Titulo_Libro.IsEnabled = true;
            Autor_Libro.IsEnabled = true;
            Fecha_Publicacion.IsEnabled = true;
            Clave_Genero.IsEnabled = true;
            Clave_Pasillo.IsEnabled = true;
            Clave_Libro.IsEnabled = true;
            Estado.IsEnabled = true;

            //Se aplica el cambio de estados de los botones
            btn1.IsEnabled = false; //Se desactiva el boton para agregar un nuevo Genero
            btn2.IsEnabled = true; //Se activa el boton para guardar y/o actualizar Genero
            btn3.IsEnabled = false; //Se desactiva el boton para modificar el Genero
            btn4.IsEnabled = false; //Se desactiva el boton para borrar el Genero
            btn5.IsEnabled = true; //Se activa el boton para actualizar el Genero
        }

        /// <summary>
        /// Esta accion sirve para borrar el libro seleccionado
        /// </summary>
        private void Btn4_Click(object sender, RoutedEventArgs e)
        {
            //Se capturan los datos de libro en el lado de la interfaz para generar variables para colocar en un modelo
            string tituloLibro = string.Empty;
            tituloLibro = Titulo_Libro.Text;
            string autorLibro = string.Empty;
            autorLibro = Autor_Libro.Text;
            string fechaPubLibro = string.Empty;
            fechaPubLibro = Fecha_Publicacion.Text;
            DateTime fechadia = Convert.ToDateTime(fechaPubLibro);
            fechadia.ToShortDateString();
            //string fechaLibro = fechadia.ToString("yyyy/MM/dd");
            string generoLibro = string.Empty;
            generoLibro = Clave_Genero.SelectedItem.ToString();
            string pasilloLibro = string.Empty;
            pasilloLibro = Clave_Pasillo.SelectedItem.ToString();
            string valorGenero = string.Empty;
            valorGenero = Clave_Genero.SelectedItem.ToString();
            string codigoLibro = string.Empty;
            codigoLibro = Clave_Libro.Text;
            string claveLibro = string.Empty;
            claveLibro = string.Format("{0}-{1}", valorGenero, codigoLibro);
            string estadoLibro = string.Empty;
            estadoLibro = Estado.SelectedItem.ToString();
            //Se usan las variables para crear un modelo del Libro
            Libros libroCapturado = new Libros();
            libroCapturado.Clave_Genero = generoLibro;
            libroCapturado.Clave_Pasillo = pasilloLibro;
            libroCapturado.Clave_Libro = claveLibro;
            libroCapturado.Autor_Libro = autorLibro;
            libroCapturado.Titulo_Libro = tituloLibro;
            libroCapturado.Fecha_Publicacion = fechadia;
            libroCapturado.Estado = estadoLibro;
            //Del modelo, se usa un metodo para borrar el libro dentro de la base de datos
            BorrarLibros(libroCapturado);
            //Al final la eliminacion del libro, se reinicia la interfaz
            inicarPresentacion();
        }

        /// <summary>
        /// Esta accion sirve para refrescar la lista de Libros y reiniciar la interfaz
        /// </summary>
        private void Btn5_Click(object sender, RoutedEventArgs e)
        {
            //Este metodo sirve para refrescar la lista de libros registrados, asi como reiniciar la interfaz
            inicarPresentacion();
        }
        #endregion
    }
}

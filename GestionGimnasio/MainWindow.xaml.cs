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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Data;
using System.Windows.Media.Animation;
using Microsoft.Win32;


namespace GestionGimnasio
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;
        public MainWindow()
        {
            InitializeComponent();
            String connectionString = ConfigurationManager.ConnectionStrings["GestionGimnasio.Properties.Settings."
            + "GestionarGimnasioConnectionString"].ConnectionString;

            sqlConnection = new SqlConnection(connectionString);

            MuestraGimnasios();
            MuestraMaquinas();
        }

        private void MuestraGimnasios(){
            try
            {
                string consulta = "select * from Gimnasio";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable gimnasioTable = new DataTable();
                    sqlDataAdapter.Fill(gimnasioTable);
                    ListaGimnasios.DisplayMemberPath = "Nombre";
                    ListaGimnasios.SelectedValuePath = "Id";
                    ListaGimnasios.ItemsSource = gimnasioTable.DefaultView;
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        private void MuestraMaquinasAsocidas()
        {
            try
            {
                string consulta = "select * from Maquina a Inner Join " +
                "GimnasioMaquina az on a.Id=az.MaquinaId where az.GimnasioId=@GimnasioId";
                SqlCommand sqlCommand = new
                SqlCommand(consulta, sqlConnection);
                
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                using (sqlDataAdapter)
                {
                sqlCommand.Parameters.AddWithValue("@GimnasioId", ListaGimnasios.SelectedValue);
                    DataTable MaquinaTabla = new DataTable();
                    sqlDataAdapter.Fill(MaquinaTabla);
                    ListaMaquinasAsociadas.DisplayMemberPath = "nombreMaquina";
                    ListaMaquinasAsociadas.SelectedValuePath = "Id";
                    ListaMaquinasAsociadas.ItemsSource = MaquinaTabla.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void ListaGimnasios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MuestraMaquinasAsocidas();
            MuestraZooElegidoEnTextBox();
        }

        private void ListaMaquinas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MuestraMaquinaElegidoEnTextBox();
        }

        private void MuestraMaquinas()
        {
            try
            {
                string consulta = "Select * from Maquina";
                SqlDataAdapter sqlDataAdapter = new
                SqlDataAdapter(consulta, sqlConnection);
                using (sqlDataAdapter)
                {
                    DataTable maquinaTabla = new DataTable();
                    sqlDataAdapter.Fill(maquinaTabla);

                    ListaMaquinas.DisplayMemberPath = "nombreMaquina";
                    ListaMaquinas.SelectedValuePath = "Id";
                    ListaMaquinas.ItemsSource = maquinaTabla.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void EliminarGym_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Delete from Gimnasio where id=@GimnasioId";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@GimnasioId", ListaGimnasios.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            finally
            {
                MuestraGimnasios();
                sqlConnection.Close();
            }
        }

        private void AgregarGym_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Insert into Gimnasio values (@Nombre)";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Nombre", miTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            finally
            {
                MuestraGimnasios();
                sqlConnection.Close();//Cerramos la conexión
            }
        }

        private void AgregarMaquinaGym_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Insert into GimnasioMaquina values (@GimnasioId, @MaquinaId)";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();

                sqlCommand.Parameters.AddWithValue("@GimnasioId", ListaGimnasios.SelectedValue);

                sqlCommand.Parameters.AddWithValue("@MaquinaId", ListaMaquinas.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                MuestraMaquinasAsocidas();
                sqlConnection.Close();
            }

        }

        private void EliminarMaquina_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Delete from Maquina where id=@MaquinaId";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@MaquinaId", ListaMaquinas.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            finally
            {
                MuestraMaquinas();
                sqlConnection.Close();//Cerramos la conexión
            }
        }

        private void AgregarMaquina_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Insert into Maquina values (@nombreMaquina)";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@nombreMaquina", miTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            finally
            {
                MuestraMaquinas() ;
                sqlConnection.Close();//Cerramos la conexión
            }
        }

        private void MuestraZooElegidoEnTextBox()
        {
            try
            {
                string consulta = "select Nombre from Gimnasio where Id = @GimnasioId";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@GimnasioId",ListaGimnasios.SelectedValue);
                    DataTable GimnasioDataTabla = new DataTable();
                    sqlDataAdapter.Fill(GimnasioDataTabla);
                    miTextBox.Text = GimnasioDataTabla.Rows[0]["Nombre"].ToString();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void MuestraMaquinaElegidoEnTextBox()
        {
            try
            {
                string consulta = "select nombreMaquina from Maquina where Id = @MaquinaId";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@MaquinaId", ListaMaquinas.SelectedValue);
                    DataTable MaquinaDataTabla = new DataTable();
                    sqlDataAdapter.Fill(MaquinaDataTabla);
                    miTextBox.Text = MaquinaDataTabla.Rows[0]["nombreMaquina"].ToString();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void ActualizarGym_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Update Gimnasio Set Nombre=@Nombre where Id = @GimnasioId";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@GimnasioId", ListaGimnasios.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Nombre", miTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                MuestraGimnasios();
                sqlConnection.Close();//Cerramos la conexión
            }

        }

        private void ActualizarMaquina_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Update Maquina Set nombreMaquina=@nombreMaquina where Id = @MaquinaId";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@MaquinaId", ListaMaquinas.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@nombreMaquina", miTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                MuestraMaquinas();
                sqlConnection.Close();//Cerramos la conexión
            }

        }

        private void QuitarMaquina_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Delete from Maquina where id=@MaquinaId";
                SqlCommand sqlCommand = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@MaquinaId", ListaMaquinas.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            finally
            {
                MuestraMaquinas();
                sqlConnection.Close();
            }
        }
    }
}

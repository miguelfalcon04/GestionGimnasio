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
                    ListaMaquinasAsociadas.DisplayMemberPath = "Nombre";
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
        }
    }
}

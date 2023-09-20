using MySqlConnector;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace WpfKutyak
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string leiro = "datasource=127.0.0.1;port=3306;username=root;password=;database=kutyak";
        MySqlConnection kapcsolat;

        public MainWindow()
        {
            InitializeComponent();
            kapcsolat = new MySqlConnection(leiro);
            kapcsolat.Open();
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand parancs = new MySqlCommand("SELECT COUNT(*) FROM nevek;", kapcsolat);
            MySqlDataReader olvaso = parancs.ExecuteReader();

            olvaso.Read();
            MessageBox.Show($"Kutyanevek száma: {olvaso.GetInt32(0)}");
            olvaso.Close();
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand parancs = new MySqlCommand("SELECT AVG(életkor) FROM kutyak;", kapcsolat);
            MySqlDataReader olvaso = parancs.ExecuteReader();

            olvaso.Read();
            MessageBox.Show($"Kutyák átlag életkora: {Math.Round(olvaso.GetDouble(0), 2)}");
            olvaso.Close();
        }

        private void btn7_Click(object sender, RoutedEventArgs e)
        {  
            string p = "SELECT nevek.kutyanév, fajtak.név FROM kutyak INNER JOIN nevek ON név_id = nevek.id INNER JOIN fajtak ON fajta_id = fajtak.id ORDER BY életkor DESC LIMIT 1;";
            MySqlCommand parancs = new MySqlCommand(p, kapcsolat);
            MySqlDataReader olvaso = parancs.ExecuteReader();

            olvaso.Read();
            MessageBox.Show($"Legidősebb kutya neve és fajtája: {olvaso.GetString(0)}, {olvaso.GetString(1)}");
            olvaso.Close();
        }

        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            lb8.Items.Clear();
            string p = "SELECT fajtak.név, COUNT(*) FROM kutyak INNER JOIN fajtak ON fajta_id = fajtak.id WHERE utolsó_orvosi_ellenőrzés = \"2018.01.10\" GROUP BY fajtak.név;";
            MySqlCommand parancs = new MySqlCommand(p, kapcsolat);
            MySqlDataReader olvaso = parancs.ExecuteReader();

            while (olvaso.Read())
            {
                lb8.Items.Add($"{olvaso.GetString(0)}: {olvaso.GetInt32(1)} kutya");
            }
            olvaso.Close();
        }

        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            string p = "SELECT utolsó_orvosi_ellenőrzés, COUNT(*) FROM kutyak GROUP BY utolsó_orvosi_ellenőrzés ORDER BY 2 DESC LIMIT 1;";
            MySqlCommand parancs = new MySqlCommand(p, kapcsolat);
            MySqlDataReader olvaso = parancs.ExecuteReader();

            olvaso.Read();
            MessageBox.Show($"Legjobban leterhelt nap: {olvaso.GetString(0)}, {olvaso.GetInt32(1)} kutya");
            olvaso.Close();
        }

        private void btn10_Click(object sender, RoutedEventArgs e)
        {
            string p = "SELECT nevek.kutyanév, COUNT(*) FROM kutyak INNER JOIN nevek ON név_id = nevek.id GROUP BY nevek.kutyanév ORDER BY 2 DESC";
            MySqlCommand parancs = new MySqlCommand(p, kapcsolat);
            MySqlDataReader olvaso = parancs.ExecuteReader();

            List<string> sorok = new List<string>();
            while (olvaso.Read())
            {
                sorok.Add($"{olvaso.GetString(0)};{olvaso.GetInt32(1)}");
            }
            olvaso.Close();

            File.WriteAllLines("névsztatisztika.txt", sorok);
            MessageBox.Show("névsztatisztika.txt");
        }
    }
}

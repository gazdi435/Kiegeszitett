using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.IO;
using System.Collections.ObjectModel;
using minikozfelvir.Classok;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using System.Windows.Documents;
using System.Threading.Tasks;

namespace minikozfelvir
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        private static readonly ObservableCollection<Diak> adatokList = new();
        private readonly MySqlConnection connection;

        public MainWindow()
        {
            InitializeComponent();

            dgMain.ItemsSource = adatokList;
            dgMain.SelectionMode = DataGridSelectionMode.Extended;
            connection = new(SqlHelper.ConnectionString);
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new()
            {
                Title = "Betöltés",
                Filter = "CSV fájl (*.csv)|*.csv|Json file (*.json)|*.json",
                InitialDirectory = @"C:\"
            };
            if (ofd.ShowDialog() is true)
            {
                if (adatokList.Count > 0)
                {
                    if (MessageBox.Show("Szeretné törölni az eddig tárolt adatokat?", "Választás", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        adatokList.Clear();
                }
                switch (Path.GetExtension(ofd.FileName).ToLower())
                {
                    case ".json":
                        string segedString = "";
                        File.ReadAllLines(ofd.FileName).ToList().ForEach(x => segedString += x);
                        //adatokList.Add(JsonSerializer.Deserialize<Diak>(File.ReadAllText(ofd.FileName)));
                        //segedString[0] = "";
                        MessageBox.Show(segedString);

                        break;
                    case ".csv":
                        File.ReadAllLines(ofd.FileName).ToList().ForEach(x => adatokList.Add(new Diak(x.Split(';'))));
                        break;
                    default:
                        break;
                }

            }
        }

        private void Torol_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgMain.SelectedIndex <= 0)
                    throw new Exception("A művelethez legalább egy kijelölt elemnek kell lenni!");
                MessageBoxResult msgbValasz = MessageBox.Show("Biztosan törölni szeretné?", "Megerősítés", MessageBoxButton.YesNo);
                if (msgbValasz == MessageBoxResult.Yes)
                {
                    List<Diak> newList = new();
                    foreach (Diak torolniKivantSor in dgMain.SelectedItems)
                    {
                        newList.Add(torolniKivantSor);
                    }

                    foreach (Diak torolniKivantDiak in newList)
                    {
                        adatokList.Remove(torolniKivantDiak);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UjDiak_Click(object sender, RoutedEventArgs e)
        {
            Diak ujDiak = new();
            DiakFelvevo ujDiakFelvevo = new(ujDiak, Top, Left);
            try
            {
                ujDiakFelvevo.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            adatokList.Add(ujDiak);
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (adatokList.ToList().Count <= 0)
                    throw new Exception("Hiba! Adatok nélkül nem lehet exportálni!");
                SaveFileDialog sfd = new()
                {
                    Title = "Mentés",
                    FileName = "Mentett fájl",
                    Filter = "CSV fájl (.csv)|*.csv|Json file|*.json"
                };
                if (sfd.ShowDialog() == true)
                {
                    if (adatokList.ToList().Count <= 0)
                        throw new Exception("Hiba! Adatok nélkül nem lehet exportálni!");
                    if (File.Exists(sfd.FileName))
                    {
                        File.Delete(sfd.FileName);
                    }

                    switch (Path.GetExtension(sfd.FileName).ToLower())
                    {
                        case ".json":
                            adatokList.ToList().ForEach(x => File.AppendAllText(sfd.FileName, JsonSerializer.Serialize(x, options) + "\n"));
                            break;
                        case ".csv":
                            adatokList.ToList().ForEach(x => File.AppendAllText(sfd.FileName, x.ToString() + "\n"));
                            break;
                        default:
                            break;
                    }
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void MeretValtozas(object sender, SizeChangedEventArgs e)
        {
            this.Width = this.Height * 1.69811320754717;
        }

        private void Szerkeszt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgMain.SelectedItems.Count > 1)
                    throw new Exception("Hiba! Csak egy sort jelöljön ki a szerkeztéshez!");
                if (dgMain.SelectedItems.Count < 1)
                    throw new Exception("Hiba! Jelöljön ki egy sort!");
                Diak ujDiak = dgMain.SelectedItem as Diak;
                DiakFelvevo ujDiakFelvevo = new(ujDiak, Top, Left);
                ujDiakFelvevo.ShowDialog();
                dgMain.SelectedItem = ujDiak;


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private async void Sql_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Importálni szeretnél(Igen) vagy exportálni(Nem)?", "", MessageBoxButton.YesNoCancel);

            if (res == MessageBoxResult.Yes)
            {
                if (await SqlImport())
                    MessageBox.Show("Adatok sikeresen beimportálva.");
                else
                    MessageBox.Show("Adatok beimportálása sikertelen.");
            }

            if(res == MessageBoxResult.No)
            {
                if(await SqlExportAsync())
                    MessageBox.Show("Adatok sikeresen exportálva.");
                else
                    MessageBox.Show("Adatok exportálása sikertelen.");
            }
        }

        private async Task<bool> SqlImport()
        {
            List<Diak> list = await connection.Import();
            try
            {
                if(list.Count == 0)
                    throw new Exception("Hiba! Az adatbázis üres!");
                if (adatokList.Count > 0)
                {
                    if (MessageBox.Show("Szeretné törölni az eddig tárolt adatokat?", "Választás", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        adatokList.Clear();
                }
                list.ForEach(G => adatokList.Add(G));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.Close();
                return false;
            }
            return true;
        }

        private async Task<bool> SqlExportAsync()
        {
            try
            {
                if (adatokList.Count == 0)
                    throw new Exception("Hiba! Adatok nélkül nem lehet exportálni!");
                await connection.Export(adatokList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.Close();
                return false;
            }
            return true;
        }
    }
}
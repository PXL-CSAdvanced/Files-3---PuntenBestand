using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Files_3___PuntenBestand;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    // Dialoogvenster op voorhand aanmaken (gebruiken we in meerdere event procedures)
    private OpenFileDialog ofd = new OpenFileDialog()
    {
        Filter = "Csv bestanden (*.csv) |*.csv|Tekstbestanden (*.txt) |*.txt|Alle bestanden (*.*)|*.*",
        InitialDirectory = System.IO.Path.GetFullPath(@"..\..\Bestanden"),
        Title = "Bestand openen",
        FileName = "Punten.csv",
        AddExtension = true,
        DefaultExt = "csv"
    };

    private List<PuntenAdmin> pntAdmin = new List<PuntenAdmin>();

    public MainWindow()
    {
        InitializeComponent();
    }

    private void readButton_Click(object sender, RoutedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        if (ofd.ShowDialog() != true)
        {
            MessageBox.Show("File does not exist",
                    "Error message",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            return;
        }

        string fileName = ofd.FileName;
        FileInfo fi = new FileInfo(fileName);
        if (!fi.Exists)
        {
            return;
        }

        using (StreamReader sr = fi.OpenText())
        {
            while (!sr.EndOfStream)
            {
                PuntenAdmin ad = new PuntenAdmin();
                string[] lines = sr.ReadLine().Split(';'); // CSV-file
                string[] name = lines[0].Trim().Split(' ');
                ad.Familyname = name[0].Trim();
                ad.Firstname = name[1].Trim();
                ad.Email = lines[1].Trim();
                ad.Gender = lines[2].Trim();

                string score = lines[3].Substring(0, 3).Trim();
                string total = lines[3].Substring(3, 3).Trim();
                ad.Score = int.Parse(score);
                ad.TotalScore = int.Parse(total);

                sb.Append($"{(ad.Firstname + ' ' + ad.Familyname),-21}{ad.Email,-21}");
                sb.Append($"{ad.Gender,-5}{ad.Score}/{ad.TotalScore}".Replace("\"", ""));
                sb.AppendLine();

                pntAdmin.Add(ad);
            }
        }
        transformButton.IsEnabled = true;
        resultTextBox.Text = sb.ToString();
    }

    private void transformButton_Click(object sender, RoutedEventArgs e)
    {
        SaveFileDialog sfd = new SaveFileDialog
        {
            Filter = "Alle bestanden (*.*)|*.*| CSV bestanden (*.csv)|*.csv| Tekstbestanden (*.txt)|*.txt",
            FilterIndex = 2,
            InitialDirectory = System.IO.Path.GetFullPath(@"..\..\Bestanden"),
            Title = "Geef een bestandsnaam op",
            FileName = "PuntenVerwerkt.csv",
            // Bevestiging vragen bij overschrijven van een bestand
            OverwritePrompt = true
        };

        if (sfd.ShowDialog() != true)
        {
            MessageBox.Show("De resultaten zijn niet verwerkt.",
                "Resultaten",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            return;
        }

        // Bestand opslaan
        string fileName = sfd.FileName;
        using (StreamWriter sw = File.CreateText(fileName))
        {
            // Gegevens wegschrijven
            foreach (var item in pntAdmin)
            {
                sw.WriteLine($"{item.Familyname + ' ' + item.Firstname,-50}{item.Percent(),-10:p}{item.Grade(),-10}");
            }
        }

        MessageBox.Show($"De resultaten zijn verwerkt. Je kan de resultaten nalezen in het bestand {fileName}",
            "Resultaten",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
        checkButton.IsEnabled = true;
    }

    private void checkButton_Click(object sender, RoutedEventArgs e)
    {
        ofd.ShowDialog();
        string fileName = ofd.FileName;

        FileInfo fi = new FileInfo(fileName);
        if (!fi.Exists)
        {
            resultTextBox.Clear();
            MessageBox.Show($"Bestand {System.IO.Path.GetFileName(fileName)} bestaat niet",
                "Foutmelding",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        using (StreamReader sr = fi.OpenText())
        {
            resultTextBox.Text = sr.ReadToEnd();
        }
    }

    private void closeButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
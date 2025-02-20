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
    private Dictionary<string, float> _resultList = new Dictionary<string, float>();

    public MainWindow()
    {
        InitializeComponent();
    }

    private void readButton_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog()
        {
            Filter = "Alle bestanden|*.*|Tekstbestanden|*.txt",
            FilterIndex = 2,
            InitialDirectory = Environment.CurrentDirectory, // onder bin\Debug
            Title = "Bestand openen",
            AddExtension = true,
            DefaultExt = "txt",
            FileName = "Punten.txt"
        };

        if (ofd.ShowDialog() == false)
        {
            return; // Indien we niet op Openen klikken
        }

        StringBuilder sb = new StringBuilder();
        using (StreamReader sr = new StreamReader(ofd.FileName))
        {
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string name = line.Substring(0, 18).Trim();
                string mail = line.Substring(19, 20).Trim();
                string gender = line.Substring(40, 1); // Geen Trim() nodig, want 1 karakter maar
                string pointsNotation = line.Substring(48, 6).Trim();
                sb.AppendLine($"{name,-21}{mail,-21}{gender,-5}{pointsNotation,-9}");

                float point = float.Parse(pointsNotation.Substring(0, 3)); // eerste 3 cijfers
                float max = float.Parse(pointsNotation.Substring(3, 3)); // laatste 3 cijfers
                float points = point / max; // kommagetal van 0 tot 1
                _resultList[name] = points; // puntenlijst.Add(naam, punten);
            }
        }
        // De TextBox gaat enkel en alleen mooie uitgelijnde kolommen hebben
        // wanneer we de FontFamily instellen op een monospaced lettertype!!! vb/ Consolas
        // (FontFamily="Consolas")
        resultTextBox.Text = sb.ToString();
        transformButton.IsEnabled = true;
    }

    private void transformButton_Click(object sender, RoutedEventArgs e)
    {
        SaveFileDialog sfd = new SaveFileDialog()
        {
            Filter = "Alle bestanden (*.*)|*.*|Tekstbestanden (*.txt)|*.txt", // keuze uit deze bestandstypes
            FilterIndex = 2, // kies het 2de (tekstbestanden) uit bovenstaande filter standaard aangeduid
            Title = "Geef een bestandsnaam op", // titel
            OverwritePrompt = true,  // bevestiging vragen bij overschrijven van bestand 
            AddExtension = true, // extensie wordt toegevoegd aan bestand
            DefaultExt = "txt", // standaardextensie die wordt toegevoegd is txt
            FileName = "PuntenVerwerkt.txt", // bestandsnaam van bestand dat je wil opslaan
            InitialDirectory = Environment.CurrentDirectory // onder bin\Debug
        };

        if (sfd.ShowDialog() == false)
        {
            return; // Indien we niet op Opslaan klikken
        }

        using (StreamWriter sw = new StreamWriter(sfd.FileName))
        {
            foreach (var paar in _resultList)
            {
                string name = paar.Key;
                float points = paar.Value;
                string grade = (points >= 0.85f) ? "Geslaagd" : "Niet Geslaagd";

                // :P zet kommagetal tussen 0 en 1 om naar percentage met 2 cijfers achter de komma
                sw.WriteLine($"{name,-24}{points,-10:P}{grade,-20}");
            }
        }
        checkButton.IsEnabled = true;

        MessageBox.Show($"De resultaten zijn verwerkt. Je kan de resultaten nalezen in het bestand {sfd.FileName}",
            "Resultaten",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private void checkButton_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog()
        {
            Filter = "Alle bestanden (*.*)|*.*|Tekstbestanden (*.txt)|*.txt",
            FilterIndex = 2,
            InitialDirectory = Environment.CurrentDirectory,
            Title = "Bestand openen",
            AddExtension = true,
            DefaultExt = "txt",
            FileName = "PuntenVerwerkt.txt"
        };

        resultTextBox.Clear();
        if (ofd.ShowDialog() == false)
        {
            return; // Indien we niet op Openen klikken
        }

        FileInfo fi = new FileInfo(ofd.FileName);
        if (fi.Exists)
        {
            string text;
            using (StreamReader sr = fi.OpenText())
            {
                text = sr.ReadToEnd();
            }
            resultTextBox.Text = text;
        }
        else
        {
            MessageBox.Show($"Bestand {System.IO.Path.GetFileName(ofd.FileName)} bestaat niet",
                "Foutmelding",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }

    private void closeButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
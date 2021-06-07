using ControlzEx.Theming;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace Communicator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        static SerialPort _serialPort;
        string recieved_data;
        bool specialEvent = false;

        public MainWindow()
        {
            InitializeComponent();

            LoadSettings();
            Terminal.FontSize = terminalFontSize;
            if (terminalFontBold) { Terminal.FontWeight = FontWeights.Bold; } else { Terminal.FontWeight = FontWeights.Normal; }
            ThemeManager.Current.ChangeTheme(this, themeMode + "." + themeColor);
            _serialPort = new SerialPort();

        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings window = new Communicator.Settings(this);
            var settings = window.ShowDialog();

        }        

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveFile = new SaveFileDialog();
            TextRange range;
            StreamWriter writer;

            range = new TextRange(Terminal.Document.ContentStart, Terminal.Document.ContentEnd);
            string testi = range.Text;

            saveFile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFile.FilterIndex = 1;
            saveFile.RestoreDirectory = true;
            saveFile.CreatePrompt = true;
            saveFile.OverwritePrompt = true;
            saveFile.FileName = "log.txt";
            saveFile.DefaultExt = "txt";
            RichTextBox asd = Terminal;

            bool? result = saveFile.ShowDialog();

            if (result != null && result == true)
            {
                writer = new StreamWriter(saveFile.FileName);
                writer.Write(testi);
                writer.Close();
            }
        }

        private void tbSend_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                CommunicateOut();
            }
        }

        private void btnSend_Click(object sender = null, RoutedEventArgs e = null)
        {
            CommunicateOut();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            Terminal.SelectAll();
            Terminal.Selection.Text = "";
        }


    }
}

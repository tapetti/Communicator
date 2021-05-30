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


        private void LoadSettings()
        {
            DataGeneralSettings settings = new DataGeneralSettings();
            XmlManager reader = new XmlManager();

            settings = reader.XmlDataSettingsReader(AppDomain.CurrentDomain.BaseDirectory + @"\Configuration\AppSettings.xml");

            try
            {
                themeMode = settings.themeMode;
                themeColor = settings.themeColor;
                Terminal_Color_PC = settings.Terminal_Color_PC;
                Terminal_Color_Rec = settings.Terminal_Color_Rec;
                terminalFontSize = settings.terminalFontSize;
                terminalFontBold = settings.terminalFontBold;
            }
            catch
            {
                themeMode = "Light";
                themeColor = "Green";
                Terminal_Color_PC = Colors.Red;
                Terminal_Color_Rec = Colors.Blue;
                terminalFontSize = 8;
                terminalFontBold = false;
            }

            string[] availablePorts = SerialPort.GetPortNames();

            foreach (string port in availablePorts)
            {
                tbPorts.Items.Add(port);
            }

            tbParitity.Items.Add("None"); tbParitity.Items.Add("Even"); tbParitity.Items.Add("Odd"); tbParitity.Items.Add("Space"); tbParitity.Items.Add("Mark");
            tbStopBits.Items.Add("1"); tbStopBits.Items.Add("1.5"); tbStopBits.Items.Add("2");
            tbHandshake.Items.Add("None"); tbHandshake.Items.Add("RTS"); tbHandshake.Items.Add("RTS XON/XOFF"); tbHandshake.Items.Add("XON/XOFF");
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

        private void CommunicateOut()
        {
            Terminal.AppendText(">> " + tbSend.Text + "\r", Terminal_Color_PC.ToString());
            SerialCmdSend(tbSend.Text);
            tbSend.Clear();
            tbSend.Focus();

            
        }

        private void SerialCmdSend(string text)
        {
            if (_serialPort.IsOpen)
            {
                try
                {
                    byte[] hexstring = Encoding.ASCII.GetBytes(text);
                    
                    foreach (byte hexval in hexstring)
                    {
                        byte[] _hexval = new byte[] { hexval }; // need to convert byte to byte[] to write
                        _serialPort.Write(_hexval, 0, 1);
                        Thread.Sleep(1);
                    }
                }
                catch (Exception ex)
                {
                    Terminal.AppendText("---ERROR IN SENDING---\r", "Red");
                }
            }
            else
            {
                Terminal.AppendText("---CONNECTION IS NOT OPEN---\r", "Red");
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            Terminal.SelectAll();
            Terminal.Selection.Text = "";
        }

        private void btnOpenConnection_Click(object sender, RoutedEventArgs e)
        {

            if (!_serialPort.IsOpen)
            {

                _serialPort.PortName = tbPorts.SelectedValue.ToString();
                _serialPort.BaudRate = Int32.Parse(tbBaud.Text);
                _serialPort.ReadTimeout = 500;
                _serialPort.WriteTimeout = 500;

                switch (tbParitity.Text)
                {

                    case "None":
                        _serialPort.Parity = Parity.None;
                        break;
                    case "Even":
                        _serialPort.Parity = Parity.Even;
                        break;
                    case "Mark":
                        _serialPort.Parity = Parity.Mark;
                        break;
                    case "Odd":
                        _serialPort.Parity = Parity.Odd;
                        break;
                    case "Space":
                        _serialPort.Parity = Parity.Space;
                        break;
                }

                switch (tbStopBits.Text)
                {
                    case "1":
                        _serialPort.StopBits = StopBits.One;
                        break;
                    case "1.5":
                        _serialPort.StopBits = StopBits.OnePointFive;
                        break;
                    case "2":
                        _serialPort.StopBits = StopBits.Two;
                        break;
                }

                switch (tbHandshake.Text)
                {
                    case "None":
                        _serialPort.Handshake = Handshake.None;
                        break;
                    case "RTS":
                        _serialPort.Handshake = Handshake.RequestToSend;
                        break;
                    case "RTS XON/XOFF":
                        _serialPort.Handshake = Handshake.RequestToSendXOnXOff;
                        break;
                    case "XON/XOFF":
                        _serialPort.Handshake = Handshake.XOnXOff;
                        break;
                }

                _serialPort.DataBits = Int32.Parse(tbDataBits.Text);

                _serialPort.Open();

                _serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Recieve);


                var converter = new System.Windows.Media.BrushConverter();
                var brush = (Brush)converter.ConvertFromString("#90EE90");                

                btnOpenConnection.Content = "Close connection";                
                btnOpenConnection.Background = brush;

            }
            else
            {
                var converter = new System.Windows.Media.BrushConverter();
                var brush = (Brush)converter.ConvertFromString("#FFFFA07A");

                btnOpenConnection.Content = "Open connection";
                btnOpenConnection.Background = brush;
                _serialPort.Close();
            }

        }

        private delegate void UpdateUiTextDelegate(string text);
        private void Recieve(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            recieved_data = _serialPort.ReadExisting();
            Dispatcher.Invoke(DispatcherPriority.Send, new UpdateUiTextDelegate(WriteData), recieved_data);
        }
        private void WriteData(string text)
        {
            Terminal.AppendText("<< " + text + "\r", Terminal_Color_Rec.ToString());
        }


        private void ClosePort(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }
    }
}

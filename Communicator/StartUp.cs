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
    public partial class MainWindow : MetroWindow
    {
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

                try
                {
                    tbTimeout.Text = settings.Timeout.ToString();
                    tbBaud.Text = settings.BaudRate.ToString();
                    tbDataBits.Text = settings.DataBits.ToString();
                }
                catch
                {
                    tbTimeout.Text = "500";
                    tbBaud.Text = "8";
                    tbDataBits.Text = "19200";
                }
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
    }
}

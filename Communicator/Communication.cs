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
        private delegate void UpdateUiTextDelegate(string text);


        #region Open/Close Connection
        private void ClosePort(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
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

                if (!string.IsNullOrEmpty(tbSpecial.Text))
                {
                    ConnectionSpecial();
                }

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
                specialEvent = false;
            }

        }

        private void ConnectionSpecial()
        {
            /*if (!specialEvent)
            {
                string command = tbSpecial.Text;

                if(command == "\n") {
                    while (!specialEvent)
                    {
                        SerialCmdSend(Key.Enter.ToString());
                    }
                }
            }*/
        }
        #endregion


        #region Read Data
        private void Recieve(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(1000);
            recieved_data = _serialPort.ReadExisting();
            Dispatcher.Invoke(DispatcherPriority.Send, new UpdateUiTextDelegate(WriteData), recieved_data);
        }
        private void WriteData(string text)
        {
            Terminal.AppendText("<< " + text + "\r", Terminal_Color_Rec.ToString());
            Terminal.ScrollToEnd();

            if (!specialEvent)
            {
                specialEvent = true;
            }
        }
        #endregion

        #region Send Data
        private void CommunicateOut()
        {
            Terminal.AppendText(">> " + tbSend.Text + "\r", Terminal_Color_PC.ToString());
            SerialCmdSend(tbSend.Text);
            Terminal.ScrollToEnd();
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
        #endregion
        
    }
}

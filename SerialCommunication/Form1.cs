using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SerialCommunication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();
                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;

                comboBoxBaudrate.SelectedIndex = comboBoxBaudrate.Items.IndexOf("115200");
            }
            catch (Exception)
            { }
        }

        private void cboPoort_DropDown(object sender, EventArgs e)
        {
            try
            {
                string selected = (string)comboBoxPoort.SelectedItem;
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();

                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);

                comboBoxPoort.SelectedIndex = comboBoxPoort.Items.IndexOf(selected);
            }
            catch (Exception)
            {
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            // Connect or disconnect from the selected serial port
            if (!serialPortArduino.IsOpen)
            {
                // Validate selection
                if (comboBoxPoort.SelectedItem == null)
                {
                    labelStatus.Text = "Kies eerst een COM-poort";
                    return;
                }

                // Apply serial settings before opening
                serialPortArduino.PortName = comboBoxPoort.SelectedItem.ToString();
                int baud;
                if (!int.TryParse(comboBoxBaudrate.Text, out baud)) baud = 115200;
                serialPortArduino.BaudRate = baud;
                serialPortArduino.DataBits = 8;
                serialPortArduino.Parity = Parity.None;
                serialPortArduino.StopBits = StopBits.One;
                serialPortArduino.Handshake = Handshake.None;
                serialPortArduino.RtsEnable = true;
                serialPortArduino.DtrEnable = true;

                try
                {
                    serialPortArduino.Open();
                    radioButtonVerbonden.Checked = true;
                    buttonConnect.Text = "Disconnect";
                    labelStatus.Text = $"Verbonden met {serialPortArduino.PortName}";
                }
                catch (UnauthorizedAccessException uaEx)
                {
                    // Port is in use or access denied
                    labelStatus.Text = $"Toegang geweigerd tot {serialPortArduino.PortName}: {uaEx.Message}";
                    try { if (serialPortArduino.IsOpen) serialPortArduino.Close(); } catch { }
                    radioButtonVerbonden.Checked = false;
                    buttonConnect.Text = "Connect";
                }
                catch (System.IO.IOException ioEx)
                {
                    labelStatus.Text = $"IO error: {ioEx.Message}";
                    radioButtonVerbonden.Checked = false;
                    buttonConnect.Text = "Connect";
                }
                catch (Exception ex)
                {
                    labelStatus.Text = "Error: " + ex.Message;
                    try { if (serialPortArduino.IsOpen) serialPortArduino.Close(); } catch { }
                    radioButtonVerbonden.Checked = false;
                    buttonConnect.Text = "Connect";
                }
            }
            else
            {
                try
                {
                    serialPortArduino.Close();
                }
                catch (Exception ex)
                {
                    labelStatus.Text = "Error bij sluiten: " + ex.Message;
                }
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
                labelStatus.Text = "Niet verbonden";
            }
        }

        private void checkBoxDigital2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    string commando; // set d2 high/low
                    if (checkBoxDigital2.Checked) commando = "set d2 high";
                    else commando = "set d2 low";
                    serialPortArduino.WriteLine(commando);
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }

        }
        private void checkBoxDigital3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    string commando; // set d2 high/low
                    if (checkBoxDigital3.Checked) commando = "set d3 high";
                    else commando = "set d3 low";
                    serialPortArduino.WriteLine(commando);
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void checkBoxDigital4_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    string commando; // set d4 high/low
                    if (checkBoxDigital4.Checked) commando = "set d4 high";
                    else commando = "set d4 low";
                    serialPortArduino.WriteLine(commando);
                }

            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }
    }
}

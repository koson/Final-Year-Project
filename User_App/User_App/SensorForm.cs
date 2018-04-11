using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace User_App
{
    public partial class SensorForm : Form
    {
        Chamber[] chambers;
        Sensor[] sensors;
        bool editing;
        public SensorForm(Chamber[] chambers, bool editExisting)
        {
            this.sensors = GetAllSensors(chambers);
            this.chambers = chambers;
            editing = editExisting;
            InitializeComponent();
        }

        private void SensorForm_Load(object sender, EventArgs e)
        {
            chamberPicker.DisplayMember = "Text";
            chamberPicker.ValueMember = "Value";

            List<Object> items = new List<Object>();
            for (int i = 0; i < chambers.Length; i++)
            {
                items.Add(new { Text = chambers[i].Name, Value = chambers[i] });
            }

            chamberPicker.DataSource = items;

            sensorPicker.DisplayMember = "Text";
            sensorPicker.ValueMember = "Value";

            List<Object> items2 = new List<Object>();
            for (int i = 0; i < sensors.Length; i++)
            {
                items2.Add(new { Text = sensors[i].Description, Value = sensors[i] });
            }
            if (editing)
            {
                editExistingButton.Checked = true;
            }
            else
            {
                createNewButton.Checked = true;
            }

            sensorPicker.DataSource = items2;
        }

        private void createNewOption_CheckedChanged(object sender, EventArgs e)
        {
            if (createNewButton.Checked == true)
            {
                sensorPicker.Enabled = false;
                //disable ID pick option
            }
            else
            {
                sensorPicker.Enabled = true;
                //enable ID pick option
            }
        }
        private Sensor[] GetAllSensors(Chamber[] chambers)
        {
            List<Sensor> sensors = new List<Sensor>();
            for (int i = 0; i < chambers.Length; i++)
            {
                for (int j = 0; j < chambers[i].sensors.Length; j++)
                {
                    sensors.Add(chambers[i].sensors[j]);
                }
            }
            return sensors.ToArray();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            String message = "Are you sure you want to cancel?";
            String caption = "Confirm";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);
            if(result == DialogResult.Yes)
            {
                this.Dispose();
            }
        }

        private Boolean ValidateInput(String sensorPort, String sensorType, String sensorScale, String sensorOffset, String sensorRegister, Chamber chamber, String sensorAddress)
        {
            int register = 0;
            int type = 0;
            try
            {
                int.Parse(sensorPort);
                type = int.Parse(sensorType);
                double.Parse(sensorScale);
                double.Parse(sensorOffset);
                register = int.Parse(sensorRegister);
                System.Net.IPAddress.Parse(sensorAddress);
            }catch(Exception e)
            {
                MessageBoxButtons btns = MessageBoxButtons.OK;
                String caption = "Error";
                MessageBox.Show(e.Message, caption, btns);
                return false;
            }
            if (register > 0 && register <= 8 && type >= 0 && type <=3 && chamber !=null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void UpdateFields(Sensor s)
        {
            if (s != null)
            {
                chamberPicker.SelectedValue = (GetChamberByID(s.ChamberID));
                registerPicker.SelectedIndex = s.Register - 1;
                typePicker.SelectedIndex = s.SensorType;
                sensorNameBox.Text = s.Description;
                sensorIPBox.Text = s.Address;
                portBox.Text = s.Port.ToString();
                scaleBox.Text = s.Scale.ToString();
                offsetBox.Text = s.Offset.ToString();
                enabledBox.Checked = true;
            }
        }

        private Chamber GetChamberByID(int id)
        {
            Chamber toReturn = null;
            for(int i = 0; i < chambers.Length; i++)
            {
                if(chambers[i].ID == id)
                {
                    toReturn = chambers[i];
                }
            }
            return toReturn;
        }

        private void sensorPicker_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateFields((Sensor)sensorPicker.SelectedValue);
        }

        private void typePicker_SelectedValueChanged(object sender, EventArgs e)
        {
            if(typePicker.SelectedIndex == 0) //temperature
            {
                offsetBox.Enabled = false;
                scaleBox.Enabled = false;
            }
            else
            {
                offsetBox.Enabled = true;
                scaleBox.Enabled = true;
            }
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            Sensor s = (Sensor)sensorPicker.SelectedValue;
            int sensorID = s.ID;
            String sensorName = sensorNameBox.Text;
            String sensorAddress = sensorIPBox.Text;
            String sensorType = typePicker.SelectedIndex.ToString();
            String sensorScale = scaleBox.Text;
            String sensorOffset = offsetBox.Text;
            Chamber chamber = (Chamber)chamberPicker.SelectedValue;
            String sensorPort = portBox.Text;
            String sensorRegister = (registerPicker.SelectedIndex + 1).ToString();
            Boolean sensorEnabled = enabledBox.Checked;

            String args = "";
            if (ValidateInput(sensorPort, sensorType, sensorScale, sensorOffset, sensorRegister, chamber, sensorAddress))
            {
                if (editExistingButton.Checked == true)
                {
                    args = "editSensor " + sensorID.ToString() + " \"" + sensorName + "\" " + sensorEnabled.ToString() + " " + sensorType + " " + chamber.ID.ToString() + " \"" + sensorAddress + "\" " + sensorPort + " " + sensorRegister + " " + sensorScale + " " + sensorOffset;
                }
                else
                {
                    args = "addSensor \"" + sensorAddress + "\" " + sensorPort + " " + sensorType + " " + chamber.ID.ToString() + " " + sensorRegister + " " + sensorScale + " " + sensorOffset + " \"" + sensorName + "\" " + sensorEnabled.ToString();
                }
                Boolean success = DeserialiseProcessorOutput(CallProcessor(args));
                if (success)
                {
                    String message = "Success";
                    String caption = "Success";
                    MessageBoxButtons btns = MessageBoxButtons.OK;
                    DialogResult result = MessageBox.Show(message, caption, btns);
                    if (result == DialogResult.OK)
                    {
                        this.Dispose();
                    }
                }
                else
                {
                    String message = "An error has occured with the processing application";
                    String caption = "Error";
                    MessageBoxButtons btns = MessageBoxButtons.OK;
                    MessageBox.Show(message, caption, btns);
                }
            }
        }

        private String CallProcessor(string args)
        {
            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = @"..\..\Resources\ProcessingApplication.exe",
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = args,
                RedirectStandardOutput = true
            };
            String result;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }

        private Boolean DeserialiseProcessorOutput(String output)
        {
            if (output.Contains("<Success value=\"True\" />"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

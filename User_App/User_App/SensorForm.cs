using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace User_App
{
    /// <summary>
    /// Class for displaying form allowing creation/editing of sensors within the system
    /// </summary>
    public partial class SensorForm : Form
    {
        Chamber[] chambers;
        Sensor[] sensors;
        bool editing;
        /// <summary>
        /// constructor method. Initialises form and sets globals
        /// </summary>
        /// <param name="chambers">current array of chambers from the main program window</param>
        /// <param name="editExisting"> boolean for whether form is for editing or creating sensors</param>
        public SensorForm(Chamber[] chambers, bool editExisting)
        {
            this.sensors = GetAllSensors(chambers);
            this.chambers = chambers;
            editing = editExisting;
            InitializeComponent();
        }

        /// <summary>
        /// load method. sets contents of sensor combobox and chamber combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// handles the changing of the radio buttons. enables or disables sensor combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// method to get all sensors for all chambers
        /// </summary>
        /// <param name="chambers">array of chamber to get sensors for</param>
        /// <returns>array of sensors</returns>
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

        /// <summary>
        /// method handling the cancellatiob of the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Method for validating form input.
        /// checks valid IP address
        /// checks for positive, integer port value
        /// checks for parsable scale and offset values
        /// </summary>
        /// <param name="sensorPort"> the value of the sensor port input box</param>
        /// <param name="sensorType">the value of the sensor type input box</param>
        /// <param name="sensorScale">the value of the sensor scale input box</param>
        /// <param name="sensorOffset">the value of the sensor offset input box</param>
        /// <param name="sensorRegister">the value of the sensor register picker</param>
        /// <param name="chamber">the value of the chamber selection box</param>
        /// <param name="sensorAddress">the value of the sensor IP address input box</param>
        /// <returns>returns true if all values are valid</returns>
        private Boolean ValidateInput(String sensorPort, String sensorType, String sensorScale, String sensorOffset, String sensorRegister, Chamber chamber, String sensorAddress)
        {
            int register = 0;
            int type = 0;
            int port = 0;
            try
            {
                port = int.Parse(sensorPort);
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
            if (register > 0 && register <= 8 && type >= 0 && type <=3 && chamber !=null && port > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// method to update form. Populates all fields if edit existing option is checked
        /// </summary>
        /// <param name="s"></param>
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

        /// <summary>
        /// Method to get a chamber object for a given ID
        /// </summary>
        /// <param name="id">integer value for the chamber ID</param>
        /// <returns>returns the chamber object, null if chamber does not exist</returns>
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

        /// <summary>
        /// method to handle the change in existing sensor to edit. calls form update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sensorPicker_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateFields((Sensor)sensorPicker.SelectedValue);
        }

        /// <summary>
        /// handles a change in the type picker combobox. Disables scale and offset inputs if type is temperature
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Method to handle form submission. Valiedates input then calls processing application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void submitButton_Click(object sender, EventArgs e)
        {
            Sensor s;
            int sensorID = 0;
            if(sensorPicker.SelectedValue !=null)
            {
                s = (Sensor)sensorPicker.SelectedValue;
                sensorID = s.ID;
            }
            
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

        /// <summary>
        /// Method to call processing application
        /// </summary>
        /// <param name="args">the command line parameters for the processing application</param>
        /// <returns>XML output of processing application</returns>
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

        /// <summary>
        /// Parses processor output
        /// </summary>
        /// <param name="output"></param>
        /// <returns>parsed boolean indicating a successful run of the processor</returns>
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

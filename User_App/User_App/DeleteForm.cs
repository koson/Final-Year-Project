using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace User_App
{
    /// <summary>
    /// Class for displaying the delete chambers/sensors form
    /// </summary>
    public partial class DeleteForm : Form
    {
        Chamber[] chambers;
        Sensor[] sensors;
        Boolean sensor;
        /// <summary>
        /// class constructor
        /// </summary>
        /// <param name="chambers"> The main chambers array passed from the main application window</param>
        /// <param name="sensor"> whether the form was called to delete sensors or not</param>
        public DeleteForm(Chamber[] chambers, Boolean sensor)
        {
            this.sensor = sensor;
            this.chambers = chambers;
            this.sensors = MakeSensorArray();
            InitializeComponent();
            if (sensor)
            {
                deleteSensorOption.Checked = true;
                deleteChamberOption.Checked = false;
            }
            else
            {
                deleteSensorOption.Checked = false;
                deleteChamberOption.Checked = true;
            }
        }

        /// <summary>
        /// changes the items within the combobox
        /// </summary>
        /// <param name="areSensors"> true sets the combobox to sensors</param>
        private void changePickerItems(Boolean areSensors)
        {
            toDeletePicker.DisplayMember = "Text";
            toDeletePicker.ValueMember = "Value";
            List<Object> items = new List<Object>();
            if (areSensors)
            {
                for (int i = 0; i < sensors.Length; i++)
                {
                    items.Add(new { Text = sensors[i].Description, Value = sensors[i] });
                }
                
            }
            else
            {
                for (int i = 0; i < chambers.Length; i++)
                {
                    items.Add(new { Text = chambers[i].Name, Value = chambers[i] });
                }
            }
            toDeletePicker.DataSource = items;
        }

        /// <summary>
        /// method to put sensors from all chambers into a single array
        /// </summary>
        /// <returns>an array of sensors</returns>
        private Sensor[] MakeSensorArray()
        {
            List<Sensor> sensors = new List<Sensor>();
            for(int i = 0; i < chambers.Length; i++)
            {
                for(int j = 0; j < chambers[i].sensors.Length; j++)
                {
                    sensors.Add(chambers[i].sensors[j]);
                }
            }
            return sensors.ToArray();
        }

        /// <summary>
        /// Action completed after cancel button is clicked - whole form closed after confirmation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            String message = "Are you sure you want to cancel?";
            String caption = "Confirm";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// Action after submit button is clicked. Processing application called to delete currently selected item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void submitButton_Click(object sender, EventArgs e)
        {
            if(deleteChamberOption.Checked == true)
            {
                Chamber c = (Chamber)toDeletePicker.SelectedValue;
                String message = "This action will delete all sensors and sensor data associated with" + c.Name + ", Continue?";
                String caption = "Warning";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, caption, buttons);
                if(result == DialogResult.Yes)
                {
                    if(DeserialiseProcessorOutput(CallProcessor("removeChamber " + c.ID)))
                    {
                        String message2 = "Success";
                        String caption2 = "Success";
                        MessageBoxButtons buttons2 = MessageBoxButtons.OK;
                        DialogResult result2 = MessageBox.Show(message2, caption2, buttons2);
                        if(result2 == DialogResult.OK)
                        {
                            this.Dispose();
                        }
                    }
                    else
                    {
                        String message2 = "An error has occured with the processing application";
                        String caption2 = "Error";
                        MessageBoxButtons buttons2 = MessageBoxButtons.OK;
                        MessageBox.Show(message2, caption2, buttons2);
                    }
                }
            }
            else
            {
                Sensor s = (Sensor)toDeletePicker.SelectedValue;
                String message = "This action will delete all data associated with" + s.Description + ", Continue?";
                String caption = "Warning";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, caption, buttons);
                if (result == DialogResult.Yes)
                {
                    if(DeserialiseProcessorOutput(CallProcessor("removeSensor " + s.ID)))
                    {
                        String message2 = "Success";
                        String caption2 = "Success";
                        MessageBoxButtons buttons2 = MessageBoxButtons.OK;
                        DialogResult result2 = MessageBox.Show(message2, caption2, buttons2);
                        if (result2 == DialogResult.OK)
                        {
                            this.Dispose();
                        }
                    }
                    else
                    {
                        String message2 = "An error has occured with the processing application";
                        String caption2 = "Error";
                        MessageBoxButtons buttons2 = MessageBoxButtons.OK;
                        MessageBox.Show(message2, caption2, buttons2);
                    }
                }
            }
        }

        /// <summary>
        /// Method called when radio buttons changed. Calls the changePickerItems method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteChamberOption_CheckedChanged(object sender, EventArgs e)
        {
            if(deleteChamberOption.Checked == true)
            {
                changePickerItems(false);
            }
            else
            {
                changePickerItems(true);
            }
        }

        /// <summary>
        /// Method to call the processor application
        /// </summary>
        /// <param name="args"></param>
        /// <returns>the result of the processor (XML string)</returns>
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
        /// deserialises processor output
        /// </summary>
        /// <param name="output"></param>
        /// <returns>parsed boolean value from the XML string</returns>
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

        /// <summary>
        /// Method called when form is loaded. picker items initially populated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteForm_Load(object sender, EventArgs e)
        {
            changePickerItems(sensor);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace User_App
{
    public partial class DeleteForm : Form
    {
        Chamber[] chambers;
        Sensor[] sensors;
        Boolean sensor;
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

        private void DeleteForm_Load(object sender, EventArgs e)
        {
            changePickerItems(sensor);
        }
    }
}

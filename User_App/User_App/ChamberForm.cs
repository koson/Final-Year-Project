using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace User_App
{
    public partial class ChamberForm : Form
    {
        Chamber[] chambers;
        public ChamberForm(Chamber[] newChambers, bool editExisting)
        {
            InitializeComponent();
            this.chambers = newChambers;
            if(editExisting == true)
            {
                existingChamberOption.Checked = true;
                newChamberOption.Checked = false;
            }
            else
            {
                newChamberOption.Checked = true;
            }
            this.TopMost = true;
        }

        private void ChamberForm_Load(object sender, EventArgs e)
        {
            this.AcceptButton = chamberSubmitButton;
            this.CancelButton = chamberCancelButton;
            chamberIDPicker.DisplayMember = "Text";
            chamberIDPicker.ValueMember = "Value";

            List<Object> items = new List<Object>();
            for (int i = 0; i < chambers.Length; i++)
            {
                items.Add(new { Text = chambers[i].Name, Value = chambers[i] });
            }

            chamberIDPicker.DataSource = items;
        }

        private void chamberSubmitButton_Click(object sender, EventArgs e)
        {
            int chamberID = 0;
            String chamberName = newChamberName.Text;
            String chamberDescription = newChamberDescription.Text;
            String args;

            if(validateInput(chamberName, chamberDescription))
            {
                if (existingChamberOption.Checked == true)
                {
                    chamberID = (int)((Chamber)chamberIDPicker.SelectedValue).ID;
                    args = "editChamber " + chamberID + " \"" + chamberName + "\" \"" + chamberDescription + "\"";
                }
                else
                {
                    args = "addChamber \"" + chamberDescription + "\" \"" + chamberName + "\"";
                }

                bool success = DeserialiseProcessorOutput(CallProcessor(args));

                if (success)
                {
                    String message = "Success";
                    String caption = "Success";
                    MessageBoxButtons btns = MessageBoxButtons.OK;
                    DialogResult result = MessageBox.Show(message, caption, btns);
                    if(result == DialogResult.OK)
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
            else
            {
                String message = "You have entered an invalid name or description";
                String caption = "Error";
                MessageBoxButtons btns = MessageBoxButtons.OK;
                MessageBox.Show(message, caption, btns);
            }
        }

        private Boolean validateInput(String name, String description)
        {
            if(name.Length <= 45 && description.Length <= 45)
            {
                return true;
            }
            else
            {
                return false;
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

        private void newChamberOption_CheckedChanged(object sender, EventArgs e)
        {
            if(newChamberOption.Checked == true)
            {
                chamberIDPicker.Enabled = false;
                newChamberName.Text = "Name";
                newChamberDescription.Text = "Description";
                //disable ID pick option
            }
            else
            {
                chamberIDPicker.Enabled = true;
                UpdateFields();
                //enable ID pick option
            }
        }
        
        private void UpdateFields()
        {
            Chamber c = (Chamber)chamberIDPicker.SelectedValue;
            newChamberName.Text = c.Name;
            newChamberDescription.Text = c.Description;
        }

        private void chamberIDPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFields();
        }

        private void chamberCancelButton_Click(object sender, EventArgs e)
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
    }
}

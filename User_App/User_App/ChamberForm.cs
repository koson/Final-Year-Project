using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace User_App
{
    /// <summary>
    /// Class responsible for creating/modifying chambers
    /// </summary>
    public partial class ChamberForm : Form
    {
        Chamber[] chambers;
        /// <summary>
        /// Constructor method. Sets radio buttons and loads form
        /// </summary>
        /// <param name="newChambers">Array of current chambers in system</param>
        /// <param name="editExisting"> Whether the form is for editing or creating new chambers</param>
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

        /// <summary>
        /// Method for loading combobox items once form is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Method called when form is submitted. New chamber created or existing chamber edited
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    args = "addChamber \"" + chamberName + "\" \"" + chamberDescription + "\"";
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

        /// <summary>
        /// Method for validating form inputs. currently only restricts length of strings
        /// </summary>
        /// <param name="name">value of the name textbox</param>
        /// <param name="description">value of the description textbox</param>
        /// <returns>returns true if inputs are valid</returns>
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

        /// <summary>
        /// Method for calling processing application
        /// </summary>
        /// <param name="args"></param>
        /// <returns>returns XML string produced as processor output</returns>
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
        /// Deserialise output of processing application
        /// </summary>
        /// <param name="output"></param>
        /// <returns>parsed boolean from XML input string</returns>
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
        /// Method for enabbling/disabling chamber combobox on radio button change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        
        /// <summary>
        /// updates all field in form
        /// </summary>
        private void UpdateFields()
        {
            Chamber c = (Chamber)chamberIDPicker.SelectedValue;
            newChamberName.Text = c.Name;
            newChamberDescription.Text = c.Description;
        }

        /// <summary>
        /// method calling UpdateFields when chamber combobox is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chamberIDPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFields();
        }

        /// <summary>
        /// method to handle the user cancelling the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

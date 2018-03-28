using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace User_App
{
    public partial class ChamberForm : Form
    {
        Chamber[] chambers;
        DialogForm dialog;
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
                    args = "editChamber " + chamberID + " \"" + chamberDescription + "\" \"" + chamberName + "\"";
                }
                else
                {
                    args = "addChamber \"" + chamberDescription + "\" \"" + chamberName + "\"";
                }

                bool success = DeserialiseProcessorOutput(CallProcessor(args));

                if (success)
                {
                    dialog = new DialogForm();
                    //success
                    //close form
                }
                else
                {
                    dialog = new DialogForm();
                    //error with processor
                }
            }
            else
            {
                //error validating input
                dialog = new DialogForm();
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
                FileName = @"C:\Users\Raife\source\repos\Final-Year-Project\ProcessingApplication\ProcessingApplication\bin\Debug\ProcessingApplication.exe",
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
                //disable ID pick option
            }
            else
            {
                chamberIDPicker.Enabled = true;
                //enable ID pick option
            }
        }
    }
}

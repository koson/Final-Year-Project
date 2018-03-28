using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            this.Dispose();
        }

        private void ValidateInput()
        {

        }

        private void UpdateFields(Sensor s)
        {
            if (s != null)
            {
                SetChamberPicker(GetChamberByID(s.ChamberID));
                SetRegisterPicker(s.Register);
            }
        }

        delegate void SetChamberPickerCallback(Chamber c);
        private void SetChamberPicker(Chamber c)
        {
            if (this.chamberPicker.InvokeRequired)
            {
                SetChamberPickerCallback p = new SetChamberPickerCallback(SetChamberPicker);
                this.Invoke(p, new object[] { c });
            }
            else
            {
                this.chamberPicker.SelectedValue = c;
            }
        }

        delegate void SetRegisterPickerCallback(int register);
        private void SetRegisterPicker(int register)
        {
            if (this.registerPicker.InvokeRequired)
            {
                SetRegisterPickerCallback p = new SetRegisterPickerCallback(SetRegisterPicker);
                this.Invoke(p, new object[] { register });
            }
            else
            {
                this.registerPicker.SelectedValue = register;
            }
        }

        delegate void SetTypePickerCallback(int type);
        private void SetTypePicker(int type)
        {
            if (this.typePicker.InvokeRequired)
            {
                SetTypePickerCallback p = new SetTypePickerCallback(SetRegisterPicker);
                this.Invoke(p, new object[] { type });
            }
            else
            {
                switch (type)
                {
                    case 0:
                        this.typePicker.SelectedValue = "Temperature";
                        break;

                    case 1:
                        this.typePicker.SelectedValue = "Pressure";
                        break;

                    case 2:
                        this.typePicker.SelectedValue = "Humidity";
                        break;
                }
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
    }
}

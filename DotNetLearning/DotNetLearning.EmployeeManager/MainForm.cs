using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotNetLearning.EmployeeManager
{

    public partial class MainForm : Form
    {


        public MainForm()
        {
            InitializeComponent();
        }

        int indexRow;
        DataTable emplyeesDt = new DataTable();
        private void Form1_Load(object sender, EventArgs e)
        {
            CountryBind();
            //  table.Columns.Add("ID", typeof(int));
            emplyeesDt.Columns.Add("ID", typeof(int));
            emplyeesDt.Columns.Add("Name", typeof(string));
            emplyeesDt.Columns.Add("Age", typeof(int));
            emplyeesDt.Columns.Add("Salary", typeof(int));
            emplyeesDt.Columns.Add("Country", typeof(string));
            emplyeesDt.Columns.Add("State", typeof(string));
            emplyeesDt.Columns.Add("Plant", typeof(string));

            dgvEmployees.DataSource = emplyeesDt;
            textBoxID.Enabled = false;
            textBoxAge.Enabled = false;
            textBoxName.Enabled = false;
            textBoxSalary.Enabled = false;

            btnNew.Enabled = false;
            
               

            

        }

        public void CountryBind()
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = DataStore.GetCountryList();

            cmbCountry.DataSource = bs;

            cmbCountry.ValueMember = "CountryId";
            cmbCountry.DisplayMember = "CountryName";
           

        }

        public void StateBind(int CountryId)
        {
            cmbState.SelectedValueChanged -= cmbState_SelectedValueChanged;

            BindingSource bs = new BindingSource();
            bs.DataSource = DataStore.GetStateByCountryId(CountryId);
            cmbState.DataSource = bs;

            cmbState.ValueMember = "StateId";
            cmbState.DisplayMember = "StateName";

            cmbState.SelectedValueChanged += cmbState_SelectedValueChanged;
            cmbState.SelectedValue = -1;
            cmbState.SelectedValue = (bs.DataSource as IList<State>).FirstOrDefault()?.StateId;
        }

        public void PlantBind(int StateId)
        {
            cmbPlant.SelectedValueChanged -= cmbPlant_SelectedValueChanged;

            BindingSource bs = new BindingSource();
            bs.DataSource = DataStore.GetPlantByStateId(StateId);
            cmbPlant.DataSource = bs;

            cmbPlant.ValueMember = "PlantId";
            cmbPlant.DisplayMember = "PlantName";

            cmbPlant.SelectedValueChanged += cmbPlant_SelectedValueChanged;

            cmbPlant.SelectedValue = -1;
            cmbPlant.SelectedValue = (bs.DataSource as IList<Plant>).FirstOrDefault()?.PlantId;
        }
        private void cmbCountry_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmbCountry.SelectedValue != null)
            {
                int CountryId = Convert.ToInt32(cmbCountry.SelectedValue.ToString());
                StateBind(CountryId);
            }
           
        }



        private void cmbState_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbState.SelectedValue != null)
            {
                int StateId = Convert.ToInt32(Convert.ToString(cmbState.SelectedValue));
                PlantBind(StateId);
            }
        }

        public void PopulatePlantEmpoyees(int plantId)
        {
            btnNew.Enabled = true;

            emplyeesDt.Rows.Clear();
            var plant = DataStore.GetPlantByPlantId(plantId);
            if (plant != null)
            {
                var employees = DataStore.GetEmployeesByPlantId(plantId);
                var state = DataStore.GetStateByStateId(plant.StateId);
                var country = DataStore.GetCountryByCountryId(state.CountryId);
                foreach (Employee emp in employees)
                {
                    emplyeesDt.Rows.Add(emp.ID, emp.Name, emp.Age, emp.salary, country.CountryName, state.StateName, plant.PlantName);
                }
            }
            dgvEmployees.DataSource = emplyeesDt;

        }
        private void cmbPlant_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbPlant.SelectedValue != null)
            {
                int plantId = Convert.ToInt32(Convert.ToString(cmbPlant.SelectedValue));
                PopulatePlantEmpoyees(plantId);
            }
        }


        private void btnNew_Click(object sender, EventArgs e)
        {
            btnSave.Text = "Save";
            textBoxID.Enabled = false;
            textBoxAge.Enabled = true;
            textBoxName.Enabled = true;
            textBoxSalary.Enabled = true;

            //btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            
            textBoxID.Text = "";
            textBoxName.Text = "";
            textBoxAge.Text = "";
            textBoxSalary.Text = "";
            btnSave.Text = "Save";


            if (cmbCountry.SelectedIndex == -1 || cmbCountry.SelectedIndex == 0)
            {
                btnNew.Enabled = false;
                btnSave.Enabled = false;
            }

         



        }

        private void dgvEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBoxID.Enabled = false;
            textBoxAge.Enabled = true;
            textBoxName.Enabled = true;
            textBoxSalary.Enabled = true;
            btnSave.Text = "Update";
            btnDelete.Enabled = true;
            indexRow = e.RowIndex;
            DataGridViewRow row = dgvEmployees.Rows[indexRow];
            //textBoxID.Text = row.Cells[0].Value.ToString();
            textBoxID.Text= row.Cells[0].Value.ToString();
            textBoxName.Text = row.Cells[1].Value.ToString();
            textBoxAge.Text = row.Cells[2].Value.ToString();
            textBoxSalary.Text = row.Cells[3].Value.ToString();
            cmbCountry.Text = row.Cells[4].Value.ToString();
            cmbState.Text = row.Cells[5].Value.ToString();
            cmbPlant.Text = row.Cells[6].Value.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxID.Enabled = false;
                textBoxAge.Enabled = false;
                textBoxName.Enabled = false;
                textBoxSalary.Enabled = false;

                

                    int plantId = Convert.ToInt32(Convert.ToString(cmbPlant.SelectedValue));
                var employeesList = DataStore.GetEmployeesByPlantId(plantId);

                int empId = -1;
                int.TryParse(textBoxID.Text, out empId);

                Employee employee = employeesList.FirstOrDefault(emp => emp.ID == empId);       //employee object from text box data
                if (btnSave.Text == "Update" && employee == null)
                    throw new Exception("Employee not found");

                if (employee == null)
                    employee = new Employee();

                //Update employee details                
                employee.Name = textBoxName.Text;
                employee.Age = Convert.ToInt32(textBoxAge.Text);
                employee.salary = Convert.ToInt32(textBoxSalary.Text);

                if (btnSave.Text == "Save")
                {
                    int maxAvailable = employeesList.Count > 0 ? employeesList.Max(emp => emp.ID) + 1 : 1;
                    employee.ID = maxAvailable;
                    employee.PlantId = plantId;
                    DataStore.AddEmployee(employee);
                   // employeesList.Add(employee);
                    // dgvEmployees.DataSource = employeesList;
                    PopulatePlantEmpoyees(plantId);
                }
                else
                {

                    int rowCount = dgvEmployees.Rows.Count;

                    DataGridViewRow newDataRow = dgvEmployees.Rows[indexRow];
                    newDataRow.Cells[0].Value = textBoxID.Text;
                    newDataRow.Cells[1].Value = textBoxName.Text;
                    newDataRow.Cells[2].Value = textBoxAge.Text;
                    newDataRow.Cells[3].Value = textBoxSalary.Text;
                    newDataRow.Cells[4].Value = cmbCountry.Text;
                    newDataRow.Cells[5].Value = cmbState.Text;
                    newDataRow.Cells[6].Value = cmbPlant.Text;


                }
                textBoxID.Text = "";
                textBoxName.Text = "";
                textBoxAge.Text = "";
                textBoxSalary.Text = "";

            }
            catch (Exception)
            {

                MessageBox.Show(text: "Select Country First");

            }
           
        }



        private void btnDelete_Click_1(object sender, EventArgs e)
        {

            textBoxID.Enabled = false;
            textBoxAge.Enabled = false;
            textBoxName.Enabled = false;
            textBoxSalary.Enabled = false;


            int plantId = Convert.ToInt32(Convert.ToString(cmbPlant.SelectedValue));
            var employeesList = DataStore.GetEmployeesByPlantId(plantId);

            int empId = -1;
            int.TryParse(textBoxID.Text, out empId);
            Employee employee = employeesList.FirstOrDefault(emp => emp.ID == empId);
            
            DataStore.RemoveEmployee(employee);
          //  employeesList.Remove(employee);

             PopulatePlantEmpoyees(plantId);

            
            textBoxID.Text = "";
            textBoxName.Text = "";
            textBoxAge.Text = "";
            textBoxSalary.Text = "";
        }

        


        private void textBoxAge_Validating(object sender, CancelEventArgs e)
        {
            int a;
          

            if (int.TryParse(textBoxAge.Text, out a))
            {
                if (a <= 14 || a >= 65)
                {
                  
                    e.Cancel = true; ;
                    textBoxAge.Focus();errorProvider1.SetError(textBoxAge, "Please Enter Age Between 14-65 !!");
                    btnSave.Enabled = false;
                    btnNew.Enabled = false;
                }
                else
                {
                    e.Cancel = false;
                    btnSave.Enabled = true;
                    btnNew.Enabled = true;
                }
            }
            else
            {
                
                e.Cancel = true;
                textBoxAge.Focus(); errorProvider1.SetError(textBoxAge, "Age cant be a string");
            }
        }

        private void textBoxSalary_Validating(object sender, CancelEventArgs e)
        {
            int s;


            if (int.TryParse(textBoxSalary.Text, out s))
            {
                if (s <= 0)
                {

                    e.Cancel = true;
                    textBoxSalary.Focus(); errorProvider1.SetError(textBoxSalary, "Salary cant be Zero Rupees!!");
                    btnSave.Enabled = false;
                    btnNew.Enabled = false;

                }
                else
                {
                    
                    e.Cancel = false;
                    btnNew.Enabled = true;
                    btnSave.Enabled = true;
                    //textBoxSalary.errorProvider1.SetError(textBoxSalary, "");




                }
            }
            else
            {

                e.Cancel = true;
                textBoxSalary.Focus(); errorProvider1.SetError(textBoxSalary, "Input Salary in Integer");
            }
        }

        private void textBoxAge_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(textBoxAge, "");
        }

        private void textBoxSalary_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(textBoxSalary, "");
        }
    }
}

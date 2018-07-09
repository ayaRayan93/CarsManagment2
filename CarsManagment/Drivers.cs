using DevExpress.XtraGrid.Views.Grid;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarsManagment
{
    public partial class Drivers : Form
    {
        CarsMainForm carsMainForm;
        MySqlConnection dbconnection;
        public Drivers(CarsMainForm carsMainForm)
        {
            try
            {
                InitializeComponent();
                dbconnection = new MySqlConnection(connection.connectionString);
                this.carsMainForm = carsMainForm;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Cars_Load(object sender, EventArgs e)
        {
            try
            {
                DisplayDrivers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
               carsMainForm.bindRecordDriverForm(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DataRowView driverRow = (DataRowView)(((GridView)gridControlStores.MainView).GetRow(((GridView)gridControlStores.MainView).GetSelectedRows()[0]));
                carsMainForm.bindUpdateDriverForm(driverRow, this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                dbconnection.Open();
                DataRowView setRow = (DataRowView)(((GridView)gridControlStores.MainView).GetRow(((GridView)gridControlStores.MainView).GetSelectedRows()[0]));
                
                if (setRow != null)
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete the item?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string Query = "delete from drivers where Driver_ID=" + setRow[0].ToString();
                        MySqlCommand MyCommand = new MySqlCommand(Query, dbconnection);
                        MyCommand.ExecuteNonQuery();

                        string query = "ALTER TABLE drivers AUTO_INCREMENT = 1;";
                        MySqlCommand com = new MySqlCommand(query, dbconnection);
                        com.ExecuteNonQuery();

                        // UserControl.UserRecord("store", "delete", setRow[0].ToString(), DateTime.Now, dbconnection);
                        DisplayDrivers();

                    }
                    else if (dialogResult == DialogResult.No)
                    { }

                }
                else
                {
                    MessageBox.Show("select row");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dbconnection.Close();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                carsMainForm.bindReportDriverForm(gridControlStores);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        //functions
        public void DisplayDrivers()
        {
            string qeury = "select Driver_ID as 'الكود',Driver_Name as 'السائق',Driver_Phone as 'التلفون',Driver_Address as 'العنوان',Driver_BairthDate as 'تاريخ الميلاد',Driver_License as 'الرخصة',Driver_NationalID as 'الرقم القومي',Driver_StartWorkDate as 'تاريخ بدء العمل' from drivers";
            MySqlDataAdapter adapter = new MySqlDataAdapter(qeury, dbconnection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            gridControlStores.DataSource = dataSet.Tables[0];
           // gridView1.Columns[0].Visible = false;
        }

      
    }
}

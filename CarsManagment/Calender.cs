using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Helpers;
using MySql.Data.MySqlClient;
using DevExpress.XtraScheduler;


namespace CarsManagment
{
    public partial class Calender : Form {
      
        DataSet schedulerDataSet;
        MySqlDataAdapter appointmentDataAdapter;
        MySqlDataAdapter resourceDataAdapter;
        MySqlConnection dbconnection;

        public Calender() {
            InitializeComponent();
            InitSkinGallery();
            schedulerControl.Start = System.DateTime.Now;
            this.schedulerStorage.AppointmentsChanged += OnAppointmentChangedInsertedDeleted;
            this.schedulerStorage.AppointmentsInserted += OnAppointmentInserted;
            this.schedulerStorage.AppointmentsDeleted += OnAppointmentChangedInsertedDeleted;
        }

        private void OnAppointmentInserted(object sender, DevExpress.XtraScheduler.PersistentObjectsEventArgs e) {
            appointmentDataAdapter.Update(schedulerDataSet.Tables["Appointments"]);
            schedulerDataSet.AcceptChanges();
        }

            private void InitScheduler() {
            schedulerDataSet = new DataSet();
            string selectAppointments = "SELECT * FROM Appointments";
            string selectResources = "SELECT * FROM resources";

            dbconnection = new MySqlConnection(connection.connectionString);
            dbconnection.Open();

            appointmentDataAdapter = new MySqlDataAdapter(selectAppointments, dbconnection);
            appointmentDataAdapter.RowUpdated +=appointmentDataAdapter_RowUpdated;
            appointmentDataAdapter.Fill(schedulerDataSet, "Appointments");

            resourceDataAdapter = new MySqlDataAdapter(selectResources, dbconnection);
            resourceDataAdapter.Fill(schedulerDataSet, "Resources");

            MapAppointmentData();
            MapResourceData();

            MySqlCommandBuilder cmdBuilder= new MySqlCommandBuilder(appointmentDataAdapter);
            appointmentDataAdapter.InsertCommand = cmdBuilder.GetInsertCommand();
            appointmentDataAdapter.DeleteCommand = cmdBuilder.GetDeleteCommand();
            appointmentDataAdapter.UpdateCommand = cmdBuilder.GetUpdateCommand();

            schedulerStorage.Appointments.DataSource = schedulerDataSet;
            schedulerStorage.Appointments.DataMember = "Appointments";
            schedulerStorage.Resources.DataSource = schedulerDataSet;
            schedulerStorage.Resources.DataMember = "Resources";

            dbconnection.Close();
        }

        private void MapResourceData()
        {
            schedulerStorage.Resources.Mappings.Id = "ID";
            schedulerStorage.Resources.Mappings.Caption = "Description";
        }

        private void MapAppointmentData()
        {
            schedulerStorage.Appointments.Mappings.AllDay = "AllDay";
            schedulerStorage.Appointments.Mappings.Description = "Description";
            schedulerStorage.Appointments.Mappings.End = "End";
            schedulerStorage.Appointments.Mappings.Label = "Label";
            schedulerStorage.Appointments.Mappings.Location = "Location";
            schedulerStorage.Appointments.Mappings.RecurrenceInfo = "RecurrenceInfo";
            schedulerStorage.Appointments.Mappings.ReminderInfo = "ReminderInfo";
            schedulerStorage.Appointments.Mappings.Start = "Start";
            schedulerStorage.Appointments.Mappings.Status = "Status";
            schedulerStorage.Appointments.Mappings.Subject = "Subject";
            schedulerStorage.Appointments.Mappings.Type = "Type";
            schedulerStorage.Appointments.Mappings.ResourceId = "ResourceIDs";
        }

        void appointmentDataAdapter_RowUpdated(object sender, MySqlRowUpdatedEventArgs e)
        {
 	        if(e.Status == UpdateStatus.Continue && e.StatementType == StatementType.Insert) {
                int id = 0;
                MySqlCommand cmd = new MySqlCommand("SELECT LAST_INSERT_ID()", dbconnection);
                id = Convert.ToInt32(cmd.ExecuteScalar());
                e.Row["ID"] = id;
            }
        }

        private void OnAppointmentChangedInsertedDeleted(object sender, DevExpress.XtraScheduler.PersistentObjectsEventArgs e) {
            appointmentDataAdapter.Update(schedulerDataSet.Tables["Appointments"]);
            schedulerDataSet.AcceptChanges();
        }
        void InitSkinGallery() {
            SkinHelper.InitSkinGallery(rgbiSkins, true);
        }

        private void Form1_Load(object sender, EventArgs e) {
            InitScheduler();
        }

    }
}
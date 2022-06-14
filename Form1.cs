using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace TreeViewDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT Name FROM sysdatabases ORDER BY Name",new SqlConnection("data source=.;database=master;uid=sa;pwd=sql"));
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                tvw.Nodes.Add(sdr["Name"].ToString());
            }
            sdr.Close();
            cmd.Connection.Close();
        }

        private void tvw_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level==0)
            {
                e.Node.Nodes.Clear();
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT TABLE_SCHEMA FROM INFORMATION_SCHEMA.TABLES ORDER BY 1", new SqlConnection(string.Format("data source=.;database={0};uid=sa;pwd=sql", e.Node.Text)));
                cmd.Connection.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    e.Node.Nodes.Add(sdr["TABLE_SCHEMA"].ToString());
                }
                sdr.Close();
                cmd.Connection.Close();
            }
            else if (e.Node.Level == 1)
            { 
                e.Node.Nodes.Clear();
                SqlCommand cmd = new SqlCommand(string.Format("SELECT DISTINCT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='{0}' ORDER BY 1",e.Node.Text), new SqlConnection(string.Format("data source=.;database={0};uid=sa;pwd=sql", e.Node.Parent.Text)));
                cmd.Connection.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    e.Node.Nodes.Add(sdr["TABLE_Name"].ToString());
                }
                sdr.Close();
                cmd.Connection.Close();                
            }
            else if (e.Node.Level == 2)
            {
                SqlDataAdapter sda = new SqlDataAdapter(string.Format("SELECT * FROM {0}.{1}",e.Node.Parent.Text,e.Node.Text),new SqlConnection(string.Format("data source=.;database={0};uid=sa;pwd=sql",e.Node.Parent.Parent.Text)));
                DataSet ds = new DataSet();
                sda.Fill(ds);
                new Form2(ds.Tables[0]).ShowDialog();
            }
        }
    }
}

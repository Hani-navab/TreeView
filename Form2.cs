using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeViewDemo
{
    public partial class Form2 : Form
    {
        public Form2(DataTable tbl)
        {
            InitializeComponent();
            dbg.DataSource = tbl;
        }
    }
}

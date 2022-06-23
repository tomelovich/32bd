using System;
using System.Data;
using System.Data.OleDb;

using System.Windows.Forms;

namespace Part_32.Task_1
{
    public partial class Form1 : Form
    {
        private const string _conString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\практика\32\Part_32.Task_1\Lab30.accdb";
        private readonly DataSet _dataSet;

        public Form1()
        {
            InitializeComponent();

            _dataSet = new DataSet();
            Load += FormMain_Load;

            dataGridView1.SelectionChanged += DataGridViewMaster_SelectionChanged;
            
            using (var adpa = new OleDbDataAdapter("SELECT * FROM ИнформацияОТуристах", _conString))
            using (var adpb = new OleDbDataAdapter("SELECT * FROM Туристы", _conString))
            using (var adp1 = new OleDbDataAdapter("SELECT * FROM Оплата", _conString))
            using (var adp2 = new OleDbDataAdapter("SELECT * FROM Путевки", _conString))
            using (var adp3 = new OleDbDataAdapter("SELECT * FROM Сезон", _conString))
            using (var adp4 = new OleDbDataAdapter("SELECT * FROM Туры", _conString))
            {
                adpa.Fill(_dataSet, "ИнформацияОТуристах");
                adpb.Fill(_dataSet, "Туристы");
                adp1.Fill(_dataSet, "Оплата");
                adp2.Fill(_dataSet, "Путевки");
                adp3.Fill(_dataSet, "Сезон");
                adp4.Fill(_dataSet, "Туры");
            }

            dataGridView3.DataSource = _dataSet.Tables["Оплата"];
            dataGridView4.DataSource = _dataSet.Tables["Путевки"];
            dataGridView5.DataSource = _dataSet.Tables["Сезон"];
            dataGridView6.DataSource = _dataSet.Tables["Туры"];
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            _dataSet.Tables[0].Constraints.Add("КодТуриста", _dataSet.Tables[0].Columns[0], true);
            _dataSet.Tables[1].Constraints.Add("КодТуриста", _dataSet.Tables[1].Columns[0], true);

            _dataSet.Relations.Add("ИнформацияТуриста", _dataSet.Tables[1].Columns[0], _dataSet.Tables[0].Columns[0]);

            dataGridView1.DataSource = _dataSet.Tables["Туристы"];
            LoadChildData(0);

            dataGridView1.SelectionChanged += DataGridViewMaster_SelectionChanged;
        }

        private void LoadChildData(int rowIndex)
        {
            var parentRow = _dataSet.Tables["Туристы"].Rows[rowIndex];
            var childRows = parentRow.GetChildRows("ИнформацияТуриста");
            var childTable = _dataSet.Tables["ИнформацияОТуристах"].Clone();

            foreach (var row in childRows)
            {
                childTable.ImportRow(row);
            }

            dataGridView2.DataSource = childTable;
        }

        private void DataGridViewMaster_SelectionChanged(object sender, EventArgs e)
        {
            LoadChildData(dataGridView1.CurrentRow.Index);
        }

    }
}

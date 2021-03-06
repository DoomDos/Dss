﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class FormTuyenSinh : Form
    {
        public FormTuyenSinh(string id, string nam)
        {
            InitializeComponent();
            this.id = id;
            this.nam = nam;
        }
        string id;
        string nam;
        
        private void ketnoicsdl(string id)
        {            
            string sql = "select Truong.TenTruong, TuyenSinh.* from Truong,TuyenSinh where TuyenSinh.MaTruong='" + id +"' AND Truong.MaTruong='" + id + "'";
            dataGridViewTS.DataSource = ExcuteSql.connectDB(sql); //đổ dữ liệu vào datagridview
        }
        private void buttonThem_Click(object sender, EventArgs e)
        {
            var formTuyenSinhThem = new FormTuyenSinhThem(id,null);
            formTuyenSinhThem.FormClosing += new FormClosingEventHandler(ChildFormClosing);
            formTuyenSinhThem.ShowDialog();
        }

        private void buttonDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormTuyenSinh_Load(object sender, EventArgs e)
        {
            string sql = "SELECT DISTINCT TuyenSinh.Nam FROM TuyenSinh ORDER BY TuyenSinh.Nam ASC; ";
            DataTable dt = ExcuteSql.connectDB(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
                comboBoxNam.Items.Add(dt.Rows[i][0].ToString());
            comboBoxNam.SelectedIndex = 0;
            ketnoicsdl(id);

            
        }

        private void buttonXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string idDelTr = dataGridViewTS.CurrentRow.Cells[1].Value.ToString();
                string idDelNam = dataGridViewTS.CurrentRow.Cells[2].Value.ToString();
                string sql = "DELETE FROM TuyenSinh WHERE MaTruong ='" + idDelTr + "' AND Nam = '" + idDelNam +"'"; //Xóa theo mã trường và năm
                var confirmResult = MessageBox.Show("Bạn chắc chắn muốn xóa năm "+ idDelNam +"?",
                                     "Xóa năm!!",
                                     MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    ExcuteSql.excuteCom(sql);
                    MessageBox.Show("Xóa thành công");
                    resetForm();
                }
                
            }
            catch
            {
                MessageBox.Show("Không thể xóa!");
            }
        }
        public void resetForm()
        {
            comboBoxNam.Items.Clear();
            comboBoxNam.Items.Add("Tất cả");
            string sql = "SELECT DISTINCT TuyenSinh.Nam FROM TuyenSinh ORDER BY TuyenSinh.Nam ASC; ";
            DataTable dt = ExcuteSql.connectDB(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
                comboBoxNam.Items.Add(dt.Rows[i][0].ToString());
            this.comboBoxNam.SelectedIndex = 0;
            this.ketnoicsdl(id);
        }

        private void comboBoxNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nam = comboBoxNam.GetItemText(comboBoxNam.SelectedItem);
            if (nam != "Tất cả")
            {
                string sql = "select Truong.TenTruong, TuyenSinh.* from Truong,TuyenSinh where TuyenSinh.Nam='" + nam + "' AND Truong.MaTruong='" + id + "' AND TuyenSinh.MaTruong='" + id + "'";                
                dataGridViewTS.DataSource = ExcuteSql.connectDB(sql); //đổ dữ liệu vào datagridview
            }
            else {
                ketnoicsdl(id);
            }
        }        

        private void buttonSua_Click(object sender, EventArgs e)
        {   nam = dataGridViewTS.CurrentRow.Cells[2].Value.ToString();
            var formTuyenSinhThem = new FormTuyenSinhThem(id,nam);
            formTuyenSinhThem.FormClosing += new FormClosingEventHandler(ChildFormClosing);
            formTuyenSinhThem.ShowDialog();            
        }
        private void ChildFormClosing(object sender, FormClosingEventArgs e)
        {
            resetForm();
        }        
    }
}

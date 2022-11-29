using Npgsql;
using System.Data;
using System.Windows.Forms;

namespace Responsi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private NpgsqlConnection conn;
        string connstring = "Host=localhost;Port=2022;Username=postgres;Password=informatika;Database=ResponsiNew";
        public DataTable dt;
        public static NpgsqlCommand cmd;
        private string sql = null;
        private DataGridViewRow r;


        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"select * from st_insert(:_nama,:_id_dept)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_name", tbNama.Text);
                cmd.Parameters.AddWithValue("_id_dept", cbDept.Text);
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Data Users Berhasil Diinputkan", "Well Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    btnLoadData.PerformClick();
                    tbNama.Text = cbDept.Text = null;
                    r = null;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Insert FAIL!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Mohon pilih baris data yang akan diedit", "Good!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                conn.Open();
                sql = @"select * from st_update(:_id,:name,:_alamat,:_no_handphone)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id_karyawan", r.Cells["_id_karyawan"].Value.ToString());
                cmd.Parameters.AddWithValue("_nama", tbNama.Text);
                cmd.Parameters.AddWithValue("_id_dept", cbDept.Text);
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Data Users Berhasil Diedit", "Well Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    btnLoadData.PerformClick();
                    tbNama.Text = cbDept.Text = null;
                    r = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Edit FAIL!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Mohon pilih baris yang akan didelete", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Apakah benar anda ingin menghapus data " + r.Cells["_nama"].Value.ToString() + " ?", "Hapus data terkonfirmasi",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)

                try
                {
                    conn.Open();
                    sql = @"select * from st_delete(:_id_karyawan)";
                    cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("_id_karyawan", r.Cells["_id_karyawan"].Value.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:" + ex.Message, "Delete FAIL!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                dgvData.DataSource = null;
                sql = "select * from st_select";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                NpgsqlDataReader rd = cmd.ExecuteReader();
                dt.Load(rd);
                dgvData.DataSource = dt;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "FAIL!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                r = dgvData.Rows[e.RowIndex];
                tbNama.Text = r.Cells["_nama"].Value.ToString();
                cbDept.Text = r.Cells["_id_dept"].Value.ToString();

            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
        }
    }
}
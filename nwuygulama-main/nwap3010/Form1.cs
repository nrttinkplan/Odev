
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace nwap3010
{
    public partial class Form1 : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlCommand cmdtedarik;

        string constr = "Data Source=DESKTOP-J07VOSM\\SQLEXPRESS;Initial Catalog = dbnwind; Integrated Security = True";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnkaydet_Click(object sender, EventArgs e)
        {
            con = new SqlConnection(constr);

            con.Open();
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"insert into Products(ProductName,SupplierID,CategoryID,UnitPrice) " +
                $"values('{txturunad.Text.ToString()}',{cmbtedarik.SelectedValue},{cmbkategori.SelectedValue},{nupbirimfiyat.Value})";
            cmd.ExecuteNonQuery();
            con.Close();
            tazele();

        }


        private void tazele()
        {
            con = new SqlConnection(constr);
            con.Open();

            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from Products order by " +
                "ProductID desc";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            con = new SqlConnection(constr);
            con.Open();


            //Kategori bilgileri cmbkategori combosuna aktarýlýyor
            cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select CategoryID,CategoryName from Categories";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            cmbkategori.ValueMember = "CategoryID";
            cmbkategori.DisplayMember = "CategoryName";
            cmbkategori.DataSource = dt;


            //Tedarikçiler bilgileri cmbtedarik combosuna aktarýlýyor

            cmdtedarik = new SqlCommand();
            cmdtedarik.Connection = con;
            cmdtedarik.CommandText = "select SupplierID,CompanyName from Suppliers";
            cmdtedarik.ExecuteNonQuery();
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter(cmdtedarik);
            da2.Fill(dt2);
            cmbtedarik.ValueMember = "SupplierID";
            cmbtedarik.DisplayMember = "CompanyName";
            cmbtedarik.DataSource = dt2;
            tazele();
            con.Close();





        }

        private void btnsil_Click(object sender, EventArgs e)
        {
            // DataGridView'da seçili satýra bak
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Seçili satýrýn indeksini al
                int selectedRowIndex = dataGridView1.SelectedRows[0].Index;

                // DataGridView'da seçili satýrdaki verinin ID'sini al
                int ProductID = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells["ProductID"].Value);

                // Northwind veritabanýna baðlan

                {
                    con.Open();

                    // Veriyi silme SQL komutunu oluþtur
                    string deleteQuery = "Delete From Products WHERE ProductID = @ProductID";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", ProductID);

                        // Komutu çalýþtýr ve etkilenen satýr sayýsýný kontrol et
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Veri baþarýyla silindi.");
                            tazele();

                        }
                        else
                        {
                            MessageBox.Show("Veri silinirken bir hata oluþtu.");
                            tazele();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen silinecek bir satýr seçin.");
                tazele();
            }


            con.Close();

        }

        private void btnbul_Click(object sender, EventArgs e)
        {

            // Kullanýcýdan metni al
            string arananMetin = txturunad.Text;

            if (!string.IsNullOrEmpty(arananMetin))
            {
                // Veritabanýndan verileri çek
                // Örneðin, SQL sorgusuyla Northwind veritabanýndan veri çekiyoruz:
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    con.Open();
                    string query = "SELECT * FROM Products WHERE ProductName LIKE @arananMetin";
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@arananMetin", "%" + arananMetin + "%");
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable table = new DataTable();
                        adapter.Fill(table);


                        // DataGridView'i güncelle
                        dataGridView1.DataSource = table;
                    }
                }
            }
            con.Close();

        }

        private void btnguncel_Click(object sender, EventArgs e)
        {
            
            {
                con.Open();
                // DataGridView'de seçili bir satýr var mý kontrol et
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    // Seçili satýrýn DataRowView objesini al
                    DataRowView selectedRow = dataGridView1.SelectedRows[0].DataBoundItem as DataRowView;

                    // DataRowView objesinden ProductName, CategoryID ve SupplierID deðerlerini al
                    string productName = selectedRow["ProductName"].ToString();
                    int categoryID = Convert.ToInt32(selectedRow["CategoryID"]);
                    int supplierID = Convert.ToInt32(selectedRow["SupplierID"]);

                    // Bu deðerleri istediðiniz gibi kullanabilirsiniz
                    // Örneðin MessageBox ile gösteriyorum
                    MessageBox.Show($"ProductName: {productName}\nCategoryID: {categoryID}\nSupplierID: {supplierID}");
                }
                else
                {
                    MessageBox.Show("Lütfen bir satýr seçin.");
                }

            }
            con.Close();
        }
    }
}

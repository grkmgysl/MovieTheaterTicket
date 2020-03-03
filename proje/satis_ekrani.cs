using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace proje
{
    public partial class satis_ekrani : Form
    {
        public satis_ekrani()
        {
            InitializeComponent();
        }


        public ArrayList alinan_koltuklar = new ArrayList();
        public string seans_id;

        private void satis_ekrani_Load(object sender, EventArgs e)
        {
            int bilet_fiyati = 20;
            for (int i=0; i<alinan_koltuklar.Count; i++)
            {
                textBox1.Text += alinan_koltuklar[i] + "   ";
            }
            textBox2.Text = seans_id;
            textBox3.Text = (bilet_fiyati * alinan_koltuklar.Count).ToString(); 
        }



        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < alinan_koltuklar.Count; i++)
            {
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = "Server=.\\sqlexpress;Database=sinema;Trusted_Connection=true";
                cn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "insert into biletler values(@koltuk_no,getdate(),@seans_id)";
                cmd.Parameters.AddWithValue("koltuk_no", Convert.ToInt32(alinan_koltuklar[i]));
                cmd.Parameters.AddWithValue("seans_id", seans_id);
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();
                
                
            }
            MessageBox.Show("Bilet satisi basarili");
        }

        private void satis_ekrani_FormClosing(object sender, FormClosingEventArgs e)
        {
            alinan_koltuklar.Clear();
        }
    }
}

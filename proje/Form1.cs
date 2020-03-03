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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataTable salonlar = new DataTable();
        
        private void Form1_Load(object sender, EventArgs e)
        {
            button3.BackColor = Color.Green;
            button4.BackColor = Color.Red;
            
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = "Server=.\\sqlexpress;Database=sinema;Trusted_Connection=True";
            cn.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select * from salonlar";
            cmd.Connection = cn;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            
            da.Fill(salonlar);

            

            for (int i = 0; i < salonlar.Rows.Count; i++)
            {
                
                treeView1.Nodes.Add(salonlar.Rows[i][0].ToString(), salonlar.Rows[i][1].ToString());
                SqlConnection cn2 = new SqlConnection();
                cn2.ConnectionString = "Server=.\\sqlexpress;Database=sinema;Trusted_Connection=True";
                cn2.Open();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = "select * from seanslar s inner join filmler f on s.film_id = f.film_id where salon_id = @id";
                cmd2.Parameters.AddWithValue("id", salonlar.Rows[i][0].ToString());
                cmd2.Connection = cn2;

                SqlDataReader dr = cmd2.ExecuteReader();
                while (dr.Read())
                {
                    
                    treeView1.Nodes[i].Nodes.Add(dr["seans_id"].ToString() , dr["saat"].ToString() + "(" + dr["film_adi"] + ")" );
                }

            }
        }
        public string pub_seans_id;
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            panel1.Controls.Clear();
            string seans_id = treeView1.SelectedNode.Name;
            ArrayList koltuklar = new ArrayList();
            pub_seans_id = seans_id;        //seans id public yapar

            SqlConnection cn2 = new SqlConnection();
            cn2.ConnectionString = "Server=.\\sqlexpress;Database=sinema;Trusted_Connection=True";
            cn2.Open();

            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "select * from biletler where seans_id=@seans_id";
            cmd2.Parameters.AddWithValue("seans_id", seans_id);
            cmd2.Connection = cn2;

            SqlDataReader dr = cmd2.ExecuteReader();
            while (dr.Read())
            {

                koltuklar.Add(dr["koltuk_no"]);
            }

            int sayac = 1;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (koltuklar.Contains(sayac))      //her koltugu cizdiginde arrayde o koltuk numarasi varmi diye bakar
                    {
                        Button btn = new Button();
                        
                        btn.Width = 30;
                        btn.Height = 30;
                        btn.Top = i * 35;
                        btn.Left = j * 35;
                        btn.Text = ((i * 10) + (j + 1) ).ToString();
                        btn.Click += Btn_Click;
                        btn.BackColor = Color.Red;
                        panel1.Controls.Add(btn);
                    }
                    else
                    {
                        Button btn = new Button();
                        
                        btn.Width = 30;
                        btn.Height = 30;
                        btn.Top = i * 35;
                        btn.Left = j * 35;
                        btn.Text = ((i * 10) + (j + 1)).ToString();
                        btn.Click += Btn_Click;
                        btn.BackColor = Color.Green;
                        panel1.Controls.Add(btn);
                    }
                    sayac++;
                }
            }
        }


        public ArrayList secilmis_koltuklar = new ArrayList();


        private void Btn_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;      //hangi butona basildigi bilgisi icin sender unbox
            if (b.BackColor == Color.Green)     //eger yesilse secileni listeye ekler ve sari yapar
            {
                secilmis_koltuklar.Add(b.Text);
                b.BackColor = Color.Yellow;
            }
            else if(b.BackColor == Color.Red)
            {
                MessageBox.Show("Bu koltuk dolu!\nLütfen başka bir koltuk seçiniz.");
            }
            else
            {
                secilmis_koltuklar.Remove(b.Text);
                b.BackColor = Color.Green;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            satis_ekrani se = new satis_ekrani();
            se.alinan_koltuklar = secilmis_koltuklar;       //secilmis listi form2 ye yollar
            se.seans_id = pub_seans_id;     //seans id form2 ye yollar
            se.Show();
        }
    }
}

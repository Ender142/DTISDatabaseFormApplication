using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DTISKisiEkleme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string resimpath;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Title = "Resim";
            file.Filter = "jpeg (*.jpg)|*.jpg|png (*.png)|*.png";
            if (file.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(file.FileName);
                resimpath = file.FileName.ToString();
            }
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream(resimpath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] resim = br.ReadBytes((int)fs.Length);
            br.Close();
            fs.Close();
            //veritabanı bağlantısı
            SqlConnection bag = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=dtis;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("Insert into resim(resim) values (@image)", bag);
            cmd.Parameters.Add("@image", SqlDbType.Image, resim.Length).Value = resim;
            try
            {
                bag.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Unsur Veritabanına Kaydedildi");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                bag.Close();
            }

        }
    }
}

using Renci.SshNet;
using System.Net.Sockets;
namespace ZIPORD
{

    public partial class Form1 : Form
    {
        private string ip = "";
        private string username = "";
        private string port = "22";
        private string password = "";
        private string remotepath = "";
        private SshClient sshCon;
        private ScpClient scpCon;
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = ip;
            textBox2.Text = username;
            textBox4.Text = port.ToString();
            textBox3.Text = password;
            textBox5.Text = remotepath;
        }
        private void Groupbox1_Load(object sender, EventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ip = textBox1.Text;
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            username = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            password = textBox3.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Select sel = new Select();
            sel.ip = textBox1.Text;
            sel.port = textBox4.Text;
            sel.username = textBox2.Text;
            sel.password = textBox3.Text;
            sel.remotepath = textBox5.Text;
            try
            {
                sshCon = new SshClient(ip, int.Parse(port), username, password);
                sshCon.Connect();
                scpCon = new ScpClient(ip, int.Parse(port), username, password);
                scpCon.Connect();
                MessageBox.Show("Connection is successful!");
                sel.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection error: {ex.Message}");
            }
           
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            port = textBox4.Text;
        }


        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            remotepath = textBox5.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

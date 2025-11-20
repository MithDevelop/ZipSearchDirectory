using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Org.BouncyCastle.Asn1.Cmp;

namespace ZIPORD
{
    public partial class Select : Form
    {
        private SshClient sshClient;
        private ScpClient scpClient;
        private bool isFullscreen = false;
        public string ip { get; set; }
        public string port { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string remotepath { get; set; }
        public string rem = "";

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]

        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        public Select()
        {
            InitializeComponent();
            this.groupBox1.MouseDown += groupBox1_MouseDown;
            this.KeyPreview = true;
            textBox1.TextChanged += textBox1_TextChanged;
            textBox1.KeyDown += TextBox1_KeyDown;

        }

        private void Select_Load(object sender, EventArgs e)
        {
            textBox1.Text = remotepath;
            connect();
        }
        private void connect()
        {
            try
            {
                sshClient = new SshClient(ip, int.Parse(port), username, password);
                sshClient.Connect();
                scpClient = new ScpClient(ip, int.Parse(port), username, password);
                scpClient.Connect();
                SearchZipFiles(remotepath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection error: {ex.Message}");
            }
        }
        private void SearchZipFiles(string remotePath)
        {
 
            try
            {

                var command = sshClient.CreateCommand($"find {remotePath} -type f -iname \"*.zip\" 2>/dev/null");
                var result = command.Execute();

                var filePaths = result.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                flowLayoutPanel1.Controls.Clear();
                int fileCount = filePaths.Length;
                UpdateFileCounter(fileCount);

                foreach (var filePath in filePaths)
                {
                    var fileName = Path.GetFileName(filePath);

                    var btnDownload = new Button
                    {
                        Text = $"zip: {fileName} - download.",
                        Tag = filePath,
                        Height = 30,
                        Margin = new Padding(5),
                        FlatStyle = FlatStyle.Flat,
                        FlatAppearance =
                {
                    BorderSize = 1,
                    BorderColor = Color.Silver
                },
                        TextAlign = ContentAlignment.MiddleLeft
                    };
                    btnDownload.Width = flowLayoutPanel1.ClientSize.Width - 40;

                    btnDownload.Click += BtnDownload_Click;
                    flowLayoutPanel1.Controls.Add(btnDownload);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"File search error: {ex.Message}");
            }
        }
        private void BtnDownload_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var remoteFilePath = (string)button.Tag;
            var fileName = Path.GetFileName(remoteFilePath);

            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.FileName = fileName;
                saveDialog.Filter = "ZIP files (*.zip)|*.zip|All files (*.*)|*.*";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var fileStream = File.Create(saveDialog.FileName))
                        {
                            scpClient.Download(remoteFilePath, fileStream);
                        }

                        MessageBox.Show($"zip {fileName} successfully downloaded!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Connection error: {ex.Message}");
                    }
                }
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            scpClient?.Disconnect();
            scpClient?.Dispose();

            sshClient?.Disconnect();
            sshClient?.Dispose();
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }
        private void UpdateFileCounter(int count)
        {
            label1.Text = $"Found: {count} file";
            if (count == 0)
            {
                label1.ForeColor = Color.Red;
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SearchZipFiles(remotepath);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            remotepath = textBox1.Text;
        }
        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Enter the path to search for zip files.");
                    return;
                }
                remotepath = textBox1.Text;
                SearchZipFiles(remotepath);
            }
        }
        private void groupBox1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }
    }
}

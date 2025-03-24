using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            throw new Exception("exception from UI thread");
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                MessageBox.Show("await Task.Run() throwing exception");
                throw new Exception("await task run exception");
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                MessageBox.Show("Task.Run() throwing exception (no await)");
                throw new Exception("task run exception (no await)");
            });
        }
    }
}

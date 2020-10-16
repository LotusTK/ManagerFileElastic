using ManagerFile.ElasticModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagerFile
{
    public partial class ConsoleForm : Form
    {
        private ElasticModel.FileDAO dao;
        public ConsoleForm()
        {
            InitializeComponent();
        }

        private void ConsoleForm_Load(object sender, EventArgs e)
        {
            try
            {
                dao = new ElasticModel.FileDAO();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            File f = new File()
            {
                Name = "Test",
                Path = "test1/test2/test3",
                DateCreate = DateTime.UtcNow,
                Extension = ".txt",
                Content = "Sóng gió cuộc đời không bằng trăm năm đợi em..."
            };

            var response = dao.Create(f);
            txtConsole.Text = response.ToString();
        }

        private async void btnDelete_ClickAsync(object sender, EventArgs e)
        {
            //id là hashCode
            //var response = dao.Delete()
            var response = await dao.Delete("7fe1a332-4981-4804-9dee-17db8bc50561");
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            var response = dao.SearchAll();
            txtConsole.Text = response.ToString();
            var response2 = dao.SearchByField("Content", "Sóng gió cuộc đời");
            txtConsole.Text = response2.ToString();
        }

        private async void btnEdit_ClickAsync(object sender, EventArgs e)
        {
            File myFileTest = new File { Id = "7fe1a332-4981-4804-9dee-17db8bc50561", Content = "Test edit" };
            var response2 = await dao.Edit(myFileTest.Id, myFileTest);
        }
    }
}

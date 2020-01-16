using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Diagnostics;
using System.Reflection;

namespace WindowsFormsApplication1
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent(); //инициализируются все объекты формы
        }
        /// <summary>
        /// Действия при загрузке MenuForm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuForm_Load(object sender, EventArgs e)
        {
            Mouse.mice.Clear(); // Очистка списка мышей при загрузке / перезапуске формы
        }
        /// <summary>
        /// Нажатие на кнопку "Начать игру (один игрок)"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected internal void button1_Click(object sender, EventArgs e)
        {
            Mouse.mice.Clear();
            GameForm game_form = new GameForm();
            GameForm.current_mouse = 0;
            Mouse.Add();
            game_form.Owner = this;
            game_form.ShowDialog();
        }
        /// <summary>
        /// Нажатие на кнопку "Создать игру (на несколько игроков)"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            File.WriteAllBytes("Server.exe", Properties.Resources.Server);
            Process.Start("Server.exe");
        }
        /// <summary>
        /// Нажатие на кнопку "Подключение к игре"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            ConnectServerForm connect_server_form = new ConnectServerForm();
            connect_server_form.Owner = this;
            connect_server_form.Show();
        }
        /// <summary>
        /// Нажатие на кнопку "Выход"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            Mouse.mice.Clear();
            Application.Exit();
        }
    }
}
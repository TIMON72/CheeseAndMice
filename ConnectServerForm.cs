using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace WindowsFormsApplication1
{
    public partial class ConnectServerForm : Form
    {
        protected internal static string number_of_game; // Номер игры
        protected internal static int[,] map = new int[Server.number_of_strings, Server.number_of_columns]; // Инициализация карты
        static string IP_part1 { get; set; }
        static string IP_part2 { get; set; }
        static string IP_part3 { get; set; }
        static string IP_part4 { get; set; }

        public ConnectServerForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Нажатие на кнопку "Подключиться"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            IP_part1 = maskedTextBox1.Text;
            IP_part2 = maskedTextBox2.Text;
            IP_part3 = maskedTextBox3.Text;
            IP_part4 = maskedTextBox4.Text;
            string PORT_str = maskedTextBox5.Text;
            string IP;
            IP = IP_part1 + "." + IP_part2 + "." + IP_part3 + "." + IP_part4; // Формируем IP
            IP = IP.Replace(" ", ""); // Убираем оттуда пробелы, если они есть
            // Если IP введен в неверном формате
            if (IP_part1 == "" || IP_part2 == "" || IP_part3 == "" || IP_part4 == "" || PORT_str == "")
            {
                MessageBox.Show("Вы неверно ввели IP-адрес хоста");
            }
            // Если IP введен в правильном формате
            else
            {
                int PORT = Convert.ToInt32(PORT_str.Replace(" ", ""));
                Ping Pinger = new Ping();
                PingReply Reply = Pinger.Send(IP, 1);
                // Если соединение по выбранному IP существует, то устанавливаем его
                if (true || Reply.Status == IPStatus.Success)
                {
                    MenuForm menu_form = Owner as MenuForm;
                    menu_form.Hide(); // Скрываем форму с меню
                    Server.IP = IP; // Передаем IP в модуль Server для отправки команд на сервер
                    Server.PORT = PORT; // Передаем PORT в модуль Server для отправки команд на сервер
                    Server.StartOfGame(); // Заходим на сервер и ожидаем начала игры
                    GameForm game_form = new GameForm();
                    game_form.Owner = menu_form; // Задаем владельца формы с игрой
                    game_form.Show(); // Если сформирована команда, то начинаем игру
                    Close(); // Закрываем текущую форму
                }
                // Если соединения по выбранному IP не существует
                else
                {
                    MessageBox.Show("Ошибка соединения с хостом");
                }
            }
        }
    }
}
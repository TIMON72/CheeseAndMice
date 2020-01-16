using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Media;
using System.Diagnostics;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class GameForm : Form
    {
        // Инициализация игровых настроек
        protected internal static bool no_reply = false; // Защита ф-ции подведения итогов от таймера
        int time_of_effect; // Длительность эффектов
        double wall_speed = 1; // Скорость прохождения через стены
        double normal_speed { get; set; }
        // Инициализация клавиш
        bool key_left_pressed = false;
        bool key_right_pressed = false;
        bool key_up_pressed = false;
        bool key_down_pressed = false;
        bool key_shift_pressed = false;
        // Инициализация бонусов, сыра, мышеловок
        bool bonus = false;
        bool cheese = false;
        bool trap = false;
        // Инициализация минут и секунд таймера
        int m = Server.minutes;
        int s = Server.seconds;
        // Инициализация матрицы
        protected internal static int[,] map = new int[Server.number_of_strings, Server.number_of_columns];
        // Инициализация мышей
        protected internal static int current_mouse = 0;
        int mouse0 = 0;
        int mouse1 = 1;
        int mouse2 = 2;
        int mouse3 = 3;

        public GameForm()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        /// <summary>
        /// Действия при загрузке GameForm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_Load(object sender, EventArgs e)
        {
            MenuForm menu_form = Owner as MenuForm;
            menu_form.Hide(); // Скрываем форму с меню
            map = Server.CreatingOfLevel(); // Получаем уровень игры
            GlobalTimer.Start(); // Разрешаем передвижение игроков
            GameTimer.Start(); // Запускаем таймер игры
        }
        /// <summary>
        /// Действия после закрытия GameForm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            no_reply = false;
            GameTimer.Stop(); // Останавливаем таймер игры
            EffectTimer.Stop(); // Останавливаем действующие эффекты
            GlobalTimer.Stop(); // Останавливаем передвижение игроков
            ServerTimer.Stop(); // Останавливаем обмен данных с сервером
            MenuForm menu_form = Owner as MenuForm;
            menu_form.Show(); // Вызываем FormMenu
        }
        /// <summary>
        /// Действие при нажатии клавиши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                key_left_pressed = true;
                key_right_pressed = false;
                key_up_pressed = false;
                key_down_pressed = false;
                Mouse.mice[current_mouse].image_mouse = Server.image_mouse_left;
            }
            if (e.KeyCode == Keys.Right)
            {
                key_left_pressed = false;
                key_right_pressed = true;
                key_up_pressed = false;
                key_down_pressed = false;
                Mouse.mice[current_mouse].image_mouse = Server.image_mouse_right;
            }
            if (e.KeyCode == Keys.Up)
            {
                key_left_pressed = false;
                key_right_pressed = false;
                key_up_pressed = true;
                key_down_pressed = false;
                Mouse.mice[current_mouse].image_mouse = Server.image_mouse_up;
            }
            if (e.KeyCode == Keys.Down)
            {
                key_left_pressed = false;
                key_right_pressed = false;
                key_up_pressed = false;
                key_down_pressed = true;
                Mouse.mice[current_mouse].image_mouse = Server.image_mouse_down;
            }
            if (e.KeyCode == Keys.ShiftKey)
            {
                Mouse.mice[current_mouse].speed = wall_speed;
                key_shift_pressed = true;
            }
        }
        /// <summary>
        /// Действие при отжатии клавиши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            key_left_pressed = false;
            key_right_pressed = false;
            key_up_pressed = false;
            key_down_pressed = false;
            if (e.KeyCode == Keys.ShiftKey)
            {
                Mouse.mice[current_mouse].speed = normal_speed;
                key_shift_pressed = false;
            }
        }
        /// <summary>
        /// Рисование карты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < Server.number_of_strings; i++)
                for (int j = 0; j < Server.number_of_columns; j++)
                {
                    switch (map[i, j])
                    {
                        case 0: // трава
                            {
                                e.Graphics.DrawImage(Server.image_grass, Server.shift_of_map_Left + j * Server.size_of_cell, Server.shift_of_map_Top + i * Server.size_of_cell, Server.size_of_cell, Server.size_of_cell);
                                break;
                            }
                        case 1: // непрогрызаемая стена
                            {
                                e.Graphics.DrawImage(Server.image_wall, Server.shift_of_map_Left + j * Server.size_of_cell, Server.shift_of_map_Top + i * Server.size_of_cell, Server.size_of_cell, Server.size_of_cell);
                                break;
                            }
                        case 2: // бонус
                            {
                                e.Graphics.DrawImage(Server.image_bonus, Server.shift_of_map_Left + j * Server.size_of_cell, Server.shift_of_map_Top + i * Server.size_of_cell, Server.size_of_cell, Server.size_of_cell);
                                break;
                            }
                        case 3: // сыр
                            {
                                e.Graphics.DrawImage(Server.image_cheese, Server.shift_of_map_Left + j * Server.size_of_cell, Server.shift_of_map_Top + i * Server.size_of_cell, Server.size_of_cell, Server.size_of_cell);
                                break;
                            }
                        case 4: // обычная стена
                            {
                                e.Graphics.DrawImage(Server.image_wall, Server.shift_of_map_Left + j * Server.size_of_cell, Server.shift_of_map_Top + i * Server.size_of_cell, Server.size_of_cell, Server.size_of_cell);
                                break;
                            }
                        case 5: // мышеловка
                            {
                                e.Graphics.DrawImage(Server.image_trap, Server.shift_of_map_Left + j * Server.size_of_cell, Server.shift_of_map_Top + i * Server.size_of_cell, Server.size_of_cell, Server.size_of_cell);
                                break;
                            }
                    }
                }
            // Если мышь не одна, то прорисовываем остальных
            if (Mouse.mice.Count == 4)
            {
                e.Graphics.DrawImage(Mouse.mice[mouse0].image_mouse, Mouse.mice[mouse0].position);
                e.Graphics.DrawImage(Mouse.mice[mouse1].image_mouse, Mouse.mice[mouse1].position);
                e.Graphics.DrawImage(Mouse.mice[mouse2].image_mouse, Mouse.mice[mouse2].position);
                e.Graphics.DrawImage(Mouse.mice[mouse3].image_mouse, Mouse.mice[mouse3].position);
                ServerTimer.Start(); // Запускаем цикличную отправку/получения данных с сервера
            }
            // Прорисовываем текущую мышь
            e.Graphics.DrawImage(Mouse.mice[current_mouse].image_mouse, Mouse.mice[current_mouse].position);
            // Отображение некоторых данных на экране
            p1_number.Text = "Игрок №" + Convert.ToString(mouse0 + 1);
            p1_cheese.Text = "Собрано сыра: " + Mouse.mice[mouse0].number_of_cheeses.ToString();
            if (Mouse.mice.Count == 4)
            {
                p2_number.Text = "Игрок №" + Convert.ToString(mouse1 + 1);
                p2_cheese.Text = "Собрано сыра: " + Mouse.mice[mouse1].number_of_cheeses.ToString();
                p3_number.Text = "Игрок №" + Convert.ToString(mouse2 + 1);
                p3_cheese.Text = "Собрано сыра: " + Mouse.mice[mouse2].number_of_cheeses.ToString();
                p4_number.Text = "Игрок №" + Convert.ToString(mouse3 + 1);
                p4_cheese.Text = "Собрано сыра: " + Mouse.mice[mouse3].number_of_cheeses.ToString();
            }
        }
        /// <summary>
        /// Правила при движении
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        protected internal bool CheckingOfMovement(int left, int top, string direction, int index)
        {
            bool ok = false;
            int i1 = 0;
            int j1 = 0;
            double double_i = (Convert.ToDouble(top) - Server.shift_of_map_Top) / Server.size_of_cell;
            double double_j = (Convert.ToDouble(left) - Server.shift_of_map_Left) / Server.size_of_cell;
            int i_left_edge = Convert.ToInt32(Math.Floor(double_i));
            int i_right_edge = Convert.ToInt32(Math.Ceiling(double_i));
            int j_left_edge = Convert.ToInt32(Math.Floor(double_j));
            int j_right_edge = Convert.ToInt32(Math.Ceiling(double_j));
            switch (direction)
            {
                case "left":
                    {
                        j1 = j_right_edge;
                        if (map[i_left_edge, j1 - 1] == 1 ||
                            map[i_right_edge, j1 - 1] == 1)
                        {
                            ok = false;
                        }
                        else if (map[i_left_edge, j1 - 1] == 4 ||
                            map[i_right_edge, j1 - 1] == 4)
                        {
                            if (key_shift_pressed)
                                ok = true;
                            else
                                ok = false;
                        }
                        else if (map[i_left_edge, j1 - 1] == 2 ||
                            map[i_right_edge, j1 - 1] == 2)
                        {
                            if (index == current_mouse)
                            {
                                bonus = true;
                            }
                            map[i_left_edge, j1 - 1] = 0;
                            map[i_right_edge, j1 - 1] = 0;
                        }
                        else if (map[i_left_edge, j1 - 1] == 3 ||
                            map[i_right_edge, j1 - 1] == 3)
                        {
                            if (index == current_mouse)
                            {
                                cheese = true;
                            }
                            map[i_left_edge, j1 - 1] = 0;
                            map[i_right_edge, j1 - 1] = 0;
                        }                       
                        else if (map[i_left_edge, j1 - 1] == 5 ||
                            map[i_right_edge, j1 - 1] == 5)
                        {
                            if (index == current_mouse)
                            {
                                trap = true;
                            }
                            if (current_mouse == 0)
                            {
                                Mouse.mice[current_mouse].start_X = 1;
                                Mouse.mice[current_mouse].start_Y = 1;
                                Mouse.mice[current_mouse].position = new Rectangle(Server.shift_of_map_Left + Mouse.mice[0].start_X * Server.size_of_cell, Server.shift_of_map_Top + Mouse.mice[0].start_Y * Server.size_of_cell, Server.size_of_cell, Server.size_of_cell);
                                Mouse.mice[current_mouse].image_mouse = Server.image_mouse_down;
                            }
                            else if (current_mouse == 1)
                            {
                                Mouse.mice[current_mouse].start_X = 1;
                                Mouse.mice[current_mouse].start_Y = Server.number_of_columns - 1;
                                Mouse.mice[current_mouse].position = new Rectangle(Server.shift_of_map_Left + Mouse.mice[1].start_X * Server.size_of_cell, Mouse.mice[1].start_Y * Server.size_of_cell, Server.size_of_cell, Server.size_of_cell);
                                Mouse.mice[current_mouse].image_mouse = Server.image_mouse_up;
                            }
                            else if (current_mouse == 2)
                            {
                                Mouse.mice[current_mouse].start_X = Server.number_of_strings - 1;
                                Mouse.mice[current_mouse].start_Y = 1;
                                Mouse.mice[current_mouse].position = new Rectangle(Mouse.mice[2].start_X * Server.size_of_cell, Server.shift_of_map_Top + Mouse.mice[2].start_Y * Server.size_of_cell, Server.size_of_cell, Server.size_of_cell);
                                Mouse.mice[current_mouse].image_mouse = Server.image_mouse_down;
                            }
                            else if (current_mouse == 3)
                            {
                                Mouse.mice[current_mouse].start_X = Server.number_of_strings - 1;
                                Mouse.mice[current_mouse].start_Y = Server.number_of_columns - 1;
                                Mouse.mice[current_mouse].position = new Rectangle(Mouse.mice[3].start_X * Server.size_of_cell, Mouse.mice[3].start_Y * Server.size_of_cell, Server.size_of_cell, Server.size_of_cell);
                                Mouse.mice[current_mouse].image_mouse = Server.image_mouse_up;
                            }
                            map[i_left_edge, j1 - 1] = 0;
                            map[i_right_edge, j1 - 1] = 0;
                        }
                        else
                        {
                            ok = true;
                        }

                        break;
                    }
                case "right":
                    {
                        j1 = j_left_edge;
                        if (map[i_left_edge, j1 + 1] == 1 ||
                            map[i_right_edge, j1 + 1] == 1)
                        {
                            ok = false;
                        }

                        else if (map[i_left_edge, j1 + 1] == 4 ||
                            map[i_right_edge, j1 + 1] == 4)
                        {
                            if (key_shift_pressed)
                                ok = true;
                            else
                                ok = false;
                        }

                        else if (map[i_left_edge, j1 + 1] == 2 ||
                            map[i_right_edge, j1 + 1] == 2)
                        {
                            if (index == current_mouse)
                            {
                                bonus = true;
                            }
                            map[i_left_edge, j1 + 1] = 0;
                            map[i_right_edge, j1 + 1] = 0;
                        }

                        else if (map[i_left_edge, j1 + 1] == 3 ||
                            map[i_right_edge, j1 + 1] == 3)
                        {
                            if (index == current_mouse)
                            {
                                cheese = true;
                            }
                            map[i_left_edge, j1 + 1] = 0;
                            map[i_right_edge, j1 + 1] = 0;
                        }

                        else if (map[i_left_edge, j1 + 1] == 5 ||
                            map[i_right_edge, j1 + 1] == 5)
                        {
                            if (index == current_mouse)
                            {
                                trap = true;
                            }
                            if (current_mouse == 0)
                            {
                                Mouse.mice[current_mouse].start_X = 1;
                                Mouse.mice[current_mouse].start_Y = 1;
                                Mouse.mice[current_mouse].position = new Rectangle(Server.shift_of_map_Left + Mouse.mice[0].start_X * Server.size_of_cell, Server.shift_of_map_Top + Mouse.mice[0].start_Y * Server.size_of_cell, Server.size_of_cell, Server.size_of_cell);
                                Mouse.mice[current_mouse].image_mouse = Server.image_mouse_down;
                            }
                            else if (current_mouse == 1)
                            {
                                Mouse.mice[current_mouse].start_X = 1;
                                Mouse.mice[current_mouse].start_Y = Server.number_of_columns - 1;
                                Mouse.mice[current_mouse].position = new Rectangle(Server.shift_of_map_Left + Mouse.mice[1].start_X * Server.size_of_cell, Mouse.mice[1].start_Y * Server.size_of_cell, Server.size_of_cell, Server.size_of_cell);
                                Mouse.mice[current_mouse].image_mouse = Server.image_mouse_up;
                            }
                            else if (current_mouse == 2)
                            {
                                Mouse.mice[current_mouse].start_X = Server.number_of_strings - 1;
                                Mouse.mice[current_mouse].start_Y = 1;
                                Mouse.mice[current_mouse].position = new Rectangle(Mouse.mice[2].start_X * Server.size_of_cell, Server.shift_of_map_Top + Mouse.mice[2].start_Y * Server.size_of_cell, Server.size_of_cell, Server.size_of_cell);
                                Mouse.mice[current_mouse].image_mouse = Server.image_mouse_down;
                            }
                            else if (current_mouse == 3)
                            {
                                Mouse.mice[current_mouse].start_X = Server.number_of_strings - 1;
                                Mouse.mice[current_mouse].start_Y = Server.number_of_columns - 1;
                                Mouse.mice[current_mouse].position = new Rectangle(Mouse.mice[3].start_X * Server.size_of_cell, Mouse.mice[3].start_Y * Server.size_of_cell, Server.size_of_cell, Server.size_of_cell);
                                Mouse.mice[current_mouse].image_mouse = Server.image_mouse_up;
                            }
                            map[i_left_edge, j1 + 1] = 0;
                            map[i_right_edge, j1 + 1] = 0;
                        }
                        else
                        {
                            ok = true;
                        }
                        break;
                    }
                case "up":
                    {
                        i1 = i_right_edge;
                        if (map[i1 - 1, j_left_edge] == 1 ||
                            map[i1 - 1, j_right_edge] == 1)
                        {
                            ok = false;
                        }
                        else if (map[i1 - 1, j_left_edge] == 4 ||
                            map[i1 - 1, j_right_edge] == 4)
                        {
                            if (key_shift_pressed)
                                ok = true;
                            else
                                ok = false;
                        }
                        else if (map[i1 - 1, j_left_edge] == 2 ||
                            map[i1 - 1, j_right_edge] == 2)
                        {
                            if (index == current_mouse)
                            {
                                bonus = true;
                            }
                            map[i1 - 1, j_left_edge] = 0;
                            map[i1 - 1, j_right_edge] = 0;
                        }
                        else if (map[i1 - 1, j_left_edge] == 3 ||
                            map[i1 - 1, j_right_edge] == 3)
                        {
                            if (index == current_mouse)
                            {
                                cheese = true;
                            }
                            map[i1 - 1, j_left_edge] = 0;
                            map[i1 - 1, j_right_edge] = 0;
                        }
                        else if (map[i1 - 1, j_left_edge] == 5 ||
                            map[i1 - 1, j_right_edge] == 5)
                        {
                            if (index == current_mouse)
                            {
                                trap = true;
                            }
                            if (current_mouse == 0)
                            {
                                Mouse.mice[current_mouse].start_X = 1;
                                Mouse.mice[current_mouse].start_Y = 1;
                                Mouse.mice[current_mouse].position = new Rectangle(Server.shift_of_map_Left + Mouse.mice[0].start_X * Server.size_of_cell, Server.shift_of_map_Top + Mouse.mice[0].start_Y * Server.size_of_cell, Server.size_of_cell, Server.size_of_cell);
                                Mouse.mice[current_mouse].image_mouse = Server.image_mouse_down;
                            }
                            else if (current_mouse == 1)
                            {
                                Mouse.mice[current_mouse].start_X = 1;
                                Mouse.mice[current_mouse].start_Y = Server.number_of_columns - 1;
                                Mouse.mice[current_mouse].position = new Rectangle(Server.shift_of_map_Left + Mouse.mice[1].start_X * Server.size_of_cell, Mouse.mice[1].start_Y * Server.size_of_cell, Server.size_of_cell, Server.size_of_cell);
                                Mouse.mice[current_mouse].image_mouse = Server.image_mouse_up;
                            }
                            else if (current_mouse == 2)
                            {
                                Mouse.mice[current_mouse].start_X = Server.number_of_strings - 1;
                                Mouse.mice[current_mouse].start_Y = 1;
                                Mouse.mice[current_mouse].position = new Rectangle(Mouse.mice[2].start_X * Server.size_of_cell, Server.shift_of_map_Top + Mouse.mice[2].start_Y * Server.size_of_cell, Server.size_of_cell, Server.size_of_cell);
                                Mouse.mice[current_mouse].image_mouse = Server.image_mouse_down;
                            }
                            else if (current_mouse == 3)
                            {
                                Mouse.mice[current_mouse].start_X = Server.number_of_strings - 1;
                                Mouse.mice[current_mouse].start_Y = Server.number_of_columns - 1;
                                Mouse.mice[current_mouse].position = new Rectangle(Mouse.mice[3].start_X * Server.size_of_cell, Mouse.mice[3].start_Y * Server.size_of_cell, Server.size_of_cell, Server.size_of_cell);
                                Mouse.mice[current_mouse].image_mouse = Server.image_mouse_up;
                            }
                            map[i1 - 1, j_left_edge] = 0;
                            map[i1 - 1, j_right_edge] = 0;
                        }
                        else
                        {
                            ok = true;
                        }
                        break;
                    }
                case "down":
                    {
                        i1 = i_left_edge;
                        if (map[i1 + 1, j_left_edge] == 1 ||
                            map[i1 + 1, j_right_edge] == 1)
                        {
                            ok = false;
                        }
                        else if (map[i1 + 1, j_left_edge] == 4 ||
                             map[i1 + 1, j_right_edge] == 4)
                        {
                            if (key_shift_pressed)
                                ok = true;
                            else
                                ok = false;
                        }
                        else if (map[i1 + 1, j_left_edge] == 2 ||
                            map[i1 + 1, j_right_edge] == 2)
                        {
                            if (index == current_mouse)
                            {
                                bonus = true;
                            }
                            map[i1 + 1, j_left_edge] = 0;
                            map[i1 + 1, j_right_edge] = 0;
                        }
                        else if (map[i1 + 1, j_left_edge] == 3 ||
                            map[i1 + 1, j_right_edge] == 3)
                        {
                            if (index == current_mouse)
                            {
                                cheese = true;
                            }
                            map[i1 + 1, j_left_edge] = 0;
                            map[i1 + 1, j_right_edge] = 0;
                        }
                        else if (map[i1 + 1, j_left_edge] == 5 ||
                            map[i1 + 1, j_right_edge] == 5)
                        {
                            if (index == current_mouse)
                            {
                                trap = true;
                            }
                            if (current_mouse == 0)
                            {
                                Mouse.mice[current_mouse].start_X = 1;
                                Mouse.mice[current_mouse].start_Y = 1;
                                Mouse.mice[current_mouse].position = new Rectangle(Server.shift_of_map_Left + Mouse.mice[0].start_X * Server.size_of_cell, Server.shift_of_map_Top + Mouse.mice[0].start_Y * Server.size_of_cell, Server.size_of_cell, Server.size_of_cell);
                                Mouse.mice[current_mouse].image_mouse = Server.image_mouse_down;
                            }
                            else if (current_mouse == 1)
                            {
                                Mouse.mice[current_mouse].start_X = 1;
                                Mouse.mice[current_mouse].start_Y = Server.number_of_columns - 1;
                                Mouse.mice[current_mouse].position = new Rectangle(Server.shift_of_map_Left + Mouse.mice[1].start_X * Server.size_of_cell, Mouse.mice[1].start_Y * Server.size_of_cell, Server.size_of_cell, Server.size_of_cell);
                                Mouse.mice[current_mouse].image_mouse = Server.image_mouse_up;
                            }
                            else if (current_mouse == 2)
                            {
                                Mouse.mice[current_mouse].start_X = Server.number_of_strings - 1;
                                Mouse.mice[current_mouse].start_Y = 1;
                                Mouse.mice[current_mouse].position = new Rectangle(Mouse.mice[2].start_X * Server.size_of_cell, Server.shift_of_map_Top + Mouse.mice[2].start_Y * Server.size_of_cell, Server.size_of_cell, Server.size_of_cell);
                                Mouse.mice[current_mouse].image_mouse = Server.image_mouse_down;
                            }
                            else if (current_mouse == 3)
                            {
                                Mouse.mice[current_mouse].start_X = Server.number_of_strings - 1;
                                Mouse.mice[current_mouse].start_Y = Server.number_of_columns - 1;
                                Mouse.mice[current_mouse].position = new Rectangle(Mouse.mice[3].start_X * Server.size_of_cell, Mouse.mice[3].start_Y * Server.size_of_cell, Server.size_of_cell, Server.size_of_cell);
                                Mouse.mice[current_mouse].image_mouse = Server.image_mouse_up;
                            }
                            map[i1 + 1, j_left_edge] = 0;
                            map[i1 + 1, j_right_edge] = 0;
                        }
                        else
                        {
                            ok = true;
                        }
                        break;
                    }
            }
            return ok;
        }
        /// <summary>
        /// Пересоздание изображения (имитация движения)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GlobalTimer_Tick(object sender, EventArgs e)
        {
            // Если не зажат шифт, то считываем текущую скорость мыши
            if (!key_shift_pressed)
            {
                normal_speed = Mouse.mice[current_mouse].speed;
            }
            MovementOfMouse();
            Refresh();
        }
        /// <summary>
        /// Автоцентрирование мыши
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int AutoAligment(int x, int y)
        {
            double double_x = (Convert.ToDouble(x) - Server.shift_of_map_Top) / Server.size_of_cell;
            double double_y = (Convert.ToDouble(y) - Server.shift_of_map_Left) / Server.size_of_cell;
            double x1 = Math.Abs(double_x - Math.Floor(double_x));
            double y1 = Math.Abs(double_y - Math.Floor(double_y));
            double low_limit = 0.15;
            double high_limit = 0.85;
            if (x1 > high_limit)
                return 1;
            else if (x1 < low_limit && x1 > 0)
                return 2;
            else if (y1 > high_limit)
                return 3;
            else if (y1 < low_limit && y1 > 0)
                return 4;
            else
                return 0;
        }
        /// <summary>
        /// Изменение координат мыши и действия при нахождении бонусов и сыра
        /// </summary>
        private void MovementOfMouse()
        {
            int x = Mouse.mice[current_mouse].position.Left - Server.shift_of_map_Left; // Координата X текущей мыши
            int y = Mouse.mice[current_mouse].position.Top - Server.shift_of_map_Top; // Координата Y текущей мыши
            int step = Convert.ToInt32(Mouse.mice[current_mouse].speed); // Шаг
            double double_x = (Convert.ToDouble(x)) / Server.size_of_cell; // Позиция X внутри ячейки
            double double_y = (Convert.ToDouble(y)) / Server.size_of_cell; // Позиция Y внутри ячейки
            // Нажата клавиша влево
            if (key_left_pressed)
            {
                // Проверка на преграды и прочее
                if (CheckingOfMovement(Mouse.mice[current_mouse].position.Left, Mouse.mice[current_mouse].position.Top, "left", current_mouse))
                {
                    Mouse.mice[current_mouse].position.Location = new Point(Mouse.mice[current_mouse].position.Left - step, Mouse.mice[current_mouse].position.Top);
                }
            }
            // Нажата клавиша вправо
            if (key_right_pressed)
            {
                // Проверка на преграды и прочее
                if (CheckingOfMovement(Mouse.mice[current_mouse].position.Left, Mouse.mice[current_mouse].position.Top, "right", current_mouse))
                {
                    Mouse.mice[current_mouse].position.Location = new Point(Mouse.mice[current_mouse].position.Left + step, Mouse.mice[current_mouse].position.Top);
                }
            }
            // Нажата клавиша вверх
            if (key_up_pressed)
            {
                // Проверка на преграды и прочее
                if (CheckingOfMovement(Mouse.mice[current_mouse].position.Left, Mouse.mice[current_mouse].position.Top, "up", current_mouse))
                {
                    Mouse.mice[current_mouse].position.Location = new Point(Mouse.mice[current_mouse].position.Left, Mouse.mice[current_mouse].position.Top - step);
                }
            }
            // Нажата клавиша вниз
            if (key_down_pressed)
            {
                // Проверка на преграды и прочее
                if (CheckingOfMovement(Mouse.mice[current_mouse].position.Left, Mouse.mice[current_mouse].position.Top, "down", current_mouse))
                {
                    Mouse.mice[current_mouse].position.Location = new Point(Mouse.mice[current_mouse].position.Left, Mouse.mice[current_mouse].position.Top + step);
                }
            }
            // Если никакая клавиша не нажата
            if (!key_left_pressed && !key_right_pressed && !key_up_pressed && !key_down_pressed)
            {
                int result = AutoAligment(Mouse.mice[current_mouse].position.Left, Mouse.mice[current_mouse].position.Top);
                // Проверка на автоцентровку (выравнивание по правому краю)
                if (result == 1)
                {
                    int coordinate_X_of_right_cell = Convert.ToInt32(Math.Ceiling(double_x) * Server.size_of_cell + Server.shift_of_map_Left);
                    Mouse.mice[current_mouse].position.Location = new Point(coordinate_X_of_right_cell, y + Server.shift_of_map_Left);
                }
                // Проверка на автоцентровку (выравнивание по левому краю)
                if (result == 2)
                {
                    int coordinate_X_of_left_cell = Convert.ToInt32(Math.Floor(double_x) * Server.size_of_cell + Server.shift_of_map_Left);
                    Mouse.mice[current_mouse].position.Location = new Point(coordinate_X_of_left_cell, y + Server.shift_of_map_Left);
                }
                // Проверка на автоцентровку (выравнивание по нижнему краю)
                if (result == 3)
                {
                    int coordinate_Y_of_down_cell = Convert.ToInt32(Math.Ceiling(double_y) * Server.size_of_cell + Server.shift_of_map_Top);
                    Mouse.mice[current_mouse].position.Location = new Point(x + Server.shift_of_map_Left, coordinate_Y_of_down_cell);
                }
                // Проверка на автоцентровку (выравнивание по верхнему краю)
                if (result == 4)
                {
                    int coordinate_Y_of_up_cell = Convert.ToInt32(Math.Floor(double_y) * Server.size_of_cell + Server.shift_of_map_Top);
                    Mouse.mice[current_mouse].position.Location = new Point(x + Server.shift_of_map_Left, coordinate_Y_of_up_cell);
                }
            }
            // Если подобрали бонус
            if (bonus)
            {
                bonus = false;
                Mouse.mice[current_mouse].speed = 3;
                EffectTimer.Stop(); // Останавливаем таймер, если он запущен
                time_of_effect = 10;
                EffectTimer.Start(); // Запускаем таймер эффекта бонуса
                Server.bonus_sound.Play();
            }
            // Если подобрали сыр
            if (cheese)
            {
                cheese = false;
                Mouse.mice[current_mouse].number_of_cheeses++;
                Server.cheese_sound.Play();
            }
            // Если наткнулись на мышеловку
            if (trap)
            {
                trap = false;
                Mouse.mice[current_mouse].speed = 1;
                EffectTimer.Stop(); // Останавливаем таймер, если он запущен
                time_of_effect = 10;
                EffectTimer.Start(); // Запускаем таймер эффекта бонуса
                Server.cheese_sound.Play();
            }
        }
        /// <summary>
        /// Таймер игры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            timer.Text = m + ":" + s;
            s = s - 1;
            if (s == -1)
            {
                m = m - 1;
                s = 59;
            }
            if (m == -1 && !no_reply)
            {
                timer.Text = "Время вышло";
                GameTimer.Stop(); // Останавливаем таймер игры
                EffectTimer.Stop(); // Останавливаем действующие эффекты
                GlobalTimer.Stop(); // Останавливаем передвижение игроков
                if (Mouse.mice.Count == 4)
                {
                    ServerTimer.Stop(); // Прекращаем обмен данных с сервером
                    Server.ConclusionOfGame(); // Подведение итогов
                    Close();
                }
                else if (Mouse.mice.Count == 1)
                {
                    MessageBox.Show("Время вышло!", "Конец игры");
                    Close();
                }
                no_reply = true;
            }
        }
        /// <summary>
        /// Таймер запросов на сервер
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < Mouse.mice.Count; i++)
            {
                if (i == current_mouse)
                {
                    i++;
                }
                else
                {
                    Server.UpdateLevel(); // Обновление уровня
                    Mouse.mice[i].position.Location = new Point(Mouse.mice[i].position.Left, Mouse.mice[i].position.Top);
                }
            }
        }
        /// <summary>
        /// Таймер действия эффектов (иначе - баффов)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EffectTimer_Tick(object sender, EventArgs e)
        {
            time_of_effect = time_of_effect - 1;
            // Если таймер закончился
            if (time_of_effect == -1)
            {
                EffectTimer.Stop();
                // Если действовало ускорение, то возвращаем исходную скорость
                if (Mouse.mice[current_mouse].speed == 3)
                {
                    Mouse.mice[current_mouse].speed = 2;
                }
                // Если действовало замедление, то возвращаем исходную скорость
                if (Mouse.mice[current_mouse].speed == 1)
                {
                    Mouse.mice[current_mouse].speed = 2;
                }
            }
        }
    }
}
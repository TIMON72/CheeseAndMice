using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;

namespace WindowsFormsApplication1
{
    class Mouse
    {
        protected internal int start_X { get; set; } // Начальная координата X
        protected internal int start_Y { get; set; } // Начальная координата Y
        protected internal double speed { get; set; } // Скорость мыши
        protected internal Rectangle position; // Позиция мыши
        protected internal Image image_mouse; // Изображение мыши
        protected internal int number_of_cheeses { get; set; } // Количество сыра
        protected internal static List<Mouse> mice = new List<Mouse>(); // Список мышей

        /// <summary>
        /// Добавление мышей в список
        /// </summary>
        protected internal static void Add()
        {
            Mouse new_mouse = new Mouse();
            new_mouse.speed = 2;
            new_mouse.number_of_cheeses = 0;
            mice.Add(new_mouse);
        }
    }
    class Server
    {
        // Инициализация текстур
        protected internal static SoundPlayer bonus_sound = new SoundPlayer(Properties.Resources.bonus_sound); // Звук при подборе бонуса
        protected internal static SoundPlayer cheese_sound = new SoundPlayer(Properties.Resources.cheese_sound); // Звук при подборе сыра
        protected internal static Image image_wall = Properties.Resources.wall_picture; // Текстура стены
        protected internal static Image image_grass = Properties.Resources.grass_picture; // Текстура травы
        protected internal static Image image_cheese = Properties.Resources.cheese_picture; // Текстура сыра
        protected internal static Image image_bonus = Properties.Resources.bonus_picture; // Текстура бонуса
        protected internal static Image image_trap = Properties.Resources.trap_picture; // Текстура мышеловки
        protected internal static Image image_mouse_left = Properties.Resources.mouse_left_picture; // Текстура мыши налево
        protected internal static Image image_mouse_right = Properties.Resources.mouse_right_picture;  // Текстура мыши направо
        protected internal static Image image_mouse_down = Properties.Resources.mouse_down_picture; // Текстура мыши вниз
        protected internal static Image image_mouse_up = Properties.Resources.mouse_up_picture; // Текстура мыши вверх
        // Настройки для сервера
        protected internal static int minutes = 1; // Время игры (минуты)
        protected internal static int seconds = 0; // Время игры (секунды)
        protected internal static string number_of_game { get; set; } // Номер игры
        private const string step1 = "1"; // Действие один на сервере
        private const string step2 = "2"; // Действие два на сервере
        private const string step3 = "3"; // Действие три на сервере
        protected internal static string IP = ""; // IP-адрес сервера
        protected internal static int PORT; // Выбор порта
        // Настройки уровня
        protected internal const int size_of_cell = 30; // Размер ячейки
        protected internal const int shift_of_map_Left = 30; // Отступ поля слева
        protected internal const int shift_of_map_Top = 30; // Отступ поля сверху
        protected internal const int number_of_strings = 20; // Количество строк (делать четным)
        protected internal const int number_of_columns = 20; // количество столбцов (делать четным)
        protected internal static int[,] map = new int[number_of_strings, number_of_columns]; // Инициализация матрицы уровня

        /// <summary>
        /// Создание уровня
        /// </summary>
        protected internal static int[,] CreatingOfLevel()
        {
            if (Mouse.mice.Count == 4) // Если четыре игрока
            {
                // Размещение и прорисовка мышей
                Mouse.mice[0].start_X = 1;
                Mouse.mice[0].start_Y = 1;
                Mouse.mice[0].position = new Rectangle(shift_of_map_Left + Mouse.mice[0].start_X * size_of_cell, shift_of_map_Top + Mouse.mice[0].start_Y * size_of_cell, size_of_cell, size_of_cell);
                Mouse.mice[0].image_mouse = image_mouse_down;
                Mouse.mice[1].start_X = 1;
                Mouse.mice[1].start_Y = number_of_columns - 1;
                Mouse.mice[1].position = new Rectangle(shift_of_map_Left + Mouse.mice[1].start_X * size_of_cell, Mouse.mice[1].start_Y * size_of_cell, size_of_cell, size_of_cell);
                Mouse.mice[1].image_mouse = image_mouse_up;
                Mouse.mice[2].start_X = number_of_strings - 1;
                Mouse.mice[2].start_Y = 1;
                Mouse.mice[2].position = new Rectangle(Mouse.mice[2].start_X * size_of_cell, shift_of_map_Top + Mouse.mice[2].start_Y * size_of_cell, size_of_cell, size_of_cell);
                Mouse.mice[2].image_mouse = image_mouse_down;
                Mouse.mice[3].start_X = number_of_strings - 1;
                Mouse.mice[3].start_Y = number_of_columns - 1;
                Mouse.mice[3].position = new Rectangle(Mouse.mice[3].start_X * size_of_cell, Mouse.mice[3].start_Y * size_of_cell, size_of_cell, size_of_cell);
                Mouse.mice[3].image_mouse = image_mouse_up;
                // Передача полученной карты с сервера клиенту
                return map;
            }
            if (Mouse.mice.Count == 1) // Если один игрок
            {
                // Размещение и прорисовка мыши
                Mouse.mice[0].start_X = 1;
                Mouse.mice[0].start_Y = 1;
                Mouse.mice[0].position = new Rectangle(shift_of_map_Left + Mouse.mice[0].start_X * size_of_cell, shift_of_map_Top + Mouse.mice[0].start_Y * size_of_cell, size_of_cell, size_of_cell);
                Mouse.mice[0].image_mouse = image_mouse_down;
                // Создание крайних стен
                for (int i = 0; i < number_of_strings; i++)
                {
                    for (int j = 0; j < number_of_columns; j++)
                    {
                        map[0, j] = 1;
                        map[number_of_strings - 1, j] = 1;
                        map[i, 0] = 1;
                        map[i, number_of_columns - 1] = 1;
                    }
                }
                // Генерация случайных стен, бонусов внутри map
                Random rnd = new Random();
                for (int i = 2; i < number_of_strings - 2; i = i + 2)
                {
                    for (int j = 1; j < number_of_columns - 1; j++)
                    {
                        map[i, j] = rnd.Next(0, 100);
                        if (map[i, j] % 47 == 0)
                        {
                            map[i, j] = 2;
                        }
                        else if (map[i, j] % 3 == 0)
                        {
                            map[i, j] = 3;
                        }
                        else if (map[i, j] % 2 == 0)
                        {
                            map[i, j] = 4;
                        }
                        else if (map[i, j] % 5 == 0)
                        {
                            map[i, j] = 5;
                        }
                        else
                        {
                            map[i, j] = 0;
                        }
                    }
                }
                for (int j = 2; j < number_of_strings - 2; j = j + 2)
                {
                    // 
                    for (int i = 1; i < number_of_columns - 1; i++)
                    {
                        map[i, j] = rnd.Next(0, 100);
                        if (map[i, j] % 47 == 0)
                        {
                            map[i, j] = 2;
                        }
                        else if (map[i, j] % 3 == 0)
                        {
                            map[i, j] = 3;
                        }
                        else if (map[i, j] % 2 == 0)
                        {
                            map[i, j] = 4;
                        }
                        else if (map[i, j] % 5 == 0)
                        {
                            map[i, j] = 5;
                        }
                        else
                        {
                            map[i, j] = 0;
                        }
                    }
                }
                return map;
            }
            return map;
        }
        /// <summary>
        /// Ф-ция передачи сообщения на сервер
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected internal static string SendMessage(string message)
        {
            try {
                IPHostEntry ipHost = Dns.GetHostEntry(IP);
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, PORT);
                byte[] bytes = new byte[1024];
                Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                sender.Connect(ipEndPoint); // Подсоединение к серверу
                // Преобразование в байтовый тип (для отправки сообщения)
                byte[] msg = Encoding.UTF8.GetBytes(message);
                // Отправляем данные через сокет
                int bytesSent = sender.Send(msg);
                // Получаем ответ от сервера
                int bytesRec = sender.Receive(bytes);
                // Преобразование из байтового типа (для получения сообщения)
                string reply = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                sender.Shutdown(SocketShutdown.Both); // Отсоединение от сервера
                sender.Close(); // Закрытие текущего соединения
                if (reply == "")
                    reply = SendMessage(message);
                return reply;
            }
            catch (Exception ex)
            {
                string error = Convert.ToString(ex);
                Thread.Sleep(100);
                string m = SendMessage(message);
                if (m == "")
                {
                    m = SendMessage(message);
                }
                return m;
            }
        }
        /// <summary>
        /// Ф-ция начала игры
        /// </summary>
        protected internal static void StartOfGame()
        {
            string answer;
            number_of_game = "000";
            Mouse.mice.Clear(); // Очистка списка мышей
            // Выполняем, пока не придет ответ с сервера с индексом игрока и картой
            do
            {
                Thread.Sleep(1000); // Задержка отправки данных
                answer = SendMessage(step1 + "-" + number_of_game);
                // Если номер команды 000, то присваиваем номер, пришедший с сервера
                if (number_of_game == "000")
                {
                    number_of_game = answer.Substring(2, 3);
                }
            } while (answer.Length <= 5);
            Mouse.Add(); Mouse.Add(); Mouse.Add(); Mouse.Add(); // Добавляем 4-х мышей
            GameForm.current_mouse = Convert.ToInt32(answer.Substring(6, 1)); // Присваиваем индекс 
            // текущей мыши, определенной сервером
            string arr = answer.Substring(8); // Получаем карту, сгенерированную сервером
            int t = 0; // Коретка, что передвигается при чтении символа
            for (int i = 0; i < number_of_strings; i++)
            {
                for (int j = 0; j < number_of_columns; j++)
                {
                    map[i, j] = Convert.ToInt32(arr.Substring(t, 1));
                    t++;
                }
            }
            Thread.Sleep(4000); // Пауза для завершения 1-го шага (для корректной работы)
        }
        /// <summary>
        /// Ф-ция запросов на сервер для обновления состояния на карте
        /// </summary>
        /// <param name="obj"></param>
        protected internal static void UpdateLevel()
        {
            string temp_str;
            int temp_int;
            string answer;
            int x = Mouse.mice[GameForm.current_mouse].position.Left; // Координата X текущей мыши
            string str_x = string.Format("{0:d3}", x);
            int y = Mouse.mice[GameForm.current_mouse].position.Top; // Координата Y текущей мыши
            string str_y = string.Format("{0:d3}", y);
            string t = Convert.ToString(GameForm.current_mouse); // Номер текущей мыши
            int k = Mouse.mice[GameForm.current_mouse].number_of_cheeses; // Количество собранного сыра текущей мыши
            string str_k = string.Format("{0:d3}", k);
            answer = SendMessage(step2 + "-" + number_of_game + "-" + str_x + "-" + str_y + "-" + t + "-0-" + str_k); // Отправляем данные текущей мыши
            // и получаем данные о других мышах
            string[] arr_answer = answer.Split(' '); // Разбиваем ответ на массивы, где каждый эл-т - данные об одной мыши
            // Заносим данные о каждой мыши в список мышей
            for (int i = 0; i < arr_answer.Length - 1; i++)
            {
                temp_int = Convert.ToInt32(arr_answer[i].Substring(14, 1)); // Получаем индекс мыши
                temp_str = arr_answer[i].Substring(6, 3); // Получаем координату X
                x = Convert.ToInt32(temp_str);
                temp_str = arr_answer[i].Substring(10, 3); // Получаем координату Y
                y = Convert.ToInt32(temp_str);

                if (temp_int != GameForm.current_mouse) // Если индекс мыши не равен индексу текущей мыши
                {
                    int previous_X = Mouse.mice[temp_int].position.Left; // Координата X мыши до получения значения с сервера
                    int previous_Y = Mouse.mice[temp_int].position.Top; // Координата Y мыши дo получения значения с сервера
                    
                    Mouse.mice[temp_int].position = new Rectangle(x, y, size_of_cell, size_of_cell); // Записываем координаты
                                                                                                     // мыши в список
                    temp_str = arr_answer[i].Substring(18, 3); // Получаем количество сыра у мыши
                    Mouse.mice[temp_int].number_of_cheeses = Convert.ToInt32(temp_str); // Записываем в список кол-во сыра
                                                                                        // Определяем картинку для мыши по ее смещению
                    GameForm game_form = new GameForm(); // Создаем экземпляр класса для использования ф-ций
                    if (previous_X - x > 0)
                    {
                        Mouse.mice[temp_int].image_mouse = image_mouse_left;
                        game_form.CheckingOfMovement(x, y, "left", temp_int);
                    }
                    else if (previous_X - x < 0)
                    {
                        Mouse.mice[temp_int].image_mouse = image_mouse_right;
                        game_form.CheckingOfMovement(x, y, "right", temp_int);
                    }
                    if (previous_Y - y > 0)
                    {
                        Mouse.mice[temp_int].image_mouse = image_mouse_up;
                        game_form.CheckingOfMovement(x, y, "up", temp_int);
                    }
                    else if (previous_Y - y < 0)
                    {
                        Mouse.mice[temp_int].image_mouse = image_mouse_down;
                        game_form.CheckingOfMovement(x, y, "down", temp_int);
                    }
                }
            }
        }
        /// <summary>
        /// Ф-ция подведения итогов игры и определения ее победителя
        /// </summary>
        protected internal static void ConclusionOfGame()
        {
            if (!GameForm.no_reply)
            {
                GameForm.no_reply = true;
                int index_of_winner; // Номер игрока-победителя
                int number_of_cheese_of_winner; // Количество сыра у победителя
                string answer; // Строка ответа с сервера
                string t = Convert.ToString(GameForm.current_mouse); // Номер текущей мыши
                int k = Mouse.mice[GameForm.current_mouse].number_of_cheeses; // Количество собранного сыра текущей мыши
                string str_k = string.Format("{0:d3}", k);
                do
                {
                    answer = SendMessage(step3 + "-" + number_of_game + "-" + t + "-" + str_k); // Отправляем данные текущей мыши
                    Thread.Sleep(500);
                } while (answer == "3");
                // и получаем данные о других мышах
                index_of_winner = Convert.ToInt32(answer.Substring(2, 1)); // Получаем индекс мыши-победителя
                number_of_cheese_of_winner = Convert.ToInt32(answer.Substring(4, 3)); // Получаем количество сыра мыши-победителя
                if (index_of_winner == GameForm.current_mouse)
                {
                    MessageBox.Show("Поздравляем! Вы победили! Хотите сыграть еще одну игру?", "Конец игры");
                }
                else
                {
                    MessageBox.Show("Победил игрок под номером " +
                        Convert.ToString(index_of_winner + 1), "Конец игры");
                }
            }
        }
    }
}
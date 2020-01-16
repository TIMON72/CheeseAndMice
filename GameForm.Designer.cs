namespace WindowsFormsApplication1
{
    partial class GameForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            this.GlobalTimer = new System.Windows.Forms.Timer(this.components);
            this.GameTimer = new System.Windows.Forms.Timer(this.components);
            this.p1_cheese = new System.Windows.Forms.Label();
            this.менюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.создатьИгруToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.подключитьсяToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.начатьСНачалаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer = new System.Windows.Forms.Label();
            this.ServerTimer = new System.Windows.Forms.Timer(this.components);
            this.p1_number = new System.Windows.Forms.Label();
            this.p2_number = new System.Windows.Forms.Label();
            this.p2_cheese = new System.Windows.Forms.Label();
            this.p3_number = new System.Windows.Forms.Label();
            this.p3_cheese = new System.Windows.Forms.Label();
            this.p4_number = new System.Windows.Forms.Label();
            this.p4_cheese = new System.Windows.Forms.Label();
            this.EffectTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // GlobalTimer
            // 
            this.GlobalTimer.Interval = 1;
            this.GlobalTimer.Tick += new System.EventHandler(this.GlobalTimer_Tick);
            // 
            // GameTimer
            // 
            this.GameTimer.Interval = 1000;
            this.GameTimer.Tick += new System.EventHandler(this.GameTimer_Tick);
            // 
            // p1_cheese
            // 
            this.p1_cheese.AutoSize = true;
            this.p1_cheese.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.p1_cheese.ForeColor = System.Drawing.Color.DarkRed;
            this.p1_cheese.Location = new System.Drawing.Point(638, 56);
            this.p1_cheese.Name = "p1_cheese";
            this.p1_cheese.Size = new System.Drawing.Size(11, 13);
            this.p1_cheese.TabIndex = 0;
            this.p1_cheese.Text = "-";
            // 
            // менюToolStripMenuItem
            // 
            this.менюToolStripMenuItem.Name = "менюToolStripMenuItem";
            this.менюToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // создатьИгруToolStripMenuItem
            // 
            this.создатьИгруToolStripMenuItem.Name = "создатьИгруToolStripMenuItem";
            this.создатьИгруToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // подключитьсяToolStripMenuItem
            // 
            this.подключитьсяToolStripMenuItem.Name = "подключитьсяToolStripMenuItem";
            this.подключитьсяToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // начатьСНачалаToolStripMenuItem
            // 
            this.начатьСНачалаToolStripMenuItem.Name = "начатьСНачалаToolStripMenuItem";
            this.начатьСНачалаToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // timer
            // 
            this.timer.AutoSize = true;
            this.timer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.timer.ForeColor = System.Drawing.Color.DarkBlue;
            this.timer.Location = new System.Drawing.Point(312, 3);
            this.timer.Name = "timer";
            this.timer.Size = new System.Drawing.Size(146, 24);
            this.timer.TabIndex = 2;
            this.timer.Text = "Время пошло!";
            // 
            // ServerTimer
            // 
            this.ServerTimer.Interval = 1;
            this.ServerTimer.Tick += new System.EventHandler(this.ServerTimer_Tick);
            // 
            // p1_number
            // 
            this.p1_number.AutoSize = true;
            this.p1_number.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.p1_number.ForeColor = System.Drawing.Color.DarkRed;
            this.p1_number.Location = new System.Drawing.Point(638, 43);
            this.p1_number.Name = "p1_number";
            this.p1_number.Size = new System.Drawing.Size(11, 13);
            this.p1_number.TabIndex = 6;
            this.p1_number.Text = "-";
            // 
            // p2_number
            // 
            this.p2_number.AutoSize = true;
            this.p2_number.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.p2_number.ForeColor = System.Drawing.Color.DarkBlue;
            this.p2_number.Location = new System.Drawing.Point(638, 69);
            this.p2_number.Name = "p2_number";
            this.p2_number.Size = new System.Drawing.Size(11, 13);
            this.p2_number.TabIndex = 9;
            this.p2_number.Text = "-";
            // 
            // p2_cheese
            // 
            this.p2_cheese.AutoSize = true;
            this.p2_cheese.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.p2_cheese.ForeColor = System.Drawing.Color.DarkBlue;
            this.p2_cheese.Location = new System.Drawing.Point(638, 82);
            this.p2_cheese.Name = "p2_cheese";
            this.p2_cheese.Size = new System.Drawing.Size(11, 13);
            this.p2_cheese.TabIndex = 8;
            this.p2_cheese.Text = "-";
            // 
            // p3_number
            // 
            this.p3_number.AutoSize = true;
            this.p3_number.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.p3_number.ForeColor = System.Drawing.Color.DarkGreen;
            this.p3_number.Location = new System.Drawing.Point(638, 95);
            this.p3_number.Name = "p3_number";
            this.p3_number.Size = new System.Drawing.Size(11, 13);
            this.p3_number.TabIndex = 11;
            this.p3_number.Text = "-";
            // 
            // p3_cheese
            // 
            this.p3_cheese.AutoSize = true;
            this.p3_cheese.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.p3_cheese.ForeColor = System.Drawing.Color.DarkGreen;
            this.p3_cheese.Location = new System.Drawing.Point(638, 108);
            this.p3_cheese.Name = "p3_cheese";
            this.p3_cheese.Size = new System.Drawing.Size(11, 13);
            this.p3_cheese.TabIndex = 10;
            this.p3_cheese.Text = "-";
            // 
            // p4_number
            // 
            this.p4_number.AutoSize = true;
            this.p4_number.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.p4_number.ForeColor = System.Drawing.Color.DarkMagenta;
            this.p4_number.Location = new System.Drawing.Point(638, 121);
            this.p4_number.Name = "p4_number";
            this.p4_number.Size = new System.Drawing.Size(11, 13);
            this.p4_number.TabIndex = 13;
            this.p4_number.Text = "-";
            // 
            // p4_cheese
            // 
            this.p4_cheese.AutoSize = true;
            this.p4_cheese.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.p4_cheese.ForeColor = System.Drawing.Color.DarkMagenta;
            this.p4_cheese.Location = new System.Drawing.Point(638, 134);
            this.p4_cheese.Name = "p4_cheese";
            this.p4_cheese.Size = new System.Drawing.Size(11, 13);
            this.p4_cheese.TabIndex = 12;
            this.p4_cheese.Text = "-";
            // 
            // EffectTimer
            // 
            this.EffectTimer.Interval = 1000;
            this.EffectTimer.Tick += new System.EventHandler(this.EffectTimer_Tick);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gold;
            this.ClientSize = new System.Drawing.Size(754, 661);
            this.Controls.Add(this.p4_number);
            this.Controls.Add(this.p4_cheese);
            this.Controls.Add(this.p3_number);
            this.Controls.Add(this.p3_cheese);
            this.Controls.Add(this.p2_number);
            this.Controls.Add(this.p2_cheese);
            this.Controls.Add(this.p1_number);
            this.Controls.Add(this.timer);
            this.Controls.Add(this.p1_cheese);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Мышки и Сыр";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GameForm_FormClosed);
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GameForm_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer GlobalTimer;
        private System.Windows.Forms.Timer GameTimer;
        private System.Windows.Forms.Label p1_cheese;
        private System.Windows.Forms.ToolStripMenuItem менюToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem начатьСНачалаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.Label timer;
        private System.Windows.Forms.ToolStripMenuItem подключитьсяToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem создатьИгруToolStripMenuItem;
        private System.Windows.Forms.Timer ServerTimer;
        private System.Windows.Forms.Label p1_number;
        private System.Windows.Forms.Label p2_number;
        private System.Windows.Forms.Label p2_cheese;
        private System.Windows.Forms.Label p3_number;
        private System.Windows.Forms.Label p3_cheese;
        private System.Windows.Forms.Label p4_number;
        private System.Windows.Forms.Label p4_cheese;
        private System.Windows.Forms.Timer EffectTimer;
    }
}


namespace ATHLETIC_fitness2
{
    partial class CoachForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            button4 = new Button();
            label2 = new Label();
            button2 = new Button();
            richTextBox1 = new RichTextBox();
            dateTimePicker1 = new DateTimePicker();
            label1 = new Label();
            button1 = new Button();
            tabPage2 = new TabPage();
            groupBox1 = new GroupBox();
            button3 = new Button();
            textBox4 = new TextBox();
            textBox3 = new TextBox();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            pictureBox1 = new PictureBox();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(2, 1);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1366, 682);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.BackgroundImage = Properties.Resources._class;
            tabPage1.Controls.Add(button4);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(button2);
            tabPage1.Controls.Add(richTextBox1);
            tabPage1.Controls.Add(dateTimePicker1);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(button1);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1358, 649);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Home";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.BackColor = SystemColors.ActiveCaption;
            button4.Font = new Font("Times New Roman", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button4.Location = new Point(1187, 562);
            button4.Name = "button4";
            button4.Size = new Size(152, 81);
            button4.TabIndex = 6;
            button4.Text = "Exit";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = SystemColors.GradientActiveCaption;
            label2.Font = new Font("Times New Roman", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ActiveCaptionText;
            label2.Location = new Point(574, 32);
            label2.Name = "label2";
            label2.Size = new Size(294, 34);
            label2.TabIndex = 5;
            label2.Text = "Show workouts by date";
            // 
            // button2
            // 
            button2.BackColor = SystemColors.GradientActiveCaption;
            button2.Font = new Font("Times New Roman", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button2.Location = new Point(434, 161);
            button2.Name = "button2";
            button2.Size = new Size(557, 67);
            button2.TabIndex = 4;
            button2.Text = "Show";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = SystemColors.GradientInactiveCaption;
            richTextBox1.Font = new Font("Times New Roman", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextBox1.Location = new Point(434, 251);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(557, 383);
            richTextBox1.TabIndex = 3;
            richTextBox1.Text = "";
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.CalendarForeColor = SystemColors.ActiveCaption;
            dateTimePicker1.Font = new Font("Times New Roman", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dateTimePicker1.Location = new Point(632, 104);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(359, 34);
            dateTimePicker1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.GradientActiveCaption;
            label1.Font = new Font("Times New Roman", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ActiveCaptionText;
            label1.Location = new Point(434, 103);
            label1.Name = "label1";
            label1.Size = new Size(143, 34);
            label1.TabIndex = 1;
            label1.Text = "Select date";
            // 
            // button1
            // 
            button1.BackColor = SystemColors.ActiveCaption;
            button1.Font = new Font("Times New Roman", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.Location = new Point(1187, 9);
            button1.Name = "button1";
            button1.Size = new Size(152, 81);
            button1.TabIndex = 0;
            button1.Text = "Edit profile";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // tabPage2
            // 
            tabPage2.BackColor = SystemColors.InactiveCaption;
            tabPage2.Controls.Add(groupBox1);
            tabPage2.Controls.Add(pictureBox1);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1358, 649);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Edit profile";
            // 
            // groupBox1
            // 
            groupBox1.BackColor = SystemColors.GradientInactiveCaption;
            groupBox1.Controls.Add(button3);
            groupBox1.Controls.Add(textBox4);
            groupBox1.Controls.Add(textBox3);
            groupBox1.Controls.Add(textBox2);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Location = new Point(772, 94);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(509, 451);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Edit profile";
            // 
            // button3
            // 
            button3.BackColor = SystemColors.InactiveCaption;
            button3.Font = new Font("Times New Roman", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button3.Location = new Point(167, 330);
            button3.Name = "button3";
            button3.Size = new Size(173, 78);
            button3.TabIndex = 8;
            button3.Text = "Save";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // textBox4
            // 
            textBox4.Font = new Font("Times New Roman", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox4.Location = new Point(271, 259);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(210, 42);
            textBox4.TabIndex = 7;
            // 
            // textBox3
            // 
            textBox3.Font = new Font("Times New Roman", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox3.Location = new Point(271, 192);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(210, 42);
            textBox3.TabIndex = 6;
            // 
            // textBox2
            // 
            textBox2.Font = new Font("Times New Roman", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox2.Location = new Point(271, 124);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(210, 42);
            textBox2.TabIndex = 5;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Times New Roman", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(271, 59);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(210, 42);
            textBox1.TabIndex = 4;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Times New Roman", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(39, 259);
            label6.Name = "label6";
            label6.Size = new Size(83, 34);
            label6.TabIndex = 3;
            label6.Text = "Email";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Times New Roman", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(39, 195);
            label5.Name = "label5";
            label5.Size = new Size(90, 34);
            label5.TabIndex = 2;
            label5.Text = "Phone";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Times New Roman", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(39, 127);
            label4.Name = "label4";
            label4.Size = new Size(137, 34);
            label4.TabIndex = 1;
            label4.Text = "Last name";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Times New Roman", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(39, 62);
            label3.Name = "label3";
            label3.Size = new Size(140, 34);
            label3.TabIndex = 0;
            label3.Text = "First name";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.coach;
            pictureBox1.Location = new Point(-59, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1414, 646);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // CoachForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1357, 676);
            Controls.Add(tabControl1);
            Margin = new Padding(2);
            Name = "CoachForm";
            Text = "CoachForm";
            FormClosed += CoachForm_FormClosed;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Label label2;
        private Button button2;
        private RichTextBox richTextBox1;
        private DateTimePicker dateTimePicker1;
        private Label label1;
        private Button button1;
        private GroupBox groupBox1;
        private Button button3;
        private TextBox textBox4;
        private TextBox textBox3;
        private TextBox textBox2;
        private TextBox textBox1;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private PictureBox pictureBox1;
        private Button button4;
    }
}
namespace cue_and_guess
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBox1 = new TextBox();
            button1 = new Button();
            textBox2 = new TextBox();
            send_button = new Button();
            listBox1 = new ListBox();
            textBox3 = new TextBox();
            change_user_name = new Button();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.ButtonHighlight;
            textBox1.Location = new Point(209, 23);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.Size = new Size(239, 361);
            textBox1.TabIndex = 0;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // button1
            // 
            button1.Location = new Point(481, 23);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 1;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(209, 400);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(191, 27);
            textBox2.TabIndex = 2;
            textBox2.WordWrap = false;
            textBox2.KeyDown += textBox2_KeyDown;
            // 
            // send_button
            // 
            send_button.Location = new Point(395, 400);
            send_button.Name = "send_button";
            send_button.Size = new Size(53, 28);
            send_button.TabIndex = 3;
            send_button.Text = "send";
            send_button.UseVisualStyleBackColor = true;
            send_button.Click += send_button_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 20;
            listBox1.Location = new Point(23, 103);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(169, 324);
            listBox1.TabIndex = 4;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(23, 60);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(103, 27);
            textBox3.TabIndex = 6;
            textBox3.Enter += textBox3_Enter;
            textBox3.KeyDown += textBox3_KeyDown;
            textBox3.Leave += textBox3_Leave;
            // 
            // change_user_name
            // 
            change_user_name.Location = new Point(121, 59);
            change_user_name.Name = "change_user_name";
            change_user_name.Size = new Size(71, 28);
            change_user_name.TabIndex = 7;
            change_user_name.Text = "change";
            change_user_name.UseVisualStyleBackColor = true;
            change_user_name.Click += change_user_name_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1039, 450);
            Controls.Add(change_user_name);
            Controls.Add(textBox3);
            Controls.Add(listBox1);
            Controls.Add(send_button);
            Controls.Add(textBox2);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private Button button1;
        private TextBox textBox2;
        private Button send_button;
        private ListBox listBox1;
        private TextBox textBox3;
        private Button change_user_name;
    }
}
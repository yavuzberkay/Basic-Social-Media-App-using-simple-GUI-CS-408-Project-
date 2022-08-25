namespace client
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_ip = new System.Windows.Forms.TextBox();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.button_connect = new System.Windows.Forms.Button();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.textBox_message = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button_send = new System.Windows.Forms.Button();
            this.label_username = new System.Windows.Forms.Label();
            this.username_textbox = new System.Windows.Forms.TextBox();
            this.AllSweets = new System.Windows.Forms.Button();
            this.button_login = new System.Windows.Forms.Button();
            this.disconnect_button = new System.Windows.Forms.Button();
            this.all_users = new System.Windows.Forms.Button();
            this.follow_button = new System.Windows.Forms.Button();
            this.follow_textbox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.followed_sweets_button = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.block_textbox = new System.Windows.Forms.TextBox();
            this.block_button = new System.Windows.Forms.Button();
            this.button_followers = new System.Windows.Forms.Button();
            this.button_allfollowing = new System.Windows.Forms.Button();
            this.button_mysweets = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.sweetid_textbox = new System.Windows.Forms.TextBox();
            this.delete_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 162);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 192);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port:";
            // 
            // textBox_ip
            // 
            this.textBox_ip.Location = new System.Drawing.Point(116, 159);
            this.textBox_ip.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_ip.Name = "textBox_ip";
            this.textBox_ip.Size = new System.Drawing.Size(153, 22);
            this.textBox_ip.TabIndex = 2;
            // 
            // textBox_port
            // 
            this.textBox_port.Location = new System.Drawing.Point(116, 192);
            this.textBox_port.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(153, 22);
            this.textBox_port.TabIndex = 3;
            // 
            // button_connect
            // 
            this.button_connect.Location = new System.Drawing.Point(173, 219);
            this.button_connect.Margin = new System.Windows.Forms.Padding(2);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(93, 28);
            this.button_connect.TabIndex = 4;
            this.button_connect.Text = "connect";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // logs
            // 
            this.logs.Location = new System.Drawing.Point(325, 64);
            this.logs.Margin = new System.Windows.Forms.Padding(2);
            this.logs.Name = "logs";
            this.logs.Size = new System.Drawing.Size(400, 320);
            this.logs.TabIndex = 5;
            this.logs.Text = "";
            // 
            // textBox_message
            // 
            this.textBox_message.Enabled = false;
            this.textBox_message.Location = new System.Drawing.Point(89, 338);
            this.textBox_message.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_message.Name = "textBox_message";
            this.textBox_message.Size = new System.Drawing.Size(126, 22);
            this.textBox_message.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 340);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Message:";
            // 
            // button_send
            // 
            this.button_send.Enabled = false;
            this.button_send.Location = new System.Drawing.Point(221, 332);
            this.button_send.Margin = new System.Windows.Forms.Padding(2);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(87, 32);
            this.button_send.TabIndex = 8;
            this.button_send.Text = "send";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // label_username
            // 
            this.label_username.AutoSize = true;
            this.label_username.Location = new System.Drawing.Point(32, 80);
            this.label_username.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label_username.Name = "label_username";
            this.label_username.Size = new System.Drawing.Size(73, 17);
            this.label_username.TabIndex = 9;
            this.label_username.Text = "Username";
            // 
            // username_textbox
            // 
            this.username_textbox.Enabled = false;
            this.username_textbox.Location = new System.Drawing.Point(116, 77);
            this.username_textbox.Margin = new System.Windows.Forms.Padding(1);
            this.username_textbox.Name = "username_textbox";
            this.username_textbox.Size = new System.Drawing.Size(153, 22);
            this.username_textbox.TabIndex = 10;
            // 
            // AllSweets
            // 
            this.AllSweets.Enabled = false;
            this.AllSweets.Location = new System.Drawing.Point(452, 392);
            this.AllSweets.Margin = new System.Windows.Forms.Padding(1);
            this.AllSweets.Name = "AllSweets";
            this.AllSweets.Size = new System.Drawing.Size(125, 26);
            this.AllSweets.TabIndex = 11;
            this.AllSweets.Text = "All Sweets";
            this.AllSweets.UseVisualStyleBackColor = true;
            this.AllSweets.Click += new System.EventHandler(this.AllSweets_Click);
            // 
            // button_login
            // 
            this.button_login.Enabled = false;
            this.button_login.Location = new System.Drawing.Point(173, 103);
            this.button_login.Margin = new System.Windows.Forms.Padding(1);
            this.button_login.Name = "button_login";
            this.button_login.Size = new System.Drawing.Size(93, 29);
            this.button_login.TabIndex = 12;
            this.button_login.Text = "Login";
            this.button_login.UseVisualStyleBackColor = true;
            this.button_login.Click += new System.EventHandler(this.button_login_Click);
            // 
            // disconnect_button
            // 
            this.disconnect_button.Enabled = false;
            this.disconnect_button.Location = new System.Drawing.Point(173, 257);
            this.disconnect_button.Name = "disconnect_button";
            this.disconnect_button.Size = new System.Drawing.Size(93, 30);
            this.disconnect_button.TabIndex = 13;
            this.disconnect_button.Text = "disconnect";
            this.disconnect_button.UseVisualStyleBackColor = true;
            this.disconnect_button.Click += new System.EventHandler(this.disconnect_button_Click);
            // 
            // all_users
            // 
            this.all_users.Enabled = false;
            this.all_users.Location = new System.Drawing.Point(325, 392);
            this.all_users.Name = "all_users";
            this.all_users.Size = new System.Drawing.Size(122, 26);
            this.all_users.TabIndex = 14;
            this.all_users.Text = "All Users";
            this.all_users.UseVisualStyleBackColor = true;
            this.all_users.Click += new System.EventHandler(this.all_users_Click);
            // 
            // follow_button
            // 
            this.follow_button.Enabled = false;
            this.follow_button.Location = new System.Drawing.Point(221, 380);
            this.follow_button.Name = "follow_button";
            this.follow_button.Size = new System.Drawing.Size(87, 33);
            this.follow_button.TabIndex = 15;
            this.follow_button.Text = "Follow";
            this.follow_button.UseVisualStyleBackColor = true;
            this.follow_button.Click += new System.EventHandler(this.follow_button_Click);
            // 
            // follow_textbox
            // 
            this.follow_textbox.Enabled = false;
            this.follow_textbox.Location = new System.Drawing.Point(89, 385);
            this.follow_textbox.Name = "follow_textbox";
            this.follow_textbox.Size = new System.Drawing.Size(126, 22);
            this.follow_textbox.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 388);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 17);
            this.label4.TabIndex = 18;
            this.label4.Text = "Username: ";
            // 
            // followed_sweets_button
            // 
            this.followed_sweets_button.Enabled = false;
            this.followed_sweets_button.Location = new System.Drawing.Point(581, 392);
            this.followed_sweets_button.Name = "followed_sweets_button";
            this.followed_sweets_button.Size = new System.Drawing.Size(144, 26);
            this.followed_sweets_button.TabIndex = 19;
            this.followed_sweets_button.Text = "Followed Sweets";
            this.followed_sweets_button.UseVisualStyleBackColor = true;
            this.followed_sweets_button.Click += new System.EventHandler(this.followed_sweets_button_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(2, 431);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 17);
            this.label5.TabIndex = 20;
            this.label5.Text = "Username:";
            // 
            // block_textbox
            // 
            this.block_textbox.Enabled = false;
            this.block_textbox.Location = new System.Drawing.Point(85, 428);
            this.block_textbox.Name = "block_textbox";
            this.block_textbox.Size = new System.Drawing.Size(130, 22);
            this.block_textbox.TabIndex = 21;
            // 
            // block_button
            // 
            this.block_button.Enabled = false;
            this.block_button.Location = new System.Drawing.Point(221, 428);
            this.block_button.Name = "block_button";
            this.block_button.Size = new System.Drawing.Size(87, 26);
            this.block_button.TabIndex = 22;
            this.block_button.Text = "Block";
            this.block_button.UseVisualStyleBackColor = true;
            this.block_button.Click += new System.EventHandler(this.block_button_Click);
            // 
            // button_followers
            // 
            this.button_followers.Enabled = false;
            this.button_followers.Location = new System.Drawing.Point(325, 428);
            this.button_followers.Name = "button_followers";
            this.button_followers.Size = new System.Drawing.Size(192, 29);
            this.button_followers.TabIndex = 23;
            this.button_followers.Text = "All Followers";
            this.button_followers.UseVisualStyleBackColor = true;
            this.button_followers.Click += new System.EventHandler(this.button_followers_Click);
            // 
            // button_allfollowing
            // 
            this.button_allfollowing.Enabled = false;
            this.button_allfollowing.Location = new System.Drawing.Point(523, 428);
            this.button_allfollowing.Name = "button_allfollowing";
            this.button_allfollowing.Size = new System.Drawing.Size(202, 29);
            this.button_allfollowing.TabIndex = 24;
            this.button_allfollowing.Text = "All Folowing";
            this.button_allfollowing.UseVisualStyleBackColor = true;
            this.button_allfollowing.Click += new System.EventHandler(this.button_allfollowing_Click);
            // 
            // button_mysweets
            // 
            this.button_mysweets.Enabled = false;
            this.button_mysweets.Location = new System.Drawing.Point(423, 463);
            this.button_mysweets.Name = "button_mysweets";
            this.button_mysweets.Size = new System.Drawing.Size(192, 29);
            this.button_mysweets.TabIndex = 25;
            this.button_mysweets.Text = "My Sweets";
            this.button_mysweets.UseVisualStyleBackColor = true;
            this.button_mysweets.Click += new System.EventHandler(this.button_mysweets_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 469);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 17);
            this.label6.TabIndex = 26;
            this.label6.Text = "Sweet Id: ";
            // 
            // sweetid_textbox
            // 
            this.sweetid_textbox.Enabled = false;
            this.sweetid_textbox.Location = new System.Drawing.Point(85, 466);
            this.sweetid_textbox.Name = "sweetid_textbox";
            this.sweetid_textbox.Size = new System.Drawing.Size(130, 22);
            this.sweetid_textbox.TabIndex = 27;
            // 
            // delete_button
            // 
            this.delete_button.Enabled = false;
            this.delete_button.Location = new System.Drawing.Point(221, 465);
            this.delete_button.Name = "delete_button";
            this.delete_button.Size = new System.Drawing.Size(87, 27);
            this.delete_button.TabIndex = 28;
            this.delete_button.Text = "Delete";
            this.delete_button.UseVisualStyleBackColor = true;
            this.delete_button.Click += new System.EventHandler(this.delete_button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 504);
            this.Controls.Add(this.delete_button);
            this.Controls.Add(this.sweetid_textbox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button_mysweets);
            this.Controls.Add(this.button_allfollowing);
            this.Controls.Add(this.button_followers);
            this.Controls.Add(this.block_button);
            this.Controls.Add(this.block_textbox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.followed_sweets_button);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.follow_textbox);
            this.Controls.Add(this.follow_button);
            this.Controls.Add(this.all_users);
            this.Controls.Add(this.disconnect_button);
            this.Controls.Add(this.button_login);
            this.Controls.Add(this.AllSweets);
            this.Controls.Add(this.username_textbox);
            this.Controls.Add(this.label_username);
            this.Controls.Add(this.button_send);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_message);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.button_connect);
            this.Controls.Add(this.textBox_port);
            this.Controls.Add(this.textBox_ip);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_ip;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.TextBox textBox_message;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.Label label_username;
        private System.Windows.Forms.TextBox username_textbox;
        private System.Windows.Forms.Button AllSweets;
        private System.Windows.Forms.Button button_login;
        private System.Windows.Forms.Button disconnect_button;
        private System.Windows.Forms.Button all_users;
        private System.Windows.Forms.Button follow_button;
        private System.Windows.Forms.TextBox follow_textbox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button followed_sweets_button;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox block_textbox;
        private System.Windows.Forms.Button block_button;
        private System.Windows.Forms.Button button_followers;
        private System.Windows.Forms.Button button_allfollowing;
        private System.Windows.Forms.Button button_mysweets;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox sweetid_textbox;
        private System.Windows.Forms.Button delete_button;
    }
}


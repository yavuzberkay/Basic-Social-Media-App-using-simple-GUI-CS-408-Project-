using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client
{
    public partial class Form1 : Form
    {

        bool terminating = false; //bool values to check if the client is connected or not
        bool connected = false;
        Socket clientSocket;
        string username = "";

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
            
        }

        


    private void button_connect_Click(object sender, EventArgs e) //Connects the client to the server using the IP and port number
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string IP = textBox_ip.Text;
            
            int portNum;
        
            if (Int32.TryParse(textBox_port.Text, out portNum))
            {
                try//if we can't connect, catch block catches the exception
                {
                        clientSocket.Connect(IP, portNum); //client tries to connect to the server
                        button_connect.Enabled = false;
                        button_login.Enabled = true;
                        username_textbox.Enabled = true;
                        terminating = false;
                        connected = true;
                        disconnect_button.Enabled = true;
                        logs.AppendText("Connected to the server! Please login.\n");
                       

                    Thread receiveThread = new Thread(Receive); //if successfully connects, a new thread starts to receive the messages from the server
                        receiveThread.Start(); 
                    
                }
                catch
                {
                    logs.AppendText("Could not connect to the server!\n"); 
                }
            }
            else //port is not integer
            {
                logs.AppendText("Check the port\n");
            }

        }

        private void Receive()
        {
            while(connected) //while we are connected to the server, we receive info
            {
                try //using the try catch block to catch exceptions
                {
                    Byte[] buffer = new Byte[128];  
                    clientSocket.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    
                    if (incomingMessage.Contains("\0"))
                    {
                        incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
                    }

                    
                    if (incomingMessage.Length > 9 && incomingMessage.Substring(0, 10) == "Your sweet") //if the first 10 characters of the received string is your sweet,
                    {                                                     //we append it on the table
                        logs.AppendText(incomingMessage);
                    }
                    else//if it is not the user's sweet
                    {

                        if (incomingMessage != "") //if incoming message is not empty, print the message on richtextbox
                        {
                            
                            if(incomingMessage == "The client has disconnected")
                            {
                                logs.AppendText("Server: " + incomingMessage);
                                connected = false;
                            }

                            else
                            {
                                logs.AppendText(incomingMessage);
                            }
          
                        
                        }
                        if (incomingMessage == "User already logged in\n") //If the incoming message is "User already logged in" disconnect the client from the server.
                        {
                            logs.AppendText("The client has disconnected");
                            disconnect_button.Enabled = false;
                            button_connect.Enabled = true;
                            AllSweets.Enabled = false;
                            all_users.Enabled = false;
                            textBox_message.Enabled = false;
                            button_send.Enabled = false;
                            follow_textbox.Enabled = false;
                            follow_button.Enabled = false;
                            followed_sweets_button.Enabled = false;
                            block_button.Enabled = false;
                            block_textbox.Enabled = false;
                            button_followers.Enabled = false;
                            button_allfollowing.Enabled = false;
                            button_mysweets.Enabled = false;
                            sweetid_textbox.Enabled = false;
                            delete_button.Enabled = false;
                            clientSocket.Close();
                            connected = false;
                        }
                    }
                   
                }
                catch 
                {
                    if (!terminating)
                    {
                        logs.AppendText("The server has disconnected\n");
                        disconnect_button.Enabled = false;
                        AllSweets.Enabled = false;
                        all_users.Enabled = false;
                        button_connect.Enabled = true;
                        textBox_message.Enabled = false;
                        button_send.Enabled = false;
                        follow_button.Enabled = false;
                        follow_textbox.Enabled = false;
                        followed_sweets_button.Enabled = false;
                        block_button.Enabled = false;
                        block_textbox.Enabled = false;
                        button_followers.Enabled = false;
                        button_allfollowing.Enabled = false;
                        button_mysweets.Enabled = false;
                        sweetid_textbox.Enabled = false;
                        delete_button.Enabled = false;
                    }

                    clientSocket.Close();
                    connected = false;
                }

            }
        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connected = false;
            terminating = true;
            Environment.Exit(0);
        }

        private void button_send_Click(object sender, EventArgs e) //when the user clicks on send, the message in the textbox is sent to the server.
        {
            string message = textBox_message.Text;

            if(message != "" && message.Length <= 64)
            {
                Byte[] buffer = Encoding.Default.GetBytes(message);
                clientSocket.Send(buffer);
            }

        }

     

        private void button_login_Click(object sender, EventArgs e)//when user clicks on the login button, the text in the textbox is taken as the username
        {                                                          //and the user tries to log in to the server.
            
            username = username_textbox.Text;
            if (username != "" && username.Length <= 64)
            {
                Byte[] buffer2 = Encoding.Default.GetBytes(username);
                clientSocket.Send(buffer2);

            }

            button_login.Enabled = false;
            if(connected)
            {
                button_send.Enabled = true;
                textBox_message.Enabled = true;
                AllSweets.Enabled = true;
                all_users.Enabled = true;
                follow_textbox.Enabled = true;
                follow_button.Enabled = true;
                followed_sweets_button.Enabled = true;
                block_button.Enabled = true;
                button_followers.Enabled = true;
                block_textbox.Enabled = true;
                button_allfollowing.Enabled = true;
                button_mysweets.Enabled = true;
                sweetid_textbox.Enabled = true;
                delete_button.Enabled = true;


            }


        }

        private void disconnect_button_Click(object sender, EventArgs e) //when the user clicks on the disconnect button, client disconnects from the server
        {
            terminating = true;
            string disconnect_button_msg = "I want to disconnect"; //a string is sent to a server saying "I want to disconnect";
            Byte[] buffer2 = Encoding.Default.GetBytes(disconnect_button_msg);
            clientSocket.Send(buffer2);

            disconnect_button.Enabled = false;
            AllSweets.Enabled = false;
            all_users.Enabled = false;
            follow_textbox.Enabled = false;
            follow_button.Enabled = false;
            followed_sweets_button.Enabled = false;
            block_button.Enabled = false;
            block_textbox.Enabled = false;
            button_followers.Enabled = false;
            button_allfollowing.Enabled = false;
            button_mysweets.Enabled = false;
            sweetid_textbox.Enabled = false;
            delete_button.Enabled = false;



            if (button_login.Enabled == true)
            {
                button_login.Enabled = false;
            }

            button_connect.Enabled = true; //connect button enabled again
            textBox_ip.Enabled = true;
            textBox_port.Enabled = true;

            if(button_send.Enabled == true)
            {
                button_send.Enabled = false;
                textBox_message.Enabled = false;
                follow_textbox.Enabled = false;
                follow_button.Enabled = false;
                followed_sweets_button.Enabled = false;
                block_button.Enabled = false;
                block_textbox.Enabled = false;
                button_followers.Enabled = false;
                button_allfollowing.Enabled = false;
                button_mysweets.Enabled = false;
                sweetid_textbox.Enabled = false;
                delete_button.Enabled = false;
            }
            clientSocket.Close();
            connected = false;
           
            if(username == "")
            {
                logs.AppendText("The client has disconnected from the server\n");
            }
            else
            {
                logs.AppendText("The user " + username + " has disconnected from the server\n");
            }

            
        }

        private void AllSweets_Click(object sender, EventArgs e) //when the user clicks All Sweets button, a message is sent to the server 
        {                                                       //that says "I want all the sweets".
            string all_sweets_request = "I want all the sweets";
            if (all_sweets_request != "" && all_sweets_request.Length <= 64)
            {
                Byte[] buffer2 = Encoding.Default.GetBytes(all_sweets_request);
                clientSocket.Send(buffer2);

            }
        }

        private void all_users_Click(object sender, EventArgs e)// when the user clicks All Users button, a message is sent to the server that says 
        {                                                       //"I want all the users"
            string all_users_request = "I want all the users";

            if (all_users_request != "" && all_users_request.Length <= 64)
            {
                Byte[] buffer2 = Encoding.Default.GetBytes(all_users_request);
                clientSocket.Send(buffer2);

            }
        }

        private void follow_button_Click(object sender, EventArgs e) //when the user clicks follow button, a message is sent to the server that says
        {                                                            // "I want to follow" + username
            if(follow_textbox.Text != "")
            {
                string follow_request = "I want to follow " + follow_textbox.Text;
                if (follow_request != "" && follow_request.Length <= 64)
                {                   
                    Byte[] buffer2 = Encoding.Default.GetBytes(follow_request);
                    clientSocket.Send(buffer2);

                }

            }
            else
            {
                logs.AppendText("You did not enter any username.");
            }
                
        }

        private void followed_sweets_button_Click(object sender, EventArgs e) //when the user clicjs followed_sweets_button, a message is sent to the server
        {                                                                       //that says "I want all the followed sweets"
            string all_followed_sweets = "I want all the followed sweets";

            if (all_followed_sweets != "" && all_followed_sweets.Length <= 64)
            {
                Byte[] buffer2 = Encoding.Default.GetBytes(all_followed_sweets);
                clientSocket.Send(buffer2);

            }
        }

        private void block_button_Click(object sender, EventArgs e)//when the user clicjs block_button, a message is sent to the server
        {                                                                       //that says "I want to block" + the name of the user they are trying to block.
            
            string block_person = "I want to block " + block_textbox.Text;

            if(block_person != "" && block_person.Length <= 64)
            {
                Byte[] buffer2 = Encoding.Default.GetBytes(block_person);
                clientSocket.Send(buffer2);
            }
        }

        private void button_followers_Click(object sender, EventArgs e)//when the user clicks followers_button, a message is sent to the server
                                                                              //that says "I want all my followers"
        {
            string followers = "I want all my followers";
            if (followers != "" && followers.Length <= 64)
            {
                Byte[] buffer2 = Encoding.Default.GetBytes(followers);
                clientSocket.Send(buffer2);
            }

        }

        private void button_allfollowing_Click(object sender, EventArgs e) //when the user clicjs allfollowing_button, a message is sent to the server
                                                                          //that says "I want all following"
            {
            string following = "I want all following";
            if (following != "" && following.Length <= 64)
            { 
                Byte[] buffer2 = Encoding.Default.GetBytes(following);
                clientSocket.Send(buffer2);
            }
        }

        private void button_mysweets_Click(object sender, EventArgs e)//when the user clicjs mysweets_button, a message is sent to the server
                                                                               //that says "I want all my sweets"
        {
            string my_sweets = "I want all my sweets";
            if (my_sweets != "" && my_sweets.Length <= 64)
            {
                Byte[] buffer2 = Encoding.Default.GetBytes(my_sweets);
                clientSocket.Send(buffer2);
            }
        }

        private void delete_button_Click(object sender, EventArgs e)//when the user clicjs delete_button, a message is sent to the server
        {                                                                       //that says "I want to delete" + the sweet id of the sweet.
            string delete_sweet = "I want to delete " + sweetid_textbox.Text;
            if (delete_sweet != "" && delete_sweet.Length <= 64)
            {
                Byte[] buffer2 = Encoding.Default.GetBytes(delete_sweet);
                clientSocket.Send(buffer2);
            }
        }
    }
}

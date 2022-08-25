using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server
{
    public partial class Form1 : Form
    {

        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); 
        List<Socket> clientSockets = new List<Socket>();

        
        bool terminating = false;
        bool listening = false;
        List<string> usernameList = new List<string>();

        

        
        int id;

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        private void button_listen_Click(object sender, EventArgs e) //code to be executed when clicked on button listen
        {
            int serverPort;

            if(Int32.TryParse(textBox_port.Text, out serverPort))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(6);

                //implementing necessary button changes
                listening = true;
                button_listen.Enabled = false;
                textBox_message.Enabled = true;
                button_send.Enabled = true;

                //starting the thread that listens to the port
                Thread acceptThread = new Thread(Accept);
                acceptThread.Start();

                logs.AppendText("Started listening on port: " + serverPort + "\n");

            }
            else
            {
                logs.AppendText("Please check port number \n");
            }
        }
       
        private void Accept() //this function listens to the port and accepts new clients as they connect
        {
            while(listening)
            {
                try //using try and catch blocks to catch a possible error
                    {  

                        Socket newClient = serverSocket.Accept();
                        clientSockets.Add(newClient);
                        logs.AppendText("A client is connected, waiting for log in.\n");
                        

                        Thread receiveThread = new Thread(() => Receive(newClient)); // updated
                        receiveThread.Start();
                }

                    
                catch
                {
                    if (terminating)
                    {
                        listening = false;
                    }
                    else
                    {
                        logs.AppendText("The socket stopped working.\n");
                    }

                }
            }
        }

        private void Receive(Socket thisClient) // function that receives any kind of information from the clients
        {
            
            bool connected = true;
            string disconnect_msg = "The client has disconnected\n";
            
            
            Byte[] buffer = new Byte[64];
            

            thisClient.Receive(buffer);

            string usernameMessage = Encoding.Default.GetString(buffer); //getting the username of the user


            usernameMessage = usernameMessage.Substring(0, usernameMessage.IndexOf("\0"));

            if (usernameMessage == "I want to disconnect") //if the message from client is "I want to disconnect", server closes the socket and
            {                                              //removes the socket from clientsockets list.
                thisClient.Close();

                clientSockets.Remove(thisClient);
                connected = false;
                logs.AppendText("A client has disconnected from the server\n");
            }
            else //if the message is not "I want to disconnect, then it is the name of the user.
            {

                int found = 0;
                string[] lines = System.IO.File.ReadAllLines(@"C:\Users\berka\Desktop\proj\client\client\user-db.txt");
                int counter = 0;
                foreach (string line in System.IO.File.ReadLines(@"C:\Users\berka\Desktop\proj\client\client\user-db.txt")) //going through each line of user-db.txt
                {

                    counter++;
                }
                for (int i = 0; i < counter; i++)
                {


                    if (lines[i] == usernameMessage) //if username is found in the user-db.txt found becomes one and we break from the loop
                    {
                        found = 1;
                        break;
                    }

                }

                if (found == 1) //if username is found we do the folowing
                {
                    if (usernameList.Contains(usernameMessage)) //we check if a client with the given username is already connected or not.
                    {                                           //if so, the new client is disconnected from the server and removed from the client list.

                        string already_loggedin = "User already logged in\n";
                        logs.AppendText(already_loggedin);
                        if (already_loggedin != "" && already_loggedin.Length <= 128)
                        {
                            Byte[] buffer_5 = Encoding.Default.GetBytes(already_loggedin);
                            thisClient.Send(buffer_5);
                        }

                        try
                        {
                            thisClient.Shutdown(SocketShutdown.Both);
                        }
                        finally
                        {
                            
                            thisClient.Close(); 
                        }
                        clientSockets.Remove(thisClient);
                    }
                    else //else, it means this is a new user. So we act accordingly
                    {
                        string message_from_server = usernameMessage + " has logged in to Sweeter. \n"; 
                        if (message_from_server != "" && message_from_server.Length <= 128)
                        {
                            Byte[] buffer_4 = Encoding.Default.GetBytes(message_from_server); //we send a message back to the client to notify that it is logged in to Sweeter.
                            thisClient.Send(buffer_4);
                        }
                        logs.AppendText(usernameMessage + " has logged in to Sweeter. \n");
                        usernameList.Add(usernameMessage); //we add the new user's username to the currently online users list
                    }

                }
                else //if username cannot be found in the user-db.txt, we give an error and disconnect the client from the server.
                {
                    logs.AppendText(usernameMessage + " cannot register. \n");
                    if (disconnect_msg != "" && disconnect_msg.Length <= 128)
                    {
                        Byte[] buffer_5 = Encoding.Default.GetBytes(disconnect_msg);
                        thisClient.Send(buffer_5);
                    }
                    thisClient.Close();
                    clientSockets.Remove(thisClient);
                    connected = false;
                }

                while (connected && !terminating) //if we can enter this while loop, it means that user logged in to the Switter successfully and
                {                                   //we begin to receive information from the client.
                    try
                    {

                        Byte[] buffer_2 = new Byte[64];

                        thisClient.Receive(buffer_2);

                        string incomingMessage = Encoding.Default.GetString(buffer_2);


                        incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                        if (incomingMessage == "I want all the sweets") //if incoming message is "I want all the sweets", 
                        {                                               //it means a user wants all the sweets that has been sent by other users.
                            logs.AppendText(usernameMessage + " requested sweet feed\n");

                            List<string> blocked_by = new List<string>();
                            foreach (string line in System.IO.File.ReadLines(@"C:\Users\berka\Desktop\proj\server\server\blocked.txt"))
                            {
                                string[] line_arr = line.Split(' ');
                                if(line_arr[0] != usernameMessage)
                                {
                                    foreach(string word in line_arr)
                                    {
                                        if(word == usernameMessage)
                                        {
                                            blocked_by.Add(line_arr[0]);
                                        }
                                    }
                                }
                            }
                            foreach (string line in System.IO.File.ReadLines(@"C:\Users\berka\Desktop\proj\server\server\sweets.txt")) // we get the sweets from the txt file and send them to client
                            {
                                string[] words = line.Split(' ');
                                //logs.AppendText(line);
                                if (words[1] != usernameMessage && !blocked_by.Contains(words[1]))
                                {
                                    if (line != "" && line.Length <= 128)
                                    {
                                        Byte[] all_sweet_buffer = Encoding.Default.GetBytes(line + "\n");
                                        thisClient.Send(all_sweet_buffer);
                                        Thread.Sleep(1);
                                    }
                                }
                            }

                        }

                        else if (incomingMessage == "I want all the users") //if incoming message is "I want all the users"
                        {                                                  //it means a user wants the name of all the users.
                            logs.AppendText(usernameMessage + " requested all the users\n");
                            foreach (string line in System.IO.File.ReadLines(@"C:\Users\berka\Desktop\proj\client\client\user-db.txt")) // we get the users from the txt file and send them to client
                            {

                                if (line != "" && line.Length <= 128)
                                {
                                    Byte[] all_sweet_buffer = Encoding.Default.GetBytes("Username: " + line + "\n");
                                    thisClient.Send(all_sweet_buffer);


                                }

                            }
                        }

                        else if (incomingMessage == "I want all my followers")//if incoming message is "I want all my followers"
                        {                                                  //it means a user wants the name of all of their followers.
                            
                            logs.AppendText("User " + usernameMessage + " has requested the list of all of their followers.\n");
                            bool follower = false;
                            foreach (string line in System.IO.File.ReadLines(@"C:\Users\berka\Desktop\proj\server\server\following.txt")) //we get the followers list from the following txt
                            {
                                string[] line_arr = line.Split(' ');
                                if(line_arr[0] != usernameMessage)
                                {
                                    foreach(string word in line_arr)
                                    {
                                        if (word == usernameMessage)
                                        {
                                            follower = true;
                                            if (line_arr[0] != "" && line_arr[0].Length <= 128)
                                            {
                                                Byte[] all_sweet_buffer = Encoding.Default.GetBytes("You are followed by: " + line_arr[0] + "\n");
                                                thisClient.Send(all_sweet_buffer);


                                            }
                                        }
                                    }
                                }
                            }

                            if(!follower)
                            {
                                logs.AppendText("User " + usernameMessage + " does not have any followers.\n");
                                Byte[] all_sweet_buffer = Encoding.Default.GetBytes("You do not have any followers.\n");
                                thisClient.Send(all_sweet_buffer);
                            }

                        }

                        else if(incomingMessage == "I want all following")//if incoming message is "I want all following"
                            {                                            //it means a user wants the names of all the people they follow
                              
                            bool following = false;
                            logs.AppendText("User " + usernameMessage + " has requested the list of all the people they follow.\n");
                            foreach (string line in System.IO.File.ReadLines(@"C:\Users\berka\Desktop\proj\server\server\following.txt")) //we get the names of people they follow from the following.txt and send them to the client.
                            {
                                string[] line_arr = line.Split(' ');
                                if(line_arr[0] == usernameMessage)
                                {
                                    following = true;
                                    for(int i = 1; i < line_arr.Length; i++)
                                    {
                                        if (line_arr[i] != "" && line_arr[i].Length <= 128)
                                        {
                                            Byte[] all_sweet_buffer = Encoding.Default.GetBytes("You are following: " + line_arr[i] + "\n");
                                            thisClient.Send(all_sweet_buffer);
                                            

                                        }
                                    }
                                }
                            }

                            if(!following)
                            {
                                logs.AppendText("User " + usernameMessage + " is not following anyone.\n");
                                Byte[] all_sweet_buffer = Encoding.Default.GetBytes("You are not following anyone.\n");
                                thisClient.Send(all_sweet_buffer);
                            }
                        }

                        else if(incomingMessage == "I want all my sweets") //if incoming message is "I want all my sweets"
                        {                                                  //it means a user wants the list of all of the sweets they posted.
                                    
                            logs.AppendText("User " + usernameMessage + " has requested all of their sweets.\n");
                            bool sweets = false;
                            foreach (string line in System.IO.File.ReadLines(@"C:\Users\berka\Desktop\proj\server\server\sweets.txt")) //we get the sweets from sweets.txt and send them to the client.
                            {
                                string[] line_arr = line.Split(' ');
                                if(line_arr[1] == usernameMessage)
                                {
                                    sweets = true;
                                    if (line != "" && line.Length <= 128)
                                    {
                                        Byte[] all_sweet_buffer = Encoding.Default.GetBytes(line + "\n");
                                        thisClient.Send(all_sweet_buffer);


                                    }
                                }
                            }

                            if(!sweets)
                            {
                                logs.AppendText("User " + usernameMessage + " does not have any sweets.\n");
                                Byte[] all_sweet_buffer = Encoding.Default.GetBytes("You do not have any sweets.\n");
                                thisClient.Send(all_sweet_buffer);
                            }
                        }

                        else if (incomingMessage.Length > 15 && incomingMessage.Substring(0,16) == "I want to delete")//if incoming message is "I want to delete"
                        {                                                  //it means a user wants to delete a sweet.
                                        
                            string to_be_deleted_id = incomingMessage.Substring(17);
                            bool sweet_found = false;
                            bool not_own_sweet = false;
                            bool deleted = false;
                            string to_be_deleted_line = "";
                            foreach (string line in System.IO.File.ReadLines(@"C:\Users\berka\Desktop\proj\server\server\sweets.txt")) //we check if the sweet they want to delete exists and if it exists we check if it belongs to the user.
                            {
                                string[] line_arr = line.Split(' ');
                                if (line_arr[line_arr.Length -4] == to_be_deleted_id)
                                {
                                    sweet_found = true;
                                    if (line_arr[1] == usernameMessage)
                                    {
                                        deleted = true;
                                        to_be_deleted_line = line;
                                    }
                                    else
                                    {
                                        not_own_sweet = true;
                                    }
                                }
                            }

                            if(sweet_found == false) //we give error if there is no sweet with the given sweet id
                            {
                                logs.AppendText("There is no sweet with given sweet id\n");
                                Byte[] sweet_not_found = Encoding.Default.GetBytes("There is no sweet with sweet id: " + to_be_deleted_id + "\n");
                                thisClient.Send(sweet_not_found);
                            }

                            else if(not_own_sweet)//we give error if the sweet doesn't belong to the user
                            {
                                logs.AppendText("You can't delete someone else's sweet\n");
                                Byte[] not_own_sweet_buffer = Encoding.Default.GetBytes("The sweet you are trying to delete is not yours\n");
                                thisClient.Send(not_own_sweet_buffer);
                            }

                            else if(deleted)//if the conditions above did not occur, we delete the sweet from the sweets.txt
                            {
                                File.WriteAllLines(@"C:\Users\berka\Desktop\proj\server\server\sweets.txt", File.ReadLines(@"C:\Users\berka\Desktop\proj\server\server\sweets.txt").Where(l => l.Split(' ')[l.Split(' ').Length - 4] != to_be_deleted_id).ToList());
                                logs.AppendText("The sweet with sweet id: " + to_be_deleted_id + " has been deleted.\n");
                                Byte[] sweet_not_found = Encoding.Default.GetBytes("The sweet with sweet id: " + to_be_deleted_id + " has been deleted.\n");
                                thisClient.Send(sweet_not_found);
                            }

                        }

                        else if (incomingMessage.Length > 15 && incomingMessage.Substring(0, 16) == "I want to follow") // If the incoming meesage is "I want to follow ..."
                        {                                                                                               // it means a user wants to follow another user.
                            string follow_username = incomingMessage.Substring(17);

                            bool username_found = false;
                            foreach (string line in System.IO.File.ReadLines(@"C:\Users\berka\Desktop\proj\client\client\user-db.txt")) // we check if there is a user with the given username in the database.
                            {
                                if (line == follow_username)
                                {
                                    username_found = true;
                                }
                            }

                            if (username_found == false)//if not we give an error
                            {
                                Byte[] username_not_found = Encoding.Default.GetBytes("Username not found in the database\n");
                                thisClient.Send(username_not_found);
                                logs.AppendText(Encoding.Default.GetString(username_not_found));

                            }

                            else // we add the given username to the list of people that the client user follows.
                            {
                                string temp_line = "";
                                bool user_in_txt = false;
                                bool duplicates = false;
                                bool username_same = false;
                                bool blocked = false;
                                foreach (string line in System.IO.File.ReadLines(@"C:\Users\berka\Desktop\proj\server\server\blocked.txt")) //we check if the person that the user is trying to follow has blocked them or not
                                {
                                    string[] line_arr = line.Split(' ');
                                    if (line_arr[0] == follow_username)
                                    {
                                        foreach (string word in line_arr)
                                        {
                                            if (word == usernameMessage)
                                            {
                                                blocked = true;

                                            }
                                        }
                                    }
                                }

                                if (blocked) //we give error if the person they are trying to follow has blocked them
                                {
                                    logs.AppendText("The user " + usernameMessage+" is blocked by " + follow_username +  "\n");
                                    Byte[] follow_fails = Encoding.Default.GetBytes("The user you are trying to follow has blocked you!\n");
                                    thisClient.Send(follow_fails);
                                }

                                else
                                {
                                    foreach (string line in System.IO.File.ReadLines(@"C:\Users\berka\Desktop\proj\server\server\following.txt"))
                                    {
                                        string[] line_arr = line.Split(' ');
                                        if (line_arr[0] == usernameMessage)
                                        {

                                            if (usernameMessage == follow_username)
                                            {
                                                username_same = true;
                                            }
                                            foreach (string word in line_arr)
                                            {
                                                if (word == follow_username)
                                                {
                                                    duplicates = true;
                                                }
                                            }

                                            if (username_same) //if client user tries to follow him/herself we give an error.
                                            {
                                                logs.AppendText("You can't follow yourself!\n");
                                                Byte[] follow_fails = Encoding.Default.GetBytes("You can't follow yourself!\n");
                                                thisClient.Send(follow_fails);
                                            }

                                            else if (duplicates) //if the client user tries to follow someone that they have already been following, we give an error.
                                            {
                                                logs.AppendText(usernameMessage + " is already following " + follow_username + "\n");
                                                Byte[] follow_fails = Encoding.Default.GetBytes(usernameMessage + " is already following " + follow_username + "\n");
                                                thisClient.Send(follow_fails);

                                            }
                                            else //if the errors mentioned above did not occur, we add the given username to the list of people that client user follows.
                                            {
                                                user_in_txt = true;
                                                temp_line = line + " " + follow_username + "\n";
                                                logs.AppendText(usernameMessage + " has followed " + follow_username + "\n");

                                                Byte[] follow_success = Encoding.Default.GetBytes(usernameMessage + " has followed " + follow_username + "\n");
                                                thisClient.Send(follow_success);
                                            }



                                        }
                                    }

                                    if (usernameMessage == follow_username)
                                    {
                                        logs.AppendText("You can't follow yourself!\n");
                                        Byte[] follow_fails = Encoding.Default.GetBytes("You can't follow yourself!\n");
                                        thisClient.Send(follow_fails);
                                    }
                                    else if (user_in_txt == true) //if the errors mentioned above did not occur, we add the given username to the list of people that client user follows.
                                    {


                                        File.WriteAllLines(@"C:\Users\berka\Desktop\proj\server\server\following.txt", File.ReadLines(@"C:\Users\berka\Desktop\proj\server\server\following.txt").Where(l => l.Split(' ')[0] != usernameMessage).ToList());
                                        File.AppendAllText(@"C:\Users\berka\Desktop\proj\server\server\following.txt", temp_line);
                                    }

                                    else if (duplicates == false)
                                    {
                                        temp_line = usernameMessage + " " + follow_username + "\n";
                                        logs.AppendText(usernameMessage + " has followed " + follow_username + "\n");

                                        Byte[] follow_success = Encoding.Default.GetBytes(usernameMessage + " has followed " + follow_username + "\n");
                                        thisClient.Send(follow_success);
                                        File.AppendAllText(@"C:\Users\berka\Desktop\proj\server\server\following.txt", temp_line);
                                    }
                                }
                            }

                        }

                        else if (incomingMessage == "I want all the followed sweets") //if the incoming message is, "I want all the followed sweets, it means that user
                        {                                                            //wants the sweets from the people they follow.
                            logs.AppendText(usernameMessage + " requested followed sweet feed\n");

                            bool user_following = false;
                            foreach (string line in System.IO.File.ReadLines(@"C:\Users\berka\Desktop\proj\server\server\following.txt")) //we go through the following.txt
                            {
                                if (line.Split(' ')[0] == usernameMessage) //we find the people that client user is following and print the sweets of those users.
                                {
                                    user_following = true;
                                    string temp_line = line.Substring(usernameMessage.Length + 1);
                                    string[] temp_line_arr = temp_line.Split(' ');
                                    for (int i = 0; i < temp_line_arr.Length; i++)
                                    {
                                        foreach (string line_2 in System.IO.File.ReadLines(@"C:\Users\berka\Desktop\proj\server\server\sweets.txt")) // we get the sweets from the txt file and send them to client
                                        {
                                            string[] words = line_2.Split(' ');
                                            //logs.AppendText(line);
                                            if (words[1] == temp_line_arr[i])
                                            {
                                                if (line_2 != "" && line_2.Length <= 128)
                                                {
                                                    Byte[] all_sweet_buffer = Encoding.Default.GetBytes(line_2 + "\n");
                                                    thisClient.Send(all_sweet_buffer);
                                                    Thread.Sleep(1);
                                                }
                                            }
                                        }

                                    }
                                    break;
                                }
                            }



                            if (user_following == false) // we give an error when user is not following anyone
                            {
                                string error_message = "User " + usernameMessage + " is not following anyone!\n";
                                logs.AppendText(error_message);
                                Byte[] error_buffer = Encoding.Default.GetBytes(error_message + "\n");
                                thisClient.Send(error_buffer);
                            }


                        }

                        else if (incomingMessage.Length >= 14 && incomingMessage.Substring(0, 15) == "I want to block")//if incoming message is "I want to block"
                        {                                                  //it means a user wants to block a person
                                            
                            string block_username = incomingMessage.Substring(16);

                            bool username_found = false;
                            foreach (string line in System.IO.File.ReadLines(@"C:\Users\berka\Desktop\proj\client\client\user-db.txt")) // we check if there is a user with the given username in the database.
                            {
                                if (line == block_username)
                                {
                                    username_found = true;
                                }
                            }

                            if (username_found == false)//if not we give an error
                            {
                                Byte[] username_not_found = Encoding.Default.GetBytes("Username not found in the database\n");
                                thisClient.Send(username_not_found);
                                logs.AppendText(Encoding.Default.GetString(username_not_found));

                            }

                            else
                            {

                                string temp_line = "";
                                string temp_line_2 = "";
                                bool user_in_txt = false;
                                bool duplicates = false;
                                bool username_same = false;
                                foreach (string line in System.IO.File.ReadLines(@"C:\Users\berka\Desktop\proj\server\server\blocked.txt"))
                                {
                                    string[] line_arr = line.Split(' ');
                                    if (line_arr[0] == usernameMessage)
                                    {

                                        if (usernameMessage == block_username)
                                        {
                                            username_same = true;
                                        }
                                        foreach (string word in line_arr)
                                        {
                                            if (word == block_username)
                                            {
                                                duplicates = true;
                                            }
                                        }

                                        if (username_same) //if client user tries to block him/herself we give an error.
                                        {
                                            logs.AppendText("You can't block yourself!\n");
                                            Byte[] follow_fails = Encoding.Default.GetBytes("You can't block yourself!\n");
                                            thisClient.Send(follow_fails);
                                        }

                                        else if (duplicates) //if the client user tries to block someone that they have already blocked, we give an error.
                                        {
                                            logs.AppendText(usernameMessage + " has already blocked " + block_username + "\n");
                                            Byte[] follow_fails = Encoding.Default.GetBytes(usernameMessage + " has already blocked " + block_username + "\n");
                                            thisClient.Send(follow_fails);

                                        }
                                        else //if the errors mentioned above did not occur, we add the given username to the list of people that client user blocked.
                                        {
                                            user_in_txt = true;
                                            temp_line = line + " " + block_username + "\n";
                                            logs.AppendText(usernameMessage + " has blocked " + block_username + "\n");

                                            Byte[] follow_success = Encoding.Default.GetBytes(usernameMessage + " has blocked " + block_username + "\n");
                                            thisClient.Send(follow_success);
                                        }



                                    }







                                }

                                if(usernameMessage == block_username)
                                {
                                    logs.AppendText("You can't block yourself!\n");
                                    Byte[] follow_fails = Encoding.Default.GetBytes("You can't block yourself!\n");
                                    thisClient.Send(follow_fails);
                                }

                                else if (user_in_txt == true) //if the errors mentioned above did not occur, we add the given username to the list of people that client user blocked.
                                {


                                    File.WriteAllLines(@"C:\Users\berka\Desktop\proj\server\server\blocked.txt", File.ReadLines(@"C:\Users\berka\Desktop\proj\server\server\blocked.txt").Where(l => l.Split(' ')[0] != usernameMessage).ToList());
                                    File.AppendAllText(@"C:\Users\berka\Desktop\proj\server\server\blocked.txt", temp_line);

                                   


                                }

                                else if (duplicates == false)
                                {
                                    temp_line = usernameMessage + " " + block_username + "\n";
                                    logs.AppendText(usernameMessage + " has blocked " + block_username + "\n");

                                    Byte[] follow_success = Encoding.Default.GetBytes(usernameMessage + " has blocked " + block_username + "\n");
                                    thisClient.Send(follow_success);
                                    File.AppendAllText(@"C:\Users\berka\Desktop\proj\server\server\blocked.txt", temp_line);
                                }

                                foreach (string line in System.IO.File.ReadLines(@"C:\Users\berka\Desktop\proj\server\server\following.txt"))
                                {
                                    string[] line_arr = line.Split(' ');
                                    if (line_arr[0] == block_username)
                                    {
                                        foreach(string word in line_arr)
                                        {
                                            if(word != usernameMessage)
                                            {
                                                temp_line_2 = temp_line_2 + word + " ";
                                            }

                                        }
                                    }

                                }

                                temp_line_2 += "\n";

                                if(temp_line_2 != block_username + " \n")
                                {
                                    File.WriteAllLines(@"C:\Users\berka\Desktop\proj\server\server\following.txt", File.ReadLines(@"C:\Users\berka\Desktop\proj\server\server\following.txt").Where(l => l.Split(' ')[0] != block_username).ToList());
                                    File.AppendAllText(@"C:\Users\berka\Desktop\proj\server\server\following.txt", temp_line_2);
                                }

                                else
                                {
                                    File.WriteAllLines(@"C:\Users\berka\Desktop\proj\server\server\following.txt", File.ReadLines(@"C:\Users\berka\Desktop\proj\server\server\following.txt").Where(l => l.Split(' ')[0] != block_username).ToList());

                                }

                            }
                        }

                        else if (incomingMessage == "I want to disconnect") //if client wants to disconnect, server closes the socket and removes the client from the clientSockets list.
                        {
                            thisClient.Close();
                            usernameList.Remove(usernameMessage);
                            clientSockets.Remove(thisClient);

                            connected = false;
                            logs.AppendText(usernameMessage + " has disconnected from the server");
                        }
                        else //if the incoming message is none of the above, it means that it is a sweet. We get the sweets from the client and write them to the sweets.txt
                        {
                            File.AppendAllText(@"C:\Users\berka\Desktop\proj\server\server\sweets.txt", "");
                            if (File.ReadAllText(@"C:\Users\berka\Desktop\proj\server\server\sweets.txt") == "")
                            {
                                id = 0;
                            }

                            else
                            {
                                var lastLine = File.ReadLines(@"C:\Users\berka\Desktop\proj\server\server\sweets.txt").Last();
                                string[] last_line_array = lastLine.Split(' ');
                                id = int.Parse((last_line_array[last_line_array.Length - 4].ToString()));
                            }
                            var timeString = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                            string output_string = "Username: " + usernameMessage + " Sweet: " + incomingMessage + " Sweet Id: " + (id + 1).ToString() + " Time: " + timeString + "\n";
                            string your_sweet = "Your sweet: " + output_string;
                            Byte[] your_sweet_buffer = Encoding.Default.GetBytes(your_sweet);
                            thisClient.Send(your_sweet_buffer);


                            File.AppendAllText(@"C:\Users\berka\Desktop\proj\server\server\sweets.txt", output_string);


                        }

                    }
                    catch
                    {
                        if (!terminating)
                        {

                            logs.AppendText("A client has disconnected\n");
                        }
                        usernameList.Remove(usernameMessage); //when the user is disconnected we remove it from the users list
                        connected = false;
                    }
                }
            }
        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listening = false;
            terminating = true;
            Environment.Exit(0);
        }

        private void button_send_Click(object sender, EventArgs e) //the message in the textbox is sent to the client
        {
            string message = textBox_message.Text;
            if(message != "" && message.Length <= 64)
            {
                Byte[] buffer = Encoding.Default.GetBytes(message);
                foreach (Socket client in clientSockets)
                {
                    try
                    {
                        client.Send(buffer);
                    }
                    catch
                    {
                        logs.AppendText("There is a problem! Check the connection...\n");
                        terminating = true;
                        textBox_message.Enabled = false;
                        button_send.Enabled = false;
                        textBox_port.Enabled = true;
                        button_listen.Enabled = true;
                        serverSocket.Close();
                    }

                }
            }
        }

       
    }
}

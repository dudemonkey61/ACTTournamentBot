using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Timers;

namespace OsuTournamentBot
{
    class IrcClient
    {
        private string userName;
        private string password;
        private string channel;
        private string ip;
        private int port;

        public TcpClient tcpClient;
        private StreamReader inputStream;
        private StreamWriter outputStream;

        public IrcClient(string ip, int port, string userName, string password)
        {
            this.userName = userName;
            this.port = port;
            this.password = password;
            this.ip = ip;

            connect();
        }

        public void connect()
        {
            tcpClient = new TcpClient(ip, port);
            inputStream = new StreamReader(tcpClient.GetStream());
            outputStream = new StreamWriter(tcpClient.GetStream());

            outputStream.WriteLine("PASS " + password);
            outputStream.WriteLine("NICK " + userName);
            outputStream.WriteLine("USER " + userName + " 8 * :" + userName);
            outputStream.Flush();
        }

        public void joinRoom(string channel)
        {
            this.channel = channel;
            outputStream.WriteLine("JOIN #" + channel);
            outputStream.Flush();
        }

        public void sendIrcMessage(string message)
        {
            outputStream.WriteLine(message);
            outputStream.Flush();
        }

        public string readMessage()
        {
            string message = inputStream.ReadLine();
            return message;
        }

        public void sendPrivMessage(string message, string username)
        {
            sendIrcMessage("PRIVMSG "+username+" :"+message);
        }

        public void sendChatMessage(string message, string channel)
        {
            sendIrcMessage("PRIVMSG #" + channel + " :" + message);
        }
        
    }
}

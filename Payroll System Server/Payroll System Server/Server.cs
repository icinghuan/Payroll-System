using System;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Payroll_System_Server
{
    class Server
    {
        private static byte[] result = new byte[1024];
        private static int myProt = 8500;   //端口  
        private static IPAddress ip = IPAddress.Parse("127.0.0.1");//服务器IP地址  
        private static Socket serverSocket;
        public static void Start()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, myProt));  //绑定IP地址：端口  
            serverSocket.Listen(500);    //设定最多500个排队连接请求
            Console.WriteLine("服务器启动，启动监听{0}成功", serverSocket.LocalEndPoint.ToString());
            //通过Clientsoket发送数据  
            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();
            //Console.ReadLine();
        }

        /// <summary>  
        /// 监听客户端连接  
        /// </summary>  
        private static void ListenClientConnect()
        {
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                Thread receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start(clientSocket);
            }
        }

        /// <summary>  
        /// 接收消息  
        /// </summary>  
        /// <param name="clientSocket"></param>  
        private static void ReceiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            while (true)
            {
                try
                {
                    //通过clientSocket接收数据  
                    int receiveNumber = myClientSocket.Receive(result);
                    if (receiveNumber <= 0)
                        break;
                    Console.WriteLine("接收客户端 {0} 消息 {1}", myClientSocket.RemoteEndPoint.ToString(), Encoding.ASCII.GetString(result, 0, receiveNumber));
                    string str = Encoding.UTF8.GetString(result, 0, receiveNumber);
                    myClientSocket.Send(Encoding.UTF8.GetBytes(Message.rec(str)));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("客户端 {0} 错误 {1}", myClientSocket.RemoteEndPoint.ToString(), ex.Message);
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                    break;
                }
            }
        }
    }
}

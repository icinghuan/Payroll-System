using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Payroll_System_Client
{
    class Client
    {
        private static byte[] result = new byte[1024];//接收数据缓存
        private static int myProt = 8500;//服务器端口  
        private static IPAddress ip = IPAddress.Parse("127.0.0.1");//服务器IP地址  
        private static Socket clientSocket;
        private static string receiveMessage;
        //private static Thread myThread;
        public static bool IsConnect()//与服务器建立连接，建立失败时返回false
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(new IPEndPoint(ip, myProt)); //配置服务器IP与端口  
                //连接服务器成功
            }
            catch
            {
                MessageBox.Show("连接服务器失败，请稍后再试！", "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //连接服务器失败
                return false;
            }
            return true;
        }
        public static void Disconnect()//与服务器断开连接
        {
            if (clientSocket != null)
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
        }
        public static void Send(string sendMessage)
        {
            //通过 clientSocket 发送数据  
            try
            {
                clientSocket.Send(Encoding.UTF8.GetBytes(sendMessage));
            }
            catch
            {
                MessageBox.Show("与服务器通讯失败，请稍后再试！", " 通讯失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public static void Receive()
        {
            //通过clientSocket接收数据 
            receiveMessage = "";
            try
            {
                //接收不到信息时会阻塞线程，用多线程执行，多线程出错，待解决-------------从改用多线程到放弃。。。
                int receiveLength = clientSocket.Receive(result);
                receiveMessage = Encoding.UTF8.GetString(result, 0, receiveLength);
            }
            catch (Exception ex)
            {
                MessageBox.Show("与服务器通讯失败，请稍后再试！"+ex, " 通讯失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public static async void delayAsync()//异步等待10s
        {
            await Task.Delay(10000);
            if (receiveMessage == "")
            {
                MessageBox.Show("与服务器通讯失败，请稍后再试！", " 通讯失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //myThread.Abort();
            }
            Disconnect();
        }
        public static string Rec(string Message)//发送一个信息后等待服务器返回信息，包含服务器连接和断开（即用即连）
        {
            if (IsConnect())
            {
                Send(Message);
                Receive();
                //myThread = new Thread(Receive);
                //myThread.Start();
                //计时器，5s未收到服务器信息则认为通讯失败
                //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                //stopwatch.Restart();
                //while (receiveMessage == "" && stopwatch.ElapsedMilliseconds <= 5000) ;
                //stopwatch.Stop();
                //if (receiveMessage == "")
                //{
                //MessageBox.Show("与服务器通讯失败，请稍后再试！", " 通讯失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //myThread.Abort();
                //}
                Disconnect();
                //delayAsync();
            }
            return receiveMessage;
        }
    }
}

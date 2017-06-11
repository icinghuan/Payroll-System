using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Payroll_System_Client
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;//下拉框初始值为0（对应员工）
        }

        static int loginFailedCount = 0;
        async void delayAsync()//异步等待10s
        {
            await Task.Delay(10000);
            button1.Enabled = true;
        }
        public static string account;
        public static string pwd;
        public static string id;
        private void button1_Click(object sender, EventArgs e)
        {
            account = textBox1.Text;
            pwd = textBox2.Text;
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^[A-Za-z][A-Za-z0-9]{2,24}$");
            //通过正则表达式限制用户名至少为3位，且以字母开头后接字母或数字
            if (!reg.IsMatch(account))
            {
                MessageBox.Show("您输入的用户名有误！\n必须为字母后接字母或数字的组合，且长度至少3位！", "登录失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (pwd == "")//密码为空时
            {
                MessageBox.Show("密码不能为空！", "登录失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }//输入数据有误时的提示

            if (comboBox1.SelectedIndex == 0)//员工身份登录
            {
                string rec = Client.Rec("1 " + account + " " + pwd);//自定消息，发送至服务器，开头的1或者0分别代表不同事务，例如1代表员工登录，0代表管理员登录，在服务器代码中有详细备注
                if (rec == null)//没有回应则返回
                    return;
                if (rec == "0")//服务器回复的自定义消息，不为0代表用户存在且密码正确
                {
                    MessageBox.Show("您输入的用户名或密码错误！", "登录失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    loginFailedCount++;                    
                }
                else
                {
                    MessageBox.Show("欢迎进入工资单管理系统！", "登陆成功！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    id = rec;
                    Employee employee = new Employee();
                    employee.Show();//打开员工界面
                    this.Hide();
                }
            }

            if (comboBox1.SelectedIndex == 1)//管理员身份登录
            {
                string rec = Client.Rec("0 " + account + " " + pwd);
                if (rec == null)
                    return;
                if (rec == "1")
                {
                    MessageBox.Show("欢迎进入工资单管理系统！", "登陆成功！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Admin admin = new Admin();
                    admin.Show();//打开管理员界面
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("您输入的用户名或密码错误！", "登录失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    loginFailedCount++;
                }    
            }

            if (loginFailedCount >= 5)//登录失败次数过多时锁定登录按钮10s
            {
                MessageBox.Show("登录失败次数过多，请稍后再试！", "登录失败！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loginFailedCount = 0;
                button1.Enabled = false;
                delayAsync();
            }
        }

    }
}

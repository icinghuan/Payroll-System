using System;
using System.Windows.Forms;
using System.IO;

namespace Payroll_System_Client
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
        }
        private static string account;//保存账号信息
        private static string pwd;//保存密码
        private void Admin_Load(object sender, EventArgs e)
        {
            account = Login.account;
            pwd = Login.pwd;
        }

        private void Admin_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void timer1_Tick(object sender, EventArgs e)//显示时间及欢迎信息
        {
            string time = System.DateTime.Now.ToString();
            label1.Text = "管理员 " + account + "   现在是" + time;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)//选择不同操作
        {
            if (comboBox1.SelectedIndex == 0)
            {
                groupBox1.Enabled = true;
                button1.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                numericUpDown7.Enabled = false;
            }
            if (comboBox1.SelectedIndex == 1)
            {
                groupBox1.Enabled = true;
                //button2.Enabled = true;
                button1.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = true;
                numericUpDown7.Enabled = true;
            }
            if (comboBox1.SelectedIndex == 2)
            {
                groupBox1.Enabled = true;
                //button3.Enabled = true;
                button2.Enabled = false;
                button1.Enabled = false;
                button4.Enabled = true;
                numericUpDown7.Enabled = true;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)//选择不同员工类型
        {
            if (comboBox2.SelectedIndex == 0)
            {
                numericUpDown1.Enabled = true;
                numericUpDown2.Enabled = false;
                comboBox3.Enabled = false;
            }
            if (comboBox2.SelectedIndex == 1)
            {
                numericUpDown1.Enabled = false;
                numericUpDown2.Enabled = true;
                comboBox3.Enabled = false;
            }
            if (comboBox2.SelectedIndex == 2)
            {
                numericUpDown1.Enabled = false;
                numericUpDown2.Enabled = true;
                comboBox3.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)//创建员工
        {
            string account = textBox1.Text;
            string pwd = textBox2.Text;
            string name = textBox3.Text;
            string mail = textBox4.Text;
            string socialnum = textBox5.Text;
            string phone = textBox6.Text;
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^[A-Za-z][A-Za-z0-9]{2,24}$");
            //通过正则表达式限制用户名至少为3位，且以字母开头后接字母或数字
            if (!reg.IsMatch(account))
            {
                MessageBox.Show("用户名格式错误！\n必须为字母后接字母或数字的组合，且长度至少3位！", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!Util.IsValidPassword(pwd))
            {
                MessageBox.Show("密码格式错误！", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!Util.IsValidUserName(name))
            {
                MessageBox.Show("姓名格式错误！", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!Util.IsEmail(mail))
            {
                MessageBox.Show("邮箱格式错误！", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!Util.IsNumber(socialnum))
            {
                MessageBox.Show("社会保险号码格式错误！", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!Util.IsNumber(phone))
            {
                MessageBox.Show("电话号码格式错误！", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("员工类型未选择！", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (comboBox2.SelectedIndex == 2 && comboBox3.SelectedIndex == -1)
            {
                MessageBox.Show("提成比例未选择！", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string type = "null";
            string hourlyrate = "null";
            string salary = "null";
            string commissionedrate = "null";
            if (comboBox2.SelectedIndex == 0)
            {
                type = "hour";
                hourlyrate = numericUpDown1.Value.ToString();
            }
            if (comboBox2.SelectedIndex == 1)
            {
                type = "salaried";
                salary = numericUpDown2.Value.ToString();
            }
            if (comboBox2.SelectedIndex == 2)
            {
                type = "commissioned";
                salary = numericUpDown2.Value.ToString();
                if (comboBox3.SelectedIndex == 0)
                {
                    commissionedrate = "0.1";
                }
                if (comboBox3.SelectedIndex == 1)
                {
                    commissionedrate = "0.15";
                }
                if (comboBox3.SelectedIndex == 2)
                {
                    commissionedrate = "0.25";
                }
                if (comboBox3.SelectedIndex == 3)
                {
                    commissionedrate = "0.35";
                }
            }
            //MessageBox.Show("2 " + account + " " + pwd + " " + name + " " + type + " " + mail + " " +
                        //socialnum + " " + numericUpDown3.Value.ToString() + " " + numericUpDown4.Value.ToString()
                        //+ " " + numericUpDown5.Value.ToString() + " " + phone + " " + hourlyrate + " " + salary +
                        //" " + commissionedrate + " " + numericUpDown6.Value.ToString());
            string rec = Client.Rec("2 " + account + " " + pwd + " " + name + " " + type + " " + mail + " " +
                        socialnum + " " + numericUpDown3.Value.ToString() + " " + numericUpDown4.Value.ToString()
                        + " " + numericUpDown5.Value.ToString() + " " + phone + " " + hourlyrate + " " + salary +
                        " " + commissionedrate + " " + numericUpDown6.Value.ToString());//生成发送到服务器的自定义消息
            if (rec == null)
                return;
            if (rec == "0")
            {
                MessageBox.Show("用户名已存在！", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                MessageBox.Show("创建成功！员工id为"+rec, "创建成功！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //MessageBox.Show("服务器发生未知错误！请稍后再试", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        private static string id;//保存员工id
        private void button4_Click(object sender, EventArgs e)//查询员工
        {
            button2.Enabled = false;
            button3.Enabled = false;
            id = numericUpDown7.Value.ToString();
            string rec = Client.Rec("3 " + id);
            if (rec == null)
                return;
            if (rec == "0")
            {
                MessageBox.Show("该员工不存在！", "查询失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                string[] strList = rec.Split(' ');
                textBox1.Text = strList[0];
                textBox2.Text = strList[1];
                textBox3.Text = strList[2];
                if (strList[3] == "hour")
                    comboBox2.SelectedIndex = 0;
                if (strList[3] == "salaried")
                    comboBox2.SelectedIndex = 1;
                if (strList[3] == "commissioned")
                    comboBox2.SelectedIndex = 2;
                textBox4.Text = strList[4];
                textBox5.Text = strList[5];
                numericUpDown3.Value = int.Parse(strList[6]);
                numericUpDown4.Value = int.Parse(strList[7]);
                numericUpDown5.Value = int.Parse(strList[8]);
                textBox6.Text = strList[9];
                if (strList[10] != "")
                    numericUpDown1.Value = int.Parse(strList[10]);
                if (strList[11] != "")
                    numericUpDown2.Value = int.Parse(strList[11]);
                if (strList[12] == "0.1" || strList[12] == "0.10")
                    comboBox3.SelectedIndex = 0;
                if (strList[12] == "0.15")
                    comboBox3.SelectedIndex = 1;
                if (strList[12] == "0.25")
                    comboBox3.SelectedIndex = 2;
                if (strList[12] == "0.35")
                    comboBox3.SelectedIndex = 3;
                numericUpDown6.Value = int.Parse(strList[13]);
                if (comboBox1.SelectedIndex == 1)
                {
                    button2.Enabled = true;
                    button3.Enabled = false;
                }
                if (comboBox1.SelectedIndex == 2)
                {
                    button2.Enabled = false;
                    button3.Enabled = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)//大部分复制自button1，修改员工
        {
            string account = textBox1.Text;
            string pwd = textBox2.Text;
            string name = textBox3.Text;
            string mail = textBox4.Text;
            string socialnum = textBox5.Text;
            string phone = textBox6.Text;
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^[A-Za-z][A-Za-z0-9]{2,24}$");
            //通过正则表达式限制用户名至少为3位，且以字母开头后接字母或数字
            if (!reg.IsMatch(account))
            {
                MessageBox.Show("用户名格式错误！\n必须为字母后接字母或数字的组合，且长度至少3位！", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!Util.IsValidPassword(pwd))
            {
                MessageBox.Show("密码格式错误！", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!Util.IsValidUserName(name))
            {
                MessageBox.Show("姓名格式错误！", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!Util.IsEmail(mail))
            {
                MessageBox.Show("邮箱格式错误！", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!Util.IsNumber(socialnum))
            {
                MessageBox.Show("社会保险号码格式错误！", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!Util.IsNumber(phone))
            {
                MessageBox.Show("电话号码格式错误！", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("员工类型未选择！", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (comboBox2.SelectedIndex == 2 && comboBox3.SelectedIndex == -1)
            {
                MessageBox.Show("提成比例未选择！", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string type = "null";
            string hourlyrate = "null";
            string salary = "null";
            string commissionedrate = "null";
            if (comboBox2.SelectedIndex == 0)
            {
                type = "hour";
                hourlyrate = numericUpDown1.Value.ToString();
            }
            if (comboBox2.SelectedIndex == 1)
            {
                type = "salaried";
                salary = numericUpDown2.Value.ToString();
            }
            if (comboBox2.SelectedIndex == 2)
            {
                type = "commissioned";
                salary = numericUpDown2.Value.ToString();
                if (comboBox3.SelectedIndex == 0)
                {
                    commissionedrate = "0.1";
                }
                if (comboBox3.SelectedIndex == 1)
                {
                    commissionedrate = "0.15";
                }
                if (comboBox3.SelectedIndex == 2)
                {
                    commissionedrate = "0.25";
                }
                if (comboBox3.SelectedIndex == 3)
                {
                    commissionedrate = "0.35";
                }
            }
            //MessageBox.Show("2 " + account + " " + pwd + " " + name + " " + type + " " + mail + " " +
            //socialnum + " " + numericUpDown3.Value.ToString() + " " + numericUpDown4.Value.ToString()
            //+ " " + numericUpDown5.Value.ToString() + " " + phone + " " + hourlyrate + " " + salary +
            //" " + commissionedrate + " " + numericUpDown6.Value.ToString());
            string rec = Client.Rec("4 " + id + " " + account + " " + pwd + " " + name + " " + type + " " + mail + " " +
                        socialnum + " " + numericUpDown3.Value.ToString() + " " + numericUpDown4.Value.ToString()
                        + " " + numericUpDown5.Value.ToString() + " " + phone + " " + hourlyrate + " " + salary +
                        " " + commissionedrate + " " + numericUpDown6.Value.ToString());
            if (rec == null)
                return;
            if (rec == "0")
            {
                MessageBox.Show("服务器错误！", "修改失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                MessageBox.Show("修改成功！", "修改成功！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)//删除员工
        {
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("确定要删除吗？\n被删除员工将在发薪后从系统删除", "确认删除", messButton);
            if (dr == DialogResult.OK)//如果点击“确定”按钮
            {
                string rec = Client.Rec("5 " + id);
                if (rec == null)
                    return;
                if (rec == "0")
                {
                    MessageBox.Show("员工已经在删除队列中！", "删除失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    MessageBox.Show("删除成功！", "删除成功！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else//如果点击“取消”按钮
            {
                return;
            }
        }

        private void button11_Click(object sender, EventArgs e)//清空报表
        {
            textBox7.Clear();
            button11.Enabled = false;
        }

        private void button10_Click(object sender, EventArgs e)//保存报表到本地
        {
            StreamWriter myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string str;
                str = saveFileDialog1.FileName;
                myStream = new StreamWriter(saveFileDialog1.FileName);
                myStream.Write(textBox7.Text);
                myStream.Flush();
                myStream.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)//生成报表
        {
            string id = numericUpDown8.Value.ToString();//防止和之前查询的id冲突
            string rec0 = Client.Rec("3 " + id);//查询员工是否存在
            if (rec0 == null)
                return;
            if (rec0 == "0")
            {
                MessageBox.Show("该员工不存在！", "查询失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string name = rec0.Split(' ')[2];
            if (comboBox4.SelectedIndex == 0)//总工作时间
            {
                DateTime begin = dateTimePicker2.Value;
                DateTime end = dateTimePicker3.Value;
                if (begin.Date > end.Date)
                {
                    MessageBox.Show("开始时间大于结束时间！", "生成失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                string str = "16 " + id + " " + begin.Date + " " + end.Date;
                //MessageBox.Show(str);
                string rec = Client.Rec(str);
                if (rec == null)
                    return;
                if (rec == "fail")
                {
                    MessageBox.Show("生成报表失败！请稍后再试", "生成失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    button10.Enabled = true;
                    button11.Enabled = true;
                    textBox7.Text += begin.ToString().Split(' ')[0] + "-" + end.ToString().Split(' ')[0] + " id : " + id + "  name : " + name + "  Total Hours Worked : " + rec + "\r\n";
                    return;
                }
            }
            if (comboBox4.SelectedIndex == 1)//总工资
            {
                DateTime begin = dateTimePicker2.Value;
                DateTime end = dateTimePicker3.Value;
                if (begin.Date > end.Date)
                {
                    MessageBox.Show("开始时间大于结束时间！", "生成失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                string str = "19 " + id + " " + begin.Date + " " + end.Date;
                //MessageBox.Show(str);
                string rec = Client.Rec(str);
                if (rec == null)
                    return;
                if (rec == "fail")
                {
                    MessageBox.Show("生成报表失败！请稍后再试", "生成失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    button10.Enabled = true;
                    button11.Enabled = true;
                    textBox7.Text += begin.ToString().Split(' ')[0] + "-" + end.ToString().Split(' ')[0] + " id : " + id + "  name : " + name + " Total Pay Year-to-Date : " + rec + "\r\n";
                    return;
                }
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)//选择生成的报表类型
        {
            button5.Enabled = true;
            numericUpDown8.Enabled = true;
        }
    }
}

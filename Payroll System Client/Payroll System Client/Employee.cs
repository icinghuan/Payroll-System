using System;
using System.Windows.Forms;
using System.IO;

namespace Payroll_System_Client
{
    public partial class Employee : Form
    {
        public Employee()
        {
            InitializeComponent();
        }

        private void Employee_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }
        private static string account;
        private static string pwd;
        private static string type;
        private static string id;
        private void Employee_Load(object sender, EventArgs e)//窗口初始化时与服务端通讯查询员工类型
        {
            account = Login.account;
            pwd = Login.pwd;
            id = Login.id;
            string rec = Client.Rec("3 " + id);
            if (rec == null)
                return;
            if (rec == "0")
            {
                MessageBox.Show("该员工不存在！", "服务器错误！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                string[] strList = rec.Split(' ');
                if (strList[3] == "hour")
                    type = "hour";
                if (strList[3] == "salaried")
                    type = "salaried";
                if (strList[3] == "commissioned")
                {
                    type = "commissioned";
                    groupBox1.Enabled = true;
                }
                if (type == "hour")
                    ty = "小时工";
                if (type == "salaried")
                    ty = "月薪员工";
                if (type == "commissioned")
                    ty = "提成员工";
            }
            //MessageBox.Show(dateTimePicker1.Value.ToString());
        }
        private static string ty = "";
        private void timer1_Tick(object sender, EventArgs e)
        {
            string time = System.DateTime.Now.ToString();
            label1.Text = "员工 " + account + "   员工id为 " + id + "   员工类型为 " + ty + "   现在是" + time;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)//选择维护订购单的操作
        {
            if (comboBox1.SelectedIndex == 0)
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                numericUpDown1.Enabled = true;
                dateTimePicker1.Enabled = true;
                numericUpDown7.Enabled = false;
                button4.Enabled = false;
                button1.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = false;
            }
            if (comboBox1.SelectedIndex == 1)
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                numericUpDown1.Enabled = true;
                dateTimePicker1.Enabled = true;
                numericUpDown7.Enabled = true;
                button4.Enabled = true;
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
            }
            if (comboBox1.SelectedIndex == 2)
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                numericUpDown1.Enabled = false;
                dateTimePicker1.Enabled = false;
                numericUpDown7.Enabled = true;
                button4.Enabled = true;
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
            }
        }
        private static string pid;
        private void button1_Click(object sender, EventArgs e)//创建订购单
        {
            string contact = textBox1.Text;
            string address = textBox2.Text;
            string product = textBox3.Text;
            string date = dateTimePicker1.Value.GetDateTimeFormats()[0];
            string amount = numericUpDown1.Value.ToString();
            if (contact == "")
            {
                MessageBox.Show("顾客联络点不能为空", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (address == "")
            {
                MessageBox.Show("顾客账单地址不能为空", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (product == "")
            {
                MessageBox.Show("购买商品不能为空", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!Util.IsValidUserName(contact))
            {
                MessageBox.Show("顾客联络点格式错误！", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!Util.IsValidUserName(address))
            {
                MessageBox.Show("顾客账单地址格式错误！", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!Util.IsValidUserName(product))
            {
                MessageBox.Show("购买商品格式错误！", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string rec = Client.Rec("6 " + id + " " + contact + " " + address + " " + product + " " + date + " " + amount);
            if (rec == null)
                return;
            if (rec == "0")
            {
                MessageBox.Show("订单创建失败！请稍后再试", "创建失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                pid = rec;
                MessageBox.Show("创建成功！订购单id为" + pid, "创建成功！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)//修改订购单
        {
            string contact = textBox1.Text;
            string address = textBox2.Text;
            string product = textBox3.Text;
            string date = dateTimePicker1.Value.GetDateTimeFormats()[0];
            string amount = numericUpDown1.Value.ToString();
            if (contact == "")
            {
                MessageBox.Show("顾客联络点不能为空", "修改失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (address == "")
            {
                MessageBox.Show("顾客账单地址不能为空", "修改失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (product == "")
            {
                MessageBox.Show("购买商品不能为空", "修改失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!Util.IsValidUserName(contact))
            {
                MessageBox.Show("顾客联络点格式错误！", "修改失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!Util.IsValidUserName(address))
            {
                MessageBox.Show("顾客账单地址格式错误！", "修改失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!Util.IsValidUserName(product))
            {
                MessageBox.Show("购买商品格式错误！", "修改失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string rec = Client.Rec("8 " + pid + " " + contact + " " + address + " " + product + " " + date + " " + amount);
            if (rec == null)
                return;
            if (rec == "0")
            {
                MessageBox.Show("订单修改失败！请稍后再试", "修改失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                MessageBox.Show("修改成功！", "修改成功！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)//删除订购单
        {
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("确定要删除吗？", "确认删除", messButton);
            if (dr == DialogResult.OK)//如果点击“确定”按钮
            {
                string rec = Client.Rec("9 " + pid);
                if (rec == null)
                    return;
                if (rec == "0")
                {
                    MessageBox.Show("订单删除失败！请稍后再试", "删除失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    MessageBox.Show("删除成功！", "删除成功！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)//查询订购单
        {
            button2.Enabled = false;
            button3.Enabled = false;
            pid = numericUpDown7.Value.ToString();
            string rec = Client.Rec("7 " + pid);
            if (rec == null)
                return;
            if (rec == "0")
            {
                MessageBox.Show("该订单不存在！", "查询失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string[] strList = rec.Split(' ');
            if (strList[0] != id)
            {
                MessageBox.Show("该订单不属于您！", "查询失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (strList[6] == "closed")
            {
                MessageBox.Show("订单已关闭！", "查询失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                MessageBox.Show(rec);
                textBox1.Text = strList[1];
                textBox2.Text = strList[2];
                textBox3.Text = strList[3];
                dateTimePicker1.Value = Convert.ToDateTime(strList[4]);
                numericUpDown1.Value = int.Parse(strList[5]);
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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)//选择收款方式
        {
            if (comboBox2.SelectedIndex == 0)
            {
                textBox6.Enabled = false;
                textBox5.Enabled = false;
                textBox4.Enabled = false;
            }
            if (comboBox2.SelectedIndex == 1)
            {
                textBox6.Enabled = true;
                textBox5.Enabled = false;
                textBox4.Enabled = false;
            }
            if (comboBox2.SelectedIndex == 2)
            {
                textBox6.Enabled = false;
                textBox5.Enabled = true;
                textBox4.Enabled = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)//修改收款方式
        {
            string payment = "pickup";
            string address = "null";
            string bankaccount = "null";
            string bankname = "null";
            if (comboBox2.SelectedIndex == 1)
            {
                address = textBox6.Text;
                payment = "mail";
                if (address == "")
                {
                    MessageBox.Show("邮寄地址不能为空", "修改失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (!Util.IsValidUserName(address))
                {
                    MessageBox.Show("邮寄地址格式错误", "修改失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

            }
            if (comboBox2.SelectedIndex == 2)
            {
                bankaccount = textBox5.Text;
                bankname = textBox4.Text;
                payment = "deposit";
                if (bankaccount == "")
                {
                    MessageBox.Show("银行账户不能为空", "修改失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (!Util.IsNumber(bankaccount))
                {
                    MessageBox.Show("银行账户格式错误", "修改失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (bankname == "")
                {
                    MessageBox.Show("账户姓名不能为空", "修改失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (!Util.IsValidUserName(bankname))
                {
                    MessageBox.Show("账户姓名格式错误", "修改失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            string rec = Client.Rec("10 " + id + " " + payment + " " + address + " " + bankaccount + " " + bankname);
            if (rec == null)
                return;
            if (rec == "0")
            {
                MessageBox.Show("支付方式修改失败！请稍后再试", "修改失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                MessageBox.Show("修改成功！", "修改成功！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }
        private DateTime mon;
        private DateTime fri;
        private void button6_Click(object sender, EventArgs e)//开始维护考勤表，与服务端通讯，查询考勤表及项目清单
        {
            button6.Enabled = false;
            string rec = Client.Rec("11 " + id);
            if (rec == null)
            {
                button6.Enabled = true;
                return;
            }
            if (rec == "0")
            {
                button6.Enabled = true;
                MessageBox.Show("服务器查找考勤表失败！请稍后再试", "维护失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                comboBox3.Enabled = true;
                string[] recList = rec.Split(' ');
                mon = Convert.ToDateTime(recList[0]);
                fri = Convert.ToDateTime(recList[2]);
                dateTimePicker4.Value = mon;
                dateTimePicker5.Value = fri;
                int c = 0;
                foreach (var i in recList)
                {
                    if (c >= 4)
                        comboBox3.Items.Add(i);
                    c++;
                }
                return;
            }
        }

        private string chargenum;
        private int[] worktime = new int[6];//worktime[5] save the tid of the timecard
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)//选择记账编号
        {
            comboBox5.Enabled = false;
            numericUpDown2.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            chargenum = comboBox3.Text;
            string rec = Client.Rec("12 " + id + " " + chargenum + " " + mon + " " + fri);
            //MessageBox.Show(rec);
            if (rec == null)
                return;
            if (rec == "0")
            {
                MessageBox.Show("读取项目失败！请稍后再试", "读取失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string[] recList = rec.Split(' ');
            if (recList[0] == "submitted")
            {
                comboBox5.Enabled = true;
                MessageBox.Show("该考勤表已经提交！无法更改", "读取失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                for (int c = 0; c < 6; ++c)
                {
                    worktime[c] = int.Parse(recList[c + 1]);
                }
                return;
            }
            else
            {
                comboBox5.Enabled = true;
                numericUpDown2.Enabled = true;
                button7.Enabled = true;
                button8.Enabled = true;
                int c = 0;
                foreach (var s in recList)
                {
                    worktime[c] = int.Parse(s);
                    c++;
                }
                return;
            }
        }

        private void button7_Click(object sender, EventArgs e)//保存考勤表信息
        {
            string str = "13";
            foreach (var i in worktime)
                str += " " + i.ToString();
            string rec = Client.Rec(str);
            if (rec == null)
                return;
            if (rec == "0")
            {
                MessageBox.Show("保存考勤表失败！请稍后再试", "保存失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                MessageBox.Show("保存考勤表成功！", "保存失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void button8_Click(object sender, EventArgs e)//提交考勤表信息，服务端会检查是否超时工作
        {
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("确定要提交吗？\n提交后无法更改", "确认提交", messButton);
            if (dr == DialogResult.OK)//如果点击“确定”按钮
            {
                string str = "14";
                foreach (var i in worktime)
                    str += " " + i.ToString();
                string rec = Client.Rec(str);
                if (rec == null)
                    return;
                if (rec == "0")
                {
                    MessageBox.Show("提交考勤表失败！请稍后再试", "提交失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (rec == "ok")
                {
                    button7.Enabled = false;
                    button8.Enabled = false;
                    MessageBox.Show("提交考勤表成功！", "提交失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                MessageBox.Show("提交考勤表失败！\n星期" + rec + "工作时间超时", "提交失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }                
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)//选择星期
        {
            numericUpDown2.Value = worktime[comboBox5.SelectedIndex];
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)//星期一到星期五对应每天的工作时间
        {
            worktime[comboBox5.SelectedIndex] = int.Parse(numericUpDown2.Value.ToString());
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)//选择报表类型
        {
            button9.Enabled = true;
            comboBox6.Enabled = false;
            if (comboBox4.SelectedIndex == 1)
            {
                string rec = Client.Rec("15");
                if (rec == null)
                {
                    return;
                }
                if (rec == "0")
                {
                    MessageBox.Show("服务器查找项目失败！请稍后再试", "查询失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    comboBox6.Items.Clear();
                    comboBox6.Enabled = true;
                    string[] recList = rec.Split(' ');
                    int c = 0;
                    foreach (var i in recList)
                    {
                        if (c >= 1)
                            comboBox6.Items.Add(i);
                        c++;
                    }
                    return;
                }
            }
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

        private void button9_Click(object sender, EventArgs e)//生成报表
        {
            if (comboBox4.SelectedIndex == 0)//总工作时间
            {
                DateTime begin = dateTimePicker2.Value;
                DateTime end = dateTimePicker3.Value;
                if (begin.Date > end.Date)
                {
                    MessageBox.Show("开始时间大于结束时间！", "生成失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                string str = "16 "+id+" "+begin.Date+" "+end.Date;
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
                    textBox7.Text += begin.ToString().Split(' ')[0] + "-" + end.ToString().Split(' ')[0] + " Total Hours Worked : " + rec + "\r\n";
                    return;
                }
            }
            if (comboBox4.SelectedIndex == 1)//项目总工作时间
            {
                string pid = comboBox6.Text;//防止和之前的pid冲突所以重新定义了一个局部变量
                if (pid == "")
                {
                    MessageBox.Show("未选择记账编号！", "生成失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                DateTime begin = dateTimePicker2.Value;
                DateTime end = dateTimePicker3.Value;
                if (begin.Date > end.Date)
                {
                    MessageBox.Show("开始时间大于结束时间！", "生成失败！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                string str = "17 " + id + " " + begin.Date + " " + end.Date + " " + pid;
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
                    textBox7.Text += begin.ToString().Split(' ')[0] + "-" + end.ToString().Split(' ')[0] + " Total Hours Worked for a Project : " + rec + "\r\n";
                    return;
                }
            }
            if (comboBox4.SelectedIndex == 2)//剩余假期
            {
                string str = "18 " + id;
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
                    textBox7.Text += "Vacation/Sick Leave : " + rec + "\r\n";
                    return;
                }
            }
            if (comboBox4.SelectedIndex == 3)//总工资
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
                    textBox7.Text += begin.ToString().Split(' ')[0] + "-" + end.ToString().Split(' ')[0] + " Total Pay Year-to-Date : " + rec + "\r\n";
                    return;
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)//清空报表
        {
            textBox7.Clear();
            button11.Enabled = false;
        }
    }
}

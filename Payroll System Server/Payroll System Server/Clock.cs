using System;
using System.Linq;
using System.Net.Mail;
using System.Net;

namespace Payroll_System_Server
{
    class Clock
    {
        public static void mail(string mailTo)
        {   //假定企业域名为acme.com
            return;//预留，暂不发送邮件
            MailMessage msg = new MailMessage();
            string mailFrom = "system@acme.com";            
            msg.From = new MailAddress(mailFrom, mailFrom);         //发件人的Email地址
            msg.To.Add(new MailAddress(mailTo, mailTo));                 //发送的目标地址
            msg.Subject = "工资单已支付";                                      //邮件标题
            msg.Body = "您本月（周）应得工资已经支付，请注意查收。 "+DateTime.Now.ToString();  //邮件内容
            msg.IsBodyHtml = true;                                                            //邮件正文格式
            msg.Priority = MailPriority.Normal;      //优先级别有3个级别 Normal 一般 High 最高 Low 最低
            SmtpClient c = new SmtpClient();                                         //邮件发送类对象   
            c.Host = "smtp.acme.com";                       //smtp协议地址
            string userName = mailFrom.Substring(0, mailFrom.IndexOf("@"));      //取发件人Email用户名
            c.Credentials = new NetworkCredential("system", "systempassword");//用户名及密码
            c.Send(msg);
            msg.Dispose();   //释放资源
            c.Dispose();     //释放资源
            Console.WriteLine("The email to " + mailTo + " has been mailed.");
        }

        public static void Paycheck(string name, int amount)//姓名，金额——打印支付支票
        {
            //print paychek 打印支票
        }

        public static void Paycheck(string name, int amount, string address)//姓名，金额，地址——打印支付支票并邮寄
        {
            //print paycheck 打印支票
            //mail paycheck 邮寄支票（打印快递单）
        }

        public static bool Deposit(int amount, string bankaccount, string bankname)//数额，账户，账户姓名——与银行系统连接并汇款
        {
            //与银行系统连接并支付，如果支付成功返回true，支付失败返回false
            return true;
        }

        public static void Delete(int id)//发完工资后删除需要删除的员工，先判断是否在删除队列中，如果在，就删除（已经发完工资了）
        {
            using (var db2 = new payrollEntities())
            {
                var waits = db2.Database.SqlQuery<waitdelete>("select * from waitdelete");
                foreach(var i in waits)
                {
                    if (id == i.id)
                    {
                        using (var db3 = new payrollEntities())
                        {
                            db3.Database.ExecuteSqlCommand("delete from employee where id = '" + id + "'");
                        }
                        using (var db3 = new payrollEntities())
                        {
                            db3.Database.ExecuteSqlCommand("delete from waitdelete where id = '" + id + "'");
                        }
                    }
                }
            }

        }

        public static void Payroll(object obj)//自动发工资的程序
        {
            DateTime now = DateTime.Now;
            using (var db = new payrollEntities())//每小时检查一次未支付账单，支付（这里就是为了防止银行系统无法连接，所以每小时检查一次）
            {
                var records = db.Database.SqlQuery<record>("select * from record where `status` = 'no'");
                foreach (var r in records)
                {
                    int id = r.id;
                    employee e;
                    perference p;
                    using (var db2 = new payrollEntities())
                    {
                        e = db2.Database.SqlQuery<employee>("select * from employee where id = '" + id + "'").Single();
                    }
                    using (var db2 = new payrollEntities())
                    {
                        p = db2.Database.SqlQuery<perference>("select * from perference where id = '" + id + "'").Single();                        
                    }
                    if (p.payment == "pickup")//直接领取
                    {
                        Paycheck(e.name, r.amount);
                        Console.WriteLine("pickup : " + e.name + " has been paid by " + DateTime.Now.ToString()+ ".The amout is " + r.amount + ". ");
                        mail(e.mail);
                        Delete(id);//查询该员工是否需要删除
                        using (var db2 = new payrollEntities())
                        {
                            db2.Database.ExecuteSqlCommand("update record set `status` = 'yes' where pid = '" + r.pid + "'");
                        }
                    }
                    if (p.payment == "mail")//邮寄支票
                    {
                        Paycheck(e.name, r.amount, p.address);
                        Console.WriteLine("mail : " + e.name + " has been paid by " + DateTime.Now.ToString() + ".The amout is " + r.amount + ". ");
                        mail(e.mail);
                        Delete(id);//查询该员工是否需要删除
                        using (var db2 = new payrollEntities())
                        {
                            db2.Database.ExecuteSqlCommand("update record set `status` = 'yes' where pid = '" + r.pid + "'");
                        }
                    }
                    if (p.payment == "deposit")//存入银行账户
                    {
                        if (Deposit(r.amount, p.bankaccount, p.bankname))
                        {
                            Console.WriteLine("desposit : " + e.name + " has been paid by " + DateTime.Now.ToString() + ".The amout is " + r.amount + ". ");
                            mail(e.mail);
                            Delete(id);//查询该员工是否需要删除
                            using (var db2 = new payrollEntities())
                            {
                                db2.Database.ExecuteSqlCommand("update record set `status` = 'yes' where pid = '" + r.pid + "'");
                            }
                        }
                    }

                }
            }
                
            if (now >= GetWeekLastDayFri().AddHours(20) && now < GetWeekLastDayFri().AddHours(21))//每周五20点-21点执行小时工工资计算
                                                                                                    //（主程序定时器每小时执行一次），为了避免刚好20点启动程序所以前面有=后面判断没有=，每周只执行一次
            {
                //Console.WriteLine(now);
                using (var db = new payrollEntities())
                {
                    var employees = db.Database.SqlQuery<employee>("select * from employee where type = 'hour'");//小时工
                    foreach (var e in employees)
                    {
                        timecard t;
                        using (var db2 = new payrollEntities())
                        {
                            if (db2.Database.SqlQuery<timecard>("select * from timecard where id = '" + e.id + "' and begin = '" + GetWeekFirstDayMon().ToString() + "' and end = '" + GetWeekLastDayFri().ToString() + "' and chargenum = '0'").Count<timecard>() == 0)
                                return;
                            else
                                t = db2.Database.SqlQuery<timecard>("select * from timecard where id = '" + e.id + "' and begin = '" + GetWeekFirstDayMon().ToString() + "' and end = '" + GetWeekLastDayFri().ToString() + "' and chargenum = '0'").Single();
                        }
                        double pay = 0;                        
                        long[] a = new long[5];
                        a[0] = t.mon; a[1] = t.tue; a[2] = t.wed; a[3] = t.thu; a[4] = t.fri;
                        for (int i = 0; i < 5; ++i)
                            if (a[i] > 8)
                                pay += 8 * Convert.ToDouble(e.hourlyrate) + Convert.ToDouble(a[i] - 8) * Convert.ToDouble(e.hourlyrate) * 1.5;
                            else
                                pay += Convert.ToDouble(a[i]) * Convert.ToDouble(e.hourlyrate);
                        int paya = Convert.ToInt32(Convert.ToInt64(pay) - e.tax - e.pension - e.medical);
                        using (var db2 = new payrollEntities())
                        {
                            db2.Database.ExecuteSqlCommand("insert into record (id,date,amount,`status`) values ('" + e.id + "','" + now.ToString() + "','" + paya.ToString() + "','no')");//生成工资记录，no表示尚未支付
                        }
                    }
                }
            }

            if (now >= GetLastWorkingTime().AddHours(20) && now < GetLastWorkingTime().AddHours(21))//每月最后一个工作日20点-21点执行月薪员工和提成员工工资计算
                                                                                                    //（主程序定时器每小时执行一次），为了避免刚好20点启动程序所以前面有=后面判断没有=，每周只执行一次
            {
                //Console.WriteLine(now);
                using (var db = new payrollEntities())
                {
                    var employees = db.Database.SqlQuery<employee>("select * from employee where type = 'salaried'");//月薪员工
                    foreach (var e in employees)
                    {                      
                        int paya = Convert.ToInt32(Convert.ToInt64(e.salary) - e.tax - e.pension - e.medical);
                        using (var db2 = new payrollEntities())
                        {
                            db2.Database.ExecuteSqlCommand("insert into record (id,date,amount,`status`) values ('" + e.id + "','" + now.ToString() + "','" + paya.ToString() + "','no')");//生成工资记录，no表示尚未支付
                        }
                    }
                }

                using (var db = new payrollEntities())
                {
                    var employees = db.Database.SqlQuery<employee>("select * from employee where type = 'commissioned'");//提成员工
                    foreach (var e in employees)
                    {
                        double pay = 0;
                        pay = Convert.ToDouble(Convert.ToInt64(e.salary) - e.tax - e.pension - e.medical);
                        using (var db2 = new payrollEntities())
                        {
                            var ps = db2.Database.SqlQuery<purchaseorder>("select * from purchaseorder where id = '" + e.id + "' and `status` = 'open'");
                            foreach (var p in ps)
                            {
                                pay += Convert.ToDouble(p.amount) * Convert.ToDouble(e.commissionedrate);
                                using (var db3 = new payrollEntities())
                                {
                                    db3.Database.ExecuteSqlCommand("update purchaseorder set `status` = 'closed' where pid = '" + p.pid + "'");
                                }
                            }                            
                        }
                        int paya = Convert.ToInt32(pay);
                        using (var db2 = new payrollEntities())
                        {
                            db2.Database.ExecuteSqlCommand("insert into record (id,date,amount,`status`) values ('" + e.id + "','" + now.ToString() + "','" + paya.ToString() + "','no')");//生成工资记录，no表示尚未支付
                        }
                    }
                }

            }


        }

        /// <summary>  
        /// 得到本周第一个工作日(星期一)  
        /// </summary>  
        public static DateTime GetWeekFirstDayMon()
        {
            //星期一为第一天  
            DateTime datetime = DateTime.Now;
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);

            //因为是以星期一为第一天，所以要判断weeknow等于0时，要向前推6天。  
            weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
            int daydiff = (-1) * weeknow;

            //本周第一天  
            string FirstDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(FirstDay);
        }


        /// <summary>  
        /// 得到本周最后一个工作日(星期五)  
        /// </summary>  
        public static DateTime GetWeekLastDayFri()
        {
            DateTime datetime = DateTime.Now;
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);
            weeknow = (weeknow == 0 ? 7 : weeknow);
            int daydiff = (7 - weeknow);

            //本周最后一天  
            string LastDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");//周日
            return Convert.ToDateTime(LastDay).AddDays(-2);//返回周日减去两天——周五
        }

        /// <summary>  
        /// 得到本月最后一个工作日
        /// </summary>  
        public static DateTime GetLastWorkingTime()//the last working day of the month
        {
            DateTime now = DateTime.Now;
            DateTime d1 = new DateTime(now.Year, now.Month, 1);//the first day of the month
            DateTime d2 = d1.AddMonths(1).AddDays(-1);//the last day of the month
            DateTime d = d2;
            while (d >= d1)
            {
                if (d.DayOfWeek.ToString() != "Saturday" && d.DayOfWeek.ToString() != "Sunday")
                    break;
                d = d.AddDays(-1);
            }
            return d;
        }
    }
}

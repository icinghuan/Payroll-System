using System;
using System.Linq;

namespace Payroll_System_Server
{
    class Message
    {
        public static string rec(string str)//客户端发来自定义消息，服务端处理消息（sql查询或执行）生成自定义消息并返回
        {                                   //strList[0]是消息头部，不同的strList[0]对应不同的操作请求
            string[] strList = str.Split(' ');
            string sql = "";

            //预留：检查sql权限及注入

            if (strList[0] == "0")//admin login 管理登录
            {
                sql = "select * from admin where account = '" + strList[1] + "' and pwd = '" + strList[2] + "'";
                using (var db = new payrollEntities())
                {
                    if (db.Database.SqlQuery<admin>(sql).Count<admin>() == 0)
                        return "0";
                    else
                        return "1";
                }
            }
            if (strList[0] == "1")//employee login 员工登录
            {
                sql = "select * from employee where account = '" + strList[1] + "' and pwd = '" + strList[2] + "'";
                using (var db = new payrollEntities())
                {
                    if (db.Database.SqlQuery<employee>(sql).Count<employee>() == 0)
                        return "0";
                    else
                        return db.Database.SqlQuery<employee>(sql).Single().id.ToString();
                }
            }

            string sqlStr = "";
            string id = "";
            if (strList[0] == "2")//create employee 创建员工
            {
                //MessageBox.Show("2 " + account + " " + pwd + " " + name + " " + type + " " + mail + " " +
                //socialnum + " " + numericUpDown3.Value.ToString() + " " + numericUpDown4.Value.ToString()
                //+ " " + numericUpDown5.Value.ToString() + " " + phone + " " + hourlyrate + " " + salary +
                //" " + commissionedrate + " " + numericUpDown6.Value.ToString());
                strList[0] = "";
                sqlStr = "(";
                int c = 0;
                foreach(var i in strList)
                {
                    if (c == 3)
                        sqlStr += "null,";
                    if (i == "null")
                        sqlStr += "null,";
                    else
                        sqlStr += "'"+ i + "',";
                    c++;
                }
                sqlStr = sqlStr.Remove(1, 3);
                sqlStr = sqlStr.Remove(sqlStr.Length - 1, 1) + ")";
                try
                {
                    using (var db = new payrollEntities())
                    {
                        db.Database.ExecuteSqlCommand("insert into employee values " + sqlStr);
                        id = db.Database.SqlQuery<employee>("select * from employee where account = '" + strList[1] + "'").Single().id.ToString();
                        //Console.WriteLine(id);
                        db.Database.ExecuteSqlCommand("insert into perference values ('" + id + "','pickup',null,null,null)");
                        db.Database.ExecuteSqlCommand("insert into vacation values ('" + id + "','10')");
                        return id;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return "0";
                }
            }

            if (strList[0] == "3")//select employee 查询员工
            {
                try
                {
                    using (var db = new payrollEntities())
                    {
                        var emp = db.Database.SqlQuery<employee>("select * from employee where id = '" + strList[1] + "'").Single();
                        //Console.WriteLine(emp.account+" "+emp.pwd + " " + emp.name + " " + emp.type + " " + emp.mail + " " + emp.socialnum +
                        //" " + emp.tax + " " + emp.pension + " " + emp.medical + " " + emp.phone + " " + emp.hourlyrate + " " + emp.salary 
                        //+ " " + emp.commisionedrate + " " + emp.hourlimit);
                        return emp.account + " " + emp.pwd + " " + emp.name + " " + emp.type + " " + emp.mail + " " + emp.socialnum +
                            " " + emp.tax + " " + emp.pension + " " + emp.medical + " " + emp.phone + " " + emp.hourlyrate + " " + 
                            emp.salary + " " + emp.commissionedrate + " " + emp.hourlimit;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return "0";
                }
            }

            if (strList[0] == "4")//update employee 修改员工
            {
                id = strList[1];
                sql = "update employee set account = '" + strList[2] + "', pwd = '" + strList[3] + "', name = '" + strList[4]
                    + "', type =  '" + strList[5] + "', mail = '" + strList[6] + "', socialnum = '" + strList[7] + "', tax = '" + strList[8] + "', pension = '" + strList[9]
                    + "', medical = '" + strList[10] + "', phone = '" + strList[11] + "', hourlyrate = ";
                if (strList[12] == "null")
                    sql += "null" + ", salary = ";
                else
                    sql += "'" + strList[12] + "', salary = ";
                if (strList[13] == "null")
                    sql += "null" + ", commissionedrate = ";
                else
                    sql += "'" + strList[13] + "', commissionedrate = ";
                if (strList[14] == "null")
                    sql += "null" + ", hourlimit = '" + strList[15] + "'";
                else
                    sql += "'" + strList[14] + "', hourlimit = '" + strList[15] + "'";
                sql += " where id = '" + id + "'";
                //Console.WriteLine(sql);
                try
                {
                    using (var db = new payrollEntities())
                    {
                        db.Database.ExecuteSqlCommand(sql);
                        return "1";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return "0";
                }
            }

            if (strList[0] == "5")//delete employee 删除员工
            {
                sql = "insert into waitdelete values ('" + strList[1] + "');";
                //Console.WriteLine(sql);
                try
                {
                    using (var db = new payrollEntities())
                    {
                        db.Database.ExecuteSqlCommand(sql);
                        return "1";
                    }
                }
                catch
                {
                    return "0";
                }
            }

            if (strList[0] == "6")//create order 创建订购单
            {
                //string rec = Client.Rec("6 " + id + " " + contact + " " + address + " " + product + " " + date + " " + amount);
                sql = "insert into purchaseorder values (null,'" + strList[1] + "','" + strList[2] + "','" + strList[3] + "','" + strList[4] + "','" + strList[5] + "','" + strList[6] + "','open')";
                //Console.WriteLine(sql);
                try
                {
                    using (var db = new payrollEntities())
                    {
                        db.Database.ExecuteSqlCommand(sql);
                        return db.Database.SqlQuery<purchaseorder>("select * from purchaseorder where pid = (select max(pid) from purchaseorder)").Single().pid.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return "0";
                }
            }

            if (strList[0] == "7")//select order 查询订购单
            {
                sql = "select * from purchaseorder where pid ='" + strList[1] + "'";
                //Console.WriteLine(sql);
                try
                {
                    using (var db = new payrollEntities())
                    {
                        var i = db.Database.SqlQuery<purchaseorder>(sql).Single();
                        //Console.WriteLine(i.id.ToString() + " " + i.contact + " " + i.address + " " + i.product + " " + i.date.GetDateTimeFormats()[0] + " " + i.amount.ToString() + " " + i.status);
                        return i.id.ToString() + " " + i.contact + " " + i.address + " " + i.product + " " + i.date.GetDateTimeFormats()[0] + " " + i.amount.ToString() + " " + i.status;                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return "0";
                }
            }

            if (strList[0] == "8")//update order 修改订购单
            {
                //string rec = Client.Rec("8 " + ppid + " " + contact + " " + address + " " + product + " " + date + " " + amount);
                sql = "update purchaseorder set contact = '" + strList[2] + "',address = '" + strList[3] + "',product = '" + strList[4] + "',`date` = '" + strList[5] + "',amount = '" + strList[6] + "' where pid = '" + strList[1] + "'";
                //Console.WriteLine(sql);
                try
                {
                    using (var db = new payrollEntities())
                    {
                        db.Database.ExecuteSqlCommand(sql);
                        return "1";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return "0";
                }
            }

            if (strList[0] == "9")//delete purchaseorder 删除订购单
            {
                sql = "delete from purchaseorder where pid = '" + strList[1] + "'";
                try
                {
                    using (var db = new payrollEntities())
                    {
                        db.Database.ExecuteSqlCommand(sql);
                        return "1";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return "0";
                }
            }

            if (strList[0] == "10")//update perference 修改员工个人偏好（支付方式）
            {
                //string rec = Client.Rec("10 " + pid + " " + payment + " " + address + " " + bankaccount + " " + bankname);
                if (strList[2] == "pickup")
                    sql = "update perference set payment = '" + strList[2] + "',address = null,bankaccount = null,bankname = null where id = '" + strList[1] + "'";
                if (strList[2] == "mail")
                    sql = "update perference set payment = '" + strList[2] + "',address = '"+strList[3]+"',bankaccount = null,bankname = null where id = '" + strList[1] + "'";
                if (strList[2] == "deposit")
                    sql = "update perference set payment = '" + strList[2] + "',address = null,bankaccount = '"+strList[4]+"',bankname = '"+strList[5]+"' where id = '" + strList[1] + "'";
                //Console.WriteLine(sql);
                try
                {
                    using (var db = new payrollEntities())
                    {
                        db.Database.ExecuteSqlCommand(sql);
                        return "1";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return "0";
                }
            }

            if (strList[0] == "11")//select timecard 查询考勤表，如果不存在则生成一个新的考勤表，记账编号为0时是所有记账编号对应这个员工在该考勤时间中的总和统计，为了便于检查工作时间是否超时
            {
                DateTime mon = Clock.GetWeekFirstDayMon();
                DateTime fri = Clock.GetWeekLastDayFri();
                //Console.WriteLine(mon);
                //Console.WriteLine(fri);
                sql = "select * from timecard where id = '" + strList[1] + "' and begin = '" + mon + "' and end = '" + fri + "'";
                try
                {
                    using (var db = new payrollEntities())
                    {
                        if (db.Database.SqlQuery<timecard>(sql).Count<timecard>() == 0)
                        {
                            db.Database.ExecuteSqlCommand("insert into timecard (id,begin,end,status) values ('" + strList[1] + "','" + mon + "','" + fri + "','submitted')");                            
                        }
                        string re = mon + " " + fri;
                        using (var db2 = new projectmanagementEntities())
                        {
                            var projects = db2.Database.SqlQuery<project>("select * from project");
                            foreach (var i in projects)
                            {
                                re += " " + i.chargenum.ToString();//返回可用的记账编号
                            }
                        }
                        return re;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return "0";
                }
            }

            if (strList[0] == "12")//select timecard 查询考勤表，根据记账编号查询考勤表
            {
                sql = "select * from timecard where id = '" + strList[1] + "' and begin = '" + strList[3] + "' and end = '" + strList[5] + "' and chargenum = '" + strList[2] + "'";
                try
                {
                    using (var db = new payrollEntities())
                    {
                        if (db.Database.SqlQuery<timecard>(sql).Count<timecard>() == 0)
                        {
                            db.Database.ExecuteSqlCommand("insert into timecard （id,begin,end,chargenum) values ('" + strList[1] + "','" + strList[3] + "','" + strList[5] + "','"+strList[2]+"')");
                        }
                        var i = db.Database.SqlQuery<timecard>(sql).Single();
                        string re = "";
                        if (i.status == "submitted")
                            re = "submitted ";
                        re += i.mon + " " + i.tue + " " + i.wed + " " + i.thu + " " + i.fri + " " + i.tid;
                        return re;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return "0";
                }
            }

            if (strList[0] == "13")//update timecard 保存考勤表信息，保存时还未提交，无需检查工作时间是否超时
            {
                sql = "update timecard set mon = '" + strList[1] + "',tue = '" + strList[2] + "',wed = '" + strList[3] + "',thu = '" + strList[4] + "',fri = '" + strList[5] + "' where tid = '" + strList[6] + "'";
                try
                {
                    using (var db = new payrollEntities())
                    {
                        db.Database.ExecuteSqlCommand(sql);
                        return "1";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return "0";
                }
            }

            if (strList[0] == "14")//update timecard 提交考勤表信息，检查是否超时工作，同时将考勤表状态置为submitted
            {
                try
                {
                    using (var db = new payrollEntities())
                    {
                        var t0 = db.Database.SqlQuery<timecard>("select * from timecard where tid = '" + strList[6] + "'").Single();
                        var e0 = db.Database.SqlQuery<employee>("select * from employee where id = '" + t0.id + "'").Single();
                        int hourlimit = Convert.ToInt32(e0.hourlimit);
                        var i0 = db.Database.SqlQuery<timecard>("select * from timecard where chargenum = '0' and id = '" + t0.id + "' and begin = '" + t0.begin + "' and end = '" + t0.end + "'").Single();
                        long[] a0 = new long[5];
                        a0[0] = i0.mon+long.Parse(strList[1]);
                        a0[1] = i0.tue + long.Parse(strList[2]);
                        a0[2] = i0.wed + long.Parse(strList[3]);
                        a0[3] = i0.thu + long.Parse(strList[4]);
                        a0[4] = i0.fri + long.Parse(strList[5]);
                        long atime = a0[0] + a0[1] + a0[2] + a0[3] + a0[4];
                        long time = long.Parse(strList[1])+ long.Parse(strList[2])+ long.Parse(strList[3])+ long.Parse(strList[4])+ long.Parse(strList[5]);
                        for (int i = 0; i < 5; ++i)
                            if (a0[i] > hourlimit)
                                return (i+1).ToString();
                        DateTime now = DateTime.Now;
                        sql = "update timecard set status = 'submitted',time = '"+time+"',subtime = '" + now.ToString() + "',mon = '" + strList[1] + "',tue = '" + strList[2] + "',wed = '" + strList[3] + "',thu = '" + strList[4] + "',fri = '" + strList[5] + "' where tid = '" + strList[6] + "'";
                        db.Database.ExecuteSqlCommand(sql);
                        sql = "update timecard set time = '"+atime+"',mon = '" + a0[0] + "',tue = '" + a0[1] + "',wed = '" + a0[2] + "',thu = '" + a0[3] + "',fri = '" + a0[4] + "' where chargenum = '0' and id = '" + t0.id + "' and begin = '" + t0.begin + "' and end = '" + t0.end + "'";
                        db.Database.ExecuteSqlCommand(sql);
                        return "ok";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return "0";
                }
            }

            if (strList[0] == "15")//select chargenum 查询可用的记账编号
            {
                try
                {
                    using (var db2 = new projectmanagementEntities())
                    {
                        var projects = db2.Database.SqlQuery<project>("select * from project");
                        string re = "ok";
                        foreach (var i in projects)
                        {
                            re += " " + i.chargenum.ToString();//返回可用的记账编号
                        }
                        return re;
                    }
                }
                catch
                {
                    return "0";
                }
            }

            if (strList[0] == "16")//生成报表 总工作时间
            {
                //string str = "16 "+id+" "+begin.Date+" "+end.Date;
                DateTime begin = Convert.ToDateTime(strList[2]);
                DateTime end = Convert.ToDateTime(strList[4]);
                try
                {
                    using (var db = new payrollEntities())
                    {
                        var timecards = db.Database.SqlQuery<timecard>("select * from timecard where id = '" + strList[1] + "' and chargenum = '0'");
                        long ans = 0;               
                        foreach(var i in timecards)
                        {
                            if (i.begin > end)
                                continue;
                            if (i.end < begin)
                                continue;
                            DateTime j = begin;
                            //Console.WriteLine(i.begin);
                            while (j <= end)
                            {
                                if (i.begin <= j && j <= i.end)//i.begin j i.end
                                {
                                    if (j.DayOfWeek.ToString() == "Monday")
                                        ans += i.mon;
                                    if (j.DayOfWeek.ToString() == "Tuesday")
                                        ans += i.tue;
                                    if (j.DayOfWeek.ToString() == "Wednesday")
                                        ans += i.wed;
                                    if (j.DayOfWeek.ToString() == "Thursday")
                                        ans += i.thu;
                                    if (j.DayOfWeek.ToString() == "Friday")
                                        ans += i.fri;
                                    //Console.WriteLine(j);
                                }
                                j = j.AddDays(1);
                            }
                        }                        
                        return ans.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return "fail";
                }
            }

            if (strList[0] == "17")//生成报表 项目总工作时间
            {
                DateTime begin = Convert.ToDateTime(strList[2]);
                DateTime end = Convert.ToDateTime(strList[4]);
                try
                {
                    using (var db = new payrollEntities())
                    {
                        var timecards = db.Database.SqlQuery<timecard>("select * from timecard where id = '" + strList[1] + "' and chargenum = '"+strList[6]+"'");
                        long ans = 0;
                        foreach (var i in timecards)
                        {
                            if (i.begin > end)
                                continue;
                            if (i.end < begin)
                                continue;
                            DateTime j = begin;
                            while (j <= end)
                            {
                                if (i.begin <= j && j <= i.end)//i.begin j i.end
                                {
                                    if (j.DayOfWeek.ToString() == "Monday")
                                        ans += i.mon;
                                    if (j.DayOfWeek.ToString() == "Tuesday")
                                        ans += i.tue;
                                    if (j.DayOfWeek.ToString() == "Wednesday")
                                        ans += i.wed;
                                    if (j.DayOfWeek.ToString() == "Thursday")
                                        ans += i.thu;
                                    if (j.DayOfWeek.ToString() == "Friday")
                                        ans += i.fri;
                                    //Console.WriteLine(j);
                                }
                                j = j.AddDays(1);
                            }
                        }
                        return ans.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return "fail";
                }
            }

            if (strList[0] == "18")//生成报表 剩余假期
            {
                sql = "select * from vacation where id = '" + strList[1] + "'";
                try
                {
                    using (var db = new payrollEntities())
                    {
                        var i = db.Database.SqlQuery<vacation>(sql).Single();
                        return i.ramain.ToString();//本来应该是remain的，打错了= =
                    }
                }
                catch
                {
                    return "fail";
                }
            }

            if (strList[0] == "19")//生成报表 总工资
            {
                //string str = "19 " + id + " " + begin.Date + " " + end.Date;
                DateTime begin = Convert.ToDateTime(strList[2]);
                DateTime end = Convert.ToDateTime(strList[4]);
                try
                {
                    using (var db = new payrollEntities())
                    {
                        var records = db.Database.SqlQuery<record>("select * from record where id = '" + strList[1] + "'");
                        long ans = 0;
                        foreach (var i in records)
                        {
                            if (i.date > end)
                                continue;
                            if (i.date < begin)
                                continue;
                            if (i.date <= end && i.date >= begin)//begin i.date end
                                ans += i.amount;
                        }
                        return ans.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return "fail";
                }
            }

            return "0";
        }
    }
}

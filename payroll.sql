/*
SQLyog 企业版 - MySQL GUI v8.14 
MySQL - 5.0.18-nt : Database - payroll
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`payroll` /*!40100 DEFAULT CHARACTER SET latin1 */;

USE `payroll`;

/*Table structure for table `admin` */

DROP TABLE IF EXISTS `admin`;

CREATE TABLE `admin` (
  `account` varchar(24) NOT NULL COMMENT '账户',
  `pwd` varchar(24) NOT NULL COMMENT '密码',
  `id` int(10) unsigned NOT NULL auto_increment COMMENT '管理员编号',
  PRIMARY KEY  (`account`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*Data for the table `admin` */

insert  into `admin`(`account`,`pwd`,`id`) values ('admin','admin',1);

/*Table structure for table `employee` */

DROP TABLE IF EXISTS `employee`;

CREATE TABLE `employee` (
  `account` varchar(24) NOT NULL COMMENT '账户',
  `pwd` varchar(24) NOT NULL COMMENT '密码',
  `id` int(10) unsigned NOT NULL auto_increment COMMENT '员工编号',
  `name` varchar(12) NOT NULL COMMENT '姓名',
  `type` varchar(12) NOT NULL COMMENT '员工类型（小时工hour，月薪salaried，提成commissioned）',
  `mail` varchar(24) NOT NULL COMMENT '邮箱地址',
  `socialnum` varchar(24) NOT NULL COMMENT '社会保险号码',
  `tax` int(11) unsigned NOT NULL COMMENT '标准税收',
  `pension` int(11) unsigned NOT NULL COMMENT '养老金',
  `medical` int(11) unsigned NOT NULL COMMENT '医疗保险',
  `phone` varchar(24) NOT NULL COMMENT '电话号码',
  `hourlyrate` int(11) unsigned default NULL COMMENT '计时工资（小时工）',
  `salary` int(11) unsigned default NULL COMMENT '月薪（月薪和提成）',
  `commissionedrate` decimal(3,2) default NULL COMMENT '提成比例（提成）',
  `hourlimit` int(10) unsigned NOT NULL COMMENT '工作时长限制',
  PRIMARY KEY  (`account`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*Data for the table `employee` */

insert  into `employee`(`account`,`pwd`,`id`,`name`,`type`,`mail`,`socialnum`,`tax`,`pension`,`medical`,`phone`,`hourlyrate`,`salary`,`commissionedrate`,`hourlimit`) values ('emp1','123',1,'employee1','hour','emp1@acme.com','10001',0,0,0,'10001',50,NULL,NULL,12),('emp2','123',2,'employee2','salaried','emp2@acme.com','10002',500,200,300,'10002',NULL,8000,NULL,8),('emp3','123',3,'employee3','commissioned','emp3@acme.com','10003',500,200,300,'10003',NULL,8000,'0.25',10),('emp4','123',4,'employee4','hour','emp4@acme.com','10004',0,0,0,'10004',40,NULL,NULL,16);

/*Table structure for table `perference` */

DROP TABLE IF EXISTS `perference`;

CREATE TABLE `perference` (
  `id` int(11) NOT NULL COMMENT '员工编号',
  `payment` varchar(10) NOT NULL default 'pickup' COMMENT '付款方式（自取pickup，邮寄mail，直存deposit）',
  `address` varchar(64) default NULL COMMENT '邮寄地址',
  `bankaccount` varchar(24) default NULL COMMENT '银行账户',
  `bankname` varchar(24) default NULL COMMENT '账户姓名',
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*Data for the table `perference` */

insert  into `perference`(`id`,`payment`,`address`,`bankaccount`,`bankname`) values (1,'pickup','road123',NULL,NULL),(2,'mail','road',NULL,NULL),(3,'deposit',NULL,'1231231','jack'),(4,'pickup',NULL,NULL,NULL);

/*Table structure for table `purchaseorder` */

DROP TABLE IF EXISTS `purchaseorder`;

CREATE TABLE `purchaseorder` (
  `pid` int(10) unsigned NOT NULL auto_increment COMMENT '订单编号',
  `id` int(10) unsigned NOT NULL COMMENT '员工编号',
  `contact` varchar(24) NOT NULL COMMENT '客户接触点',
  `address` varchar(24) NOT NULL COMMENT '客户帐单地址',
  `product` varchar(24) NOT NULL COMMENT '购买的产品',
  `date` date NOT NULL COMMENT '订单日期',
  `amount` int(11) unsigned NOT NULL COMMENT '销售数额',
  `status` varchar(6) default 'open' COMMENT '订单状态（未完成为open，完成为closed）',
  PRIMARY KEY  (`pid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*Data for the table `purchaseorder` */

insert  into `purchaseorder`(`pid`,`id`,`contact`,`address`,`product`,`date`,`amount`,`status`) values (1,3,'asd','asd','asd','2017-06-11',20000,'closed');

/*Table structure for table `record` */

DROP TABLE IF EXISTS `record`;

CREATE TABLE `record` (
  `pid` int(11) unsigned NOT NULL auto_increment COMMENT '支付编号',
  `id` int(11) NOT NULL COMMENT '员工编号',
  `date` date NOT NULL COMMENT '支付日期',
  `amount` int(11) NOT NULL COMMENT '支付数额',
  `status` varchar(4) NOT NULL default 'no' COMMENT '支付状态（未支付为no，已支付为yes）',
  PRIMARY KEY  (`pid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 CHECKSUM=1 DELAY_KEY_WRITE=1 ROW_FORMAT=DYNAMIC;

/*Data for the table `record` */

insert  into `record`(`pid`,`id`,`date`,`amount`,`status`) values (1,1,'2017-06-11',1000,'yes'),(2,2,'2017-06-11',7000,'yes'),(3,3,'2017-06-11',12000,'yes');

/*Table structure for table `timecard` */

DROP TABLE IF EXISTS `timecard`;

CREATE TABLE `timecard` (
  `tid` int(10) unsigned NOT NULL auto_increment COMMENT '考勤编号',
  `id` int(10) unsigned NOT NULL COMMENT '员工编号',
  `begin` date NOT NULL COMMENT '开始时间',
  `end` date NOT NULL COMMENT '结束时间',
  `subtime` datetime default NULL COMMENT '提交时间',
  `mon` int(11) unsigned NOT NULL default '0' COMMENT '星期一工作时间',
  `tue` int(11) unsigned NOT NULL default '0' COMMENT '星期五工作时间',
  `wed` int(11) unsigned NOT NULL default '0' COMMENT '星期三工作时间',
  `thu` int(11) unsigned NOT NULL default '0' COMMENT '星期四工作时间',
  `fri` int(11) unsigned NOT NULL default '0' COMMENT '星期五工作时间',
  `time` int(11) unsigned NOT NULL default '0' COMMENT '工作时长',
  `chargenum` int(11) default '0' COMMENT '记账编号（记账编号为0位其他编号之和的统计）',
  `status` varchar(12) NOT NULL default 'no' COMMENT '提交状态（未提交为no，已提交为submitted）',
  PRIMARY KEY  (`tid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 CHECKSUM=1 DELAY_KEY_WRITE=1 ROW_FORMAT=DYNAMIC;

/*Data for the table `timecard` */

insert  into `timecard`(`tid`,`id`,`begin`,`end`,`subtime`,`mon`,`tue`,`wed`,`thu`,`fri`,`time`,`chargenum`,`status`) values (1,1,'2017-06-05','2017-06-09',NULL,2,3,4,5,6,20,0,'submitted'),(2,1,'2017-06-05','2017-06-09','2017-06-11 13:31:39',1,2,3,4,5,15,1,'submitted'),(3,1,'2017-06-05','2017-06-09','2017-06-11 13:32:19',1,1,1,1,1,5,2,'submitted'),(4,2,'2017-06-05','2017-06-09',NULL,5,8,5,2,1,21,0,'submitted'),(5,2,'2017-06-05','2017-06-09','2017-06-11 16:30:57',0,0,0,0,0,0,1,'submitted'),(6,2,'2017-06-05','2017-06-09','2017-06-11 16:35:20',5,8,5,2,1,21,2,'submitted'),(7,3,'2017-06-05','2017-06-09',NULL,0,0,0,0,0,0,0,'submitted');

/*Table structure for table `vacation` */

DROP TABLE IF EXISTS `vacation`;

CREATE TABLE `vacation` (
  `id` int(10) unsigned NOT NULL COMMENT '员工编号',
  `ramain` int(10) unsigned default '0' COMMENT '剩余假期',
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*Data for the table `vacation` */

insert  into `vacation`(`id`,`ramain`) values (1,10),(2,10),(3,10),(4,10);

/*Table structure for table `waitdelete` */

DROP TABLE IF EXISTS `waitdelete`;

CREATE TABLE `waitdelete` (
  `id` int(10) unsigned NOT NULL COMMENT '员工编号',
  PRIMARY KEY  (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*Data for the table `waitdelete` */

insert  into `waitdelete`(`id`) values (4);

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

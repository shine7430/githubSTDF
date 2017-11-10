using Renci.SshNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFTP
{
    class Program
    {
        const int port = 22; //端口
        const string hostIntel = "ift.intel.com";
        const string usernameIntel = "jxu16"; //用户名
        //readonly string passwordIntel = ConfigurationManager.AppSettings["passwordIntel"].ToString(); //"Shi$197712";密码
        const string IntelFT = @"/Samar14/Manufacturing/FinalTest/STDF";
        const string IntelCP = @"/Samar14/Manufacturing/Sort/STDF/";
        const string IntelWAT = @"/Samar14/Manufacturing/WAT/";


        const string hostSPRD = "ftp.spreadtrum.com";
        const string usernameSPRD = "iTest_Intel"; //用户名
        const string passwordSPRD = "1dCx42tX";//密码
        const string SPRDFT = @"/FT/PRODUCTION/TESTER/";
        const string SPRDCP = @"/WS/PRODUCTION/TESTER/";
        const string SPRDWAT = @"/WAT/PRODUCTION/TESTER/";

        const string RouterFTLocalPath = "C:\\SFTP\\Intel\\FT\\";
        const string RouterCPLocalPath = "C:\\SFTP\\Intel\\CP\\";
        const string RouterWATLocalPath = "C:\\SFTP\\Intel\\WAT\\";

        //@"/FT/DEBUG";//读取、上传文件的目录 "/"为根目录
        //const string uploadfile = @"c:\1.xml"; //上传文件地址
        public static int iCount = 0;
        public static int MaxCount = 10;//允许线程池中运行最多10个线程
        static ManualResetEvent eventX = new ManualResetEvent(false);  //新建ManualResetEvent对象并且初始化为无信号状态

        static void Main(string[] args)
        {
            //Console.WriteLine(args[0].ToString());
            //Console.ReadKey();
            //return;
            string passwordIntel = ConfigurationManager.AppSettings["passwordIntel"].ToString();
            SFTPHelp sftpo = new SFTPHelp(hostIntel, "22", usernameIntel, passwordIntel);
            SFTPHelp sftpoSPRD = new SFTPHelp(hostSPRD, "22", usernameSPRD, passwordSPRD);

            //goto gototag;
            #region FT

            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "----------------------------Begin Move FT File");
            Log.Info("", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "---------------------------Begin Move FT File");
            ArrayList ALFT = sftpo.GetFileList(IntelFT);
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "--FT Files count:" + ALFT.Count);
            Log.Info("", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "--FT Files count:" + ALFT.Count);
            //return;
            foreach (string filename in ALFT)
            {
                try
                {
                    //copy file from intel sftp to local
                    sftpo.Get(IntelFT + "\\" + filename, RouterFTLocalPath + filename);

                    //move file from local to spreadtrum sftp
                    sftpoSPRD.Put(RouterFTLocalPath + filename, SPRDFT + filename);

                    //delete file from intel sftp
                    sftpo.Delete(IntelFT + "\\" + filename);

                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-IntelFT-" + filename + "-Move Successful");
                    Log.Info(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-IntelFT--" + filename, "IntelFT--" + filename + "-Move Successful");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-IntelFT-" + filename + "-Move Failed");
                    Log.Error("IntelFT--" + filename, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "--" + ex.Message);
                }

            //    Thread t = new Thread(() => CopyFile(IntelFT, RouterFTLocalPath, SPRDFT, filename, "IntelFT"));

            //    t.IsBackground = true;        //設置為後臺線程,程式關閉后進程也關閉,如果不設置true，則程式關閉,此線程還在內存,不會關閉

            //    t.Start();  // 在新线程中运行Go()
            }
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "---------------------------End Move FT File");
            Log.Info("", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "---------------------------End Move FT File");
            #endregion

            #region CP
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "---------------------------Begin Move CP File");
            Log.Info("", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "---------------------------Begin Move CP File");
            ArrayList ALCP = sftpo.GetFileList(IntelCP);
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "--CP Files count:" + ALCP.Count);
            Log.Info("", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "--CP Files count:" + ALCP.Count);

            foreach (string filename in ALCP)
            {
                try
                {
                    //copy file from intel sftp to local
                    sftpo.Get(IntelCP + "\\" + filename, RouterCPLocalPath + filename);

                    //move file from local to spreadtrum sftp
                    sftpoSPRD.Put(RouterCPLocalPath + filename, SPRDCP + filename);

                    //delete file from intel sftp
                    sftpo.Delete(IntelCP + "\\" + filename);

                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-IntelCP-" + filename + "-Move Successful");
                    Log.Info(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-IntelCP--" + filename, "IntelCP--" + filename + "-Move Successful");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-IntelCP-" + filename + "-Move Failed");
                    Log.Error("IntelCP--" + filename, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "--" + ex.Message);
                }

                //Thread t = new Thread(() => CopyFile(IntelCP, RouterCPLocalPath, SPRDCP, filename, "IntelCP"));

                //t.IsBackground = true;        //設置為後臺線程,程式關閉后進程也關閉,如果不設置true，則程式關閉,此線程還在內存,不會關閉

                //t.Start();  // 在新线程中运行Go()

            }
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "---------------------------End Move CP File");
            Log.Info("", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "---------------------------End Move CP File");
            #endregion
     
            #region WAT

            //gototag:
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "---------------------------Begin Move WAT File");
            Log.Info("", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "---------------------------Begin Move WAT File");
            ArrayList ALWAT = sftpo.GetFileList(IntelWAT);
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "--WAT Files count:" + ALWAT.Count);
            Log.Info("", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-- WATFiles count:" + ALWAT.Count);

            foreach (string filename in ALWAT)
            {
                try
                {
                    //copy file from intel sftp to local
                    sftpo.Get(IntelWAT + "\\" + filename, RouterWATLocalPath + filename);

                    //move file from local to spreadtrum sftp
                    //sftpoSPRD.Put(RouterWATLocalPath + filename, SPRDWAT + filename);

                    //delete file from intel sftp
                    //sftpo.Delete(IntelWAT + "\\" + filename);

                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-IntelWAT-" + filename + "-Move Successful");
                    Log.Info(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-IntelWAT--" + filename, "IntelWAT--" + filename + "-Move Successful");

                    //break;
                    //Thread t = new Thread(() => CopyFile(IntelWAT, RouterWATLocalPath, SPRDWAT, filename, "IntelWAT"));
                    //t.IsBackground = true;        //設置為後臺線程,程式關閉后進程也關閉,如果不設置true，則程式關閉,此線程還在內存,不會關閉
                    //t.Start();  // 在新线程中运行Go()
                }
                catch (Exception ex)
                {
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-IntelWAT-" + filename + "-Move Failed");
                    Log.Error("IntelWAT--" + filename, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "--" + ex.Message);
                }
            }
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "---------------------------End Move WAT File");
            Log.Info("", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "---------------------------End Move WAT File");
            #endregion

            //sftpo.Put(@"D:\Plan1.xlsx", workingdirectory + "/Plan1.xlsx");
            //sftpo.Delete(workingdirectory + "/Plan1.xlsx");

            //Console.WriteLine("等待线程池完成操作。。。"); //等待事件的完成，即线程调用ManualResetEvent.Set()方法
            //eventX.WaitOne(Timeout.Infinite, true); //WaitOne()方法使调用它的线程等待直到eventX.Set()方法被调用
            //Console.WriteLine("线程池结束！");
            sftpo.Disconnect();
            sftpoSPRD.Disconnect();
            //Console.ReadKey();
        }

        static void CopyFile(string IntelPath, string RouterLocalPath, string SPRDPath, string filename, string ClassName)
        {
            string passwordIntel = ConfigurationManager.AppSettings["passwordIntel"].ToString();
            SFTPHelp sftpo = new SFTPHelp(hostIntel, "22", usernameIntel, passwordIntel);
            SFTPHelp sftpoSPRD = new SFTPHelp(hostSPRD, "22", usernameSPRD, passwordSPRD);

            try
            {

                //copy file from intel sftp to local
                sftpo.Get(IntelPath + "\\" + filename, RouterLocalPath + filename);

                //move file from local to spreadtrum sftp
                sftpoSPRD.Put(RouterLocalPath + filename, SPRDPath + filename);

                //delete file from intel sftp
                sftpo.Delete(IntelPath + "\\" + filename);
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-" + ClassName + "-" + filename + "-Move Successful");
                Log.Info(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-" + ClassName + "--" + filename, ClassName + "--" + filename + "-Move Successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-" + ClassName + "-" + filename + "-Move Failed");
                Log.Error(ClassName+"--" + filename, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "--" + ex.Message);
            }
            finally
            {
                sftpo.Disconnect();
                sftpoSPRD.Disconnect();
                Interlocked.Increment(ref iCount);
                if (iCount == MaxCount)
                {
                    Console.WriteLine();
                    Console.WriteLine("设置ManualResetEvent为有信号状态，Setting eventX ");
                    eventX.Set();
                }
            }


        }
    }
}

using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        const int port = 22; //端口
        const string host = "ftp.spreadtrum.com"; //sftp地址	

        const string username = "iTest_Intel"; //用户名
        const string password = "1dCx42tX";//密码
        const string workingdirectory = "/";//读取、上传文件的目录 "/"为根目录
        const string uploadfile = @"c:\1.xml"; //上传文件地址

        protected void Page_Load(object sender, EventArgs e)
        {
            using (var client = new SftpClient(host, port, username, password)) //创建连接对象
            {
                client.Connect(); //连接


                client.ChangeDirectory(workingdirectory); //切换目录


                var listDirectory = client.ListDirectory(workingdirectory); //获取目录下所有文件

                foreach (var fi in listDirectory) //遍历文件
                {
                    Console.WriteLine(" - " + fi.Name);
                    // client.DeleteFile(fi.FullName);//删除文件
                }

                //using (var fileStream = new FileStream(uploadfile, FileMode.Open))
                //{
                //    //client.BufferSize = 4 * 1024; // bypass Payload error large
                //    //client.UploadFile(fileStream, Path.GetFileName(uploadfile)); //上传文件
                //    //UploadFile方法没有返回值，无法判断文件是否上传成功，我想到的解决办法是，上传后再获取一下文件列表，如果文件列表count比上传之前大，说明上传成功。当然
                //    //这样的前提是只有你一个人上传。不知各位大神有没有其它办法
                //}
            }
        }
    }
}
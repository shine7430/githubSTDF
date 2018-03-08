using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections;
using System.IO;
using Renci.SshNet;

namespace SFTP
{

    /************************描述 SFTP操作类******************************************
    **创建者  : aaa
    **创建时间: 2015-03-11
    **描述	: SFTP操作类
    *********************************************************************************/

    /// <summary>
    /// SFTP操作类
    /// </summary>
    public class SFTPHelp
    {
        #region 字段或属性
        private SftpClient sftp;
        /// <summary>
        /// SFTP连接状态
        /// </summary>
        public bool Connected { get { return sftp.IsConnected; } }
        #endregion

        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">端口</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        public SFTPHelp(string ip, string port, string user, string pwd)
        {
            sftp = new SftpClient(ip, Int32.Parse(port), user, pwd);
        }
        #endregion

        #region 连接SFTP
        /// <summary>
        /// 连接SFTP
        /// </summary>
        /// <returns>true成功</returns>
        public bool Connect()
        {
            try
            {
                if (!Connected)
                {
                    sftp.Connect();
                }
                return true;
            }
            catch (Exception ex)
            {
                //TxtLog.WriteTxt(CommonMethod.GetProgramName(), string.Format("连接SFTP失败，原因：{0}", ex.Message));
                throw new Exception(string.Format("连接SFTP失败，原因：{0}", ex.Message));
            }
        }
        #endregion

        #region 断开SFTP
        /// <summary>
        /// 断开SFTP
        /// </summary> 
        public void Disconnect()
        {
            try
            {
                if (sftp != null && Connected)
                {
                    sftp.Disconnect();
                }
            }
            catch (Exception ex)
            {
                //TxtLog.WriteTxt(CommonMethod.GetProgramName(), string.Format("断开SFTP失败，原因：{0}", ex.Message));
                throw new Exception(string.Format("断开SFTP失败，原因：{0}", ex.Message));
            }
        }
        #endregion

        #region SFTP上传文件
        /// <summary>
        /// SFTP上传文件
        /// </summary>
        /// <param name="localPath">本地路径</param>
        /// <param name="remotePath">远程路径</param>
        public bool Put(string localPath, string remotePath)
        {
            bool result = false;
            try
            {
                using (var file = File.OpenRead(localPath))
                {
                    Connect();
                    sftp.UploadFile(file, remotePath, true);
                    //Disconnect();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                //TxtLog.WriteTxt(CommonMethod.GetProgramName(), string.Format("SFTP文件上传失败，原因：{0}", ex.Message));
                throw new Exception(string.Format("SFTP文件上传失败，原因：{0}", ex.Message));
            }
            return result;
        }

        public bool Put(byte[] byt, string remotePath)
        {
            bool result = false;
            try
            {
                
                using (Stream stream = new MemoryStream(byt))
                {
                    Connect();
                    sftp.UploadFile(stream, remotePath, true);
                    //Disconnect();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                //TxtLog.WriteTxt(CommonMethod.GetProgramName(), string.Format("SFTP文件上传失败，原因：{0}", ex.Message));
                throw new Exception(string.Format("SFTP文件上传失败，原因：{0}", ex.Message));
            }
            return result;
        }

        #endregion

        #region SFTP获取文件
        /// <summary>
        /// SFTP获取文件
        /// </summary>
        /// <param name="remotePath">远程路径</param>
        /// <param name="localPath">本地路径</param>
        public byte[] Get(string remotePath)
        {
            try
            {
                Connect();
                var byt = sftp.ReadAllBytes(remotePath);
                Console.WriteLine((byt.Length / 1024.00 / 1024.00).ToString("F2") + "M");
                Log.Info("Size:", (byt.Length / 1024.00 / 1024.00).ToString("F2") + "M");
                return byt;
            }
            catch (Exception ex)
            {
                //TxtLog.WriteTxt(CommonMethod.GetProgramName(), string.Format("SFTP文件获取失败，原因：{0}", ex.Message));
                throw new Exception(string.Format("SFTP文件获取失败，原因：{0}", ex.Message));
            }
            return null;

        }

        public bool Get(string remotePath, string localPath)
        {
            bool result = false;
            try
            {
                Connect();
                var byt = sftp.ReadAllBytes(remotePath);
                Console.WriteLine((byt.Length / 1024.00 / 1024.00).ToString("F2") + "M");
                Log.Info("Size:", (byt.Length / 1024.00 / 1024.00).ToString("F2") + "M");
                //Disconnect();
                if (!File.Exists(localPath))
                {
                    File.WriteAllBytes(localPath, byt);
                }
                else
                {
                    File.Delete(localPath);
                    File.WriteAllBytes(localPath, byt);
                }

                result = true;
            }
            catch (Exception ex)
            {
                //TxtLog.WriteTxt(CommonMethod.GetProgramName(), string.Format("SFTP文件获取失败，原因：{0}", ex.Message));
                throw new Exception(string.Format("SFTP文件获取失败，原因：{0}", ex.Message));
            }
            return result;

        }
        #endregion

        #region 删除SFTP文件
        /// <summary>
        /// 删除SFTP文件 
        /// </summary>
        /// <param name="remoteFile">远程路径</param>
        public void Delete(string remoteFile)
        {
            try
            {
                Connect();
                sftp.Delete(remoteFile);
                //Disconnect();
            }
            catch (Exception ex)
            {
                //TxtLog.WriteTxt(CommonMethod.GetProgramName(), string.Format("SFTP文件删除失败，原因：{0}", ex.Message));
                throw new Exception(string.Format("SFTP文件删除失败，原因：{0}", ex.Message));
            }
        }
        #endregion

        #region 获取SFTP文件列表
        /// <summary>
        /// 获取SFTP文件列表
        /// </summary>
        /// <param name="remotePath">远程目录</param>
        /// <param name="fileSuffix">文件后缀</param>
        /// <returns></returns>
        public ArrayList GetFileList(string remotePath, string fileSuffix)
        {
            try
            {
                Connect();
                var files = sftp.ListDirectory(remotePath);
                //Disconnect();
                var objList = new ArrayList();
                foreach (var file in files)
                {
                    string name = file.Name;
                    if (name.Length > (fileSuffix.Length + 1) && fileSuffix == name.Substring(name.Length - fileSuffix.Length))
                    {
                        objList.Add(name);
                    }
                }
                return objList;
            }
            catch (Exception ex)
            {
                //TxtLog.WriteTxt(CommonMethod.GetProgramName(), string.Format("SFTP文件列表获取失败，原因：{0}", ex.Message));
                throw new Exception(string.Format("SFTP文件列表获取失败，原因：{0}", ex.Message));
            }
        }

        public ArrayList GetFileList(string remotePath)
        {
            try
            {
                Connect();
                var files = sftp.ListDirectory(remotePath);
                //Disconnect();
                var objList = new ArrayList();
                foreach (var file in files)
                {
                    string name = file.Name;
                    objList.Add(name);
                }
                
                return objList;
            }
            catch (Exception ex)
            {
                //TxtLog.WriteTxt(CommonMethod.GetProgramName(), string.Format("SFTP文件列表获取失败，原因：{0}", ex.Message));
                throw new Exception(string.Format("SFTP文件列表获取失败，原因：{0}", ex.Message));
            }
        }
        #endregion

        #region 移动SFTP文件
        /// <summary>
        /// 移动SFTP文件
        /// </summary>
        /// <param name="oldRemotePath">旧远程路径</param>
        /// <param name="newRemotePath">新远程路径</param>
        public void Move(string oldRemotePath, string newRemotePath)
        {
            try
            {
                Connect();
                sftp.RenameFile(oldRemotePath, newRemotePath);
                //Disconnect();
            }
            catch (Exception ex)
            {
                //TxtLog.WriteTxt(CommonMethod.GetProgramName(), string.Format("SFTP文件移动失败，原因：{0}", ex.Message));
                throw new Exception(string.Format("SFTP文件移动失败，原因：{0}", ex.Message));
            }
        }
        #endregion

    }

}




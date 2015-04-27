using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace guiziTCPfangyuan
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
 
            // 绑定异常处理
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            Application.Run(new Form1());
        }
        /// <summary>
        /// 应用程序界面错误处理
        /// </summary>
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            try
            {
                string errorMsg = "应用程序发生界面类致命错误，即将关闭。错误信息如下：\r\n " + e.Exception.Message;
                //log.Fatal(errorMsg, e.Exception);
                MessageBox.Show(errorMsg, "致命错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch
            {
                MessageBox.Show("应用程序发生界面类致命错误，即将关闭。且无法记录错误日志。错误信息如下：\r\n "
                    + e.Exception.Message, "致命错误", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
            }
            finally
            {
                Application.Exit();
            }
        }
        /// <summary>
        /// 线程错误处理
        /// </summary>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;
                string errorMsg = "应用程序发生致命错误，即将关闭。错误信息如下：\r\n " + ex.Message;

                //log.Fatal(errorMsg, ex);
                MessageBox.Show(errorMsg, "致命错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception exc)
            {
                MessageBox.Show("应用程序发生致命错误，即将关闭。且无法记录错误日志，错误信息如下："
                    + exc.Message, "致命错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                Application.Exit();
            }
        }
    }
}

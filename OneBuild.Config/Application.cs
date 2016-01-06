namespace OneBuild.Config
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using OneBuild.Logger;
    using System.Xml.Linq;
    using System.IO;

    public static class Application
    {
        private static readonly Logger mLog = Logger.Instance(typeof(Application));

        private static string jobDir;
        public static string JobDir { get { return jobDir; } }

        private static string planDir;
        public static string PlanDir { get { return planDir; } }

        private static string tempDir;
        public static string TempDir { get { return tempDir; } }

        private static string reportDir;
        public static string ReportDir { get { return reportDir; } }

        //private static string logDir;
        //public static string LogDir { get { return logDir; } }

        public static void Initialize(string initfile = null)
        {
            if (String.IsNullOrEmpty(initfile))
            {
                initfile = ConfigurationManager.AppSettings.Get("initfile");
            }
            mLog.Info($"初始化Application环境,配置文件名\"{initfile}\"");
            if (String.IsNullOrEmpty(initfile))
            {
                throw new Exception("初始化文件名不能为空,请检查文件配置项.");
            }
            if (!File.Exists(initfile))
            {
                throw new FileNotFoundException($"初始化文件\"{initfile}\"不存在,请检查该文件是否存在.");
            }
            try
            {
                XElement root = XElement.Load(initfile);
                jobDir = root.Element("job").Element("dir").Value;
                if (!Directory.Exists(jobDir))
                {
                    Directory.CreateDirectory(jobDir);
                }
                mLog.Info($"初始化JobDir为{JobDir}.");
                planDir = Path.Combine(jobDir, root.Element("job").Element("plan").Value);
                if (!Directory.Exists(planDir))
                {
                    Directory.CreateDirectory(planDir);
                }
                mLog.Info($"初始化PlanDir为{PlanDir}.");
                tempDir = Path.Combine(jobDir, root.Element("job").Element("temp").Value);
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }
                mLog.Info($"初始化PlanDir为{TempDir}.");
                reportDir = Path.Combine(jobDir, root.Element("job").Element("report").Value);
                if (!Directory.Exists(reportDir))
                {
                    Directory.CreateDirectory(reportDir);
                }
                mLog.Info($"初始化PlanDir为{ReportDir}.");
                //logDir = Path.Combine(jobDir, root.Element("job").Element("report").Value);
                //if (!Directory.Exists(reportDir))
                //{
                //    Directory.CreateDirectory(reportDir);
                //}
                //mLog.Info($"初始化PlanDir为{ReportDir}.");
            }
            catch (Exception e)
            {
                mLog.Error(e.ToString());
                throw e;
            }
        }
    }
}

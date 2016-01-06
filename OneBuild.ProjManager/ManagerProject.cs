namespace OneBuild.ProjManager
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ManagerProject
    {
        private static ManagerProject manager;
        private static readonly object syncRoot = new object();
        private List<Project> projects = new List<Project>();

        private ManagerProject()
        {

        }

        public ManagerProject GetInstance()
        {
            if (manager == null)
            {
                lock (syncRoot)
                {
                    if (manager == null)
                    {
                        manager = new ManagerProject();
                    }
                }
            }
            return manager;
        }

        public void NewProject(string proj)
        {
            JObject jsonObject = JObject.Parse(proj);
            string step = jsonObject["step"].ToString();
            if(step == "1")
            {
                Project project = new Project();
                project.Name = jsonObject["proj-name"].ToString();
            }
        }
    }
}

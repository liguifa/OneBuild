namespace OneBuild.ProjManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Project
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string Name { get; set; }
    }
}

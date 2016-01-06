using OneBuild.Common;
using OneBuild.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBuild.UI.Easyui
{
    public class EasyUI
    {
        private readonly static object syncRoot = new object();

        public string BuildFileTree(FileSystemNodeTree node, bool isRoot = true)
        {
            StringBuilder builder = new StringBuilder();
            if (node.Name.Equals("root"))
            {
                builder.Append($"[{{\"id\":\"{node.NodeId}\",\"state\":\"open\",\"text\":\"这台电脑\"");
            }
            else
            {
                if (isRoot)
                {
                    builder.Append($"{{\"id\":\"{node.NodeId}\",\"text\":\"{node.Name}\",\"attributes\":\"{{'isLoad':'{node.IsLoad}','path':'{node.Path}'}}\"");
                }
            }
            if (isRoot && (node.Type == FileSystemType.Folder || node.Type == FileSystemType.Disk))
            {
                builder.Append($",\"state\":\"closed\"");
            }
            if (node.Type == FileSystemType.Folder || node.Type == FileSystemType.Disk || node.Name.Equals("root"))
            {
                if (node.Children.Count > 0)
                {
                    if (isRoot)
                    {
                        builder.Append($",\"children\":[");
                    }
                    else
                    {
                        builder.Append("[");
                    }
                    Parallel.ForEach(node.Children, n =>
                    {
                        string nodeChildernStr = BuildFileTree(n);
                        lock (syncRoot)
                        {
                            builder.Append(nodeChildernStr);
                        }
                    });
                    builder.Remove(builder.Length - 1, 1);

                    if (isRoot)
                    {
                        builder.Append($"]");
                    }
                    else
                    {
                        builder.Remove(builder.Length - 1, 1);
                        builder.Append("}]");
                    }
                }
            }
            if (node.Name.Equals("root"))
            {
                builder.Append("}]");
            }
            else
            {
                if (isRoot)
                {
                    builder.Append("},");
                }
            }
            return builder.ToString();
        }

        public string Navigate(string url)
        {
            if (!File.Exists(url))
            {
                throw new FileNotFoundException($"not found file \"{url}\"");
            }
            string html = File.ReadAllText(url);
            return html;
        }
    }
}

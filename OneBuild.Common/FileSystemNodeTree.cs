namespace OneBuild.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public class FileSystemNodeTree
    {
        public Guid NodeId { get; set; }

        public string Name { get; set; }

        public FileSystemType Type { get; set; }

        public bool IsLoad { get; set; }

        public string Path { get; set; }

        private FileSystemNodeTree()
        {
        }

        public List<FileSystemNodeTree> Children { get; set; }

        public static FileSystemNodeTree CreateNode(string name, FileSystemType type,string path, bool isLoad = false)
        {
            FileSystemNodeTree node = new FileSystemNodeTree();
            node.Name = name.Replace(@"\","");
            node.Type = type;
            node.NodeId = Guid.NewGuid();
            node.Children = new List<FileSystemNodeTree>();
            node.IsLoad = isLoad;
            node.Path = path.Replace(@"\",@"\\");
            return node;
        }

        public void AddChildren(FileSystemNodeTree node)
        {
            this.Children.Add(node);
        }
    }

    public enum FileSystemType
    {
        Disk,
        Folder,
        File,
        Unknown
    }
}

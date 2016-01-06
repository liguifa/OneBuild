using OneBuild.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBuild.Config
{
    public static class Machine
    {
        private readonly static object syncRoot = new object();

        private static string applicationDir;
        public static string ApplicationDir { get { return applicationDir; } }
        public static FileSystemNodeTree GetFileSystemTree(DirectoryInfo path = null)
        {
            if (path == null)
            {
                return BuildFileSystemTree();
            }
            else
            {
                return BuildFileSystemTree(path, false);
            }
        }

        static Machine()
        {
            applicationDir = AppDomain.CurrentDomain.BaseDirectory;
        }

        private static FileSystemNodeTree BuildFileSystemTree()
        {
            //创建Root节点
            FileSystemNodeTree root = FileSystemNodeTree.CreateNode("root", FileSystemType.Unknown,"roor",true);
            DriveInfo[] drives = DriveInfo.GetDrives();
            Parallel.ForEach(drives, (drive) =>
            {
                if (drive.IsReady)
                {
                    FileSystemNodeTree node = FileSystemNodeTree.CreateNode(drive.Name, FileSystemType.Disk, "root",true);
                    FileSystemNodeTree nodeChildrn = BuildFileSystemTree(drive.RootDirectory, false);
                    nodeChildrn.Children.ForEach(n => node.AddChildren(n));
                    lock (syncRoot)
                    {
                        root.AddChildren(node);
                    }
                }
            });
            return root;
        }

        private static FileSystemNodeTree BuildFileSystemTree(DirectoryInfo directory, bool isChildren = true)
        {
            FileSystemNodeTree node = FileSystemNodeTree.CreateNode(directory.Name, FileSystemType.Folder,directory.FullName, true);
            DirectoryInfo[] directories = directory.GetDirectories();
            Parallel.ForEach(directories, folder =>
            {
                try
                {
                    FileSystemNodeTree folderNode;
                    if (isChildren)
                    {
                        folderNode = GetFileSystemTree(folder);
                    }
                    else
                    {
                        folderNode = FileSystemNodeTree.CreateNode(folder.Name, FileSystemType.Folder, folder.FullName,false);
                    }
                    lock (syncRoot)
                    {
                        node.AddChildren(folderNode);
                    }
                }
                catch (Exception e)
                {

                }
            });
            FileInfo[] files = directory.GetFiles();
            Parallel.ForEach(files, file =>
            {
                FileSystemNodeTree fileNode = FileSystemNodeTree.CreateNode(file.Name, FileSystemType.File,file.FullName,true);
                lock (syncRoot)
                {
                    node.AddChildren(fileNode);
                }
            });
            return node;
        }
    }
}

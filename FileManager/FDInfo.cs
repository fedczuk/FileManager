using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FileManager
{
    interface IDiskEntry
    {
        string Name { get; set; }
        string Extension { get; }
    }

    class FInfo : IDiskEntry
    {
        public FInfo(FileInfo fi)
        {
            this.Name = fi.Name;
            this.Extension = fi.Extension;
            this.Size = fi.Length;
        }

        #region IDiskEntry Members

        public string Name { get; set; }
        public string Extension { get; private set; }

        #endregion

        public long Size { get; private set; }
    }

    class DInfo : IDiskEntry
    {
        public DInfo(DirectoryInfo di)
        {
            this.Name = di.Name;
            this.Extension = "<DIR>";
        }

        #region IDiskEntry Members

        public string Name { get; set; }
        public string Extension { get; private set; }

        #endregion
    }
}

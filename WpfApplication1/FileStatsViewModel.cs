﻿/************************************************************************

   AvalonDock

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the New BSD
   License (BSD) as published at http://avalondock.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up AvalonDock in Extended WPF Toolkit Plus at http://xceed.com/wpf_toolkit

   Stay informed: follow @datagrid on Twitter or Like facebook.com/datagrids

  **********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace WpfApplication1
{
    class FileStatsViewModel : ToolViewModel
    {
        public FileStatsViewModel()
            :base("File Stats")
        {
            Workspace.Instance.ActiveDocumentChanged += new EventHandler(OnActiveDocumentChanged);
            ContentId = ToolContentId;

            //BitmapImage bi = new BitmapImage();
            //bi.BeginInit();
            //bi.UriSource = new Uri("pack://application:,,/Images/property-blue.png");
            //bi.EndInit();
            //IconSource = bi;
        }

        public const string ToolContentId = "FileStatsTool";

        void OnActiveDocumentChanged(object sender, EventArgs e)
        {
            if (Workspace.Instance.ActiveDocument != null)
            {
                var file =  Workspace.Instance.ActiveDocument as FileViewModel;
                if(file != null &&
                    file.FilePath != null &&
                    File.Exists(file.FilePath))
                {
                    var fi = new FileInfo(file.FilePath);
                    FileSize = fi.Length;
                    LastModified = fi.LastWriteTime;
                }
                else
                {

                    FileSize = 0;
                    LastModified = DateTime.MinValue;
                }
            }
            else
            {
                FileSize = 0;
                LastModified = DateTime.MinValue;
            }
        }

        #region FileSize

        private long _fileSize;
        public long FileSize
        {
            get { return _fileSize; }
            set
            {
                if (_fileSize != value)
                {
                    _fileSize = value;
                    RaisePropertyChanged("FileSize");
                }
            }
        }

        #endregion

        #region LastModified

        private DateTime _lastModified;
        public DateTime LastModified
        {
            get { return _lastModified; }
            set
            {
                if (_lastModified != value)
                {
                    _lastModified = value;
                    RaisePropertyChanged("LastModified");
                }
            }
        }

        #endregion




    }
}

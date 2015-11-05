/************************************************************************

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
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using Xceed.Wpf.AvalonDock.Layout;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace WpfApplication1
{
    class Workspace : ViewModelBase
    {
        protected Workspace()
        { 

        }

        static Workspace _this = new Workspace();

        public static Workspace This
        {
            get { return _this; }
        }


        ObservableCollection<DocumentViewModel> _files = new ObservableCollection<DocumentViewModel>();
        ReadOnlyObservableCollection<DocumentViewModel> _readonyFiles = null;
        public ReadOnlyObservableCollection<DocumentViewModel> Files
        {
            get
            {
                if (_readonyFiles == null)
                    _readonyFiles = new ReadOnlyObservableCollection<DocumentViewModel>(_files);

                return _readonyFiles;
            }
        }


        ObservableCollection<ParameterTabViewModel> _tools = new ObservableCollection<ParameterTabViewModel>();
        ReadOnlyObservableCollection<ParameterTabViewModel> _readonyTools = null;
        public ReadOnlyObservableCollection<ParameterTabViewModel> Tools
        {
            get
            {
                if (_readonyTools == null)
                    _readonyTools = new ReadOnlyObservableCollection<ParameterTabViewModel>(_tools);

                return _readonyTools;
            }
        }


        FileStatsViewModel _fileStats = null;
        public FileStatsViewModel FileStats
        {
            get
            {
                if (_fileStats == null)
                    _fileStats = new FileStatsViewModel();

                return _fileStats;
            }
        }

        private CategoryTreePaneViewModel m_categoryTreeViewModel = null;
        public CategoryTreePaneViewModel CategoryTree
        {
            get
            {
                return m_categoryTreeViewModel = m_categoryTreeViewModel ?? new CategoryTreePaneViewModel("m_0000"); 
            }
        }
        //public 

        #region OpenCommand
        RelayCommand _openCommand = null;
        public ICommand OpenCommand
        {
            get
            {
                if (_openCommand == null)
                {
                    _openCommand = new RelayCommand((p) => OnOpen(p), (p) => CanOpen(p));
                }

                return _openCommand;
            }
        }

        private bool CanOpen(object parameter)
        {
            return true;
        }

        private void OnOpen(object parameter)
        {
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog().GetValueOrDefault())
            {
                var fileViewModel = Open(dlg.FileName);
                ActiveDocument = fileViewModel;
            }
        }

        public FileViewModel Open(string filepath)
        {
            var fileViewModel = _files.OfType<FileViewModel>().FirstOrDefault(fm => fm.FilePath == filepath);
            if (fileViewModel != null)
                return fileViewModel;

            fileViewModel = new FileViewModel(filepath);
            _files.Add(fileViewModel);
            return fileViewModel;
        }

        #endregion 

        #region NewCommand
        RelayCommand _newCommand = null;
        public ICommand NewCommand
        {
            get
            {
                if (_newCommand == null)
                {
                    _newCommand = new RelayCommand((p) => OnNew(p), (p) => CanNew(p));
                }

                return _newCommand;
            }
        }

        private bool CanNew(object parameter)
        {
            return true;
        }
        IdInfoTablePaneViewModel m_idInfoTable;
        public IdInfoTablePaneViewModel IdInfoTable
        {
            get
            {
                if(null == m_idInfoTable)
                {
                    m_idInfoTable = new IdInfoTablePaneViewModel();
                }
                return m_idInfoTable;
            }
        }
        private void OnNew(object parameter)
        {
            //_files.Add(new FileViewModel());
            _files.Add(CategoryTree);
            _files.Add(IdInfoTable);
            IdInfoTable.SelectedItemChanged += IdInfoTable_SelectedItemChanged;
            
            ActiveDocument = _files.Last();
        }

        void IdInfoTable_SelectedItemChanged(object sender, ParameterViewModelBase e)
        {
            var tab = new ParameterTabViewModel(CategoryTree,e);
            if (!_tools.Contains(tab))
                _tools.Add(tab);
        }

        #endregion 

        private InteractionRequest<Confirmation> m_confirmationRequest = new InteractionRequest<Confirmation>();
        public IInteractionRequest ConfirmationRequest
        {
            get
            { return m_confirmationRequest; }
        }

        DelegateCommand m_createCommand;
        public ICommand CreateCommand
        { 
            get
            {
                return m_createCommand = m_createCommand ?? new DelegateCommand(_OnCreateNewId);
            }
        }

        private void _OnCreateNewId()
        {
            if(!RaiseConfirm())
            { return; }
        }

        public bool RaiseConfirm()
        {
            bool isConfirm = true;

            m_confirmationRequest.Raise(new Confirmation { Title = "aaa", Content = "contenttttt" },
                (c => isConfirm = c.Confirmed));

            return isConfirm;
        }

        #region ActiveDocument

        private DocumentViewModel _activeDocument = null;
        public DocumentViewModel ActiveDocument
        {
            get { return _activeDocument; }
            set
            {
                if (_activeDocument != value)
                {
                    _activeDocument = value;
                    RaisePropertyChanged("ActiveDocument");
                    if (ActiveDocumentChanged != null)
                        ActiveDocumentChanged(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler ActiveDocumentChanged;

        #endregion


        internal void Close(FileViewModel fileToClose)
        {
            if (fileToClose.IsDirty)
            {
                var res = MessageBox.Show(string.Format("Save changes for file '{0}'?", fileToClose.FileName), "WpfApplication1 Test App", MessageBoxButton.YesNoCancel);
                if (res == MessageBoxResult.Cancel)
                    return;
                if (res == MessageBoxResult.Yes)
                {
                    Save(fileToClose);
                }
            }

            _files.Remove(fileToClose);
        }

        internal void Save(FileViewModel fileToSave, bool saveAsFlag = false)
        {
            if (fileToSave.FilePath == null || saveAsFlag)
            {
                var dlg = new SaveFileDialog();
                if (dlg.ShowDialog().GetValueOrDefault())
                    fileToSave.FilePath = dlg.SafeFileName;
            }

            File.WriteAllText(fileToSave.FilePath, fileToSave.TextContent);
            fileToSave.IsDirty = false;
        }



    }
}

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
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using Xceed.Wpf.AvalonDock.Layout;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using WpfApplication1.Controls;

namespace WpfApplication1
{
    class Workspace : ViewModelBase
    {
        protected Workspace()
        { 

        }

        static Workspace s_instance = new Workspace();

        public static Workspace Instance
        {
            get { return s_instance; }
        }


        ObservableCollection<DocumentViewModel> m_documents = new ObservableCollection<DocumentViewModel>();
        ReadOnlyObservableCollection<DocumentViewModel> _readonyFiles = null;
        public ReadOnlyObservableCollection<DocumentViewModel> Files
        {
            get
            {
                if (_readonyFiles == null)
                {
                    m_documents.Add(ParameterFileTree);
                    m_documents.Add(FileSharePane);
                    m_documents.Add(IdInfoTablePane);
                    _readonyFiles = new ReadOnlyObservableCollection<DocumentViewModel>(m_documents);
                }
                return _readonyFiles;
            }
        }

        FileSharePaneViewModel m_fileSharePane;
        public FileSharePaneViewModel FileSharePane
        {
            get
            {
                return m_fileSharePane = m_fileSharePane ?? new FileSharePaneViewModel();
            }
        }


        ObservableCollection<ParameterTab2ViewModel> _tools = new ObservableCollection<ParameterTab2ViewModel>();
        ReadOnlyObservableCollection<ParameterTab2ViewModel> _readonyTools = null;
        public ReadOnlyObservableCollection<ParameterTab2ViewModel> Tools
        {
            get
            {
                if (_readonyTools == null)
                    _readonyTools = new ReadOnlyObservableCollection<ParameterTab2ViewModel>(_tools);

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

                ParameterFileTree.Add(dlg.FileName);
            }
        }

        public DocumentViewModel Open(string filepath)
        {
            //var fileViewModel = m_documents.OfType<FileViewModel>().FirstOrDefault(fm => fm.FilePath == filepath);
            //if (fileViewModel != null)
            //    return fileViewModel;

            //fileViewModel = new FileViewModel(filepath);
            //m_documents.Add(fileViewModel);

            
            var fileViewModel = new CategoryTreePaneViewModel(filepath);
            m_documents.Add(fileViewModel);
            fileViewModel.IsSelected = true;
            fileViewModel.IsActive = true;
            return fileViewModel;
        }

        public void OpenFile(string filePath)
        {

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
            //m_documents.Add(new FileViewModel());
            //m_documents.Add(CategoryTree);
            //m_documents.Add(IdInfoTable);
            IdInfoTable.SelectedItemChanged += IdInfoTable_SelectedItemChanged;


            ParameterFileTree.Add(null);

            ActiveDocument = m_documents.Last();


        }

        void IdInfoTable_SelectedItemChanged(object sender, ParameterViewModelBase e)
        {
            //var tab = new ParameterTabViewModel(CategoryTree,e);
            //if (!_tools.Contains(tab))
            //    _tools.Add(tab);
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

        private PaneViewModel _activeDocument = null;
        public PaneViewModel ActiveDocument
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

                    var categoryTree = _activeDocument as CategoryTreePaneViewModel;
                    if(categoryTree != null)
                    {
                        ActiveFile = categoryTree;
                    }
                }
            }
        }

        public event EventHandler ActiveDocumentChanged;

        #endregion

        private CategoryTreePaneViewModel m_activeFile = null;
        public CategoryTreePaneViewModel ActiveFile
        {
            get
            {
                return m_activeFile;
            }
            private set
            {
                SetProperty(ref m_activeFile, value);
                // ファイル変更時
                if(null != value)
                {
                    // ID詳細タブの中身を変更
                    IdInfoTablePane.Content = m_activeFile.IdInfoTablePane.Content;
                }
            }
        }


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

            m_documents.Remove(fileToClose);
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



        /// <summary>
        /// パラメータ編集タブの更新
        /// </summary>
        /// <param name="categoryTreePaneViewModel">開くファイル</param>
        /// <param name="parameterIdViewModel">開くID</param>
        /// <param name="doFloating">フローティングさせるか</param>
        internal void UpdateParameterTab(CategoryTreePaneViewModel categoryTreePaneViewModel, ParametersViewModel parameterIdViewModel, bool doFloating)
        {
            var tab = new ParameterTab2ViewModel(categoryTreePaneViewModel, parameterIdViewModel) { DoFloating = doFloating };
                        

            var first = _tools.FirstOrDefault(t => t == tab);
            // 既に追加されている場合
            if (null != first)
            {
                first.IsActive = true;
                first.IsSelected = true;
            }
            else
            {
                // 1タブも開いていない場合か、フローティングさせたい時
                if (_tools.Count == 0 || doFloating)
                {
                    // 開く
                    _tools.Add(tab);
                    return;
                }
                else
                {
                    // 1タブ開いている場合 パラメータ部分のみ変更
                    _tools[0] = tab;
                    OnPropertyChanged(() => this.Tools);
                    //_tools[0].Parameter = tab.Parameter;
                }
            }

        }

        private IdInfoTablePane2ViewModel m_idInfoTablePane;
        /// <summary>
        /// ID詳細情報
        /// </summary>
        public IdInfoTablePane2ViewModel IdInfoTablePane
        {
            get
            {
                return m_idInfoTablePane = m_idInfoTablePane ?? new IdInfoTablePane2ViewModel(null);
            }
            set
            {
                // 旧IdInfoTableを探す
                int idInfoTableIndex = 0;
                foreach(var doc in m_documents)
                {
                    if(doc == m_idInfoTablePane)
                    {
                        break;
                    }
                    ++idInfoTableIndex;
                }
                SetProperty(ref m_idInfoTablePane, value);
                if(idInfoTableIndex < m_documents.Count)
                {
                    m_documents[idInfoTableIndex] = m_idInfoTablePane;
                }
            }
        }


        /// <summary>
        /// CategoryFilePaneViewModel
        /// </summary>
        public class NodeContent : ViewModelBase
        {
            public enum NodeType
            {
                File,
                Group
            }
            private readonly NodeType m_type;
            public NodeType Type
            {
                get
                {
                    return m_type;
                }
            }

            public bool IsFileNode
            {
                get
                {
                    return m_type == NodeContent.NodeType.File;
                }
            }
            public bool IsGroupNode
            {
                get
                {
                    return m_type == NodeContent.NodeType.Group;
                }
            }
            

            public string Label
            {
                get
                {
                    
                    return Path.GetFileNameWithoutExtension(FilePath);
                }
            }

            private string m_filePath;
            public string FilePath
            {
                get
                {
                    return m_filePath;
                }
                set
                {
                    if(value != m_filePath)
                    {
                        SetProperty(ref m_filePath, value);
                    }
                }
            }

            public NodeContent(NodeType type)
            {
                m_type = type;
            }
        }


        /// <summary>
        /// パラメータファイルツリーのVM
        /// </summary>
        public class ParameterFileTreePaneViewModel : DocumentViewModel, ITreeContent<NodeContent>
        {
            int count = 0;
            NodeBase<NodeContent> m_rootNode;
            public NodeBase<NodeContent> RootNode
            {
                get
                {
                    if(null == m_rootNode)
                    {
                        m_rootNode = new NodeBase<NodeContent>(this);
                    }
                    return m_rootNode;
                }
                set
                {
                    if(value != m_rootNode)
                    {
                        SetProperty(ref m_rootNode, value);
                    }
                }

            }

            private NodeBase<NodeContent> m_selectedItem;
            public NodeBase<NodeContent> SelectedItem
            {
                get
                {
                    return m_selectedItem;
                }
                set
                {
                    if (value != m_selectedItem)
                    {
                        SetProperty(ref m_selectedItem, value);
                        m_selectedItem.IsSelected = true;
                        m_upCommand.RaiseCanExecuteChanged();
                    }
                }
            }

            DelegateCommand m_upCommand;
            public ICommand UpCommand
            {
                get
                {

                    return m_upCommand = m_upCommand ?? new DelegateCommand(_OnUp, _DoenUp);

                }
            }

            private bool _DoenUp()
            {
                return SelectedItem != null && SelectedItem.Parent != RootNode;
            }

            private void _OnUp()
            {
                _GetCurrentGroupNode(SelectedItem).Parent.Nodes.Add(SelectedItem);
                SelectedItem.Parent.Nodes.Remove(SelectedItem);
            }

            DelegateCommand m_addGroupCommand;
            public ICommand AddGroupCommand
            {
                get
                {
                    return m_addGroupCommand = m_addGroupCommand ?? new DelegateCommand(_OnAddGroup);
                }
            }

            int groupCount = 0;
            private void _OnAddGroup()
            {
                var group = RootNode;
                if (SelectedItem != null)
                {
                    group = _GetCurrentGroupNode(SelectedItem);
                }

                var added = group.Add(new NodeContent(NodeContent.NodeType.Group) { FilePath = string.Format("group {0}", ++groupCount) });
                added.IsSelected = true;
            }

            private NodeBase<NodeContent> _GetCurrentGroupNode(NodeBase<NodeContent> node)
            {
                if (node.Value.IsGroupNode)
                {
                    return node;
                }
                // fileノードの場合
                return node.Parent;
            }

            public ParameterFileTreePaneViewModel()
                : base("ファイルツリー")
            {
            }

            public void Add(string filePath)
            {
                if(filePath == null)
                {
                    filePath = "noname" + ++count;
                }
                var group = RootNode;
                if(SelectedItem != null)
                {
                    group = _GetCurrentGroupNode(SelectedItem);
                }
                var added = group.Add(new NodeContent(NodeContent.NodeType.File) { FilePath = (filePath) });
                added.IsSelected = true;
            }


            private DelegateCommand m_openCommand;
            public ICommand OpenCommand
            {
                get
                {
                    return m_openCommand = m_openCommand ?? new DelegateCommand(_OnOpen, _DoesOpen);
                }
            }

            private bool _DoesOpen()
            {
                return null != SelectedItem && SelectedItem.Value.IsFileNode;
            }

            private void _OnOpen()
            {
                Workspace.Instance.Open(SelectedItem.Value.FilePath);
            }


        }


        private ParameterFileTreePaneViewModel　m_parameterFileTree;
        /// <summary>
        /// ファイルツリー
        /// </summary>
        public ParameterFileTreePaneViewModel　ParameterFileTree
        {
            get
            {
                return m_parameterFileTree = m_parameterFileTree ?? new ParameterFileTreePaneViewModel();
            }
        }

    }
}

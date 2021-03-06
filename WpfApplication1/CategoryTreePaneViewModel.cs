﻿using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace WpfApplication1
{
    /// <summary>
    /// カテゴリツリーに表示するファイル1タブごと 
    /// </summary>
    class CategoryTreePaneViewModel : ToolViewModel
    {
        public MenuContent MenuContent { get { return Workspace.Instance.MenuContent; } }

        public bool IsDirty
        {
            get
            {
                foreach (var parameters in Collection)
                {
                    foreach (var parameter in parameters.Parameters)
                    {
                        foreach (var slot in parameter.Slots)
                        {
                            if (slot.IsDirty)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            set
            {
                foreach (var parameters in Collection)
                {
                    foreach (var parameter in parameters.Parameters)
                    {
                        foreach (var slot in parameter.Slots)
                        {
                            slot.IsDirty = value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">ファイル名</param>
        public CategoryTreePaneViewModel(string name)
            : base(name)
        {
            Title = Path.GetFileNameWithoutExtension(name);
            // パラメータファイルの読み込み
            if(!string.IsNullOrEmpty(name) && File.Exists(name))
            {

                GparamRoot gparam;

                //XmlSerializerオブジェクトを作成
                System.Xml.Serialization.XmlSerializer serializer =
                    new System.Xml.Serialization.XmlSerializer(typeof(GparamRoot));
                //読み込むファイルを開く
                using (System.IO.StreamReader sr = new System.IO.StreamReader(
                    name, new System.Text.UTF8Encoding(false)))
                {
                    //XMLファイルから読み込み、逆シリアル化する
                    gparam = (GparamRoot)serializer.Deserialize(sr);
                    
                }

                if(!GParamToParameterParser.TryParse(gparam, out this.m_collection))
                {
                    System.Windows.MessageBox.Show("gparamファイルの読み込みに失敗","");
                }
            }

            foreach(var parameters in m_collection)
            {
                foreach(var parameter in parameters.Parameters)
                {
                    foreach(var slot in parameter.Slots)
                    {
                        slot.PropertyChanged += (sender, e) =>
                        {
                            if(e.PropertyName == "IsDirty")
                            {
                                OnPropertyChanged(() => this.IsDirty);
                            }
                        };
                    }
                }
            }

        }

        private IdInfoTablePaneViewModel m_idInfoTable;
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

        private IdInfoTablePane2ViewModel m_idInfoTablePane;
        public IdInfoTablePane2ViewModel IdInfoTablePane
        {
            get
            {
                if(null == m_idInfoTablePane)
                {
                    m_idInfoTablePane = new IdInfoTablePane2ViewModel(this);
                }
                return m_idInfoTablePane;
            }
        }

        

        private ObservableCollection<CategoryViewModel> m_collection = new ObservableCollection<CategoryViewModel>();
        private ReadOnlyObservableCollection<CategoryViewModel> m_readonlyCollection = null;
        public ReadOnlyObservableCollection<CategoryViewModel> Collection
        {
            get
            {
                if (null == m_readonlyCollection)
                {
                    m_readonlyCollection = new ReadOnlyObservableCollection<CategoryViewModel>(m_collection);
                }
                return m_readonlyCollection;
            }
        }

        private ViewModelBase m_selectedItem;
        /// <summary>
        /// 選択中のID
        /// </summary>
        public ViewModelBase SelectedItem
        {
            get
            {
                return m_selectedItem;
            }
            set
            {
                if (SetProperty(ref m_selectedItem, value))
                {
                    var categoryNode = value as CategoryViewModel;
                    if(null != categoryNode)
                    {
                        m_createNewIdCommand.RaiseCanExecuteChanged();
                        return;
                    }
                    var idNode = value as ParameterRecordViewModel;
                    if(null != idNode)
                    {
                        m_createNewIdCommand.RaiseCanExecuteChanged();
                        // パラメータビューの更新
                        bool doFloating = false;
                        var modifiers = Keyboard.Modifiers;
                        if (modifiers.HasFlag(ModifierKeys.Control))
                        {
                            // Ctrlキーが押されていたらフロート化
                            doFloating = true;
                        }
                        else
                        {
                            // それ以外はビューの更新
                            doFloating = false;
                        }

                        Workspace.Instance.UpdateParameterTab(this, idNode, doFloating);

                        // ID詳細タブオープン
                        IdInfoTablePane.Content.OpenParameterTab(Collection.FirstOrDefault((category=>category.Parameters.Contains(idNode))));

                    }
                }
            }
        }

        #region Commands

        #region SaveCommand

        private DelegateCommand m_saveCommand = null;
        public ICommand SaveCommand
        {
            get
            {
                return m_saveCommand = m_saveCommand ?? new DelegateCommand(_OnSave, _DoesSave);
            }
        }

        private bool _DoesSave()
        {
            return true;

            //return IsDirty;
        }

        private void _OnSave()
        {
            Workspace.Instance.Save(this, false);
        }

        #endregion

        #region SaveAsCommand
        DelegateCommand _saveAsCommand = null;
        public ICommand SaveAsCommand
        {
            get
            {
                if (_saveAsCommand == null)
                {
                    _saveAsCommand = new DelegateCommand(OnSaveAs, CanSaveAs);
                }

                return _saveAsCommand;
            }
        }

        private bool CanSaveAs()
        {
            //return IsDirty;
            return true;
        }

        private void OnSaveAs()
        {
            Workspace.Instance.Save(this, true);
        }

        #endregion

        #region CopyIdCommand
        private DelegateCommand m_copyIdCommand = null;
        public ICommand CopyIdCommand { get { return m_copyIdCommand ?? (m_copyIdCommand = new DelegateCommand(_OnCopyId, _DoesCopyId)); } }

        private bool _DoesCopyId()
        {
            return false;
        }

        private void _OnCopyId()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region CopyParameterCommand

        private DelegateCommand m_copyParameterCommand = null;
        public ICommand CopyParameterCommand { get { return m_copyParameterCommand ?? (m_copyParameterCommand = new DelegateCommand(_OnCopyParameter, _DoesCopyParameter)); } }

        private bool _DoesCopyParameter()
        {
            return false;
        }

        private void _OnCopyParameter()
        {
            throw new NotImplementedException();
        }

        #endregion
        #region PasteCommand
        private DelegateCommand m_pasteCommand = null;
        public ICommand PasteCommand { get { return m_pasteCommand ?? (m_pasteCommand = new DelegateCommand(_OnPaste, _DoesPaste)); } }

        private bool _DoesPaste()
        {
            return false;
        }

        private void _OnPaste()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region CreateNewIdCommand
        DelegateCommand m_createNewIdCommand;
        public ICommand CreateNewIdCommand { get { return m_createNewIdCommand ?? (m_createNewIdCommand = new DelegateCommand(_OnCreateNewId, _DoesCreateNewId)); } }

        private bool _DoesCreateNewId()
        {
            return null != SelectedItem;
        }

        private void _OnCreateNewId()
        {
            CategoryViewModel categoryNode = null;
            var idNode = SelectedItem as ParameterRecordViewModel;
            if (null != idNode)
            {
                // IDの場合コレクション一覧からカテゴリノードの選択
                foreach(var categoryParameters in m_collection)
                {
                    if(categoryParameters.Parameters.Contains(idNode))
                    {
                        categoryNode = categoryParameters;
                        break;
                    }
                }
            }
            else
            {
                categoryNode = SelectedItem as CategoryViewModel;
            }

            if(null != categoryNode)
            {
                CreateNewIdWindowContentViewModel.Instance.Category = categoryNode.DispName;
            }
            

            if (!Workspace.Instance.RaiseConfirm())
            {
                // キャンセル
                return;
            }
            // パラメータの取得

            var id = CreateNewIdWindowContentViewModel.Instance.InputId;
            var comment = CreateNewIdWindowContentViewModel.Instance.InputComment;
            var name = CreateNewIdWindowContentViewModel.Instance.InputName;
            var category = CreateNewIdWindowContentViewModel.Instance.Category;

            //var newId = new ParameterRecordViewModel(category) { ID = id, Comment = comment, Name = name };
            //foreach(var slot in categoryNode.Parameters[0].Slots)
            //{
            //    var value = new TUnitValue<object>()
            //    {
            //        DispName = slot.DispName,
            //        Name = slot.Name,
            //        TabIndex = slot.TabIndex,
            //        Type = slot.Type,
            //        IsDirty = slot.IsDirty,
            //        IsExpanded = slot.IsExpanded,
            //        Value = slot.Value
            //    };
                    
            //    newId.Slots.Add(value);
            //}

            //categoryNode.Parameters.Add(newId);
            

            // #TODO:デフォルトファイルの読み込み&デフォルトIDの作成

            var defaultCategory = ConfigManager.Instance.DefaultGParam.GetCategoryFromDispName(category);
            if (null != defaultCategory)
            {
                // カテゴリのスロット一覧のコピー
                //var newRecord = DeepCopyExtensions.DeepCopy(defaultCategory.Parameters[0]);
                //var newRecord = (ParameterRecordViewModel)defaultCategory.Parameters[0].DeepCopy();
                var newRecord = defaultCategory.Parameters[0].CreateCopy();
                newRecord.ID = id;
                newRecord.Comment = comment;
                newRecord.Name = name;
                categoryNode.Parameters.Add(newRecord);
            }
        }
        #endregion  // CreateCommand

        #region InstanceCopyCommand
        private DelegateCommand m_instanceCopyCommand = null;
        public ICommand InstanceCopyCommand { get { return m_instanceCopyCommand ?? (m_instanceCopyCommand = new DelegateCommand(_OnInstanceCopy, _DoesInstanceCopy)); } }

        private bool _DoesInstanceCopy()
        {
            return false;
        }

        private void _OnInstanceCopy()
        {
            throw new NotImplementedException();
        }

        #endregion
        #region CancelEditCommand
        private DelegateCommand m_cancelEditCommand = null;
        public ICommand CancelEditCommand { get { return m_cancelEditCommand ?? (m_cancelEditCommand = new DelegateCommand(_OnCancelEdit, _DoesCancelEdit)); } }

        private bool _DoesCancelEdit()
        {
            return false;
        }

        private void _OnCancelEdit()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region DeleteIdCommand
        private DelegateCommand m_deleteIdCommand = null;
        public ICommand DeleteIdCommand { get { return m_deleteIdCommand ?? (m_deleteIdCommand = new DelegateCommand(_OnDeleteId, _DoesDeleteId)); } }

        private bool _DoesDeleteId()
        {
            return false;
        }

        private void _OnDeleteId()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region EditIdInfoCommand
        private DelegateCommand m_editIdInfoCommand = null;
        public ICommand EditIdInfoCommand { get { return m_editIdInfoCommand ?? (m_editIdInfoCommand = new DelegateCommand(_OnEditIdInfo, _DoesEditIdInfo)); } }

        private bool _DoesEditIdInfo()
        {
            return false;
        }

        private void _OnEditIdInfo()
        {
            throw new NotImplementedException();
        }
        #endregion

        #endregion
    }
}

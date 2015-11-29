using Microsoft.Practices.Prism.Commands;
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
    class CategoryTreePaneViewModel : DocumentViewModel
    {
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

                if(!GParamToParameterParser.TryParse(gparam, ref this.m_collection))
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

        

        private ObservableCollection<ParameterCollectionViewModel> m_collection = new ObservableCollection<ParameterCollectionViewModel>();
        private ReadOnlyObservableCollection<ParameterCollectionViewModel> m_readonlyCollection = null;
        public ReadOnlyObservableCollection<ParameterCollectionViewModel> Collection
        {
            get
            {
                if (null == m_readonlyCollection)
                {
                    m_readonlyCollection = new ReadOnlyObservableCollection<ParameterCollectionViewModel>(m_collection);
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
                    var categoryNode = value as ParameterCollectionViewModel;
                    if(null != categoryNode)
                    {
                        m_createNewIdCommand.RaiseCanExecuteChanged();
                        return;
                    }
                    var idNode = value as ParametersViewModel;
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





        #region CreateCommand
        DelegateCommand m_createNewIdCommand;
        public ICommand CreateNewIdCommand
        {
            get
            {
                return m_createNewIdCommand = m_createNewIdCommand ?? new DelegateCommand(_OnCreateNewId, _DoesCreateNewId);
            }
        }

        private bool _DoesCreateNewId()
        {
            return null != SelectedItem;
        }

        private void _OnCreateNewId()
        {
            ParameterCollectionViewModel categoryNode = null;
            var idNode = SelectedItem as ParametersViewModel;
            if (null != idNode)
            {
                // IDの場合コレクション一覧からカテゴリの選択
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
                categoryNode = SelectedItem as ParameterCollectionViewModel;
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

            if(null != categoryNode)
            {
                // #TODO:デフォルトファイルの読み込み&デフォルトIDの作成

                var newId = new ParametersViewModel(category) { ID = id, Comment = comment, Name = name };
                foreach(var slot in categoryNode.Parameters[0].Slots)
                {
                    var value = new EditableValue()
                    {
                        DispName = slot.DispName,
                        Name = slot.Name,
                        TabIndex = slot.TabIndex,
                        Type = slot.Type,
                        Filter = slot.Filter,
                        IsDirty = slot.IsDirty,
                        IsExpanded = slot.IsExpanded,
                        Value = slot.Value
                    };
                    
                    newId.Slots.Add(value);
                }

                categoryNode.Parameters.Add(newId);
            }
            else
            {
                // #TODO: カテゴリ名からカテゴリノードの取得
            }

        }
        #endregion  // CreateCommand
    }
}

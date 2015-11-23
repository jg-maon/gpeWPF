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

        private ParametersViewModel m_selectedItem;
        /// <summary>
        /// 選択中のID
        /// </summary>
        public ParametersViewModel SelectedItem
        {
            get
            {
                return m_selectedItem;
            }
            set
            {
                if (SetProperty(ref m_selectedItem, value))
                {
                    if(value != null)
                    {
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

                        Workspace.Instance.UpdateParameterTab(this, SelectedItem, doFloating);

                        // ID詳細タブオープン
                        IdInfoTablePane.Content.OpenParameterTab(Collection.FirstOrDefault((category=>category.Parameters.Contains(SelectedItem))));

                    }
                }
            }
        }






    }
}

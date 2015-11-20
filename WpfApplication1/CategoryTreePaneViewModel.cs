using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    /// <summary>
    /// カテゴリツリーに表示するファイル1タブごと 
    /// </summary>
    class CategoryTreePaneViewModel : DocumentViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">ファイル名</param>
        public CategoryTreePaneViewModel(string name)
            : base(name)
        {
        
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


        private ObservableCollection<ParameterViewModelBase> m_parameters = new ObservableCollection<ParameterViewModelBase>();
        private ReadOnlyObservableCollection<ParameterViewModelBase> m_readonlyParameters = null;
        public ReadOnlyObservableCollection<ParameterViewModelBase> Parameters
        {
            get
            {
                if (m_readonlyParameters == null)
                {
                    m_readonlyParameters = new ReadOnlyObservableCollection<ParameterViewModelBase>(m_parameters);
                }
                return m_readonlyParameters;
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
    }
}

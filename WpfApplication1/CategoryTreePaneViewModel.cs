using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    }
}

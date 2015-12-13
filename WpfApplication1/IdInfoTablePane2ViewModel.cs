using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    /// <summary>
    /// ID詳細タブVM
    /// </summary>
    class IdInfoTablePane2ViewModel : ToolViewModel
    {
        IdInfoTablePaneContentViewModel m_content;
        public IdInfoTablePaneContentViewModel Content
        {
            get
            {
                return m_content;
            }
            set
            {
                SetProperty(ref m_content, value);
            }
        }
        public IdInfoTablePane2ViewModel(CategoryTreePaneViewModel file)
            : base("ID詳細情報")
        {
            m_content = new IdInfoTablePaneContentViewModel(file);
        }
    }


    /// <summary>
    /// ID詳細情報タブ用中身
    /// </summary>
    class IdInfoTablePaneContentViewModel : ViewModelBase
    {
        private ObservableCollection<IdInfoTableTabPageViewModel> m_parameterTabPages = new ObservableCollection<IdInfoTableTabPageViewModel>();
        private ReadOnlyObservableCollection<IdInfoTableTabPageViewModel> m_readonlyParameterTabPages = null;
        public ReadOnlyObservableCollection<IdInfoTableTabPageViewModel> ParameterTabPages
        {
            get
            {
                return m_readonlyParameterTabPages = m_readonlyParameterTabPages ?? new ReadOnlyObservableCollection<IdInfoTableTabPageViewModel>(m_parameterTabPages);
            }
        }

        /// <summary>
        /// 対象ファイル
        /// </summary>
        private readonly CategoryTreePaneViewModel m_file;

        public IdInfoTablePaneContentViewModel(CategoryTreePaneViewModel file)
        {
            m_file = file;

        }





        /// <summary>
        /// パラメータタブを開く
        /// </summary>
        /// <param name="categoryViewModel">カテゴリ</param>
        internal void OpenParameterTab(CategoryViewModel categoryViewModel)
        {
            var openTabPage = new IdInfoTableTabPageViewModel(m_file, categoryViewModel);

            var tabPage = m_parameterTabPages.FirstOrDefault((page => page.Equals(openTabPage)));
            // 開かれていない場合
            if (null == tabPage)
            {
                m_parameterTabPages.Add(openTabPage);
                tabPage = openTabPage;
            }
            tabPage.IsActive = true;
            tabPage.IsSelected = true;
        }
    }
}

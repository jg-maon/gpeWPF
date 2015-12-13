using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WpfApplication1
{
    class IdInfoTableTabPageViewModel : DocumentViewModel, IEquatable<IdInfoTableTabPageViewModel>
    {
        private readonly CategoryTreePaneViewModel m_file;
        public CategoryTreePaneViewModel File
        {
            get
            {
                return m_file;
            }
        }
        private readonly CategoryViewModel m_categoryViewModel;

        public string CategoryName
        {
            get
            {
                return m_categoryViewModel.DispName;
            }
        }


        public ObservableCollection<ParameterRecordViewModel> Parameters
        {
            get
            {
                return m_categoryViewModel.Parameters;
            }

        }


        private ParameterRecordViewModel m_selectedItem;
        public ParameterRecordViewModel SelectedItem
        {
            get
            {
                return m_selectedItem;
            }
            set
            {
                SetProperty(ref m_selectedItem, value);
                if(null != SelectedItem)
                {
                    Workspace.Instance.UpdateParameterTab(m_file, SelectedItem, false);
                }
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="file">ファイル</param>
        /// <param name="categoryViewModel">カテゴリ</param>
        public IdInfoTableTabPageViewModel(CategoryTreePaneViewModel file, CategoryViewModel categoryViewModel)
            : base(categoryViewModel.DispName)
        {
            this.m_file = file;
            this.m_categoryViewModel = categoryViewModel;
            
        }


        #region IEquatable<IdInfoTableTabPageViewModel> メンバー
        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IdInfoTableTabPageViewModel other)
        {
            if(m_file != other.m_file)
            {
                return false;
            }
            if(m_categoryViewModel != other.m_categoryViewModel)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}

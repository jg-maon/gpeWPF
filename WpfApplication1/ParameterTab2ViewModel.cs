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
    class ParameterTab2ViewModel : ToolViewModel , IEquatable<ParameterTab2ViewModel>
    {
        private readonly CategoryTreePaneViewModel m_file;
        public CategoryTreePaneViewModel File
        {
            get
            {
                return m_file;
            }
        }

        public override string Title
        {
            get
            {
                return Parameters.CategoryName + ":ID: " + Parameters.ID + " - " + Path.GetFileNameWithoutExtension(File.Title);
            }
        }

        private readonly ParametersViewModel m_parameter;
        public ParametersViewModel Parameters
        {
            get
            {
                return m_parameter;
            }
        }

        private bool m_isInfoGroupExpanded = true;
        public bool IsInfoGroupExpanded
        {
            get
            {
                return m_isInfoGroupExpanded;
            }
            set
            {
                SetProperty(ref m_isInfoGroupExpanded, value);
                OnPropertyChanged(() => this.ButtonToolTip);
            }
        }


        private string m_searchText;
        public string SearchText
        {
            get
            {
                return m_searchText;
            }
            set
            {
                SetProperty(ref m_searchText, value);
                CollectionView.Refresh();
            }
        }


        private System.Windows.Data.CollectionView m_collectionView;
        public System.Windows.Data.CollectionView CollectionView
        {
            get
            {
                if(null == m_collectionView)
                {
                    m_collectionView = (System.Windows.Data.CollectionView)System.Windows.Data.CollectionViewSource.GetDefaultView(Parameters.Slots);
                    m_collectionView.Filter = _OnFilter;
                }
                return m_collectionView;
            }

        }

        private bool _OnFilter(object obj)
        {
            // 未記入の場合はすべて表示
            if(string.IsNullOrEmpty(SearchText))
            {
                return true;
            }
            var value = obj as EditableValue;
            if(null == value)
            {
                return true;
            }

            if(value.Value is ObservableCollection<EditableValue>)
            {
                foreach(EditableValue v in value.Value)
                {
                    if(v.Name.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return true;
                    }
                }
            }


            return (value.Name.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        #region ExpandCommand

        private DelegateCommand m_expandCommand = null;
        /// <summary>
        /// 展開/折りたたみコマンド
        /// </summary>
        public ICommand ExpandCommand
        {
            get
            {
                if(null == m_expandCommand)
                {
                    m_expandCommand = new DelegateCommand(_OnExpand, _DoesExpand);
                }
                return m_expandCommand;
            }
        }

        /// <summary>
        /// 展開/折りたたみ可能か
        /// </summary>
        /// <returns></returns>
        private bool _DoesExpand()
        {
            return true;
        }

        /// <summary>
        /// 展開/折りたたみコマンド実行処理
        /// </summary>
        private void _OnExpand()
        {
            // 全て展開
            if(_IsAllExpanded(Parameters.Slots, false) && !IsInfoGroupExpanded)
            {
                this.IsInfoGroupExpanded = true;
                _AllExpand(Parameters.Slots, true);
            }
            else
            {
                // 全て畳む
                this.IsInfoGroupExpanded = false;
                _AllExpand(Parameters.Slots, false);
            }
        }

        /// <summary>
        /// 展開状態を再帰的に一致させる
        /// </summary>
        /// <param name="collection">コレクション</param>
        /// <param name="isExpanded">展開状態</param>
        private void _AllExpand(ObservableCollection<EditableValue> collection, bool isExpanded)
        {
            foreach (var value in collection)
            {
                value.IsExpanded = isExpanded;

                // 子の設定
                var group = value.Value as ObservableCollection<EditableValue>;
                if (null != group)
                {
                    _AllExpand(group, isExpanded);
                }
            }
        }


        #endregion

        /// <summary>
        /// 展開状態の変更ボタンのtooltip
        /// </summary>
        public string ButtonToolTip
        {
            get
            {
                // 全て折りたたまれている場合のみ
                if (_IsAllExpanded(Parameters.Slots, false) && !IsInfoGroupExpanded)
                {
                    return "全て展開";
                }
                else
                {
                    return "全て畳む";
                }
            }
        }

        /// <summary>
        /// 展開状態を再帰的に探す #TODO: 再帰的にする必要はあるか？
        /// </summary>
        /// <param name="collection">探す値のコレクション</param>
        /// <param name="isExpanded">展開されているかされていないか</param>
        /// <returns>
        /// true    : 全て指定した展開状態になっている
        /// false   : 一つでも展開状態が違う
        /// </returns>
        private static bool _IsAllExpanded(ObservableCollection<EditableValue> collection, bool isExpanded)
        {
            foreach(var value in collection)
            {
                if(value.IsExpanded != isExpanded)
                {
                    return false;
                }

                // 子の探査
                var group = value.Value as ObservableCollection<EditableValue>;
                if (null != group)
                {
                    // 条件に一致しない場合
                    if(!_IsAllExpanded(group, isExpanded))
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        public ParameterTab2ViewModel(CategoryTreePaneViewModel categoryTreePaneViewModel, ParametersViewModel parameterIdViewModel)
            : base("")
        {
            m_file = categoryTreePaneViewModel;

            m_parameter = parameterIdViewModel;
            
            _AddPropertyChangedEvent(m_parameter.Slots);
        }

        /// <summary>
        /// 再帰的にプロパティ変更イベントの追加
        /// </summary>
        /// <param name="collection"></param>
        private void _AddPropertyChangedEvent(ObservableCollection<EditableValue> collection)
        {
            foreach (var value in collection)
            {
                value.PropertyChanged += _OnValue_PropertyChanged;

                // 子の設定
                var group = value.Value as ObservableCollection<EditableValue>;
                if (null != group)
                {
                    _AddPropertyChangedEvent(group);
                }
            }
        }

        void _OnValue_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var value = sender as EditableValue;
            if (null != value)
            {
                value.PropertyChanged += (s, ev) =>
                {
                    if (ev.PropertyName == "IsExpanded")
                    {
                        this.OnPropertyChanged(() => this.ButtonToolTip);
                    }
                    else if(ev.PropertyName == "Value")
                    {
                        value.IsDirty = true;
                    }
                };
            }
        }

        public bool Equals(ParameterTab2ViewModel other)
        {
            if (m_file != other.m_file)
            { return false; }
            if (m_parameter != other.m_parameter)
            {
                return false;
            }
            return true;
        }

    }
}

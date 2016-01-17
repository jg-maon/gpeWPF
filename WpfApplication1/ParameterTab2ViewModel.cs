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
        public MenuContent MenuContent { get { return Workspace.Instance.MenuContent; } }
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

        private readonly ParameterRecordViewModel m_parameters;
        public ParameterRecordViewModel Parameters
        {
            get
            {
                return m_parameters;
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

        #region パラメータの絞り込み検索
        
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
                    m_collectionView.Filter = _OnParameterFilter;
                }
                return m_collectionView;
            }

        }

        /// <summary>
        /// 末端のパラメータDispNameと入力文字のフィルター
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool _OnFilter(object obj)
        {
            var value = obj as IEditableValue;
            if(null == value)
            {
                return true;
            }


            var collection = value.Value as ObservableCollection<IEditableValue>;
            if(null != collection)
            {
                System.Windows.Data.CollectionView cv = (System.Windows.Data.CollectionView)System.Windows.Data.CollectionViewSource.GetDefaultView(collection);
                cv.Filter = _OnFilter;
                foreach(IEditableValue v in collection)
                {
                    if(v.DispName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return true;
                    }
                }
            }

            // 未記入の場合はすべて表示
            if (string.IsNullOrEmpty(SearchText))
            {
                return true;
            }

            return (value.DispName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        /// <summary>
        /// グループに文字が含まれていたら子要素全てを表示させるフィルタ
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool _OnParameterFilter(object obj)
        {
            // フィルターにかけないオブジェクトの場合
            var value = obj as IEditableValue;
            if (null == value)
            {
                return true;
            }

            var result = value.DispName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0;

            // 自身がグループ用オブジェクトの場合
            var collection = value.Value as ObservableCollection<IEditableValue>;
            if (null != collection)
            {

                System.Windows.Data.CollectionView cv = (System.Windows.Data.CollectionView)System.Windows.Data.CollectionViewSource.GetDefaultView(collection);
                // グループ名に文字を含む場合
                if(result)
                {
                    // 子は全て表示
                    cv.Filter = null;
                }
                else
                {
                    // 子にフィルタをかける
                    cv.Filter = _OnParameterFilter;
                }
                // 子要素にフィルタがかかるものがあれば自身の表示を行う
                foreach (IEditableValue v in collection)
                {
                    if (v.DispName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return true;
                    }
                }
            }

            // 未記入の場合はすべて表示
            if (string.IsNullOrEmpty(SearchText))
            {
                return true;
            }

            // パラメータ名のフィルタ結果を返す
            return result;
        }

        #endregion

        #region Commands

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
                _OnExpandAll();
            }
            else
            {
                // 全て畳む
                _OnCollapseAll();
            }
        }

        /// <summary>
        /// 展開状態を再帰的に一致させる
        /// </summary>
        /// <param name="collection">コレクション</param>
        /// <param name="isExpanded">展開状態</param>
        private void _AllExpandRecursively(ObservableCollection<IEditableValue> collection, bool isExpanded)
        {
            foreach (var value in collection)
            {
                value.IsExpanded = isExpanded;

                // 子の設定
                var group = value.Value as ObservableCollection<IEditableValue>;
                if (null != group)
                {
                    _AllExpandRecursively(group, isExpanded);
                }
            }
        }


        #endregion

        #region ExpandAllCommand

        private DelegateCommand m_expandAllCommand = null;
        public ICommand ExpandAllCommand { get { return m_expandAllCommand ?? (m_expandAllCommand = new DelegateCommand(_OnExpandAll)); } }

        private void _OnExpandAll()
        {
            this.IsInfoGroupExpanded = true;
            _AllExpandRecursively(Parameters.Slots, true);
        }
        #endregion

        #region CollapseAllCommand
        private DelegateCommand m_collapseAllCommand = null;
        public ICommand CollapseAllCommand { get { return m_collapseAllCommand ?? (m_collapseAllCommand = new DelegateCommand(_OnCollapseAll)); } }

        private void _OnCollapseAll()
        {
            this.IsInfoGroupExpanded = false;
            _AllExpandRecursively(Parameters.Slots, false);
        }
        #endregion

        #region UndoCommand

        private DelegateCommand m_undoCommand = null;
        public ICommand UndoCommand { get { return m_undoCommand ?? (m_undoCommand = new DelegateCommand(_OnUndo, _DoesUndo)); } }

        private bool _DoesUndo()
        {
            return m_undoRedoManager.CanUndo;
        }

        private void _OnUndo()
        {
            m_undoRedoManager.Undo();
            OnPropertyChanged(() => Parameters);
            OnPropertyChanged(() => UndoCommand);
            OnPropertyChanged(() => RedoCommand);
        }
        #endregion

        #region RedoCommand

        private DelegateCommand m_redoCommand = null;
        public ICommand RedoCommand { get { return m_redoCommand ?? (m_redoCommand = new DelegateCommand(_OnRedo, _DoesRedo)); } }

        private bool _DoesRedo()
        {
            return m_undoRedoManager.CanRedo;
        }

        private void _OnRedo()
        {
            m_undoRedoManager.Redo();
            OnPropertyChanged(() => Parameters);
            OnPropertyChanged(() => UndoCommand);
            OnPropertyChanged(() => RedoCommand);
        }
        #endregion

        #endregion


        /// <summary>
        /// アンドゥ・リドゥ用マネージャ
        /// </summary>
        private readonly UndoRedoManager m_undoRedoManager = new UndoRedoManager(3);

        /// <summary>
        /// アンドゥ・リドゥ用変更された値
        /// </summary>
        private Memento<IEditableValue, ParameterRecordViewModel> m_memento = null;


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
        private static bool _IsAllExpanded(ObservableCollection<IEditableValue> collection, bool isExpanded)
        {
            foreach(var value in collection)
            {
                if(value.IsExpanded != isExpanded)
                {
                    return false;
                }

                // 子の探査
                var group = value.Value as ObservableCollection<IEditableValue>;
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


        public ParameterTab2ViewModel(CategoryTreePaneViewModel categoryTreePaneViewModel, ParameterRecordViewModel parameterIdViewModel)
            : base("")
        {
            m_file = categoryTreePaneViewModel;

            m_parameters = parameterIdViewModel;
            m_memento = new EditableValueMemento(null, Parameters);

            m_parameters.ValueChanging -= m_parameters_ValueChanging;
            m_parameters.ValueChanging += m_parameters_ValueChanging;
            //m_parameters.ValueChanged -= m_parameters_ValueChanged;
            //m_parameters.ValueChanged += m_parameters_ValueChanged;

            _AddPropertyChangedEvent(m_parameters.Slots);
        }

        void m_parameters_ValueChanging(object sender, IEditableValue value)
        {
            var record = (ParameterRecordViewModel)sender;

            var current = new EditableValueMemento(value, record);
            var cmd = new MementoCommand<IEditableValue, ParameterRecordViewModel>(m_memento, current);
            if (!m_undoRedoManager.Invoke(cmd))
            {
                // 編集しすぎ

            }
            m_memento = current;

            OnPropertyChanged(() => UndoCommand);
        }

        void m_parameters_ValueChanged(object sender, IEditableValue value)
        {
            var record = (ParameterRecordViewModel)sender;

            var current = new EditableValueMemento(value, record);
            var cmd = new MementoCommand<IEditableValue, ParameterRecordViewModel>(m_memento, current);
            if (!m_undoRedoManager.Invoke(cmd))
            {
                // 編集しすぎ

            }
            m_memento = current;

            OnPropertyChanged(() => UndoCommand);
        }

        /// <summary>
        /// 再帰的にプロパティ変更イベントの追加
        /// </summary>
        /// <param name="collection"></param>
        private void _AddPropertyChangedEvent(ObservableCollection<IEditableValue> collection)
        {
            foreach (var value in collection)
            {
                value.PropertyChanged -= _OnValue_PropertyChanged;
                value.PropertyChanged += _OnValue_PropertyChanged;

                // 子の設定
                var group = value.Value as ObservableCollection<IEditableValue>;
                if (null != group)
                {
                    _AddPropertyChangedEvent(group);
                }
            }
        }

        void _OnValue_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var value = sender as IEditableValue;
            if (null != value)
            {
                value.PropertyChanged -= value_PropertyChanged;
                value.PropertyChanged += value_PropertyChanged;

                
            }
        }

        void value_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var v = sender as IEditableValue;
            if (e.PropertyName == "IsExpanded")
            {
                this.OnPropertyChanged(() => this.ButtonToolTip);
            }
            else if (e.PropertyName != "IsDirty")
            {
                v.IsDirty = true;
            }
        }

        public bool Equals(ParameterTab2ViewModel other)
        {
            if (m_file != other.m_file)
            { return false; }
            if (m_parameters != other.m_parameters)
            {
                return false;
            }
            return true;
        }


    }
}

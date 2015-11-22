﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    /// <summary>
    /// パラメータ
    /// </summary>
    public class EditableValue : ViewModelBase
    {

        private string m_name;
        /// <summary>
        /// パラメータ名(DirLight0Angle的な)
        /// </summary>
        public string Name 
        {
            get
            {
                return m_name;
            }
            set
            {
                SetProperty(ref m_name, value);
            }
        }


        private dynamic m_value;
        public dynamic Value
        {
            get
            {
                return m_value;
            }
            set
            {
                this.SetProperty(ref m_value, value);
            }
        }

        private bool m_isExpanded;
        public bool IsExpanded
        {
            get
            {
                return m_isExpanded;
            }
            set
            {
                this.SetProperty(ref m_isExpanded, value);
            }
        }

        private int m_tabIndex = 0;
        public int TabIndex
        {
            get
            {
                return m_tabIndex;
            }
            set
            {
                this.SetProperty(ref m_tabIndex, value);
            }
        }

        private string m_filter;
        public string Filter
        {
            get
            {
                return m_filter;
            }
            set
            {
                this.SetProperty(ref m_filter, value);
            }
        }

        private bool m_isDirty = false;
        /// <summary>
        /// パラメータの値が変更されたか
        /// </summary>
        public virtual bool IsDirty
        {
            get
            {
                return m_isDirty;
            }
            set
            {
                this.SetProperty(ref m_isDirty, value);
            }
        }

    }

    public class EditableValueGroup : EditableValue
    {
        public EditableValueGroup()
        {
            Value = new ObservableCollection<EditableValue>();
        }

        /// <summary>
        /// パラメータの値が変更されたか
        /// </summary>
        public override bool IsDirty
        {
            get
            {
                // 自身に登録されている子の変更状態を確認
                foreach (var value in this.Value)
                {
                    // 一つでも変更されているものがあれば
                    if (value.IsDirty)
                    {
                        return true;
                    }
                }
                return false;
            }
            set
            {
                // 自身に登録されている子の変更状態を設定
                foreach (var v in this.Value)
                {
                    v.Value = value;
                }
            }
        }
    }

    /// <summary>
    /// IDとそのIDで登録されるパラメータ群
    /// </summary>
    /// <remarks>
    /// ID1つあたりの情報
    /// </remarks>
    public class ParametersViewModel : ViewModelBase
    {
        private readonly string m_categoryName;
        public string CategoryName
        {
            get
            {
                return m_categoryName;
            }
        }

        private int m_id;
        public int ID
        {
            get
            {
                return m_id;
            }
            set
            {
                SetProperty(ref m_id, value);
            }
        }

        private string m_name;
        /// <summary>
        /// 固有名
        /// </summary>
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                this.SetProperty(ref m_name, value);
            }
        }

        private string m_comment;
        /// <summary>
        /// コメント
        /// </summary>
        public string Comment
        {
            get
            {
                return m_comment;
            }
            set
            {
                this.SetProperty(ref m_comment, value);
            }
        }


        ObservableCollection<EditableValue> m_slots;
        /// <summary>
        /// 編集可能パラメータ(表示名と実値)
        /// </summary>
        public ObservableCollection<EditableValue> Slots
        {
            get
            {
                return m_slots = m_slots ?? new ObservableCollection<EditableValue>();
            }
            set
            {
                SetProperty(ref m_slots, value);
            }
        }

        public ParametersViewModel(string categoryName)
        {
            m_categoryName = categoryName;
        }
    }

    /// <summary>
    /// Slot配列 + Edited(ID配列)
    /// </summary>
    /// <remarks>
    /// カテゴリ1つあたりの情報
    /// ParamSet配列は外部で定義
    /// </remarks>
    /// <see cref="ObservableCollection ParameterCollection"/>
    public class ParameterCollectionViewModel : ViewModelBase
    {
        private string m_name;
        /// <summary>
        /// カテゴリ名
        /// </summary>
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                SetProperty(ref m_name, value);
            }
        }

        private ObservableCollection<ParametersViewModel> m_parameters = null;
        /// <summary>
        /// ID群
        /// </summary>
        public ObservableCollection<ParametersViewModel> Parameters
        {
            get
            {
                return m_parameters = m_parameters ?? new ObservableCollection<ParametersViewModel>();
            }
            set
            {
                SetProperty(ref m_parameters, value);
            }
        }

        /// <summary>
        /// 情報が空のIDの作成
        /// </summary>
        /// <returns>
        /// 作成したID
        /// </returns>
        public ParametersViewModel CreateId()
        {
            return new ParametersViewModel(Name);
        }

        /// <summary>
        /// 情報付きIDの作成
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">固有名</param>
        /// <param name="comment">コメント</param>
        /// <returns>
        /// 作成したID
        /// </returns>
        public ParametersViewModel CreateId(int id, string name, string comment)
        {
            return new ParametersViewModel(Name) { ID = id, Name = name, Comment = comment };
        }
    }


    /// <summary>
    /// ParamSet配例
    /// </summary>
    public class _Collection : ObservableCollection<ParameterCollectionViewModel>
    { }

}

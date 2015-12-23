﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class ComboBoxValue : TUnitValue<dynamic>
    {
        public ComboBoxValue()
        { }
        
        protected ComboBoxValue(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

            var innerItems = info.GetValue("m_innerItems", typeof(ComboBoxItem[]));
            m_innerItems = (ComboBoxItem[])innerItems;
        }
        private ComboBoxItem[] m_innerItems;
        [OnDeserialized]
        private void _OnDeserialized(StreamingContext context)
        {
            m_items = new ObservableCollection<ComboBoxItem>(m_innerItems);
            m_innerItems = null;
        }
        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            //info.AddValue("m_value", m_value, m_value.GetType());
            info.AddValue("m_innerItems", m_items.ToArray());
        }

        public class ComboBoxItem
        {
            public string Text { get; private set; }
            public dynamic Value { get; private set; }
            public ComboBoxItem(string text, dynamic value)
            {
                Text = text;
                Value = value;
            }
        }

        private ObservableCollection<ComboBoxItem> m_items = new ObservableCollection<ComboBoxItem>();
        private ReadOnlyObservableCollection<ComboBoxItem> m_readonlyItems = null;
        public ReadOnlyObservableCollection<ComboBoxItem> Items
        {
            get { return m_readonlyItems ?? (m_readonlyItems = new ReadOnlyObservableCollection<ComboBoxItem>(m_items)); }
        }
        
        private dynamic m_selectedValue;
        public dynamic SelectedValue
        {
            get { return m_selectedValue; }
            set
            {
                SetProperty(ref m_selectedValue, value);
                this.Value = value;
            }
        }

        /// <summary>
        /// アイテムの追加を行う
        /// </summary>
        /// <param name="text">表示名</param>
        /// <param name="value">値</param>
        public void AddItem(string text, dynamic value)
        {
            m_items.Add(new ComboBoxItem(text, value));
        }
        /// <summary>
        /// アイテムの削除を行う
        /// </summary>
        /// <param name="text">削除する項目の表示名</param>
        public void RemoveItem(string text)
        {
            m_items.Remove(m_items.FirstOrDefault(i => i.Text == text));            
        }

    }
}

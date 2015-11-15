using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Controls
{

    public interface ITreeContent<ValueType>
    {
        NodeBase<ValueType> SelectedItem { get; set; }
        
        NodeBase<ValueType> RootNode { get; }
    }

    public class NodeBase<ValueType> : ViewModelBase
    {

        private NodeBase<ValueType> m_parent = null;
        public NodeBase<ValueType> Parent
        {
            get
            {
                return m_parent;
            }
            set
            {
                if (value != m_parent)
                {
                    SetProperty(ref m_parent, value);
                }
            }
        }

        private ObservableCollection<NodeBase<ValueType>> m_nodes = new ObservableCollection<NodeBase<ValueType>>();
        public ObservableCollection<NodeBase<ValueType>> Nodes
        {
            get
            {
                return m_nodes;
            }
            set
            {
                if (value != m_nodes)
                {
                    SetProperty(ref m_nodes, value);
                }
            }
        }

        private ValueType m_value;
        public ValueType Value
        {
            get
            {
                return m_value;
            }
            set
            {
                SetProperty(ref m_value, value);
            }
        }

        private readonly ITreeContent<ValueType> m_tree;
        public ITreeContent<ValueType> Tree
        {
            get
            {
                return m_tree;
            }
        }

        public NodeBase(ITreeContent<ValueType> tree)
        {
            m_tree = tree;
        }
        public NodeBase(ValueType value, ITreeContent<ValueType> tree)
        {
            m_tree = tree;
            m_value = value;
        }

        public NodeBase<ValueType> Add(ValueType value)
        {
            NodeBase<ValueType> node = new NodeBase<ValueType>(value, Tree);
            node.Parent = this;
            Nodes.Add(node);
            return node;
        }


        private bool m_isSelected;
        public bool IsSelected
        {
            get
            {
                return m_isSelected;
            }
            set
            {
                if (value != m_isSelected)
                {
                    SetProperty(ref m_isSelected, value);
                    if (IsSelected)
                    {
                        Tree.SelectedItem = this;
                    }
                }
            }
        }
    }

}

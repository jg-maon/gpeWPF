using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication1.Controls;

namespace WpfApplication1
{
    public class Node : ViewModelBase
    {
        public enum Type
        {
            File,
            Group
        }
        private Type m_type;
            
        public bool IsFileNode
        {
            get
            {
                return m_type == Type.File;
            }
        }
        public bool IsGroupNode
        {
            get
            {
                return m_type == Type.File;
            }
        }

        private bool? m_isChecked = true;
        public bool? IsChecked
        {
            get
            {
                return m_isChecked;
            }
            set
            {
                if(value != m_isChecked)
                {
                    SetProperty(ref m_isChecked, value);
                }
            }
        }

        private string m_label;
        public string Label
        {
            get
            {
                return m_label;
            }
            set
            {
                if(value != m_label)
                {
                    SetProperty(ref m_label, value);
                }
            }
        }

        public Node(Type type)
        {
            m_type = type;
        }

    }
    class FileSharePaneViewModel : DocumentViewModel, ITreeContent<Node>
    {



        NodeBase<Node> m_rootNode;
        public NodeBase<Node> RootNode
        {
            get
            {
                if(null == m_rootNode)
                {
                    m_rootNode = new NodeBase<Node>(new Node(Node.Type.File), this);
                }
                return m_rootNode;
            }

        }


        private NodeBase<Node> m_selectedItem;
        public NodeBase<Node> SelectedItem
        {
            get
            {
                return m_selectedItem;
            }
            set
            {
                if(value != m_selectedItem)
                {
                    SetProperty(ref m_selectedItem, value);
                }

            }
        }


        public FileSharePaneViewModel()
            : base("ファイル共有")
        { }

    }
}

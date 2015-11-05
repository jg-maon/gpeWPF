using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class MainWindowViewModel : BindableBase
    {
        private string m_name;
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                this.SetProperty(ref this.m_name, value);
            }
        }

        private string m_message;
        public string Message
        {
            get
            {
                return m_message;
            }
            set
            {
                this.SetProperty(ref this.m_message, value);
            }
        }

        private DelegateCommand m_showMessageCommand;
        public DelegateCommand ShowMessageCommand
        {
            get
            {
                m_showMessageCommand = m_showMessageCommand ?? new DelegateCommand(_ShowMessage);
                return m_showMessageCommand;
            }
        }

        private void _ShowMessage()
        {
            this.Message = string.Format("こんにちは、 {0} さん", this.Name);
        }

        public MainWindowViewModel()
        {

        }
    }
}

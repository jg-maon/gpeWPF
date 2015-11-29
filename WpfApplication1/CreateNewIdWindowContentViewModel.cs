using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class CreateNewIdWindowContentViewModel : ViewModelBase, IInteractionRequestAware
    {
        static CreateNewIdWindowContentViewModel s_instance = new CreateNewIdWindowContentViewModel();
        public static CreateNewIdWindowContentViewModel Instance
        {
            get
            {
                return s_instance;
            }
        }
        public Action FinishInteraction
        {
            get;
            set;
        }

        private INotification _notification;
        public INotification Notification
        {
            get { return _notification; }
            set { _notification = value; RaisePropertyChanged("Notification"); }
        }
        private Confirmation Confirmation
        {
            get { return (Confirmation)this.Notification; }
        }

        private string m_category;
        public string Category 
        {
            get
            {
                return m_category;
            }
            set
            {
                SetProperty(ref m_category, value);
            }
        }


        public string InputComment { get; set; }
        public string InputName { get; set; }
        public int InputId { get; set; }


        public DelegateCommand OkCommand { get; set; }

        public void OnOk()
        {
            this.Confirmation.Confirmed = true;
            this.FinishInteraction.Invoke();
        }

        public DelegateCommand CancelCommand { get; set; }

        public void OnCancel()
        {
            this.Confirmation.Confirmed = false;
            this.FinishInteraction.Invoke();
        }


        protected CreateNewIdWindowContentViewModel()
        {
            this.OkCommand = new DelegateCommand(this.OnOk);
            this.CancelCommand = new DelegateCommand(this.OnCancel);

        }

    }
}

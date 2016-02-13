/************************************************************************

   AvalonDock

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the New BSD
   License (BSD) as published at http://avalondock.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up AvalonDock in Extended WPF Toolkit Plus at http://xceed.com/wpf_toolkit

   Stay informed: follow @datagrid on Twitter or Like facebook.com/datagrids

  **********************************************************************/

using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApplication1
{
    abstract class PaneViewModel : ViewModelBase
    {
        private bool m_doFloating = false;
        /// <summary>
        /// 初期化時に設定するとフロート状態で出現させられる
        /// </summary>
        public bool DoFloating
        {
            get
            {
                return m_doFloating;
            }
            set
            {
                SetProperty(ref m_doFloating, value);
            }
        }

        public PaneViewModel()
        { }

        public PaneViewModel(string title)
        {
            Title = title;
        }


        #region Title

        private string _title = null;
        public virtual string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    RaisePropertyChanged("Title");
                }
            }
        }

        #endregion

        public ImageSource IconSource
        {
            get;
            protected set;
        }

        #region ContentId

        private string _contentId = null;
        public string ContentId
        {
            get { return _contentId; }
            set
            {
                if (_contentId != value)
                {
                    _contentId = value;
                    RaisePropertyChanged("ContentId");
                }
            }
        }

        #endregion

        #region IsSelected

        private bool m_isSelected = false;
        public bool IsSelected
        {
            get { return m_isSelected; }
            set
            {
                if (SetProperty(ref m_isSelected, value))
                {
                }
            }
        }

        #endregion

        #region IsActive

        private bool _isActive = false;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    RaisePropertyChanged("IsActive");
                }
            }
        }

        #endregion

        private double m_floatingWidth = 100.0;
        public double FloatingWidth
        {
            get
            {
                return m_floatingWidth;
            }
            set
            {
                SetProperty(ref m_floatingWidth, value);
            }
        }

        private double m_floatingHeight = 100.0;
        public double FloatingHeight
        {
            get
            {
                return m_floatingHeight;
            }
            set
            {
                SetProperty(ref m_floatingHeight, value);
            }
        }

        private DelegateCommand m_closeCommand = null;
        public ICommand CloseCommand { get { return m_closeCommand ?? (m_closeCommand = new DelegateCommand(OnClose, DoesClose)); } }

        private bool m_canClose = true;
        public bool CanClose
        {
            get { return m_canClose; }
            set
            {
                SetProperty(ref m_canClose, value);
                OnPropertyChanged(() => CloseCommand);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool DoesClose()
        {
            return CanClose;
        }

        protected abstract void OnClose();


    }
}
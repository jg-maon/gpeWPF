/************************************************************************

   AvalonDock

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the New BSD
   License (BSD) as published at http://avalondock.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up AvalonDock in Extended WPF Toolkit Plus at http://xceed.com/wpf_toolkit

   Stay informed: follow @datagrid on Twitter or Like facebook.com/datagrids

  **********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.Prism.Mvvm;
using System.Runtime.Serialization;

namespace WpfApplication1
{
    [Serializable]
    public class ViewModelBase : BindableBase, ISerializable
    {

        public ViewModelBase()
        { }

        public ViewModelBase(SerializationInfo info, StreamingContext context)
        {

        }

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }



        #region ISerializable メンバー

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }

        #endregion
    }
}

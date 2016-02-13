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
using Xceed.Wpf.AvalonDock.Layout;

namespace WpfApplication1
{
    class LayoutInitializer : ILayoutUpdateStrategy
    {
        public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
        {
            //AD wants to add the anchorable into destinationContainer
            //just for test provide a new anchorablepane 
            //if the pane is floating let the manager go ahead
            //LayoutAnchorablePane destPane = destinationContainer as LayoutAnchorablePane;
            if (destinationContainer != null &&
                destinationContainer.FindParent<LayoutFloatingWindow>() != null)
                return false;


            var content = anchorableToShow.Content;
            string destPaneName = "";
            if(content is IdInfoTablePane2ViewModel)
            {
                destPaneName = "IdInfoTablePane";
                ////layout.BottomSide.InsertChildAt(0, anchorableToShow);
                //layout.BottomSide.Children[0].Children.Add(anchorableToShow);
                //return true;
            }
            else if(content is CategoryTreePaneViewModel)
            {
                destPaneName = "CategoryTreePane";
            }
            else if (content is WpfApplication1.Workspace.ParameterFileTreePaneViewModel)
            {
                destPaneName = "ParameterFileTreePane";
            }
            else if(content is FileSharePaneViewModel)
            {
                destPaneName = "FileSharePane";
            }
            else if(content is ParameterTab2ViewModel)
            {
                destPaneName = "ParameterTabPane";
            }
            else
            { return false; }

            foreach(var pane in layout.Descendents().OfType<LayoutAnchorablePane>().Where(p=>p.ChildrenCount  >= 1))
            {
                for (int i = 0; i < pane.ChildrenCount; ++i)
                {
                    var child = pane.Children[i];
                    if (child.Content == null)
                    {
                        child.IsVisible = false;
                        --i;
                    }
                }
            }

            var toolsPane = layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault(d => d.Name == destPaneName);
            if (toolsPane != null)
            {
                toolsPane.Children.Add(anchorableToShow);
                return true;
            }

            return false;

        }


        public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown)
        {
            var content = anchorableShown.Content as PaneViewModel;
            if(content != null && content.DoFloating)
            {
                anchorableShown.Float();
            }
        }


        public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument anchorableToShow, ILayoutContainer destinationContainer)
        {
            return false;
        }

        public void AfterInsertDocument(LayoutRoot layout, LayoutDocument anchorableShown)
        {

        }
    }
}

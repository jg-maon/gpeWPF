using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfApplication1.Controls;

namespace WpfApplication1
{
    class TreeViewItemBehaviors
    {


        public static bool GetIsEnableDragDropMove(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnableDragDropMoveProperty);
        }

        public static void SetIsEnableDragDropMove(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnableDragDropMoveProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsEnableDragDropMove.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEnableDragDropMoveProperty =
            DependencyProperty.RegisterAttached("IsEnableDragDropMove", typeof(bool), typeof(TreeViewItemBehaviors), new PropertyMetadata(false,_OnDragDropMoveEnableChanged));



        private static void _OnDragDropMoveEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = d as FrameworkElement;

            if((bool)e.NewValue)
            {
                // 有効化
                item.PreviewMouseDown += treeViewItem_PreviewMouseDown;
                item.MouseMove += treeViewItem_MouseMove;
                item.Drop += treeViewItem_Drop;
                //item.DragEnter += item_DragEnter;
                item.QueryContinueDrag += item_QueryContinueDrag;
                //item.DragOver += item_DragOver;
            }
            else
            {
                // 無効化
                item.PreviewMouseDown -= treeViewItem_PreviewMouseDown;
                item.MouseMove -= treeViewItem_MouseMove;
                item.Drop -= treeViewItem_Drop;
                item.QueryContinueDrag -= item_QueryContinueDrag;

            }
        }

        static void item_DragOver(object sender, DragEventArgs e)
        {
        }

        static void item_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            //マウスの右ボタンが押されていればドラッグをキャンセル
            //"2"はマウスの右ボタンを表す
            if (e.KeyStates == DragDropKeyStates.RightMouseButton)
                e.Action = DragAction.Cancel;
        }

        static void item_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }

        private static void treeViewItem_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) { return; }
            //this.lastLeftMouseButtonDownPoint = e.GetPosition(this.treeView);
        }

        /// <summary>
        /// TreeView のマウス移動時処理<br />
        /// クリックされながら特定距離以上移動したらドラッグドロップを開始する。
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">引数</param>
        private static void treeViewItem_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released) { return; }

            //var currentPosition = e.GetPosition(this.treeView);
            //if (Math.Abs(currentPosition.X - this.lastLeftMouseButtonDownPoint.X) <= minimumDragDistance &&
            //    Math.Abs(currentPosition.Y - this.lastLeftMouseButtonDownPoint.Y) <= minimumDragDistance)
            //{
            //    return;
            //}
            //var holder = new TreeItemViewModelHolder(this.treeView.SelectedItem as TreeItemViewModelBase);
            TreeViewItem item = sender as TreeViewItem;
            if (item == null)
            { return; }
            var s = e.Source;
            var el = item.InputHitTest(e.GetPosition(s as FrameworkElement)) as FrameworkElement;
            if(el == null)
            { return; }
            DragDrop.DoDragDrop(el, item, DragDropEffects.Move);
        }

        /// <summary>
        /// TreeView の要素にドロップされた時の処理<br />
        /// 要素の配置を変更する。
        /// </summary>
        /// <param name="sender">発生元</param>
        /// <param name="e">引数</param>
        private static void treeViewItem_Drop(object sender, DragEventArgs e)
        {
            e.Handled = true;
            e.Effects = DragDropEffects.None;

            var srcHolder = e.Data.GetData(typeof(TreeViewItem)) as TreeViewItem;
            if (srcHolder == null) { return; }
            var srcVM = srcHolder.DataContext as NodeBase<Workspace.NodeContent>;
            


            FrameworkElement destItem = sender as TreeViewItem;
            NodeBase<Workspace.NodeContent> destNode, destGroup;

            if (destItem != null)
            {
                destNode = destItem.DataContext as NodeBase<Workspace.NodeContent>;
                destGroup = destNode;
                if (destNode.Value.IsFileNode)
                {
                    destGroup = destNode.Parent;
                }
            }
            else
            {
                // ルートノードに移動
                destItem = sender as TreeView;
                if(destItem == null)
                {return;}
                //destNode = ((Workspace.ParameterFileTreePaneViewModel)destItem.DataContext).RootNode;
                destNode = srcVM.Tree.RootNode;
                destGroup = destNode;
            }

            if(destNode != null)
            {
                if (srcVM == destNode) { return; }
                //var destVM = destItem.Header as TreeItemViewModelBase;


                // 追加先のグループが自身の親グループの場合、キャンセル
                if(destGroup == srcVM.Parent)
                { return; }

                // #TODO: 追加先が自身の子グループだった場合キャンセル

                // 自身の削除
                srcVM.Parent.Nodes.Remove(srcVM);

                destGroup.Add(srcVM.Value);


            }
        }


        

        public static Brush GetDragOveredBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(DragOveredBrushProperty);
        }

        public static void SetDragOveredBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(DragOveredBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for DragOveredBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DragOveredBrushProperty =
            DependencyProperty.RegisterAttached("DragOveredBrush", typeof(Brush), typeof(TreeViewItemBehaviors), new PropertyMetadata(null, _OnDragOveredBrushChanged));

        private static void _OnDragOveredBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var item = d as FrameworkElement;
            
            if(e.NewValue != null)
            {
                item.PreviewDragEnter += item_PreviewDragEnter;
                item.PreviewDragLeave += item_PreviewDragLeave;
                item.PreviewDrop += item_PreviewDrop;
            }
            else
            {
                item.PreviewDragEnter -= item_PreviewDragEnter;
                item.PreviewDragLeave -= item_PreviewDragLeave;
                item.PreviewDrop -= item_PreviewDrop;
            }
        }

        static void item_PreviewDrop(object sender, DragEventArgs e)
        {
            var el = sender as Control;
            var normalBrush = GetNormalBackground(el);
            if (normalBrush != null)
            {
                el.Background = normalBrush;
            }
        }





        public static Brush GetNormalBackground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(NormalBackgroundProperty);
        }

        public static void SetNormalBackground(DependencyObject obj, Brush value)
        {
            obj.SetValue(NormalBackgroundProperty, value);
        }

        // Using a DependencyProperty as the backing store for NormalBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NormalBackgroundProperty =
            DependencyProperty.RegisterAttached("NormalBackground", typeof(Brush), typeof(TreeViewItemBehaviors), new PropertyMetadata(null));

        

        static void item_PreviewDragEnter(object sender, DragEventArgs e)
        {
            var el = sender as Control;
    
            Brush brush = GetDragOveredBrush(el);
            if (brush != null)
            {
                el.Background = brush;
            }
        }

        static void item_PreviewDragLeave(object sender, DragEventArgs e)
        {

            var el = sender as Control;
            var normalBrush = GetNormalBackground(el);
            if (normalBrush != null)
            {
                el.Background = normalBrush;
            }
        }







        


    }
}

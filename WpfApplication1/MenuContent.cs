using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class MenuContent : ViewModelBase
    {

        public string MenuItemEdit { get { return Properties.Resources.MenuItemEdit; } }

        // アンドゥ(元に戻す)
        public string MenuItemUndo { get { return Properties.Resources.MenuItemUndo; } }
        // リドゥ(やり直し)
        public string MenuItemRedo { get { return Properties.Resources.MenuItemRedo; } }
        // IDのコピー
        public string MenuItemCopyId { get { return Properties.Resources.MenuItemCopyId; } }
        // パラメータのコピー
        public string MenuItemCopyParameter { get { return Properties.Resources.MenuItemCopyParameter; } }
        // 貼り付け
        private string m_menuItemPaste = Properties.Resources.MenuItemPaste;
        public string MenuItemPaste
        {
            get
            {
                return m_menuItemPaste;
            }
            set
            {
                SetProperty(ref m_menuItemPaste, value);
            }
        }
        // 新規IDの作成
        public string MenuItemNewId { get { return Properties.Resources.MenuItemNewId; } }
        public string MenuItemCreateNewId { get { return Properties.Resources.MenuItemNewId; } }
        // インスタンスコピーの作成
        public string MenuItemInstanceCopy { get { return Properties.Resources.MenuItemInstanceCopy; } }
        // 編集の取り消し
        public string MenuItemCancelEdit { get { return Properties.Resources.MenuItemCancelEdit; } }
        // IDの削除
        public string MenuItemDeleteId { get { return Properties.Resources.MenuItemDeleteId; } }
        // ID簡易情報の編集
        public string MenuItemEditIdInfo { get { return Properties.Resources.MenuItemEditIdInfo; } }
        // 検索
        public string MenuItemFind { get { return Properties.Resources.MenuItemFind; } }

        public string MenuItemView { get { return Properties.Resources.MenuItemView; } }
        // グループの一斉展開
        public string MenuItemExpandAll { get { return Properties.Resources.MenuItemExpandAll; } }
        // グループの一斉折りたたみ
        public string MenuItemCollapseAll { get { return Properties.Resources.MenuItemCollapseAll; } }
        // ファイルツリーの表示
        public string MenuItemFileTreePane { get { return Properties.Resources.MenuItemFileTreePane; } }
        // 実機接続タブの表示
        public string MenuItemConnectionPane { get { return Properties.Resources.MenuItemConnectionPane; } }


        public string MenuItemIdInfoTable { get { return Properties.Resources.MenuItemIdInfoTable; } }
        // ID詳細＞ID詳細タブの表示
        public string MenuItemIdInfoTablePane { get { return Properties.Resources.MenuItemIdInfoTablePane; } }
        // ID詳細＞項目の表示
        public string MenuItemIdInfoTableColumns { get { return Properties.Resources.MenuItemIdInfoTableColumns; } }
        // ファイル共有タブの表示
        public string MenuItemFileSharePane { get { return Properties.Resources.MenuItemFileSharePane; } }


        public string MenuItemFileShare { get { return Properties.Resources.MenuItemFileShare; } }
        // 編集中のファイルのインポート
        public string MenuItemImport { get { return Properties.Resources.MenuItemImport; } }
        // 編集中のファイルのチェックイン
        public string MenuItemCheckIn { get { return Properties.Resources.MenuItemCheckIn; } }
        // 編集中のファイルのチェックアウト取り消し
        public string MenuItemCancelCheckOut { get { return Properties.Resources.MenuItemCancelCheckOut; } }
        // 選択中のファイルのサーバーバージョン取得
        public string MenuItemAcquisition { get { return Properties.Resources.MenuItemAcquisition; } }
        // チェックアウト中のファイルを全てチェックイン
        public string MenuItemCheckInAll { get { return Properties.Resources.MenuItemCheckInAll; } }
        // 全てのチェックアウト
        public string MenuItemCheckOutAll { get { return Properties.Resources.MenuItemCheckOutAll; } }
        // 全てのチェックアウト取り消し
        public string MenuItemCancelCheckOutAll { get { return Properties.Resources.MenuItemCancelCheckOutAll; } }
        // 全てのサーバーバージョン取得
        public string MenuItemAcquisitionAll { get { return Properties.Resources.MenuItemAcquisitionAll; } }
        // 状態の更新
        public string MenuItemUpdate { get { return Properties.Resources.MenuItemUpdate; } }

        public string MenuItemSettings { get { return Properties.Resources.MenuItemSettings; } }
        // ファイル共有設定
        public string MenuItemSettingFileShare { get { return Properties.Resources.MenuItemSettingFileShare; } }
        // スタイル設定
        public string MenuItemSettingStyle { get { return Properties.Resources.MenuItemSettingStyle; } }
        // オプション-フィルタ設定
        public string MenuItemOption { get { return Properties.Resources.MenuItemOption; } }

        public string MenuItemHelp { get { return Properties.Resources.MenuItemHelp; } }
        // ヘルプドキュメントの表示
        public string MenuItemHelpDocument { get { return Properties.Resources.MenuItemHelpDocument; } }
        // バージョン情報
        public string MenuItemVersionInfo { get { return Properties.Resources.MenuItemVersionInfo; } }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    /// <summary>
    /// アンドゥ・リドゥコマンドマネージャ
    /// </summary>
    /// <remarks>
    /// コンストラクタで保存するスタックのサイズを決め、
    /// アンドゥ用スタックが決めたサイズ以下の場合に処理を追加できる
    /// サイズ以上の場合は、最初に追加したものからなくしていく
    /// </remarks>
    public sealed class UndoRedoManager
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="maxSize">保存するスタックのサイズ(デフォルト:uint.MaxValue)</param>
        public UndoRedoManager(uint maxSize = uint.MaxValue)
        {
            m_maxSize = maxSize;
        }

        /// <summary>
        /// アンドゥ可能か
        /// </summary>
        public bool CanUndo { get { return m_undoStack.Count >= 1; } }
        /// <summary>
        /// リドゥ可能か
        /// </summary>
        public bool CanRedo { get { return m_redoStack.Count >= 1; } }

        /// <summary>
        /// 保存するスタックサイズ
        /// </summary>
        private uint m_maxSize = 0;
        /// <summary>
        /// アンドゥ用スタック
        /// </summary>
        private readonly List<IUndoRedoCommand> m_undoStack = new List<IUndoRedoCommand>();
        /// <summary>
        /// リドゥ用スタック
        /// </summary>
        private readonly List<IUndoRedoCommand> m_redoStack = new List<IUndoRedoCommand>();

        /// <summary>
        /// push_back
        /// </summary>
        /// <param name="list"></param>
        /// <param name="command"></param>
        private static void _Push(List<IUndoRedoCommand> list, IUndoRedoCommand command)
        {
            // 最後にコマンドを追加
            list.Add(command);
        }
        /// <summary>
        /// push_back, pop_front
        /// </summary>
        /// <param name="list"></param>
        /// <param name="command"></param>
        private static void _PushAndPopFront(List<IUndoRedoCommand> list, IUndoRedoCommand command)
        {
            // 最後にコマンドを追加
            list.Add(command);
            // 最初のコマンドを削除
            list.Remove(list.First());
        }

        /// <summary>
        /// Back, pop_back
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static IUndoRedoCommand _Pop(List<IUndoRedoCommand> list)
        {
            // 最後に追加したコマンドの取得
            var command = list.Last();
            // コマンドをリストから削除
            list.Remove(command);

            return command;
        }


        /// <summary>
        /// Do処理　コマンドの実行
        /// </summary>
        /// <param name="command">実行するコマンド</param>
        /// <returns>
        /// コマンドを実行したか
        /// false   : アンドゥ用スタックがいっぱい
        /// </returns>
        public bool Invoke(IUndoRedoCommand command)
        {
            bool ret = true;
            // アンドゥスタックのサイズチェック
            if(m_undoStack.Count >= m_maxSize)
            {
                // 最初の要素を削除して追加
                _PushAndPopFront(m_undoStack, command);
                ret = false;
            }
            else
            {
                // アンドゥ用スタックに実行したコマンドを積んでおく
                _Push(m_undoStack, command);
            }
            // コマンドの実行
            command.Invoke();
            
            // リドゥ用スタックのクリア
            m_redoStack.Clear();

            return ret;
        }

        /// <summary>
        /// 元に戻す
        /// </summary>
        public void Undo()
        {
            if (!CanUndo)
            {
                return;
            }
            // アンドゥの実行
            var command = _Pop(m_undoStack);
            command.Undo();

            // 実行したコマンドをリドゥ用スタックに積む
            _Push(m_redoStack, command);

        }

        /// <summary>
        /// やり直し
        /// </summary>
        public void Redo()
        {
            if(!CanRedo)
            {
                return;
            }

            // リドゥの実行
            var command = _Pop(m_redoStack);
            command.Redo();

            // 実行したコマンドをアンドゥ用スタックに積む
            _Push(m_undoStack, command);
        }

        /// <summary>
        /// スタックのクリア
        /// </summary>
        public void Refresh()
        {
            m_undoStack.Clear();
            m_redoStack.Clear();
        }
    }
}

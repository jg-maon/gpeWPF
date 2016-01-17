namespace WpfApplication1
{
    /// <summary>
    /// 状態保存
    /// </summary>
    /// <typeparam name="StateType">保存する状態の型</typeparam>
    /// <typeparam name="TargetType">反映対象のオブジェクトの型</typeparam>
    public abstract class Memento<StateType, TargetType>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="state"></param>
        public Memento(StateType state) { this.State = state; }
        /// <summary>
        /// 状態の設定と取得
        /// </summary>
        public StateType State { get; protected set; }
        /// <summary>
        /// 反映対象オブジェクトの取得と設定
        /// </summary>
        protected TargetType Target { get; set; }

        /// <summary>
        /// 状態の設定
        /// </summary>
        /// <param name="state"></param>
        public abstract void SetMemento(StateType state);
    }


    /// <summary>
    /// Mementoオブジェクト用コマンド
    /// </summary>
    /// <typeparam name="StateType">Mementoオブジェクトに保存する型</typeparam>
    /// <typeparam name="TargetType">Mementoオブジェクトの反映対象の型</typeparam>
    public sealed class MementoCommand<StateType, TargetType> : IUndoRedoCommand
    {
        private Memento<StateType, TargetType> m_memento = null;
        private StateType m_prev;
        private StateType m_next;

        public MementoCommand(Memento<StateType, TargetType> prev, Memento<StateType, TargetType> next)
        {
            m_memento = prev;
            m_prev = prev.State;
            m_next = next.State;
        }

        #region IUndoRedoCommand メンバー

        public void Invoke()
        {
            m_prev = m_memento.State;
            m_memento.SetMemento(m_next);
        }

        public void Undo()
        {
            m_memento.SetMemento(m_prev);
        }

        public void Redo()
        {
            m_memento.SetMemento(m_next);
        }

        #endregion
    }
}

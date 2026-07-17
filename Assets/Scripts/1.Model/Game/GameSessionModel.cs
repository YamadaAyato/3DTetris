using System;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///    1回のゲーム進行中の状態を保持するモデル。
    /// </summary>
    public class GameSessionModel
    {
        /// <summary> 現在操作中のアクティブなピース。 </summary>
        public ActivePiece CurrentPiece { get; private set; }

        /// <summary> 現在操作中のアクティブなピースが存在するか。 </summary>
        public bool HasCurrentPiece => CurrentPiece != null;

        /// <summary> ゲームオーバー状態かどうか。 </summary>
        public bool IsGameOver { get; private set; }

        /// <summary>
        ///     現在操作中のアクティブなピースを設定する。
        /// </summary>
        /// <param name="piece"> 設定するアクティブなピース </param>
        public void SetCurrentPiece(ActivePiece piece)
        {
            CurrentPiece = piece ?? throw new ArgumentNullException(nameof(piece));
        }

        /// <summary>
        ///     現在操作中のアクティブなピースをクリアする。
        /// </summary>
        public void ClearCurrentPiece() => CurrentPiece = null;

        /// <summary>
        ///     ゲームオーバー状態を設定する。
        /// </summary>
        public void SetGameOver() => IsGameOver = true;

        /// <summary>
        ///     ゲームセッションの状態をリセットする。
        /// </summary>
        public void Reset()
        {
            CurrentPiece = null;
            IsGameOver = false;
        }
    }
}

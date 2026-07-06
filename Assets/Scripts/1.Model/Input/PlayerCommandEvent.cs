using UnityEngine.InputSystem;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///    プレイヤーが入力したコマンドとその入力アクションのフェーズを表す構造体。
    /// </summary>
    public readonly struct PlayerCommandEvent
    {
        public PlayerCommandEvent(PlayerCommand command, InputActionPhase actionPhase)
        {
            Command = command;
            ActionPhase = actionPhase;
        }

        /// <summary> 実行されたゲーム内コマンドを取得します。 </summary>
        public PlayerCommand Command { get; }

        /// <summary> 入力アクションのフェーズを取得します。 </summary>
        public InputActionPhase ActionPhase { get; }
    }
}

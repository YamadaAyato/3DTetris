using ThreeDTetris.Model;
using UnityEngine;

namespace ThreeDTetris.View
{
    /// <summary>
    ///     Unity InputSystem の ActionMap名と Action名を管理し、ゲーム内操作へ変換する。
    /// </summary>
    internal static class InputActionMapNames
    {
        public const string GAMEPLAY_MAP_NAME = "Gameplay";

        public const string MOVE_LEFT = "MoveLeft";
        public const string MOVE_RIGHT = "MoveRight";
        public const string ROTATE_PIECE = "RotatePiece";
        public const string SOFT_DROP = "SoftDrop";
        public const string HARD_DROP = "HardDrop";
        public const string ROTATE_CONTAINER_LEFT = "RotateContainerLeft";
        public const string ROTATE_CONTAINER_RIGHT = "RotateContainerRight";
        public const string RESET_CAMERA = "ResetCamera";
        public const string PAUSE = "Pause";

        /// <summary>
        ///     InputSystem の Action名から対応するゲーム内操作を取得する。
        /// </summary>
        /// <param name="actionName"> Action名 </param>
        /// <param name="playerCommand"> 取得されたゲーム内操作 </param>
        /// <returns></returns>
        public static bool TryGetCommand(string actionName, out PlayerCommand playerCommand)
        {
            switch (actionName)
            {
                case MOVE_LEFT:
                    playerCommand = PlayerCommand.MoveLeft;
                    return true;

                case MOVE_RIGHT:
                    playerCommand = PlayerCommand.MoveRight;
                    return true;

                case ROTATE_PIECE:
                    playerCommand = PlayerCommand.Rotate;
                    return true;

                case SOFT_DROP:
                    playerCommand = PlayerCommand.SoftDrop;
                    return true;

                case HARD_DROP:
                    playerCommand = PlayerCommand.HardDrop;
                    return true;

                case ROTATE_CONTAINER_LEFT:
                    playerCommand = PlayerCommand.RotateContainerLeft;
                    return true;

                case ROTATE_CONTAINER_RIGHT:
                    playerCommand = PlayerCommand.RotateContainerRight;
                    return true;

                case RESET_CAMERA:
                    playerCommand = PlayerCommand.ResetCamera;
                    return true;

                case PAUSE:
                    playerCommand = PlayerCommand.Pause;
                    return true;

                default:
                    playerCommand = default;
                    return false;
            }

        }
    }
}

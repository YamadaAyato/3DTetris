namespace ThreeDTetris.Model
{
    /// <summary>
    ///     プレイヤーが入力するコマンドの種類を表す列挙型。
    /// </summary>
    public enum PlayerCommand
    {
        MoveLeft,
        MoveRight,
        Rotate,
        SoftDrop,
        HardDrop,
        RotateContainerLeft,
        RotateContainerRight,
        ResetCamera,
        Pause,
    }
}

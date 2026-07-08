namespace ThreeDTetris.Model
{
    /// <summary>
    ///    デフォルトの立方体盤面の面IDを定義するクラス。
    ///    初期開発で使う立方体盤面の面IDを定義するためのクラス。
    ///    将来的にデータ化等ををして面IDを定義する場合は、このクラスは不要になる。
    /// </summary>
    public static class DefaultCubeBoardFaceIds
    {
        public static readonly BoardFaceId Front = new BoardFaceId(0);

        public static readonly BoardFaceId Right = new BoardFaceId(1);

        public static readonly BoardFaceId Back = new BoardFaceId(2);

        public static readonly BoardFaceId Left = new BoardFaceId(3);
    }
}

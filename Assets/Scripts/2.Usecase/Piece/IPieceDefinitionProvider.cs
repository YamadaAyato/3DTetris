using ThreeDTetris.Model;

namespace ThreeDTetris.Usecase
{
    /// <summary>
    ///     次に生成されるピースの定義を提供するインターフェース。
    /// </summary>
    public interface IPieceDefinitionProvider
    {
        /// <summary>
        ///    次に生成されるピースの定義を取得する。
        /// </summary>
        /// <returns> 次に生成されるピースの定義 </returns>
        PieceDefinition GetNext();
    }
}

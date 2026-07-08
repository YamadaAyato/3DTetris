using System.Collections.Generic;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///     立方体盤面用位相構造を生成するクラス。
    /// </summary>
    public class DefaultCubeBoardTopologyFactory
    {
        /// <summary>
        ///     立方体盤面用位相構造を生成する
        /// </summary>
        /// <param name="dimensions"></param>
        /// <returns></returns>
        public static IBoardTopology Create(BoardDimensions dimensions)
        {
            BoardFaceId[] faceIds =
            {
                DefaultCubeBoardFaceIds.Front,
                DefaultCubeBoardFaceIds.Right,
                DefaultCubeBoardFaceIds.Back,
                DefaultCubeBoardFaceIds.Left
            };

            // 面とその面が持っている隣接情報を作成。
            Dictionary<BoardFaceId, BoardHorizontalNeighbors> neighborsByFaceId = new()
            {
                {
                    DefaultCubeBoardFaceIds.Front,
                    new BoardHorizontalNeighbors(DefaultCubeBoardFaceIds.Left, DefaultCubeBoardFaceIds.Right)
                },
                {
                    DefaultCubeBoardFaceIds.Right,
                    new BoardHorizontalNeighbors(DefaultCubeBoardFaceIds.Front, DefaultCubeBoardFaceIds.Back)
                },
                {
                    DefaultCubeBoardFaceIds.Back,
                    new BoardHorizontalNeighbors(DefaultCubeBoardFaceIds.Right, DefaultCubeBoardFaceIds.Left)
                },
                {
                    DefaultCubeBoardFaceIds.Left,
                    new BoardHorizontalNeighbors(DefaultCubeBoardFaceIds.Back, DefaultCubeBoardFaceIds.Front)
                }
            };

            BoardTopologyDefinition definition = new(
                dimensions,
                faceIds,
                neighborsByFaceId);

            return new DataDrivenBoardTopology(definition);
        }
    }
}

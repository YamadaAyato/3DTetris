using System;
using System.Collections.Generic;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///     プリズム形状のボードを作成するためのファクトリクラス。
    /// </summary>
    public class PrismBoardShapeFactory
    {
        /// <summary>
        ///     正方形の面を持つ立方体のボード形状を作成します。
        /// </summary>
        /// <param name="faceWidth"> 立方体の各面の幅 </param>
        /// <param name="faceHeight"> 立方体の各面の高さ </param>
        /// <returns> 作成された立方体のボード形状定義 </returns>
        public static BoardShapeDefinition CreateCube(int faceWidth, int faceHeight)
        {
            BoardDimensions dimensions = new BoardDimensions(faceWidth, faceHeight);

            return CreateLoopPrism(
                new[]
                {
                    dimensions,
                    dimensions,
                    dimensions,
                    dimensions
                });
        }

        /// <summary>
        ///     長方形の面を持つ直方体のボード形状を作成します。
        /// </summary>
        /// <param name="frontBackWidth"> 前後の面の幅 </param>
        /// <param name="sideWidth"> 側面の幅 </param>
        /// <param name="faceHeight"> 面の高さ </param>
        /// <returns> 作成された直方体のボード形状定義 </returns>
        public static BoardShapeDefinition CreateRectangularPrism(int frontBackWidth, int sideWidth, int faceHeight)
        {
            return CreateLoopPrism(
                new[]
                {
                    new BoardDimensions(frontBackWidth, faceHeight),
                    new BoardDimensions(sideWidth, faceHeight),
                    new BoardDimensions(frontBackWidth, faceHeight),
                    new BoardDimensions(sideWidth, faceHeight)
                });
        }

        /// <summary>
        ///     三角形の面を持つ三角柱のボード形状を作成します。
        /// </summary>
        /// <param name="firstWidth"> 三角形の最初の辺の幅 </param>
        /// <param name="secondWidth"> 三角形の2番目の辺の幅 </param>
        /// <param name="thirdWidth"> 三角形の3番目の辺の幅 </param>
        /// <param name="faceHeight"> 三角形の面の高さ </param>
        /// <returns> 作成された三角柱のボード形状定義 </returns>
        public static BoardShapeDefinition CreateTriangularPrism(int firstWidth, int secondWidth, int thirdWidth, int faceHeight)
        {
            return CreateLoopPrism(
                new[]
                {
                    new BoardDimensions(firstWidth, faceHeight),
                    new BoardDimensions(secondWidth, faceHeight),
                    new BoardDimensions(thirdWidth, faceHeight)
                });
        }

        /// <summary>
        ///     ループ状のプリズム形状を作成します。
        /// </summary>
        /// <param name="faceDimensions"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        private static BoardShapeDefinition CreateLoopPrism(IReadOnlyList<BoardDimensions> faceDimensions)
        {
            if (faceDimensions == null)
            {
                throw new ArgumentNullException(nameof(faceDimensions));
            }

            if (faceDimensions.Count < 3)
            {
                throw new ArgumentException("プリズム形状は少なくとも3つの面を持つ必要があります。", nameof(faceDimensions));
            }

            // それぞれの面に対して、BoardFaceIdを作成し、BoardDimensionsとBoardHorizontalNeighborsを設定します。
            var faceIds = new BoardFaceId[faceDimensions.Count];
            Dictionary<BoardFaceId, BoardDimensions> dimensionsByFaceId = new();
            Dictionary<BoardFaceId, BoardHorizontalNeighbors> horizontalNeighborsByFaceId = new();

            // 各面のBoardFaceIdを作成し、BoardDimensionsを設定
            for (int i = 0; i < faceDimensions.Count; i++)
            {
                var faceId = new BoardFaceId(i);
                faceIds[i] = faceId;
                dimensionsByFaceId.Add(faceId, faceDimensions[i]);
            }

            // 各面の左右の隣接面を設定
            for (int i = 0; i < faceIds.Length; i++)
            {
                var leftFaceId = faceIds[(i - 1 + faceIds.Length) % faceIds.Length];
                var rightFaceId = faceIds[(i + 1) % faceIds.Length];
                horizontalNeighborsByFaceId.Add(faceIds[i], new BoardHorizontalNeighbors(leftFaceId, rightFaceId));
            }

            return new BoardShapeDefinition(faceIds, dimensionsByFaceId, horizontalNeighborsByFaceId, faceIds[0]);
        }
    }
}

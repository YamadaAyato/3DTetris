using System;
using System.Collections.Generic;
using ThreeDTetris.Model;

namespace ThreeDTetris.Usecase
{
    /// <summary>
    ///     完成したラインの削除と、削除後のブロックの落下を処理するユースケース。
    /// </summary>
    public class LineClearUsecase
    {
        public LineClearUsecase(BoardModel boardModel, IBoardTopology boardTopology)
        {
            _boardModel = boardModel ?? throw new ArgumentNullException(nameof(boardModel));
            _boardTopology = boardTopology ?? throw new ArgumentNullException(nameof(boardTopology));
        }

        /// <summary>
        ///     完成したラインを削除し、削除後のブロックの落下を処理する。
        /// </summary>
        /// <returns> 削除されたラインの情報と、落下したブロックの情報を含む結果オブジェクト </returns>
        public LineClearResult ClearCompletedLines()
        {
            HashSet<BoardCellPosition> removedPositions = new();
            Dictionary<BoardFaceId, List<int>> clearedYsByFace = new();
            HashSet<FaceLinePairKey> checkPairKeys = new();

            FindCompletedLines(removedPositions, clearedYsByFace, checkPairKeys);

            if (removedPositions.Count == 0)
            {
                return new LineClearResult(
                    removedPositions: Array.Empty<BoardCellPosition>(),
                    movedBlocks: Array.Empty<BoardBlockMove>()
                    );
            }

            BoardCellPosition[] removedPositionsArray = new BoardCellPosition[removedPositions.Count];
            removedPositions.CopyTo(removedPositionsArray);

            for (int i = 0; i < removedPositionsArray.Length; i++)
            {
                _boardModel.RemoveBlock(removedPositionsArray[i]);
            }

            BoardBlockMove[] moves = CreateCollapseMoves(clearedYsByFace);
            ApplyMovesToBoard(moves);

            return new LineClearResult(removedPositionsArray, moves);
        }

        private readonly BoardModel _boardModel;
        private readonly IBoardTopology _boardTopology;

        /// <summary>
        ///     完成している2つの面のラインを見つけ、削除する。
        /// </summary>
        /// <param name="removedPositions"> 削除されたブロックの位置を格納する集合 </param>
        /// <param name="clearedYsByFace"> 各面ごとの削除されたラインのY座標を格納する辞書 </param>
        /// <param name="checkPairKeys"> 既にチェックした面のペアとラインの組み合わせを格納する集合 </param>
        private void FindCompletedLines(
            HashSet<BoardCellPosition> removedPositions,
            Dictionary<BoardFaceId, List<int>> clearedYsByFace,
            HashSet<FaceLinePairKey> checkPairKeys)
        {
            for (int faceIndex = 0; faceIndex < _boardTopology.FaceIds.Count; faceIndex++)
            {
                BoardFaceId faceId = _boardTopology.FaceIds[faceIndex];
                BoardFaceId rightFaceId = _boardTopology.GetHorizontalNeighbors(faceId).RightFaceId;

                int sharedHeight = Math.Min(
                    _boardTopology.GetFaceHeight(faceId),
                    _boardTopology.GetFaceHeight(rightFaceId));

                for (int y = 0; y < sharedHeight; y++)
                {
                    FaceLinePairKey pairKey = new FaceLinePairKey(faceId, rightFaceId, y);

                    if (!checkPairKeys.Add(pairKey))
                    {
                        continue;
                    }

                    if (!IsLineFull(faceId, y) || !IsLineFull(rightFaceId, y))
                    {
                        continue;
                    }

                    AddClearedLine(removedPositions, clearedYsByFace, faceId, y);
                    AddClearedLine(removedPositions, clearedYsByFace, rightFaceId, y);
                }
            }
        }

        /// <summary>
        ///     指定された面の指定されたY座標のラインが完成しているかどうかを判定する。
        /// </summary>
        /// <param name="faceId"> 判定対象の面のID </param>
        /// <param name="y"> 判定対象のY座標 </param>
        /// <returns> ラインが完成している場合はtrue、そうでない場合はfalse </returns>
        private bool IsLineFull(BoardFaceId faceId, int y)
        {
            int width = _boardTopology.GetFaceWidth(faceId);

            for (int x = 0; x < width; x++)
            {
                BoardCellPosition position = new BoardCellPosition(faceId, x, y);
                if (!_boardModel.IsOccupied(position))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     削除対象ラインを登録する。
        /// </summary>
        /// <param name="removedPositions"> 削除されたブロックの位置を格納する集合 </param>
        /// <param name="clearedYsByFace"> 各面ごとの削除されたラインのY座標を格納する辞書 </param>
        /// <param name="faceId"> 対象の面のID </param>
        /// <param name="y"> 対象のY座標 </param>
        private void AddClearedLine(
            HashSet<BoardCellPosition> removedPositions,
            Dictionary<BoardFaceId, List<int>> clearedYsByFace,
            BoardFaceId faceId,
            int y)
        {
            if (!clearedYsByFace.TryGetValue(faceId, out List<int> clearedYs))
            {
                clearedYs = new List<int>();
                clearedYsByFace.Add(faceId, clearedYs);
            }

            if (!clearedYs.Contains(y))
            {
                clearedYs.Add(y);
            }

            int faceWidth = _boardTopology.GetFaceWidth(faceId);

            for (int x = 0; x < faceWidth; x++)
            {
                BoardCellPosition position = new BoardCellPosition(faceId, x, y);
                removedPositions.Add(position);
            }
        }

        /// <summary>
        ///     指定された移動をボードに適用する。
        /// </summary>
        /// <param name="moves"> 適用する移動のリスト </param>
        private void ApplyMovesToBoard(IReadOnlyList<BoardBlockMove> moves)
        {
            if (moves.Count == 0)
            {
                return;
            }

            BoardCellPosition[] toPositions = new BoardCellPosition[moves.Count];

            for (int i = 0; i < toPositions.Length; i++)
            {
                _boardModel.RemoveBlock(moves[i].From);
                toPositions[i] = moves[i].To;
            }

            _boardModel.PlaceBlocks(toPositions);
        }

        ///// <summary>
        /////     指定された面の右隣の面のIDを取得する。
        ///// </summary>
        ///// <param name="faceId"> 対象の面のID </param>
        ///// <returns> 右隣の面のID </returns>
        //private BoardFaceId GetRightFaceId(BoardFaceId faceId)
        //{
        //    BoardCellPosition rightEdgePosition = new BoardCellPosition(
        //        faceId,
        //        _boardTopology.Dimensions.FaceWidth - 1,
        //        0);

        //    return _boardTopology.GetRight(rightEdgePosition).FaceId;
        //}

        /// <summary>
        ///      削除されたラインの下にあるブロックを落下させるための移動を作成する。
        /// </summary>
        /// <param name="clearedYsByFace"> 各面ごとの削除されたラインのY座標を格納する辞書 </param>
        /// <returns> 作成されたブロックの移動の配列 </returns>
        private BoardBlockMove[] CreateCollapseMoves(Dictionary<BoardFaceId, List<int>> clearedYsByFace)
        {
            IReadOnlyList<BoardCellPosition> occupiedPos = _boardModel.GetOccupiedPositions();
            List<BoardBlockMove> moves = new();

            for (int i = 0; i < occupiedPos.Count; i++)
            {
                BoardCellPosition pos = occupiedPos[i];

                if (!clearedYsByFace.TryGetValue(pos.FaceId, out List<int> clearedYs))
                {
                    continue;
                }

                int downCount = CountClearedLinesBelow(pos.Y, clearedYs);

                if (downCount == 0)
                {
                    continue;
                }

                BoardCellPosition to = new BoardCellPosition(
                    pos.FaceId,
                    pos.X,
                    pos.Y - downCount);

                moves.Add(new BoardBlockMove(pos, to));
            }

            return moves.ToArray();
        }

        /// <summary>
        ///     指定されたY座標の下にある削除されたラインの数を数える。
        /// </summary>
        /// <param name="y"> 対象のY座標 </param>
        /// <param name="clearedYs"> 削除されたラインのY座標のリスト </param>
        /// <returns> 指定されたY座標の下にある削除されたラインの数 </returns>
        private static int CountClearedLinesBelow(int y, List<int> clearedYs)
        {
            int count = 0;

            for (int i = 0; i < clearedYs.Count; i++)
            {
                if (clearedYs[i] < y)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        ///     同じ面ペアとY座標の組み合わせを表すキー。
        /// </summary>
        private readonly struct FaceLinePairKey : IEquatable<FaceLinePairKey>
        {
            public FaceLinePairKey(BoardFaceId firstFaceId, BoardFaceId secondFaceId, int y)
            {
                if (firstFaceId.Value <= secondFaceId.Value)
                {
                    FirstFaceValue = firstFaceId.Value;
                    SecondFaceValue = secondFaceId.Value;

                }
                else
                {
                    FirstFaceValue = secondFaceId.Value;
                    SecondFaceValue = firstFaceId.Value;
                }

                Y = y;
            }

            private int FirstFaceValue { get; }
            private int SecondFaceValue { get; }
            private int Y { get; }

            public bool Equals(FaceLinePairKey other)
            {
                return FirstFaceValue == other.FirstFaceValue &&
                    SecondFaceValue == other.SecondFaceValue &&
                          Y == other.Y;
            }

            public override bool Equals(object obj)
            {
                return obj is FaceLinePairKey other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(FirstFaceValue, SecondFaceValue, Y);
            }
        }
    }
}

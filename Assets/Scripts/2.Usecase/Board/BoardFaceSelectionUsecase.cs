using System;
using ThreeDTetris.Model;

namespace ThreeDTetris.Usecase
{
    /// <summary>
    ///     盤面の面の選択を管理するユースケース。
    /// </summary>
    public class BoardFaceSelectionUsecase
    {
        public BoardFaceSelectionUsecase(BoardControlStateModel boardControlStateModel, IBoardTopology boardTopology)
        {
            _boardControlStateModel = boardControlStateModel ?? throw new ArgumentNullException(nameof(boardControlStateModel));
            _boardTopology = boardTopology ?? throw new ArgumentNullException(nameof(boardTopology));
        }

        /// <summary>
        ///     現在の面を左に回転させる。
        /// </summary>
        /// <returns> 回転後の面ID </returns>
        public BoardFaceId RotationLeft()
        {
            BoardFaceId currentFaceId = _boardControlStateModel.CurrentFaceId;
            BoardFaceId nextFaceId = _boardTopology.GetHorizontalNeighbors(currentFaceId).LeftFaceId;

            _boardControlStateModel.SetCurrentFaceId(nextFaceId);
            return nextFaceId;
        }

        /// <summary>
        ///     現在の面を右に回転させる。
        /// </summary>
        /// <returns> 回転後の面ID </returns>
        public BoardFaceId RotationRight()
        {
            BoardFaceId currentFaceId = _boardControlStateModel.CurrentFaceId;
            BoardFaceId nextFaceId = _boardTopology.GetHorizontalNeighbors(currentFaceId).RightFaceId;

            _boardControlStateModel.SetCurrentFaceId(nextFaceId);
            return nextFaceId;
        }

        /// <summary>
        ///     現在の面を指定された面に設定する。
        /// </summary>
        /// <param name="faceId">設定する面ID</param>
        public void SetCurrentFace(BoardFaceId faceId)
        {
            if (!_boardTopology.ContainsFace(faceId))
            {
                throw new InvalidOperationException($"面ID {faceId} は存在しません。");
            }

            _boardControlStateModel.SetCurrentFaceId(faceId);
        }

        private readonly BoardControlStateModel _boardControlStateModel;
        private readonly IBoardTopology _boardTopology;
    }
}

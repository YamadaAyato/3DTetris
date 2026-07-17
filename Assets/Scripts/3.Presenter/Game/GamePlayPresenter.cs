using System;
using System.Collections.Generic;
using ThreeDTetris.Model;
using ThreeDTetris.Usecase;

namespace ThreeDTetris.Presenter
{
    /// <summary>
    ///     ゲームプレイの進行を管理するプレゼンター。
    /// </summary>
    public class GamePlayPresenter
    {
        public GamePlayPresenter(
            GameSessionModel sessionModel,
            GamePlayUsecase gamePlayUsecase,
            IPieceDefinitionProvider pieceDefinitionProvider,
            RandomSpawnFaceProvider spawnFaceProvider,
            PieceSpawnSettings spawnSettings,
            PieceFallSettings fallSettings,
            PiecePositionResolver positionResolver,
            IGameBoardView gameBoardView,
            LineClearUsecase lineClearUsecase,
            PieceFaceSwitchUsecase faceSwitchUsecase,
            BoardFaceSelectionUsecase boardFaceSelectionUsecase,
            IBoardCameraView boardCameraView,
            BoardControlStateModel boardControlStateModel)
        {
            _sessionModel = sessionModel ?? throw new ArgumentNullException(nameof(sessionModel));
            _gamePlayUsecase = gamePlayUsecase ?? throw new ArgumentNullException(nameof(gamePlayUsecase));
            _pieceDefinitionProvider = pieceDefinitionProvider ?? throw new ArgumentNullException(nameof(pieceDefinitionProvider));
            _spawnFaceProvider = spawnFaceProvider ?? throw new ArgumentNullException(nameof(spawnFaceProvider));
            _spawnSettings = spawnSettings;
            _fallSettings = fallSettings;
            _positionResolver = positionResolver ?? throw new ArgumentNullException(nameof(positionResolver));
            _gameBoardView = gameBoardView ?? throw new ArgumentNullException(nameof(gameBoardView));
            _lineClearUsecase = lineClearUsecase ?? throw new ArgumentNullException(nameof(lineClearUsecase));
            _pieceFaceSwitchUsecase = faceSwitchUsecase ?? throw new ArgumentNullException(nameof(faceSwitchUsecase));
            _boardFaceSelectionUsecase = boardFaceSelectionUsecase ?? throw new ArgumentNullException(nameof(boardFaceSelectionUsecase));
            _boardCameraView = boardCameraView ?? throw new ArgumentNullException(nameof(boardCameraView));
            _boardControlStateModel = boardControlStateModel ?? throw new ArgumentNullException(nameof(boardControlStateModel));
        }

        /// <summary>
        ///     ゲームを開始する。
        /// </summary>
        public void StartGame()
        {
            TrySpawnNewPiece();
            RefreshActivePieceView();
        }

        /// <summary>
        ///     ゲームの進行を更新する。
        /// </summary>
        /// <param name="deltaTime"> 経過時間（秒） </param>
        public void Tick(float deltaTime)
        {
            if (_sessionModel.IsGameOver)
            {
                _gameBoardView.ClearActivePiece();
                return;
            }

            _fallElapsedSeconds += deltaTime;

            // 一定時間経過したら、ピースを1段落下させる
            if (_fallElapsedSeconds >= _fallSettings.FallIntervalSeconds)
            {
                ExecutePlayerCommand(PlayerCommand.SoftDrop);
                _fallElapsedSeconds = 0f;
            }
        }

        /// <summary>
        ///     入力アクションを実行する。
        /// </summary>
        /// <param name="actionName"> 実行するアクションの名前 </param>
        public void ExecuteInputAction(string actionName)
        {
            if (!InputActionMapNames.TryGetCommand(actionName, out var command))
            {
                return;
            }

            ExecutePlayerCommand(command);
        }

        /// <summary>
        ///     プレイヤーのコマンドを実行する。
        /// </summary>
        /// <param name="command"> 実行するプレイヤーのコマンド </param>
        public void ExecutePlayerCommand(PlayerCommand command)
        {
            if (_sessionModel.IsGameOver)
            {
                _gameBoardView.ClearActivePiece();
                return;
            }

            // コンテナの回転コマンドの場合は、ピースの面を切り替える。
            if (IsFaceChangeCommand(command))
            {
                SwitchFace(command);
                return;
            }

            // 現在のピースが存在しない場合は、新しいピースをスポーンする
            if (!_sessionModel.HasCurrentPiece)
            {
                TrySpawnNewPiece();
                RefreshActivePieceView();
                return;
            }

            // コマンドを実行する前の現在のピースを保持する
            ActivePiece commandTargetPiece = _sessionModel.CurrentPiece;
            _gamePlayUsecase.ExecuteCommand(command);

            // コマンド実行後に現在のピースがロックされたかどうかを判定する
            bool wasLocked = commandTargetPiece != null &&
                !_sessionModel.IsGameOver &&
                !_sessionModel.HasCurrentPiece;

            // ピースがロックされた場合は、固定ブロックとして確定し、新しいピースをスポーンする
            if (wasLocked)
            {
                CommitLockedPiece(commandTargetPiece);
                ClearComletedLines();
                TrySpawnNewPiece();

                // 新しいピースがスポーンされたので、落下時間をリセットする
                _fallElapsedSeconds = 0f;
            }

            RefreshActivePieceView();
        }

        private readonly GameSessionModel _sessionModel;
        private readonly GamePlayUsecase _gamePlayUsecase;
        private readonly IPieceDefinitionProvider _pieceDefinitionProvider;
        private readonly RandomSpawnFaceProvider _spawnFaceProvider;
        private readonly PieceSpawnSettings _spawnSettings;
        private readonly PieceFallSettings _fallSettings;
        private readonly PiecePositionResolver _positionResolver;
        private readonly IGameBoardView _gameBoardView;
        private readonly LineClearUsecase _lineClearUsecase;
        private readonly PieceFaceSwitchUsecase _pieceFaceSwitchUsecase;
        private readonly BoardFaceSelectionUsecase _boardFaceSelectionUsecase;
        private readonly IBoardCameraView _boardCameraView;
        private readonly BoardControlStateModel _boardControlStateModel;

        private float _fallElapsedSeconds = 0f;

        /// <summary>
        ///     新しいピースをスポーンする。
        /// </summary>
        /// <returns> ピースのスポーンに成功した場合はtrue、それ以外はfalse </returns>
        private bool TrySpawnNewPiece()
        {
            if (_sessionModel.IsGameOver ||
                _sessionModel.HasCurrentPiece)
            {
                return false;
            }

            // 次のピースの定義を取得し、ランダムなスポーン面IDを選択して、ピースをスポーンする
            var definition = _pieceDefinitionProvider.GetNext();
            var spawnFaceId = _spawnFaceProvider.GetRandomSpawnFaceId();
            bool spawned = _gamePlayUsecase.TrySpawnCurrentPiece(definition, spawnFaceId, _spawnSettings);

            if (!spawned)
            {
                _gameBoardView.ClearActivePiece();
                return false;
            }

            _boardFaceSelectionUsecase.SetCurrentFace(spawnFaceId);
            _boardCameraView.FocusFace(spawnFaceId.Value);

            return true;
        }

        /// <summary>
        ///     現在のピースのビューを更新する。
        /// </summary>
        private void RefreshActivePieceView()
        {
            if (_sessionModel.IsGameOver ||
                !_sessionModel.HasCurrentPiece)
            {
                _gameBoardView.ClearActivePiece();
                return;
            }

            // 現在のピースの位置を取得し、ビューに描画する
            IReadOnlyList<BoardCellPosition> positions = _positionResolver.Resolve(_sessionModel.CurrentPiece);
            _gameBoardView.RenderActivePiece(ConvertToViewDatas(positions));
        }

        private void CommitLockedPiece(ActivePiece LockedPiece)
        {
            IReadOnlyList<BoardCellPosition> positions = _positionResolver.Resolve(LockedPiece);
            _gameBoardView.CommitActivePieceAsFixedBlock(ConvertToViewDatas(positions));
        }

        private void ClearComletedLines()
        {
            LineClearResult result = _lineClearUsecase.ClearCompletedLines();

            if (result.HasRemovedBlocks)
            {
                _gameBoardView.RemoveFixedBlock(ConvertToViewDatas(result.RemovedPositions));
                _gameBoardView.MoveFixedBlock(ConvertToMoveViewDatas(result.MovedBlocks));
            }
        }

        private void SwitchFace(PlayerCommand command)
        {
            BoardFaceId nextFaceId = command == PlayerCommand.RotateContainerRight
                ? _boardFaceSelectionUsecase.RotationRight()
                : _boardFaceSelectionUsecase.RotationLeft();

            if (_sessionModel.HasCurrentPiece)
            {
                bool switched = _pieceFaceSwitchUsecase.TrySwitchFace(_sessionModel.CurrentPiece, nextFaceId);

                if (!switched)
                {
                    _sessionModel.SetGameOver();
                    _gameBoardView.ClearActivePiece();
                    return;
                }
            }

            _boardCameraView.FocusFace(nextFaceId.Value);
            RefreshActivePieceView();

            _fallElapsedSeconds = 0f;
        }

        /// <summary>
        ///     ModelのBoardCellPositionのリストを、View用のBoardCellViewDataの配列に変換する。
        /// </summary>
        /// <param name="positions"> 変換するBoardCellPositionのリスト </param>
        /// <returns> 変換後のBoardCellViewDataの配列 </returns>
        private static BoardCellViewData[] ConvertToViewDatas(IReadOnlyList<BoardCellPosition> positions)
        {
            var viewDatas = new BoardCellViewData[positions.Count];
            for (int i = 0; i < positions.Count; i++)
            {
                BoardCellPosition pos = positions[i];
                viewDatas[i] = new BoardCellViewData(pos.FaceId.Value, pos.X, pos.Y);
            }
            return viewDatas;
        }

        private static BoardCellMoveViewData[] ConvertToMoveViewDatas(IReadOnlyList<BoardBlockMove> moves)
        {
            var viewDatas = new BoardCellMoveViewData[moves.Count];

            for (int i = 0; i < moves.Count; i++)
            {
                viewDatas[i] = new BoardCellMoveViewData(
                    ConvertToViewData(moves[i].From),
                    ConvertToViewData(moves[i].To));
            }
            return viewDatas;
        }

        private static BoardCellViewData ConvertToViewData(BoardCellPosition position)
        {
            return new BoardCellViewData(position.FaceId.Value, position.X, position.Y);
        }

        private static bool IsFaceChangeCommand(PlayerCommand command)
        {
            return command == PlayerCommand.RotateContainerRight ||
                   command == PlayerCommand.RotateContainerLeft;
        }
    }
}

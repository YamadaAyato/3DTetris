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
            PiecePositionResolver positionResolver,
            IActivePieceView activePieceView)
        {
            _sessionModel = sessionModel ?? throw new ArgumentNullException(nameof(sessionModel));
            _gamePlayUsecase = gamePlayUsecase ?? throw new ArgumentNullException(nameof(gamePlayUsecase));
            _pieceDefinitionProvider = pieceDefinitionProvider ?? throw new ArgumentNullException(nameof(pieceDefinitionProvider));
            _spawnFaceProvider = spawnFaceProvider ?? throw new ArgumentNullException(nameof(spawnFaceProvider));
            _spawnSettings = spawnSettings;
            _positionResolver = positionResolver ?? throw new ArgumentNullException(nameof(positionResolver));
            _activePieceView = activePieceView ?? throw new ArgumentNullException(nameof(activePieceView));
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
                return;
            }

            if (!_sessionModel.HasCurrentPiece)
            {
                TrySpawnNewPiece();
                RefreshActivePieceView();
                return;
            }

            _gamePlayUsecase.ExecuteCommand(command);

            // ゲームプレイの結果として、現在のピースが消滅した場合は、新しいピースをスポーンする。
            if (!_sessionModel.IsGameOver && !_sessionModel.HasCurrentPiece)
            {
                TrySpawnNewPiece();
            }

            RefreshActivePieceView();
        }

        private readonly GameSessionModel _sessionModel;
        private readonly GamePlayUsecase _gamePlayUsecase;
        private readonly IPieceDefinitionProvider _pieceDefinitionProvider;
        private readonly RandomSpawnFaceProvider _spawnFaceProvider;
        private readonly PieceSpawnSettings _spawnSettings;
        private readonly PiecePositionResolver _positionResolver;
        private readonly IActivePieceView _activePieceView;

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

            var definition = _pieceDefinitionProvider.GetNext();
            var spawnFaceId = _spawnFaceProvider.GetRandomSpawnFaceId();
            return _gamePlayUsecase.TrySpawnCurrentPiece(definition, spawnFaceId, _spawnSettings);
        }

        /// <summary>
        ///     現在のピースのビューを更新する。
        /// </summary>
        private void RefreshActivePieceView()
        {
            if (_sessionModel.IsGameOver ||
                !_sessionModel.HasCurrentPiece)
            {
                _activePieceView.Clear();
                return;
            }

            IReadOnlyList<BoardCellPosition> positions = _positionResolver.Resolve(_sessionModel.CurrentPiece);

            BoardCellViewData[] cellViewDataArray = new BoardCellViewData[positions.Count];

            for (int i = 0; i < positions.Count; i++)
            {
                var pos = positions[i];
                cellViewDataArray[i] = new BoardCellViewData(pos.FaceId.Value, pos.X, pos.Y);
            }
            _activePieceView.Render(cellViewDataArray);
        }
    }
}

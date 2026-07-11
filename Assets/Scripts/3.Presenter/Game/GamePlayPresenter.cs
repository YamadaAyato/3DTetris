using System;
using ThreeDTetris.Model;
using ThreeDTetris.Usecase;

namespace ThreeDTetris.Presenter
{
    public class GamePlayPresenter
    {
        public GamePlayPresenter(
            GameSessionModel sessionModel,
            GamePlayUsecase gamePlayUsecase,
            IPieceDefinitionProvider pieceDefinitionProvider,
            RandomSpawnFaceProvider spawnFaceProvider,
            PieceSpawnSettings spawnSettings)
        {
            _sessionModel = sessionModel ?? throw new ArgumentNullException(nameof(sessionModel));
            _gamePlayUsecase = gamePlayUsecase ?? throw new ArgumentNullException(nameof(gamePlayUsecase));
            _pieceDefinitionProvider = pieceDefinitionProvider ?? throw new ArgumentNullException(nameof(pieceDefinitionProvider));
            _spawnFaceProvider = spawnFaceProvider ?? throw new ArgumentNullException(nameof(spawnFaceProvider));
            _spawnSettings = spawnSettings;
        }

        public void StartGame()
        {
            TrySpawnNewPiece();
        }

        public void ExecuteInputAction(string actionName)
        {
            if (!InputActionMapNames.TryGetCommand(actionName, out var command))
            {
                return;
            }

            ExecutePlayerCommand(command);
        }

        public void ExecutePlayerCommand(PlayerCommand command)
        {
            if (_sessionModel.IsGameOver)
            {
                return;
            }

            if (!_sessionModel.HasCurrentPiece)
            {
                TrySpawnNewPiece();
                return;
            }

            _gamePlayUsecase.ExecuteCommand(command);
        }

        private readonly GameSessionModel _sessionModel;
        private readonly GamePlayUsecase _gamePlayUsecase;
        private readonly IPieceDefinitionProvider _pieceDefinitionProvider;
        private readonly RandomSpawnFaceProvider _spawnFaceProvider;
        private readonly PieceSpawnSettings _spawnSettings;

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
    }
}

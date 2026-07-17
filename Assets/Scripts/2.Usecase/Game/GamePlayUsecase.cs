using System;
using ThreeDTetris.Model;

namespace ThreeDTetris.Usecase
{
    /// <summary>
    ///    ゲームプレイのユースケースを管理するクラス。
    /// </summary>
    public class GamePlayUsecase
    {
        public GamePlayUsecase(
            GameSessionModel gameSessionModel,
            PieceSpawnUsecase pieceSpawnUsecase,
            PieceMoveUsecase pieceMoveUsecase,
            PieceLockUsecase pieceLockUsecase,
            PieceRotationUsecase pieceRotationUsecase,
            PieceDropUsecase pieceDropUsecase)
        {
            _gameSessionModel = gameSessionModel ?? throw new ArgumentNullException(nameof(gameSessionModel));
            _pieceSpawnUsecase = pieceSpawnUsecase ?? throw new ArgumentNullException(nameof(pieceSpawnUsecase));
            _pieceMoveUsecase = pieceMoveUsecase ?? throw new ArgumentNullException(nameof(pieceMoveUsecase));
            _pieceLockUsecase = pieceLockUsecase ?? throw new ArgumentNullException(nameof(pieceLockUsecase));
            _pieceRotationUsecase = pieceRotationUsecase ?? throw new ArgumentNullException(nameof(pieceRotationUsecase));
            _pieceDropUsecase = pieceDropUsecase ?? throw new ArgumentNullException(nameof(pieceDropUsecase));
        }

        /// <summary>
        ///     新しいピースを生成し、ゲームセッションに設定する。
        /// </summary>
        /// <param name="definition"> 生成するピースの定義 </param>
        /// <param name="spawnFaceId"> ピースの生成面ID </param>
        /// <param name="spawnSettings"> ピースの生成設定 </param>
        /// <returns> ピースの生成に成功した場合はtrue、失敗した場合はfalse </returns>
        public bool TrySpawnCurrentPiece(
            PieceDefinition definition,
            BoardFaceId spawnFaceId,
            PieceSpawnSettings spawnSettings)
        {
            // ゲームオーバー状態の場合は、新しいピースを生成できない。
            if (_gameSessionModel.IsGameOver)
            {
                return false;
            }

            // すでに操作中のピースが存在する場合は、新しいピースを生成できない。
            if (_gameSessionModel.HasCurrentPiece)
            {
                return false;
            }

            // 新しいピースを生成し、配置可能かどうかを検証する。
            // 配置できない場合はゲームオーバーとする。
            if (!_pieceSpawnUsecase.TrySpawn(
                definition,
                spawnFaceId,
                spawnSettings,
                out ActivePiece activePiece))
            {
                _gameSessionModel.SetGameOver();
                return false;
            }

            _gameSessionModel.SetCurrentPiece(activePiece);
            return true;
        }

        /// <summary>
        ///     プレイヤー操作を実行する。
        /// </summary>
        /// <param name="command"> 実行するプレイヤーコマンド </param>
        /// <returns> コマンドの実行に成功した場合はtrue、失敗した場合はfalse </returns>
        public bool ExecuteCommand(PlayerCommand command)
        {
            if (_gameSessionModel.IsGameOver)
            {
                return false;
            }

            if (!_gameSessionModel.HasCurrentPiece)
            {
                return false;
            }

            switch (command)
            {
                case PlayerCommand.MoveLeft:
                    return _pieceMoveUsecase.MoveLeft(_gameSessionModel.CurrentPiece);
                case PlayerCommand.MoveRight:
                    return _pieceMoveUsecase.MoveRight(_gameSessionModel.CurrentPiece);
                case PlayerCommand.Rotate:
                    return _pieceRotationUsecase.Rotate(_gameSessionModel.CurrentPiece);
                case PlayerCommand.SoftDrop:
                    return DropCurrentPieceOneStep();
                case PlayerCommand.HardDrop:
                    return HardDropCurrentPiece();
                case PlayerCommand.RotateContainerLeft:
                case PlayerCommand.RotateContainerRight:
                case PlayerCommand.ResetCamera:
                case PlayerCommand.Pause:
                    return false;

                default:
                    throw new ArgumentOutOfRangeException(nameof(command), command, "未対応のプレイヤーコマンドです。");
            }
        }

        private readonly GameSessionModel _gameSessionModel;
        private readonly PieceSpawnUsecase _pieceSpawnUsecase;
        private readonly PieceMoveUsecase _pieceMoveUsecase;
        private readonly PieceLockUsecase _pieceLockUsecase;
        private readonly PieceRotationUsecase _pieceRotationUsecase;
        private readonly PieceDropUsecase _pieceDropUsecase;

        /// <summary>
        ///    現在のピースを1ステップ落下させる。
        /// </summary>
        /// <returns> ピースの落下に成功した場合はtrue、失敗した場合はfalse </returns>
        private bool DropCurrentPieceOneStep()
        {
            ActivePiece currentPiece = _gameSessionModel.CurrentPiece;

            // ピースを1ステップ落下させる。落下できない場合はピースをロックする。
            if (_pieceDropUsecase.DropOneStep(currentPiece))
            {
                return true;
            }

            return LockCurrentPiece();
        }

        /// <summary>
        ///     現在のピースをハードドロップさせる。
        /// </summary>
        /// <returns> ピースのハードドロップに成功した場合はtrue、失敗した場合はfalse </returns>
        private bool HardDropCurrentPiece()
        {
            ActivePiece currentPiece = _gameSessionModel.CurrentPiece;

            // ピースをハードドロップさせる。ハードドロップ後はピースをロックする。
            _pieceDropUsecase.HardDrop(currentPiece);
            return LockCurrentPiece();
        }

        /// <summary>
        ///    現在のピースをロックする。
        /// </summary>
        /// <returns> ピースのロックに成功した場合はtrue、失敗した場合はfalse </returns>
        private bool LockCurrentPiece()
        {
            ActivePiece currentPiece = _gameSessionModel.CurrentPiece;

            // ピースをロックする。ロックできない場合はゲームオーバーとする。
            if (!_pieceLockUsecase.Lock(currentPiece))
            {
                _gameSessionModel.SetGameOver();
                return false;
            }

            _gameSessionModel.ClearCurrentPiece();
            return true;
        }
    }
}

using System;
using ThreeDTetris.Model;
using ThreeDTetris.Presenter;
using ThreeDTetris.Usecase;
using ThreeDTetris.View;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ThreeDTetris.Container
{
    /// <summary>
    ///     ゲームプレイに必要な依存関係を登録するライフタイムスコープ。
    /// </summary>
    public class GameplayLifetimeScope : LifetimeScope
    {
        [Header("Board")]
        [SerializeField] private int _faceWidth = 8;
        [SerializeField] private int _faceHeight = 20;

        [Header("Piece")]
        [SerializeField] private PieceDefinitionCatalogAsset _pieceDefinitionCatalogAsset;
        [SerializeField] private PieceSpawnSettingsAsset _pieceSpawnSettingsAsset;

        [Header("Random")]
        [SerializeField] private bool _useRandomSeed = false;
        [SerializeField] private int _randomSeed = 0;

        protected override void Configure(IContainerBuilder builder)
        {
            if (_pieceDefinitionCatalogAsset == null)
            {
                throw new InvalidOperationException("ピース定義カタログアセットが設定されていません。");
            }

            if (_pieceSpawnSettingsAsset == null)
            {
                throw new InvalidOperationException("ピーススポーン設定アセットが設定されていません。");
            }

            BoardDimensions boardDimensions = new BoardDimensions(_faceWidth, _faceHeight);
            IBoardTopology boardTopology = DefaultCubeBoardTopologyFactory.Create(boardDimensions);

            BoardModel boardModel = new BoardModel(boardTopology);
            GameSessionModel gameSessionModel = new();

            PiecePositionResolver piecePositionResolver = new PiecePositionResolver(boardTopology);
            PiecePlacementValidator placementValidator = new PiecePlacementValidator(boardModel, piecePositionResolver);

            PieceDefinitionCatalog definitionCatalog = _pieceDefinitionCatalogAsset.CreateCatalog();
            PieceSpawnSettings spawnSettings = _pieceSpawnSettingsAsset.CreateSettings();

            IPieceDefinitionProvider pieceDefinitionProvider = _useRandomSeed
                ? new RandomBagPieceDefinitionProvider(definitionCatalog, _randomSeed)
                : new RandomBagPieceDefinitionProvider(definitionCatalog);

            RandomSpawnFaceProvider spawnFaceProvider = _useRandomSeed
                ? new RandomSpawnFaceProvider(boardTopology, _randomSeed)
                : new RandomSpawnFaceProvider(boardTopology);

            builder.RegisterInstance<IBoardTopology>(boardTopology);
            builder.RegisterInstance(boardModel);
            builder.RegisterInstance(gameSessionModel);
            builder.RegisterInstance(piecePositionResolver);
            builder.RegisterInstance(placementValidator);
            builder.RegisterInstance<IPieceDefinitionProvider>(pieceDefinitionProvider);
            builder.RegisterInstance(spawnFaceProvider);
            builder.RegisterInstance(spawnSettings);

            builder.Register<PieceSpawnUsecase>(Lifetime.Singleton);
            builder.Register<PieceMoveUsecase>(Lifetime.Singleton);
            builder.Register<PieceLockUsecase>(Lifetime.Singleton);
            builder.Register<PieceRotationUsecase>(Lifetime.Singleton);
            builder.Register<PieceDropUsecase>(Lifetime.Singleton);
            builder.Register<GamePlayUsecase>(Lifetime.Singleton);

            builder.RegisterComponentInHierarchy<ActivePieceView>().AsImplementedInterfaces();
            builder.Register<GamePlayPresenter>(Lifetime.Singleton);

            builder.RegisterComponentInHierarchy<GamePlayEntryView>();
            builder.RegisterComponentInHierarchy<PlayerInputView>();
        }
    }
}

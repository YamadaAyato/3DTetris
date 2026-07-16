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
        [SerializeField] private BoardShapeType _boardShapeType = BoardShapeType.Cube;
        [SerializeField] private int _faceHeight = 20;

        [Header("Cube")]
        [SerializeField] private int _faceWidth = 8;

        [Header("RectangularPrism")]
        [SerializeField] private int _rectFrontBackWidth = 8;
        [SerializeField] private int _rectSideWidth = 4;

        [Header("TrianglePrism")]
        [SerializeField] private int _triFirstWidth = 8;
        [SerializeField] private int _triSecondWidth = 6;
        [SerializeField] private int _triThirdWidth = 4;

        [Header("Piece")]
        [SerializeField] private PieceDefinitionCatalogAsset _pieceDefinitionCatalogAsset;
        [SerializeField] private PieceSpawnSettingsAsset _pieceSpawnSettingsAsset;

        [Header("Piece Fall Settings")]
        [SerializeField] private float _pieceFallIntervalSeconds = 1.0f;

        [Header("Random")]
        [SerializeField] private bool _useRandomSeed = false;
        [SerializeField] private int _randomSeed = 0;

        private enum BoardShapeType
        {
            Cube,
            RectangularPrism,
            TrianglePrism,
        }

        /// <summary>
        ///     ライフタイムスコープの依存関係を登録する。
        /// </summary>
        /// <param name="builder"> 依存関係を登録するためのコンテナビルダー </param>
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


            BoardShapeDefinition boardShapeDefinition = CreateBoardShapeDefinition();
            IBoardTopology boardTopology = new DataDrivenBoardTopology(boardShapeDefinition);

            BoardModel boardModel = new BoardModel(boardTopology);
            GameSessionModel gameSessionModel = new();
            BoardControlStateModel boardControlStateModel = new(boardTopology.InitialFaceId);

            PiecePositionResolver piecePositionResolver = new(boardTopology);
            PiecePlacementValidator placementValidator = new PiecePlacementValidator(boardModel, piecePositionResolver);

            PieceDefinitionCatalog definitionCatalog = _pieceDefinitionCatalogAsset.CreateCatalog();
            PieceSpawnSettings spawnSettings = _pieceSpawnSettingsAsset.CreateSettings();
            PieceFallSettings fallSettings = new PieceFallSettings(_pieceFallIntervalSeconds);

            IPieceDefinitionProvider pieceDefinitionProvider = _useRandomSeed
                ? new RandomBagPieceDefinitionProvider(definitionCatalog, _randomSeed)
                : new RandomBagPieceDefinitionProvider(definitionCatalog);

            RandomSpawnFaceProvider spawnFaceProvider = _useRandomSeed
                ? new RandomSpawnFaceProvider(boardTopology, _randomSeed)
                : new RandomSpawnFaceProvider(boardTopology);

            builder.RegisterInstance<IBoardTopology>(boardTopology);
            builder.RegisterInstance(boardModel);
            builder.RegisterInstance(gameSessionModel);
            builder.RegisterInstance(boardControlStateModel);
            builder.RegisterInstance(piecePositionResolver);
            builder.RegisterInstance(placementValidator);
            builder.RegisterInstance<IPieceDefinitionProvider>(pieceDefinitionProvider);
            builder.RegisterInstance(spawnFaceProvider);
            builder.RegisterInstance(spawnSettings);
            builder.RegisterInstance(fallSettings);

            builder.Register<PieceSpawnUsecase>(Lifetime.Singleton);
            builder.Register<PieceMoveUsecase>(Lifetime.Singleton);
            builder.Register<PieceLockUsecase>(Lifetime.Singleton);
            builder.Register<PieceRotationUsecase>(Lifetime.Singleton);
            builder.Register<PieceFaceSwitchUsecase>(Lifetime.Singleton);
            builder.Register<BoardFaceSelectionUsecase>(Lifetime.Singleton);
            builder.Register<PieceDropUsecase>(Lifetime.Singleton);
            builder.Register<LineClearUsecase>(Lifetime.Singleton);
            builder.Register<GamePlayUsecase>(Lifetime.Singleton);

            builder.RegisterComponentInHierarchy<GameBoardView>().AsImplementedInterfaces();
            builder.RegisterComponentInHierarchy<BoardCameraView>().AsImplementedInterfaces();

            builder.Register<GamePlayPresenter>(Lifetime.Singleton);

            builder.RegisterComponentInHierarchy<GamePlayEntryView>();
            builder.RegisterComponentInHierarchy<PlayerInputView>();
        }

        private BoardShapeDefinition CreateBoardShapeDefinition()
        {
            return _boardShapeType switch
            {
                BoardShapeType.Cube => PrismBoardShapeFactory.CreateCube(
                    _faceWidth,
                    _faceHeight),

                BoardShapeType.RectangularPrism => PrismBoardShapeFactory.CreateRectangularPrism(
                    _rectFrontBackWidth,
                    _rectSideWidth,
                    _faceHeight),

                BoardShapeType.TrianglePrism => PrismBoardShapeFactory.CreateTriangularPrism(
                    _triFirstWidth,
                    _triSecondWidth,
                    _triThirdWidth,
                    _faceHeight),

                _ => throw new InvalidOperationException($"不明なボード形状タイプ: {_boardShapeType}"),
            };
        }
    }
}

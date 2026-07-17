using System;
using ThreeDTetris.Model;

namespace ThreeDTetris.Usecase
{
    /// <summary>
    ///     ピース定義でbag方式でランダムにピースを提供するクラス。
    ///     コンストラクタの使用法はRandomSpawnFaceProviderと同様。
    /// </summary>
    public class RandomBagPieceDefinitionProvider : IPieceDefinitionProvider
    {
        public RandomBagPieceDefinitionProvider(PieceDefinitionCatalog catalog)
            : this(catalog, new Random())
        {

        }

        public RandomBagPieceDefinitionProvider(PieceDefinitionCatalog catalog, int seed)
            : this(catalog, new Random(seed))
        {
        }

        public RandomBagPieceDefinitionProvider(PieceDefinitionCatalog catalog, Random random)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _random = random ?? throw new ArgumentNullException(nameof(random));
            _bag = new PieceDefinition[_catalog.Count];

            RefillBag();
        }

        public PieceDefinition GetNext()
        {
            if (_nextIndex >= _bag.Length)
            {
                RefillBag();
            }

            PieceDefinition definition = _bag[_nextIndex];
            _nextIndex++;

            return definition;
        }

        private readonly PieceDefinitionCatalog _catalog;
        private readonly Random _random;
        private readonly PieceDefinition[] _bag;

        private int _nextIndex;

        /// <summary>
        ///     ピースのbagを補充する。
        /// </summary>
        private void RefillBag()
        {
            for (int i = 0; i < _bag.Length; i++)
            {
                _bag[i] = _catalog.PieceDefinitions[i];
            }

            ShuffleBag();
            _nextIndex = 0;
        }

        /// <summary>
        ///     ピースのbagをシャッフルする。
        /// </summary>
        private void ShuffleBag()
        {
            for (int i = _bag.Length - 1; i > 0; i--)
            {
                int swapIndex = _random.Next(i + 1);

                var temp = _bag[i];
                _bag[i] = _bag[swapIndex];
                _bag[swapIndex] = temp;
            }
        }
    }
}

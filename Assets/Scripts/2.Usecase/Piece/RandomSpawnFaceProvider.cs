using System;
using ThreeDTetris.Model;

namespace ThreeDTetris.Usecase
{
    /// <summary>
    ///     ランダムに生成可能な面を提供するクラス。
    ///     : thisにより、同じクラスの別のコンストラクタを呼び出すことで、コードの重複を避けることができる。
    /// </summary>
    public class RandomSpawnFaceProvider
    {
        /// <summary>
        ///     通常の乱数生成器を使用してランダムに生成可能な面を提供する。
        /// </summary>
        /// <param name="boardTopology"></param>
        public RandomSpawnFaceProvider(IBoardTopology boardTopology)
            : this(boardTopology, new Random())
        {
        }

        /// <summary>
        ///     指定されたシード値を使用してランダムに生成可能な面を提供する。
        /// </summary>
        /// <param name="boardTopology"> ボードのトポロジー情報 </param>
        /// <param name="seed"> 
        /// 乱数生成のシード値 
        /// 同じ値を指定すると、同じ順序で面が選ばれる。
        /// </param>
        public RandomSpawnFaceProvider(IBoardTopology boardTopology, int seed)
            : this(boardTopology, new Random(seed))
        {
        }

        private RandomSpawnFaceProvider(IBoardTopology boardTopology, Random random)
        {
            _boardTopology = boardTopology ?? throw new ArgumentNullException(nameof(boardTopology));
            _random = random ?? throw new ArgumentNullException(nameof(random));

            if (_boardTopology.FaceIds.Count == 0)
            {
                throw new InvalidOperationException("生成可能な面が存在しません。");
            }
        }

        /// <summary>
        ///     ランダムに生成可能な面のIDを取得する。
        /// </summary>
        /// <returns> ランダムに選ばれた面のID </returns>
        public BoardFaceId GetRandomSpawnFaceId()
        {
            int index = _random.Next(_boardTopology.FaceIds.Count);
            return _boardTopology.FaceIds[index];
        }

        private readonly IBoardTopology _boardTopology;
        private readonly Random _random;
    }
}

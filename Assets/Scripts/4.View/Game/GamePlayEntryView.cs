using ThreeDTetris.Presenter;
using UnityEngine;
using VContainer;

namespace ThreeDTetris.View
{
    /// <summary>
    ///     ゲームプレイのエントリーポイントとなるビュー。
    /// </summary>
    public class GamePlayEntryView : MonoBehaviour
    {
        [Inject]
        public void Construct(GamePlayPresenter gamePlayPresenter)
        {
            _gamePlayPresenter = gamePlayPresenter;
        }

        private GamePlayPresenter _gamePlayPresenter;

        private void Start()
        {
            _gamePlayPresenter?.StartGame();
        }

        private void Update()
        {
            _gamePlayPresenter?.Tick(Time.deltaTime);
        }
    }
}

using System;
using ThreeDTetris.Presenter;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace ThreeDTetris.View
{
    /// <summary>
    ///     PlayerInput の入力通知をゲーム内操作へ変換する入力ビュー。
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputView : MonoBehaviour
    {
        [Inject]
        public void Construct(GamePlayPresenter gamePlayPresenter)
        {
            _gamePlayPresenter = gamePlayPresenter ?? throw new ArgumentNullException(nameof(gamePlayPresenter));
        }

        private PlayerInput _playerInput;
        private GamePlayPresenter _gamePlayPresenter;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            ValidateActions();
        }

        private void OnEnable()
        {
            _playerInput.onActionTriggered += ActionTriggered;
        }

        private void OnDisable()
        {
            _playerInput.onActionTriggered -= ActionTriggered;
        }

        private void ActionTriggered(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Performed)
            {
                return;
            }

            _gamePlayPresenter?.ExecuteInputAction(context.action.name);
        }

        private void ValidateActions()
        {
            ValidateAction(InputActionMapNames.MOVE_LEFT);
            ValidateAction(InputActionMapNames.MOVE_RIGHT);
            ValidateAction(InputActionMapNames.ROTATE_PIECE);
            ValidateAction(InputActionMapNames.SOFT_DROP);
            ValidateAction(InputActionMapNames.HARD_DROP);
            ValidateAction(InputActionMapNames.ROTATE_CONTAINER_LEFT);
            ValidateAction(InputActionMapNames.ROTATE_CONTAINER_RIGHT);
            ValidateAction(InputActionMapNames.RESET_CAMERA);
            ValidateAction(InputActionMapNames.PAUSE);
        }

        private void ValidateAction(string actionName)
        {
            if (_playerInput.actions.FindAction(actionName, throwIfNotFound: false) != null)
            {
                return;
            }

            Debug.LogError($"[{nameof(PlayerInputView)}] PlayerInput の Actions に {actionName} が存在しません。", this);
        }
    }
}

using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputBuffer : MonoBehaviour
{


    private const string MOVE_ACTION = "Move";
    private const string LOOK_ACTION = "Look";
    private const string JUMP_ACTION = "Jump";
    private const string ATTACK_ACTION = "Attack";
    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _jumpAction;
    private InputAction _attackAction;
    private PlayerInput _playerInput;
    private InputView _inputView;
    private void Awake()
    {
        _inputView = Instantiate(Resources.Load<InputView>("Prefabs/InputView"));
        PlayerInputInitialize();
        _inputView.Initialize(_playerInput.actions.bindings.Count());
        _inputView.SetButtonText(ConvertStrings());
    }

    private string[] ConvertStrings()
    {
        return _playerInput.actions.bindings.Select(binding => binding.ToDisplayString()).ToArray();
    }

    private void PlayerInputInitialize()
    {
        if(!TryGetComponent(out PlayerInput playerInput)) return;
        _playerInput = playerInput;
        _moveAction = _playerInput.actions[MOVE_ACTION];
        _lookAction = _playerInput.actions[LOOK_ACTION];
        _jumpAction = _playerInput.actions[JUMP_ACTION];
        _attackAction = _playerInput.actions[ATTACK_ACTION];
    }

    private void OnDestroy()
    {
        if (_inputView != null)
        {
            Destroy(_inputView.gameObject);
        }
    }
}
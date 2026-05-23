using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputBuffer : MonoBehaviour
{
    private PlayerInput _input;
    [SerializeField]
    private TextMeshProUGUI[] _texts;
    InputAction action;
    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        RebindMoveUp();
    }

    private void RebindMoveUp()
    {
         action = _input.actions["Move"];
        int bindingIndex = 1; // Up

        action.Disable();

        action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsHavingToMatchPath("<Keyboard>")
            .OnComplete(operation =>
            {
                operation.Dispose();
                action.Enable();

                Debug.Log(action.bindings[bindingIndex].ToDisplayString());
            })
            .Start();
    }

    private void Update()
    {
        for(int i = 0; i < action.bindings.Count;i++)
        {
            _texts[i].text = action.bindings[i].ToDisplayString();
        }
    }
}
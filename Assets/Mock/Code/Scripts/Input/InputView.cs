using TMPro;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(GridLayoutGroup))]
public class InputView : ViewBase
{
    public void Initialize(int uiCount)
    {
        GenerateLayout();
        GenerateButtonByActionsCount(uiCount);

    }

    private void GenerateLayout()
    {
        GridLayoutGroup gridLayoutGroup = GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = 10;
        gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
    }

    public void SetButtonText(string[] buttonTexts)
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = buttonTexts[i];
        }
    }

    [SerializeField]
    private Button _buttonPrefab;
    private Button[] _buttons;

    private void GenerateButtonByActionsCount(int count)
    {
        _buttons = new Button[count];
        for (int i = 0; i < count; i++)
        {
            _buttons[i] = Instantiate(_buttonPrefab, transform);
            _buttons[i].onClick.AddListener(() => Debug.Log($"Button {i} pressed"));
            _buttons[i].name = $"Button_{i}";
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        foreach (Button button in _buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }
}
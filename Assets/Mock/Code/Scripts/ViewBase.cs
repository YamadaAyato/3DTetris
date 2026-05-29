using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Canvas))]
public abstract class ViewBase : MonoBehaviour
{
    public virtual void Show()
    {
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<Canvas>().enabled = true;
    }

    public virtual void Hide()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<Canvas>().enabled = false;
    }

    private void Awake()
    {
        Initialize();
        OnAwake();
    }
    private void Start()
    {
        OnStart();
    }
    private void Initialize()
    {
    }
    protected virtual void OnDestroy() { }

    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }
}
using UnityEngine;
using UnityEngine.EventSystems;
public abstract class ViewBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnExit();
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


    protected virtual void OnEnter() { }
    protected virtual void OnExit() { }
    protected virtual void OnDestroy() { }

    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }
}
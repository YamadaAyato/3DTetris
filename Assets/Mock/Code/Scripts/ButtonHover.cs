using UnityEngine;
using DG.Tweening;
public class ButtonHover : ViewBase
{
    private float _scaleMin = 0.5f;
    private float _scaleMax = 1.00f;
    private float _duration = 0.5f;
    protected override void OnEnter()
    {
        transform.DOKill();
        transform.DOScale(_scaleMin, _duration).SetEase(Ease.OutQuad);
    }
    protected override void OnExit()
    {
        transform.DOKill();
        transform.DOScale(_scaleMax, _duration).SetEase(Ease.OutQuad);
    }
}
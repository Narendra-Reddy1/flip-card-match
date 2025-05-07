using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class CustomButton : Button
{
    [Header("Custom Setting")]
    [SerializeField] private bool clickAnimate = true;

    [SerializeField] private bool _dontInteractOnAnimation = true;
    [SerializeField] private Transform _animatableComponent;

    protected override void Start()
    {
        base.Start();
        transition = Transition.None;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (_animatableComponent)
        {
            _animatableComponent.DOScale(0.9f, 0.1f).SetEase(Ease.OutQuad);
        }
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (_animatableComponent)
        {
            _animatableComponent.DOScale(1f, 0.1f).SetEase(Ease.OutQuad);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

    }

    public override void OnPointerClick(PointerEventData eventData)
    {

        if (clickAnimate && IsInteractable())
        {
            if (_dontInteractOnAnimation)
                interactable = false;
            if (_animatableComponent)
                _animatableComponent.DOScale(1f, 0.1f).SetEase(Ease.OutQuad).onComplete += () =>
                {
                    if (_dontInteractOnAnimation)
                        interactable = true;
                    base.OnPointerClick(eventData);
                };
            if (!_animatableComponent)
                gameObject.transform.DOScale(1.05f, 0.1f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    if (_dontInteractOnAnimation)
                        interactable = true;
                    base.OnPointerClick(eventData);
                });
        }
        else
        {
            base.OnPointerClick(eventData);
        }

    }


}

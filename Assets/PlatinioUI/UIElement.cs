using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIElement : MonoBehaviour 
{
    public RectTransform rect
    {
        get
        {
            if (_rect == null)
                _rect = GetComponent<RectTransform>();
            return _rect;
        }

        set
        { _rect = value; }
    }

    public PlatinioUI.Direction entryFrom;
    public PlatinioUI.Animation entryAnimation;
    public float entryAnimationTime;
    public PlatinioUI.Direction exitTo;
    public PlatinioUI.Animation exitAnimation;
    public float exitAnimationTime;
    public bool isBusy;

    private RectTransform _rect;
    private Vector2 targetPos;
    
    private PlatinioUI platinioUI
    {
        get
        {
            if(_platinioUI == null)
                _platinioUI = GameObject.FindObjectOfType<PlatinioUI>() as PlatinioUI;
            return _platinioUI;
        }
        set { _platinioUI = value; }
    }
    private PlatinioUI _platinioUI;

    void Start()
    {
        
    }

    public void SetTargetPos()
    {
        targetPos = new Vector2(transform.position.x , transform.position.y);
    }


    public void Show(Action OnComplete = null)
    {
        if(isBusy)
            return;

        isBusy = true;
        LeanTweenType ease = PlatinioUI.GetEase(entryAnimation);
        //set initial pos
        switch (entryFrom)
        {
            case PlatinioUI.Direction.BOTTOM:
                transform.position = new Vector3(targetPos.x , -platinioUI.verticalOffset , 0);
                gameObject.SetActive(true);
                LeanTween.moveY(gameObject, targetPos.y, entryAnimationTime).setEase(ease).setOnComplete(() =>
                    {
                        if (OnComplete != null)
                            OnComplete();
                        isBusy = false;
                    });
                break;
            case PlatinioUI.Direction.LEFT:
                transform.position = new Vector3(-platinioUI.horizontalOffset, targetPos.y, 0);
                gameObject.SetActive(true);
                LeanTween.moveX(gameObject, targetPos.x, entryAnimationTime).setEase(ease).setOnComplete(() =>
                    {
                        if (OnComplete != null)
                            OnComplete();
                        isBusy = false;
                    });
                break;
            case PlatinioUI.Direction.RIGHT:
                transform.position = new Vector3(platinioUI.horizontalOffset, targetPos.y, 0);
                gameObject.SetActive(true);
                LeanTween.moveX(gameObject, targetPos.x, entryAnimationTime).setEase(ease).setOnComplete(() =>
                    {
                        if (OnComplete != null)
                            OnComplete();
                        isBusy = false;
                    });
                break;
            case PlatinioUI.Direction.UP:
                transform.position = new Vector3(targetPos.x, platinioUI.verticalOffset, 0);
                gameObject.SetActive(true);
                LeanTween.moveY(gameObject, targetPos.y, entryAnimationTime).setEase(ease).setOnComplete(() =>
                    {
                        if (OnComplete != null)
                            OnComplete();
                        isBusy = false;
                    });
                break;
        }
    }

    public void Hide(Action OnComplete = null)
    {
        if (isBusy)
            return;

        isBusy = true;
        LeanTweenType ease = PlatinioUI.GetEase(exitAnimation);
        //set initial pos
        switch (exitTo)
        {
            case PlatinioUI.Direction.BOTTOM:
                LeanTween.moveY(gameObject, -platinioUI.verticalOffset, entryAnimationTime).setEase(ease).setOnComplete(() =>
                {
                    if (OnComplete != null)
                        OnComplete();
                    isBusy = false;
                    gameObject.SetActive(false);
                });
            
                break;
            case PlatinioUI.Direction.LEFT:
                LeanTween.moveX(gameObject, -platinioUI.horizontalOffset, entryAnimationTime).setEase(ease).setOnComplete(() =>
                {
                    isBusy = false;
                    gameObject.SetActive(false);
                });
                break;
            case PlatinioUI.Direction.RIGHT:
                LeanTween.moveX(gameObject, platinioUI.horizontalOffset, entryAnimationTime).setEase(ease).setOnComplete(() =>
                {
                    if (OnComplete != null)
                        OnComplete();
                    isBusy = false;
                    gameObject.SetActive(false);
                });
                break;
            case PlatinioUI.Direction.UP:
                LeanTween.moveY(gameObject, platinioUI.verticalOffset, entryAnimationTime).setEase(ease).setOnComplete(() =>
                {
                    if (OnComplete != null)
                        OnComplete();
                    isBusy = false;
                    gameObject.SetActive(false);
                });
                break;
        }
    }
	
}

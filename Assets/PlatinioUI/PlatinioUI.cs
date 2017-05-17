using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlatinioUI : MonoBehaviour 
{

    #region SINGLETON
    private static PlatinioUI platinioUI;
    public static PlatinioUI instance
    {
        get
        {
            if (!platinioUI)
            {
                platinioUI = FindObjectOfType(typeof(PlatinioUI)) as PlatinioUI;



                if (!platinioUI)
                    Debug.LogError("You need to have a PlatinioUI script active in the scene");
            }

            return platinioUI;
        }

    }
    #endregion SINGLETON


    public RectTransform canvasRect;
    public UIScreen initialScreen;
    public float horizontalOffset;
    public float verticalOffset;
    
    private UIScreen beforeScreen;
    private UIScreen currentScreen;
    private UIScreen nextScreen;

    public enum Direction
    {
        UP,
        LEFT,
        BOTTOM,
        RIGHT,
    }

    public enum Animation
    {
        BACK,
        BOUNCE,
        CIRC,
        CUBIC,
        ELASTIC,
        EXPO,
        QUAD,
        QUART,
        QUINT,
        SINE
    }
       

    void Awake()
    {
        GameObject clone = Instantiate(initialScreen.gameObject , Vector3.zero , Quaternion.identity) as GameObject;
        clone.transform.parent = canvasRect;
        clone.transform.localScale = Vector3.one;        

        nextScreen = initialScreen.next;
        currentScreen = clone.GetComponent<UIScreen>();
        beforeScreen = initialScreen.before;

        currentScreen.rect.sizeDelta = new Vector2(1.0f , 1.0f);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            MoveToNext();
        if (Input.GetKeyDown(KeyCode.DownArrow))
            MoveToBack();

    }

    private void MoveTo(UIScreen screen, Action OnComplete = null)
    {
        GameObject clone = Instantiate(screen.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
        clone.transform.parent = canvasRect;
        clone.transform.localScale = Vector3.one;

        //set pos
        switch (screen.entryFrom)
        {
            case Direction.UP:
                clone.transform.position = new Vector3(0, verticalOffset, 0);
                break;
            case Direction.LEFT:
                clone.transform.position = new Vector3(-horizontalOffset, 0, 0);
                break;
            case Direction.BOTTOM:
                clone.transform.position = new Vector3(0, -verticalOffset, 0);
                break;
            case Direction.RIGHT:
                clone.transform.position = new Vector3(horizontalOffset, 0, 0);
                break;

        }



        GameObject screenToDelete = currentScreen.gameObject;
        currentScreen = clone.GetComponent<UIScreen>();
        currentScreen.rect.sizeDelta = new Vector2(1.0f, 1.0f);

        LeanTweenType ease = GetEase(currentScreen.AnimationType);

        if (currentScreen.entryFrom == Direction.UP || currentScreen.entryFrom == Direction.BOTTOM)
        {
            if (ease != null)
            {
                LeanTween.moveY(currentScreen.gameObject, 0, currentScreen.animTime).setEase(ease).setOnComplete(() =>
                {
                    if (OnComplete != null)
                        OnComplete();
                    Destroy(screenToDelete);
                    currentScreen.UpdateElementsPos();
                });
            }

            else
            {
                LeanTween.moveY(currentScreen.gameObject, 0, currentScreen.animTime).setOnComplete(() =>
                {
                    if (OnComplete != null)
                        OnComplete();
                    Destroy(screenToDelete);
                    currentScreen.UpdateElementsPos();
                });
            }

        }

        else
        {
            if (ease != null)
            {
                LeanTween.moveX(currentScreen.gameObject, 0, currentScreen.animTime).setEase(ease).setOnComplete(() =>
                {
                    if (OnComplete != null)
                        OnComplete();
                    Destroy(screenToDelete);
                    currentScreen.UpdateElementsPos();
                });
            }

            else
            {
                LeanTween.moveX(currentScreen.gameObject, 0, currentScreen.animTime).setOnComplete(() =>
                {
                    if (OnComplete != null)
                        OnComplete();
                    Destroy(screenToDelete);
                    currentScreen.UpdateElementsPos();
                });
            }
        }
            
        nextScreen = currentScreen.next;
        beforeScreen = currentScreen.before;
    }

    public static LeanTweenType GetEase(Animation anim)
    {
        switch(anim)
        {
            case Animation.BACK:
                return LeanTweenType.easeOutBack;
            case Animation.BOUNCE:
                return LeanTweenType.easeOutBounce;
            case Animation.CIRC:
                return LeanTweenType.easeOutCirc;
            case Animation.CUBIC:
                return LeanTweenType.easeOutCubic;
            case Animation.ELASTIC:
                return LeanTweenType.easeOutElastic;
            case Animation.EXPO:
                return LeanTweenType.easeOutExpo;
            case Animation.QUAD:
                return LeanTweenType.easeOutQuad;
            case Animation.QUART:
                return LeanTweenType.easeOutQuart;
            case Animation.QUINT:
                return LeanTweenType.easeOutQuint;
            case Animation.SINE:
                return LeanTweenType.easeOutSine;
        }

        return LeanTweenType.easeOutSine;
    }

    public void MoveToNext(Action OnComplete = null)
    {
        MoveTo(nextScreen);

       
    }

    public void MoveToBack(Action OnComplete = null)
    {
        MoveTo(beforeScreen);
    }
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIScreen : MonoBehaviour 
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

    private RectTransform _rect;

    public PlatinioUI.Animation AnimationType;
    public PlatinioUI.Direction entryFrom;
    public float animTime;
    public UIScreen before;
    public UIScreen next;
    public List<UIElement> elements;
    public event Action OnAnimationComplete = delegate{};

    void Start()
    {
        for (int n = 0; n < elements.Count; n++)
        {
            elements[n].SetTargetPos();
            elements[n].gameObject.SetActive(false);
        }
    }


    public void UpdateElementsPos()
    {
        for (int n = 0; n < elements.Count; n++)
        {
            elements[n].SetTargetPos();            
        }
    }

    public void ShowElement(string name)
    {
        for (int n = 0; n < elements.Count; n++)
        {
            if (elements[n].gameObject.name == name)
            {
                elements[n].Show(OnAnimationComplete);
            }
                
        }
    }

    public void ShowAll()
    {
        for (int n = 0; n < elements.Count; n++)
        {
            elements[n].Show(OnAnimationComplete);
        }
        
    }

    public void HideElement(string name)
    {
        for (int n = 0; n < elements.Count; n++)
        {
            if (elements[n].gameObject.name == name)
                elements[n].Hide(OnAnimationComplete);       

        }
    }

    public void HideAll()
    {
        for (int n = 0; n < elements.Count; n++)
        {
            elements[n].Hide(OnAnimationComplete);
        }
    }

}

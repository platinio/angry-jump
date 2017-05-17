using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public List<GameObject> items;
    public GameObject window;
    public float animTime;
    public LeanTweenType ease;

    private int currentSelection;
    private GameObject beforeItem;
    private GameObject currentItem;
    private GameObject nextItem;
    private bool isBusy;

    void Start()
    {
        PlatinioUI.instance.OnAnimationComplete += Initialize;
    }

    public void Initialize()
    {
        currentItem = Instantiate(items[currentSelection] , window.transform) as GameObject;
        currentItem.transform.localScale = Vector3.one;
        currentItem.transform.position = new Vector3(0, 0, 0);

        if (items.Count > 1)
        {
            nextItem = Instantiate(items[currentSelection + 1], window.transform) as GameObject;
            nextItem.transform.localScale = Vector3.one;
            nextItem.transform.position = new Vector3(PlatinioUI.instance.horizontalOffset , 0 , 0);
        }

        beforeItem = Instantiate(items[items.Count - 1], window.transform) as GameObject;
        beforeItem.transform.localScale = Vector3.one;
        beforeItem.transform.position = new Vector3(-PlatinioUI.instance.horizontalOffset, 0, 0);


        PlatinioUI.instance.OnAnimationComplete -= Initialize;
    }

    public void MoveToNext()
    {
        if (isBusy)
            return;

        isBusy = true;

        currentSelection++;

        if (currentSelection > items.Count - 1)
            currentSelection = 0;

        LeanTween.moveX(currentItem, currentItem.transform.position.x - PlatinioUI.instance.horizontalOffset, animTime).setEase(ease);
        LeanTween.moveX(nextItem, nextItem.transform.position.x - PlatinioUI.instance.horizontalOffset, animTime).setEase(ease).setOnComplete(() =>
        {
            isBusy = false;
        }
            ); ;
        
        if(beforeItem != null)
            Destroy(beforeItem);

        beforeItem = currentItem;
        currentItem = nextItem;
        
        if (currentSelection + 1 < items.Count)
        {
            nextItem = Instantiate(items[currentSelection + 1], window.transform) as GameObject;
            nextItem.transform.localScale = Vector3.one;
            nextItem.transform.position = new Vector3(PlatinioUI.instance.horizontalOffset, 0, 0);
        }

        else
        {
            nextItem = Instantiate(items[0], window.transform) as GameObject;
            nextItem.transform.localScale = Vector3.one;
            nextItem.transform.position = new Vector3(PlatinioUI.instance.horizontalOffset, 0, 0);
            
        }

        

    }

    public void MoveToBefore()
    {

        if (isBusy)
            return;

        isBusy = true;

        currentSelection--;
        if (currentSelection < 0)
            currentSelection = items.Count - 1;
        LeanTween.moveX(currentItem, currentItem.transform.position.x + PlatinioUI.instance.horizontalOffset, animTime).setEase(ease);
        LeanTween.moveX(beforeItem, beforeItem.transform.position.x + PlatinioUI.instance.horizontalOffset, animTime).setEase(ease).setOnComplete(() =>
        {
            isBusy = false;
        }
            ); ;

        if (nextItem != null)
            Destroy(nextItem);

        nextItem = currentItem;
        currentItem = beforeItem;
      


        if (currentSelection - 1 >= 0)
        {
            beforeItem = Instantiate(items[currentSelection - 1], window.transform) as GameObject;
            beforeItem.transform.localScale = Vector3.one;
            beforeItem.transform.position = new Vector3(-PlatinioUI.instance.horizontalOffset, 0, 0);
        }
        else
        {
            beforeItem = Instantiate(items[items.Count - 1], window.transform) as GameObject;
            beforeItem.transform.localScale = Vector3.one;
            beforeItem.transform.position = new Vector3(-PlatinioUI.instance.horizontalOffset, 0, 0);
        }

        Debug.Log(currentSelection);
    }

}

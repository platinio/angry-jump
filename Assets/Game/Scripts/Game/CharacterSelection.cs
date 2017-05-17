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

    void Start()
    {
        currentItem = Instantiate(items[currentSelection] , window.transform) as GameObject;
        currentItem.transform.localScale = Vector3.one;
        currentItem.transform.position = new Vector3(-PlatinioUI.instance.horizontalOffset, 0, 0);

        if (items.Count > 1)
        {
            currentSelection++;
            nextItem = Instantiate(items[currentSelection], window.transform) as GameObject;
            nextItem.transform.localScale = Vector3.one;
        }
        
    }

    public void MoveToNext()
    {
        LeanTween.moveX(currentItem,currentItem.transform.position.x -PlatinioUI.instance.horizontalOffset, animTime).setEase(ease);
        LeanTween.moveX(nextItem, 0, animTime).setEase(ease);

    }

    public void MoveToBefore()
    {
 
    }

}

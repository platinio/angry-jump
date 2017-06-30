using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    //inspector
    [SerializeField] private List<GameObject>   _items               = null;
    [SerializeField] private Transform          _window              = null;
    [SerializeField] private float              _animTime            = 0.5f;
    [SerializeField] private LeanTweenType      _ease                = LeanTweenType.easeInElastic;
    [SerializeField] private int                _currentSelection    = 0;

    //private
    private GameObject beforeItem       = null;
    private GameObject currentItem      = null;
    private GameObject nextItem         = null;
    private bool isBusy                 = false;

    //public
    public int CurrentSelection { get { return _currentSelection; } }


    void Start()
    {
        //initialize after the animation
        PlatinioUI.instance.OnAnimationComplete += Initialize;
        _window = transform.parent;
    }

   
    /// <summary>
    /// Initialize the character selection window
    /// </summary>
    public void Initialize()
    {
        currentItem = Instantiate(_items[_currentSelection] , _window) as GameObject;
        currentItem.transform.localScale = Vector3.one;
        currentItem.transform.position = new Vector3(0, 0, 0);

        if (_items.Count > 1)
        {
            nextItem = Instantiate(_items[_currentSelection + 1], _window) as GameObject;
            nextItem.transform.localScale = Vector3.one;
            nextItem.transform.position = new Vector3(PlatinioUI.instance.horizontalOffset , 0 , 0);
        }

        beforeItem = Instantiate(_items[_items.Count - 1], _window) as GameObject;
        beforeItem.transform.localScale = Vector3.one;
        beforeItem.transform.position = new Vector3(-PlatinioUI.instance.horizontalOffset, 0, 0);


        PlatinioUI.instance.OnAnimationComplete -= Initialize;
    }

    /// <summary>
    /// Move to next selection
    /// </summary>
    public void MoveToNext()
    {
        //if we are animating comeback
        if (isBusy)
            return;

        //set what we are busy
        isBusy = true;

        _currentSelection++;

        if (_currentSelection > _items.Count - 1)
            _currentSelection = 0;

        LeanTween.moveX(currentItem, currentItem.transform.position.x - PlatinioUI.instance.horizontalOffset, _animTime).setEase(_ease);
        LeanTween.moveX(nextItem, nextItem.transform.position.x - PlatinioUI.instance.horizontalOffset, _animTime).setEase(_ease).setOnComplete(() =>
        {
            isBusy = false;
        }
            ); ;
        
        if(beforeItem != null)
            Destroy(beforeItem);
        //update items
        beforeItem = currentItem;
        currentItem = nextItem;
        
        if (_currentSelection + 1 < _items.Count)
        {
            nextItem = Instantiate(_items[_currentSelection + 1], _window) as GameObject;
            nextItem.transform.localScale = Vector3.one;
            nextItem.transform.position = new Vector3(PlatinioUI.instance.horizontalOffset, 0, 0);
        }

        else
        {
            nextItem = Instantiate(_items[0], _window) as GameObject;
            nextItem.transform.localScale = Vector3.one;
            nextItem.transform.position = new Vector3(PlatinioUI.instance.horizontalOffset, 0, 0);
            
        }

        

    }

    /// <summary>
    /// Move to previus selection
    /// </summary>
    public void MoveToBefore()
    {

        if (isBusy)
            return;

        isBusy = true;

        _currentSelection--;
        if (_currentSelection < 0)
            _currentSelection = _items.Count - 1;
        LeanTween.moveX(currentItem, currentItem.transform.position.x + PlatinioUI.instance.horizontalOffset, _animTime).setEase(_ease);
        LeanTween.moveX(beforeItem, beforeItem.transform.position.x + PlatinioUI.instance.horizontalOffset, _animTime).setEase(_ease).setOnComplete(() =>
        {
            isBusy = false;
        }
            ); ;

        if (nextItem != null)
            Destroy(nextItem);

        nextItem = currentItem;
        currentItem = beforeItem;
      


        if (_currentSelection - 1 >= 0)
        {
            beforeItem = Instantiate(_items[_currentSelection - 1], _window) as GameObject;
            beforeItem.transform.localScale = Vector3.one;
            beforeItem.transform.position = new Vector3(-PlatinioUI.instance.horizontalOffset, 0, 0);
        }
        else
        {
            beforeItem = Instantiate(_items[_items.Count - 1], _window) as GameObject;
            beforeItem.transform.localScale = Vector3.one;
            beforeItem.transform.position = new Vector3(-PlatinioUI.instance.horizontalOffset, 0, 0);
        }

        
    }

}

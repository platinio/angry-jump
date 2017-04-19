using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region SINGLETON
    private static GameManager gameManager;
    public static GameManager instance
    {
        get
        {
            if (!gameManager)
            {
                gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;



                if (!gameManager)
                    Debug.LogError("You need to have a GameManager script active in the scene");
            }

            return gameManager;
        }

    }
    #endregion SINGLETON

    #region PUBLIC_FIELDS
    public GameObject mark;
    public List<Color> colors;
    public float delay;
    public List<GameObject> normalPieces;
    public List<GameObject> damagePieces;
    public float platformsSpeed;
    public float platformOffset;
    [Range(0,100)]
    public int chanceToNormal;
    [Tooltip("Distance in x plane betewn player and piece")]
    public int distance;
    #endregion PUBLIC_FIELDS

    #region PRIVATE_FIELDS
    public float height;
    private float timer;
    private Player player;
    private Transform piecesParent;
    private Transform initialPlatform;
    private readonly int maxChance = 11;
    private bool isFirstTime = true;
    #endregion PRIVATE_FIELDS

    #region UNITY_EVENTS
    void Awake()
    {
        //get references
        player = GameObject.FindObjectOfType<Player>();

        if (player == null)
            Debug.LogError("Player no found in current scene");

        //create piecesParent
        piecesParent = new GameObject("PiecesParent").transform;

        //initial platform
        initialPlatform = GameObject.Find("SmallPlatform").transform;
    }

    void Start()
    {
        //set initial color to start platform
        for (int n = 0; n < initialPlatform.childCount; n++)
        {
            SpriteRenderer sprite = initialPlatform.GetChild(n).GetComponent<SpriteRenderer>();
            sprite.color = GetRandomColor();
        }

        //initilize values
        //get the initial height
        SpriteRenderer playerRender = player.GetComponent<SpriteRenderer>();
        float playerScale = player.transform.localScale.y;
        height = player.transform.position.y - ((playerRender.bounds.size.y / 2.0f) * playerScale);

        Instantiate(mark , new Vector2(player.transform.position.x , height) , Quaternion.identity);

        Debug.Log(height);
        chanceToNormal /= 10;

        player.Initialize();

        //start creating platforms
        CreateRandomPiece();

    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > delay)
            CreateRandomPiece();
            
        
    }


    #endregion UNITY_EVENTS
        
    public void UpdateHeight(float h)
    {        
        height +=  h;        
    }
    

    public void CreateRandomPiece()
    {
        //reset timr
        timer = 0;

        //random number betewn 0 and maxChance
        int r = Random.Range(0 , maxChance);
        int index;
        GameObject clone;

        //check if we instantiate a normal piece or a damage piece
        if (r <= chanceToNormal)
        {
            index = Random.Range(0, normalPieces.Count);
            clone = normalPieces[index];
        }
        else
        {
            index = Random.Range(0, normalPieces.Count);
            clone = damagePieces[index];
        }

        //create the piece
        InstantiatePiece(clone);
    }

    public Color GetRandomColor()
    {
        return colors[Random.Range(0 , colors.Count)];
    }

    private void InstantiatePiece(GameObject go)
    {
        //instantiate piece
        PlatformController clone = Instantiate(go , piecesParent).GetComponent<PlatformController>();
        clone.Initialize();

        //set random colors
        for(int n = 0 ; n < clone.transform.childCount ; n++)
        {
            SpriteRenderer sprite = clone.transform.GetChild(n).GetComponent<SpriteRenderer>();
            sprite.color = GetRandomColor();    
        }

        //if is the first time set the height and put it to 0
        if (isFirstTime)
        {
            UpdateHeight(clone.height / 2.0f);
            //clone.height = 0;
            isFirstTime = false;
        }

        int r = Random.Range(0, 2);
        Vector2 pos = r == 0 ? new Vector2(player.transform.position.x + distance, height) : new Vector2(player.transform.position.x - distance , height + platformOffset);
        clone.transform.localPosition = pos;

        //set speed
        clone.GetComponent<PlatformController>().speed = platformsSpeed * (r == 0 ? -1.0f : 1.0f);
              
    }

    
  

}

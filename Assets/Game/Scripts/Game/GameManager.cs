using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public float errorRange;
    public float coinDistance;
    public GameObject coinPrefab;
    public List<Color> colors;
    public float delay;
    public List<GameObject> normalPieces;
    public List<GameObject> damagePieces;
    public float platformSpeed;
    public float platformOffset;
    [Range(0,100)]
    public int chanceToNormal;
    [Tooltip("Distance in x plane betewn player and piece")]
    public int distance;
    public float height;

    [HideInInspector] public Transform initialPlatform;

    #endregion PUBLIC_FIELDS

    #region PRIVATE_FIELDS    
    private float timer;
    private Player player;
    private Transform piecesParent;
    private readonly int maxChance = 11;
    private bool isFirstTime = true;
    private bool wasReset;
    private SpriteRenderer playerSP;
    private PlatformController previusPlatform;
    private PlatformController currentPlatform;
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
        playerSP = player.GetComponent<SpriteRenderer>();
        ResetHeight();
              
                
        chanceToNormal /= 10;

        player.Initialize();

        //start creating platforms
        CreateRandomPiece();
        CreateCoin();
    }
    void Update()
    {
        
        timer += Time.deltaTime;
        if (timer > delay)
            CreateRandomPiece();

        
                
    }
    #endregion UNITY_EVENTS

    

    #region PUBLIC_METHODS

    public void CreateCoin()
    {
        Vector2 coinPos = player.transform.position;
        coinPos.y += coinDistance;

        Instantiate(coinPrefab , coinPos , Quaternion.identity);
    }

    public void UpdateHeight(float h)
    {
        height += h;
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

    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);        
    }

    public void AlingPlatforms()
    {
        StartCoroutine(AlingPlatformsRoutine());
    }

    IEnumerator AlingPlatformsRoutine()
    {
        
        float XPos = initialPlatform.position.x;
        Vector2 newPos;

        for (int n = 0; n < piecesParent.childCount; n++)
        {
            PlatformController platform = piecesParent.GetChild(n).GetComponent<PlatformController>();

            if (platform.isConnected)
            {
                newPos = platform.transform.position;
                newPos.x = XPos;
                platform.transform.position = newPos;
            }

            yield return new WaitForEndOfFrame();
        }
        
        /*
        PlatformController[] platforms = new PlatformController[piecesParent.childCount];
        float XPos = initialPlatform.position.x;

        for (int n = 0; n < piecesParent.childCount; n++)
        {
            PlatformController platform = piecesParent.GetChild(n).GetComponent<PlatformController>();

            if (platform.isConnected)
                platforms[n] = platform;              
            

            yield return new WaitForEndOfFrame();
        }

        for (int n = 0; n < platforms.Length; n++)
        {
            if (platforms[n] != null)
            {
                Vector2 newPos = platforms[n].transform.position;
                newPos.x = XPos;
                platforms[n].transform.position = newPos;
            }
                        
        }
        */

        Debug.Log("Player pos set");
        newPos = player.transform.position;
        newPos.x = XPos;
        player.transform.position = newPos;

        yield return new WaitForEndOfFrame();
            
    }


    #endregion PUBLIC_METHODS

    #region PRIVATE_METHODS
    //cada vez que se genera una nueva plataforma debemos chequear que vaya a la altura correcta
    private void InstantiatePiece(GameObject go)
    {
        //instantiate piece
        currentPlatform = Instantiate(go , piecesParent).GetComponent<PlatformController>();
        currentPlatform.Initialize();

        

        //set random colors
        for (int n = 0; n < currentPlatform.transform.childCount; n++)
        {
            SpriteRenderer sprite = currentPlatform.transform.GetChild(n).GetComponent<SpriteRenderer>();
            sprite.color = GetRandomColor();    
        }

        //if is the first time set the height and put it to 0
        if (isFirstTime)
        {
            UpdateHeight(currentPlatform.height / 2.0f);
            isFirstTime = false;

            if (currentPlatform.isVertical)
                UpdateHeight(0.4f);            

        }


        if (previusPlatform != null)
        {
            if (!previusPlatform.isVertical && currentPlatform.isVertical)
                UpdateHeight((previusPlatform.height / 2) + 0.12f);
            else if (previusPlatform.isVertical && currentPlatform.isVertical)
                UpdateHeight((previusPlatform.height / 2) + 0.52f);
            else if (previusPlatform.isVertical && !currentPlatform.isVertical)
                UpdateHeight((previusPlatform.height / 2) + 0.7f);
        }
        
        

        //get random
        int r = Random.Range(0, 2);
        //choose left or right
        Vector2 pos = r == 0 ? new Vector2(player.transform.position.x + distance, height) : new Vector2(player.transform.position.x - distance, height);
        pos = CheckPosition(pos);
        currentPlatform.transform.localPosition = pos;

        //set speed
        currentPlatform.speed = platformSpeed * (r == 0 ? -1.0f : 1.0f);
        previusPlatform = currentPlatform;
              
    }

    private Vector2 CheckPosition(Vector2 pos)
    {
        float playerScale = player.transform.localScale.y;
        float playerHeight = player.transform.position.y - ((playerSP.bounds.size.y / 2.0f) * playerScale);

        if (Vector2.Distance(new Vector2(0, pos.y), new Vector2(0, playerHeight)) > currentPlatform.distanceToReset)
        {            
            if(currentPlatform.isVertical)
                return new Vector2(pos.x, playerHeight + (currentPlatform.height + 0.1f));
            else
                return new Vector2(pos.x, playerHeight + (currentPlatform.height /2.0f));
        }            

        return pos;
    }

    private void ResetHeight()
    {
        float playerScale = player.transform.localScale.y;
        height = player.transform.position.y - ((playerSP.bounds.size.y / 2.0f) * playerScale);
    }
    #endregion PRIVATE_METHODS
}

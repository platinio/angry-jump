using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    Playing,
    GameOver,
    Pause
}


// ========================================= //
// Name: GameManager.cs
// Description: Controls game flow, instantiate new platforms, and decides positions
// ========================================= //
public class GameManager : MonoBehaviour
{
    AnimatorOverrideController overrideAnimator;
    //singleton
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
    public GameState gameState { get { return _gameState; } }
    public float errorRange;
    public float coinDistance;
    public GameObject coinPrefab;
    public List<Color> colors;
    public float delay;
    public List<GameObject> normalPieces;
    public List<GameObject> damagePieces;
    public float platformSpeed;
    public float platformOffset;
    [Range(0,100)] public int chanceToNormal;
    [Tooltip("Distance in x plane betewn player and piece")] public int distance;
    public float height;
    [HideInInspector] public Transform initialPlatform;
    public Transform playerPos;
    #endregion PUBLIC_FIELDS

    #region PRIVATE_FIELDS    
    private GameState _gameState;
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
        string characterName = GameSettings.characterName;
        GameObject playerGO = Instantiate(Resources.Load<GameObject>("Characters/" + characterName + "/" + characterName) , playerPos.position , Quaternion.identity);
        //get references
        player = playerGO.GetComponent<Player>();

        if (player == null)
        {
            Debug.LogError("GameManagerError: Player no found in current scene");
            //if we are in editor end game
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
            

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
        //Initialize height to 0
        ResetHeight();             
        //reduce to %        
        chanceToNormal /= 10;
        player.Initialize();
        //start creating platforms
        CreateRandomPiece();
        //create a initial coin
        CreateCoin();
    }
    void Update()
    {        
        //if the player miss the platform, we will wait a small amount of time before instantiate another to be sure there is no another platform
        timer += Time.deltaTime;
        if (timer > delay)
            CreateRandomPiece();            
    }
    #endregion UNITY_EVENTS
    

    #region PUBLIC_METHODS
    /// <summary>
    /// Instantiate a coin
    /// </summary>
    public void CreateCoin()
    {
        Vector2 coinPos = player.transform.position;
        coinPos.y += coinDistance;

        Instantiate(coinPrefab , coinPos , Quaternion.identity);
    }
    /// <summary>
    /// Updates height in h
    /// </summary>
    /// <param name="h">values add to height</param>
    public void UpdateHeight(float h)
    {
        height += h;
    }
    /// <summary>
    /// Create a random piece, can be a damage piece if we are on medium or hard mode
    /// </summary>
    public void CreateRandomPiece()
    {
        //reset timer, to instante a platform base on time
        timer = 0;
        
        int index;
        GameObject clone;

        //if we are on easy mode  just use normal pieces
        if (GameSettings.gameMode == GameMode.EASY)
        {
            index = Random.Range(0, normalPieces.Count);
            clone = normalPieces[index];
        }
        //if we are on medium or hard mode, maybe we can use damage pieces to kill player
        else
        {
            //random number betewn 0 and maxChance
            int r = Random.Range(0, maxChance);
            //check if we instantiate a normal piece or a damage piece
            if (r <= chanceToNormal)
            {
                index = Random.Range(0, normalPieces.Count);
                clone = normalPieces[index];
            }
            else
            {
                index = Random.Range(0, damagePieces.Count);
                clone = damagePieces[index];
            }
        }
                

        //create the piece
        InstantiatePiece(clone);
    }
    /// <summary>
    /// return a random Color
    /// </summary>
    /// <returns>gets a random color</returns>
    public Color GetRandomColor()
    {
        return colors[Random.Range(0 , colors.Count)];
    }
    /// <summary>
    /// ends the current game
    /// </summary>
    public void GameOver()
    {
        if (_gameState == GameState.GameOver)
            return;

        _gameState = GameState.GameOver;
        UIManager.instance.ShowGameOverTimer();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);        
    }
    /// <summary>
    /// Aling all the platforms to be in a line, call when the player gets a coin
    /// </summary>
    public void AlingPlatforms()
    {
        StartCoroutine(AlingPlatformsRoutine());
    }

    IEnumerator AlingPlatformsRoutine()
    {
        //gets initial x from the initial platform
        float XPos = initialPlatform.position.x;
        Vector2 newPos;
        //gets all the platforms and if it is conected(touching another platform), we set x to initial pos, maybe we can get this better saving all the platforms
        //in a array when the gets instantiate and delete it when it is destroy maybe can be more faster on running, but will occupe more RAM so we need more testing,
        //so far it goes well on mi phone
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
        //set pos of player
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
        //float playerHeight = player.transform.position.y - ((playerSP.bounds.size.y / 2.0f) * playerScale);
        float playerHeight = player.LastKnowPos.y - ((playerSP.bounds.size.y / 2.0f) * playerScale);

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

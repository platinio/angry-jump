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

    [Header("UI Elements")]
    public Text score;

    #endregion PUBLIC_FIELDS

    #region PRIVATE_FIELDS
    public float height;
    private float timer;
    private Player player;
    private Transform piecesParent;
    private Transform initialPlatform;
    private readonly int maxChance = 11;
    private bool isFirstTime = true;
    private SpriteRenderer playerSP;
    private int currentScore;
    private PlatformController previusPlatform;
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

    #region PUBLIC_METHODS
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

    public void AddScore()
    {
        currentScore++;
        int length = currentScore.ToString().Length;       
       
        string text = string.Empty;

        if (length == 1)
            text = "Score 000" + currentScore;
        else if(length == 2)
            text = "Score 00" + currentScore;
        else if (length == 3)
            text = "Score 0" + currentScore;
        else 
            text = "Score " + currentScore;

        
        score.text = text;
        
    }
    #endregion PUBLIC_METHODS

    #region PRIVATE_METHODS
    //cada vez que se genera una nueva plataforma debemos chequear que vaya a la altura correcta
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
            isFirstTime = false;
        }

        
        
        if (previusPlatform != null)
        {
            if (!previusPlatform.isVertical && clone.isVertical)
                UpdateHeight((previusPlatform.height / 2) + 0.15f); 
            else if(previusPlatform.isVertical && clone.isVertical)
                UpdateHeight((previusPlatform.height / 2) + 0.52f);
            else if (previusPlatform.isVertical && !clone.isVertical)
                UpdateHeight((previusPlatform.height / 2) + 0.0f);
        }

        //get random
        int r = Random.Range(0, 2);
        //choose left or right
        Vector2 pos = r == 0 ? new Vector2(player.transform.position.x + distance, height) : new Vector2(player.transform.position.x - distance, height); 
        pos = CheckPosition(pos , clone);
        clone.transform.localPosition = pos;

        //set speed
        clone.speed = platformSpeed * (r == 0 ? -1.0f: 1.0f);
        previusPlatform = clone;
              
    }

    private Vector2 CheckPosition(Vector2 pos , PlatformController PC)
    {
        float playerScale = player.transform.localScale.y;
        float playerHeight = player.transform.position.y - ((playerSP.bounds.size.y / 2.0f) * playerScale);

        if (Vector2.Distance(new Vector2(0, pos.y), new Vector2(0, playerHeight)) > errorRange)
        {
            return new Vector2(pos.x, playerHeight + (PC.height / 2.0f));
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

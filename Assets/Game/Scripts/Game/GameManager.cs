using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    
    
    //inspector
    [SerializeField]                private float               _errorRange         = 1.5f;
    [SerializeField]                private float               _coinDistance       = 9.0f;
    [SerializeField]                private GameObject          _coinPrefab         = null;
    [SerializeField]                private List<Color>         _colors             = null;
    [SerializeField]                private float               _delay              = 4.0f;
    [SerializeField]                private List<GameObject>    _normalPieces       = null;
    [SerializeField]                private List<GameObject>    _damagePieces       = null;
    [SerializeField]                private float               _platformSpeed      = 3.0f;
    [SerializeField]                private float               _platformOffset     = 0.0f;
    [SerializeField] [Range(0,100)] private int                 _chanceToNormal     = 62;    
    [SerializeField]                private int                 _distance           = 5;
    [SerializeField]                private float               _height             = 0;    
    [SerializeField]                private Transform           _playerPos          = null;

    //private  
    private Transform           _initialPlatform    = null;
    private GameState           _gameState          = GameState.Playing;
    private float               _timer              = 0.0f;
    private Player              _player             = null;
    private Transform           _piecesParent       = null;
    private readonly int        _maxChance          = 11;
    private bool                _isFirstTime        = true;
    private bool                _wasReset           = false;
    private SpriteRenderer      _playerSP           = null;
    private PlatformController  _previusPlatform    = null;
    private PlatformController  _currentPlatform    = null;


    //public   
    public GameState GameState          { get { return _gameState;          } }
    public Transform InitialPlatform    { get { return _initialPlatform;    } }

    
    #region UNITY_EVENTS
    void Awake()
    {
        //initialize player
        string characterName = GameSettings.characterName;
        string dir = "Characters/" + characterName + "/" + characterName;
        Debug.Log(dir);
        GameObject playerGO = Instantiate(Resources.Load<GameObject>(dir), _playerPos.position, Quaternion.identity);
        Debug.Log(playerGO.name);
        //get references
        _player = playerGO.GetComponent<Player>();

        //create piecesParent
        _piecesParent = new GameObject("PiecesParent").transform;
        _initialPlatform = GameObject.Find("SmallPlatform").transform;

       
    }

    void Start()
    {

        //set initial color to start platform
        for (int n = 0; n < _initialPlatform.childCount; n++)
        {
            SpriteRenderer sprite = _initialPlatform.GetChild(n).GetComponent<SpriteRenderer>();
            sprite.color = GetRandomColor();
        }

        //initilize values
        //get the initial height
        _playerSP = _player.GetComponent<SpriteRenderer>();
        //Initialize height to 0
        ResetHeight();
        //reduce to %        
        _chanceToNormal /= 10;
        _player.Initialize();
        //start creating platforms
        CreateRandomPiece();
        //create a initial coin
        CreateCoin();

        
    }

   

    void Update()
    {
        if (_gameState == GameState.GameOver)
            return;
        
        //if the player miss the platform, we will wait a small amount of time before instantiate another to be sure there is no another platform
        _timer += Time.deltaTime;
        if (_timer > _delay)
            CreateRandomPiece();            
    }
    #endregion UNITY_EVENTS
    

    #region PUBLIC_METHODS
    /// <summary>
    /// revive the player aftr use a life
    /// </summary>
    public void RevivePlayer()
    {
        //TO DO: discount a life
        _player.isDead                                  = false;        
        _player.GetComponent<Rigidbody2D>().velocity    = Vector2.zero;
        _player.transform.position                      = new Vector3(_initialPlatform.position.x , _player.LastKnowPos.y + 2.5f);
        _gameState                                      = GameState.Playing;
        _timer                                          = 0.0f;

        UIManager.instance.HideGameOverTimer();        
    }

    /// <summary>
    /// Instantiate a coin
    /// </summary>
    public void CreateCoin()
    {
        Vector2 coinPos = _player.transform.position;
        coinPos.y       += _coinDistance;

        Instantiate(_coinPrefab , coinPos , Quaternion.identity);
    }
    /// <summary>
    /// Updates height in h
    /// </summary>
    /// <param name="h">values add to height</param>
    public void UpdateHeight(float h)
    {
        _height += h;
        _timer  = 0.0f;
    }
    /// <summary>
    /// Create a random piece, can be a damage piece if we are on medium or hard mode
    /// </summary>
    public void CreateRandomPiece()
    {
        //reset timer, to instante a platform base on time
        _timer = 0;
        
        int index;
        GameObject clone;

        //if we are on easy mode  just use normal pieces
        if (GameSettings.gameMode == GameMode.EASY)
        {
            index = Random.Range(0, _normalPieces.Count);
            clone = _normalPieces[index];
        }
        //if we are on medium or hard mode, maybe we can use damage pieces to kill player
        else
        {
            //random number betewn 0 and maxChance
            int r = Random.Range(0, _maxChance);
            //check if we instantiate a normal piece or a damage piece
            if (r <= _chanceToNormal)
            {
                index = Random.Range(0, _normalPieces.Count);
                clone = _normalPieces[index];
            }
            else
            {
                index = Random.Range(0, _damagePieces.Count);
                clone = _damagePieces[index];
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
        return _colors[Random.Range(0 , _colors.Count)];
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// ends the current game
    /// </summary>
    public void GameOver()
    {
        if (_gameState == GameState.GameOver)
            return;

        SoundManager.instance.PlayGameOverSound();

        StaticData.numberOfDies++;

        if (StaticData.numberOfDies == 2)
        {
            Debug.Log("loading ads");
            StaticData.numberOfDies = 0;
        }

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
        float XPos = _initialPlatform.position.x;
        Vector2 newPos;
        //gets all the platforms and if it is conected(touching another platform), we set x to initial pos, maybe we can get this better saving all the platforms
        //in a array when the gets instantiate and delete it when it is destroy maybe can be more faster on running, but will occupe more RAM so we need more testing,
        //so far it goes well on mi phone
        for (int n = 0; n < _piecesParent.childCount; n++)
        {
            PlatformController platform = _piecesParent.GetChild(n).GetComponent<PlatformController>();

            if (platform.isConnected)
            {
                newPos = platform.transform.position;
                newPos.x = XPos;
                platform.transform.position = newPos;
            }

            yield return new WaitForEndOfFrame();
        }
        //set pos of player
        newPos = _player.transform.position;
        newPos.x = XPos;
        _player.transform.position = newPos;

        yield return new WaitForEndOfFrame();
            
    }


    #endregion PUBLIC_METHODS

    #region PRIVATE_METHODS
    //cada vez que se genera una nueva plataforma debemos chequear que vaya a la altura correcta
    private void InstantiatePiece(GameObject go)
    {
        //instantiate piece
        _currentPlatform = Instantiate(go , _piecesParent).GetComponent<PlatformController>();
        _currentPlatform.Initialize();

        

        //set random colors
        for (int n = 0; n < _currentPlatform.transform.childCount; n++)
        {
            SpriteRenderer sprite = _currentPlatform.transform.GetChild(n).GetComponent<SpriteRenderer>();
            sprite.color = GetRandomColor();    
        }

        //if is the first time set the height and put it to 0
        if (_isFirstTime)
        {
            UpdateHeight(_currentPlatform.height / 2.0f);
            _isFirstTime = false;

            if (_currentPlatform.isVertical)
                UpdateHeight(0.4f);            

        }


        if (_previusPlatform != null)
        {
            if (!_previusPlatform.isVertical && _currentPlatform.isVertical)
                UpdateHeight((_previusPlatform.height / 2) + 0.12f);
            else if (_previusPlatform.isVertical && _currentPlatform.isVertical)
                UpdateHeight((_previusPlatform.height / 2) + 0.52f);
            else if (_previusPlatform.isVertical && !_currentPlatform.isVertical)
                UpdateHeight((_previusPlatform.height / 2) + 0.7f);
        }
        
        

        //get random
        int r = Random.Range(0, 2);
        //choose left or right
        Vector2 pos = r == 0 ? new Vector2(_player.transform.position.x + _distance, _height) : new Vector2(_player.transform.position.x - _distance, _height);
        pos = CheckPosition(pos);
        _currentPlatform.transform.localPosition = pos;

        //set speed
        _currentPlatform.speed  = _platformSpeed * (r == 0 ? -1.0f : 1.0f);
        _previusPlatform        = _currentPlatform;
              
    }

    private Vector2 CheckPosition(Vector2 pos)
    {
        float playerScale = _player.transform.localScale.y;
        //float playerHeight = player.transform.position.y - ((playerSP.bounds.size.y / 2.0f) * playerScale);
        float playerHeight = _player.LastKnowPos.y - ((_playerSP.bounds.size.y / 2.0f) * playerScale);

        if (Vector2.Distance(new Vector2(0, pos.y), new Vector2(0, playerHeight)) > _currentPlatform.distanceToReset)
        {            
            if(_currentPlatform.isVertical)
                return new Vector2(pos.x, playerHeight + (_currentPlatform.height + 0.1f));
            else
                return new Vector2(pos.x, playerHeight + (_currentPlatform.height /2.0f));
        }            

        return pos;
    }

    private void ResetHeight()
    {
        float playerScale = _player.transform.localScale.y;
        _height = _player.transform.position.y - ((_playerSP.bounds.size.y / 2.0f) * playerScale);
    }
    #endregion PRIVATE_METHODS
}

﻿using System.Collections;
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
    public List<GameObject> normalPieces;
    public List<GameObject> damagePieces;
    [Range(0,100)]
    public int chanceToNormal;
    [Tooltip("Distance in x plane betewn player and piece")]
    public int distance;
    #endregion PUBLIC_FIELDS

    #region PRIVATE_FIELDS
    private float height;
    private Player player;
    private Transform piecesParent;
    private Transform initialPlatform;
    private readonly int maxChance = 11;
   
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

        chanceToNormal /= 10;
    }
    #endregion UNITY_EVENTS

    private float delay = 2.0f;
    private float timer = 5.0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > delay)
        {
            CreateRandomPiece();
            timer = 0;
        }
    }

    public void CreateRandomPiece()
    {
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
        float r = Random.Range(0, 256) / 255.0f;
        float g = Random.Range(0, 256) / 255.0f;
        float b = Random.Range(0, 256) / 255.0f;

        return new Color(r, g, b);
    }

    private void InstantiatePiece(GameObject go)
    {
        //instantiate piece
        GameObject clone = Instantiate(go , piecesParent) as GameObject;
        
        //set random colors
        for(int n = 0 ; n < clone.transform.childCount ; n++)
        {
            SpriteRenderer sprite = clone.transform.GetChild(n).GetComponent<SpriteRenderer>();
            sprite.color = GetRandomColor();

            if(n == 0)
                height += sprite.bounds.size.y * clone.transform.localScale.y;
        }

        int r = Random.Range(0, 2);
        Vector2 pos = r == 0 ? new Vector2(player.transform.position.x + distance, height) : new Vector2(player.transform.position.x - distance , height);
        clone.transform.position = pos;


        
    }

    
  

}
// These are the libraries this script uses

using System;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamesManager : MonoBehaviour {


    //Using headers to label variable groups in the Inspector
    [Header("ENEMY SETTINGS")]

    //These are the variables that will set the enemy parameters. We are setting them in the GameManager because there is... 
    //...no enemy in the scene by default. They get created at runtime from the GameManager script (see SpawnEnemy() below)

    private Vector3 origPos;

    public float initialEnemySpawnDelay;
    public float timeBetweenEnemySpawns;

    [Range(1, 10)] public int maxEnemysOnScreen;

    private int score;

    public Text scoreText, gamesOver;

    Player player;
    GameObject alien;
    GameObject playerS;

    //Player player;

    float xPos = -40f;
    float t;
    public bool gamesGameOver = false;
    // Start is always called once at the start of the game, or when the object containing this script first becomes active. 
    private void Start() {
        //In Unity, when you put a Prefab in a folder called "Assets/Resources", you can then use a Resources.Load method that loads the prefab at runtime.
        //This way you don't need to have the prefab in the scene when the game starts. 
        score = 0;
        t = initialEnemySpawnDelay;
        alien = Resources.Load<GameObject>("Alien") as GameObject;
        player = GameObject.Find("Player").GetComponent<Player>();
        playerS = Resources.Load<GameObject>("Player") as GameObject;
        scoreText.enabled = true;
        gamesOver.enabled = false;
        origPos = player.transform.position;
    }

    // In Unity, Update() is a function that runs every frame. 
    private void Update() {
        if (gamesGameOver == true) {
            gamesOver.text = "GAME OVER\nPress Space \nto Try Again";
            gamesOver.enabled = true;
        }
        if ((Input.GetKeyDown(KeyCode.Space)) && gamesGameOver == true) {
            GameObject players = Instantiate(playerS, origPos, playerS.transform.localRotation);
            var player = players.GetComponent<Player>();
            player.gamesDone = false;

            gamesGameOver = false;
            gamesOver.enabled = false;
            score = 0;
        }
        if (gamesGameOver == false) {
            player.playerLives = 1;
            gamesOver.enabled = false;
            gameOn();
            if (t > 0) {
                t -= Time.deltaTime;
            }
            else {
                SpawnEnemy();
                t = timeBetweenEnemySpawns;
            }
        }
        GameOver();
    }
    private void gameOn() {
        DisplayScores();
    }
    void SpawnEnemy() {
        var randPos = new System.Random(); // ?????
        Vector2 pos = new Vector2(xPos, randPos.Next(20, 29));
        var randSpeed = randPos.Next(2, 5);
        GameObject enemy = Instantiate(alien, pos, alien.transform.localRotation);
        var es = enemy.GetComponent<EnemySpawn>();
        es.moveSpeed = randSpeed;
    }
    public void IncreaseScore() {
        score += 1;
    }
    public void GameOver() {
        if (player.playerLives == 0) {
            gamesGameOver = true;
        }
    }
    void DisplayScores() {
        scoreText.text = "Score  " + score + "00".ToString();
    }
}

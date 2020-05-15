// These are the libraries this script uses

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamesManager : MonoBehaviour {


    //Using headers to label variable groups in the Inspector
    [Header("ENEMY SETTINGS")]

    //These are the variables that will set the enemy parameters. We are setting them in the GameManager because there is... 
    //...no enemy in the scene by default. They get created at runtime from the GameManager script (see SpawnEnemy() below)

    int enemyHealth = 1;
    public float initialEnemySpawnDelay;
    public float timeBetweenEnemySpawns;

    [Range(1, 10)] public int maxEnemysOnScreen;

    private int score;

    public Text scoreText,gamesOver;

    GameObject alien;

    Player player;

    float xPos = 10.5f;
    float t;
    bool gameOver = false;
    bool gameDone = false;
    float countdown = 0f;
    // Start is always called once at the start of the game, or when the object containing this script first becomes active. 
    private void Start() {
        //In Unity, when you put a Prefab in a folder called "Assets/Resources", you can then use a Resources.Load method that loads the prefab at runtime.
        //This way you don't need to have the prefab in the scene when the game starts. 
        score = 0;
        t = initialEnemySpawnDelay;
        alien = Resources.Load<GameObject>("Alien") as GameObject;
        player = GameObject.Find("Player").GetComponent<Player>();
        scoreText.enabled = true;
        gamesOver.enabled = false;

    }

    // In Unity, Update() is a function that runs every frame. 
    private void Update() {
        if (gameDone == true) {
            gamesOver.text = "GAME OVER";
            gamesOver.enabled = true;
            countdown += Time.deltaTime;
        }
        if (countdown > 5) {
            SceneManager.LoadScene(1);
        }
        if (gameOver == false) {
            //Debug.Log(player.playerLives);
            // This is a basic timer for spawning enemies
            //It uses the timeBetweenEnemySpawns value to space Spawning apart. We call SpawnEnemy() to do the actual spawning
            gameOn();
            if (t > 0) {
                t -= Time.deltaTime;
            }
            else {
                SpawnEnemy();
                t = timeBetweenEnemySpawns;
            }
        }

        GameOver(); //
    }
    private void gameOn() {
        DisplayScores();
    }
    void SpawnEnemy() {
        var randPos = new System.Random(); // ?????
        Vector2 pos = new Vector2(xPos, randPos.Next(-4, 4));
        var randSpeed = randPos.Next(2, 10);
        GameObject enemy = Instantiate(alien, pos, alien.transform.localRotation);
        var es = enemy.GetComponent<EnemySpawn>();
        es.moveSpeed = randSpeed;
    }
    public void IncreaseScore() {
        score += 1;
    }
    public void GameOver() {
        if (player.playerLives == 0) {
            gameOver = true;
            gameDone = true;
        }
    }
    void DisplayScores() {
        scoreText.text = "Score  " + score + "000".ToString();
    }
}

// This is the library this script uses
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

    //These are the variables that will set the enemy parameters. 
    // We are hiding them from the inspector because, we are setting them in the GameManager and then using those values...
    //...when an enemy is spawned
    GamesManager gameManager;

    [HideInInspector]
    public float moveSpeed = 5f;
    [HideInInspector]
    public int health = 1;


    // Start is always called once at the start of the game, or when the object containing this script first becomes active. 
    private void Start() {
        gameManager = GameObject.Find("GamesGameManager").GetComponent<GamesManager>();
    }

    // In Unity, Update() is a function that runs every frame. 
    private void Update() {
            transform.position = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y, -1);
    }
    public void OnTriggerEnter2D(Collider2D collision) {

        if (collision.gameObject.tag == "Delete") {
            Destroy(gameObject);
            gameManager.IncreaseScore();
        }
        if (collision.gameObject.tag == "Player") {
            Destroy(gameObject);
        }
    }
}
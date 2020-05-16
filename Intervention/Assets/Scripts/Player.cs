using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour {

    GamesManager gamesManager;

    float moveSpeed = 10;
    public float playerLives = 1;
    public bool gamesDone = false;

    float minX = -58f;
    float maxX = -43f;
    float minY = 20f;
    float maxY = 29f;

    // Update is called once per frame
    void Update() {
        if (gamesDone == false) {
            playerMovement();
        }
    }
    private void playerMovement() {
        float bounds = .1f;
        float movementX = Input.GetAxis("Horizontal");
        float movementY = Input.GetAxis("Vertical");
        float offSetX;
        float offSetY;

        offSetX = movementX * (moveSpeed * Time.deltaTime);
        offSetY = movementY * (moveSpeed * Time.deltaTime);

        if (transform.position.x > minX && transform.position.x < maxX) {
            transform.position = new Vector3(transform.position.x + offSetX, transform.position.y, transform.position.z);
        }
        if (transform.position.x < minX) {
            transform.position = new Vector3(transform.position.x + bounds, transform.position.y, transform.position.z);
        }
        if (transform.position.x > maxX) {
            transform.position = new Vector3(transform.position.x - bounds, transform.position.y, transform.position.z);
        }
        if (transform.position.y > minY && transform.position.y < maxY) {
            transform.position = new Vector3(transform.position.x, transform.position.y + offSetY, transform.position.z);
        }
        if (transform.position.y < minY) {
            transform.position = new Vector3(transform.position.x, transform.position.y + bounds, transform.position.z);
        }
        if (transform.position.y > maxY) {
            transform.position = new Vector3(transform.position.x, transform.position.y - bounds, transform.position.z);
        }

    }
    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            Destroy(gameObject);
            gamesDone = true;
            playerLives--;
        }
    }
}

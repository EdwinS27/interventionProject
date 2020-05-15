using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour {

    GamesManager gamesManager;

    float moveSpeed = 2;
    public float playerLives = 1;
    bool gamesDone = false;

    // Update is called once per frame
    void Update() {
        if (gamesDone == false) {
            playerMovement();
        }
    }
    private void playerMovement() {
        float movementX = Input.GetAxis("Horizontal");
        float movementY = Input.GetAxis("Vertical");
        float offSetX;
        float offSetY;

        offSetX = movementX * (moveSpeed * Time.deltaTime);
        offSetY = movementY * (moveSpeed * Time.deltaTime);

        if (transform.position.x > -7f && transform.position.x < 7) {
            transform.position = new Vector3(transform.position.x + offSetX, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -7) {
            transform.position = new Vector3(-6.9f, transform.position.y, transform.position.z);
        }
        if (transform.position.x > 7) {
            transform.position = new Vector3(6.9f, transform.position.y, transform.position.z);
        }
        if (transform.position.y > -4 && transform.position.y < 4) {
            transform.position = new Vector3(transform.position.x, transform.position.y + offSetY, transform.position.z);
        }
        if (transform.position.y < -4) {
            transform.position = new Vector3(transform.position.x, -3.9f, transform.position.z);
        }
        if (transform.position.y > 4) {
            transform.position = new Vector3(transform.position.x, 3.9f, transform.position.z);
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

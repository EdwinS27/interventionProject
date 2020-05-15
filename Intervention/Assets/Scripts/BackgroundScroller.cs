using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour {

    SpriteRenderer sprites;

    public Transform leftPos;
    public Transform rightPos;

    Vector2 pos1, pos2;

    public int orderNum;

    bool timeToLoop;

    Vector3 defaultPos;

    float speed = 5;

    void Start() {
        sprites = GetComponent<SpriteRenderer>();
        float width = sprites.bounds.extents.x * 2;
        transform.position = new Vector3(width * orderNum, 0, 0);
        defaultPos = transform.position;

        pos1 = leftPos.position;
        pos2 = rightPos.position;
    }

    // Update is called once per frame
    void Update() {
        float xOffset = speed * Time.deltaTime;
        transform.Translate(-xOffset, 0, 0);

        float xDifference = transform.position.x - defaultPos.x;

        if (Mathf.Abs(xDifference) >= sprites.bounds.extents.x * (2f + (float)orderNum * 2)) {
            reloadScreen();
        }

    }
    void reloadScreen() {
        transform.position = pos2;
    }
}

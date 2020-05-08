using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    float timeStart;
    int currentHour = 12;
    int lastHour;
    [SerializeField] Text inUnityTime;
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        monitorTime();
    }

    void monitorTime() {
        timeStart += Time.deltaTime;
        if (timeStart > 9f) {
            inUnityTime.text = "Time: " + currentHour + ":" + (Mathf.Round(timeStart) + " pm".ToString());
        }
        else {
            inUnityTime.text = "Time: " + currentHour + ":" + "0" + (Mathf.Round(timeStart) + " pm".ToString());
        }
        /*
         // This resets the day. 
        if (currentHour > lastHourOfDay - 1) {
            currentHour = 6;
            //currentDay++;
        }
        */
        if (timeStart >= 60f) {
            timeStart = 0f;
            currentHour++;
        }
    }
}

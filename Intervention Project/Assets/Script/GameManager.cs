using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public string userName;
    private int maxMessage = 25;

    public GameObject chatPanel, textObject;
    public InputField chatBox;

    public Color playerMessage;
    public Color info;

    [SerializeField] Text inUnityTime;
    [SerializeField] List<Message> messageList = new List<Message>();
    float timeStart;
    int currentHour = 12;
    int lastHour;
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (chatBox.text != "") {
            if (Input.GetKeyDown(KeyCode.Return)) {
                SendMessageToChat(userName + ": " +chatBox.text, Message.MessageType.playerMessage);
                chatBox.text = "";
            }
        }
        else {
            if (!chatBox.isFocused && Input.GetKeyDown(KeyCode.Return)) {
                chatBox.ActivateInputField();
            }
        }

        if (!chatBox.isFocused) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                SendMessageToChat("You Pressed the Space Key", Message.MessageType.info);
            }
        }
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

    public void SendMessageToChat(string text, Message.MessageType messageType) {
        if (messageList.Count >= maxMessage) {
            messageList.Remove(messageList[0]);
            // need to Destroy the game object because if only remove from message list the game object will still exist.
            Destroy(messageList[0].textObject.gameObject);
        }

        Message newMessage = new Message();
        newMessage.text = text;
        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = MessageTypeColor(messageType);

        messageList.Add(newMessage);
    }

    Color MessageTypeColor(Message.MessageType messageType) {

        Color color = info;
        // we will change the color of the messages being sent here depending on the user.
        switch (messageType) {
            case Message.MessageType.playerMessage:
                color = playerMessage;
                break;
                /*
            case Message.MessageType.info:
                color = info;
                break;
                */
        }

        return color;
    }
}
[System.Serializable]
public class Message {
    public string text;
    public Text textObject;
    public MessageType messageType;

    public enum MessageType {
        playerMessage, info
    }
}
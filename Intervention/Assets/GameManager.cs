using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour {
    string[] allQuestionsAnswers = {
        "I'll call you in an hour",
        "Let's do a zoom movie night!",
        "We can meet on Discord at 4",
        "Let's make a group chat",
        "What time can you call?",
        "Anyone want to meet over zoom"
    };

    string[] generalQuestions = { "What time can you call?", "Anyone want to meet over zoom" };
    string[] generalResponses = {"I'll call you in an hour",
        "Let's do a zoom movie night!",
        "We can meet on Discord at 4",
        "Let's make a group chat",
        "What game are you playing?",
        "Can I talk to you about the assignment later?",
        "Do you want to work on the assignment together?",
        "We can work on the assignment together"
    };

    string[] teacherResponses = { "Can't wait to see you all again!"
    };

    string[] gameQuestions = { "What game are you playing?"
    };
    string[] gameRespones = { "I love that game!"
    };

    string[] schoolQuestions = {"Can I talk to you about the assignment later?",
        "Do you want to work on the assignment together?"};
    string[] schoolRespones = { "We can work on the assignment together" };


    public string userName;
    private int maxMessage = 50;

    public GameObject slackMessageWindow, slackMessage, coverSlack;
    public InputField slackInputField;

    bool slackHidden = true;

    public Color playerMessage;
    public Color info;

    //[SerializeField] Text inUnityTime;
    [SerializeField] List<Message> messageList = new List<Message>();
    float timeStart;
    int currentHour = 12;
    int lastHour;

    int currentMessageCount = 0;
    int previousMessageCount = 0;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        BotCheckMessageAndRespond();
        trackMouse();
        if (slackInputField.text != "") {
            if (Input.GetKeyDown(KeyCode.Return)) {
                SendMessageToChat(userName + ": " + slackInputField.text, Message.MessageType.playerMessage);
                slackInputField.text = "";
            }
        }
        else {
            if (!slackInputField.isFocused && Input.GetKeyDown(KeyCode.Return)) {
                slackInputField.ActivateInputField();
            }
        }
        if (!slackInputField.isFocused) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                SendMessageToChat("You Pressed the Space Key", Message.MessageType.info);
            }
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
        GameObject newText = Instantiate(slackMessage, slackMessageWindow.transform);

        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = MessageTypeColor(messageType);

        messageList.Add(newMessage);
        currentMessageCount++;

        Debug.Log(messageList.Count);
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
    private void trackMouse() {
        // https://kylewbanks.com/blog/unity-2d-detecting-gameobject-clicks-using-raycasts
        // How I learned to use raycast 2D
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            Debug.Log("Mouse was clicked \n Mouse X: " + mousePos2D.x + " Mouse Y: " + mousePos2D.y);
            if (mousePos2D.x > -10 && mousePos2D.x < -6 && mousePos2D.y < 5 && mousePos2D.y > 3) {
                slackHidden = !slackHidden;
                coverSlack.SetActive(slackHidden);
            }
        }
    } // end of getObject


    // This will be the game class mate
    void BotCheckMessageAndRespond() {
        // When a new message comes in we want to check that message and have the "BOT" respond
        if (currentMessageCount > previousMessageCount) {
            string messageCheck = messageList[currentMessageCount - 1].text;
            if (messageList[currentMessageCount - 1].messageType == Message.MessageType.playerMessage) {
                Debug.Log("The player sent a message so we need to respond");
                // checking that the message had a question mark
                if (messageCheck.Contains("?")) {
                    Debug.Log("Check Message contains a ?");
                    if (messageCheck.Contains("game") && messageCheck.Contains("playing")) {
                        SendMessageToChat("Classmate: I am playing the founder", Message.MessageType.classMate);
                    }
                    if (messageCheck.Contains("assignment")) {
                        SendMessageToChat("Classmate: I didn't know we had an assignment! \n RIP..", Message.MessageType.classMate);
                    }
                    else {

                    }
                }
            }
            // will need to create a function to make sure that the previous message was not sent by the bot.
            previousMessageCount++;
        }
    }
}// end of Game Manager Class


[System.Serializable]
public class Message {
    public string text;
    public Text textObject;
    public MessageType messageType;

    public enum MessageType {
        playerMessage, info, classMate
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject chatPanel, textObject;
    public InputField chatBox;
    public GameObject slackWindow;

    public Color playerMessage;
    public Color info;

    [SerializeField] Text inUnityTime;
    [SerializeField] List<Message> messageList = new List<Message>();
    float timeStart;
    int currentHour = 12;
    int lastHour;

    int currentMessageCount = 0;
    int previousMessageCount = 0;
    void Start() {
        //slackWindow = 
    }

    // Update is called once per frame
    void Update() {
        getObject();
        BotCheckMessageAndRespond();
        if (chatBox.text != "") {
            if (Input.GetKeyDown(KeyCode.Return)) {
                SendMessageToChat(userName + ": " + chatBox.text, Message.MessageType.playerMessage);
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
    // This will be the game class mate
    void BotCheckMessageAndRespond() {
        bool checkForBot = false;
        // When a new message comes in we want to check that message and have the "BOT" respond
        if (currentMessageCount > previousMessageCount) {
            // A easier way of telling who is sending the message is by checking the message type.
            // If the message type was from anyone but player then it should continue with this method.
            // each message will be put into this messageCheck variable after its put into the system.
            string messageCheck = messageList[currentMessageCount - 1].text;
            if (messageList[currentMessageCount - 1].messageType == Message.MessageType.playerMessage) {
                Debug.Log("The player sent a message");
            }
            else {
                Debug.Log("The player did not send a message");
            }

            for (int i = 0; i < allQuestionsAnswers.Length; i++) {
                if (messageCheck.Contains(allQuestionsAnswers[i].ToLower())) {
                    checkForBot = true;
                    Debug.Log("Check for Bot was true");
                }
                if (checkForBot) {
                    break;
                }
                Debug.Log("Checking the array of all questions and answers, this is the current number : " + i);
            }

            if (checkForBot != true) {
                Debug.Log("Check for Bot was not true");
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
                else {

                }
            }
            // will need to create a function to make sure that the previous message was not sent by the bot.
            previousMessageCount++;
        } // end of if statement: messageChecker
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
    private void getObject() {
        RaycastHit click;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0)) {
            if (Physics.Raycast(ray, out click, 100.0f)) {
                if (click.transform != null) {
                    if (click.collider.gameObject.tag == "SlackButton") {

                    }

                }
            }
        }
    } // end of getObject
} // end of Class

[System.Serializable]
public class Message {
    public string text;
    public Text textObject;
    public MessageType messageType;

    public enum MessageType {
        playerMessage, info, classMate
    }
}
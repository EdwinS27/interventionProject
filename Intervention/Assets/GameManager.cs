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
    // will have max messages for each chat channel
    private int maxMessages = 25;

    public List<GameObject> messagingSystem;
    public GameObject slackMessage, coverSlack, professorChat, classmateChat1, classmateChat2, classmateChat3, classmateChat4, classmateChat5;
    public InputField slackInputField;

    bool slackHidden = true;

    public Color playerMessage;
    public Color info;

    //[SerializeField] Text inUnityTime;

    // Dont need to see these
    List<Message> professor1Chat = new List<Message>();
    List<Message> classmate1Chat = new List<Message>();
    List<Message> classmate2Chat = new List<Message>();
    List<Message> classmate3Chat = new List<Message>();
    List<Message> classmate4Chat = new List<Message>();
    List<Message> classmate5Chat = new List<Message>();

    // Currently Off
    float timeStart;
    int currentHour = 12;
    int lastHour;

    int whoAmIMessaging = 0;

    int currentMessageCount = 0;
    int previousMessageCount = 0;
    // Start is called before the first frame update
    void Start() {
        messagingSystem.Add(professorChat);
        messagingSystem.Add(classmateChat1);
        messagingSystem.Add(classmateChat2);
        messagingSystem.Add(classmateChat3);
        messagingSystem.Add(classmateChat4);
        messagingSystem.Add(classmateChat5);
        classmateChat1.SetActive(false);
        classmateChat2.SetActive(false);
        classmateChat3.SetActive(false);
        classmateChat4.SetActive(false);
        classmateChat5.SetActive(false);
        professorChat.SetActive(true);
    }

    // Update is called once per frame
    void Update() {
        clearChat();
        activateChat(); // calls activateChat to know where each messsage should go.
        BotCheckMessageAndRespond();
        trackMouse();
        useInputField();
    }
    void useInputField() {
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
        /*
        if (!slackInputField.isFocused) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                SendMessageToChat("You Pressed the Space Key", Message.MessageType.info);
            }
        }
        */
    }
    void clearChat() {
        if (professor1Chat.Count >= maxMessages) {
            professor1Chat.Remove(professor1Chat[0]);
            // need to Destroy the game object because if only remove from message list the game object will still exist.
            Destroy(professor1Chat[0].textObject.gameObject);
        }
        if (classmate1Chat.Count >= maxMessages) {
            classmate1Chat.Remove(classmate1Chat[0]);
            // need to Destroy the game object because if only remove from message list the game object will still exist.
            Destroy(classmate1Chat[0].textObject.gameObject);
        }
        if (classmate2Chat.Count >= maxMessages) {
            classmate2Chat.Remove(classmate2Chat[0]);
            // need to Destroy the game object because if only remove from message list the game object will still exist.
            Destroy(classmate2Chat[0].textObject.gameObject);
        }
        if (classmate3Chat.Count >= maxMessages) {
            classmate3Chat.Remove(classmate3Chat[0]);
            // need to Destroy the game object because if only remove from message list the game object will still exist.
            Destroy(classmate3Chat[0].textObject.gameObject);
        }
        if (classmate4Chat.Count >= maxMessages) {
            classmate4Chat.Remove(classmate4Chat[0]);
            // need to Destroy the game object because if only remove from message list the game object will still exist.
            Destroy(classmate4Chat[0].textObject.gameObject);
        }
        if (classmate5Chat.Count >= maxMessages) {
            classmate5Chat.Remove(classmate5Chat[0]);
            // need to Destroy the game object because if only remove from message list the game object will still exist.
            Destroy(classmate5Chat[0].textObject.gameObject);
        }
    }
    public void SendMessageToChat(string text, Message.MessageType messageType) {
        /*
        */
        Message newMessage = new Message();
        newMessage.text = text;
        // Debug.Log(whoAmIMessaging); // working as intended but this is just to see who should be getting a message
        GameObject newText = Instantiate(slackMessage, messagingSystem[whoAmIMessaging].transform);
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = MessageTypeColor(messageType);
        // rip can't take this out without having to expand the code more
        if (whoAmIMessaging == 0) {
            professor1Chat.Add(newMessage);
        }
        if (whoAmIMessaging == 1) {
            classmate1Chat.Add(newMessage);
        }
        if (whoAmIMessaging == 2) {
            classmate2Chat.Add(newMessage);
        }
        if (whoAmIMessaging == 3) {
            classmate3Chat.Add(newMessage);
        }
        if (whoAmIMessaging == 4) {
            classmate4Chat.Add(newMessage);
        }
        if (whoAmIMessaging == 5) {
            classmate5Chat.Add(newMessage);
        }
        currentMessageCount++;
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
            // Locations to "hide" the Slack Window
            if ((mousePos2D.x > -10 && mousePos2D.x < -6 && mousePos2D.y < 5 && mousePos2D.y > 3) ||
                (mousePos2D.x > -7 && mousePos2D.x < -6 && mousePos2D.y < -2 && mousePos2D.y > -5)) {
                slackHidden = !slackHidden;
                coverSlack.SetActive(slackHidden);
            }
            // For activating the professors chat
            if (mousePos2D.x > -0.5f && mousePos2D.x < 1.5f && mousePos2D.y < 4.2f && mousePos2D.y > 4) {
                whoAmIMessaging = 0;
            }
            // For activating the classmate one's chat
            if (mousePos2D.x > -0.5f && mousePos2D.x < 1.5f && mousePos2D.y < 3.6f && mousePos2D.y > 3.4f) {
                whoAmIMessaging = 1;
            }
            // For activating the classmate two's chat
            if (mousePos2D.x > -0.5f && mousePos2D.x < 1.5f && mousePos2D.y < 3.2f && mousePos2D.y > 2.9f) {
                whoAmIMessaging = 2;
            }
            // For activating the classmate three's chat
            if (mousePos2D.x > -0.5f && mousePos2D.x < 1.5f && mousePos2D.y < 2.8f && mousePos2D.y > 2.4f) {
                whoAmIMessaging = 3;
            }
            // For activating the classmate four's chat
            if (mousePos2D.x > -0.5f && mousePos2D.x < 1.5f && mousePos2D.y < 2.2f && mousePos2D.y > 2f) {
                whoAmIMessaging = 4;
            }
            // For activating the classmate five's chat
            if (mousePos2D.x > -0.5f && mousePos2D.x < 1.5f && mousePos2D.y < 1.7f && mousePos2D.y > 1.5f) {
                whoAmIMessaging = 5;
            }
        }
    } // end of getObject
    void activateChat() {
        if (whoAmIMessaging == 0) {
            classmateChat1.SetActive(false);
            classmateChat2.SetActive(false);
            classmateChat3.SetActive(false);
            classmateChat4.SetActive(false);
            classmateChat5.SetActive(false);
            professorChat.SetActive(true);
        }
        else if (whoAmIMessaging == 1) {
            classmateChat1.SetActive(true);
            classmateChat2.SetActive(false);
            classmateChat3.SetActive(false);
            classmateChat4.SetActive(false);
            classmateChat5.SetActive(false);
            professorChat.SetActive(false);
        }
        else if (whoAmIMessaging == 2) {
            classmateChat1.SetActive(false);
            classmateChat2.SetActive(true);
            classmateChat3.SetActive(false);
            classmateChat4.SetActive(false);
            classmateChat5.SetActive(false);
            professorChat.SetActive(false);
        }
        else if (whoAmIMessaging == 3) {
            classmateChat1.SetActive(false);
            classmateChat2.SetActive(false);
            classmateChat3.SetActive(true);
            classmateChat4.SetActive(false);
            classmateChat5.SetActive(false);
            professorChat.SetActive(false);
        }
        else if (whoAmIMessaging == 4) {
            classmateChat1.SetActive(false);
            classmateChat2.SetActive(false);
            classmateChat3.SetActive(false);
            classmateChat4.SetActive(true);
            classmateChat5.SetActive(false);
            professorChat.SetActive(false);
        }
        else if (whoAmIMessaging == 5) {
            classmateChat1.SetActive(false);
            classmateChat2.SetActive(false);
            classmateChat3.SetActive(false);
            classmateChat4.SetActive(false);
            classmateChat5.SetActive(true);
            professorChat.SetActive(false);
        }
        else {

        }
    }
    // This will be the game class mate
    void BotCheckMessageAndRespond() {
        /*
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
        */
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
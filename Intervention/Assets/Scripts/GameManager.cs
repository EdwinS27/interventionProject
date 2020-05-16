using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    string[] profResponse = { "Hello!", "What do you need help with?", "Let's schedule a 1-on-1 meeting through zoom.",
        "I have a few openings on my Calendly for tomorrow.", "How about we go through the code together", "I'll check the repo.",
        "Great, speak with you later.", "I don't play games.", "Sorry I am busy, I will get back to you later."
    };
    string[] classmateResponse = {"Hello.", "Hey", "Hi!","Let's do a zoom movie night!", "I did not understand the assignment either",
        "Can I talk to you about the assignment later?", "Do you want to work on the assignment together?", "We can work on the assignment together later."
            , "Sorry I am busy, I will get back to you later."
    };
    string[] gameResponse = { "I play the founder.", "What game do you play", "Cool", "Sounds fun", "Do you want to play a game together?", "Awesome. Let's do that some other time."
    };
    string[] names = { "Professor Kent: ", "Alexis: ", "Celia: ", "Debra: ",
        "Kevin: ", "Russell: "
    };

    public string userName;
    // will have max messages for each chat channel
    private int maxMessages = 8;

    int[] messagesSentPrev = { 0, 0, 0, 0, 0, 0, 0 };
    int[] messagesSentCurr = { 0, 0, 0, 0, 0, 0, 0 };

    public List<GameObject> messagingSystem;
    public GameObject slackMessage, professorChat, classmateChat1, classmateChat2, classmateChat3, classmateChat4, classmateChat5, slackApp, gameApp, gameCam;
    public InputField slackInputField;
    public Text currentlySpeaking;

    bool slackHidden = true;
    bool gameHidden = true;

    public Color playerMessage, info, professor;

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
        slackApp.SetActive(false);
        gameApp.SetActive(false);
        gameCam.SetActive(false);
        currentlySpeaking.enabled = true;
        currentlySpeaking.text = names[0];
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
                SendMessageToChat(userName + ": " + slackInputField.text, 0);
                slackInputField.text = "";
            }
        }
        else {
            if (!slackInputField.isFocused && Input.GetKeyDown(KeyCode.Return)) {
                slackInputField.ActivateInputField();
            }
        }
    }
    void clearChat() {
        if (professor1Chat.Count >= maxMessages) {
            professor1Chat.Remove(professor1Chat[0]);
            // need to Destroy the game object because if only remove from message list the game object will still exist.
            Destroy(professor1Chat[0].textObject.gameObject);
            messagesSentCurr[0]--;
            messagesSentPrev[0]--;
        }
        if (classmate1Chat.Count >= maxMessages) {
            classmate1Chat.Remove(classmate1Chat[0]);
            // need to Destroy the game object because if only remove from message list the game object will still exist.
            Destroy(classmate1Chat[0].textObject.gameObject);
            messagesSentCurr[1]--;
            messagesSentPrev[1]--;
        }
        if (classmate2Chat.Count >= maxMessages) {
            classmate2Chat.Remove(classmate2Chat[0]);
            // need to Destroy the game object because if only remove from message list the game object will still exist.
            Destroy(classmate2Chat[0].textObject.gameObject);
            messagesSentCurr[2]--;
            messagesSentPrev[2]--;
        }
        if (classmate3Chat.Count >= maxMessages) {
            classmate3Chat.Remove(classmate3Chat[0]);
            // need to Destroy the game object because if only remove from message list the game object will still exist.
            Destroy(classmate3Chat[0].textObject.gameObject);
            messagesSentCurr[3]--;
            messagesSentPrev[3]--;
        }
        if (classmate4Chat.Count >= maxMessages) {
            classmate4Chat.Remove(classmate4Chat[0]);
            // need to Destroy the game object because if only remove from message list the game object will still exist.
            Destroy(classmate4Chat[0].textObject.gameObject);
            messagesSentCurr[4]--;
            messagesSentPrev[4]--;
        }
        if (classmate5Chat.Count >= maxMessages) {
            classmate5Chat.Remove(classmate5Chat[0]);
            // need to Destroy the game object because if only remove from message list the game object will still exist.
            Destroy(classmate5Chat[0].textObject.gameObject);
            messagesSentCurr[5]--;
            messagesSentPrev[5]--;
        }
    }
    public void SendMessageToChat(string text, int messageType) {
        Message newMessage = new Message();
        newMessage.text = text;
        // Debug.Log(whoAmIMessaging); // working as intended but this is just to see who should be getting a message
        GameObject newText = Instantiate(slackMessage, messagingSystem[whoAmIMessaging].transform);
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = MessageTypeColor(messageType);
        newMessage.messageType = messageType;
        // rip can't take this out without having to expand the code more
        if (whoAmIMessaging == 0) {
            professor1Chat.Add(newMessage);
            messagesSentCurr[0] += 1;
            Debug.Log("Messages in the array" + messagesSentCurr[0]);
        }
        if (whoAmIMessaging == 1) {
            classmate1Chat.Add(newMessage);
            messagesSentCurr[1] += 1;
        }
        if (whoAmIMessaging == 2) {
            classmate2Chat.Add(newMessage);
            messagesSentCurr[2] += 1;
        }
        if (whoAmIMessaging == 3) {
            classmate3Chat.Add(newMessage);
            messagesSentCurr[3] += 1;
        }
        if (whoAmIMessaging == 4) {
            classmate4Chat.Add(newMessage);
            messagesSentCurr[4] += 1;
        }
        if (whoAmIMessaging == 5) {
            classmate5Chat.Add(newMessage);
            messagesSentCurr[5] += 1;
        }
    }
    Color MessageTypeColor(int messageType) {
        Color color = info;
        // we will change the color of the messages being sent here depending on the user.
        if (messageType > -1) {
            color = playerMessage;
        }
        return color;
    }
    private void trackMouse() {
        // https://kylewbanks.com/blog/unity-2d-detecting-gameobject-clicks-using-raycasts
        // How I learned to use raycast 2D
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            //Debug.Log("Mouse was clicked \n Mouse X: " + mousePos2D.x + " Mouse Y: " + mousePos2D.y);
            // Locations to "hide" the Slack Window
            if ((mousePos2D.x > -10 && mousePos2D.x < -6 && mousePos2D.y < 5 && mousePos2D.y > 3) ||
                (mousePos2D.x > -7 && mousePos2D.x < -6 && mousePos2D.y < -2 && mousePos2D.y > -5)) {
                slackHidden = !slackHidden;
                slackApp.SetActive(slackHidden);
            }
            if (mousePos2D.x > -8.5f && mousePos2D.x < -7.2f && mousePos2D.y < 1.2f && mousePos2D.y > 0.2f) {
                //Debug.Log("found the game");
                gameHidden = !gameHidden;
                gameApp.SetActive(gameHidden);
                gameCam.SetActive(gameHidden);

            }
            // For activating the professors chat
            if (mousePos2D.x > -0.5f && mousePos2D.x < 1.5f && mousePos2D.y < 4.2f && mousePos2D.y > 4) {
                whoAmIMessaging = 0;
                currentlySpeaking.text = names[0];
            }
            // For activating the classmate one's chat
            if (mousePos2D.x > -0.5f && mousePos2D.x < 1.5f && mousePos2D.y < 3.6f && mousePos2D.y > 3.4f) {
                whoAmIMessaging = 1;
                currentlySpeaking.text = names[1];
            }
            // For activating the classmate two's chat
            if (mousePos2D.x > -0.5f && mousePos2D.x < 1.5f && mousePos2D.y < 3.2f && mousePos2D.y > 2.9f) {
                whoAmIMessaging = 2;
                currentlySpeaking.text = names[2];
            }
            // For activating the classmate three's chat
            if (mousePos2D.x > -0.5f && mousePos2D.x < 1.5f && mousePos2D.y < 2.8f && mousePos2D.y > 2.4f) {
                whoAmIMessaging = 3;
                currentlySpeaking.text = names[3];
            }
            // For activating the classmate four's chat
            if (mousePos2D.x > -0.5f && mousePos2D.x < 1.5f && mousePos2D.y < 2.2f && mousePos2D.y > 2f) {
                whoAmIMessaging = 4;
                currentlySpeaking.text = names[4];
            }
            // For activating the classmate five's chat
            if (mousePos2D.x > -0.5f && mousePos2D.x < 1.5f && mousePos2D.y < 1.7f && mousePos2D.y > 1.5f) {
                whoAmIMessaging = 5;
                currentlySpeaking.text = names[5];
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
        // adjusting for Prof
        if (messagesSentCurr[0] > messagesSentPrev[0]) {
            var message = professor1Chat[messagesSentCurr[0] - 1];
            if (message.messageType == 0) {
                string messageCheck = message.text.ToLower();
                Debug.Log("messagetype " + message.messageType);
                //Debug.Log("The player sent a message so we need to respond");
                if (messageCheck.Contains("hello ") || messageCheck.Contains("hi ") || messageCheck.Contains("hello") || messageCheck.Contains("hi") || messageCheck.Contains("hey")) {
                    SendMessageToChat(names[0] + profResponse[0], 1);
                }
                if (messageCheck.Contains("help")|| messageCheck.Contains("assignment")) {
                    SendMessageToChat(names[0] + profResponse[1], 1);
                }
                if (messageCheck.Contains("error")) {
                    SendMessageToChat(names[0] + profResponse[2], 1);
                }
                if (messageCheck.Contains("meeting")) {
                    SendMessageToChat(names[0] + profResponse[2], 1);
                    SendMessageToChat(names[0] + profResponse[3], 1);
                }
                if (messageCheck.Contains("code")) {
                    SendMessageToChat(names[0] + profResponse[4], 1);
                }
                if (messageCheck.Contains("repo")) {
                    SendMessageToChat(names[0] + profResponse[5], 1);
                }
                if (messageCheck.Contains("thanks")) {
                    SendMessageToChat(names[0] + profResponse[6], 1);
                }
                if (messageCheck.Contains("game")) {
                    SendMessageToChat(names[0] + profResponse[7], 1);
                }
                if (messageCheck.Contains("assignment") || false || messageCheck.Contains("help") == false && messageCheck.Contains("hello") == false
                   && messageCheck.Contains("hi") && messageCheck.Contains("hey") == false) {
                    SendMessageToChat(names[0] + profResponse[8], 1);
                }
            }
            else {
                Debug.Log("Not the player don't respond");
            }
            messagesSentPrev[0] += 1;
        }
        // adjusting for 1
        if (messagesSentCurr[1] > messagesSentPrev[1]) {
            var message = classmate1Chat[messagesSentCurr[1] - 1];
            if (message.messageType == 0) {
                string messageCheck = message.text.ToLower();
                if (messageCheck.Contains("hello ") || messageCheck.Contains("hi ") || messageCheck.Contains("hello") || messageCheck.Contains("hi") || messageCheck.Contains("hey")) {
                    int randomResponse = Random.Range(0,3);
                    SendMessageToChat(names[1] + classmateResponse[randomResponse], 2);
                }
                if (messageCheck.Contains("movie")) {
                    int randomResponse = Random.Range(0, 3);
                    SendMessageToChat(names[1] + classmateResponse[3], 2);
                }
                if (messageCheck.Contains("assignment") && messageCheck.Contains("understand") && messageCheck.Contains("not") || messageCheck.Contains("didn")) {
                    SendMessageToChat(names[1] + classmateResponse[4], 2);
                }
                if (messageCheck.Contains("assignment") && messageCheck.Contains("help")) {
                    SendMessageToChat(names[1] + classmateResponse[5], 2);
                }
                if (messageCheck.Contains("assignment") && messageCheck.Contains("together")) {
                    SendMessageToChat(names[1] + classmateResponse[6], 2);
                }
                if (messageCheck.Contains("yes") && classmate1Chat[messagesSentCurr[1]-2].text.Contains(classmateResponse[6])) {
                    SendMessageToChat(names[1] + classmateResponse[7], 2);
                }
                if (messageCheck.Contains("game") && messageCheck.Contains("play") && messageCheck.Contains("do you")) {
                    SendMessageToChat(names[1] + gameResponse[0], 2);
                }
                if (messageCheck.Contains("game") || messageCheck.Contains("games")) {
                    SendMessageToChat(names[1] + gameResponse[1], 2);
                }
                if (messageCheck.Contains("play") && messageCheck.Contains("together") || messageCheck.Contains("play?")) {
                    SendMessageToChat(names[1] + gameResponse[4], 2);
                }
                if (messageCheck.Contains("yes") && classmate1Chat[messagesSentCurr[1] - 2].text.Contains(gameResponse[4])) {
                    SendMessageToChat(names[1] + gameResponse[5], 2);
                }
                if ((messageCheck.Contains("assignment") == false && messageCheck.Contains("help") == false && messageCheck.Contains("hello") == false
                   && messageCheck.Contains("hi") == false && messageCheck.Contains("hey") == false && messageCheck.Contains("yes") == false && messageCheck.Contains("game") == false) == true) {
                    SendMessageToChat(names[1] + classmateResponse[5], 2);
                }
                //Debug.Log(classmate1Chat[messagesSentCurr[1] - 1].text);
            }
            messagesSentPrev[1] = messagesSentPrev[1] + 1;
        }
        // adjusting for 2
        if (messagesSentCurr[2] > messagesSentPrev[2]) {
            var message = classmate1Chat[messagesSentCurr[2] - 1];
            if (message.messageType == 0) {
                string messageCheck = message.text.ToLower();
                if (messageCheck.Contains("hello ") || messageCheck.Contains("hi ") || messageCheck.Contains("hello") || messageCheck.Contains("hi") || messageCheck.Contains("hey")) {
                    int randomResponse = Random.Range(0, 3);
                    SendMessageToChat(names[2] + classmateResponse[randomResponse], 2);
                }
                if (messageCheck.Contains("movie")) {
                    int randomResponse = Random.Range(0, 3);
                    SendMessageToChat(names[2] + classmateResponse[3], 2);
                }
                if (messageCheck.Contains("assignment") && messageCheck.Contains("understand") && messageCheck.Contains("not") || messageCheck.Contains("didn")) {
                    SendMessageToChat(names[2] + classmateResponse[4], 2);
                }
                if (messageCheck.Contains("assignment") && messageCheck.Contains("help")) {
                    SendMessageToChat(names[2] + classmateResponse[5], 2);
                }
                if (messageCheck.Contains("assignment") && messageCheck.Contains("together")) {
                    SendMessageToChat(names[2] + classmateResponse[6], 2);
                }
                if (messageCheck.Contains("yes") && classmate1Chat[messagesSentCurr[1] - 2].text.Contains(classmateResponse[6])) {
                    SendMessageToChat(names[2] + classmateResponse[7], 2);
                }
                if (messageCheck.Contains("game") && messageCheck.Contains("play") && messageCheck.Contains("do you")) {
                    SendMessageToChat(names[2] + gameResponse[0], 2);
                }
                if (messageCheck.Contains("game") || messageCheck.Contains("games")) {
                    SendMessageToChat(names[2] + gameResponse[1], 2);
                }
                if (messageCheck.Contains("play") && messageCheck.Contains("together") || messageCheck.Contains("play?")) {
                    SendMessageToChat(names[2] + gameResponse[4], 2);
                }
                if (messageCheck.Contains("yes") && classmate2Chat[messagesSentCurr[1] - 2].text.Contains(gameResponse[4])) {
                    SendMessageToChat(names[2] + gameResponse[5], 2);
                }
                if ((messageCheck.Contains("assignment") == false && messageCheck.Contains("help") == false && messageCheck.Contains("hello") == false
                   && messageCheck.Contains("hi") == false && messageCheck.Contains("hey") == false && messageCheck.Contains("yes") == false && messageCheck.Contains("game") == false) == true) {
                    SendMessageToChat(names[2] + classmateResponse[5], 2);
                }
            }
            messagesSentPrev[2] = messagesSentPrev[2] + 1;
        }
        // adjusting for 3
        if (messagesSentCurr[3] > messagesSentPrev[3]) {
            var message = classmate1Chat[messagesSentCurr[3] - 1];
            string messageCheck = message.text.ToLower();
            if (message.messageType == 0) {
                if (messageCheck.Contains("hello ") || messageCheck.Contains("hi ") || messageCheck.Contains("hello") || messageCheck.Contains("hi") || messageCheck.Contains("hey")) {
                    int randomResponse = Random.Range(0, 3);
                    SendMessageToChat(names[3] + classmateResponse[randomResponse], 2);
                }
                if (messageCheck.Contains("movie")) {
                    int randomResponse = Random.Range(0, 3);
                    SendMessageToChat(names[3] + classmateResponse[3], 2);
                }
                if (messageCheck.Contains("assignment") && messageCheck.Contains("understand") && messageCheck.Contains("not") || messageCheck.Contains("didn")) {
                    SendMessageToChat(names[3] + classmateResponse[4], 2);
                }
                if (messageCheck.Contains("assignment") && messageCheck.Contains("help")) {
                    SendMessageToChat(names[3] + classmateResponse[5], 2);
                }
                if (messageCheck.Contains("assignment") && messageCheck.Contains("together")) {
                    SendMessageToChat(names[3] + classmateResponse[6], 2);
                }
                if (messageCheck.Contains("yes") && classmate1Chat[messagesSentCurr[1] - 2].text.Contains(classmateResponse[6])) {
                    SendMessageToChat(names[3] + classmateResponse[7], 2);
                }
                if (messageCheck.Contains("game") && messageCheck.Contains("play") && messageCheck.Contains("do you")) {
                    SendMessageToChat(names[3] + gameResponse[0], 2);
                }
                if (messageCheck.Contains("game") || messageCheck.Contains("games")) {
                    SendMessageToChat(names[3] + gameResponse[1], 2);
                }
                if (messageCheck.Contains("play") && messageCheck.Contains("together") || messageCheck.Contains("play?")) {
                    SendMessageToChat(names[3] + gameResponse[4], 2);
                }
                if (messageCheck.Contains("yes") && classmate3Chat[messagesSentCurr[1] - 2].text.Contains(gameResponse[4])) {
                    SendMessageToChat(names[3] + gameResponse[5], 2);
                }
                if ((messageCheck.Contains("assignment") == false && messageCheck.Contains("help") == false && messageCheck.Contains("hello") == false
                   && messageCheck.Contains("hi") == false && messageCheck.Contains("hey") == false && messageCheck.Contains("yes") == false && messageCheck.Contains("game") == false) == true) {
                    SendMessageToChat(names[3] + classmateResponse[5], 2);
                }
            }
            messagesSentPrev[3] = messagesSentPrev[3] + 1;
        }
        // adjusting for 4
        if (messagesSentCurr[4] > messagesSentPrev[4]) {
            var message = classmate1Chat[messagesSentCurr[4] - 1];
            if (message.messageType == 0) {
                string messageCheck = message.text.ToLower();
                if (messageCheck.Contains("hello ") || messageCheck.Contains("hi ") || messageCheck.Contains("hello") || messageCheck.Contains("hi")) {
                    int randomResponse = Random.Range(0, 3);
                    SendMessageToChat(names[4] + classmateResponse[randomResponse], 2);
                }
                if (messageCheck.Contains("movie")) {
                    int randomResponse = Random.Range(0, 3);
                    SendMessageToChat(names[4] + classmateResponse[3], 2);
                }
                if (messageCheck.Contains("assignment") && messageCheck.Contains("understand") && messageCheck.Contains("not") || messageCheck.Contains("didn")) {
                    SendMessageToChat(names[4] + classmateResponse[4], 2);
                }
                if (messageCheck.Contains("assignment") && messageCheck.Contains("help")) {
                    SendMessageToChat(names[4] + classmateResponse[5], 2);
                }
                if (messageCheck.Contains("assignment") && messageCheck.Contains("together")) {
                    SendMessageToChat(names[4] + classmateResponse[6], 2);
                }
                if (messageCheck.Contains("yes") && classmate4Chat[messagesSentCurr[1] - 2].text.Contains(classmateResponse[6])) {
                    SendMessageToChat(names[4] + classmateResponse[7], 2);
                }
                if (messageCheck.Contains("game") && messageCheck.Contains("play") && messageCheck.Contains("do you")) {
                    SendMessageToChat(names[4] + gameResponse[0], 2);
                }
                if (messageCheck.Contains("game") || messageCheck.Contains("games")) {
                    SendMessageToChat(names[4] + gameResponse[1], 2);
                }
                if (messageCheck.Contains("play") && messageCheck.Contains("together") || messageCheck.Contains("play?")) {
                    SendMessageToChat(names[4] + gameResponse[4], 2);
                }
                if (messageCheck.Contains("yes") && classmate4Chat[messagesSentCurr[1] - 2].text.Contains(gameResponse[4])) {
                    SendMessageToChat(names[4] + gameResponse[5], 2);
                }
                if ((messageCheck.Contains("assignment") == false && messageCheck.Contains("help") == false && messageCheck.Contains("hello") == false
                   && messageCheck.Contains("hi") == false && messageCheck.Contains("hey") == false && messageCheck.Contains("yes") == false && messageCheck.Contains("game") == false) == true) {
                    SendMessageToChat(names[4] + classmateResponse[8], 2);
                }
            }
            messagesSentPrev[4] = messagesSentPrev[4] + 1;
        }
        // adjusting for 5
        if (messagesSentCurr[5] > messagesSentPrev[5]) {
            var message = classmate1Chat[messagesSentCurr[5] - 1];
            if (message.messageType == 0) {
                string messageCheck = message.text.ToLower();
                if (messageCheck.Contains("hello ") || messageCheck.Contains("hi ") || messageCheck.Contains("hello") || messageCheck.Contains("hi") || messageCheck.Contains("hey")) {
                    int randomResponse = Random.Range(0, 3);
                    SendMessageToChat(names[4] + classmateResponse[randomResponse], 2);
                }
                if (messageCheck.Contains("movie")) {
                    int randomResponse = Random.Range(0, 3);
                    SendMessageToChat(names[5] + classmateResponse[3], 2);
                }
                if (messageCheck.Contains("assignment") && messageCheck.Contains("understand") && messageCheck.Contains("not") || messageCheck.Contains("didn")) {
                    SendMessageToChat(names[5] + classmateResponse[4], 2);
                }
                if (messageCheck.Contains("assignment") && messageCheck.Contains("help")) {
                    SendMessageToChat(names[5] + classmateResponse[5], 2);
                }
                if (messageCheck.Contains("assignment") && messageCheck.Contains("together")) {
                    SendMessageToChat(names[5] + classmateResponse[6], 2);
                }
                if (messageCheck.Contains("yes") && classmate1Chat[messagesSentCurr[1] - 2].text.Contains(classmateResponse[6])) {
                    SendMessageToChat(names[5] + classmateResponse[7], 2);
                }
                if (messageCheck.Contains("game") && messageCheck.Contains("play") && messageCheck.Contains("do you")) {
                    SendMessageToChat(names[4] + gameResponse[0], 2);
                }
                if (messageCheck.Contains("game") || messageCheck.Contains("games")) {
                    SendMessageToChat(names[4] + gameResponse[1], 2);
                }
                if (messageCheck.Contains("play") && messageCheck.Contains("together") || messageCheck.Contains("play?")) {
                    SendMessageToChat(names[4] + gameResponse[4], 2);
                }
                if (messageCheck.Contains("yes") && classmate4Chat[messagesSentCurr[1] - 2].text.Contains(gameResponse[4])) {
                    SendMessageToChat(names[4] + gameResponse[5], 2);
                }
                if ((messageCheck.Contains("assignment") == false && messageCheck.Contains("help") == false && messageCheck.Contains("hello") == false
                   && messageCheck.Contains("hi") == false && messageCheck.Contains("hey") == false && messageCheck.Contains("yes") == false) == true) {
                    SendMessageToChat(names[4] + classmateResponse[5], 2);
                }
            }
            messagesSentPrev[5] = messagesSentPrev[5] + 1;
        }
    }
}// end of Game Manager Class
[System.Serializable]
public class Message {
    public string text;
    public Text textObject;
    public int messageType;

    // 0 = user
    // 1 = professor
    // 2 = classmates
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class MessageStream : MonoBehaviour
{
    /*singleton*/
    public static MessageStream MSInstance {get; private set;}

    /*<-+-*-~-=-=-~-*-+-member-+-*-~-=-=-~-*-+->*/
    private List<Message> messageList = new List<Message>();
    private bool hideMessage = false;

    [SerializeField] TextMeshProUGUI infoPanel;

    /*<-+-*-~-=-=-~-*-+-eventMethod-+-*-~-=-=-~-*-+->*/
    void Awake() {
        if (MSInstance == null) {
            MSInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        messageList.Add(new Message("GAME START"));
    }

    void Update()
    {
        // フレームカウントを進める
        messageList.RemoveAll(message => message.addFrame());

        if (hideMessage) {
            infoPanel.text = "";
            return;
        }

        string showMessages = "";
        foreach (Message message in messageList) {
            showMessages += message.getMessage() + "\n";
        }
        
        infoPanel.text = showMessages;
    }

    /*<-+-*-~-=-=-~-*-+-method-+-*-~-=-=-~-*-+->*/
    public void addMessage(Message message) { messageList.Add(message); }
    public void setHideMessage(bool hideMessage) { this.hideMessage = hideMessage; }
}



public class Message
{
    /* Member */
    protected string message;
    protected int frameToLive = 300;
    protected int frameCount = 0;

    /* Message method */
    public Message(string message) { this.message = message; }
    public string getMessage() { return this.message; }

    /* Frame method */
    public void setFrameToLive(int frameToLive) { this.frameToLive = frameToLive; }
    public bool addFrame() {
        this.frameCount++;
        return this.frameToLive == this.frameCount;
    }
    public void resetFrame() { this.frameCount = 0; }
}



public class AcquireMessage : Message
{
    public AcquireMessage(Item item)
        : base(
            item.itemAmount() == 0
                ? $"{item.itemName()} を獲得"
                : $"{item.itemName()} x{item.itemAmount()} を獲得"
        ) {}
}



public class KillMessage : Message
{
    public KillMessage(string enemyName, int score)
        : base(
            $"{enemyName} を倒した\nスコア +{score}"
        )
    { }
}



public class HitMessage : Message
{
    public HitMessage(string enemyName)
        : base(
            $"{enemyName} に攻撃された"
        )
    { }
}

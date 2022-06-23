using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChatManager : MonoBehaviourPunCallbacks
{
    public List<string> chatList = new List<string>();
    public Button sendBtn;
    public Text chatLog;
    public Text chattingList;
    public InputField input;
    public ScrollRect scroll_rect;
    string chatters;

    float timer;
    bool timerOn;
    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        scroll_rect = GameObject.FindObjectOfType<ScrollRect>();
    }
    public void SendButtonOnClicked()
    {
        
        if (input.text.Equals(""))
            return;

        string msg = string.Format("[{0}] {1}", PhotonNetwork.LocalPlayer.NickName, input.text);
        photonView.RPC("ReceiveMsg", RpcTarget.OthersBuffered, msg);
        ReceiveMsg(msg);
        input.ActivateInputField();
        input.text = "";
        timerOn = true;
        timer = 0;
    }
    private void Update()
    {
        chatterUpdate();
        SetInvisible();
        if (Input.GetKeyDown(KeyCode.Return) && !input.isFocused)
        {
            SendButtonOnClicked();
            
        }

        
    }
    private void SetInvisible()
    {
        if (timerOn == true&&!input.isFocused)
        {
            timer += Time.deltaTime;
            if (timer >= 3)
            {
                timer = 0;
                Debug.Log("투명해지기");
                timerOn = false;
            }
        }
    }
    private void chatterUpdate()
    {
        chatters = "Player List\n";
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            chatters += p.NickName + "\n";
        }
        chattingList.text = chatters;
    }

    [PunRPC]
    public void ReceiveMsg(string msg)
    {
        chatLog.text += "\n" + msg;
        scroll_rect.verticalNormalizedPosition = 0.0f;
    }
}
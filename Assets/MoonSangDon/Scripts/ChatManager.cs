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
    public GameObject chatBox;
    public PlayerControl emoteCon;
    private string chatters;

    float timer;
    bool timerOn;
    bool isAct;
    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        scroll_rect = GameObject.FindObjectOfType<ScrollRect>();
        chatBox.SetActive(true);
        timerOn = true;
    }
    public void SendButtonOnClicked()
    {
        if (input.text.Equals(""))
            return;

        string msg = string.Format("[{0}] {1}", PhotonNetwork.LocalPlayer.NickName, input.text);
        photonView.RPC("ReceiveMsg", RpcTarget.OthersBuffered, msg);
        ReceiveMsg(msg);
        input.text = "";
        timerOn = true;
        timer = 0;
    }
    private void Update()
    {
        ChatterUpdate();
        SetInvisible();
        if (Input.GetKeyDown(KeyCode.Return) && !input.isFocused)
        {
            SendButtonOnClicked();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("리턴들어옴");
            if (!isAct||input.isFocused)
            {
                Debug.Log("!isAct");
                chatBox.SetActive(true);
                isAct = true;
                timerOn = false;
                input.ActivateInputField();

            }
            else if (isAct&&!input.isFocused)
            {
                Debug.Log("isAct");
                input.DeactivateInputField();
                isAct = false;
                timerOn = true;
                timer = 0;
            }
        }
    }
    private void SetInvisible()
    {
        if (timerOn == true)
        {
            timer += Time.deltaTime;
            if (timer >= 3)
            {
                    Debug.Log("투명해지기");
                    timerOn = false;
                chatBox.SetActive(false);
                //StartCoroutine(FadeInStart());
                timer = 0;
            }
        }
    }
    private void ChatterUpdate()
    {
        chatters = "Player List\n";
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            chatters += p.NickName + " , ";
        }
        chattingList.text = chatters;
    }

    [PunRPC]
    public void ReceiveMsg(string msg)
    {
        chatLog.text += "\n" + msg;
        scroll_rect.verticalNormalizedPosition = 0.0f;
    }

    public void Emote1()
    {
        //emoteCon.EmotePopUp(0);
        timer = 0;
    }
    public void Emote2()
    {
        //emoteCon.EmotePopUp(1);
        timer = 0;
    }
    public void Emote3()
    {
        //emoteCon.EmotePopUp(2);
        timer = 0;
    }
    public void Emote4()
    {
        //emoteCon.EmotePopUp(3);
        timer = 0;
    }
    public void Emote5()
    {
        //emoteCon.EmotePopUp(4);
        timer = 0;
    }
    public void Emote6()
    {
        //emoteCon.EmotePopUp(5);
        timer = 0;
    }
    public void Emote7()
    {
        //emoteCon.EmotePopUp(6);
        timer = 0;
    }
    public void Emote8()
    {
        //emoteCon.EmotePopUp(7);
        timer = 0;
    }
    public void Emote9()
    {
        //emoteCon.EmotePopUp(8);
        timer = 0;
    }
    public void Emote10()
    {
        //emoteCon.EmotePopUp(9);
        timer = 0;
    }
    public IEnumerator FadeInStart()
    {
        for (float f = 1; f > 0; f -= 0.02f)
        {
            Color c = chatBox.GetComponentInChildren<Image>().color;
            c.a = f;
            chatBox.GetComponentInChildren<Image>().color = c;
            yield return null;
        }
        yield return new WaitForSeconds(1);
        chatBox.SetActive(false);
        isAct = false;
    }

}
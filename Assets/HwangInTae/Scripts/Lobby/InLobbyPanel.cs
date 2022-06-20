using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class InLobbyPanel : MonoBehaviour
{
    public GameObject roomContent;
    public GameObject roomEntryPrefab;
    public Sprite lockImage;
    private Dictionary<string, RoomInfo> cachedRoomList;//TODO:TODO:
    private Dictionary<string, GameObject> roomListEntries;//TODO:TODO:

    private void Start()
    {
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();
    }

    private void OnDisable()
    {
        cachedRoomList.Clear();
        roomListEntries.Clear();
    }

    public void OnBackButtonClicked()
    {
        PhotonNetwork.LeaveLobby();
        LobbyManager.instance.SetActivePanel(LobbyManager.PANEL.Connect);
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
    }

    public void ClearRoomList()
    {
        cachedRoomList.Clear();
        ClearRoomListView();
    }

    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            GameObject entry = Instantiate(roomEntryPrefab);
            entry.transform.SetParent(roomContent.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<RoomEntry>().Initialize((string)info.CustomProperties["displayname"], (string)info.CustomProperties["password"], (byte)info.PlayerCount, info.MaxPlayers);
            //CustomProerties[""]시발새끼
            if(info.CustomProperties["password"]!=null)
            {
                //비밀번호 프리팹 보이게 하기
            }
            roomListEntries.Add(info.Name, entry);
        }
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }
            }
            else if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }

    private void ClearRoomListView()
    {
        foreach (GameObject entry in roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        roomListEntries.Clear();
    }
}

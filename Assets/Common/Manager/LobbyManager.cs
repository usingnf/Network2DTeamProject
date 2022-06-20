﻿using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager instance { get; private set; }

    [Header("Panel")]
    public LoginPanel loginPanel;
    public InConnectPanel inConnectPanel;
    public CreateRoomPanel createRoomPanel;
    public InLobbyPanel inLobbyPanel;
    public InRoomPanel inRoomPanel;
    public InfoPanel infoPanel;
    public OptionPanel optionPanel;

    #region UNITY

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            SetActivePanel(PANEL.Connect);//여기 참고할것
        }
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public enum PANEL { Login, Connect, Lobby, Room, CreateRoom, Option}
    public void SetActivePanel(PANEL panel)
    {
        loginPanel.gameObject.SetActive(panel == PANEL.Login);
        inConnectPanel.gameObject.SetActive(panel == PANEL.Connect);
        createRoomPanel.gameObject.SetActive(panel == PANEL.CreateRoom);
        inLobbyPanel.gameObject.SetActive(panel == PANEL.Lobby);
        inRoomPanel.gameObject.SetActive(panel == PANEL.Room);
        optionPanel.gameObject.SetActive(panel == PANEL.Option);
    }

    public void ShowError(string error)
    {
        infoPanel.ShowError(error);
    }

    #endregion UNITY

    #region PHTON CALLBACK

    public override void OnConnectedToMaster()
    {
        SetActivePanel(PANEL.Connect);
    }
    

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        inLobbyPanel.OnRoomListUpdate(roomList);
    }
    public override void OnJoinedLobby()
    {
        inLobbyPanel.ClearRoomList();
    }

    public override void OnLeftLobby()
    {
        inLobbyPanel.ClearRoomList();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetActivePanel(PANEL.Connect);
        infoPanel.ShowError("Create Room Failed with Error(" + returnCode + ") : " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if (true)
        {
            SetActivePanel(PANEL.Connect);
            infoPanel.ShowError("비밀번호가 틀렸어요! \n다시 입력해주세요 \n^오^");
        }
        else
        {
            SetActivePanel(PANEL.Connect);
            infoPanel.ShowError("Join Room Failed with Error(" + returnCode + ") : " + message);
        }
        
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = "데구리방 " + Random.Range(1000, 10000);
        RoomOptions options = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.CreateRoom(roomName, options, null);
        Debug.Log(roomName + " 방만듬");
    }

    public override void OnJoinedRoom()
    {
        // TODO : SetActivePanel(PANEL.Room);
        PhotonNetwork.LoadLevel(1);
        Debug.Log("방들어감");
    }

    public override void OnLeftRoom()
    {
        SetActivePanel(PANEL.Connect);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        inRoomPanel.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        inRoomPanel.OnPlayerLeftRoom(otherPlayer);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        inRoomPanel.OnMasterClientSwitched(newMasterClient);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        inRoomPanel.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
    }

    public void LocalPlayerPropertiesUpdated()
    {
        inRoomPanel.LocalPlayerPropertiesUpdated();
    }

    #endregion
}

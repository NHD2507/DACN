using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button createRoom;
    [SerializeField] private Button joinRoom;
    [SerializeField] private TMP_InputField createInput;
    [SerializeField] private TMP_InputField joinInput;
    [SerializeField] private GameObject playerPrefab; // Prefab của player cần spawn

    public void Start()
    {
        // Kết nối với Photon nếu chưa kết nối
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        createRoom.onClick.AddListener(CreateRoom);
        joinRoom.onClick.AddListener(JoinRoom);
    }

    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(createInput.text))
        {
            PhotonNetwork.CreateRoom(createInput.text);
        }
        else
        {
            Debug.LogWarning("Room name cannot be empty!");
        }
    }

    public void JoinRoom()
    {
        if (!string.IsNullOrEmpty(joinInput.text))
        {
            PhotonNetwork.JoinRoom(joinInput.text);
        }
        else
        {
            Debug.LogWarning("Room name cannot be empty!");
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Failed to join room: {message} (Error code: {returnCode})");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room Successfully!");

        // Load scene "Game" nếu không phải trong scene đó
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1) // Người đầu tiên trong phòng có thể load scene
        {
            PhotonNetwork.LoadLevel("Game");
        }
        else
        {
            SpawnPlayer(); // Spawn player khi đã ở scene "Game"
        }
    }

    private void SpawnPlayer()
    {
        // Tạo vị trí ngẫu nhiên để spawn player
        Vector3 spawnPosition = new Vector3(
            Random.Range(-10f, 10f),
            1f,
            Random.Range(-10f, 10f)
        );

        // Instantiate player prefab qua Photon
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
    }
}

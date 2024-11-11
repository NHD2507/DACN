
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMessageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;


    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }

    private void Start()
    {
        GameMultiplayer.Instance.OnFailedToJoinGame += GameMultiplayer_OnFailedToJoinGame;
        GameLobby.Instance.OnCreateLobbyStarted += GameLobby_OnCreateLobbyStarted;
        GameLobby.Instance.OnCreateLobbyFailed += GameLobby_OnCreateLobbyFailed;
        GameLobby.Instance.OnJoinStarted += GameLobby_OnJoinStarted;
        GameLobby.Instance.OnJoinFailed += GameLobby_OnJoinFailed;
        GameLobby.Instance.OnQuickJoinFailed += GameLobby_OnQuickJoinFailed;

        Hide();
    }

    private void GameLobby_OnQuickJoinFailed(object sender, System.EventArgs e)
    {
        ShowMessage("Không tìm thấy sảnh để tham gia nhanh!");
    }

    private void GameLobby_OnJoinFailed(object sender, System.EventArgs e)
    {
        ShowMessage("Tham gia sảnh thất bại!");
    }

    private void GameLobby_OnJoinStarted(object sender, System.EventArgs e)
    {
        ShowMessage("Đang vào sảnh...");
    }

    private void GameLobby_OnCreateLobbyFailed(object sender, System.EventArgs e)
    {
        ShowMessage("Tạo sảnh thất bại!");
    }

    private void GameLobby_OnCreateLobbyStarted(object sender, System.EventArgs e)
    {
        ShowMessage("Đang tạo sảnh...");
    }

    private void GameMultiplayer_OnFailedToJoinGame(object sender, System.EventArgs e)
    {
        if (NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMessage("Kết nối thất bại");
        }
        else
        {
            ShowMessage(NetworkManager.Singleton.DisconnectReason);
        }
    }

    private void ShowMessage(string message)
    {
        Show();
        messageText.text = message;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameMultiplayer.Instance.OnFailedToJoinGame -= GameMultiplayer_OnFailedToJoinGame;
        GameLobby.Instance.OnCreateLobbyStarted -= GameLobby_OnCreateLobbyStarted;
        GameLobby.Instance.OnCreateLobbyFailed -= GameLobby_OnCreateLobbyFailed;
        GameLobby.Instance.OnJoinStarted -= GameLobby_OnJoinStarted;
        GameLobby.Instance.OnJoinFailed -= GameLobby_OnJoinFailed;
        GameLobby.Instance.OnQuickJoinFailed -= GameLobby_OnQuickJoinFailed;
    }
}

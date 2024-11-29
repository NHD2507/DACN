using Photon.Pun;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;

    // Khoảng giới hạn spawn trên không gian 3D
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = 1f;
    public float maxY = 5f;
    public float minZ = -10f;
    public float maxZ = 10f;

    private void Start()
    {
        // Chỉ spawn người chơi khi kết nối với Photon thành công
        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        // Tạo vị trí spawn ngẫu nhiên trong không gian 3D
        Vector3 randomPosition = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            Random.Range(minZ, maxZ)
        );

        // Instantiate người chơi qua PhotonNetwork
        PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
    }
}

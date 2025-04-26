using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void StartGame()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }

    public void DeBuging()
    {
        Debug.Log("GOGO");
    }
}

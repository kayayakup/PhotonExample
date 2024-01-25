using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class BallScript : MonoBehaviour
{
    Rigidbody BallRb;
    PhotonView PV;

    int player1Score = 0, player2Score = 0;
    public TextMeshProUGUI player1ScoreText, player2ScoreText;
    public GameObject panel;

    private void Start()
    {
        Time.timeScale = 1;
        BallRb = GetComponent<Rigidbody>();
        PV= GetComponent<PhotonView>();
    }

    [PunRPC]
    public void Starter()
    {
        BallRb.velocity = new Vector3(5, 5, 0);

        ShowScore();
    }

    public void ShowScore()
    {
        player1ScoreText.text = PhotonNetwork.PlayerList[0].NickName + ": " + player1Score.ToString();
        player2ScoreText.text = PhotonNetwork.PlayerList[1].NickName + ": " + player2Score.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (PV.IsMine)
        {
            if (collision.gameObject.name == "1.Kale")
            {
                PV.RPC("Gol", RpcTarget.All, 0, 1);
            }
            else if(collision.gameObject.name == "2.Kale")
            {
                PV.RPC("Gol", RpcTarget.All, 1, 0);
            }
        }
    }

    [PunRPC]
    public void Gol(int player1, int player2)
    {
        player1Score += player1;
        player2Score += player2;

        ShowScore();

        Service();
    }

    public void Service()
    {
        BallRb.velocity = Vector3.zero;
        transform.position = new Vector3(0, 0, -1);

        BallRb.velocity = new Vector3(5, 5, 0);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (player1Score > 4 || player2Score > 4)
        {
            panel.SetActive(true);
            Time.timeScale = 0;
        }
    }
}

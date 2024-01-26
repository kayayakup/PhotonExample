using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Pun.UtilityScripts;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    PhotonView photonView;

    TextMeshProUGUI yazi;

    // Start is called before the first frame update
    void Start()
    {
        yazi = GameObject.Find("yazi").GetComponent<TextMeshProUGUI>();
        photonView = GetComponent<PhotonView>();

        if(photonView.IsMine)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                transform.position = new Vector3(10, 0, -1);
                InvokeRepeating(nameof(PlayerControl), 0, 0.5f);
            }
            else if (!PhotonNetwork.IsMasterClient)
            {
                transform.position = new Vector3(-10, 0, -1);
            }
        }
    }

    void PlayerControl()
    {
        if(PhotonNetwork.PlayerList.Length==2)
        {
            photonView.RPC("DeleteLetter", RpcTarget.All, null);
            GameObject.Find("Ball").GetComponent<PhotonView>().RPC("Starter",RpcTarget.All, null);
            CancelInvoke(nameof(PlayerControl));
        }
    }


    [PunRPC]
    private void DeleteLetter()
    {
        yazi.text = null;
    }
    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            Move();
            if(transform.position.y > 3)
            {
                transform.position = new Vector3(transform.position.x, 3, transform.position.z);
            }
            if(transform.position.y < -3)
            {
                transform.position = new Vector3(transform.position.x, -3, transform.position.z);
            }
        }
    }

    void Move()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = new Vector3(transform.position.x, mousePos.y, -1);
    }
}

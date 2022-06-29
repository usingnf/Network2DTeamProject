using Photon.Pun;
using UnityEngine;

public class MoonsEdit_button : MonoBehaviourPun
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine == true)
            {
                GameManager.Instance.PrintInfo(
                string.Format("{0}가 리셋발판을 밟았습니다", other.gameObject.name));

                //SoundManager.Instance.PlaySound("Die", this.transform.position, 1, 1);
                PhotonNetwork.LoadLevel("StageScene_5");
                //other.transform.GetComponent<PlayerControl>().Return();
            }

        }
    }
}

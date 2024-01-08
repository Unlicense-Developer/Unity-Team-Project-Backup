using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public void TeleportPlayer()
    {
        // 플레이어 GameObject를 태그로 찾기
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // 플레이어가 제대로 찾아졌는지 확인
        if (player != null)
        {
            // 플레이어에서 PlayerInteract 컴포넌트 가져오기
            PlayerInteract playerInteract = player.GetComponent<PlayerInteract>();

            // PlayerInteract 컴포넌트가 제대로 찾아졌는지 확인
            if (playerInteract != null)
            {   // 순간이동
                Vector3 newPosition = this.transform.position;
                // 순간이동 후 interactUI 비활성화
                playerInteract.DisableAllInteractUIs();
                Debug.Log("순간이동");

                Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();

                if (playerRigidbody != null)
                {
                    playerRigidbody.MovePosition(newPosition);
                }
                else
                {
                    CharacterController playerController = player.GetComponent<CharacterController>();
                    if (playerController != null)
                    {
                        playerController.enabled = false;
                        player.transform.position = newPosition;
                        playerController.enabled = true;
                    }
                    else
                    {
                        player.transform.position = newPosition;
                    }
                }
            }
        }
    }
}
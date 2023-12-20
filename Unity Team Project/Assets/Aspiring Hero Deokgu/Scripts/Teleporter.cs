using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public void TeleportPlayer()
    {
        // �÷��̾� GameObject�� �±׷� ã��
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // �÷��̾ ����� ã�������� Ȯ��
        if (player != null)
        {
            // �÷��̾�� PlayerInteract ������Ʈ ��������
            PlayerInteract playerInteract = player.GetComponent<PlayerInteract>();

            // PlayerInteract ������Ʈ�� ����� ã�������� Ȯ��
            if (playerInteract != null)
            {   // �����̵�
                Vector3 newPosition = this.transform.position;
                // �����̵� �� interactUI ��Ȱ��ȭ
                playerInteract.DisableAllInteractUIs();
                Debug.Log("�����̵�");

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
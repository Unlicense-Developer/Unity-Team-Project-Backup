using UnityEngine;

public class ChestController : MonoBehaviour
{
    public GameObject interactionUI;
    Animator ani;
    public bool isOpen = false;

    private void Start()
    {
        ani = GetComponent<Animator>();
    }

    public void ToggleChest()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PlayerInteract playerInteract = player.GetComponent<PlayerInteract>();
            
            if (playerInteract != null)
            {
                if (!isOpen)
                {
                    // ���� ���� ���� �ִϸ��̼� �� ����
                    ani.SetBool("IsOpen", true);
                    Debug.Log("���ڰ� ���Ƚ��ϴ�.");
                    isOpen = true;
                }
                else
                {
                    // ���� ���� �ݱ� �ִϸ��̼� �� ����
                    ani.SetBool("IsOpen", false);
                    Debug.Log("���ڰ� �������ϴ�.");
                    isOpen = !isOpen;
                }
            }
        }   
    }
}
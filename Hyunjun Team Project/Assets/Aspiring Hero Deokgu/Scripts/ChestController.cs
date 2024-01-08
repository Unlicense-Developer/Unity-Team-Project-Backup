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
                    // 보물 상자 열기 애니메이션 및 로직
                    ani.SetBool("IsOpen", true);
                    Debug.Log("상자가 열렸습니다.");
                    isOpen = true;
                }
                else
                {
                    // 보물 상자 닫기 애니메이션 및 로직
                    ani.SetBool("IsOpen", false);
                    Debug.Log("상자가 닫혔습니다.");
                    isOpen = !isOpen;
                }
            }
        }   
    }
}
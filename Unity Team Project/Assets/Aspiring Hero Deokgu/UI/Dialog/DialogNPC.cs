using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogNPC : MonoBehaviour
{
    [SerializeField]
    private DialogSystem dialogSystem01;

    [SerializeField]
    private DialogSystem dialogSystem02;

    [SerializeField]
    private GameObject buttonCanvas;

    [SerializeField]
    private string sceneTargetName; // �ε��� ���� �̸�

    bool isTalking = false;

    public IEnumerator Talk()
    {
        // ��ȭ �ý��ۿ� ���� ��ȭ ����
        if (dialogSystem01.isFirst == true && isTalking == false)
        {
            // ù ��° ��� �б� ����
            isTalking = true;
            yield return new WaitUntil(() => dialogSystem01.UpdateDialog());

            // ������ Ȯ�� �� ǥ��
            if (buttonCanvas != null)
            {
                DisplayChoices();
            }

            // �� ��° ��� �б� ����
            if (dialogSystem02 != null)
            {
                yield return new WaitUntil(() => dialogSystem02.UpdateDialog());
            }

            // ��ȭ ���� �� ó��
            if (buttonCanvas == null)
            {
                isTalking = false;
            }
        }
    }
    private void DisplayChoices()
    {
        // ��� ��ư�� ǥ���մϴ�
        buttonCanvas.SetActive(true);
    }

    public void TeleportPlayer(Vector3 newPosition)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = newPosition;
        }
    }

    public void ChoiceSelected()
    {
        // ���� �� ��� ��ư�� �ٽ� ����ϴ�
        buttonCanvas.SetActive(false);
        isTalking = false;
    }

    public void LoadTargetScene()
    {
        //SceneManager.LoadScene(sceneTargetName);
        LoadingSceneManager.instance.StartLoadScene(sceneTargetName);
    }
}

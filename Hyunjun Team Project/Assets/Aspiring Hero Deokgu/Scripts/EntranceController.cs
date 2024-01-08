using UnityEngine;
using UnityEngine.SceneManagement;

public class EntranceController : MonoBehaviour
{
    public string sceneToLoad; // ������ ���� �̸�

    public void Enter()
    {
        // �ٸ� ��ҷ� �����ϴ� ����, ���� ��� �� ��ȯ
        Debug.Log($"Entering {sceneToLoad}.");
        SceneManager.LoadScene(sceneToLoad);
        WorldSoundManager.Instance.PlayBGM(sceneToLoad);
    }
}
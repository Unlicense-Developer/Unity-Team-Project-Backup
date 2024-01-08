using UnityEngine;
using UnityEngine.SceneManagement;

public class EntranceController : MonoBehaviour
{
    public string sceneToLoad; // 입장할 씬의 이름

    public void Enter()
    {
        // 다른 장소로 입장하는 로직, 예를 들어 씬 전환
        Debug.Log($"Entering {sceneToLoad}.");
        SceneManager.LoadScene(sceneToLoad);
        WorldSoundManager.Instance.PlayBGM(sceneToLoad);
    }
}
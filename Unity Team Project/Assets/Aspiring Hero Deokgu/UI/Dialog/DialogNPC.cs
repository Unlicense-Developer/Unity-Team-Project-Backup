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
    private string sceneTargetName; // 로드할 씬의 이름

    bool isTalking = false;

    public IEnumerator Talk()
    {
        // 대화 시스템에 따라 대화 진행
        if (dialogSystem01.isFirst == true && isTalking == false)
        {
            // 첫 번째 대사 분기 시작
            isTalking = true;
            yield return new WaitUntil(() => dialogSystem01.UpdateDialog());

            // 선택지 확인 및 표시
            if (buttonCanvas != null)
            {
                DisplayChoices();
            }

            // 두 번째 대사 분기 시작
            if (dialogSystem02 != null)
            {
                yield return new WaitUntil(() => dialogSystem02.UpdateDialog());
            }

            // 대화 종료 후 처리
            if (buttonCanvas == null)
            {
                isTalking = false;
            }
        }
    }
    private void DisplayChoices()
    {
        // 모든 버튼을 표시합니다
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
        // 선택 후 모든 버튼을 다시 숨깁니다
        buttonCanvas.SetActive(false);
        isTalking = false;
    }

    public void LoadTargetScene()
    {
        //SceneManager.LoadScene(sceneTargetName);
        LoadingSceneManager.instance.StartLoadScene(sceneTargetName);
    }
}

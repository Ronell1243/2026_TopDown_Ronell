using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutSceneManager : MonoBehaviour
{
    [Header("UI")]
    public Image cutSceneDisplay; // 컷씬이 그려질 UI Image

    [Header("컷씬 리스트")]
    public Sprite[] cutSceneSprites; // 여기에 사용할 그림들을 순서대로 드래그 앤 드롭

    [Header("다음으로 이동할 게임 씬")]
    public string nextSceneName;

    private int currentIndex = 0; // 현재 보여지고 있는 이미지 인스턴스 번호

    void Start()
    {
        // 첫 번째 이미지로 시작
        if (cutSceneSprites.Length > 0)
        {
            DisplayCutscene();
        }
        else
        {
            Debug.LogError("컷씬 이미지가 등록되지 않았습니다! 바로 게임 씬으로 넘어갑니다.");
            EndCutscene();
        }
    }

    // 화살표 버튼에 연결할 함수
    public void OnNextButtonClick()
    {
        currentIndex++;

        // 아직 보여줄 컷씬이 남아있다면 다음 그림 표시
        if (currentIndex < cutSceneSprites.Length)
        {
            DisplayCutscene();
        }
        // 다 보여줬다면 게임 시작
        else
        {
            EndCutscene();
        }
    }

    void DisplayCutscene()
    {
        cutSceneDisplay.sprite = cutSceneSprites[currentIndex];
    }

    void EndCutscene()
    {
        // 플레이어 조작이 가능한 실제 게임 씬으로 전환
        SceneManager.LoadScene(nextSceneName);
    }
}
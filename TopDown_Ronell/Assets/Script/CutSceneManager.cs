using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutSceneManager : MonoBehaviour
{
    [Header("UI")]
    public Image cutSceneDisplay; // 컷씬이 그려질 UI Image

    [Header("컷씬 리스트")]
    public Sprite[] cutSceneSprites; // 여기에 사용할 그림들을 순서대로 드래그 앤 드롭

    public AudioSource audioSource;

    [Header("알람 소리")]
    public AudioClip specialSound1; // 재생할 효과음
    public int[] sound1TargetIndices;

    [Header("냉장고 소리")]
    public AudioClip specialSound2; // 재생할 효과음
    public int[] sound2TargetIndices;

    [Header("달리는 소리")]
    public AudioClip specialSound3; // 재생할 효과음
    public int[] sound3TargetIndices;
    
    [Header("반짝반짝 소리")]
    public AudioClip specialSound4; // 재생할 효과음
    public int[] sound4TargetIndices;


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

        if (IsTargetIndex(currentIndex, sound1TargetIndices))
        {
            if (audioSource != null && specialSound1 != null)
            {
                audioSource.PlayOneShot(specialSound1);
            }
        }

        if (IsTargetIndex(currentIndex, sound2TargetIndices))
        {
            if (audioSource != null && specialSound2 != null)
            {
                audioSource.PlayOneShot(specialSound2);
            }
        }

        if (IsTargetIndex(currentIndex, sound3TargetIndices))
        {
            if (audioSource != null && specialSound3 != null)
            {
                audioSource.PlayOneShot(specialSound3);
            }
        }

        if (IsTargetIndex(currentIndex, sound4TargetIndices))
        {
            if (audioSource != null && specialSound4 != null)
            {
                audioSource.PlayOneShot(specialSound4);
            }
        }
    }

    // 지정된 배열 안에 현재 컷씬 번호가 포함되어 있는지 확인하는 함수
    bool IsTargetIndex(int index, int[] targetIndices)
    {
        if (targetIndices == null) return false;

        // 배열을 돌면서 일치하는 숫자가 있는지 확인
        for (int i = 0; i < targetIndices.Length; i++)
        {
            if (targetIndices[i] == index)
            {
                return true; // 일치하는 번호가 있으면 true 반환
            }
        }
        return false; // 없으면 false 반환
    }

    void EndCutscene()
    {
        // 플레이어 조작이 가능한 실제 게임 씬으로 전환
        SceneManager.LoadScene(nextSceneName);
    }
}
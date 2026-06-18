using UnityEngine;
using TMPro;

public class StageManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private bool isGameActive = true;

    void Start()
    {
        // 게임 시작 시 이전 데이터 리셋
        GameDataManager.Instance.ResetStageData();
    }

    void Update()
    {
        if (!isGameActive) return;

        // 매 프레임 시간을 누적시킵니다.
        GameDataManager.Instance.elapsedTime += Time.deltaTime;

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText == null) return;

        float t = GameDataManager.Instance.elapsedTime;

        // 분, 초, 밀리초 계산
        int minutes = (int)(t / 60);
        int seconds = (int)(t % 60);
        int fraction = (int)((t * 100) % 100);

        // 정렬된 문자열로 포맷팅 (예: 01:23.45)
        timerText.text = string.Format("플레이 타임 : {0:00}:{1:00}.{2:00}", minutes, seconds, fraction);
    }

    // 골인 지점에 도착했을 때 타이머를 멈추는 함수
    public void StopTimer()
    {
        isGameActive = false;
    }
}
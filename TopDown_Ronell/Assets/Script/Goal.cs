using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    [Header("엔딩 조건 시간 설정 (초 단위)")]
    public float BestEndingCutoff = 30f;
    public float GoodEndingCutoff = 60f;
    public float NormalEndingCutoff = 60f;
    public float BadEndingCutoff = 60f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 이번 판에 아이템 5개를 다 모았는지 검사
            if (GameDataManager.Instance.currentItemCount >= 5)
            {
                // 타이머 정지
                StageManager stageManager = FindFirstObjectByType<StageManager>();
                if (stageManager != null) stageManager.StopTimer();

                // ✨ [신규 추가] 골인 성공했으니 최고 시간 기록 검사 및 JSON 저장 실행!
                GameDataManager.Instance.CheckAndSaveBestTime();

                float finalTime = GameDataManager.Instance.elapsedTime;
                if (finalTime < BestEndingCutoff)
                {
                    SceneManager.LoadScene("BestEndingScene");
                }
                else if (finalTime < GoodEndingCutoff)
                {
                    SceneManager.LoadScene("GoodEndingScene");
                }
                else if (finalTime < NormalEndingCutoff)
                {
                    SceneManager.LoadScene("NormalEndingScene");
                }
                else
                {
                    SceneManager.LoadScene("BadEndingScene");
                }
            }
            else
            {
                int missingItems = 5 - GameDataManager.Instance.currentItemCount;
                Debug.Log($"아직 집으로 돌아갈 수 없습니다! 아이템 {missingItems}개가 부족합니다.");
            }
        }
    }
}
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int deathCount = 0; // 기존에 있던 변수

    // 💾 JSON으로 영구 저장할 새로운 데이터 변수들 추가!
    public int totalCollectedItems = 0;   // 여태까지 먹은 총 아이템 개수
    public float bestClearTime = 999999f; // 최고 클리어 기록 (낮을수록 좋으므로 초기값은 크게)
}

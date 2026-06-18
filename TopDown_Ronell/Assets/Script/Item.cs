using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName = "기본 아이템";
    public AudioClip itemPickupSound;
    public GameObject floatingTextPrefab; // 1단계에서 만든 프리팹을 넣을 칸
    public Vector3 offset = new Vector3(0f, 1.2f, 0f); // 머리 위로 띄울 높이 간격

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 1. 데이터 저장 및 세이브 실행
            GameDataManager.Instance.GainItemJson();

            // 2. 플레이어 머리 위 위치에 텍스트 프리팹 생성!
            if (floatingTextPrefab != null)
            {
                // 플레이어의 현재 위치 + 머리 위 오프셋 값
                Vector3 spawnPosition = collision.transform.position + offset;

                // 텍스트 생성 (회전 없이 똑바로 서 있게 생성)
                GameObject go = Instantiate(floatingTextPrefab, spawnPosition, Quaternion.identity);
                
                //생성된 텍스트의 부모를 방금 부딪힌 플레이어(collision)의 transform으로 지정합니다!
                go.transform.SetParent(collision.transform);

                FloatingText ft = go.GetComponent<FloatingText>();
                if (ft != null)
                {
                    // 인스펙터 창에 적어둔 이름 뒤에 " 획득!"을 붙여서 글자를 세팅합니다.
                    // 예: "빨간 보석 획득!", "열쇠 획득!"
                    ft.SetText($"{itemName} 획득!");
                }
            }

            if (itemPickupSound != null && SoundManager.Instance != null)
            {
               // SoundManager.Instance.PlaySFX(itemPickupSound); 
            }
            // 3. 아이템 오브젝트 파괴
            Destroy(gameObject);
        }

            
           
        
    }
}
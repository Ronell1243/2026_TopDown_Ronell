using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 2f;       // 위로 올라가는 속도
    public float destroyTime = 0.8f;   // 사라질 시간
    public float fadeSpeed = 3f;       // 투명해지는 속도

    private TextMeshProUGUI textMesh;
    private Color textColor;

    void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh != null)
        {
            textColor = textMesh.color;
        }
    }

    void Start()
    {
        // 지정된 시간 뒤에 자동으로 오브젝트 삭제
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        // 1. 매 프레임 위로 이동 (Y축 방향)
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // 2. 시간이 지날수록 점차 투명해지게 만들기 (알파값 감소)
        if (textMesh != null)
        {
            textColor.a = Mathf.MoveTowards(textColor.a, 0f, fadeSpeed * Time.deltaTime);
            textMesh.color = textColor;
        }
    }
    public void SetText(string newText)
    {
        // 만약 Awake보다 이 함수가 먼저 실행될 경우를 대비해 한 번 더 체크합니다.
        if (textMesh == null) textMesh = GetComponentInChildren<TextMeshProUGUI>();

        if (textMesh != null)
        {
            textMesh.text = newText;
        }
    }
}
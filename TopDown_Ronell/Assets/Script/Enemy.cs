using UnityEngine;

public class PhysicsEnemy : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 탑다운 물리 이동 시 적이 회전하느라 멋대로 구르는 것을 방지
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            // 플레이어가 있는 방향(벡터) 계산
            Vector2 direction = (player.position - transform.position).normalized;

            // Rigidbody2D를 통해 이동 처리
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
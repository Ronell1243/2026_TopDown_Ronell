using UnityEngine;

public class SlidingTile : MonoBehaviour
{
    [Header("설정")]
    public float slideSpeed = 15f;   // 미끄러지는 속도
    public LayerMask wallLayer;      // 부딪힐 벽 레이어

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. 밟은 오브젝트가 플레이어인지 확인
        if (collision.CompareTag("Player"))
        {
            // 플레이어의 Rigidbody2D와 기존 컨트롤러를 가져옵니다.
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            PlayerController playerCtrl = collision.GetComponent<PlayerController>();

            if (playerRb != null && playerCtrl != null)
            {
                // 2. 현재 플레이어가 걷고 있던 속도(방향)를 파악합니다.
                Vector2 currentVelocity = playerRb.linearVelocity; // 유니티 버전에 따라 velocity일 수 있습니다.
                if (currentVelocity == Vector2.zero) return; // 멈춰 서서 들어왔다면 무시

                // 움직이던 방향을 상하좌우 중 하나로 단순화 (-1 또는 1)
                Vector2 moveDir = Vector2.zero;
                if (Mathf.Abs(currentVelocity.x) > Mathf.Abs(currentVelocity.y))
                {
                    moveDir.x = currentVelocity.x > 0 ? 1f : -1f;
                }
                else
                {
                    moveDir.y = currentVelocity.y > 0 ? 1f : -1f;
                }

                // 3. 기존 플레이어 컨트롤러 컴포넌트를 잠시 꺼버립니다! (조작 불가능 상태 전환)
                playerCtrl.enabled = false;

                // 4. 벽을 찾아 미끄러지는 별도의 코루틴 연산을 시작합니다.
                StartCoroutine(SlideRoutine(playerRb, playerCtrl, moveDir));
            }
        }
    }

    private System.Collections.IEnumerator SlideRoutine(Rigidbody2D rb, PlayerController ctrl, Vector2 dir)
    {
        Vector2 startPos = rb.position;

        // 레이저를 쏴서 부딪힐 벽을 찾습니다.
        RaycastHit2D hit = Physics2D.Raycast(startPos, dir, 99f, wallLayer);

        if (hit.collider != null)
        {
            Vector2 hitPoint = hit.point;
            // 벽 속에 파고들지 않도록 목적지 계산
            Vector2 targetPos = new Vector2(
                Mathf.Round(hitPoint.x - (dir.x * 0.5f)),
                Mathf.Round(hitPoint.y - (dir.y * 0.5f))
            );

            // 목적지에 도달할 때까지 플레이어를 강제로 밀어버립니다.
            while (Vector2.Distance(rb.position, targetPos) > 0.001f)
            {
                Vector2 nextPos = Vector2.MoveTowards(rb.position, targetPos, slideSpeed * Time.deltaTime);
                rb.MovePosition(nextPos);
                yield return null;
            }

            rb.MovePosition(targetPos); // 위치 오차 보정
        }

        // 5. 벽에 부딪혀서 미끄러짐이 끝나면 플레이어 컨트롤러를 다시 켜줍니다! (조작 권한 반환)
        ctrl.enabled = true;
    }
}
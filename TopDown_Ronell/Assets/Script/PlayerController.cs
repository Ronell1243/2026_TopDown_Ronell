using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Sprite[] spriteUp;
    public Sprite[] spriteDown;
    public Sprite[] spriteLeft;
    public Sprite[] spriteRight;
    public float frameTime = 0.15f;
    public int playerHP = 0;
    public int playerAttack = 0;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 input;
    private Vector2 velocity;
    private Sprite[] currentSprites;
    private int frameIndex = 0;
    private float timer = 0f;

    private Vector2 spawnPosition;

    public void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
        velocity = input.normalized * moveSpeed;

        if (input.sqrMagnitude > 0.01f)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                if (input.x > 0)
                    ChangeSprites(spriteRight);
                else
                    ChangeSprites(spriteLeft);
            }
            else
            {
                if (input.y > 0)
                    ChangeSprites(spriteUp);
                else
                    ChangeSprites(spriteDown);
            }
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        currentSprites = spriteDown;
        sr.sprite = currentSprites[0];
    }

    // 짚고 넘어가기: 중첩되었던 Start() 함수 구조를 하나로 합쳐서 깔끔하게 해결했습니다.
    void Start()
    {
        // 1. 게임 시작할 때 현재 맵의 최초 시작 위치를 올바르게 기억합니다.
        spawnPosition = transform.position;

        // 2. 데이터 매니저에서 값 가져오기
        moveSpeed = GameDataManager.Instance.GetPlayerMoveSpeed();
        playerHP = GameDataManager.Instance.GetPlayerHp();
        playerAttack = GameDataManager.Instance.GetPlayerAttack();

        // 3. 튜토리얼 체크
        if (GameDataManager.Instance.isTutorialFinished == 0)
        {
            Debug.Log("튜토리얼 오픈!");
            GameDataManager.Instance.isTutorialFinished = 1;
        }
    }

    private void Update()
    {
        if (input.sqrMagnitude <= 0.01f)
        {
            frameIndex = 0;
            sr.sprite = currentSprites[frameIndex];
            return;
        }

        timer += Time.deltaTime;

        if (timer >= frameTime)
        {
            timer = 0f;
            frameIndex++;

            if (frameIndex >= currentSprites.Length)
                frameIndex = 0;

            sr.sprite = currentSprites[frameIndex];
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    private void ChangeSprites(Sprite[] newSprites)
    {
        if (currentSprites == newSprites)
            return;

        currentSprites = newSprites;
        frameIndex = 0;
        timer = 0f;
        sr.sprite = currentSprites[frameIndex];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("적과 충돌! 리스폰합니다.");

            // 만들어둔 리스폰 함수를 여기서 호출해 줍니다.
            Respawn();
        }
    }

    public void Respawn()
    {
        Debug.Log("플레이어 리스폰 위치로 이동: " + spawnPosition);

        // Rigidbody2D 포지션과 transform 포지션을 둘 다 확실히 옮겨줍니다.
        rb.position = spawnPosition;
        transform.position = spawnPosition;

        // 리스폰 되었을 때 키 입력이나 움직이던 속도를 제로(0)로 멈춥니다.
        input = Vector2.zero;
        velocity = Vector2.zero;
    }
}

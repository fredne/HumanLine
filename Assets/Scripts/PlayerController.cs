using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject gameManagerObject;
    private GameManager gameManager;
    private List<GameObject> humanLine;
    private List<Vector2> linePositions;
    public GameObject headPrefab;
    public GameObject bodyPrefab;
    private SpriteRenderer spriteRenderer;
    public float moveInterval = 0.2f;
    private float moveTimer;
    private Vector2 moveDir;
    public float speed;
    public EmotionState emotionState;

    private BoxCollider2D boxCollider;
    private Vector2 mapSize;

    private bool itemTrigger;
    private Item collisionItem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        humanLine = new List<GameObject>();
        linePositions = new List<Vector2>();
        moveDir = Vector2.zero;
        speed = 1f;
        emotionState = EmotionState.None;

        gameManager = gameManagerObject.GetComponent<GameManager>();

        mapSize = gameManager.GetMapSize();
        boxCollider = GetComponent<BoxCollider2D>();

        GameObject headObject = Instantiate(headPrefab);
        headObject.transform.localPosition = new Vector3(0, 0, -3);
        humanLine.Add(headObject);
        linePositions.Add(moveDir);
        spriteRenderer = headObject.GetComponent<SpriteRenderer>();

        itemTrigger = false;
        collisionItem = null;
    }

    // Update is called once per frame
    void Update()
    {
        // 키보드 업데이트
        KeyUpdate();

        // 감정 상태에 따라 색 변경
        if (emotionState != EmotionState.None)
            spriteRenderer.color = gameManager.GetColorList()[(int)emotionState];

        // 주기에 따라 이동 업데이트
        moveTimer += Time.deltaTime;
        if(moveTimer > moveInterval)
        {
            moveTimer -= moveInterval;
            Move();
        }

    }
    private void LateUpdate()
    {
        if (itemTrigger)
        {
            if (collisionItem.State == emotionState) CreateBody();
            else Debug.Log("경고: 감정 상태가 맞지 않습니다.");
            itemTrigger = false;

        }
        boxCollider.transform.position = linePositions[0];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            gameManager.SetItemTrigger();
            itemTrigger = true;
            collisionItem = collision.gameObject.GetComponent<Item>();
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Game Over");
            gameObject.SetActive(false);
        }
    }

    void KeyUpdate()
    {
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame && (humanLine.Count < 2 || moveDir != Vector2.left)) moveDir = Vector2.right;
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame && (humanLine.Count < 2 || moveDir != Vector2.right)) moveDir = Vector2.left;
        if (Keyboard.current.upArrowKey.wasPressedThisFrame && (humanLine.Count < 2 || moveDir != Vector2.down)) moveDir = Vector2.up;
        if (Keyboard.current.downArrowKey.wasPressedThisFrame && (humanLine.Count < 2 || moveDir != Vector2.up)) moveDir = Vector2.down;


        if (Keyboard.current.qKey.wasPressedThisFrame) emotionState = EmotionState.Angry;
        else if (Keyboard.current.wKey.wasPressedThisFrame) emotionState = EmotionState.Sad;
        else if (Keyboard.current.eKey.wasPressedThisFrame) emotionState = EmotionState.Delight;

        // 확인용 코드
        if (Keyboard.current.zKey.wasPressedThisFrame) moveDir = Vector2.zero;
    }

    void Move()
    {
        GameObject head = humanLine[0];
        Vector2 pos = head.transform.position;
        Vector2 nextGird = pos + moveDir * speed;

        if (Mathf.Abs(nextGird.x) >= (mapSize.x / 2f) || Mathf.Abs(nextGird.y) >= (mapSize.y / 2f))
        {
            gameObject.SetActive(false);
            Debug.Log("Out!!!");
        }
        else
        {
            linePositions.Insert(0, nextGird);
            if (linePositions.Count > humanLine.Count)
                linePositions.RemoveAt(linePositions.Count - 1);

            for (int i = 0; i < humanLine.Count; ++i)
            {
                humanLine[i].transform.position = linePositions[i];
            }
        }
    }

    void CreateBody()
    {
        GameObject bodyObject = Instantiate(bodyPrefab);
        int lineCount = linePositions.Count;
        Vector2 pos1 = linePositions[lineCount - 1];
        bodyObject.transform.position = pos1;

        if (linePositions.Count > 1)
        {
            Vector2 pos2 = linePositions[lineCount - 2];
            Vector2 dist = pos2 - pos1;
            bodyObject.transform.position = new Vector3(pos1.x - Mathf.Abs(dist.x), pos1.y - Mathf.Abs(dist.y), -1);
        }
        else
        {
            Vector2 pos = pos1 - moveDir;
            bodyObject.transform.position = new Vector3(pos.x, pos.y, -1);
        }

        humanLine.Add(bodyObject);
        linePositions.Add(pos1);

        //Debug.Log($"Human Line Count: {humanLine.Count}");
        //Debug.Log($"Line Position Count: {linePositions.Count}");

    }

    public List<Vector2> GetLinePos() => linePositions;

}

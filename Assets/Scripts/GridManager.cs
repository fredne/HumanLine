using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject gameManagerObject;
    private GameManager gameManager;
    public GameObject itemPrefab;

    private bool itemTrigger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = gameManagerObject.GetComponent<GameManager>();
        itemTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LateUpdate()
    {
        if (itemTrigger)
        {
            ItemGenerate();
            itemTrigger = false;
        }
    }

    void ItemGenerate()
    {
        Vector2 mapSize = gameManager.floor.transform.localScale;
        float xScale = mapSize.x / 2f;
        float yScale = mapSize.y / 2f;
        Vector2 itemPos = GetRandomPos(xScale, yScale);
        bool checkPos = true;

        List<Vector2> linePosList = gameManager.GetPlayerLinePos();
        while (checkPos)
        {
            foreach (Vector2 linePos in linePosList)
            {
                if (itemPos == linePos) itemPos = GetRandomPos(xScale, yScale);
                else
                {
                    checkPos = false;
                    Debug.Log($"itemPos: {itemPos}, linePos: {linePos}");
                }
            }
        }

        GameObject item = Instantiate(itemPrefab);
        Vector3 pos = new Vector3(itemPos.x, itemPos.y, -2);
        item.transform.localPosition = itemPos;

        int colorNum = Random.Range(0, 3);
        item.GetComponent<Item>().State = (EmotionState)colorNum;
        SpriteRenderer itemRenderer = item.GetComponent<SpriteRenderer>();
        itemRenderer.color = gameManager.GetColorList()[colorNum];
    }

    public void SetItemTrigger()
    {
        itemTrigger = true;
    }

    private Vector2 GetRandomPos(float xScale, float yScale)
    {
        int x = Mathf.FloorToInt(xScale);
        int y = Mathf.FloorToInt(yScale);
        Vector2 newPos = new Vector2(Random.Range(-x, x + 1), Random.Range(-y, y + 1));
        Debug.Log($"scale: ({x}, {y}) newPos: {newPos}");
        return newPos;
    }

}

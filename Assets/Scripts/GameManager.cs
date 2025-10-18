using System.Collections.Generic;
using UnityEngine;

public enum EmotionState
{
    Angry, Sad, Delight, None
};

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject generator;
    public GameObject floor;

    private GridManager gridManager;

    private List<GameObject> deadObject;

    private List<Color> colorList;
    public Color colorAngry;
    public Color colorSad;
    public Color colorDelight;
    private Vector2 mapSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridManager = generator.GetComponent<GridManager>();
        deadObject = new List<GameObject>();

        mapSize = floor.gameObject.transform.localScale;

        colorList = new List<Color>();
        colorAngry = HexColor("#FF4136");
        colorSad = HexColor("#0074D9");
        colorDelight = HexColor("#FFDC00");
        colorList.Add(colorAngry);
        colorList.Add(colorSad);
        colorList.Add(colorDelight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        foreach (GameObject obj in deadObject)
        {
            Destroy(obj);
        }
    }

    public void SetItemTrigger()
    {
        gridManager.SetItemTrigger();
    }

    public List<Color> GetColorList() => colorList;

    public List<Vector2> GetPlayerLinePos()
    {
        return player.GetComponent<PlayerController>().GetLinePos();
    }

    private Color HexColor(string hexCode)
    {
        Color color;
        if(ColorUtility.TryParseHtmlString(hexCode, out color)) return color;

        Debug.LogError("HexColor: invalid hex code - " + hexCode);

        return Color.white;
    }
    
    public Vector2 GetMapSize() => mapSize;

}

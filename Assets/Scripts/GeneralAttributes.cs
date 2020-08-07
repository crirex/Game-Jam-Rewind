using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralAttributes: MonoBehaviour
{
    public GridMap houseGrid;

    public Inventory inventory = new Inventory();

    public Vector2 safePositionMax;
    public Vector2 safePositionMin;
    public Vector2Int firstElementToCombineIndex;
    public Vector2Int secondElementToCombineIndex;
    public Vector2Int resultElementIndex;

    public List<GameObject> gameObjectsForDictionary;
    public Dictionary<KeyValuePair<string, string>, GameObject> combinationDictionary;

    private static GeneralAttributes instance = null;
    public static GeneralAttributes Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        houseGrid = new GridMap(15, 9, 1.0f, new Vector2(1, 1));

        combinationDictionary = new Dictionary<KeyValuePair<string, string>, GameObject>();
        combinationDictionary.Add(new KeyValuePair<string, string>("Air", "Fire"), gameObjectsForDictionary[0]); //Energy
        combinationDictionary.Add(new KeyValuePair<string, string>("Fire", "Earth"), gameObjectsForDictionary[1]); //Lava
        combinationDictionary.Add(new KeyValuePair<string, string>("Fire", "Energy"), gameObjectsForDictionary[2]); //Plasma
        combinationDictionary.Add(new KeyValuePair<string, string>("Earth", "Earth"), gameObjectsForDictionary[3]); //Stone
        combinationDictionary.Add(new KeyValuePair<string, string>("Air", "Energy"), gameObjectsForDictionary[4]); //Storm
        combinationDictionary.Add(new KeyValuePair<string, string>("Lava", "Water"), gameObjectsForDictionary[5]); //Obsidian
        combinationDictionary.Add(new KeyValuePair<string, string>("Air", "Earth"), gameObjectsForDictionary[6]); //Dust
        combinationDictionary.Add(new KeyValuePair<string, string>("Stone", "Fire"), gameObjectsForDictionary[7]); //Iron
        combinationDictionary.Add(new KeyValuePair<string, string>("Stone", "Iron"), gameObjectsForDictionary[8]); //Gold
        combinationDictionary.Add(new KeyValuePair<string, string>("Energy", "Earth"), gameObjectsForDictionary[9]); //Seeds
        combinationDictionary.Add(new KeyValuePair<string, string>("Seeds", "Water"), gameObjectsForDictionary[10]); //Wood
        combinationDictionary.Add(new KeyValuePair<string, string>("Wood", "Fire"), gameObjectsForDictionary[11]); //Coal
        combinationDictionary.Add(new KeyValuePair<string, string>("Stone", "Air"), gameObjectsForDictionary[12]); //Sand
        combinationDictionary.Add(new KeyValuePair<string, string>("Sand", "Water"), gameObjectsForDictionary[13]); //Clay
        combinationDictionary.Add(new KeyValuePair<string, string>("Fire", "Sand"), gameObjectsForDictionary[14]); //Glass
        combinationDictionary.Add(new KeyValuePair<string, string>("Fire", "Clay"), gameObjectsForDictionary[15]); //Bricks
        combinationDictionary.Add(new KeyValuePair<string, string>("Stone", "Clay"), gameObjectsForDictionary[16]); //Cement
        combinationDictionary.Add(new KeyValuePair<string, string>("Coal", "Water"), gameObjectsForDictionary[17]); //Oil
    }
}

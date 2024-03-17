using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private int _settings;
    private readonly Dictionary<EPuzlleCategories, string> _puzzleCatDirectory = new Dictionary<EPuzlleCategories, string>();
    private const int settingsnumber = 2;

    public enum EPairNumber
    {
        NotSet = 0,
        E10Pairs = 10,
        E15Pairs = 15,
        E20Pairs = 20,
    }

    public enum EPuzlleCategories
    {
        NotSet,
        Fruits,
        Vegetables
    }

    public struct settings
    {
        public EPairNumber PairsNumber;
        public EPuzlleCategories PuzzleCategory;
    }

    private settings _gameSettings;
    public static GameSettings Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        SetPuzlleCatCategories();
        _gameSettings = new settings();
        ResetGameSettings();
    }
    private void SetPuzlleCatCategories()
    {
        _puzzleCatDirectory.Add(EPuzlleCategories.Fruits, "frontside");
        _puzzleCatDirectory.Add(EPuzlleCategories.Vegetables, "frontside");
    }
    public void SetPairNumver(EPairNumber pairNum)
    {
        if (_gameSettings.PairsNumber == EPairNumber.NotSet)
            _settings++;
        _gameSettings.PairsNumber = pairNum;
    }
    public void SetPuzlleCategories(EPuzlleCategories category)
    {
        if (_gameSettings.PuzzleCategory == EPuzlleCategories.NotSet)
            _settings++;
        _gameSettings.PuzzleCategory = category;
    }

    public EPairNumber GetEPairNumber()
    {
        return _gameSettings.PairsNumber;
    }
    public EPuzlleCategories GetEPuzlleCategories()
    {
        return _gameSettings.PuzzleCategory;
    }

    public void ResetGameSettings()
    {
        _settings = 0;
        _gameSettings.PuzzleCategory = EPuzlleCategories.NotSet;
        _gameSettings.PairsNumber = EPairNumber.NotSet;
    }

    public bool ALLSettingsReady()
    {
        return _settings == settingsnumber;
    }

    public string getmaterialdirectoryname()
    {
        return "Materials/";

    }
    public string GetpuzzleCattergoryDirectoryname()
    {
        if (_puzzleCatDirectory.ContainsKey(_gameSettings.PuzzleCategory))
        {
            return "Cards/" + _puzzleCatDirectory[_gameSettings.PuzzleCategory] + "/";
        }
        else
        {
            Debug.LogError("Error:Cannot Get Directory");
            return "";
        }
    }

}

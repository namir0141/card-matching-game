using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureManager : MonoBehaviour
{
    public Picture pictureprefab;
    public Transform picSpawnPostion;
    public Vector2 startpostion = new Vector2(-2.15f, 3.62f);

    public GameObject EndGamePanel;
    public GameObject NewBestScore;

    public GameObject YourScoreText;
    public GameObject EndTime;



    public enum GameState
    {
        NoAction,
        MovingOnPosition,
        DeletingPuzzles,
        FlipBack,
        Checking,
        GameEnd
    };

    public enum PuzzleState
    {
        PuzzleRotation,
        CanRotate
    };

    public enum RevealedNumber
    {
        NoRevealed,
        OneRevealed,
        TwoRevealed
    };

    [HideInInspector]
    public GameState CurrentGameState;
    public PuzzleState CurrentPuzzleState;
    public RevealedNumber PuzzleRevealedNumber;

    [HideInInspector]
    public List<Picture> picturelist;
    private Vector2 _offset = new Vector2(1.5f, 1.52f);
    private Vector2 _offsetfor15pairs = new Vector2(1.08f, 1.22f);
    private Vector2 _offsetfor20pairs = new Vector2(1.08f, 1.0f);
    private Vector3 _newScaledown = new Vector3(0.9f, 0.9f, 0.001f);

    private List<Material> _materials = new List<Material>();
    private List<string> _texturesPathList = new List<string>();
    private Material _firstmaterial;
    private string _firsttexturepath;

    private int _firstRevelPic;
    private int _secondrevelPic;
    private int _revelPicNumber = 0;
    private int _pictoDestory1;
    private int _pictoDestory2;

    private bool _coututinesstarted = false;
    private int _pairNumber;
    private int _removePiars;
    void Start()
    {
        CurrentGameState = GameState.NoAction;
        CurrentPuzzleState = PuzzleState.CanRotate;
        PuzzleRevealedNumber = RevealedNumber.NoRevealed;
        _revelPicNumber = 0;
        _firstRevelPic = -1;
        _secondrevelPic = -1;

        _removePiars = 0;
        _pairNumber = (int)GameSettings.Instance.GetEPairNumber();
        LoadMaterials();

        if (GameSettings.Instance.GetEPairNumber() == GameSettings.EPairNumber.E10Pairs)
        {
            CurrentGameState = GameState.MovingOnPosition;
            spawnpicture(4, 5, startpostion, _offset, false);
            MovePicture(4, 5, startpostion, _offset);
        }
        else if (GameSettings.Instance.GetEPairNumber() == GameSettings.EPairNumber.E15Pairs)
        {
            CurrentGameState = GameState.MovingOnPosition;
            spawnpicture(5, 6, startpostion, _offset, false);
            MovePicture(5, 6, startpostion, _offsetfor15pairs);
        }
        else if (GameSettings.Instance.GetEPairNumber() == GameSettings.EPairNumber.E20Pairs)
        {
            CurrentGameState = GameState.MovingOnPosition;
            spawnpicture(5, 8, startpostion, _offset, true);
            MovePicture(5, 8, startpostion, _offsetfor20pairs);
        }

    }

    public void CheckPicture()
    {
        CurrentGameState = GameState.Checking;
        _revelPicNumber = 0;
        for (int id = 0; id < picturelist.Count; id++)
        {
            if (picturelist[id].Reveled && _revelPicNumber < 2)
            {
                if (_revelPicNumber == 0)
                {
                    _firstRevelPic = id;
                    _revelPicNumber++;
                }
                else if (_revelPicNumber == 1)
                {
                    _secondrevelPic = id;
                    _revelPicNumber++;
                }
            }
        }
        if (_revelPicNumber == 2)
        {
            if (picturelist[_firstRevelPic].GetIndex() == picturelist[_secondrevelPic].GetIndex() && _firstRevelPic != _secondrevelPic)
            {
                CurrentGameState = GameState.DeletingPuzzles;
                _pictoDestory1 = _firstRevelPic;
                _pictoDestory2 = _secondrevelPic;
            }
            else
            {
                CurrentGameState = GameState.FlipBack;
            }
        }
        CurrentPuzzleState = PictureManager.PuzzleState.CanRotate;

        if (CurrentGameState == GameState.Checking)
        {
            CurrentGameState = GameState.NoAction;
        }
    }

    private void DestroyPicture()
    {
        PuzzleRevealedNumber = RevealedNumber.NoRevealed;
        picturelist[_pictoDestory1].Deactivate();
        picturelist[_pictoDestory2].Deactivate();
        _revelPicNumber = 0;
        _removePiars++;
        CurrentGameState = GameState.NoAction;
        CurrentPuzzleState = PuzzleState.CanRotate;
    }

    private IEnumerator FlipBack()
    {
        _coututinesstarted = true;
        yield return new WaitForSeconds(0.5f);

        picturelist[_firstRevelPic].FlipBack();
        picturelist[_secondrevelPic].FlipBack();

        picturelist[_firstRevelPic].Reveled = false;
        picturelist[_secondrevelPic].Reveled = false;

        PuzzleRevealedNumber = RevealedNumber.NoRevealed;
        CurrentGameState = GameState.NoAction;

        _coututinesstarted = false;
    }
    private void LoadMaterials()
    {
        var materialFilePath = GameSettings.Instance.getmaterialdirectoryname();
        var texturefilepath = GameSettings.Instance.GetpuzzleCattergoryDirectoryname();
        var pairnumber = (int)GameSettings.Instance.GetEPairNumber();
        const string matbasename = "Pic";
        var firstmaterialname = "Back";

        for (var index = 1; index <= pairnumber; index++)
        {
            var currentfilepath = materialFilePath + matbasename + index;
            Material mat = Resources.Load(currentfilepath, typeof(Material)) as Material;
            _materials.Add(mat);

            var currentTextureFilePAth = texturefilepath + matbasename + index;
            _texturesPathList.Add(currentTextureFilePAth);
        }
        _firsttexturepath = texturefilepath + firstmaterialname;
        _firstmaterial = Resources.Load(materialFilePath + firstmaterialname, typeof(Material)) as Material;
    }

    void Update()
    {
        if (CurrentGameState == GameState.DeletingPuzzles)
        {
            if (CurrentPuzzleState == PuzzleState.CanRotate)
            {
                DestroyPicture();
                CheckGameEnd();
            }

        }
        if (CurrentGameState == GameState.FlipBack)
        {
            if (CurrentPuzzleState == PuzzleState.CanRotate && _coututinesstarted == false)
            {
                StartCoroutine(FlipBack());
            }
        }
        if (CurrentGameState == GameState.GameEnd)
        {
            if (picturelist[_firstRevelPic].gameObject.activeSelf == false && picturelist[_secondrevelPic].gameObject.activeSelf == false && EndGamePanel.activeSelf == false)
            {
                ShowEndGameInformation();
            }
        }
    }

    private bool CheckGameEnd()
    {
        if (_removePiars == _pairNumber && CurrentGameState != GameState.GameEnd)
        {
            CurrentGameState = GameState.GameEnd;

        }
        return (CurrentGameState == GameState.GameEnd);
    }
    private void ShowEndGameInformation()
    {
        EndGamePanel.SetActive(true);
    }

    private void spawnpicture(int rows, int columns, Vector2 pos, Vector2 offset, bool scaledown)
    {
        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                var temp = (Picture)Instantiate(pictureprefab, picSpawnPostion.position, pictureprefab.transform.rotation);
                if (scaledown)
                {
                    temp.transform.localScale = _newScaledown;
                }
                temp.name = temp.name + 'c' + col + 'r' + row;
                picturelist.Add(temp);
            }
        }
        ApplyTexture();
    }
    public void ApplyTexture()
    {
        var randMatIndex = Random.Range(0, _materials.Count);
        var AppliedTimes = new int[_materials.Count];
        for (int i = 0; i < _materials.Count; i++)
        {
            AppliedTimes[i] = 0;
        }
        foreach (var o in picturelist)
        {
            var randPrevious = randMatIndex;
            var counter = 0;
            var forceMat = false;
            while (AppliedTimes[randMatIndex] >= 2 || ((randPrevious == randMatIndex) && !forceMat))
            {
                randMatIndex = Random.Range(0, _materials.Count);
                counter++;
                if (counter > 100)
                {
                    for (var j = 0; j < _materials.Count; j++)
                    {
                        if (AppliedTimes[j] < 2)
                        {
                            randMatIndex = j;
                            forceMat = true;
                        }
                    }
                    if (forceMat == false)
                    {
                        return;
                    }
                }
            }
            o.SetFirstMaterial(_firstmaterial, _firsttexturepath);
            o.ApplyFirstMaterial();
            o.SetSecondMaterial(_materials[randMatIndex], _texturesPathList[randMatIndex]);
            o.SetIndex(randMatIndex);
            o.Reveled = false;
            AppliedTimes[randMatIndex] += 1;
            forceMat = false;
        }
    }
    private void MovePicture(int rows, int colums, Vector2 pos, Vector2 offset)
    {
        var index = 0;
        for (var col = 0; col < colums; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                var targetposition = new Vector3((pos.x + (offset.x * row)), (pos.y - (offset.y * col)), 0.0f);
                StartCoroutine(MoveToPosition(targetposition, picturelist[index]));
                index++;
            }
        }
    }
    private IEnumerator MoveToPosition(Vector3 target, Picture obj)
    {
        var randomdis = 7;
        while (obj.transform.position != target)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, randomdis * Time.deltaTime);
            yield return null;
        }
    }
}

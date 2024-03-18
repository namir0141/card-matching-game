using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureManager : MonoBehaviour
{
    public Picture pictureprefab;
    public Transform picSpawnPostion;
    public Vector2 startpostion = new Vector2(-2.15f, 3.62f);

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
    void Start()
    {
        LoadMaterials();

        if (GameSettings.Instance.GetEPairNumber() == GameSettings.EPairNumber.E10Pairs)
        {
            spawnpicture(4, 5, startpostion, _offset, false);
            MovePicture(4, 5, startpostion, _offset);
        }
        else if (GameSettings.Instance.GetEPairNumber() == GameSettings.EPairNumber.E15Pairs)
        {
            spawnpicture(5, 6, startpostion, _offset, false);
            MovePicture(5, 6, startpostion, _offsetfor15pairs);
        }
        else if (GameSettings.Instance.GetEPairNumber() == GameSettings.EPairNumber.E20Pairs)
        {
            spawnpicture(5, 8, startpostion, _offset, true);
            MovePicture(5, 8, startpostion, _offsetfor20pairs);
        }

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

    }
    private void spawnpicture(int rows, int columns, Vector2 pos, Vector2 offset, bool scaledown)
    {
        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                var temp = (Picture)Instantiate(pictureprefab, picSpawnPostion.position, picSpawnPostion.transform.rotation);
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

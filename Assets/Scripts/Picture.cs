using System.Collections;
using UnityEngine;

public class Picture : MonoBehaviour
{
    private Material _firstMaterial;
    private Material _secondMaterial;
    private Quaternion CurrentRotation;

    [HideInInspector] public bool Reveled = false;
    private PictureManager _picturemanager;
    private bool _clicked = false;
    private int _index;

    public void SetIndex(int id) { _index = id; }
    public int GetIndex() { return _index; }


    private void Start()
    {
        Reveled = false;
        _clicked = false;
        _picturemanager = GameObject.Find("PictureManager").GetComponent<PictureManager>();
        CurrentRotation = gameObject.transform.rotation;
    }
    private void OnMouseDown()
    {
        if (_clicked == false)
        {
            _picturemanager.CurrentPuzzleState = PictureManager.PuzzleState.PuzzleRotation;
            StartCoroutine(LoopRotation(45, false));
            _clicked = true;
        }
    }
    public void FlipBack()
    {
        if (gameObject.activeSelf)
        {
            _picturemanager.CurrentPuzzleState = PictureManager.PuzzleState.PuzzleRotation;
            Reveled = false;
            StartCoroutine(LoopRotation(45, true));
        }
    }
    IEnumerator LoopRotation(float angle, bool FirstMat)
    {
        var rot = 0f;
        const float direction = 1f;
        const float rotSpeed = 180.0f;
        const float rotspeed1 = 90.0f;
        var startange = angle;
        var assigned = false;

        if (FirstMat)
        {
            while (rot < angle)
            {
                {
                    var step = Time.deltaTime * rotspeed1;
                    gameObject.GetComponent<Transform>().Rotate(new Vector3(0, 2, 0) * step * direction);
                    if (rot >= (startange - 2) && assigned == false)
                    {
                        ApplyFirstMaterial();
                        assigned = true;
                    }

                    rot += (1 * step * direction);
                    yield return null;
                }
            }

        }
        else
        {
            while (angle > 0)
            {
                float step = Time.deltaTime * rotSpeed;
                gameObject.GetComponent<Transform>().Rotate(new Vector3(0, 2, 0) * step * direction);
                angle -= (1 * step * direction);
                yield return null;

            }
        }
        gameObject.GetComponent<Transform>().rotation = CurrentRotation;
        if (!FirstMat)
        {
            Reveled = true;
            ApplySecondMaterial();
            _picturemanager.CheckPicture();
        }
        else
        {
            _picturemanager.PuzzleRevealedNumber = PictureManager.RevealedNumber.NoRevealed;
            _picturemanager.CurrentPuzzleState = PictureManager.PuzzleState.CanRotate;
        }
        _clicked = false;
    }

    public void SetFirstMaterial(Material mat, string texturePath)
    {
        _firstMaterial = mat;
        _firstMaterial.mainTexture = Resources.Load(texturePath, typeof(Texture2D)) as Texture2D;
    }
    public void SetSecondMaterial(Material mat, string texturePath)
    {
        _secondMaterial = mat;
        _secondMaterial.mainTexture = Resources.Load(texturePath, typeof(Texture2D)) as Texture2D;
    }
    public void ApplyFirstMaterial()
    {
        gameObject.GetComponent<Renderer>().material = _firstMaterial;
    }
    public void ApplySecondMaterial()
    {
        gameObject.GetComponent<Renderer>().material = _secondMaterial;
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}

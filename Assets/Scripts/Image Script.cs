using UnityEngine;

public class ImageScript : MonoBehaviour
{
    [SerializeField] private GameObject FlipImage;
    [SerializeField] private GameControllerScript gameController;
    [SerializeField] private int _spriteid;

    private void OnMouseDown()
    {
        if (FlipImage.activeSelf)
        {
            FlipImage.SetActive(false);
            gameController.imageOpened(this);
        }
    }

    public int spriteid
    {
        get { return _spriteid; }
    }

    public void ChangeSprite(int id, Sprite image)
    {
        _spriteid = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }
    public void Close()
    {
        FlipImage.SetActive(true); // Hide image
    }
}

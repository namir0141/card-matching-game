using UnityEngine;

public class Picture : MonoBehaviour
{
    private Material _firstMaterial;
    private Material _secondMaterial;

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
}

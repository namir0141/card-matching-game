using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour
{
    public void loadscene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class SetGameSetting : MonoBehaviour
{
    public enum EButtonType
    {
        NotSet,
        PairNumberBtn,
        PuzzleCategoryBtn,
    };

    [SerializeField] public EButtonType buttonType = EButtonType.NotSet;
    [HideInInspector] public GameSettings.EPairNumber PairNumber = GameSettings.EPairNumber.NotSet;
    [HideInInspector] public GameSettings.EPuzlleCategories PuzlleCategories = GameSettings.EPuzlleCategories.NotSet;

    private void Start()
    {

    }
    public void SetGameOption(string GameSceneName)
    {
        var comp = gameObject.GetComponent<SetGameSetting>();
        switch (comp.buttonType)
        {
            case SetGameSetting.EButtonType.PairNumberBtn:
                GameSettings.Instance.SetPairNumver(comp.PairNumber);
                break;
            case EButtonType.PuzzleCategoryBtn:
                GameSettings.Instance.SetPuzlleCategories(comp.PuzlleCategories);
                break;
        }
        if (GameSettings.Instance.ALLSettingsReady())
        {
            SceneManager.LoadScene(GameSceneName);
        }
    }


}

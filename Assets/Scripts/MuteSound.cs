using UnityEngine;
using UnityEngine.UI;

public class MuteSound : MonoBehaviour
{
    public Sprite UnMutedSprite;
    public Sprite MutedSprite;

    private Button _button;
    SpriteState _state;
    void Start()
    {
        _button = GetComponent<Button>();
        if (GameSettings.Instance.IsSoundEffectMutedPermanently())
        {
            _state.pressedSprite = MutedSprite;
            _state.highlightedSprite = MutedSprite;
            _button.GetComponent<Image>().sprite = MutedSprite;
        }
        else
        {
            _state.pressedSprite = UnMutedSprite;
            _state.highlightedSprite = UnMutedSprite;
            _button.GetComponent<Image>().sprite = UnMutedSprite;
        }
    }

    private void OnGUI()
    {
        if (GameSettings.Instance.IsSoundEffectMutedPermanently())
        {
            _button.GetComponent<Image>().sprite = UnMutedSprite;
        }
    }

    public void ToggleFxIcon()
    {
        if (GameSettings.Instance.IsSoundEffectMutedPermanently())
        {
            _state.pressedSprite = UnMutedSprite;
            _state.highlightedSprite = UnMutedSprite;
            GameSettings.Instance.MuteSoundEffectPermanently(false);
        }
        else
        {
            _state.pressedSprite = MutedSprite;
            _state.highlightedSprite = MutedSprite;
            GameSettings.Instance.MuteSoundEffectPermanently(true);
        }
        _button.spriteState = _state;
    }
}

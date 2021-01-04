using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TemplateSetter : MonoBehaviour
{
    public SpriteData spriteData;
    public ColorPalette Color;

    public void OnValidate()
    {
        if (Color == ColorPalette.Blue)SetColorGroup(0);     
        if (Color == ColorPalette.Yellow)SetColorGroup(1);       
        if (Color == ColorPalette.Pink)SetColorGroup(2);
        if (Color == ColorPalette.Green)SetColorGroup(3);
    }
    public void SetColorGroup(int i)
    {
        spriteData.pauseUI.sprite = spriteData.pauseIcons[i];
        spriteData.nextUI.sprite = spriteData.nextButton[i];
        spriteData.restartUI.sprite = spriteData.restartButtons[i];
        spriteData.playUI.sprite = spriteData.ContuniueButton[i];
        spriteData.replayUI.sprite = spriteData.replayButton[i];
    }
}
public enum ColorPalette { Blue, Yellow, Pink, Green  }
[System.Serializable]
public class SpriteData
{
    [Header("UI Elements")]
    public Image pauseUI;
    public Image nextUI;
    public Image restartUI;
    public Image playUI;
    public Image replayUI;
    public List<Sprite> pauseIcons;
    public List<Sprite> nextButton;
    public List<Sprite> restartButtons;
    public List<Sprite> ContuniueButton;
    public List<Sprite> replayButton;
}
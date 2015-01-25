using UnityEngine;
using UnityEngine.UI;

public class BigStat : MonoBehaviour
{
    public Text NameText;
    public Image HeroIcon;

    public Slider HpSlider;
    public Text HpValueText;

    public Slider DmgSlider;
    public Text DmgValueText;

    public Slider DefSlider;
    public Text DefValueText;

    public Slider AsSlider;
    public Text AsValueText;

    public Image ItemIcon1;
    public Image ItemIcon2;
    public Image ItemIcon3;

    public void SetHeroIcon(Sprite icon)
    {
        HeroIcon.sprite = icon;
    }

    public void SetName(string name)
    {
        NameText.text = name;
    }

    public void SetHp(float percent, float value)
    {
        HpSlider.value = percent;
        HpValueText.text = value + "";
    }

    public void SetDmg(float percent, float value)
    {
        DmgSlider.value = percent;
        DmgValueText.text = value + "";
    }

    public void SetDef(float percent, float value)
    {
        DefSlider.value = percent;
        DefValueText.text = value + "";
    }

    public void SetAttackSpeed(float percent, float value)
    {
        AsSlider.value = percent;
        AsValueText.text = value + "";
    }

    public void SetDmg(float percent, float value, float addVal)
    {
        UpdateUi(DmgSlider, DmgValueText, percent, value, addVal);
    }

    public void SetDef(float percent, float value, float addVal)
    {
        UpdateUi(DefSlider, DefValueText, percent, value, addVal);
    }

    public void SetAttackSpeed(float percent, float value, float addVal)
    {
        UpdateUi(AsSlider, AsValueText, percent, value, addVal);
    }

    private void UpdateUi(Slider slider, Text valText, float percent, float val, float addVal)
    {
        var txt = val + "";

        if (addVal > 0)
        {
            txt += "<color=green>+" + addVal + "</color>";
        }
        else if (addVal < -0.001f)
        {
            txt += "<color=red>" + addVal + "</color>";
        }
        else
        {

        }

        if (slider != null)
            slider.value = percent;

        if (valText != null)
            valText.text = txt;
    }
    
    public void SetItemsIcons(Sprite icon1, Sprite icon2, Sprite icon3)
    {
        ItemIcon1.sprite = icon1;
        ItemIcon2.sprite = icon2;
        ItemIcon3.sprite = icon3;
    }
}

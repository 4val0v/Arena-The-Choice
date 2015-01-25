using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TopBar : MonoBehaviour
{
    public Text PlayerName;
    public Text EnemyName;

    public Slider EnemyHp;
    public Text EnemyHpValueText;

    [SerializeField]
    private BigStat _leftBigStat;

    [SerializeField]
    private BigStat _rightBigStat;

    public Slider HpSlider;
    public Text HpValueText;

    public Slider DmgSlider;
    public Text DmgValueText;

    public Slider DefSlider;
    public Text DefValueText;

    public Slider AsSlider;
    public Text AsValueText;

    public void UpdatePlayerName(string name)
    {
        PlayerName.text = name;
        _leftBigStat.SetName(name);
    }

    public void UpdateEnemyName(string name)
    {
        EnemyName.text = name;
        _rightBigStat.SetName(name);
    }

    public void SetPlayerHp(float percent, float absolute)
    {
        HpSlider.value = percent;
        HpValueText.text = absolute + "";
        _leftBigStat.SetHp(percent, absolute);
    }

    public void SetEnemyHp(float percent, float absolute)
    {
        EnemyHp.value = percent;
        EnemyHpValueText.text = absolute + "";
        _rightBigStat.SetHp(percent, absolute);
    }

    public void SetDmg(float val)
    {
        _leftBigStat.SetDmg(1f, val);

        DmgSlider.value = 1f;
        DmgValueText.text = val + "";
    }

    public void SetDef(float val)
    {
        _leftBigStat.SetDef(1f, val);

        DefSlider.value = 1f;
        DefValueText.text = val + "";
    }

    public void SetAttackSpeed(float val)
    {
        _leftBigStat.SetAttackSpeed(1f, val);

        AsSlider.value = 1f;
        AsValueText.text = val + "";
    }

    public void SetDmg(float dmg, float addDmg)
    {
        _leftBigStat.SetDmg(1f, dmg, addDmg);

        UpdateUi(DmgSlider, DmgValueText, 1f, dmg, addDmg);
    }

    public void SetDef(float def, float addDef)
    {
        _leftBigStat.SetDef(1f, def, addDef);

        UpdateUi(DefSlider, DefValueText, 1f, def, addDef);
    }

    public void SetAttackSpeed(float attackSpeed, float addAttackSpeed)
    {
        _leftBigStat.SetAttackSpeed(1f, attackSpeed, addAttackSpeed);

        UpdateUi(AsSlider, AsValueText, 1f, attackSpeed, addAttackSpeed);
    }

    #region right
    public void SetEnemyDmg(float dmg)
    {
        _rightBigStat.SetDmg(1f, dmg);
    }

    public void SetEnemyDef(float def)
    {
        _rightBigStat.SetDef(1f, def);
    }

    public void SetEnemyAttackSpeed(float attackSpeed)
    {
        _rightBigStat.SetAttackSpeed(1f, attackSpeed);
    }

    #endregion

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

    public void SetItemsIconsLeft(Sprite icon1, Sprite icon2, Sprite icon3)
    {
        _leftBigStat.SetItemsIcons(icon1, icon2, icon3);
    }
    public void SetItemsIconsRight(Sprite icon1, Sprite icon2, Sprite icon3)
    {
        _rightBigStat.SetItemsIcons(icon1, icon2, icon3);
    }

    public void SwitchStat(bool isBig)
    {
        _leftBigStat.gameObject.SetActive(isBig);
        _rightBigStat.gameObject.SetActive(isBig);

        HpSlider.gameObject.SetActive(!isBig);
        HpValueText.gameObject.SetActive(!isBig);

        DmgSlider.gameObject.SetActive(!isBig);
        DmgValueText.gameObject.SetActive(!isBig);

        AsSlider.gameObject.SetActive(!isBig);
        AsValueText.gameObject.SetActive(!isBig);

        DefSlider.gameObject.SetActive(!isBig);
        DefValueText.gameObject.SetActive(!isBig);

        EnemyHp.gameObject.SetActive(!isBig);

        EnemyName.gameObject.SetActive(!isBig);
        PlayerName.gameObject.SetActive(!isBig);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

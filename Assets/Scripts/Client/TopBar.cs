using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TopBar : MonoBehaviour
{
    public Text PlayerName;
    public Text EnemyName;

    public Slider PlayerHp;
    public Slider EnemyHp;

    public Text DmgText;
    public Text Deftext;
    public Text AttackSpeedText;

    public void UpdatePlayerName(string name)
    {
        PlayerName.text = name;
    }

    public void UpdateEnemyName(string name)
    {
        EnemyName.text = name;
    }

    public void SetPlayerHp(float percent)
    {
        PlayerHp.value = percent;
    }

    public void SetEnemyHp(float percent)
    {
        EnemyHp.value = percent;
    }

    public void SetDmg(float dmg)
    {
        DmgText.text = dmg + "";
    }

    public void SetDef(float def)
    {
        Deftext.text = def + "";
    }

    public void SetAttackSpeed(float attackSpeed)
    {
        AttackSpeedText.text = attackSpeed + "";
    }

    public void SetDmg(float dmg, float addDmg)
    {
        var txt = dmg + "";

        if (addDmg > 0)
        {
            txt += "<color=green>+" + addDmg + "</color>";
        }
        else if (addDmg < -0.001f)
        {
            txt += "<color=red>" + addDmg + "</color>";
        }
        else
        {

        }

        DmgText.text = txt;
    }

    public void SetDef(float def, float addDef)
    {
        var txt = def + "";

        if (addDef > 0)
        {
            txt += "<color=green>+" + addDef + "</color>";
        }
        else if (addDef < -0.001f)
        {
            txt += "<color=red>" + addDef + "</color>";
        }

        Deftext.text = txt;
    }

    public void SetAttackSpeed(float attackSpeed, float addAttackSpeed)
    {
        var txt = attackSpeed + "";

        if (addAttackSpeed > 0)
        {
            txt += "<color=green>+" + addAttackSpeed + "</color>";
        }
        else if (addAttackSpeed < -0.001f)
        {
            txt += "<color=red>" + addAttackSpeed + "</color>";
        }

        AttackSpeedText.text = txt;
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

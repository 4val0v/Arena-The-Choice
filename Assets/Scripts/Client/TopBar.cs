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

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

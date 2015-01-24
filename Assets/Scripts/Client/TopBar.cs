using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TopBar : MonoBehaviour
{
    public Text PlayerName;
    public Text EnemyName;

    public Slider PlayerHp;
    public Slider EnemyHp;

    public void UpdatePlayerName(string name)
    {
        PlayerName.text = name;
    }

    public void UpdateEnemyHp(string name)
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

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

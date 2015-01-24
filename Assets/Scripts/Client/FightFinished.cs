using UnityEngine;
using UnityEngine.UI;

public class FightFinished : MonoBehaviour
{
    public Text FinishText;

    private INetClient Client { get { return PunNetClient.Instance; } }

    public void SetWinner(bool isYouWinner)
    {
        FinishText.text = isYouWinner ? Texts.WINNER_TITLE : Texts.LOSER_TITLE;
    }

    public void OnBtnClick()
    {
        Client.Disconnect();
    }
}

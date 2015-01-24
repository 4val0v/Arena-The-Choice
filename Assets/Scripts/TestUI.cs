using UnityEngine;
using System.Collections;

public class TestUI : MonoBehaviour
{
    private INetClient _client { get { return PunNetClient.Instance; } }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private string playerName = "";

    void OnGUI()
    {
        GUILayout.BeginVertical();
       // GUILayout.Label("ID:" + );
        playerName = GUILayout.TextField(playerName, GUILayout.Width(150));

        if (GUILayout.Button("SetName", GUILayout.Width(150)))
        {
            _client.UpdateName(playerName);
        }

        //if (GUI.Button(new Rect(10, 100, 140, 30), "Bot"))
        //{
        //    _client.CreateOrJoinToBattle(GameMode.PvE);
        //}

        if (GUILayout.Button("Server", GUILayout.Width(150)))
        {
            _client.CreateOrJoinToBattle(GameMode.PvP);
        }

        GUILayout.EndVertical();

        DrawEnemyData();
    }

    private void DrawEnemyData()
    {

        GUI.BeginGroup(new Rect(Screen.width - 400, 10, 400, Screen.height));
        GUILayout.BeginVertical();

        if (_client.EnemyData == null)
        {
            GUILayout.Label("No player data!");
        }
        else
        {
            var data = _client.EnemyData;

            GUILayout.Label("EnemyId:" + data.Id);
            GUILayout.Label("EnemyName:" + data.Name);
            GUILayout.Label("EnemyClass:" + data.Class);

            if (data.EquippedItems != null)
            {
                GUILayout.Label("Items(count:" + data.EquippedItems.Count + "):");

                foreach (var item in data.EquippedItems)
                {
                    GUILayout.Label("*" + item);
                }
            }
        }


        GUILayout.EndVertical();
        GUI.EndGroup();
    }
}

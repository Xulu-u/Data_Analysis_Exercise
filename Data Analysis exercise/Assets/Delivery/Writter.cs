using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using UnityEditor;

enum Table
{
    PlayerPositionEvent = 0,
    PlayerDeathEvent,
    PlayerFallsEvent,
    EnemyKills,
    NullEvent
}

public class Writter : MonoBehaviour
{
    string[] paths = new string[10];

    void Start()
    {
        if (!AssetDatabase.IsValidFolder("Assets/CSV"))
        {
            AssetDatabase.CreateFolder("Assets", "CSV");
        }

        
        paths[(int)Table.PlayerPositionEvent] = Application.dataPath + "/CSV/" + "PlayerPositionEvent.csv";
        paths[(int)Table.PlayerDeathEvent] = Application.dataPath + "/CSV/" + "PlayerDeathEvent.csv";
        paths[(int)Table.PlayerFallsEvent] = Application.dataPath + "/CSV/" + "PlayerFallsEvent.csv";
        paths[(int)Table.EnemyKills] = Application.dataPath + "/CSV/" + "EnemyKills.csv";


        if (!File.Exists(getPath(Table.PlayerPositionEvent)))
        {
            string[] RowHeadersPosition = { "PosX", "PosY", "PosZ", "SessionID", "PlayerID", "DateTime" };
            Save(RowHeadersPosition, Table.PlayerPositionEvent);
        }
        if (!File.Exists(getPath(Table.PlayerDeathEvent)))
        {
            string[] RowHeadersPosition = { "PosX", "PosY", "PosZ", "Enemy", "SessionID", "PlayerID", "DateTime" };
            Save(RowHeadersPosition, Table.PlayerDeathEvent);
        }
        if (!File.Exists(getPath(Table.PlayerFallsEvent)))
        {
            string[] RowHeadersPosition = { "PosX", "PosY", "PosZ", "Fall Type", "SessionID", "PlayerID", "DateTime" };
            Save(RowHeadersPosition, Table.PlayerFallsEvent);
        }
        
        if (!File.Exists(getPath(Table.EnemyKills)))
        {
            string[] RowHeadersPosition = { "EnemyPos X", "EnemyPos Y", "EnemyPos Z", "Enemy Type", "SessionID", "PlayerID", "DateTime" };
            Save(RowHeadersPosition, Table.EnemyKills);
        }
      

    }//--start

    void Save(string[] rowData, Table table)
    {

        string delimiter = ",";
        string filePath = getPath(table);

        File.AppendAllText(filePath, string.Join(delimiter, rowData) + ",\n");
    }

    private string getPath(Table table)
    {
        return paths[(int)table];
    }


    public void ReceiveEvent(object eventData)
    {
        // Properties serialization
        FieldInfo[] properties = eventData.GetType().GetFields();

        string[] rowDataTemp = new string[properties.Length];

        int i = 0;
        foreach (FieldInfo property in properties)
            rowDataTemp[i++] = property.GetValue(eventData).ToString().Replace(',', '.');

        Save(rowDataTemp, GetTable(eventData));
    }

    Table GetTable(object eventData)
    {
      
        if (eventData is PlayerPositionEvent)
            return Table.PlayerPositionEvent;
        else if (eventData is PlayerDeathEvent)
            return Table.PlayerDeathEvent;
        else if (eventData is PlayerFallsEvent)
            return Table.PlayerFallsEvent;
        else if (eventData is EnemyKillsEvent)
            return Table.EnemyKills;
        

        return Table.NullEvent;
    }
}

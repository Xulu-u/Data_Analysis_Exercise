using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EventFilter
{
    None = -1,
    Position,
    PlayerDeath,
    Fall,
    EnemyDeath,
};
public enum ENEMY_TYPE
{
    ALL,
    SPITTER,
    CHOMPER
}
public enum SURFACE_TYPE
{
    ALL,
    FREE_FALL,
    ACID
}
public class EventData
{
  
    public uint player_id;
    public uint session_id;
    public DateTime timestamp;

    public EventData()
    {
        player_id = 0;
        session_id = 0;
        timestamp = System.DateTime.Now;
    }

    public EventData(uint _sessions_id, uint _player_id, DateTime time)
    {
        session_id = _sessions_id;
        player_id = _player_id;
        timestamp = time;
    }

    public string GetJSON()
    {
        return JsonUtility.ToJson(this);
    }
}

public class PlayerPositionEvent : EventData     
{
    public PlayerPositionEvent()
    {
        x = 0;
        y = 0;
        z = 0;
    }

    public PlayerPositionEvent(uint session_id, uint player_id, DateTime time, Vector3 pos) : base(session_id, player_id, time)
    {
        x = pos.x;
        y = pos.y;
        z = pos.z;
    }
    public float x, y, z;
}
public class PlayerDeathEvent : EventData    // Player death position | Also used for Heatmap
{
    public PlayerDeathEvent()
    {
        x = 0;
        y = 0;
        z = 0;
        enemy = 0;
    }

    public PlayerDeathEvent(uint session_id, uint player_id, DateTime time, Vector3 pos, ENEMY_TYPE enemy) : base(session_id, player_id, time)
    {
        x = pos.x;
        y = pos.y;
        z = pos.z;
        this.enemy = (int)enemy;
    }
    public float x, y, z;
    public int enemy;
}
public class PlayerFallsEvent : EventData    // Player fall position & type of surface where player has fallen | Also used for Heatmap
{
    public PlayerFallsEvent()
    {
        x = 0;
        y = 0;
        z = 0;
        surface = 0;
    }

    public PlayerFallsEvent(uint session_id, uint player_id, DateTime time, Vector3 pos, SURFACE_TYPE surface_name) : base(session_id, player_id, time)
    {
        x = pos.x;
        y = pos.y;
        z = pos.z;
        surface = (int)surface_name;
    }
    public float x, y, z;
    public int surface;
}
public class EnemyKillsEvent : EventData    // Enemy position where killed by the player & enemy type name
{
    public EnemyKillsEvent()
    {
        x = 0;
        y = 0;
        z = 0;
        enemy = 0;
    }

    public EnemyKillsEvent(uint session_id, uint player_id, DateTime time, Vector3 enemy_pos, ENEMY_TYPE enemy_name) : base(session_id, player_id, time)
    {
        x = enemy_pos.x;
        y = enemy_pos.y;
        z = enemy_pos.z;
        enemy = (int)enemy_name;
    }
    public float x, y, z;
    public int enemy;
}

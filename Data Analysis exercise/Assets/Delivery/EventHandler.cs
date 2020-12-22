using Gamekit3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    private GameObject ellen;
    private GameObject camera;
    public static EventData eventdata;
    public static EventHandler eventhandler;

    public List<EventData> events;

    private float timeStart = 0;
    private System.DateTime timeAwake;

    public uint PlayerID = 0;
    private uint SessionID = 0;


    public void SendEventData(object eventData)
    {
        ellen.SendMessage("ReceiveEvent", eventData);
    }

    private void OnEnable()
    {
        Damageable.PlayerDeathEvent += AddPlayerDeathEvent;
        Damageable.EnemyDeathEvent += AddEnemyKilledEvent;
        DeathVolume.PlayerFallsEvent += AddPlayerFallEvent;
        

        timeAwake = System.DateTime.Now;
    }

    private void OnValidate()
    {
        PlayerID = (uint)Mathf.Clamp((int)PlayerID, 0, 99999);
    }



    private void OnDisable()
    {
        Damageable.PlayerDeathEvent -= AddPlayerDeathEvent;
        Damageable.EnemyDeathEvent -= AddEnemyKilledEvent;
        DeathVolume.PlayerFallsEvent -= AddPlayerFallEvent;

    }

    public void Awake()
    {
        ellen = GameObject.FindGameObjectWithTag("Player");
        camera = GameObject.FindGameObjectWithTag("MainCamera");

        SessionID = (uint)Random.Range(0, 99999);
    }

    void Start()
    {
        CreateLists();

        if (eventhandler == null)
        {
            eventhandler = this;
        }

        InvokeRepeating("UpdateInfoEvent", 0.0f, 0.15f);
    }
    void Update()
    {
        if (events.Count == 0)
            return;
    }

    void UpdateInfoEvent()
    {
        AddPlayerPositionEvent();
    }

    void CreateLists()
    {
        events = new List<EventData>();
    }

    public void AddPlayerPositionEvent()
    {
        SendEventData(new PlayerPositionEvent(SessionID, PlayerID, System.DateTime.Now, ellen.transform.position));
    }

    public void AddPlayerDeathEvent(ENEMY_TYPE type)
    {
        SendEventData(new PlayerDeathEvent(SessionID, PlayerID, System.DateTime.Now, ellen.transform.position, type));
    }
    public void AddEnemyKilledEvent(GameObject enemy, ENEMY_TYPE type)
    {
        SendEventData(new EnemyKillsEvent(SessionID, PlayerID, System.DateTime.Now, enemy.transform.position, type));
    }

    public void AddPlayerFallEvent(SURFACE_TYPE type)
    {
        SendEventData(new PlayerFallsEvent(SessionID, PlayerID, System.DateTime.Now, ellen.transform.position, type));
    }

    public void StartTimer()
    {
        timeStart = Time.time;
    }

    public float GetStartTime()
    {
        return timeStart;
    }
}
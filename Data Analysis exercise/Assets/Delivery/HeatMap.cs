using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class HeatMap : MonoBehaviour
{
    int gridSize = 200;
    float mapX = -34, mapY = -40;
    int[,] grid;

    public GameObject heatMapCube;
    public float cubeSize = 1;
    public float transparency = 0.8f;

    public bool showPosition = false;
    public bool showDeath = false;
    public bool showFall = false;
    public bool showEnemies = false;

    //--------------------------
    [HideInInspector]
    public List<GameObject> instancedCubes;

    private Reader reader;
    public Gradient ColorGradient;

    private float heatMax = 0;
    private float heatMin = 0;

    public void Awake()
    {
        reader = gameObject.GetComponent<Reader>();

        gridSize = (int)(cubeSize * (float)gridSize);
        gridSize = (int)(cubeSize * (float)gridSize);

        grid = new int[gridSize, gridSize];
    }
    void Start()
    { }

    public void ShowPosition()
    {
        showPosition = !showPosition;
        showDeath = false;
        showFall = false;
        showEnemies = false;
    }
    public void ShowDeath()
    {
        showPosition = false;
        showDeath = !showDeath;
        showFall = false;
        showEnemies = false;
    }
    public void ShowFall()
    {
        showPosition = false;
        showDeath = false;
        showFall = !showFall;
        showEnemies = false;
    }
    public void ShowEnemies()
    {
        showPosition = false;
        showDeath = false;
        showFall = false;
        showEnemies = !showEnemies;
    }

    public void clearMap()
    {
        foreach (GameObject obj in instancedCubes)
            Destroy(obj);
        instancedCubes.Clear();
    }

    public void reloadHeatmap()
    {
        grid = new int[gridSize, gridSize];
        heatMapCube.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        CountEvents();
        clearMap();
        ShowGrid();
    }

    private void SetValue(float x, float y)
    {
        int newX = (int)(x / cubeSize - mapX / cubeSize);
        int newY = (int)(y / cubeSize - mapY / cubeSize);

        if (newX >= 0 && newY >= 0 && newX < gridSize && newY < gridSize)
            grid[newX, newY]++;
    }

    private void OnValidate()
    {
        if (reader != null && instancedCubes.Count > 0)
            reloadHeatmap();
    }

    public void CountEvents()
    {
        float x = 0, y = 0;

        if (showPosition)
        {
            for (int i = 0; i < reader.arrPosition.Count; i++)
            {
                EventData eventData = reader.arrPosition[i];
                x = ((PlayerPositionEvent)eventData).x;
                y = ((PlayerPositionEvent)eventData).z;
                SetValue(x, y);
            }
        }

        else if (showDeath)
        {
            for (int i = 0; i < reader.arrDeath.Count; i++)
            {
                EventData eventData = reader.arrDeath[i];
                x = ((PlayerDeathEvent)eventData).x;
                y = ((PlayerDeathEvent)eventData).z;
                SetValue(x, y);
            }
        }

        else if (showFall)
        {
            for (int i = 0; i < reader.arrFalls.Count; i++)
            {
                EventData eventData = reader.arrFalls[i];
                x = ((PlayerFallsEvent)eventData).x;
                y = ((PlayerFallsEvent)eventData).z;
                SetValue(x, y);
            }
        }

        else if (showEnemies)
        {
            for (int i = 0; i < reader.arrEnemyKills.Count; i++)
            {
                EventData eventData = reader.arrEnemyKills[i];
                x = ((EnemyKillsEvent)eventData).x;
                y = ((EnemyKillsEvent)eventData).z;
                SetValue(x, y);
            }
        }

        // Change dynamically color
        heatMax = 0;
        heatMin = 0;
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (grid[i, j] > 0)
                {
                    if (grid[i, j] >= heatMax)
                        heatMax = grid[i, j];
                    else if (heatMin > grid[i, j] || heatMin == 0)
                        heatMin = grid[i, j];
                }
            }
        }
        heatMax -= heatMin;
    }

    void ShowGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (grid[x, y] > 0)
                    SpawnCube(x, y, grid[x, y]);
            }
        }
    }

    void SpawnCube(int x, int y, int count)
    {
        float pos_x = x * cubeSize + mapX + cubeSize / 2;
        float pos_y = y * cubeSize + mapY + cubeSize / 2;
        Vector3 pos = new Vector3(pos_x, GetHeight(pos_x, pos_y), pos_y);

        GameObject cube = Instantiate(heatMapCube, pos, Quaternion.identity);
        instancedCubes.Add(cube);

        float f = Mathf.Clamp01((float)count / heatMax);
        Color c = ColorGradient.Evaluate(f);
        c.a = transparency;
        cube.GetComponent<Renderer>().material.SetColor("_Color", c);
    }

    private float GetHeight(float x, float y)
    {
        Vector3 pos = new Vector3(x, 100, y);
        RaycastHit hit;
        int layerMask = 1 << 16;
        if (Physics.Raycast(pos, Vector3.down, out hit, Mathf.Infinity, layerMask))
            return hit.point.y + cubeSize / 2;
        return 0;
    }
}

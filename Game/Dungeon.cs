using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public DungeonSkillManager skillManager;
    public DungeonUI dungeonUI;
    public DungeonPlayer player;
    private bool[] questes = new bool[3];
    int deathCount = 0;
    int eliminatedBoss = 0;
    float playTime;

    void Start()
    {
        if (skillManager == null) skillManager = FindAnyObjectByType<DungeonSkillManager>();
        if (dungeonUI == null) dungeonUI = FindAnyObjectByType<DungeonUI>();
        if (player == null) player = FindAnyObjectByType<DungeonPlayer>();
        Init();
    }

    private void Init()
    {
        //SpawnMonsters(100);
    }

    void Update()
    {
        playTime += Time.deltaTime;
        dungeonUI.UpdateTimeUI(playTime);

        Quest();
    }

    void SpawnMonsters(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector2 spawnPos = (Vector2)player.transform.position + Random.insideUnitCircle * Random.Range(10, 20);
            Debug.Log(spawnPos);
            MonsterManager.Instance.SpawnMonster(spawnPos);
        }
    }

    void Quest()
    {
        if (playTime / 60 > 15)
        {
            dungeonUI.UpdateQuestUI(0, false);
        }
        if (deathCount > 0)
        {
            dungeonUI.UpdateQuestUI(2, false, deathCount);
        }
        if (eliminatedBoss > 0)
        {
            dungeonUI.UpdateQuestUI(4, true, eliminatedBoss);
        }
    }

    public void LevelUp(int level)
    {
        dungeonUI.UpdateLevelUI(level);
        skillManager.OpenSkillPopUp();
    }

    public void EXPUp(float currentEXP)
    {
        dungeonUI.UpdateEXPUI(currentEXP);
    }
}

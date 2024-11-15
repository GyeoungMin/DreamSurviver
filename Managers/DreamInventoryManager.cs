using System.Collections.Generic;
using UnityEngine;

public class DreamInventoryManager : SingletonManager<DreamInventoryManager>
{
    int dreamId = 10000;
    //던전 정보
    DungeonMapData mapData;
    //적 정보
    MonsterData monsterData;
    //플레이어 정보
    PlayerStatusData status;
    DungeonPlayerStatus playerStatus;
    //스킬 정보
    DungeonSkillManager dungeonSkillManager;

    List<SkillInfo> skills = new List<SkillInfo>();
    List<DreamInfo> dreamCrystalInfo = new List<DreamInfo>();

    public RewardUI rewardUI;

    [HideInInspector] public string crystalImageNum;

    private void Start()
    {
        bool isdata = InfoManager.Instance.LoadInfo<DreamInfo>("dreamInfo");

        crystalImageNum = UnityEngine.Random.Range(1, 101).ToString();
        mapData = new DungeonMapData();
        monsterData = new MonsterData();
        playerStatus = FindAnyObjectByType<DungeonPlayerStatus>();
        dungeonSkillManager = FindAnyObjectByType<DungeonSkillManager>();

        if (isdata)
        {
            dreamId = InfoManager.Instance.GetInfo<DreamInfo>("dreamInfo").Count + 10000;
        }
        else
        {
            dreamId = 10000;
        }
    }
    //게임 종료 후 데이터 저장시 호출
    public void SaveData()
    {
        LoadData();
        int skillCount = 0;
        #region 임시 UI - 삭제 예정
        if (rewardUI != null)
        {
            rewardUI.gameObject.SetActive(true);
        }
        #endregion
        if (dungeonSkillManager != null)
        {
            skills = dungeonSkillManager.dungeonSkills;
            dungeonSkillManager.SaveSkill();
            skillCount = dungeonSkillManager.dungeonSkills.Count;
        }
        if (playerStatus != null)
        {
            dreamCrystalInfo.Add(new DreamInfo(dreamId, mapData,
                new PlayerInfo(mapData.characterType, crystalImageNum, Grade(skillCount)),
                monsterData,
                new PlayerStatusData(playerStatus.level, playerStatus.power, playerStatus.damage, playerStatus.maxHp,
                playerStatus.recovery, playerStatus.protection, playerStatus.protectionProb,
                playerStatus.criticalPower, playerStatus.criticalProb,
                playerStatus.expIncrease, playerStatus.goldIncrease, playerStatus.coolDown,
                playerStatus.attackRange, playerStatus.attackDuration, playerStatus.attackOfNumber,
                playerStatus.projectorSpeed, playerStatus.currentHp),
                skills));
        }
        InfoManager.Instance.SaveInfo(dreamCrystalInfo, "dreamInfo");
    }
    public List<DreamInfo> LoadData()
    {
        dreamCrystalInfo = InfoManager.Instance.GetInfo<DreamInfo>("dreamInfo");

        return dreamCrystalInfo;
    }
    public string Grade(int value)
    {
        string grade = "";
        switch (value)
        {
            case int when value <= 1:
                grade = "C";
                break;
            case 2:
                grade = "B";
                break;
            case 3:
                grade = "A";
                break;
            case 4:
                grade = "S";
                break;
            case 5:
                grade = "SS";
                break;
        }
        return grade;
    }
}

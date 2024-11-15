using UnityEngine;
using System.Collections.Generic;
using System;

//스킬 적용 및 관리
public class StageSkillManager : MonoBehaviour
{
    private List<SkillInfo> skillInfos = new List<SkillInfo>();
    private List<List<SkillInfo>> skillInfoList = new List<List<SkillInfo>>();
    private List<DreamInfo> dreamInfoList = new List<DreamInfo>();
    DungeonPlayerStatus status;
    // 스킬 데이터 읽기
    private void Start()
    {
        LoadAutoSkill();
        //LoadMemoryInfo();
        Debug.Log("파편 개수(스킬) : " + LoadSkillInfo().Count);
        status = FindAnyObjectByType<DungeonPlayerStatus>();
    }
    //스킬 불러오기
    public void LoadAutoSkill()
    {
        //던전 스킬을 불러옴
        //if (InfoManager.Instance.LoadInfo<SkillInfo>("dungeonSkillInfo"))
        //{
        //    skillInfos = InfoManager.Instance.GetInfo<SkillInfo>("dungeonSkillInfo");
        //    foreach (SkillInfo data in skillInfos)
        //    {
        //        Debug.Log($"불러온 스킬 : {data.name}, type : {data.type}, level : {data.dungeonLevel}");
        //    }
        //}
    }
    //메인(방치씬)에서 스킬을 자동 발동
    public void UseAutoSkill()
    {
        //foreach (var skill in skillInfos)
        //{
        //    Type type = Type.GetType(skill.name);
        //    GameObject autoGo = new GameObject(type.Name);
        //    autoGo.AddComponent(type);
        //    autoGo.tag = "Skill";
        //    for (int i = 0; i < skill.dungeonLevel - 1; i++)
        //    {
        //        autoGo.SetActive(false);
        //        autoGo.SetActive(true);
        //    }
        //}
    }
    //파편목록 불러오기
    public List<List<SkillInfo>> LoadSkillInfo()
    {
        if (InfoManager.Instance.LoadInfo<List<SkillInfo>>("dungeonSkillsInfo"))
        {
            skillInfoList = InfoManager.Instance.GetInfo<List<SkillInfo>>("dungeonSkillsInfo");
            int memoryCount = 0;
            foreach (var memories in skillInfoList)
            {
                memoryCount++;
                foreach (var data in memories)
                {
                    Debug.Log($"{memoryCount}번째 파편 - skillname : {data.name}, type : {data.type}, level : {data.dungeonLevel}");
                }
            }
        }
        return skillInfoList;
    }
    public List<DreamInfo> LoadDreamInfo()
    {
        if (InfoManager.Instance.LoadInfo<DreamInfo>("dreamInfo"))
        {
            dreamInfoList = InfoManager.Instance.GetInfo<DreamInfo>("dreamInfo");
        }
        return dreamInfoList;
    }
    //선택한 파편에 들어있는 스킬을 자동 발동
    public void UseSkill(List<SkillInfo> memories)
    {
        GameObject[] activedGO = GameObject.FindGameObjectsWithTag("Skill");
        if (activedGO.Length > 0)
        {
            for (int i = 0; i < activedGO.Length; i++)
            {
                Destroy(activedGO[i]);
            }
        }

        foreach (var skill in memories)
        {
            //액티브 스킬만 적용한다 - 패시브 스킬은 스탯으로 저장됨
            if (skill.type == "ActiveSkill")
            {
                Type type = Type.GetType(skill.name);
                GameObject autoGo = new GameObject(type.Name);
                autoGo.AddComponent(type);
                autoGo.tag = "Skill";
                for (int i = 0; i < skill.dungeonLevel - 1; i++)
                {
                    autoGo.SetActive(false);
                    autoGo.SetActive(true);
                }
            }
        }
    }
    public void LoadStatus(DreamInfo dreamInfo)
    {
        //패시브 스킬을 플레이어 스탯으로 저장 후 적용
        if (status != null && InfoManager.Instance.LoadInfo<DreamInfo>("dreamInfo"))
        {
                status.level = dreamInfo.status.level;
                status.power = dreamInfo.status.power;
                status.damage = dreamInfo.status.damage;
                status.maxHp = dreamInfo.status.maxHp;
                status.recovery = dreamInfo.status.recovery;
                status.protection = dreamInfo.status.protection;
                status.protectionProb = dreamInfo.status.protectionProb;
                status.criticalPower = dreamInfo.status.criticalPower;
                status.criticalProb = dreamInfo.status.criticalProb;
                status.expIncrease = dreamInfo.status.expIncrease;
                status.goldIncrease = dreamInfo.status.goldIncrease;
                status.coolDown = dreamInfo.status.coolDown;
                status.attackRange = dreamInfo.status.attackRange;
                status.attackDuration = dreamInfo.status.attackDuration;
                status.attackOfNumber = dreamInfo.status.attackOfNumber;
                status.projectorSpeed = dreamInfo.status.projectorSpeed;
                status.currentHp = dreamInfo.status.currentHp;
        }
    }
}

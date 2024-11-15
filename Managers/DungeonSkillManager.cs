using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

//스킬 적용 및 관리
public class DungeonSkillManager : MonoBehaviour
{
    [Tooltip("스킬이름(스크립트)을 직접 입력하여 테스트 할 수 있습니다.")]
    public string skillName;//UI에서 받아올 스킬 이름
    private List<SkillData> skillDatas = new List<SkillData>(); //불러올 스킬 데이터
    private List<SkillData> uiDatas = new List<SkillData>(); // UI에 보낼 스킬 데이터
    [Header("현재 획득한 스킬이 나타납니다.")]
    public List<string> activeSkills = new List<string>(); // 액티브 스킬 저장
    public List<string> passiveSkills = new List<string>(); // 패시브 스킬 저장
    public List<SkillInfo> dungeonSkills = new List<SkillInfo>(); // 획득한 모든 스킬 저장
    [Header("팝업UI를 넣어주세요")]
    public GameObject selectSkillPopUp;//스킬 선택 창

    private Dictionary<string, int> systemLevels = new Dictionary<string, int>(); // 시스템 레벨
    private Dictionary<string, string> skillDescs = new Dictionary<string, string>(); // 스킬 설명

    public List<List<SkillInfo>> memories = new List<List<SkillInfo>>(); // 꿈의 파편 목록

    // 스킬 데이터 읽기
    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        DataManager.Instance.LoadData<SkillData>();
        SkillData[] datas = DataManager.Instance.GetData<SkillData>();

        //skillDatas.AddRange(datas);
        skillDatas = datas.ToList();
        systemLevels = skillDatas.ToDictionary(data => data.name, data => data.systemLevel);
        skillDescs = skillDatas.ToDictionary(data => data.name, data => data.description);
        //스킬 최초 선택 범위
        if (dungeonSkills.Count == 0)//획득한 스킬이 없으면(캐릭터 고유스킬만 있으면) 나중에 1로 수정할것 
        {
            uiDatas = skillDatas.Where(data => data.isLock == 0).ToList();
            LoadSkill();
        }
        else
        {
            LoadSkill();
        }
    }
    public void LoadSkill()
    {
        //스킬 등장 범위 제한 - 액티브 10개, 최대레벨 5
        if (activeSkills.Count >= 10)
        {
            uiDatas = uiDatas.Where(data => data.isLock == 0
            && data.dungeonLevel < 6
            && (data.skill_type == "PassiveSkill" || activeSkills.Contains(data.name))
            ).ToList();
        }
        else
        {
            uiDatas = uiDatas.Where(data => data.isLock == 0 && data.dungeonLevel < 6).ToList();
        }
        ShuffleSkill();
    }
    //스킬 섞기
    private void ShuffleSkill()
    {
        for (int i = 0; i < uiDatas.Count * 10; i++)
        {
            int rand = UnityEngine.Random.Range(0, uiDatas.Count);
            var tempSkill = uiDatas[0];
            uiDatas[0] = uiDatas[rand];
            uiDatas[rand] = tempSkill;
        }
    }
    //던전 스킬 발동
    public void UseSkill()
    {
        //SkillUI에서 받은 문자열을 클래스타입으로 변환
        Type type = Type.GetType(skillName);

        //부모 클래스의 Type을 가져옴
        Type parentType = type.BaseType;

        //획득한 스킬 타입이 액티브라면
        if (parentType.Name == "ActiveSkill")
        {
            activeSkills.Add(type.Name);//액티브 스킬을 리스트에 추가.액티브 스킬을 10개까지 제한
            activeSkills = activeSkills.Distinct().ToList();
        }
        else
        {
            passiveSkills.Add(type.Name);//패시브 스킬을 리스트에 추가
            passiveSkills = passiveSkills.Distinct().ToList();
        }

        GameObject activatedGo = GameObject.Find(skillName);
        if (activatedGo == null) //새로 획득한 스킬이면
        {
            activatedGo = new GameObject(skillName);
            activatedGo.AddComponent(type);
            activatedGo.tag = "Skill";
            //시스템 레벨 불러오기
            //systemLevels = SystemSkillManager.Instance.GetSystemInfo()[0].systemLevel;
            //스킬 정보 추가
            dungeonSkills.Add(new SkillInfo(skillName, skillName, parentType.Name, 0,
                1, systemLevels[skillName], skillDescs[skillName]));
            //UI데이터 추가
            int uiIdx = uiDatas.FindIndex(data => data.name == skillName);
            uiDatas[uiIdx].dungeonLevel = 1;
        }
        else //이미 획득한 스킬이면
        {
            activatedGo.SetActive(false);
            activatedGo.SetActive(true);

            //획득한 스킬을 찾아 레벨업
            int skillIdx = dungeonSkills.FindIndex(data => data.name == skillName);
            dungeonSkills[skillIdx].dungeonLevel++;
            //UI로 반영할 스킬 데이터 레벨업
            int uiIdx = uiDatas.FindIndex(data => data.name == skillName);
            uiDatas[uiIdx].dungeonLevel++;
        }
    }
    //스킬 저장
    public void SaveSkill()
    {
        //던전 스킬을 저장
        //InfoManager.Instance.SaveInfo(dungeonSkills, "dungeonSkillInfo");
        //foreach (SkillInfo data in dungeonSkills)
        //{
        //    Debug.Log($"저장된 스킬 : {data.name}, type : {data.type}, level : {data.dungeonLevel}");
        //}
        //저장되어있던 파편 목록이 있다면 추가
        if (InfoManager.Instance.LoadInfo<List<SkillInfo>>("dungeonSkillsInfo"))
        {
            foreach (var memory in InfoManager.Instance.GetInfo<List<SkillInfo>>("dungeonSkillsInfo"))
            {
                memories.Add(memory);
            }
        }
        memories.Add(dungeonSkills);
        //파편 목록 저장
        InfoManager.Instance.SaveInfo(memories, "dungeonSkillsInfo");
    }
    //스킬 선택UI로 정보를 보냄
    public List<SkillData> LoadSkillUIData()
    {
        return uiDatas;
    }
    //스킬 선택창 활성화
    public void OpenSkillPopUp()
    {
        selectSkillPopUp.SetActive(true);
    }
    public string LevelToText(int level)
    {
        string text = "";
        switch (level)
        {
            case 1:
            text = "Ⅰ";
                break;
            case 2:
                text = "Ⅱ";
                break;
            case 3:
                text = "Ⅲ";
                break;
            case 4:
                text = "Ⅳ";
                break;
            case 5:
                text = "Ⅴ";
                break;
        }
        return text;
    }
}

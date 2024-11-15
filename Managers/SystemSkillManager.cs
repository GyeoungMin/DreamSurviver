using System.Collections.Generic;

public class SystemSkillManager
{
    private static SystemSkillManager _instance;

    public int id;
    public string name;
    public int isLock;
    public int systemLevel;
    List<SkillInfo> systemInfos = new List<SkillInfo>();
    public static SystemSkillManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SystemSkillManager();
            }
            return _instance;
        }
    }
    public SystemSkillManager()
    {
        LoadData();
        //GetSystemSkillInfo();
    }
    private void LoadData()
    {
        DataManager.Instance.LoadData<SkillData>();
        SkillData[] datas = DataManager.Instance.GetData<SkillData>();

        if (InfoManager.Instance.LoadInfo<SkillInfo>("systemSkillsInfo"))
        {
            systemInfos = InfoManager.Instance.GetInfo<SkillInfo>("systemSkillsInfo");
        }
        else
        {
            foreach (var data in datas)
            {
                systemInfos.Add(new SkillInfo(data.id, data.sprite_name, data.isLock, data.systemLevel, data.description));
            }
            InfoManager.Instance.SaveInfo(systemInfos, "systemSkillsInfo");
        }
    }
    public List<SkillInfo> GetSystemSkillInfo()
    {
        return systemInfos;
    }
    public void SetSystemSkillInfo(int id, string sprite_name, int isLock, int systemLevel, string description)
    {
        LoadData();
        systemInfos[id - 1000].id = id;
        systemInfos[id - 1000].sprite_name = sprite_name;
        systemInfos[id - 1000].isLock = isLock;
        systemInfos[id - 1000].systemLevel = systemLevel;
        systemInfos[id - 1000].description = description;

        InfoManager.Instance.SaveInfo(systemInfos, "systemSkillsInfo");
    }
}

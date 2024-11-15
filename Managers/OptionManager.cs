using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public Button BGMToggle;
    public Button SFXToggle;
    public Button effectToggle;
    public Button exitButton;
    public Button showSkillButton;
    public Button closeButton;
    public AudioMixer mixer;
    public SpriteAtlas skillAtlas;
    public GameObject skillSlot;
    public Transform grid;
    private bool BGMIsOn = false;
    private bool SFXIsOn = true;
    private bool effectIsOn = false;
    public DungeonSkillManager skillManager;

    List<SkillData> datas = new List<SkillData>();
    List<GameObject> skillSlots = new List<GameObject>();
    List<OptionInfo> optionInfos = new List<OptionInfo>();

    private void Start()
    {
        if (InfoManager.Instance.LoadInfo<OptionInfo>("optionInfo"))
        {
            OptionInfo option = InfoManager.Instance.GetInfo<OptionInfo>("optionInfo")[0];

            BGMIsOn = option.bgmOn;
            SFXIsOn = option.sfxOn;
            effectIsOn = option.effectOn;

            UpdateUI(BGMIsOn, SFXIsOn, effectIsOn);
        }

        BGMToggle.onClick.AddListener(() =>
        {
            BGMIsOn = !BGMIsOn;
            //Debug.Log($"배경음악 {BGMIsOn}");
            mixer.SetFloat("BGM", BGMIsOn ? 0 : -80);
            UpdateUI(BGMIsOn, SFXIsOn, effectIsOn);
        });
        SFXToggle.onClick.AddListener(() =>
        {
            SFXIsOn = !SFXIsOn;
            //Debug.Log($"효과음 {SFXIsOn}");
            mixer.SetFloat("SFX", SFXIsOn ? 0 : -80);
            UpdateUI(BGMIsOn, SFXIsOn, effectIsOn);
        });
        effectToggle.onClick.AddListener(() =>
        {
            effectIsOn = !effectIsOn;
            //Debug.Log($"FX {effectIsOn}");
            UpdateUI(BGMIsOn, SFXIsOn, effectIsOn);
        });
        exitButton.onClick.AddListener(() =>
        {
            skillManager.SaveSkill();
            SceneManager.LoadScene("AutoMap_WS");
        });
        showSkillButton.onClick.AddListener(() => { Debug.Log("스킬 상세 보기"); });
        closeButton.onClick.AddListener(() => { gameObject.SetActive(false); });
    }
    private void OnEnable()
    {
        LoadSkillData();

        Time.timeScale = 0;

        for (int i = 0; i < skillManager.dungeonSkills.Count; i++)
        {
            skillSlots.Add(Instantiate(skillSlot, grid));
        }
        UpdateUI();
    }
    private void OnDisable()
    {
        Time.timeScale = 1;
        for (int i = 0; i < skillManager.dungeonSkills.Count; i++)
        {
            Destroy(skillSlots[i]);
        }
        skillSlots.Clear();
        
        UpdateUI(BGMIsOn, SFXIsOn, effectIsOn);
        InfoManager.Instance.SaveInfo(optionInfos, "optionInfo");
    }
    private void LoadSkillData()
    {
        skillManager.LoadSkill();
        datas = skillManager.LoadSkillUIData();
    }
    //UI요소 반영
    private void UpdateUI()
    {
        for (int i = 0; i < skillManager.dungeonSkills.Count; i++)
        {
            //스킬 슬롯 아이콘
            int spriteIdx = datas.FindIndex(data => data.name == skillManager.dungeonSkills[i].name);
            skillSlots[i].GetComponent<Image>().sprite
                = skillAtlas.GetSprite(datas[spriteIdx].sprite_name);
        }
    }
    private void UpdateUI(bool bgm, bool sfx, bool effect)
    {
        optionInfos.Clear();
        optionInfos.Add(new OptionInfo(bgm, sfx, effect));

        BGMToggle.transform.GetChild(0).GetComponent<TMP_Text>().text = bgm ? "On" : "Off";
        SFXToggle.transform.GetChild(0).GetComponent<TMP_Text>().text = sfx ? "On" : "Off";
        effectToggle.transform.GetChild(0).GetComponent<TMP_Text>().text = effect ? "On" : "Off";

        Debug.Log($"bgm : {bgm}, sfx : {sfx}, effect : {effect}");
    }
}

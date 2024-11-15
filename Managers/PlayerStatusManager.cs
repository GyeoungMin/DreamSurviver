using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerStatusManager
{
    public static readonly PlayerStatusManager Instance = new PlayerStatusManager();

    // 생성자
    public PlayerStatusManager()
    {
        this.SetAllStatus();
    }
    
    List<MainPlayerStatusInfo> mainPlayerStatus = new List<MainPlayerStatusInfo>();
#region 각 스테이터스 변수 모음
    // 공격력
    public int powerLevel {get; set;} // 레벨
    public float powerFigure; // 현재 수치
    public int powerAmount{get; set;} // 현재 강화 퍼센트
    public float powerGoldMaxAmount; // 현재 단계에서 레벨 1을 올리기 위해 필요한 골드의 총량
    public List<float> powerGoldList{get; set;} // 각 레벨별 1%강화마다 투자해야 될 골드 수치 리스트

    public int damageLevel{get; set;} // 피해 증가량
    public float damageFigure;
    public int damageAmount{get; set;}
    public float damageGoldAmount;
    public float damageGoldMaxAmount;
    public List<float> damageGoldList{get; set;}

    public int maxHPLevel{get; set;} // 최대체력
    public float maxHPFigure;
    public int maxHPAmount{get; set;}
    public float maxHPGoldAmount;
    public float maxHPGoldMaxAmount;
    public List<float> maxHPGoldList{get; set;}

    public int recoveryLevel{get; set;} // 회복량
    public float recoveryFigure; 
    public int recoveryAmount{get; set;} 
    public float recoveryGoldAmount; 
    public float recoveryGoldMaxAmount; 
    public List<float> recoveryGoldList{get; set;}

    public int protectionLevel{get; set;} // 방어력
    public float protectionFigure; 
    public int protectionAmount{get; set;} 
    public float protectionGoldAmount; 
    public float protectionGoldMaxAmount; 
    public List<float> protectionGoldList{get; set;}
    
    public int protectionProbLevel{get; set;} // 방어 확률
    public float protectionProbFigure;
    public int protectionProbAmount{get; set;} 
    public float protectionProbGoldAmount; 
    public float protectionProbGoldMaxAmount; 
    public List<float> protectionProbGoldList{get; set;}

    public int criticalPowerLevel{get; set;} // 치명타 배율
    public float criticalPowerFigure;
    public int criticalPowerAmount{get; set;} 
    public float criticalPowerGoldAmount; 
    public float criticalPowerGoldMaxAmount; 
    public List<float> criticalPowerGoldList{get; set;}

    public int criticalProbLevel{get; set;} // 치명타 확률
    public float criticalProbFigure; 
    public int criticalProbAmount{get; set;} 
    public float criticalProbGoldAmount; 
    public float criticalProbGoldMaxAmount; 
    public List<float> criticalProbGoldList{get; set;}

    public int expIncreaseGainLevel{get; set;} // 획득경험치 증가
    public float expIncreaseGainFigure; 
    public int expIncreaseGainAmount{get; set;} 
    public float expIncreaseGainGoldAmount; 
    public float expIncreaseGainGoldMaxAmount; 
    public List<float> expIncreaseGainGoldList{get; set;}

    public int goldGainLevel{get; set;} // 획득 골드 증가
    public float goldGainFigure; 
    public int goldGainAmount{get; set;} 
    public float goldGainGoldAmount; 
    public float goldGainGoldMaxAmount; 
    public List<float> goldGainGoldList{get; set;}

    public int coolDownLevel{get; set;} // 쿨타임 감소
    public float coolDownFigure;
    public int coolDownAmount{get; set;} 
    public float coolDownGoldAmount;
    public float coolDownGoldMaxAmount;
    public List<float> coolDownGoldList{get; set;}

    public int attackRangeLevel{get; set;} // 공격범위
    public float attackRangeFigure; 
    public int attackRangeAmount{get; set;} 
    public float attackRangeGoldAmount;
    public float attackRangeGoldMaxAmount;
    public List<float> attackRangeGoldList{get; set;}

    public int attackDurationLevel{get; set;} // 공격지속시간(발판)
    public float attackDurationFigure; 
    public int attackDurationAmount{get; set;}
    public float attackDurationGoldAmount; 
    public float attackDurationGoldMaxAmount; 
    public List<float> attackDurationGoldList{get; set;}

    public int attackOfNumberLevel{get; set;} // 공격 개수 증가
    public int attackOfNumberFigure; 
    public int attackOfNumberAmount{get; set;} 
    public float attackOfNumberGoldAmount; 
    public float attackOfNumberGoldMaxAmount; 
    public List<float> attackOfNumberGoldList{get; set;}

    public int projectorSpeedLevel{get; set;} // 투사체 속도(투사체)
    public float projectorSpeedFigure; 
    public int projectorSpeedAmount{get; set;} 
    public float projectorSpeedGoldAmount;
    public float projectorSpeedGoldMaxAmount;
    public List<float> projectorSpeedGoldList{get; set;}
#endregion

#region  데이터 저장 및 불러오기 & 초기화
    // 인포 생성 & 불러오기
    public void LoadPlayerStatus(){
        if(!InfoManager.Instance.LoadInfo<MainPlayerStatusInfo>()){
            Debug.Log($"<color=Yellow>New Player</color>");
            Init();
            InfoManager.Instance.SaveInfo<MainPlayerStatusInfo>(mainPlayerStatus);
        }
        else
        {
            Debug.Log($"<color=Yellow>Wellcom Player</color>");
            mainPlayerStatus = InfoManager.Instance.GetInfo<MainPlayerStatusInfo>();
            LoadStatusInfo(mainPlayerStatus[0]);
        }
    }

    // 신규 유저 초기화
    public void Init()
    {
        mainPlayerStatus = InfoManager.Instance.GetInfo<MainPlayerStatusInfo>();
        MainPlayerStatusInfo newPlayerStatus = new MainPlayerStatusInfo(1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,0);
        mainPlayerStatus.Add(newPlayerStatus);
        LoadStatusInfo(mainPlayerStatus[0]);
    }

    // 불러온 데이터를 매니저에 넣기
    private void LoadStatusInfo(MainPlayerStatusInfo info)
    {
        this.powerLevel = info.powerLevel;
        this.powerAmount = info.powerAmount;

        this.damageLevel = info.damageLevel;
        this.damageAmount = info.damageAmount;

        this.maxHPLevel = info.maxHPLevel; // 최대체력
        this.maxHPAmount = info.maxHPAmount;// 최대체력

        this.recoveryLevel = info.recoveryLevel; // 회복량
        this.recoveryAmount = info.recoveryAmount; // 회복량

        this.protectionLevel = info.protectionLevel; // 방어력
        this.protectionAmount = info.protectionAmount; // 방어력
        
        this.protectionProbLevel = info.protectionProbLevel; // 방어 확률
        this.protectionProbAmount = info.protectionProbAmount; // 방어 확률

        this.criticalPowerLevel = info.criticalPowerLevel; // 치명타 배율
        this.criticalPowerAmount = info.criticalPowerAmount; // 치명타 배율

        this.criticalProbLevel = info.criticalProbLevel; // 치명타 확률
        this.criticalProbAmount = info.criticalProbAmount; // 치명타 확률

        this.expIncreaseGainLevel = info.expIncreaseGainLevel; // 획득경험치 증가
        this.expIncreaseGainAmount = info.expIncreaseGainAmount; // 획득경험치 증가

        this.goldGainLevel = info.goldGainLevel; // 획득 골드 증가
        this.goldGainAmount = info.goldGainAmount; // 획득 골드 증가

        this.coolDownLevel = info.coolDownLevel; // 쿨타임 감소
        this.coolDownAmount = info.coolDownAmount; // 쿨타임 감소

        this.attackRangeLevel = info.attackRangeLevel; // 공격범위
        this.attackRangeAmount = info.attackRangeAmount; // 공격범위

        this.attackDurationLevel = info.attackDurationLevel; // 공격지속시간(발판)
        this.attackDurationAmount = info.attackDurationAmount; // 공격지속시간(발판)

        this.attackOfNumberLevel = info.attackOfNumberLevel; // 공격 개수 증가
        this.attackOfNumberAmount = info.attackOfNumberAmount; // 공격 개수 증가

        this.projectorSpeedLevel = info.projectorSpeedLevel; // 투사체 속도(투사체)
        this.projectorSpeedAmount = info.projectorSpeedAmount; // 투사체 속도(투사체)
    }

    // 인포 저장
    public void SaveStatus()
    {
        Debug.Log($"<color=Yellow>스테이터스 파워 레벨</color> >>> {this.powerLevel}");
        mainPlayerStatus[0] = new MainPlayerStatusInfo(powerLevel, powerAmount,damageLevel,damageAmount,maxHPLevel,maxHPAmount,
        recoveryLevel,recoveryAmount,protectionLevel, protectionAmount, protectionProbLevel, protectionProbAmount, criticalPowerLevel, criticalPowerAmount,
        criticalProbLevel, criticalProbAmount, expIncreaseGainLevel, expIncreaseGainAmount, goldGainLevel, goldGainAmount, coolDownLevel, coolDownAmount,
        attackRangeLevel, attackRangeAmount, attackDurationLevel, attackDurationAmount, attackOfNumberLevel, attackOfNumberAmount, projectorSpeedLevel,
        projectorSpeedAmount );
        
        InfoManager.Instance.SaveInfo<MainPlayerStatusInfo>(mainPlayerStatus);
    }

#endregion
    
#region  스테이터스 셋업 메서드
    public void SetAllStatus()
    {
        UIFunctionClass.Instance.LevelToStatus(powerLevel, out powerFigure, 0f, 10.0f, out powerGoldMaxAmount, 100000.0f, 50000.0f);
        UIFunctionClass.Instance.LevelToStatus(damageLevel, out damageFigure, 0f, 0.01f, out damageGoldMaxAmount, 50000f, 1000f);
    }
#endregion
    
}

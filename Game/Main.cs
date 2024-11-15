using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : SingletonManager<Main>
{
#region  메인에서 저장이 필요한 변수

    // 플레이어 고유 UID
    private int iPlayerUID;
    // 플레이어 디폴트 이름
    private string strPlayerName = "Dreamer";

    // 재화 변수 및 프로퍼티
    private float fPossessionGold = 100001; // 골드 현재량 //! 기본값은 신규/기존 판단해서 초기화
    public float PossessionGold{ get{ return this.fPossessionGold; }}
    private float fPossessionRune = 50; // 룬 현재량 //! 기본값은 신규/기존 판단해서 초기화
    public float PossessionRune{get{ return this.fPossessionRune; }} // 룬 현재량
#endregion

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

#region  재화 증감 메서드

    //? 재화 증가
    // 골드
    public void AnIncreaseInGold(float fGoldIncrease)
    {  
        this.fPossessionGold += fGoldIncrease;
    }
    // 룬
    public void AnIncreaseInRune(float fRuneIncrease)
    {
        this.fPossessionRune += fRuneIncrease;
    }


    //! 재화 감소
    // 골드
    public void AnDecreaseInGold(float fGoldDecrease)
    {
        this.fPossessionGold -= fGoldDecrease;
    }
    // 룬
    public void AnDecreaseInRune(float fRuneDecrease)
    {
        this.fPossessionRune -= fRuneDecrease;
    }

#endregion





}

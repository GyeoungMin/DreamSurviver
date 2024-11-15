using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameType { None = -1, Main, Dungeon }
public class GameManager : SingletonManager<GameManager>
{
    [SerializeField] GameType gameType = GameType.None;
    public GameType GameType { get { return gameType; } }

    void Start()
    {

    }

    void Update()
    {

    }
}

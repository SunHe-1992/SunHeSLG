using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UniFramework.Pooling;
using UniFramework.Singleton;



public class MonopolyService : ISingleton

{
    public static MonopolyService Inst;
    public static void Init()
    {
        Inst = UniSingleton.CreateSingleton<MonopolyService>();
    }

    public void OnCreate(object createParam)
    {
        Inst = this;
    }

    public void OnDestroy()
    {
        Inst = null;
    }

    public void OnUpdate()
    {
    }

    #region module: bank heist

    public GameBankHeist gameBankHeist { get; set; }
    public void SetUpBankHeistData()
    {
        gameBankHeist = new GameBankHeist();
        gameBankHeist.RandomData();
    }
    public class GameBankHeist
    {
        //each slot has a tokenId
        public List<int> tokenList;
        public Dictionary<int, long> rewardDic;
        public void RandomData()
        {
            tokenList = new List<int>();
            for (int i = 0; i < 7; i++)
            {
                tokenList.Add(GetRandomToken());
            }

            rewardDic = new Dictionary<int, long>() {
                {0, 10000},
                {1, 20000},
                {2, 50000},
            };
        }
        int GetRandomToken()
        {
            return Random.Range(1, 3);
        }



    }
    #endregion

}

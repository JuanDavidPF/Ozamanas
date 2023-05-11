using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using JuanPayan.Helpers;


namespace Ozamanas.Board
{
    public class BossSpawner : Cell
    {
       [SerializeField] private int inWaveNumber;

       [SerializeField] private bool inLastWave;

       [SerializeField] private CellTopElement boss;

       private CinematicIntro cinematicIntro;





       private int waves = 0;

       private bool bossSpawned = false;

       protected override void Awake()
       {
         base.Awake();
         cinematicIntro.GetComponent<CinematicIntro>();

       }

       public void OnNewWave()
       {
          
            if(inLastWave) return;
            waves++;
            if(inWaveNumber == waves) SpawnBoss();
       }

       public void OnLastWave()
       {
            SpawnBoss();
       }

       private void SpawnBoss()
       {
            if(bossSpawned) return;
            bossSpawned = true;
            CurrentTopElement = boss;
            cinematicIntro.Behaviour();
       }

        

    }
}

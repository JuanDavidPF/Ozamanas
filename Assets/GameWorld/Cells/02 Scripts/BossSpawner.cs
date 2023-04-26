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

       [SerializeField] private float focusSpeed = 0.2f;

       private Tween anchorTween;

       private CameraAnchor cameraAnchor;

       private int waves = 0;

       private bool bossSpawned = false;

       protected override void Awake()
       {
         base.Awake();
          cameraAnchor = FindObjectOfType<CameraAnchor>();

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
            anchorTween = cameraAnchor.transform.DOMove(transform.position,focusSpeed,false);
       }

        

    }
}

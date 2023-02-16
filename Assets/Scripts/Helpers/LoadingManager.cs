using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

namespace JuanPayan.Helpers
{
    public class LoadingManager : MyGenericSingleton<LoadingManager>
{

   

  [SerializeField]
    private GameObject loadingScreen;

    [SerializeField] private Image backgroundImage;

    [SerializeField] private Image progressBar;

    [SerializeField]
    private List<Sprite> wallpapers = new List<Sprite>();

    private float loadProgress = 0;
    private static List<AsyncOperation> loadingQueue = new List<AsyncOperation>();
    public static Action<AsyncOperation> OnAddAsyncOperation;

    protected override void Awake()
    {
        base.Awake();
        TurnLoadScreen(false);
    }//Closes Awake method

    public static void AddAsyncOperation(AsyncOperation operation)
    {
        OnAddAsyncOperation?.Invoke(operation);
    }//Closes AddAsyncOperation method

    private void RunAsyncOperation(AsyncOperation operation)
    {
        if (loadingQueue.Contains(operation)) return;
        StopAllCoroutines();
        loadingQueue.Add(operation);
        StartCoroutine(HandleLoadingProcess());
    }

    private IEnumerator HandleLoadingProcess()
    {
        progressBar.fillAmount = 0;


        if (!loadingScreen.activeSelf) ChooseWallpaper();
        TurnLoadScreen(true);
        foreach (AsyncOperation operation in loadingQueue)
        {
            while (!operation.isDone)
            {
                loadProgress = 0;
                foreach (AsyncOperation operationTwo in loadingQueue)
                {
                    loadProgress += operationTwo.progress;
                }
                loadProgress = loadProgress / loadingQueue.Count;
                progressBar.fillAmount = loadProgress;
                yield return null;
            }
        }

        progressBar.fillAmount = 1;
        loadingQueue.Clear();
        TurnLoadScreen(false);
    }

    public void TurnLoadScreen(bool open)
    {
        if (loadingScreen) loadingScreen.SetActive(open);
        else Debug.Log("No loading screen referenced");
    }

    private void OnEnable()
    {
        OnAddAsyncOperation += RunAsyncOperation;
    }//Closes OnEnable method

    private void OnDisable()
    {
        OnAddAsyncOperation -= RunAsyncOperation;
    }//Closes OnDisable method

    private void ChooseWallpaper()
    {
        if (!backgroundImage || wallpapers.Count == 0) return;
        Sprite wallpaper = wallpapers[UnityEngine.Random.Range(0, wallpapers.Count)];
        backgroundImage.sprite = wallpaper;
        backgroundImage.color = wallpaper ? Color.white : Color.magenta;
    }//Closes ChooseWallpaper method
   
}
}

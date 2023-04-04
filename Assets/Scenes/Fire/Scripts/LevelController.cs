using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Ozamanas.Levels;
using System;
using PathCreation;
using Ozamanas.Tags;
using DG.Tweening;

namespace Ozamanas.GameScenes
{
    public class LevelController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
    
        [Space(15)]
        [Header("Level Settings")]

        private Animator animator;

        private LevelManager levelManager;
        private PlayerController player;
        [SerializeField] private LevelData levelData;
        [SerializeField] private List<LevelController> nextLevels;
        [SerializeField] private List<Vector3> Offset = new List<Vector3>();
        [SerializeField] private Transform camAnchor;
        public Dictionary<LevelController, BezierPath> paths = new Dictionary<LevelController, BezierPath>();

        [Space(15)]
        [Header("Level UI")]
        [SerializeField] private SpriteRenderer levelIcon;

        [SerializeField] private SpriteRenderer levelBorder;

        [SerializeField] private Color levelBlocked;

        [SerializeField] private Color levelFinished;

        [SerializeField] private Color levelPlayable;

        [Space(15)]
        [Header("Path Settings")]
        [SerializeField] private GameObject prefab;
        [SerializeField] private GameObject holder;
        [SerializeField] private float spacing = 3;
        const float minSpacing = .1f;
        public LevelData LevelData { get => levelData; set => levelData = value; }
        public Transform CamAnchor { get => camAnchor; set => camAnchor = value; }



        void Awake()
        {
            animator = gameObject.GetComponent<Animator>();
             player= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
             levelManager = GetComponentInParent<LevelManager>();
        }
        void Start()
        {
            SetUpLevelColor();

            if (nextLevels.Count <= 0) return;

            foreach(LevelController level in nextLevels)
            {
                SetUPNewPath(level);
                UnLockNextLevels(level);
            } 
            
        }

        private void SetUpLevelColor()
        {
            switch(levelData.state)
            {
                case LevelState.Blocked:
                levelIcon.color = levelBlocked;
                levelBorder.color = levelBlocked;
                break;
                case LevelState.Finished:
                levelIcon.color = levelFinished;
                levelBorder.color = levelFinished;
                break;
                case LevelState.Playable:
                levelIcon.color = levelPlayable;
                levelBorder.color = levelPlayable;
                break;
            }

            levelIcon.sprite = levelData.levelIcon;
        }


        private void UnLockNextLevels(LevelController level)
        {
            if(levelData.state != LevelState.Finished) return;

            if(level.levelData.state != LevelState.Blocked) return;

            level.levelData.state = LevelState.Playable;

            level.SetUpLevelColor();

        }
        private void SetUPNewPath(LevelController level)
        {
            List<Vector3> waypoints = new List<Vector3>();
            
            waypoints.Add(transform.position);

            Vector3 middlePoint =  Vector3.Lerp(transform.position, level.gameObject.transform.position, 0.5f);

            middlePoint += Offset[UnityEngine.Random.Range(0,Offset.Count)];

            waypoints.Add(middlePoint);

            waypoints.Add(level.gameObject.transform.position);

            BezierPath bezierPath = new BezierPath (waypoints.ToArray(), false, PathSpace.xz);

            paths.Add(level,bezierPath);

            Generate(bezierPath);
        }

         private void Generate(BezierPath bezierPath) 
        {
            if ( prefab != null && holder != null) 
            {

                VertexPath path = new VertexPath(bezierPath,transform.transform);

                spacing = Mathf.Max(minSpacing, spacing);
                float dst = 0;

                while (dst < path.length) {
                    Vector3 point = path.GetPointAtDistance(dst) - transform.position;
                    Quaternion rot = path.GetRotationAtDistance (dst);
                    Instantiate (prefab, point, rot, holder.transform);
                    dst += spacing;
                }
            }
        }

        public List<Vector3> GetPathFromLevel(LevelController level)
        {
            BezierPath bezierPath;
            VertexPath path;
            Vector3 point = new Vector3(0,0,0);
           
            List<Vector3> points = new List<Vector3>();

            if(!paths.TryGetValue(level, out bezierPath )) return points;

            path = new VertexPath(bezierPath,holder.transform);

            point = path.GetPointAtTime(0.33f) - transform.position;
            points.Add(point);

            point = path.GetPointAtTime(0.66f) - transform.position;
            points.Add(point);

            point = path.GetPointAtTime(0.99f) - transform.position + player.playerOffset;
            points.Add(point);
          
            return points;
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            animator.SetTrigger("Selected");

            if(player.PlayerState == PlayerState.Running) return;

            if(levelManager.CurrentLevel == this) return;

            player.transform.DOLookAt(transform.position + player.playerOffset,0.5f);

        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            animator.SetTrigger("UnSelected");
        }

         void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if(levelData.state == LevelState.Blocked) return;

            if (player.PlayerState == PlayerState.Running) return;

              levelManager.ClearUI();

            if(levelManager.CurrentLevel == this ) 
            {
                levelManager.SetUPToPLayLevel();
                return;
            }

            if(!levelManager.CurrentLevel.nextLevels.Contains(this)) return;

            player.MoveToDestination(transform.position + player.playerOffset, levelManager.CurrentLevel.GetPathFromLevel(this));

            levelManager.CurrentLevel = this;

        }
    }
}

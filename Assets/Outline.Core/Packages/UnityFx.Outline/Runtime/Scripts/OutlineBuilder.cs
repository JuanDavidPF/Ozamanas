// Copyright (C) 2019-2021 Alexander Bogarsukov. All rights reserved.
// See the LICENSE.md file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFx.Outline
{
    /// <summary>
    /// A helper behaviour for managing content of <see cref="OutlineLayerCollection"/> via Unity Editor.
    /// </summary>
    public sealed class OutlineBuilder : MonoBehaviour
    {


        private static OutlineBuilder reference;


        #region data

        [Serializable]
        internal class ContentItem
        {
            public ContentItem()
            {

            }
            public ContentItem(int LayerIndex, GameObject Go)
            {
                this.LayerIndex = LayerIndex;
                this.Go = Go;
            }
            public GameObject Go;
            public int LayerIndex;
        }

#pragma warning disable 0649

        [SerializeField, Tooltip(OutlineResources.OutlineLayerCollectionTooltip)]
        private OutlineLayerCollection _outlineLayers;
        [SerializeField, HideInInspector]
        private List<ContentItem> _content;

#pragma warning restore 0649

        #endregion

        #region interface

        internal List<ContentItem> Content { get => _content; set => _content = value; }

        /// <summary>
        /// Gets or sets a collection of layers to manage.
        /// </summary>
        public OutlineLayerCollection OutlineLayers { get => _outlineLayers; set => _outlineLayers = value; }

        /// <summary>
        /// Clears content of all layers.
        /// </summary>
        /// <seealso cref="OutlineLayers"/>
        public void Clear()
        {
            _outlineLayers?.ClearLayerContent();
        }
        public static void AddToLayer(int index, GameObject go)
        {
            if (!reference) return;
            if (index < 0 || index >= reference._outlineLayers.Count || !go) return;


            reference._outlineLayers.GetOrAddLayer(index).Add(go);

        }//Closes 

        public static void AddToLayer(int index, List<GameObject> gameobjects)
        {
            if (!reference) return;
            if (index < 0 || index >= reference._outlineLayers.Count) return;

            foreach (var go in gameobjects)
            {
                if (!go) continue;
                reference._outlineLayers.GetOrAddLayer(index).Add(go);
            }

        }//Closes 

        public static void Remove(GameObject go)
        {
            if (!reference) return;
            if (!go) return;
            reference._outlineLayers.Remove(go);
        }
        public static void Remove(List<GameObject> gameobjects)
        {
            if (!reference) return;
            foreach (var go in gameobjects)
            {
                if (!go) continue;
                reference._outlineLayers.Remove(go);
            }
        }

        public static void Remove<T>(List<T> components) where T : Component
        {
            if (!reference) return;
            foreach (var go in components)
            {
                if (!go) continue;
                reference._outlineLayers.Remove(go.gameObject);
            }
        }



        public static void Remove(int index, GameObject go)
        {
            if (!reference) return;
            if (index < 0 || index >= reference._outlineLayers.Count) return;
            if (!go) return;

            reference._outlineLayers.GetOrAddLayer(index).Remove(go);
        }
        public static void Remove(int index, List<GameObject> gameobjects)
        {
            if (!reference) return;
            if (index < 0 || index >= reference._outlineLayers.Count) return;

            foreach (var go in gameobjects)
            {
                if (!go) continue;
                reference._outlineLayers.GetOrAddLayer(index).Remove(go);
            }
        }

        public static void Remove<T>(int index, List<T> components) where T : Component
        {
            if (!reference) return;
            foreach (var go in components)
            {
                if (!go) continue;
                reference._outlineLayers.GetOrAddLayer(index).Remove(go.gameObject);
            }
        }



        #endregion

        #region MonoBehaviour

        private void OnEnable()
        {
            reference = this;
            if (_outlineLayers && _content != null)
            {
                foreach (var item in _content)
                {
                    if (item.LayerIndex >= 0 && item.LayerIndex < _outlineLayers.Count && item.Go)
                    {
                        _outlineLayers.GetOrAddLayer(item.LayerIndex).Add(item.Go);
                    }
                }
            }
        }

#if UNITY_EDITOR

        private void Reset()
        {
            var effect = GetComponent<OutlineEffect>();

            if (effect)
            {
                _outlineLayers = effect.OutlineLayersInternal;
            }
        }

        private void OnDestroy()
        {
            reference = null;
            _outlineLayers?.ClearLayerContent();
        }

#endif

        #endregion
    }





}

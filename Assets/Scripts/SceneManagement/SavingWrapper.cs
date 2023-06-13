using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.ReorderableList;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        [SerializeField] float fadeInTimeAtBeginningOfLoad = 0.2f;

        IEnumerator Start()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(fadeInTimeAtBeginningOfLoad);
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                Save();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load()
        {   // call to the saving system
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
    }
}

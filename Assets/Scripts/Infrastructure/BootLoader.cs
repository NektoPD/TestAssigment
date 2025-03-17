using System.Collections;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure
{
    public class BootLoader : MonoBehaviour
    {
        private const string GameSceneName = "GameScene";
        private const float LoadingProgressThreshold = 0.9f;

        private PlaceableItemsSaver _itemsSaver;

        [Inject]
        public void Construct(PlaceableItemsSaver itemsSaver)
        {
            _itemsSaver = itemsSaver;
        }

        private void Start()
        {
            StartCoroutine(LoadGameRoutine());
        }

        private IEnumerator LoadGameRoutine()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(GameSceneName, LoadSceneMode.Single);
            asyncLoad.allowSceneActivation = false;

            _itemsSaver.Initialize();

            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= LoadingProgressThreshold)
                {
                    asyncLoad.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}
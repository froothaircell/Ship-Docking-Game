using CoreResources.Utils.Singletons;
using UnityEngine;

namespace CoreResources.Utils.SaveData
{
    public class PlayerPrefsManager : InitializableGenericSingleton<PlayerPrefsManager>
    {
        protected override void InitSingleton()
        {
            
        }

        protected override void CleanSingleton()
        {
            
        }

        public void GetPlayerInfo(ref int score, ref int level)
        {
            GetPlayerScore(ref score);
            GetPlayerLevel(ref level);
        }

        public void GetPlayerScore(ref int score)
        {
            if(!PlayerPrefs.HasKey(nameof(score)))
            {
                PlayerPrefs.SetInt(nameof(score), 0);
            }
            
            score = PlayerPrefs.GetInt(nameof(score));
        }

        public void GetPlayerLevel(ref int level)
        {
            if(!PlayerPrefs.HasKey(nameof(level)))
            {
                PlayerPrefs.SetInt(nameof(level), 0);
            }

            level = PlayerPrefs.GetInt(nameof(level));
        }

        public void SetPlayerInfo(int score, int level)
        {
            PlayerPrefs.SetInt(nameof(score), score);
            PlayerPrefs.SetInt(nameof(level), level);
        }

        public void SetPlayerInputPrefs(float touchRadius)
        {
            if (touchRadius >= 0f)
            {
                PlayerPrefs.SetFloat(nameof(touchRadius), touchRadius);
            }
        }

        public void GetPlayerInputPrefs(ref float touchRadius)
        {
            if (!PlayerPrefs.HasKey(nameof(touchRadius)))
            {
                PlayerPrefs.SetFloat(nameof(touchRadius), 3f);
            }

            touchRadius = PlayerPrefs.GetFloat(nameof(touchRadius));
        }

        public void ResetPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
using CoreResources.Utils.Singletons;
using UnityEngine;

namespace CoreResources.Utils.SaveData
{
    public class PlayerPrefsManager : GenericSingleton<PlayerPrefsManager>
    {
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

        public void ResetPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
using System.Collections.Generic;
using EnemySystem.Data.Enums;
using UnityEngine;

namespace EnemySystem.Data.Configs
{
    [CreateAssetMenu(menuName = "EnemySystem/ConfigTable")]
    public class EnemyConfigTable : ScriptableObject
    {
        public List<EnemyConfig> configs = new List<EnemyConfig>();
        
        public EnemyConfig GetConfig(EnemyType type)
        {
            return configs.Find(c => c.type == type);
        }
    }
}
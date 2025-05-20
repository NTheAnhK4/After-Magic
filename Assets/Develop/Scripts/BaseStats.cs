using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Develop
{
    [CreateAssetMenu(fileName = "BaseStats", menuName = "Stats/BaseStats")]
    public class BaseStats : ScriptableObject
    {
        public int attack = 10;
        public int defense = 20;
    }

}

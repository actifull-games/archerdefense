using MobileFramework.Game;
using MobileFramework.Parameters;
using UnityEngine;

namespace Characters
{
    public interface ITargetSelector : ITypeSelectable
    {
        GameObject FindNewTarget(GameRules gameRules);
    }
}
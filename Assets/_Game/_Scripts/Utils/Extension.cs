using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using DG.Tweening;
using UnityEngine;

namespace CardGame
{
    public static class Extension
    {

        public static void DelayCallback(this UIParticle uIParticle, float time, System.Action<UIParticle> beforeDisable = null)
        {
            DOVirtual.DelayedCall(time, () =>
            {
                beforeDisable?.Invoke(uIParticle);
            });
        }
    }
}

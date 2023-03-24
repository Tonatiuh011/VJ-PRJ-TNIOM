using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class Utils
    {
        public static float GetClipLength(Animator animator, string animName)
        {
            var clip = animator
            .runtimeAnimatorController
            .animationClips
            .ToList()
            .Find(x => {
                return x.name == animName;
            });

            return clip.length;
        }

        public static IEnumerator Delay(float time, Action cbBefore = null, Action cbAfter = null)
        {
            cbBefore?.Invoke();
            yield return new WaitForSeconds(time);
            cbAfter?.Invoke();
        }
    }
}

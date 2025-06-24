using System;
using System.Collections;
using System.Collections.Generic;
using AudioSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace Develop
{
    public class Test : ComponentBehavior
    {
        public SoundData data;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) SoundManager.Instance.CreateSound().WithSoundData(data).WithRandomPitch().Play();
        }
    }

}

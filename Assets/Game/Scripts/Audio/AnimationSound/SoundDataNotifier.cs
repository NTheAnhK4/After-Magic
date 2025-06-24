using System.Collections;
using System.Collections.Generic;
using AudioSystem;
using UnityEngine;

public class SoundDataNotifier : StateMachineBehaviour
{
    [SerializeField] private SoundData data;
    [SerializeField] private bool randomPitch = true;
    [SerializeField] private bool PlaySoundWhenEnter;

    private SoundReceiver soundReceiver;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (data == null) return;

        soundReceiver = animator.gameObject.GetOrAdd<SoundReceiver>();
        
        soundReceiver.OrNull()?.SetSoundData(data, randomPitch);
        if(PlaySoundWhenEnter) soundReceiver.OrNull()?.PlaySound();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        if(data.Loop && soundReceiver != null) soundReceiver.StopSound();
    }
}

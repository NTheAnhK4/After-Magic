
using AudioSystem;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundEffect : ComponentBehavior
{
    [SerializeField] private SoundData soundData;
    [SerializeField] private ButtonAnimBase btnTarget;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (btnTarget == null) btnTarget = GetComponent<ButtonAnimBase>();
    }

    private void OnEnable()
    {
        btnTarget.OrNull().beforeClick += PlaySound;
    }

    private void OnDisable()
    {
        btnTarget.OrNull().beforeClick -= PlaySound;
    }

    private void PlaySound()
    {
        if (soundData == null || SoundManager.Instance == null) return;
        SoundManager.Instance.CreateSound().WithSoundData(soundData).WithPosition(transform.position).WithRandomPitch().Play();
    }
}

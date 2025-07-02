using UnityEngine;
using UnityEngine.U2D.Animation;


public class AutoBoneReset : ComponentBehaviour
{
    [SerializeField] private SpriteSkin spriteSkin;
    [SerializeField] private Transform[] bones;
    [SerializeField] private Vector3[] originalPositions;
    [SerializeField] private Quaternion[] originalRotations;

   

    public override void LoadComponent()
    {
        base.LoadComponent();
        spriteSkin = GetComponent<SpriteSkin>();
        if (spriteSkin == null || spriteSkin.boneTransforms == null)
            return;

        bones = spriteSkin.boneTransforms;
        originalPositions = new Vector3[bones.Length];
        originalRotations = new Quaternion[bones.Length];

        for (int i = 0; i < bones.Length; i++)
        {
            originalPositions[i] = bones[i].localPosition;
            originalRotations[i] = bones[i].localRotation;
        }
    }

    void OnEnable()
    {
        if (bones == null || originalPositions == null) return;

        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].localPosition = originalPositions[i];
            bones[i].localRotation = originalRotations[i];
        }

        // Tắt và bật lại SpriteSkin để đảm bảo cập nhật lại lưới
        if (spriteSkin != null)
        {
            spriteSkin.enabled = false;
            spriteSkin.enabled = true;
        }
    }
}
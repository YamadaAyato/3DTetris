using DG.Tweening;
using UnityEngine;

namespace Mock
{
    public class CubeRoll : MonoBehaviour
    {
        [SerializeField] private float _rotateDuration;
        [SerializeField] private Ease _rotateEase;

        private Tween _rotateTween;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Rotate(Vector3.right);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Rotate(Vector3.left);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                Rotate(Vector3.up);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Rotate(Vector3.down);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                Rotate(Vector3.forward);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                Rotate(Vector3.back);
            }
        }

        private void Rotate(Vector3 axis)
        {
            if (_rotateTween != null || _rotateTween.IsActive())
            {
                return;
            }

            Quaternion taragetRotation = Quaternion.AngleAxis(90f, axis) * transform.rotation;

            _rotateTween = transform
                .DORotateQuaternion(taragetRotation, _rotateDuration)
                .SetEase(_rotateEase)
                .OnComplete(() => _rotateTween = null);
        }
    }
}

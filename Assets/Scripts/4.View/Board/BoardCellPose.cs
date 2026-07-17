using UnityEngine;

namespace ThreeDTetris.View
{
    public readonly struct BoardCellPose
    {
        public BoardCellPose(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public Vector3 Position { get; }

        public Quaternion Rotation { get; }
    }
}

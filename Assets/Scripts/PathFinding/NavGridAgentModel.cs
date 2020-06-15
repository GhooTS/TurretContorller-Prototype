using UnityEngine;

namespace Nav2D
{
    /// <summary>
    /// Describes the common characteristics of agent
    /// </summary>
    [System.Serializable]
    public struct NavGridAgentModel
    {
        [Min(1)]
        public int size;//in grid map cells
        [Min(1)]
        public int height;// in grid map cells
        [Min(0)]
        public int maxJumpHeight;
        public bool canFly;

        public NavGridAgentModel(int height = 1, int size = 1, int maxJumpHeight = 3, bool canFly = false)
        {
            this.height = height;
            this.maxJumpHeight = maxJumpHeight;
            this.size = size;
            this.canFly = canFly;
        }
    }
}
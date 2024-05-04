using UnityEngine;

public static class GameConstant
{
    public const string Death = "Death";

    // Quest System
    public const string IsQuestActive = "IsQuestActive";

    public const string IsQuestComplete = "IsQuestComplete";

    //Player Animation
    public static int JumpingUp = Animator.StringToHash("Jumping Up");

    public static int JumpingDown = Animator.StringToHash("Jumping Down");
    public static int Speed = Animator.StringToHash("Speed");
}
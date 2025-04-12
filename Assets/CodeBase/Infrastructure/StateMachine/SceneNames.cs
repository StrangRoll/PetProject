namespace CodeBase.Infrastructure.StateMachine
{
    public static class SceneNames
    {
        public const string InitScene = "InitScene";
        public const string Level = "Level";
        public const string Dungeon = "Dungeon";

        public static string FromEnum(SceneNamesEnum sceneName)
        {
            switch (sceneName)
            {
                case SceneNamesEnum.InitScene 
                    : return InitScene;
                case SceneNamesEnum.Level
                    : return Level;
                case SceneNamesEnum.Dungeon
                    : return Dungeon;
                default:
                    return null;
            }
        } 
    }
    
    public enum SceneNamesEnum
    {
        InitScene,
        Level,
        Dungeon
    }
}
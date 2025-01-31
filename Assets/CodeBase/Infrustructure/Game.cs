using CodeBase.Services.Input;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static IInputService InputService;

    public Game()
    {
        RegisterInputService();
    }

    private void RegisterInputService()
    {
        if (Application.isEditor)
            InputService = new StandaloneInputService();
        else
            InputService = new MobileInputService();
    }
}

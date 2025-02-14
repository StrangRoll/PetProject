using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField]
        private GameBootstraper _gameBootstraper;
        
        private void Awake()
        {
            var bootstrapper = FindObjectOfType<GameBootstraper>();

            if (bootstrapper == null)
                Instantiate(_gameBootstraper);
        }
    }
}
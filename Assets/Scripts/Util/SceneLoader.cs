using UnityEngine.SceneManagement;

namespace Util
{
    public static class SceneLoader
    {
        public static void LoadSceneByName(string name)
        {
            SceneManager.LoadScene(name);
        }
    }
}

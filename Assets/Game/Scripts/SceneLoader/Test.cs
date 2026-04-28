using Game.SceneLoaderSystem;
using System.Threading.Tasks;
using UnityEngine;

public class Test : MonoBehaviour
{
    public async Task Start()
    {
        await Task.Delay(5000);

        Debug.Log("INVOKE LOAD SCENE");

        SceneLoader.Instance.LoadScene(SceneEnum.MapLevel);
    }
}

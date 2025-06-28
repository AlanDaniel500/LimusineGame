using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
    public void IrAlMenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void IrAVictoria()
    {
        SceneManager.LoadScene("Victoria");
    }

    public void IrADerrota()
    {
        SceneManager.LoadScene("Derrota");
    }

    public void FinishLevel()
    {
        SceneManager.LoadScene("UpgradeScene");
    }

}

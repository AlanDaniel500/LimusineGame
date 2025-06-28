using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeSceneManager : MonoBehaviour
{
    [SerializeField] private TMP_Text puntosTexto;
    [SerializeField] private TMP_Text mejoraTexto;

    private ABBVelocidad abb;

    private void Start()
    {
        int puntos = GameSessionManager.Instance.Entregados;

        puntosTexto.text = $"¡Has ganado {puntos} puntos!";

        abb = new ABBVelocidad();
        abb.Insertar(1, 1.1f);
        abb.Insertar(3, 1.3f);
        abb.Insertar(5, 1.5f);
        abb.Insertar(7, 1.7f);
        abb.Insertar(10, 2.0f);

        float mejora = abb.BuscarMejora(puntos);

        PlayerSpeedManager.Instance.SetSpeedMultiplier(mejora);

        mejoraTexto.text = $"Velocidad mejorada a x{mejora}";
    }

    public void IrANivel2()
    {
        SceneManager.LoadScene("SampleScene2");
    }
}


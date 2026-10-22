using UnityEngine;

// Oculta el objeto en la versión para navegador (por ejemplo "salir": en la web no se puede cerrar el juego).
public class HideOnWebGL : MonoBehaviour
{
    private void Awake()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        gameObject.SetActive(false);
#endif
    }
}

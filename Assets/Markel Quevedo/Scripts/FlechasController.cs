using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FlechasController : MonoBehaviour
{
    [System.Serializable]
    public class FlechaData
    {
        public Vector3 posicion;        // Posición relativa a la imagen
        public Vector3 eulerRotation;   // Rotación local personalizada (por flecha)
    }

    public GameObject flechaPrefab;
    public List<FlechaData> flechasDatos = new List<FlechaData>();

    private List<GameObject> flechasInstanciadas = new List<GameObject>();
    private ARTrackedImageManager imageManager;

    void OnEnable()
    {
        imageManager = FindObjectOfType<ARTrackedImageManager>();
        imageManager.trackedImagesChanged += OnImageChanged;
    }

    void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnImageChanged;
    }

    void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var tracked in args.added)
        {
            InstanciarFlechas(tracked.transform);
        }
    }

    void InstanciarFlechas(Transform imagenTransform)
    {
        flechasInstanciadas.Clear();

        for (int i = 0; i < flechasDatos.Count; i++)
        {
            // Posición relativa al patrón
            Vector3 posicionMundo = imagenTransform.TransformPoint(flechasDatos[i].posicion);

            // Rotaciones combinadas:
            Quaternion rotacionBase = Quaternion.Euler(-90, 0, 0); // Tumbada
            Quaternion rotacionPersonal = Quaternion.Euler(flechasDatos[i].eulerRotation); // Personalizada por flecha
            Quaternion rotacionMundo = imagenTransform.rotation * rotacionBase * rotacionPersonal;

            GameObject flecha = Instantiate(flechaPrefab, posicionMundo, rotacionMundo);
            flecha.SetActive(i == 0); // Solo la primera está activa

            // Configura el trigger
            var trigger = flecha.GetComponent<FlechaTrigger>();
            trigger.Index = i;
            trigger.Manager = this;

            flechasInstanciadas.Add(flecha);
        }
    }

    public void ActivarSiguienteFlecha(int index)
    {
        if (index + 1 < flechasInstanciadas.Count)
        {
            flechasInstanciadas[index + 1].SetActive(true);
        }
    }
}

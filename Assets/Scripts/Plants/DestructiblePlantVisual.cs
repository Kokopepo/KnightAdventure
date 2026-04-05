using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiblePlantVisual : MonoBehaviour
{
    [SerializeField] private DestructiblePlant destructiblePlant;
    [SerializeField] private GameObject bushDeathVFXPrefab;

    private void Start()
    {
        destructiblePlant.OnDestructibleTakeDamage += DestructiblePlant_OnDestructibleTakeDamage;
    }

    private void DestructiblePlant_OnDestructibleTakeDamage(object sender, System.EventArgs e)
    {
        ShowDeathVFX();
    }

    private void ShowDeathVFX()
    {
        Instantiate(bushDeathVFXPrefab, transform.position, Quaternion.identity);
        //Instantiate - создает новый объект на сцене
    }

    private void OnDestroy()
    {
        destructiblePlant.OnDestructibleTakeDamage -= DestructiblePlant_OnDestructibleTakeDamage;
    }
}

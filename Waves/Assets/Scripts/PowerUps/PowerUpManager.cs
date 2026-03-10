using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [Header("Configuração do mapa para a IA")]
    private GameObject[] vertices; // todos os vértices na cena

    [Header("PowerUps")]
    [SerializeField] private List<PowerUpData> powerUps; // lista de prefabs + tipo

    [Serializable]
    public class PowerUpData
    {
        public GameObject prefab; // prefab do powerup
        public PowerUp.PowerUpType type;  // tipo do powerup
    }


    private void Start()
    {
        // Pega todos os vértices com a tag
        vertices = GameObject.FindGameObjectsWithTag("PowerUpVertice");

        // Começa a spawnar a cada 10 segundos
        InvokeRepeating(nameof(SpawnPowerUp), 6f, 6f);
    }
    public void UpdateVertices()
    {
        vertices = GameObject.FindGameObjectsWithTag("PowerUpVertice");
    }
    /// <summary>
    /// Instancia um powerup aleatório em um vértice vazio
    /// </summary>
    public void SpawnPowerUp()
    {
        // Busca os vértices **atualmente existentes**
        GameObject[] currentVertices = GameObject.FindGameObjectsWithTag("PowerUpVertice");

        List<GameObject> emptyVertices = currentVertices.Where(v => v.transform.childCount == 0).ToList();

        if (emptyVertices.Count == 0)
        {
            return;
        }

        // Escolhe um vértice aleatório vazio
        GameObject randomVertice = emptyVertices[UnityEngine.Random.Range(0, emptyVertices.Count)];

        // Escolhe um powerup aleatório da lista powerUps
        PowerUpData chosenPowerUp = powerUps[UnityEngine.Random.Range(0, powerUps.Count)];

        // Instancia o prefab como filho do vértice e mete o z como -2 para nao ficar atrás do mapa xD

        GameObject newPowerUp = Instantiate(chosenPowerUp.prefab, randomVertice.transform);
        newPowerUp.transform.localPosition = Vector3.zero;
        Vector3 localPos = newPowerUp.transform.localPosition;
        localPos.z = -2f;
        newPowerUp.transform.localPosition = localPos;
        // Associa o script PowerUp e define o tipo
        PowerUp puScript = newPowerUp.GetComponent<PowerUp>();
        if (puScript == null)
        {
            puScript = newPowerUp.AddComponent<PowerUp>();
        }
        puScript.type = chosenPowerUp.type;

    }
}
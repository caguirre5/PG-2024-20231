using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private Tile _tilePrefab;

    [SerializeField]
    private int _gridWidth;

    [SerializeField]
    private int _gridDepth;

    [SerializeField]
    private float _startX = 0f; // Posici�n inicial en el eje X desde el editor
    [SerializeField]
    private float _startZ = 0f; // Posici�n inicial en el eje Z desde el editor

    [SerializeField]
    private int _maxTilesToVisit = 56; // N�mero m�ximo de bloques a visitar

    private Tile[,] _tileGrid;
    private int _visitedTilesCount = 0; // Contador de bloques visitados

    private List<Vector3> _removedBlockPositions = new List<Vector3>(); // Lista de posiciones donde los bloques fueron eliminados

    [SerializeField]
    private float _minDistanceLargeObjects = 5f; // Distancia m�nima entre objetos grandes
    [SerializeField]
    private float _minDistanceSmallObjects = 2f; // Distancia m�nima entre objetos peque�os

    [SerializeField]
    private GameObject[] largeObjectPrefabs;
    [SerializeField]
    private GameObject[] smallObjectPrefabs;
    [SerializeField]
    private GameObject[] stonePrefabs; // Prefabs para las piedras

    private List<Vector3> _isolatedPositions = new List<Vector3>();

    int treeCount = 0;
    private float _minDistanceBetweenObjectsToAdd = 3f;

    [SerializeField]
    private GameObject[] specialObjects; // Array de prefabs adicionales

    private GameController _gameController;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        _tileGrid = new Tile[_gridWidth, _gridDepth];

        // Crear el grid completo
        for (int i = 0; i < _gridWidth; i++)
        {
            for (int j = 0; j < _gridDepth; j++)
            {
                float x = _startX + i;  // Ajusta la posici�n en el mundo en X
                float z = _startZ + j;  // Ajusta la posici�n en el mundo en Z
                _tileGrid[i, j] = Instantiate(_tilePrefab, new Vector3(x, 2, z), Quaternion.identity);
            }
        }

        _gameController = FindObjectOfType<GameController>();

        // Generar las habitaciones de forma secuencial, empezando por la celda inicial
        yield return GenerateMap(null, _tileGrid[6, 0]);
        FindIsolatedTiles();


        PlaceObjects();

        // Esperar hasta que la lista objectsToAdd en el GameController no esté vacía
        yield return new WaitUntil(() => _gameController.objectsToAdd.Count > 0);
        PlaceObjectsToAddInIsolatedPositions();
    }

    // M�todo que aplica Poisson Disk Sampling para colocar objetos
    private void PlaceObjects()
    {
        List<Vector3> placedObjects = new List<Vector3>();

        // Colocamos objetos grandes (árboles o piedras)
        foreach (var position in _isolatedPositions)
        {
            Vector3 candidatePoint = GenerateCandidate(position, _minDistanceLargeObjects);

            if (IsInValidPosition(candidatePoint, _isolatedPositions) && IsValidCandidate(candidatePoint, placedObjects, _minDistanceLargeObjects))
            {
                GameObject objectToPlace;

                // Si se han colocado 5 árboles, colocar una piedra en lugar de un árbol
                if (treeCount >= 5)
                {
                    objectToPlace = stonePrefabs[Random.Range(0, stonePrefabs.Length)];
                    treeCount = 0; // Reiniciar el contador después de colocar una piedra
                }
                else
                {
                    objectToPlace = largeObjectPrefabs[Random.Range(0, largeObjectPrefabs.Length)];
                    treeCount++; // Incrementar el contador de árboles
                }

                Instantiate(objectToPlace, candidatePoint, Quaternion.identity);
                placedObjects.Add(candidatePoint);
            }
        }

        // Colocamos objetos pequeños
        foreach (var position in _removedBlockPositions)
        {
            Vector3 candidatePoint = GenerateCandidate(position, _minDistanceSmallObjects);

            if (IsInValidPosition(candidatePoint, _removedBlockPositions) && IsValidCandidate(candidatePoint, placedObjects, _minDistanceSmallObjects))
            {
                GameObject smallObject = smallObjectPrefabs[Random.Range(0, smallObjectPrefabs.Length)];
                Instantiate(smallObject, candidatePoint, Quaternion.identity);
                placedObjects.Add(candidatePoint);
            }
        }

        if (specialObjects.Length > 0 && _isolatedPositions.Count > 0)
        {
            foreach (GameObject specialObject in specialObjects)
            {
                if (_isolatedPositions.Count == 0)
                {
                    Debug.LogWarning("No quedan más posiciones en _isolatedPositions para colocar objetos especiales.");
                    break; // Rompe el bucle si no quedan posiciones
                }

                // Seleccionar y eliminar una posición aleatoria de la lista para asegurar que no se reutiliza
                int randomIndex = Random.Range(0, _isolatedPositions.Count);
                Vector3 selectedPosition = _isolatedPositions[randomIndex];
                _isolatedPositions.RemoveAt(randomIndex);

                // Ajustar la altura y a 2.6
                selectedPosition.y = 2.6f;

                // Instanciar el objeto especial en la posición seleccionada
                Instantiate(specialObject, selectedPosition, Quaternion.identity);
                placedObjects.Add(selectedPosition);
            }
        }
    }


    // M�todo para verificar si el punto candidato est� dentro de un rango de tolerancia con respecto a las posiciones de bloques eliminados
    private bool IsInValidPosition(Vector3 candidatePoint, List<Vector3> positionsArray, float tolerance = 0.5f)
    {
        foreach (var position in positionsArray)
        {
            if (Vector3.Distance(candidatePoint, position) < tolerance)
            {
                return true; // El candidato est� dentro de un rango aceptable
            }
        }
        return false; // El candidato est� fuera del rango
    }

    private void PlaceObjectsToAddInIsolatedPositions()
    {
        List<Vector3> placedPositions = new List<Vector3>();

        // Seleccionar posiciones aleatorias para colocar los objetos de objectsToAdd
        foreach (var obj in _gameController.objectsToAdd)
        {
            Vector3 selectedPosition = GetRandomValidPosition(placedPositions);

            if (selectedPosition != Vector3.zero)
            {
                Instantiate(obj, selectedPosition, Quaternion.identity);
                placedPositions.Add(selectedPosition); // Agregar la posición a la lista de posiciones ocupadas
            }
            else
            {
                Debug.LogWarning("No se encontró una posición válida para colocar el objeto.");
            }
        }
    }

    private Vector3 GetRandomValidPosition(List<Vector3> placedPositions)
    {
        // Lista de posiciones válidas que cumplen con la distancia mínima
        List<Vector3> validPositions = _isolatedPositions
            .Where(pos => IsValidCandidate(pos, placedPositions, _minDistanceBetweenObjectsToAdd))
            .ToList();

        if (validPositions.Count > 0)
        {
            // Seleccionar una posición aleatoria de las válidas
            return validPositions[Random.Range(0, validPositions.Count)];
        }

        // Si no hay posiciones válidas, devolver Vector3.zero para indicar fallo
        return Vector3.zero;
    }


    private void FindIsolatedTiles()
    {
        _isolatedPositions.Clear(); // Limpiar el array antes de llenarlo
        for (int i = 0; i < _gridWidth; i++)
        {
            for (int j = 0; j < _gridDepth; j++)
            {
                Tile currentCell = _tileGrid[i, j];

                // Verificamos si el tile tiene vecinos
                bool hasRightNeighbor = (i + 1 < _gridWidth) && _tileGrid[i + 1, j].isVisited;
                bool hasLeftNeighbor = (i - 1 >= 0) && _tileGrid[i - 1, j].isVisited;
                bool hasFrontNeighbor = (j + 1 < _gridDepth) && _tileGrid[i, j + 1].isVisited;
                bool hasBackNeighbor = (j - 1 >= 0) && _tileGrid[i, j - 1].isVisited;

                // Si no tiene los 4 vecinos, almacenamos su posici�n
                if (hasRightNeighbor && hasLeftNeighbor && hasFrontNeighbor && hasBackNeighbor)
                {
                    Vector3 isolatedPosition = currentCell.transform.position;
                    _isolatedPositions.Add(isolatedPosition);
                }
            }
        }
    }

    private Vector3 GenerateCandidate(Vector3 initialPoint, float minDistance)
    {
        float angle = Random.Range(0, Mathf.PI * 2); // �ngulo aleatorio
        float distance = Random.Range(minDistance, minDistance * 2); // Distancia aleatoria
        return initialPoint + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * distance; // Generar punto
    }

    private IEnumerator GenerateMap(Tile previousCell, Tile currentCell)
    {
        // Aumentar el contador de bloques visitados
        _visitedTilesCount++;
        _removedBlockPositions.Add(currentCell.transform.position);
        currentCell.Visit();

        yield return new WaitForSeconds(0.01f);

        // Si se ha alcanzado el n�mero m�ximo de bloques a visitar, detener el proceso
        if (_visitedTilesCount >= _maxTilesToVisit)
        {
            yield break; // Termina la corutina y la generaci�n del mapa
        }

        Tile nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                // Continuar generando el mapa con la siguiente celda
                yield return GenerateMap(currentCell, nextCell);
            }
        } while (nextCell != null && _visitedTilesCount < _maxTilesToVisit);
    }

    private Tile GetNextUnvisitedCell(Tile currentCell)
    {
        var unvisitedCells = GetUnvisitedCell(currentCell);

        // Selecciona un vecino no visitado al azar
        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<Tile> GetUnvisitedCell(Tile currentCell)
    {
        // Calcula los �ndices del array a partir de las posiciones del mundo
        int x = Mathf.RoundToInt(currentCell.transform.position.x - _startX);
        int z = Mathf.RoundToInt(currentCell.transform.position.z - _startZ);

        if (x + 1 < _gridWidth)
        {
            var cellToRight = _tileGrid[x + 1, z];

            if (cellToRight.isVisited == false)
            {
                yield return cellToRight;
            }
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = _tileGrid[x - 1, z];

            if (cellToLeft.isVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if (z + 1 < _gridDepth)
        {
            var cellToFront = _tileGrid[x, z + 1];

            if (cellToFront.isVisited == false)
            {
                yield return cellToFront;
            }
        }

        if (z - 1 >= 0)
        {
            var cellToBack = _tileGrid[x, z - 1];

            if (cellToBack.isVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    private Vector3 GetValidPositionForSpecialObject(List<Vector3> placedObjects, float minDistance)
    {
    // Intenta encontrar una posición válida para el objeto especial
        foreach (var position in _isolatedPositions)
        {
            Vector3 candidatePoint = GenerateCandidate(position, minDistance);
            if (IsValidCandidate(candidatePoint, placedObjects, minDistance))
            {
                return candidatePoint;
            }
        }
        return Vector3.zero; // Devuelve Vector3.zero si no encuentra una posición adecuada
    }

    private bool IsValidCandidate(Vector3 candidate, List<Vector3> placedObjects, float minDistance)
    {
        foreach (var obj in placedObjects)
        {
            if (Vector3.Distance(candidate, obj) < minDistance)
            {
                return false; // El candidato est� demasiado cerca de otro objeto
            }
        }
        return true; // El candidato es v�lido
    }

}
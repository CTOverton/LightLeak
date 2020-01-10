using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class PerlinGenerator : MonoBehaviour
{
    public static PerlinGenerator instance = null;
    
    public int perlinTextureSizeX;
    public int perlinTextureSizeY;
    public bool randomizeNoiseOffset;
    public Vector2 perlinOffset;
    public float noiseScale = 1f;
    public int perlinGridStepSizeX = 4;
    public int perlinGridStepSizeY = 4;

    public bool visualizeGrid = false;
    public GameObject visualizationCube;
    public float visualizationHeightScale = 5f;
    public RawImage visualizationUI;

    public float seaLevel;
    public RawImage visualizationUI_Colors;

    public GameObject AlgorithmVisual;

    public GameObject tree;
    private GameObject ActiveAlgorithmVisual;

    private Texture2D perlinTexture;
    private GameObject visualizationParent = null;
    
    private Dictionary<Vector2, String> nodes = new Dictionary<Vector2, String>();

    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        if (visualizationParent == null)
        {
            visualizationParent = new GameObject("VisualizationParent");
            visualizationParent.transform.SetParent(this.transform);
        }
        else if (visualizationParent != null)
        {
            Destroy(visualizationParent);
        }
        
        GenerateNoise();
        if (visualizeGrid)
        {
//            StartCoroutine(VisualizeGrid());
//            StartCoroutine(VisualizeGrid_BETTER());
            VisualizeGrid_BETTER();
        }
    }

    

    private void GenerateNoise()
    {
        if (randomizeNoiseOffset)
        {
            perlinOffset = new Vector2(Random.Range(0,99999),Random.Range(0,99999));
        }
        
        perlinTexture = new Texture2D(perlinTextureSizeX, perlinTextureSizeY);

        for (int x = 0; x < perlinTextureSizeX; x++)
        {
            for (int y = 0; y < perlinTextureSizeY; y++)
            {
                perlinTexture.SetPixel(x,y,SampleNoise(x,y));
            }
        }
        
        perlinTexture.Apply();
        visualizationUI.texture = perlinTexture;
    }

    private Color SampleNoise(int x, int y)
    {
        float xCoord = (float) x / perlinTextureSizeX * noiseScale + perlinOffset.x;
        float yCoord = (float) y / perlinTextureSizeY * noiseScale + perlinOffset.y;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        Color perlinColor = new Color(sample, sample, sample);

        return perlinColor;
    }

    private IEnumerator  VisualizeGrid()
    {
        Dictionary<float, Color> islandTypes = new Dictionary<float, Color>();
        Dictionary<Vector2,float> nodes = new Dictionary<Vector2, float>();

        for (int x = 0; x < perlinGridStepSizeX; x++)
        {
            for (int y = 0; y < perlinGridStepSizeY; y++)
            {
                Vector2 current = new Vector2(x, y);
                float h = SampleStepped(x, y) * visualizationHeightScale;
                
                GameObject clone = Instantiate(visualizationCube, 
                    new Vector3(x, h, y) 
                    + transform.position, transform.rotation);

                if (h > seaLevel)
                {
                    // Check Neighbors
                    bool neighborFound = false;
                    for (int i = x - 1; i < x + 2; i++)
                    {
                        for (int j = y - 1; j < y + 2; j++)
                        {
                            if (neighborFound) continue;
                            
                            
                            Vector2 proposed = new Vector2(i,j);
                            float proposedHeight = SampleStepped(i, j) * visualizationHeightScale;
                                                        
                            if (current != proposed && nodes.ContainsKey(proposed) && proposedHeight > seaLevel)
                            {
                                neighborFound = true;
                                nodes.Add(current,nodes[proposed]);
                            }
    
                        } 
                    }

                    bool foundingNode = false;
                    if (!neighborFound)
                    {
                        // Start new island
                        Color randomColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                        float islandType = Random.Range(0f, 99999f);
                        Debug.Log("Adding new island " + islandType);
                        islandTypes.Add(islandType,randomColor);
                        nodes.Add(current, islandType);
                        foundingNode = true;
                    }

                    if (foundingNode)
                    {
                        clone.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
                    }
                    else
                    {
                        clone.GetComponent<MeshRenderer>().material.SetColor("_Color", islandTypes[nodes[current]]);
                    }
                    
                }
                else
                {
                    clone.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);
                }
                
                
                //clone.transform.localScale = new Vector3(1,i,1);
                clone.transform.SetParent(visualizationParent.transform);
                //GeneratedObjectControl.instance.AddObject(clone);
                yield return new WaitForFixedUpdate();
            }
        }

        /*visualizationParent.transform.position = 
            new Vector3(-perlinGridStepSizeX * .5f, 0, -perlinGridStepSizeY * .5f);*/
     // visualizationParent.transform.position = new Vector3(-perlinGridStepSizeX * .5f, -visualizationHeightScale * .5f, -perlinGridStepSizeY * .5f);   
    }
    
    void VisualizeGrid_BETTER()
    {

//        ActiveAlgorithmVisual = Instantiate(AlgorithmVisual, transform.position, transform.rotation);
//        ActiveAlgorithmVisual.transform.SetParent(this.transform);
        
        String[] Islands = new[] {"Trees", "Rocks", "Bedrock"};


        // Loop for grid size
        for (int x = 0; x < perlinGridStepSizeX; x++)
        {
            for (int y = 0; y < perlinGridStepSizeY; y++)
            {
//                Debug.Log("Starting new Check");
                Vector2 current = new Vector2(x, y);
                float perlinHeight = SampleStepped(x, y) * visualizationHeightScale;
                
//                ActiveAlgorithmVisual.transform.position = new Vector3(x,perlinHeight,y);
                check(x, y, Islands[Random.Range(0,Islands.Length)]);
//                check(x, y, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));
//                StartCoroutine(check(x,y, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f)));
//                yield return new WaitForFixedUpdate();
//                yield return null;
            }
        }
        Debug.Log("Done");
    }

    void check(int x, int y, String islandType)
    {
//        yield return new WaitForSecondsRealtime(0.1f);
//        yield return null;
        Vector2 current = new Vector2(x,y);
        float perlinHeight = SampleStepped(x, y) * visualizationHeightScale;
        
//        ActiveAlgorithmVisual.transform.position = new Vector3(x,perlinHeight,y);
        
        if (!(nodes.ContainsKey(current) || perlinHeight < seaLevel))
        {
            nodes.Add(current,islandType);
            GenCube(x, perlinHeight, y, islandType);
            
            // Check surrounding Nodes
            for (int i = x-1; i <= x+1; i++)
            {
                for (int j = y-1; j <= y+1; j++)
                {
                    if ((i >= 0 && i <= perlinGridStepSizeX) && (j >= 0 && j <= perlinGridStepSizeY) && new Vector2(i,j) != current)
                    {
//                        StartCoroutine(check(i, j, islandType));
                        check(i, j, islandType);
                    }
                }
            }
        }
        
//        Debug.Log("Done checking");
    }

    void GenCube(float x, float y, float z, String type)
    {


        switch (type)
        {
            case "Trees":
                if (Random.Range(0f,1f) < 0.5)
                {
                    GameObject treeClone = Instantiate(
                                        tree, 
                                        new Vector3(x, seaLevel+1, z) + transform.position, transform.rotation
                                    );
                                    treeClone.transform.SetParent(visualizationParent.transform);
                }
                
                break;
            case "Rocks":
                GameObject clone = Instantiate(
                    visualizationCube, 
                    new Vector3(x, y, z) + transform.position, transform.rotation
                );
                clone.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.gray);
                clone.transform.SetParent(visualizationParent.transform);
                break;
            case "Bedrock":
                GameObject Bedrock = Instantiate(
                    visualizationCube, 
                    new Vector3(x, y, z) + transform.position, transform.rotation
                );
                Bedrock.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);
                Bedrock.transform.SetParent(visualizationParent.transform);
                
                break;
            default:
                break;
        }
        
        /*GameObject clone = Instantiate(
            visualizationCube, 
            new Vector3(x, y, z) + transform.position, transform.rotation
            );
        clone.GetComponent<MeshRenderer>().material.SetColor("_Color", type);
        clone.transform.SetParent(visualizationParent.transform);*/
//        Debug.Log("X: " + x + " Y: " + y + " Z: " + z + " Color: " + color);
    }
    
    private float SampleStepped(int x, int y)
    {
        int gridStepSizeX = perlinTextureSizeX / perlinGridStepSizeX;
        int gridStepSizeY = perlinTextureSizeY / perlinGridStepSizeY;
        
        float sampleFloat = perlinTexture.GetPixel(Mathf.FloorToInt(x * gridStepSizeX), Mathf.FloorToInt(y * gridStepSizeX)).grayscale;
        
        return sampleFloat;
    }
}

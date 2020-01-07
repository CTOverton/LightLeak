using System.Collections.Generic;
using UnityEngine;

public class CreatePerlin : MonoBehaviour
{
    private Texture2D myNoiseTexture;
    private Texture2D myNoiseTextureUntouched;
    private float threshold = .55f;
    private float[,] islandData;
    private Color[,] islandColorData;
    private List<List<Vector2>> islands;
    private bool[,] islandSearched;
    private int resolution = 512;

    void Start()
    {
        myNoiseTexture = new Texture2D(resolution, resolution);
        myNoiseTextureUntouched = new Texture2D(resolution, resolution);
        islandData = new float[resolution, resolution];
        islandSearched = new bool[resolution, resolution];
        islandColorData = new Color[resolution, resolution];
        islands = new List<List<Vector2>>();

        for (int x = 0; x < myNoiseTexture.width; x++)
        {
            for (int y = 0; y < myNoiseTexture.height; y++)
            {
                float mult = .02f;
                float myNoiseValue = Mathf.PerlinNoise(x * mult, y * mult);
                myNoiseValue -= threshold;
                myNoiseValue *= 1f / (1f - threshold);

                myNoiseValue = Mathf.Clamp(myNoiseValue, 0, 1);
                islandData[x, y] = myNoiseValue;
            }
        }

        Growth();
    }

    private void UpdateTexture()
    {
        for (int x = 0; x < myNoiseTexture.width; x++)
        {
            for (int y = 0; y < myNoiseTexture.height; y++)
            {
                myNoiseTextureUntouched.SetPixel(x, y, new Color(islandData[x, y], islandData[x, y], islandData[x, y], 1));
                myNoiseTexture.SetPixel(x, y, islandColorData[x, y]);
            }
        }

        myNoiseTexture.Apply();
        myNoiseTextureUntouched.Apply();
    }

    private void Growth()
    {
        for (int x = 0; x < myNoiseTexture.width; x++)
        {
            for (int y = 0; y < myNoiseTexture.height; y++)
            {
                islandColorData[x, y] = Color.black;

                if (!islandSearched[x, y] && islandData[x,y] > 0)
                {
                    islands.Add(GrowthWalk(x, y));
                }   
            }
        }

        foreach(List<Vector2> island in islands)
        {
            Color randomColor = Random.ColorHSV(0,1,0,1,1,1);

            foreach (Vector2 gridLoc in island)
            {
                float mult = islandData[(int)gridLoc.x, (int)gridLoc.y];

                islandColorData[(int)gridLoc.x, (int)gridLoc.y] = randomColor;
            }
        }

        UpdateTexture();
    }
    
    private List<Vector2> GrowthWalk(int x, int y)
    {
        List<Vector2> returnList = new List<Vector2>();
        List<Vector2> nextSteps = new List<Vector2>();
        List<Vector2> nextStepsTemp = new List<Vector2>();

        nextSteps.Add(new Vector2(x, y));
        islandSearched[x, y] = true;

        while (nextSteps.Count > 0)
        {
            foreach (Vector2 nextStep in nextSteps)
            {
                islandSearched[(int)nextStep.x, (int)nextStep.y] = true;
                returnList.Add(nextStep);

                //stepNorth
                if (nextStep.y < resolution - 1)
                {
                    if (islandData[(int)nextStep.x, (int)nextStep.y + 1] > 0 && !islandSearched[(int)nextStep.x, (int)nextStep.y + 1])
                    {
                        nextStepsTemp.Add(nextStep + new Vector2(0, 1));
                        islandSearched[(int)nextStep.x, (int)nextStep.y + 1] = true;
                    }
                }

                //stepSouth
                if (nextStep.y > 0)
                {
                    if (islandData[(int)nextStep.x, (int)nextStep.y - 1] > 0 && !islandSearched[(int)nextStep.x, (int)nextStep.y - 1])
                    {
                        nextStepsTemp.Add(nextStep + new Vector2(0, -1));
                        islandSearched[(int)nextStep.x, (int)nextStep.y - 1] = true;
                    }
                }

                //stepEast
                if (nextStep.x < resolution - 1)
                {
                    if (islandData[(int)nextStep.x + 1, (int)nextStep.y] > 0 && !islandSearched[(int)nextStep.x + 1, (int)nextStep.y])
                    {
                        nextStepsTemp.Add(nextStep + new Vector2(1, 0));
                        islandSearched[(int)nextStep.x + 1, (int)nextStep.y] = true;
                    }
                }

                //stepWest
                if (nextStep.x > 0)
                {
                    if (islandData[(int)nextStep.x - 1, (int)nextStep.y] > 0 && !islandSearched[(int)nextStep.x - 1, (int)nextStep.y])
                    {
                        nextStepsTemp.Add(nextStep + new Vector2(-1, 0));
                        islandSearched[(int)nextStep.x - 1, (int)nextStep.y] = true;
                    }
                }
            }

            nextSteps = new List<Vector2>(nextStepsTemp);
            nextStepsTemp.Clear();
        }

        return returnList;
    }

    private void OnGUI()
    {
        if(myNoiseTexture != null)
        {
            GUI.DrawTexture(new Rect(0, 0, resolution, resolution), myNoiseTexture);
            GUI.DrawTexture(new Rect(540, 0, resolution, resolution), myNoiseTextureUntouched);
        }
    }
}
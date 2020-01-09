using UnityEngine;

public class DropBoxDropper : MonoBehaviour
{
    public GameObject boxPrefab;
	
    private Vector3[,] boxes;
    private int incrementX;
    private int incrementY;

    private bool working;

    private int amount = 20;

    private void Start()
    {
        boxes = new Vector3[amount, amount];

        for(int x = 0; x < boxes.GetLength(0); x++)
        {
            for (int y = 0; y < boxes.GetLength(1); y++)
            {
                boxes[x,y] = new Vector3(x, 0, y);
            }
        }
    }

    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            working = true;
        }
		
        if(working)
        {
            incrementX++;

            Instantiate(boxPrefab, boxes[incrementX, incrementY], Quaternion.identity);

            if(incrementX == amount - 1)
            {
                incrementY++;
                incrementX = 0;

                if(incrementY == amount - 1)
                {
                    working = false;
                }
            }
        }
    }
}
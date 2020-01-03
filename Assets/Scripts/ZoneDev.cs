using UnityEngine;

namespace ZoneDev
{
    public class Zone : Component
    {
        private Vector2 id;
        private string type;
        private int size;
        private Vector2 origin;
    
        public Zone(Vector2 id, string type, int size, Vector2 origin)
        {
            this.id = id;
            this.type = type;
            this.size = size;
            this.origin = origin;
            Generate();
        }

        public void Generate()
        {
            // Generate Zone Trigger
            GameObject trigger = Instantiate(Resources.Load("Prefabs/Zone_Trigger", typeof(GameObject)), new Vector3(origin.x + (size / 2),1,origin.y + (size / 2)), Quaternion.identity) as GameObject;
            trigger.transform.localScale = new Vector3(size,2,size);
            
            Zone_Trigger zone = trigger.GetComponent<Zone_Trigger>();
            zone.id = id;
            
            GameObject[] grass =
            {
                (GameObject)Resources.Load("Prefabs/Ground/Base_Block_Zone", typeof(GameObject)), 
                (GameObject)Resources.Load("Prefabs/Ground/Base_Block_Zone 1", typeof(GameObject))
            };
            
            GameObject[] trees =
            {
                (GameObject)Resources.Load("Prefabs/Trees/Normal/Tree", typeof(GameObject)), 
                (GameObject)Resources.Load("Prefabs/Trees/Normal/Tree 1", typeof(GameObject))
            };
            
            var block = Resources.Load("Prefabs/Ground/Base_Block", typeof(GameObject));
            switch (type)
            {
                case "home":
                    block = Resources.Load("Prefabs/Ground/Base_Block", typeof(GameObject));
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            GameObject newBlock = Instantiate(block, new Vector3(origin.x + i,0,origin.y + j), Quaternion.identity) as GameObject;
                        } 
                    }
                    break;
                default:
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            int randomIndex = Random.Range(0, grass.Length);
                            block = grass[randomIndex];
                            if (randomIndex == 1 && Random.Range(0f,1f) < 0.5)
                            {
                                int randomTreeIndex = Random.Range(0, trees.Length);
                                GameObject newTree = Instantiate(trees[randomTreeIndex], new Vector3(origin.x + i,1,origin.y + j), Quaternion.identity) as GameObject;
                                newTree.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                            }
                            
                            GameObject newBlock = Instantiate(block, new Vector3(origin.x + i,0,origin.y + j), Quaternion.identity) as GameObject;
                        } 
                    }
                    break;
            }
        }
    
        public void debug()
        {
            Debug.Log("Zone " + id);
            Debug.Log("type:  " + type);
            Debug.Log("size:  " + size);
            Debug.Log("origin:  " + origin);
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZoneDev;

public class MainGenerator : MonoBehaviour
{
    public int zoneSize = 20;

    Dictionary<Vector2,Zone> zones = new Dictionary<Vector2, Zone>();
    Vector2 origin = new Vector2(0, 0);
    private void Start()
    {
        GenPlayerHome();
    }

    private void GenPlayerHome()
    {
        if (zones.ContainsKey(origin)) return;
        
        Debug.Log("Generating Player Home");
        Zone home = new Zone(
            origin,
            "home",
            zoneSize,
            new Vector2(0, 0)
        );
        zones[origin] = home;
        

        // Top
        GenBorderZone(origin + new Vector2(-1, 1));
        GenBorderZone(origin + new Vector2(0, 1));
        GenBorderZone(origin + new Vector2(1, 1));
        
        // Center
        GenBorderZone(origin + new Vector2(-1, 0));
        GenBorderZone(origin + new Vector2(1, 0));
        
        // Bottom
        GenBorderZone(origin + new Vector2(-1, -1));
        GenBorderZone(origin + new Vector2(0, -1));
        GenBorderZone(origin + new Vector2(1, -1));
        
    }

    private void GenBorderZone(Vector2 location)
    {
        try
        {
            Zone zone = new Zone(
                location,
                "default",
                zoneSize,
                location * zoneSize
            );
            zones.Add(location, zone);
            zone.debug();
        }
        catch (ArgumentException)
        {
            Debug.Log("Zone at " + location + " already exists");
        }
    }

    public void CheckBorders(Vector2 current)
    {
        Debug.Log("GENERATING FOR " + current);
        for (int i = (int)current.x - 2; i < (int)current.x + 3; i++)
        {
            for (int j = (int)current.y - 2; j < (int)current.y + 3; j++)
            {
                Vector2 proposed = new Vector2(i,j);
                Debug.Log("Proposed " + proposed);
                if (current != proposed && !zones.ContainsKey(proposed))
                {
                    GenZone(proposed);
                }
            } 
        }
    }
    
    private void GenZone(Vector2 location)
    {
        try
        {
            Zone zone = new Zone(
                location,
                "default",
                zoneSize,
                location * zoneSize
            );
            zones.Add(location, zone);
            zone.debug();
        }
        catch (ArgumentException)
        {
            Debug.Log("Zone at " + location + " already exists");
        }
    }
    
    
}

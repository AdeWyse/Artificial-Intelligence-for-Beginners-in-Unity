using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Timeline;
using Unity.VisualScripting;

public class PathMarker
{
    public MapLocation location;
    public float G;
    public float H;
    public float F;

    public GameObject marker;
    public PathMarker parent; 

    public PathMarker (MapLocation l, float g, float h, float f,GameObject marker, PathMarker p)
    {
        location = l;
        G = g;
        H = h;
        F = f;
        this.marker = marker;
        parent = p;
    }

    public override bool Equals(object obj)
    {
        if(obj == null || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            return location.Equals(((PathMarker) obj).location);
        }
    }

    public override int GetHashCode()
    {
        return 0;
    }

}
public class FindAStarPath : MonoBehaviour
{

    public Maze maze;
    public Material closedMaterial;
    public Material openMaterial;

    List<PathMarker> open;
    List<PathMarker> closed;

    public GameObject start;
    public GameObject end;
    public GameObject pathp;

    PathMarker goalNode;
    PathMarker startNode;

    PathMarker lastPos;

    bool done = false;

    void RemoveAllMarkers()
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag("marker");
        foreach(GameObject marker in markers)
        {
            Destroy(marker);
        }
    }

    void BeginSearch()
    {
        done = false;
        RemoveAllMarkers();

        List<MapLocation> locations = new List<MapLocation>();
        for(int z = 1; z < maze.depth - 1; z++){
            for (int x = 1; x < maze.width - 1; x++)
            {
                if(maze.map[x,z] != 1)
                {
                    locations.Add(new MapLocation(x, z));
                }

            }
        }

        locations.Shuffle();

        Vector3 startLocation = new Vector3(locations[0].x * maze.scale, 0, locations[0].z * maze.scale);
        startNode = new PathMarker(new MapLocation(locations[0].x, locations[0].z), 0, 0, 0, Instantiate(start, startLocation, Quaternion.identity), null);

        Vector3 goalLocation = new Vector3(locations[1].x * maze.scale , 0, locations[1].z * maze.scale);
        goalNode = new PathMarker(new MapLocation(locations[1].x, locations[1].z), 0, 0, 0, Instantiate(end, goalLocation, Quaternion.identity), null);

        open.Clear();
        closed.Clear();

        open.Add(startNode);
        lastPos = startNode;
    }

    public void Search(PathMarker thisNode)
    {
        if (thisNode.Equals(goalNode))//Goal was found
        {
            done = true;
            return;
        }

        foreach(MapLocation dir in maze.directions){
            MapLocation neighbour = dir + thisNode.location;

            if (maze.map[neighbour.x, neighbour.z] == 1)
            {
                continue;
            }

            if(neighbour.x < 1 || neighbour.x >= maze.width || neighbour.z < 1 ||  neighbour.z >= maze.depth)
            {
                continue;
            }

            if (IsClosed(neighbour))
            {
                continue;
            }

            float G = Vector2.Distance(thisNode.location.ToVector(), neighbour.ToVector()) + thisNode.G;
            float H = Vector2.Distance(neighbour.ToVector(), goalNode.location.ToVector());
            float F = G + H;

            GameObject pathBlock = Instantiate(pathp, new Vector3(neighbour.x * maze.scale, 0, neighbour.z * maze.scale), Quaternion.identity);
        }
    }

    bool IsClosed(MapLocation marker)
    {
        foreach(PathMarker p in closed)
        {
            if (p.location.Equals(marker))
            {
                return true;
            }
        }
        return false;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            BeginSearch();
        }
    }
}

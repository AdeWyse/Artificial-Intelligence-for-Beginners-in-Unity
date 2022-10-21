using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PathMarker
{
    public MapLocation location;
    public float G;
    public float H;
    public float F;

    public GameObject marker;
    public PathMarker parent;

    public PathMarker(MapLocation l, float g, float h, float f, GameObject m, PathMarker p)
    {
        location = l;
        G = g;
        H = h;
        F = f;
        marker = m;
        parent = p;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || !this.GetType().Equals(obj.GetType()))
        {
            return false;
            
        }
        else
        {
            int xTest = ((PathMarker)obj).location.x;
            int yTest = ((PathMarker)obj).location.z;
            if ( xTest == location.x && yTest == location.z){
                return true;
            }
            return false;   
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

    List<PathMarker> open = new List<PathMarker>();
    List<PathMarker> closed = new List<PathMarker>();

    public GameObject start;
    public GameObject end;
    public GameObject pathP;

    PathMarker goalNode;
    PathMarker startNode;

    PathMarker lastPos;

    public bool done = false;
    public bool go = false;

    void RemoveAllMarkers()
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag("marker");
        foreach (GameObject marker in markers)
        {
            Destroy(marker);
        }
    }

    void BeginSearch()
    {
        done = false;
        RemoveAllMarkers();

        List<MapLocation> locations = new List<MapLocation>();
        for (int z = 1; z < maze.depth - 1; ++z)
        {
            for (int x = 1; x < maze.width - 1; ++x)
            {
                if (maze.map[x, z] != 1)
                {
                    locations.Add(new MapLocation(x, z));
                }

            }
        }

        locations.Shuffle();

        Vector3 startLocation = new Vector3(locations[0].x * maze.scale, 0f, locations[0].z * maze.scale);
        startNode = new PathMarker(new MapLocation(locations[0].x, locations[0].z), 0f, 0f, 0f, Instantiate(start, startLocation, Quaternion.identity), null);

        Vector3 goalLocation = new Vector3(locations[1].x * maze.scale, 0f, locations[1].z * maze.scale);
        goalNode = new PathMarker(new MapLocation(locations[1].x, locations[1].z), 0f, 0f, 0f, Instantiate(end, goalLocation, Quaternion.identity), null);

        open.Clear();
        closed.Clear();

        open.Add(startNode);
        lastPos = startNode;
    }

    public void Search(PathMarker thisNode)
    {
        if (thisNode == null)
        {
            return;
        }

        if (thisNode.Equals(goalNode))//Goal was found
        {
            done = true;
            return;
        }

        foreach (MapLocation dir in maze.directions)
        {
            MapLocation neighbour = dir + thisNode.location;

            if (maze.map[neighbour.x, neighbour.z] == 1)
            {
                continue;
            }

            if (neighbour.x < 1 || neighbour.x >= maze.width || neighbour.z < 1 || neighbour.z >= maze.depth)
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

            GameObject pathBlock = Instantiate(pathP, new Vector3(neighbour.x * maze.scale, 0f, neighbour.z * maze.scale), Quaternion.identity);

            TextMesh[] values = pathBlock.GetComponentsInChildren<TextMesh>();
            values[0].text = "G: " + G.ToString("0.00");
            values[1].text = "H: " + H.ToString("0.00");
            values[2].text = "F: " + F.ToString("0.00");

            if (!UpdateMarker(neighbour, G, H, F, thisNode))
            {
                open.Add(new PathMarker(neighbour, G, H, F, pathBlock, thisNode));
            }

        }

        open = open.OrderBy(p => p.F).ThenBy(n => n.H).ToList<PathMarker>();
        PathMarker pm = (PathMarker)open.ElementAt(0);

        closed.Add(pm);

        open.RemoveAt(0);
        pm.marker.GetComponent<Renderer>().material = closedMaterial;
        lastPos = pm;
    }

    bool UpdateMarker(MapLocation pos, float g, float h, float f, PathMarker parent)
    {
        foreach (PathMarker p in open)
        {
            if (p.location.Equals(pos))
            {
                p.G = g;
                p.H = h;
                p.F = f;
                p.parent = parent;
                return true;
            }

        }
        return false;

    }

    void GetPath()
    {

        RemoveAllMarkers();
        PathMarker begin = lastPos;

        while (!startNode.Equals(begin) && begin != null)
        {

            Instantiate(pathP, new Vector3(begin.location.x * maze.scale, 0.0f, begin.location.z * maze.scale), Quaternion.identity);
            begin = begin.parent;
        }

        Instantiate(pathP, new Vector3(startNode.location.x * maze.scale, 0.0f, startNode.location.z * maze.scale), Quaternion.identity);
    }

    bool IsClosed(MapLocation marker)
    {
        foreach (PathMarker p in closed)
        {
            if (p.location.x == marker.x && p.location.z == marker.z)
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
            go = true;
        }

        if (Input.GetKeyDown(KeyCode.V) && !done)
        {
            Search(lastPos);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            GetPath();
        }

        if (!done && go)
        {
            Search(lastPos);
        }
        else
        {
            if (go)
            {
                GetPath();
            }
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class MazeCellEdge : MonoBehaviour
{
    public MazeCell cell, othercell;
    public MazeDirection direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }
 public virtual void Initialize(MazeCell cell, MazeCell othercell, MazeDirection direction)
    {
        this.cell = cell;
        this.othercell = othercell;
        this.direction = direction;
        cell.SetEdge(direction, this);
        transform.parent = cell.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = direction.ToRotation();


    }
    public virtual void OnPlayerEntered()
    {

    }

    public virtual void OnPlayerExited()
    {

    }
   
}

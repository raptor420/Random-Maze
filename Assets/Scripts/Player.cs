using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private MazeCell currentcell;
    private MazeDirection currentDirection;

    private void Look(MazeDirection direction)
    {
        transform.localRotation = direction.ToRotation();
        currentDirection = direction;
    }
    public void SetLocation(MazeCell cell)
    {
        if(currentcell != null)
        {

            currentcell.OnPlayerExited();
        }
        currentcell = cell;
        transform.localPosition = cell.transform.localPosition;
        currentcell.OnplayerEntered();

    }

    private void Move( MazeDirection direction)
    {
        MazeCellEdge edge = currentcell.GetEdge(direction);
        if(edge is MazePassage)
        {

            SetLocation(edge.othercell);
        }

    }
 

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(currentDirection);

        }
       else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(currentDirection.GetNextClockwise());

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {

            Move(currentDirection.GetOpposite());
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

            Move(currentDirection.GetNextCounterClockwise());
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Look(currentDirection.GetNextCounterClockwise());
        }
        else if(Input.GetKeyDown(KeyCode.E)) {

            Look(currentDirection.GetNextClockwise());
        }
    }
}

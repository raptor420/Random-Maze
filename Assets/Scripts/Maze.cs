

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public MazeDoor doorprefab;
    [Range(0,1)]
    public float doorProbability;
    public MazeRoomSettings[] roomSettings;
    //public int SizeX, SizeZ;
    public MazeCell cellPrefab;
    public MazePassage passagePrefab;
    public MazeWall[]  wallPrefab;
    private MazeCell[,] cells;
    public float GenerateDelay;
    public IntVector2 size;
    private List<MazeRoom> Rooms = new List<MazeRoom>();
    private MazeRoom CreateRoom(int indexToExclude)
    {
        MazeRoom newRoom = ScriptableObject.CreateInstance<MazeRoom>();
        newRoom.settingIndex = Random.Range(0, roomSettings.Length);
        if(newRoom.settingIndex == indexToExclude)
        {
            newRoom.settingIndex = (newRoom.settingIndex + 1) % roomSettings.Length;


        }
        newRoom.settings = roomSettings[newRoom.settingIndex];
        Rooms.Add(newRoom);
        return newRoom;


    }

    public MazeCell GetCell(IntVector2 coordinates)
    {

        return cells[coordinates.x, coordinates.z];
    }
    public IEnumerator Generate()
    {

        cells = new MazeCell[size.x, size.z];
        List<MazeCell> activecells = new List<MazeCell>();
        DoFirstGenerationStep(activecells);
        // IntVector2 coordinates = RandomCoordinates;
        while (activecells.Count > 0)
        {
            float generateDelay = GenerateDelay;
            yield return new WaitForSeconds(generateDelay);
            // CreatCell (coordinates);
            //  coordinates += MazeDirections.RandomValue.TointVector2();
            DoNextGenerationStep(activecells);
        }
       for( int i =0; i < Rooms.Count; i++)
        {

            Rooms[i].Hide();
        }

    }

    private void DoNextGenerationStep(List<MazeCell> activecells)
    {
        int currentIndex = activecells.Count - 1;
        
        MazeCell currentcell = activecells[currentIndex];
        if (currentcell.IsFullyInitialized)
        {

            activecells.RemoveAt(currentIndex);
            return;

        }
        MazeDirection direction = currentcell.RandomUnitializedDirection;
        IntVector2 coordinates = currentcell.coordinates + direction.TointVector2();
        if (ContainsCoordinates(coordinates))
        {
            MazeCell neighbor = GetCell(coordinates);
            if(neighbor == null)
            {


                neighbor = CreatCell(coordinates);
                CreatePassage(currentcell, neighbor, direction);
                activecells.Add(neighbor);

            }
            else if(currentcell.room.settingIndex == neighbor.room.settingIndex){


                CreatePassageInSameRoom(currentcell, neighbor, direction);

            }

            else
            {

                CreateWall(currentcell, neighbor, direction);
                //activecells.RemoveAt(currentIndex);

            }

        }
        else
        {

            CreateWall(currentcell, null, direction);
           // activecells.RemoveAt(currentIndex);

        }

    }

    private void CreateWall(MazeCell cell, MazeCell othercell , MazeDirection direction)
    {
        MazeWall wall = Instantiate(wallPrefab[Random.Range(0,wallPrefab.Length)]) as MazeWall;
        wall.Initialize(cell, othercell, direction);
        if (othercell != null)
        {

            wall = Instantiate(wallPrefab[Random.Range(0, wallPrefab.Length)]) as MazeWall;
            wall.Initialize(othercell, cell, direction.GetOpposite());


        }
    }

    private void CreatePassage(MazeCell cell, MazeCell othercell, MazeDirection direction)
    {
        MazePassage prefab = Random.value < doorProbability ? doorprefab : passagePrefab;
        MazePassage passage = Instantiate(prefab) as MazePassage;
        passage.Initialize(cell, othercell, direction);
        passage = Instantiate(prefab) as MazePassage;

        if(passage is MazeDoor)
        {

            othercell.Initialize(CreateRoom(cell.room.settingIndex));

        }

        else
        {

            othercell.Initialize(cell.room);

        }
        passage.Initialize(othercell, cell, direction.GetOpposite());
    }

    private void DoFirstGenerationStep(List<MazeCell> activecells)
    {

        MazeCell newCell = CreatCell(RandomCoordinates);
        newCell.Initialize(CreateRoom(-1));
        activecells.Add(newCell);
    }

    private MazeCell CreatCell(IntVector2 coordinates)
    {
        MazeCell newcell = Instantiate(cellPrefab) as MazeCell;
        cells[coordinates.x, coordinates.z] = newcell;
        newcell.coordinates = coordinates;
        newcell.name = "Maze cell" + coordinates.x + "," + coordinates.z;
        newcell.transform.parent = transform;
        newcell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
        return newcell;
    }

    private void CreatePassageInSameRoom(MazeCell cell,MazeCell othercell, MazeDirection direction)
    {
        MazePassage passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(cell, othercell, direction);
        passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(othercell, cell, direction.GetOpposite());

        if(cell.room!= othercell.room)
        {


            MazeRoom roomtoAAssimilate = othercell.room;
            cell.room.Assimlate(roomtoAAssimilate);
            Rooms.Remove(roomtoAAssimilate);
            Destroy(roomtoAAssimilate);
        }
    }

    public IntVector2 RandomCoordinates
    {
        get
        {

            return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
        }




    }

    
    public bool ContainsCoordinates(IntVector2 coordinates)
    {

        return coordinates.x >= 0 && coordinates.x < size.x && coordinates.z >= 0 && coordinates.z < size.z;



    }

}

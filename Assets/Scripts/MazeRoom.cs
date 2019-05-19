using System.Collections.Generic;
using UnityEngine;

public class MazeRoom : ScriptableObject
{
  public int settingIndex;
    public MazeRoomSettings settings;
    public List<MazeCell> cells = new List<MazeCell>();
    public void Add (MazeCell cell)
    {
        cell.room = this;
        cells.Add(cell);

    }

    public void Assimlate(MazeRoom Room)
    {
        for (int i = 0; i < Room.cells.Count; i++)
        {
            Add(Room.cells[i]);

        }

    }

    public void Show()
    {
        for (int i = 0 ; i < cells.Count ; i ++)
        {

            cells[i].Show();
        }
    }

    public void Hide()
    {

        for (int i = 0; i < cells.Count; i++)
        {

            cells[i].Hide();
        }

    }

}

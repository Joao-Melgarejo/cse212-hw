/// <summary>
/// Defines a maze using a dictionary. The dictionary is provided by the
/// user when the Maze object is created. The dictionary will contain the
/// following mapping:
///
/// (x,y) : [left, right, up, down]
///
/// 'x' and 'y' are integers and represents locations in the maze.
/// 'left', 'right', 'up', and 'down' are boolean are represent valid directions
///
/// If a direction is false, then we can assume there is a wall in that direction.
/// If a direction is true, then we can proceed.  
///
/// If there is a wall, then throw an InvalidOperationException with the message "Can't go that way!".  If there is no wall,
/// then the 'currX' and 'currY' values should be changed.
/// </summary>
public class Maze
{
    private readonly Dictionary<ValueTuple<int, int>, bool[]> _mazeMap;
    private int _currX = 1;
    private int _currY = 1;

    public Maze(Dictionary<ValueTuple<int, int>, bool[]> mazeMap)
    {
        _mazeMap = mazeMap;
    }

    // TODO Problem 4 - ADD YOUR CODE HERE
    // PLAN (written before implementing):
    // The maze is a Map: the key is the (x, y) coordinate and the value is a bool[4]
    // that says which walls are open, in the fixed order [left, right, up, down].
    // Looking up the current cell is O(1) because the tuple key is hashed - we never
    // have to search a grid.
    //
    // Index convention (this is the contract of the data, so it must not be guessed):
    //   [0] = left  -> x - 1
    //   [1] = right -> x + 1
    //   [2] = up    -> y - 1
    //   [3] = down  -> y + 1
    // Note that "up" DECREASES y: the origin (1,1) is the top-left corner, like rows on
    // a screen, not like a math graph. The test walks from (1,1) to (6,6) and only lands
    // there with this convention.
    //
    // Every move is the same three steps, so instead of copy-pasting the logic four
    // times I wrote one private helper, Move(directionIndex, deltaX, deltaY):
    // 1. Look up the current cell with TryGetValue. If the coordinate is not in the map
    //    at all, we are off the board => that is also "Can't go that way!". Using
    //    TryGetValue instead of the [] indexer avoids a KeyNotFoundException here.
    // 2. Check the flag for the requested direction (and check the array is long enough,
    //    defensively). If it is false, there is a wall => throw
    //    InvalidOperationException with the EXACT message "Can't go that way!", because
    //    the test compares the message text.
    // 3. Otherwise update _currX / _currY by the delta.
    // Edge case: a failed move must leave the position UNCHANGED. That is why the
    // exception is thrown before any assignment happens.
    // Performance: O(1) per move (one hash lookup and one comparison).

    /// <summary>
    /// Check to see if you can move left.  If you can, then move.  If you
    /// can't move, throw an InvalidOperationException with the message "Can't go that way!".
    /// </summary>
    public void MoveLeft()
    {
        Move(0, -1, 0);
    }

    /// <summary>
    /// Check to see if you can move right.  If you can, then move.  If you
    /// can't move, throw an InvalidOperationException with the message "Can't go that way!".
    /// </summary>
    public void MoveRight()
    {
        Move(1, +1, 0);
    }

    /// <summary>
    /// Check to see if you can move up.  If you can, then move.  If you
    /// can't move, throw an InvalidOperationException with the message "Can't go that way!".
    /// </summary>
    public void MoveUp()
    {
        Move(2, 0, -1);
    }

    /// <summary>
    /// Check to see if you can move down.  If you can, then move.  If you
    /// can't move, throw an InvalidOperationException with the message "Can't go that way!".
    /// </summary>
    public void MoveDown()
    {
        Move(3, 0, +1);
    }

    /// <summary>
    /// Shared helper for the four moves.
    /// </summary>
    /// <param name="direction">Index into the bool[] of the current cell: 0=left, 1=right, 2=up, 3=down</param>
    /// <param name="deltaX">How much to change x when the move is allowed</param>
    /// <param name="deltaY">How much to change y when the move is allowed</param>
    private void Move(int direction, int deltaX, int deltaY)
    {
        // O(1) lookup of the current cell. If the coordinate is not on the map,
        // there is nowhere to go.
        if (!_mazeMap.TryGetValue((_currX, _currY), out var openDirections)
            || direction >= openDirections.Length
            || !openDirections[direction])
        {
            throw new InvalidOperationException("Can't go that way!");
        }

        _currX += deltaX;
        _currY += deltaY;
    }

    public string GetStatus()
    {
        return $"Current location (x={_currX}, y={_currY})";
    }
}
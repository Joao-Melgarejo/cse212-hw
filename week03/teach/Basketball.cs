/*
 * CSE 212 Lesson 6C 
 * 
 * This code will analyze the NBA basketball data and create a table showing
 * the players with the top 10 career points.
 * 
 * Note about columns:
 * - Player ID is in column 0
 * - Points is in column 8
 * 
 * Each row represents the player's stats for a single season with a single team.
 */

using Microsoft.VisualBasic.FileIO;

public class Basketball
{
    // PLAN (written before implementing):
    // The file has one row per player PER SEASON, so the same playerId shows up many
    // times. We want the career total, i.e. one number per player.
    //
    // Step 1 - Build a summary table with a Map (Dictionary<string, int>):
    //   key   = playerId (unique, which is exactly what a map key must be)
    //   value = the running total of points for that player
    //   For every row: if the key already exists, add this season's points to the
    //   stored total; if it does not, create it with this season's points as the
    //   starting value. Both the lookup and the update are O(1) because the key is
    //   hashed, so the whole file costs O(r) with r = number of rows.
    //   A list would have forced a linear search per row => O(r * p). The map is the
    //   right structure precisely because we need repeated lookup by name.
    //
    // Step 2 - Get the top 10:
    //   A map has no order, so to rank we must dump it into something orderable.
    //   players.ToArray() gives a KeyValuePair<string,int>[] (O(p)), and Array.Sort
    //   with a custom comparison sorts it DESCENDING by points (O(p log p)).
    //   Comparison detail: p2.Value.CompareTo(p1.Value) is used instead of
    //   p2.Value - p1.Value; the subtraction can overflow with extreme int values,
    //   CompareTo never does.
    //
    // Step 3 - Print the first 10 entries, guarding with Math.Min in case the file
    //   ever had fewer than 10 players.
    //
    // Overall performance: O(r + p log p), dominated by reading the file.
    public static void Run()
    {
        var players = new Dictionary<string, int>();

        using var reader = new TextFieldParser("basketball.csv");
        reader.TextFieldType = FieldType.Delimited;
        reader.SetDelimiters(",");
        reader.ReadFields(); // ignore header row
        while (!reader.EndOfData) {
            var fields = reader.ReadFields()!;
            var playerId = fields[0];
            var points = int.Parse(fields[8]);

            // Summary table: accumulate the points of every season for this player.
            if (players.ContainsKey(playerId))
                players[playerId] += points;
            else
                players[playerId] = points;
        }

        // Console.WriteLine($"Players: {{{string.Join(", ", players)}}}");

        // A map is not ordered, so copy it into an array and sort that array
        // descending by total points.
        var topPlayers = players.ToArray();
        Array.Sort(topPlayers, (p1, p2) => p2.Value.CompareTo(p1.Value));

        Console.WriteLine();
        for (var i = 0; i < Math.Min(10, topPlayers.Length); ++i)
        {
            Console.WriteLine(topPlayers[i]);
        }
    }
}
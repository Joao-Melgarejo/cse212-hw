using System.Text.Json;

public static class SetsAndMaps
{
    /// <summary>
    /// The words parameter contains a list of two character 
    /// words (lower case, no duplicates). Using sets, find an O(n) 
    /// solution for returning all symmetric pairs of words.  
    ///
    /// For example, if words was: [am, at, ma, if, fi], we would return :
    ///
    /// ["am & ma", "if & fi"]
    ///
    /// The order of the array does not matter, nor does the order of the specific words in each string in the array.
    /// at would not be returned because ta is not in the list of words.
    ///
    /// As a special case, if the letters are the same (example: 'aa') then
    /// it would not match anything else (remember the assumption above
    /// that there were no duplicates) and therefore should not be returned.
    /// </summary>
    /// <param name="words">An array of 2-character words (lowercase, no duplicates)</param>
    // TODO Problem 1 - ADD YOUR CODE HERE
    // PLAN (written before implementing):
    // Naive approach: for every word, scan the whole list looking for its reverse.
    // That is a loop inside a loop => O(n^2). Too slow for the efficiency test.
    //
    // Key idea: the partner of a word is not something we have to SEARCH for, it is
    // something we can COMPUTE: the partner of "am" can only ever be "ma". So the only
    // question left is "is that partner already present?", and a set answers exactly
    // that question in O(1) because it hashes the value instead of scanning it.
    //
    // What goes inside the set: the two letters of the word as a tuple (char, char)
    // instead of the word as a string. A tuple is a value type, so asking
    // seen.Contains((word[1], word[0])) costs nothing extra, while building the
    // reversed STRING would allocate a brand new string for every word in the list.
    // With one million words that allocation is what makes the O(n) version fail the
    // efficiency test, so the reversed string is only built when a pair really matches.
    //
    // Steps:
    // 1. Create an empty HashSet<(char, char)> named "seen" and a List<string> for results.
    // 2. Walk the array once (=> O(n)). For the current word:
    //    a. Special case: if word[0] == word[1] (like "aa"), the word is its own reverse.
    //       Since the input has no duplicates, it can never have a partner, so skip it
    //       WITHOUT adding it to the set. That also prevents "aa & aa" from appearing.
    //    b. If "seen" contains (word[1], word[0]) then the partner appeared EARLIER in
    //       the array, so record the pair now as "word & reversedWord".
    //    c. Add (word[0], word[1]) to "seen" so later words can find this one.
    // 3. Return the list as an array.
    //
    // Why each pair is reported exactly once: the pair is only recorded when the SECOND
    // member is reached. "am" (partner not seen yet) stays quiet and "ma" records it, so
    // "am & ma" and "ma & am" can never both fire.
    // Edge cases covered: empty array (the loop never runs => []); no matching pair
    // ("ab","ac" => []); same-letter word ("aa" => skipped); a word whose partner is
    // missing ("at" without "ta" => not returned).
    // Assumption taken from the problem statement: every word is exactly 2 characters
    // long and lower case, and the list has no duplicates.
    // Performance: n words x O(1) work each = O(n) time, O(n) memory for the set.
    public static string[] FindPairs(string[] words)
    {
        var seen = new HashSet<(char, char)>();
        var pairs = new List<string>();

        foreach (var word in words)
        {
            // A word made of the same letter ("aa") is its own reverse and, since the
            // list has no duplicates, it can never have a partner. Skip it entirely.
            if (word[0] == word[1])
                continue;

            // The partner of "am" can only be "ma": ask the set for it in O(1).
            if (seen.Contains((word[1], word[0])))
                pairs.Add($"{word} & {word[1]}{word[0]}");

            seen.Add((word[0], word[1]));
        }

        return pairs.ToArray();
    }

    /// <summary>
    /// Read a census file and summarize the degrees (education)
    /// earned by those contained in the file.  The summary
    /// should be stored in a dictionary where the key is the
    /// degree earned and the value is the number of people that 
    /// have earned that degree.  The degree information is in
    /// the 4th column of the file.  There is no header row in the
    /// file.
    /// </summary>
    /// <param name="filename">The name of the file to read</param>
    /// <returns>fixed array of divisors</returns>
    // PLAN (written before implementing):
    // This is the classic "summary table" pattern from the Maps reading: one pass over
    // the data, a Dictionary that maps each distinct value to how many times it appeared.
    // 1. The degree is the 4th column => index 3 (indexes start at 0).
    // 2. Guard: if a line does not have at least 4 fields (for example a blank line at
    //    the end of the file), skip it instead of crashing with IndexOutOfRangeException.
    // 3. If the degree is not a key yet, create it with the value 1.
    //    If it is already a key, add 1 to the value it already has.
    //    ContainsKey and the indexer are both O(1) thanks to hashing.
    // 4. Return the dictionary.
    // A List would have required searching for the degree on every single line
    // (O(rows * degrees)); the map turns that search into a hash lookup.
    // Performance: O(r) with r = number of lines. Memory: O(d), d = distinct degrees (16).
    public static Dictionary<string, int> SummarizeDegrees(string filename)
    {
        var degrees = new Dictionary<string, int>();
        foreach (var line in File.ReadLines(filename))
        {
            var fields = line.Split(",");
            // TODO Problem 2 - ADD YOUR CODE HERE
            if (fields.Length < 4)
                continue; // defensive: ignore blank or malformed lines

            var degree = fields[3];

            if (degrees.ContainsKey(degree))
                degrees[degree] += 1;
            else
                degrees[degree] = 1;
        }

        return degrees;
    }

    /// <summary>
    /// Determine if 'word1' and 'word2' are anagrams.  An anagram
    /// is when the same letters in a word are re-organized into a 
    /// new word.  A dictionary is used to solve the problem.
    /// 
    /// Examples:
    /// is_anagram("CAT","ACT") would return true
    /// is_anagram("DOG","GOOD") would return false because GOOD has 2 O's
    /// 
    /// Important Note: When determining if two words are anagrams, you
    /// should ignore any spaces.  You should also ignore cases.  For 
    /// example, 'Ab' and 'Ba' should be considered anagrams.
    /// 
    /// Reminder: You can access a letter by index in a string by 
    /// using the [] notation.
    /// </summary>
    // TODO Problem 3 - ADD YOUR CODE HERE
    // PLAN (written before implementing):
    // Two words are anagrams when they use exactly the same letters the same number of
    // times. Sorting both strings and comparing them would work but costs O(n log n).
    // A Map gives us O(n): the key is a letter, the value is how many times it appears.
    //
    // 1. Normalize every letter before using it: skip spaces, and lower case it, because
    //    the assignment says to ignore spaces and to ignore case.
    // 2. First pass over word1: build the counting table (letter -> count), +1 per letter.
    // 3. Second pass over word2: for each letter, subtract 1.
    //    - If the letter is not a key at all, word2 has a letter word1 never had => false.
    //    - If its count is already 0, word2 uses that letter more times => false.
    //    Bailing out early here is what makes "DOG" vs "GOOD" fail on the second 'O'.
    // 4. After the second pass every count must be exactly 0. Instead of walking the
    //    whole table again, keep a running counter "remaining": +1 for each letter of
    //    word1 and -1 for each matched letter of word2. remaining == 0 at the end means
    //    both words had the same number of non-space letters. That is what catches
    //    "AABBCCDD" vs "ABCD", where word1 still has leftovers.
    // Edge cases: two empty strings => true (nothing to compare); different lengths once
    // spaces are removed => caught by "remaining"; "tom marvolo riddle" vs
    // "i am lord voldemort" => true because spaces are ignored.
    // Performance: one pass per word with O(1) work per letter => O(n) time; memory is
    // O(k) where k is the number of DISTINCT letters (small and bounded).
    public static bool IsAnagram(string word1, string word2)
    {
        var letterCounts = new Dictionary<char, int>();
        var remaining = 0;

        // Pass 1: count the letters of word1.
        for (var i = 0; i < word1.Length; i++)
        {
            var letter = word1[i];
            if (letter == ' ')
                continue; // spaces are ignored

            letter = ToLower(letter);

            if (letterCounts.TryGetValue(letter, out var count))
                letterCounts[letter] = count + 1;
            else
                letterCounts[letter] = 1;

            remaining++;
        }

        // Pass 2: spend the letters of word2 against that table.
        for (var i = 0; i < word2.Length; i++)
        {
            var letter = word2[i];
            if (letter == ' ')
                continue;

            letter = ToLower(letter);

            // Not present at all, or already used up => word2 has a letter that word1
            // does not have, or has it more times than word1 does.
            if (!letterCounts.TryGetValue(letter, out var count) || count == 0)
                return false;

            letterCounts[letter] = count - 1;
            remaining--;
        }

        // Anything left over means word1 had letters that word2 never used.
        return remaining == 0;
    }

    /// <summary>
    /// Lower case table for the first 256 characters, built once when the class is first
    /// used. char.ToLowerInvariant is a full Unicode operation and is surprisingly
    /// expensive when it is called tens of millions of times inside a loop (the
    /// efficiency test feeds IsAnagram two 60-million-character strings). Precomputing
    /// the answer for every Latin-1 character turns that call into a plain array lookup,
    /// while still handling accented letters such as 'N' with a tilde correctly.
    /// </summary>
    private static readonly char[] LowerLatin1 = BuildLowerLatin1();

    private static char[] BuildLowerLatin1()
    {
        var table = new char[256];
        for (var i = 0; i < table.Length; i++)
            table[i] = char.ToLowerInvariant((char)i);

        return table;
    }

    /// <summary>
    /// Same result as char.ToLowerInvariant, but characters below 256 are answered from
    /// the precomputed table. Characters above that range (rare here) fall back to the
    /// framework method, so behaviour is unchanged.
    /// </summary>
    private static char ToLower(char letter)
    {
        return letter < LowerLatin1.Length ? LowerLatin1[letter] : char.ToLowerInvariant(letter);
    }

    /// <summary>
    /// This function will read JSON (Javascript Object Notation) data from the 
    /// United States Geological Service (USGS) consisting of earthquake data.
    /// The data will include all earthquakes in the current day.
    /// 
    /// JSON data is organized into a dictionary. After reading the data using
    /// the built-in HTTP client library, this function will return a list of all
    /// earthquake locations ('place' attribute) and magnitudes ('mag' attribute).
    /// Additional information about the format of the JSON data can be found 
    /// at this website:  
    /// 
    /// https://earthquake.usgs.gov/earthquakes/feed/v1.0/geojson.php
    /// 
    /// </summary>
    public static string[] EarthquakeDailySummary()
    {
        const string uri = "https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_day.geojson";
        using var client = new HttpClient();
        using var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        using var jsonStream = client.Send(getRequestMessage).Content.ReadAsStream();
        using var reader = new StreamReader(jsonStream);
        var json = reader.ReadToEnd();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json, options);

        // TODO Problem 5:
        // 1. Add code in FeatureCollection.cs to describe the JSON using classes and properties 
        // on those classes so that the call to Deserialize above works properly.
        // 2. Add code below to create a string out each place a earthquake has happened today and its magitude.
        // 3. Return an array of these string descriptions.
        //
        // PLAN (written before implementing):
        // The USGS feed is a map of maps. Its shape is:
        //   { "type": ..., "metadata": {...}, "features": [ { "properties": { "mag": 2.36,
        //     "place": "1km NE of Pahala, Hawaii", ... }, ... }, ... ] }
        // An object in C# IS a map whose keys are fixed at compile time, so I mirror that
        // shape with three classes (see FeatureCollection.cs):
        //   FeatureCollection -> Feature[] Features
        //   Feature           -> Properties Properties
        //   Properties        -> string Place, double? Mag
        // Deserialize is already called above with PropertyNameCaseInsensitive = true,
        // so the lowercase JSON keys ("features", "properties", "place", "mag") match the
        // PascalCase C# property names without extra attributes.
        // Then: walk the Features array and build one string per earthquake with the exact
        // format the assignment shows: "<place> - Mag <mag>".
        // Edge cases: "mag" can be null in the real feed, which is why Mag is double?
        // (a plain double would crash on null); if the feed ever returned no features,
        // the guard below returns an empty array instead of a NullReferenceException.
        // Performance: O(f) with f = number of earthquakes, plus the network call.
        if (featureCollection?.Features == null)
            return [];

        var summaries = new List<string>();
        foreach (var feature in featureCollection.Features)
        {
            summaries.Add($"{feature.Properties.Place} - Mag {feature.Properties.Mag}");
        }

        return summaries.ToArray();
    }
}
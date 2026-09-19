public class Translator
{
    public static void Run()
    {
        var englishToGerman = new Translator();
        englishToGerman.AddWord("House", "Haus");
        englishToGerman.AddWord("Car", "Auto");
        englishToGerman.AddWord("Plane", "Flugzeug");
        Console.WriteLine(englishToGerman.Translate("Car")); // Auto
        Console.WriteLine(englishToGerman.Translate("Plane")); // Flugzeug
        Console.WriteLine(englishToGerman.Translate("Train")); // ???
    }

    private Dictionary<string, string> _words = new();

    /// <summary>
    /// Add the translation from 'from_word' to 'to_word'
    /// For example, in a english to german dictionary:
    /// 
    /// my_translator.AddWord("book","buch")
    /// </summary>
    /// <param name="fromWord">The word to translate from</param>
    /// <param name="toWord">The word to translate to</param>
    /// <returns>fixed array of divisors</returns>
    // PLAN (written before implementing):
    // A map stores key -> value pairs. Here the key is the word in the original
    // language and the value is its translation.
    // Using the indexer (_words[key] = value) instead of Add(key, value) means that
    // adding the same word twice overwrites the old translation instead of throwing
    // an ArgumentException. That matches the assumption "one translation per word".
    // Performance: O(1), the cost of hashing the key.
    public void AddWord(string fromWord, string toWord)
    {
        _words[fromWord] = toWord;
    }

    /// <summary>
    /// Translates the from word into the word that this stores as the translation
    /// </summary>
    /// <param name="fromWord">The word to translate</param>
    /// <returns>The translated word or "???" if no translation is available</returns>
    // PLAN (written before implementing):
    // 1. Ask the map whether it holds that key (TryGetValue does the lookup and gives
    //    back the value in a single hashing operation).
    // 2. If it does, return the stored translation.
    // 3. If it does not, return "???" instead of letting the lookup throw a
    //    KeyNotFoundException. Never read a key with _words[key] without checking first.
    // Performance: O(1), the cost of hashing the key.
    public string Translate(string fromWord)
    {
        if (_words.TryGetValue(fromWord, out var translation))
            return translation;

        return "???";
    }
}
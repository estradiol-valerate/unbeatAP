using System.Collections.Generic;

namespace UNBEATAP.Helpers;

public static class CharacterList
{
    private static List<string> characters = new List<string>();


    public static void AddCharacter(string character)
    {
        characters.Add(character.ToLower());
    }


    public static List<string> GetCharacters()
    {
        return characters;
    }
}
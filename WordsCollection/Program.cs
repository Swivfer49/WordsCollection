using WordsCollection.Classes;

namespace WordsCollection
{
    internal class Program
    {
        static List<WordItem> words = new List<WordItem>();
        static void Main(string[] args)
        {
            string name = "WordCollection";
            while (true)
            {
                name = MenuLoop(name);
                if (name == "") break;
            }
        }

        static string MenuLoop(string fileName)
        {
            string ReturnName = "";

            string filePath = $"./{fileName}.txt";

            //tries to load the file
            //if there is no existing file
            //then there will be no words loaded
            //and, when saved, will create a new file anyways
            bool fileExists = LoadWords(filePath);

            Console.WriteLine("|E-SALT: word edition| version 0.7 |");

            if (fileExists)
            {
                Console.WriteLine($"Loaded existing file at [{filePath}]");
            }
            else
            {
                Console.WriteLine($"No file exists at [{filePath}], no words loaded");
                Console.WriteLine("A new file will be created upon saving");
            }

            while (true)
            {
                ColorConsole.WriteColor($"<{fileName}>",ConsoleColor.DarkMagenta);
                string input = Console.ReadLine()!;

                if (input == "") continue;
                else if (input == "help")
                {
                    Help();
                }
                else if (input == "save")
                {
                    SaveWords(filePath);
                }
                else if (input == "exit")
                {
                    Console.WriteLine("Would you like to save? (y/n)");
                    bool validReply = false;
                    while (!validReply)
                    {
                        char c = Console.ReadKey().KeyChar;
                        if (c == 'y')
                        {
                            SaveWords(filePath);
                            validReply = true;
                        }
                        else if(c == 'n')
                            validReply = true;
                    }
                    break;
                }
                else if (input == "clear")
                {
                    Console.Clear();
                }
                else if(input == "showtags")
                {
                    Tag.WriteTag(Tag.Tags.ToArray());
                    Console.WriteLine();
                }
                else if(input.StartsWith("findwords "))
                {
                    List<WordItem> filteredWords = words.ToList();
                    string selector = RemoveStartingString(input, "findwords ").Trim();
                    bool hidetags = false;

                    if(selector.StartsWith("hidetags "))
                    {
                        selector = RemoveStartingString(selector, "hidetags ");
                        hidetags = true;
                    }
                    if(selector == "")
                    {
                        ColorConsole.WriteError("No Selector Specified");
                        continue;
                    }
                    else if(selector.StartsWith("startswith "))
                    {
                        string requirement = RemoveStartingString(selector, "startswith ").Trim();
                        if (requirement == "")
                        {
                            ColorConsole.WriteError("Invalid selector");
                            continue;
                        }
                        filteredWords = filteredWords.Where(word=>word.word.StartsWith(requirement)).ToList();
                    }
                    else if (selector.StartsWith("contains "))
                    {
                        string requirement = RemoveStartingString(selector, "contains ").Trim();
                        if (requirement == "")
                        {
                            ColorConsole.WriteError("Invalid selector");
                            continue;
                        }
                        filteredWords = filteredWords.Where(word => word.word.Contains(requirement)).ToList();
                    }
                    else if (selector.StartsWith("endswith "))
                    {
                        string requirement = RemoveStartingString(selector, "endswith ").Trim();
                        if(requirement == "")
                        {
                            ColorConsole.WriteError("Invalid selector");
                            continue;
                        }
                        filteredWords = filteredWords.Where(word => word.word.EndsWith(requirement)).ToList();
                    }
                    else if(selector.StartsWith("withtags "))
                    {
                        string requirement = RemoveStartingString(selector, "withtags ").Trim();
                        if (requirement == "")
                        {
                            ColorConsole.WriteError("Invalid selector");
                            continue;
                        }
                        Tag[] tags = Tag.StringToTags(requirement);
                        if(tags.Length == 0)
                        {
                            ColorConsole.WriteError("No valid tags listed");
                            continue;
                        }
                        filteredWords = filteredWords.Where(
                            word => tags.All(
                                tag=>word.tagIds.Contains(tag.Id)
                            )
                        ).ToList();
                    }
                    else if (selector.StartsWith("withouttags "))
                    {
                        string requirement = RemoveStartingString(selector, "withouttags ").Trim();
                        if (requirement == "")
                        {
                            ColorConsole.WriteError("Invalid selector");
                            continue;
                        }
                        Tag[] tags = Tag.StringToTags(requirement);
                        if (tags.Length == 0)
                        {
                            ColorConsole.WriteError("No valid tags listed");
                            continue;
                        }
                        filteredWords = filteredWords.Where(
                            word => !tags.Any(
                                tag => word.tagIds.Contains(tag.Id)
                            )
                        ).ToList();
                    }
                    else if (selector == "all") { }
                    else
                    {
                        ColorConsole.WriteError("Invalid selector");
                        continue;
                    }


                    if (hidetags)
                    {
                        foreach (WordItem item in filteredWords)
                        {
                            Console.WriteLine(item.word);
                        }
                    }
                    else
                    {
                        foreach (WordItem item in filteredWords)
                        {
                            item.WriteWord();
                            Console.WriteLine();
                        }
                    }
                }
                else if(input.StartsWith("findword "))
                {
                    string wordName = RemoveStartingString(input, "findword ").Trim();
                    WordItem? wordInQuestion = GetWord(wordName);
                    if(wordInQuestion == null)
                    {
                        Console.WriteLine($"Word: [{wordName}] does not exist");
                        continue;
                    }
                    wordInQuestion.WriteWord();
                    Console.WriteLine();
                }
                else if (input.StartsWith("createword ")){
                    WordItem wordToAdd = new WordItem(input.Substring(11));
                    if (wordToAdd.word.Length == 0)
                        continue;
                    words.Add(wordToAdd);
                }
                else if(input.StartsWith("createtag "))
                {
                    Tag tagToAdd = new Tag(input.Substring(10));
                    if(tagToAdd.Name.Length == 0)
                        continue;
                    Tag.Tags.Add(tagToAdd);
                }
                else if (input.StartsWith("removeword "))
                {
                    string wordName = RemoveStartingString(input, "removeword ");
                    WordItem? wordInQuestion = GetWord(wordName);
                    if(wordInQuestion == null)
                    {
                        ColorConsole.InvalidWordName(wordName);
                    }
                    else
                    {
                        words.Remove(wordInQuestion);
                        Console.WriteLine($"Removal of \"{wordName}\" is complete");
                    }
                }
                else if(input.StartsWith("renameword "))
                {
                    string withoutStart = RemoveStartingString(input, "renameword ");
                    (string oldName, string newName) = SplitOnce(withoutStart, " to ");
                    WordItem? wordInQuestion = GetWord(oldName);
                    if(wordInQuestion == null)
                    {
                        ColorConsole.InvalidWordName(oldName);
                    }
                    else
                    {
                        if(newName == "")
                        {
                            ColorConsole.InvalidCommand();
                        }
                        else
                        {
                            wordInQuestion.word = newName;
                            Console.WriteLine($"\"{oldName}\" renamed to \"{newName}\"");
                        }
                    }
                }
                else if(input.StartsWith("renametag "))
                {
                    string withoutStart = RemoveStartingString(input, "renametag ");
                    (string oldName, string newName) = SplitOnce(withoutStart, " to ");
                    Tag? tagInQuestion = Tag.GetTag(oldName);
                    if (tagInQuestion == null)
                    {
                        ColorConsole.InvalidWordName(oldName);
                    }
                    else
                    {
                        if (newName == "")
                        {
                            ColorConsole.InvalidCommand();
                        }
                        else
                        {
                            tagInQuestion.Name = newName;
                            Console.WriteLine($"\"{oldName}\" renamed to \"{newName}\"");
                        }
                    }
                }
                else if(input.StartsWith("color "))
                {
                    string withoutStart = RemoveStartingString(input, "color ").Trim();
                    (string tagName, string colorName) = SplitOnce(withoutStart, " ");
                    if(tagName == "" || colorName == "")
                    {
                        ColorConsole.InvalidCommand();
                    }
                    else
                    {
                        Tag? tagInQuestion = Tag.GetTag(tagName);

                        if(Enum.TryParse<ConsoleColor>(colorName,true, out ConsoleColor color) && tagInQuestion != null)
                        {
                            tagInQuestion.Color = color;
                        }
                        else
                        {
                            ColorConsole.InvalidCommand();
                        }
                    }
                }
                else if(input.StartsWith("deletetag "))
                {
                    string tagName = RemoveStartingString(input, "deletetag ").Trim();
                    Tag? tagInQuestion = Tag.GetTag(tagName);
                    if(tagInQuestion == null)
                    {
                        ColorConsole.InvalidTagName(tagName);
                    }
                    else
                    {
                        RemoveTagsFromAllWords(new Tag[] {tagInQuestion});
                        Tag.Tags.Remove(tagInQuestion);
                        Console.WriteLine($"Tag \"{tagName}\" no longer exists");
                    }
                }
                else if (input.StartsWith("deletetags "))
                {
                    string tagList = RemoveStartingString(input, "deletetag ").Trim();
                    Tag[] tags = Tag.StringToTags(tagList);

                    if (tags.Length == 0)
                    {
                        ColorConsole.NoValidTags();
                    }
                    else
                    {
                        RemoveTagsFromAllWords(tags);
                        foreach(Tag tag in tags )
                        {
                            Console.Write("Removed tag: ");
                            tag.WriteTag();
                            Console.WriteLine();
                            Tag.Tags.Remove(tag);
                        }
                    }
                }
                else if(input.StartsWith("addtags "))
                {
                    (string tagsString, string wordName) = SplitOnce(
                        RemoveStartingString(input, "addtag "),
                        " to "
                    );
                    if(tagsString == "" || wordName == "")
                    {
                        ColorConsole.InvalidCommand();
                        continue;
                    }
                    Tag[] tagsToAdd = Tag.StringToTags(tagsString);
                    if(tagsToAdd.Length == 0)
                    {
                        ColorConsole.NoValidTags();
                        continue;
                    }
                    WordItem? wordInQuestion = GetWord(wordName);
                    if(wordInQuestion == null)
                    {
                        ColorConsole.InvalidWordName(wordName);
                        continue;
                    }
                    HashSet<int> tagIds = tagsToAdd.Select(tag=>tag.Id).ToHashSet();
                    wordInQuestion.tagIds = tagIds.Concat(wordInQuestion.tagIds).ToArray();
                    wordInQuestion.WriteWord();
                    Console.WriteLine();
                }
                else if (input.StartsWith("forcetags "))
                {
                    (string tagsString, string wordName) = SplitOnce(
                        RemoveStartingString(input, "forcetags "),
                        " on "
                    );
                    if (tagsString == "" || wordName == "")
                    {
                        ColorConsole.InvalidCommand();
                        continue;
                    }
                    Tag[] tagsToAdd = Tag.StringToTags(tagsString);
                    if (tagsToAdd.Length == 0)
                    {
                        ColorConsole.NoValidTags();
                        continue;
                    }
                    WordItem? wordInQuestion = GetWord(wordName);
                    if (wordInQuestion == null)
                    {
                        ColorConsole.InvalidWordName(wordName);
                        continue;
                    }
                    HashSet<int> tagIds = tagsToAdd.Select(tag => tag.Id).ToHashSet();
                    wordInQuestion.tagIds = tagIds.ToArray();
                    wordInQuestion.WriteWord();
                    Console.WriteLine();
                }
                else if(input.StartsWith("switchfile "))
                {
                    string newName = RemoveStartingString(input, "switchfile ").Trim();
                    if (newName.IndexOfAny(Path.GetInvalidFileNameChars()) == -1)
                    {
                        ReturnName = newName;
                        Console.WriteLine("Would you like to save? (y/n)");
                        bool validReply = false;
                        while (!validReply)
                        {
                            char c = Console.ReadKey().KeyChar;
                            if (c == 'y')
                            {
                                SaveWords(filePath);
                                validReply = true;
                            }
                            else if (c == 'n')
                                validReply = true;
                        }
                        words = new();
                        Tag.Tags = new();
                        break;
                    }
                    else
                    {
                        ColorConsole.InvalidPath(newName);
                    }
                }
                else if (input.StartsWith("removetags "))
                {
                    (string tagsString, string wordName) = SplitOnce(
                        RemoveStartingString(input, "removetag "),
                        " from "
                    );
                    if (tagsString == "" || wordName == "")
                    {
                        ColorConsole.InvalidCommand();
                        continue;
                    }
                    Tag[] tagsToRemove = Tag.StringToTags(tagsString);
                    if (tagsToRemove.Length == 0)
                    {
                        ColorConsole.NoValidTags();
                        continue;
                    }
                    WordItem? wordInQuestion = GetWord(wordName);
                    if (wordInQuestion == null)
                    {
                        ColorConsole.InvalidWordName(wordName);
                        continue;
                    }
                    HashSet<int> tagIds = tagsToRemove.Select(tag => tag.Id).ToHashSet();
                    wordInQuestion.tagIds = wordInQuestion.tagIds.Where(id=>!tagIds.Contains(id)).ToArray();
                    wordInQuestion.WriteWord();
                    Console.WriteLine();
                }

            }

            return ReturnName;

        }

        public static (string,string) SplitOnce(string line, string seperator) {

            int index = line.IndexOf(seperator);
            if (index == -1)
            {
                return ("", "");
            }
            string first = line.Substring(0, index).Trim();
            string second = line.Substring(index + seperator.Length).Trim();
            return (first, second);
        
        }

        public static string RemoveStartingString(string line, string startsWith)
        {
            return line.Substring(startsWith.Length);
        }
        
        public static void RemoveTagsFromAllWords(Tag[] tagsToRemove)
        {
            foreach(WordItem word in words)
            {
                List<int> tagIds = word.tagIds.ToList();
                foreach(Tag tag in tagsToRemove)
                {
                    if(tagIds.Contains(tag.Id))
                        tagIds.Remove(tag.Id);
                }
                word.tagIds = tagIds.ToArray();
            }
        }

        public static WordItem? GetWord(string name)
        {
            return words.FirstOrDefault(word => word.word == name);
        }

        static void Help()
        {

            Console.WriteLine("\n|Utility Commands|");
            Console.WriteLine("<>help -> details all commands");
            Console.WriteLine("<>save -> saves all changes to the file");
            Console.WriteLine("<>exit -> exits the program, saving if the user wishes");
            Console.WriteLine("<>clear -> clears the console");
            Console.WriteLine("<>switchfile [Name] -> switches to a new file with the name [Name]");
            Console.WriteLine("<>showtags -> shows all tags");

            Console.WriteLine("\n|Word Alterations|");
            Console.WriteLine("<>createword [WordName] -> adds a new word to the word bank with the name [WordName]");
            Console.WriteLine("<>removeword [WordName] -> removes the word with the name [WordName] if such word exsists");
            Console.WriteLine("<>renameword [WordName] to [NewName] -> renames the word with the name [WordName] to [NewName]");

            Console.WriteLine("\n|Tag Alterations|");
            Console.WriteLine("<>renametag [TagName] to [NewName] -> renames the tag with the name [TagName] to [NewName]");
            Console.WriteLine("<>createtag [TagName] -> creates a new tag named [TagName]");
            Console.WriteLine("<>color [TagName] [Color] -> sets the color of [TagName] to [Color]");
            Console.WriteLine("<>deletetag [TagName] -> deletes [TagName] and removes it from all words");

            Console.WriteLine("\n|Basic Word Searching|");
            Console.WriteLine("<>findwords startswith [string] -> lists all words that start with [string]");
            Console.WriteLine("<>findwords endswith [string] -> lists all words that end with [string]");
            Console.WriteLine("<>findwords contains [string] -> lists all words that contain [string]");
            Console.WriteLine("<>findwords all -> lists all words");
            Console.WriteLine("<>findwords hidetags... -> lists all words that match the selector, but does not include tags in output");
            Console.WriteLine("<>findword [WordName] -> finds the word named [WordName]");

            Console.WriteLine("\n|Word Searching Using Tags|");
            Console.WriteLine("<>findwords withtags [Tag1], [Tag2],... -> lists all words that have all the tags in [Tag1], [Tag2],...");
            Console.WriteLine("<>findwords withouttags [Tag1], [Tag2],... -> lists all words that have none of the tags in [Tag1], [Tag2],...");

            Console.WriteLine("\n|Tagging Words|");
            Console.WriteLine("<>addtags [Tag1], [Tag2],... to [WordName] -> adds all tags in [Tag1], [Tag2],... to the word [WordName]");
            Console.WriteLine("<>removetags [Tag1], [Tag2],... from [WordName] -> removes all tags [Tag1], [Tag2],... from [WordName]");
            Console.WriteLine("<>forcetags [Tag1], [Tag2],... on [WordName] -> sets the tags of [WordName] to the list of tags");

            Console.WriteLine("\n||");
            Console.WriteLine("<>generateword -> ");
            Console.WriteLine("<> -> ");
        }



        static bool LoadWords(string filePath)
        {
            if (File.Exists(filePath))
            {
                StreamReader streamReader = new StreamReader(filePath);

                bool isStillSeeingTags = true;

                while (isStillSeeingTags)
                {

                    string line = streamReader.ReadLine()!;

                    if(line.Trim() == "WordsBegin")
                    {
                        isStillSeeingTags = false;
                    }
                    else
                    {
                        Tag tag = new Tag(line);
                        if (tag.Name.Length > 0)
                            Tag.Tags.Add(tag);
                    }

                }

                while (!streamReader.EndOfStream)
                {

                    string line = streamReader.ReadLine()!;

                    WordItem word = new WordItem(line);

                    if (word.word.Length == 0)
                        continue;
                    words.Add(word);
                   

                }

                streamReader.Close();
                streamReader.Dispose();
                return true;

            }
            else
            {
                return false;
            }
        }

        static void SaveWords(string filePath)
        {
            StreamWriter streamWriter = new StreamWriter(filePath);

            for(int i=0;i<Tag.Tags.Count;i++)
            {
                streamWriter.WriteLine(Tag.Tags[i].ToString());
            }
            streamWriter.WriteLine("WordsBegin");
            for(int j=0;j<words.Count;j++)
            {
                streamWriter.WriteLine(words[j].ToString());
            }
            streamWriter.Close();
            streamWriter.Dispose();
        }
    }
}
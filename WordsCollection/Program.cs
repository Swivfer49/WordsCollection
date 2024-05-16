using WordsCollection.Classes;

namespace WordsCollection
{
    internal class Program
    {
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

            Console.WriteLine("|E-SALT: word edition| version 0.8 |");

            if (fileExists)
            {
                Console.WriteLine($"Loaded existing file at [{filePath}]");
            }
            else
            {
                Console.WriteLine($"No file exists at [{filePath}], no words loaded");
                Console.WriteLine("A new file will be created upon saving");
            }

            bool MayExit = false;

            while (!MayExit)
            {
                ColorConsole.WriteColor($"<{fileName}>",ConsoleColor.DarkMagenta);
                string input = Console.ReadLine()!;

                (string firstWord, string theRest) = SplitAtFirst(input, " ");

                switch (firstWord)
                {
                    case "":
                        continue;
                    case "help":
                        {
                            Help();
                            break;
                        }
                    case "save":
                        {
                            SaveWords(filePath);
                            ColorConsole.Saved();
                            break;
                        }
                    case "exit":
                        {
                            MayExit = Exit(filePath);
                            break;
                        }
                    case "clear":
                        {
                            Console.Clear();
                            break;
                        }
                    case "showtags":
                        {
                            ShowTags();
                            break;
                        }
                    case "findwords":
                        {
                            FindWords(theRest);
                            break;
                        }
                    case "findword":
                        {
                            FindWord(theRest);
                            break;
                        }
                    case "createword":
                        {
                            CreateWord(theRest);
                            break;
                        }
                    case "createtag":
                        {
                            CreateTag(theRest);
                            break;
                        }
                    case "removeword":
                        {
                            RemoveWord(theRest);
                            break;
                        }
                    case "renameword":
                        {
                            RenameWord(theRest);
                            break;
                        }
                    case "renametag":
                        {
                            RenameTag(theRest);
                            break;
                        }
                    case "color":
                        {
                            ColorTag(theRest);
                            break;
                        }
                    case "deletetag":
                        {
                            DeleteTag(theRest);
                            break;
                        }
                    case "deletetags":
                        {
                            DeleteTags(theRest);
                            break;
                        }
                    case "addtags":
                        {
                            AddTags(theRest);
                            break;
                        }
                    case "forcetags":
                        {
                            ForceTags(theRest);
                            break;
                        }
                    case "removetags":
                        {
                            RemoveTags(theRest);
                            break;
                        }
                    case "switchfile":
                        {
                            if(SwitchFile(theRest, filePath, out string? rn))
                            {
                                if(rn != null)
                                {
                                    MayExit = true;
                                    ReturnName = rn;
                                }
                            }
                            break;
                        }
                }

            }

            return ReturnName;

        }

        #region StringHelpers
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

        public static (string, string) SplitAtFirst(string line, string seperator)
        {

            int index = line.IndexOf(seperator);
            if (index == -1)
            {
                return (line, "");
            }
            string first = line.Substring(0, index).Trim();
            string second = line.Substring(index + seperator.Length).Trim();
            return (first, second);

        }

        public static string RemoveStartingString(string line, string startsWith)
        {
            return line.Substring(startsWith.Length);
        }

        #endregion StringHelpers

        #region CommandFunctions

        #region Basic Commands
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
        static bool SwitchFile(string input, string filePath, out string? ReturnName)
        {
            if (input.IndexOfAny(Path.GetInvalidFileNameChars()) == -1)
            {
                ReturnName = input;
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
                WordItem.Items = new();
                Tag.Tags = new();
                return true;
            }

            else
            {
                ColorConsole.InvalidPath(input);
                ReturnName = null;
                return false;
            }
        }
        static bool Exit(string filePath)
        {
            Console.WriteLine("Would you like to save? (y/n)");
            char c = Console.ReadKey().KeyChar;
            if (c == 'y')
            {
                SaveWords(filePath);
                return true;
            }
            else if (c == 'n')
            {
                return true;
            }
            ColorConsole.InvalidInput(c.ToString());
            return false;
        }

        #endregion Basic Commands

        #region Word Commands
        static void CreateWord(string input)
        {
            WordItem wordToAdd = new WordItem(input);
            if (wordToAdd.word.Length == 0)
            {
                ColorConsole.WriteError("Could not create word");
                return;
            }
            WordItem.Items.Add(wordToAdd);
            wordToAdd.WriteWord();
            Console.WriteLine();
        }
        static void FindWords(string input)
        {
            List<WordItem> filteredWords = WordItem.Items.ToList();
            bool hidetags = false;

            (string selectorType, string requirement) = SplitAtFirst(input, " ");

            if(selectorType == "hidetags")
            {
                if(requirement == "")
                {
                    ColorConsole.InvalidCommand();
                    return;
                }
                (selectorType, requirement) = SplitAtFirst(requirement, " ");
                hidetags = true;
            }

            switch (selectorType)
            {
                case "":
                    {
                        ColorConsole.InvalidCommand();
                        return;
                    }
                case "startswith":
                    {
                        if (requirement == "")
                        {
                            ColorConsole.WriteError("Invalid selector");
                            return;
                        }
                        filteredWords = filteredWords.Where(word => word.word.StartsWith(requirement)).ToList();
                        break;
                    }
                case "contains":
                    {
                        if (requirement == "")
                        {
                            ColorConsole.WriteError("Invalid selector");
                            return;
                        }
                        filteredWords = filteredWords.Where(word => word.word.Contains(requirement)).ToList();
                        break;
                    }
                case "endsswith":
                    {
                        if (requirement == "")
                        {
                            ColorConsole.WriteError("Invalid selector");
                            return;
                        }
                        filteredWords = filteredWords.Where(word => word.word.EndsWith(requirement)).ToList();
                        break;
                    }
                case "withtags":
                    {
                        if (requirement == "")
                        {
                            ColorConsole.WriteError("Invalid selector");
                            return;
                        }
                        Tag[] tags = Tag.StringToTags(requirement);
                        if (tags.Length == 0)
                        {
                            ColorConsole.WriteError("No valid tags listed");
                            return;
                        }
                        filteredWords = filteredWords.Where(
                            word => tags.All(
                                tag => word.tagIds.Contains(tag.Id)
                            )
                        ).ToList();
                        break;
                    }
                case "withouttags":
                    {
                        if (requirement == "")
                        {
                            ColorConsole.WriteError("Invalid selector");
                            return;
                        }
                        Tag[] tags = Tag.StringToTags(requirement);
                        if (tags.Length == 0)
                        {
                            ColorConsole.WriteError("No valid tags listed");
                            return;
                        }
                        filteredWords = filteredWords.Where(
                            word => !tags.Any(
                                tag => word.tagIds.Contains(tag.Id)
                            )
                        ).ToList();
                        break;
                    }
                case "all":
                    break;
                default:
                    {
                        ColorConsole.InvalidCommand();
                        break;
                    }
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
        static void FindWord(string input)
        {
            WordItem? wordInQuestion = WordItem.GetWord(input);
            if (wordInQuestion == null)
            {
                Console.WriteLine($"Word: [{input}] does not exist");
                return;
            }
            wordInQuestion.WriteWord();
            Console.WriteLine();
        }
        static void RemoveWord(string input)
        {
            WordItem? wordInQuestion = WordItem.GetWord(input);
            if (wordInQuestion == null)
            {
                ColorConsole.InvalidWordName(input);
            }
            else
            {
                WordItem.Items.Remove(wordInQuestion);
                Console.WriteLine($"Removal of \"{input}\" is complete");
            }
        }
        static void RenameWord(string input)
        {
            (string oldName, string newName) = SplitOnce(input, " to ");
            WordItem? wordInQuestion = WordItem.GetWord(oldName);
            if (wordInQuestion == null)
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
                    wordInQuestion.word = newName;
                    Console.WriteLine($"\"{oldName}\" renamed to \"{newName}\"");
                }
            }
        }

        #endregion Word Commands

        #region Tag Commands
        static void CreateTag(string input)
        {
            Tag tagToAdd = new Tag(input);
            if (tagToAdd.Name.Length == 0)
            {
                ColorConsole.WriteError("Could not create tag");
                return;
            }
            Tag.Tags.Add(tagToAdd);
        }
        static void ShowTags()
        {
            Tag.WriteTag(Tag.Tags.ToArray());
            Console.WriteLine();
        }
        static void RenameTag(string input)
        {
            (string oldName, string newName) = SplitOnce(input, " to ");
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
        static void ColorTag(string input)
        {
            (string tagName, string colorName) = SplitOnce(input, " ");
            if (tagName == "" || colorName == "")
            {
                ColorConsole.InvalidCommand();
            }
            else
            {
                Tag? tagInQuestion = Tag.GetTag(tagName);

                if (Enum.TryParse(colorName, true, out ConsoleColor color) && tagInQuestion != null)
                {
                    tagInQuestion.Color = color;
                }
                else
                {
                    ColorConsole.InvalidCommand();
                }
            }
        }
        static void DeleteTag(string input)
        {
            Tag? tagInQuestion = Tag.GetTag(input);
            if (tagInQuestion == null)
            {
                ColorConsole.InvalidTagName(input);
            }
            else
            {
                WordItem.RemoveTagsFromAllWords(new Tag[] { tagInQuestion });
                Tag.Tags.Remove(tagInQuestion);
                Console.WriteLine($"Tag \"{input}\" no longer exists");
            }
        }
        static void DeleteTags(string input)
        {
            Tag[] tags = Tag.StringToTags(input);

            if (tags.Length == 0)
            {
                ColorConsole.NoValidTags();
            }
            else
            {
                WordItem.RemoveTagsFromAllWords(tags);
                foreach (Tag tag in tags)
                {
                    Console.Write("Removed tag: ");
                    tag.WriteTag();
                    Console.WriteLine();
                    Tag.Tags.Remove(tag);
                }
            }
        }
        static void AddTags(string input)
        {
            (string tagsString, string wordName) = SplitOnce(input, " to ");
            if (tagsString == "" || wordName == "")
            {
                ColorConsole.InvalidCommand();
                return;
            }
            Tag[] tagsToAdd = Tag.StringToTags(tagsString);
            if (tagsToAdd.Length == 0)
            {
                ColorConsole.NoValidTags();
                return;
            }
            WordItem? wordInQuestion = WordItem.GetWord(wordName);
            if (wordInQuestion == null)
            {
                ColorConsole.InvalidWordName(wordName);
                return;
            }
            HashSet<int> tagIds = tagsToAdd.Select(tag => tag.Id).ToHashSet();
            wordInQuestion.tagIds = tagIds.Concat(wordInQuestion.tagIds).ToArray();
            wordInQuestion.WriteWord();
            Console.WriteLine();
        }
        static void ForceTags(string input)
        {
            (string tagsString, string wordName) = SplitOnce(input, " on ");
            if (tagsString == "" || wordName == "")
            {
                ColorConsole.InvalidCommand();
                return;
            }
            Tag[] tagsToAdd = Tag.StringToTags(tagsString);
            if (tagsToAdd.Length == 0)
            {
                ColorConsole.NoValidTags();
                return;
            }
            WordItem? wordInQuestion = WordItem.GetWord(wordName);
            if (wordInQuestion == null)
            {
                ColorConsole.InvalidWordName(wordName);
                return;
            }
            HashSet<int> tagIds = tagsToAdd.Select(tag => tag.Id).ToHashSet();
            wordInQuestion.tagIds = tagIds.ToArray();
            wordInQuestion.WriteWord();
            Console.WriteLine();
        }
        static void RemoveTags(string input)
        {
            (string tagsString, string wordName) = SplitOnce(input, " from ");
            if (tagsString == "" || wordName == "")
            {
                ColorConsole.InvalidCommand();
                return;
            }
            Tag[] tagsToRemove = Tag.StringToTags(tagsString);
            if (tagsToRemove.Length == 0)
            {
                ColorConsole.NoValidTags();
                return;
            }
            WordItem? wordInQuestion = WordItem.GetWord(wordName);
            if (wordInQuestion == null)
            {
                ColorConsole.InvalidWordName(wordName);
                return;
            }
            HashSet<int> tagIds = tagsToRemove.Select(tag => tag.Id).ToHashSet();
            wordInQuestion.tagIds = wordInQuestion.tagIds.Where(id => !tagIds.Contains(id)).ToArray();
            wordInQuestion.WriteWord();
            Console.WriteLine();
        }

        #endregion Tag Commands

        #endregion CommandFunctions

        #region File
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
                    WordItem.Items.Add(word);
                   

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
            for(int j=0;j<WordItem.Items.Count;j++)
            {
                streamWriter.WriteLine(WordItem.Items[j].ToString());
            }
            streamWriter.Close();
            streamWriter.Dispose();
        }
        #endregion File
    }
}
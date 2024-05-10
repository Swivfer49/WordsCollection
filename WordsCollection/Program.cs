using WordsCollection.Classes;

namespace WordsCollection
{
    internal class Program
    {
        static List<WordItem> words = new List<WordItem>();
        static void Main(string[] args)
        {
            if(args.Length == 1)
            {
                MenuLoop(args[0]);
            }
            else
            {
                MenuLoop("WordCollection");
            }
        }

        static void MenuLoop(string fileName)
        {
            string filePath = $"./{fileName}.txt";

            //tries to load the file
            //if there is no existing file
            //then there will be no words loaded
            //and, when saved, will create a new file anyways
            bool fileExists = LoadWords(filePath);

            Console.WriteLine("|E-SALT: word edition| version 0.2 |");

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
                Console.Write("<>");
                string input = Console.ReadLine()!;

                if (input == "") continue;
                if (input == "help")
                {
                    Help();
                    continue;
                }
                if (input == "save")
                {
                    SaveWords(filePath);
                    continue;
                }
                if (input == "exit")
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
                if (input == "clear")
                {
                    Console.Clear();
                }
                if(input == "findwords all")
                {
                    foreach(WordItem item in words)
                    {
                        item.WriteWord();
                        Console.WriteLine();
                    }
                }
                if (input.StartsWith("createword ")){
                    words.Add(new WordItem(input.Substring(11)));
                }
                if(input.StartsWith("createtag "))
                {
                    Tag.Tags.Add(new Tag(input.Substring(10)));
                }
                if (input.StartsWith("removeword "))
                {
                    string wordName = RemoveStartingString(input, "removeword ");
                    WordItem? wordInQuestion = GetWord(wordName);
                    if(wordInQuestion == null)
                    {
                        Console.WriteLine($"Word \"{wordName}\" does not exist");
                    }
                    else
                    {
                        words.Remove(wordInQuestion);
                        Console.WriteLine($"Removal of \"{wordName}\" is complete");
                    }
                }
                if(input.StartsWith("renameword "))
                {
                    string withoutStart = RemoveStartingString(input, "renameword ");
                    (string oldName, string newName) = SplitOnce(withoutStart, "to");
                    WordItem? wordInQuestion = GetWord(oldName);
                    if(wordInQuestion == null)
                    {
                        Console.WriteLine($"Word \"{oldName}\" does not exist");
                    }
                    else
                    {
                        if(newName == "")
                        {
                            Console.WriteLine("Invalid Command");
                        }
                        else
                        {
                            wordInQuestion.word = newName;
                            Console.WriteLine($"\"{oldName}\" renamed to \"{newName}\"");
                        }
                    }
                }

            }

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

            //done
            Console.WriteLine("\n|Utility Commands|");
            Console.WriteLine("<>help -> details all commands");
            Console.WriteLine("<>save -> saves all changes to the file");
            Console.WriteLine("<>exit -> exits the program, saving if the user wishes");
            Console.WriteLine("<>clear -> clears the console");

            //done
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
            Console.WriteLine("<>findwords [WordName] -> finds the word named [WordName]");
            Console.WriteLine("<>findwords all -> lists all words");

            Console.WriteLine("\n|Word Searching Using Tags|");
            Console.WriteLine("<>FindWords WithTag [TagName] -> lists all words that have the tag [TagName]");
            Console.WriteLine("<>FindWords WithTags [Tag1], [Tag2],... -> lists all words that have all the tags in [Tag1], [Tag2],...");

            Console.WriteLine("\n|Tagging Words|");
            Console.WriteLine("<>AddTag [TagName] to [WordName] -> adds the tag [TagName] to the word [WordName]");
            Console.WriteLine("<>AddTags [Tag1], [Tag2],... to [WordName] -> adds all tags in [Tag1], [Tag2],... to the word [WordName]");
            Console.WriteLine("<>RemoveTag [TagName] from [WordName] -> removes [TagName] from [WordName]");
            Console.WriteLine("<>RemoveTags [Tag1], [Tag2],... from [WordName] -> removes all tags [Tag1], [Tag2],... from [WordName]");
            Console.WriteLine("<>SetTagsOf [WordName] to [Tag1], [Tag2],... -> sets the tags of [WordName] to the list of tags");

            Console.WriteLine("\n||");
            Console.WriteLine("<> -> ");
            Console.WriteLine("<> -> ");
            Console.WriteLine("<> -> ");
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

                    string line = streamReader.ReadLine();

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

                    string line = streamReader.ReadLine();

                    WordItem word = new WordItem(line);

                    if(word.word.Length > 0)
                    {
                        words.Add(word);
                    }

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
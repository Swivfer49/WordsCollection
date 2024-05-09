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

            Console.WriteLine("|E-SALT: word edition| version 0.1 |");

            if (fileExists)
            {
                Console.WriteLine($"Loaded existing file at [{filePath}]");
            }
            else
            {
                Console.WriteLine($"No file exists at [{filePath}], no words loaded");
            }

            while (true)
            {
                Console.Write("<>");
                string input = Console.ReadLine()!;

                if (input == "") continue;
                if (input == "Help")
                {
                    Help();
                    continue;
                }
                if (input == "Save")
                {
                    SaveWords(filePath);
                    continue;
                }
                if (input == "Exit")
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
                if (input == "Clear")
                {
                    Console.Clear();
                }
                if(input == "FindWords All")
                {
                    foreach(WordItem item in words)
                    {
                        item.WriteWord();
                        Console.WriteLine();
                    }
                }
                if (input.StartsWith("CreateWord ")){
                    words.Add(new WordItem(input.Substring(11)));
                }
                if(input.StartsWith("CreateTag "))
                {
                    Tag.Tags.Add(new Tag(input.Substring(10)));
                }

            }

        }


        static void Help()
        {
            Console.WriteLine("\n|Utility Commands|");
            Console.WriteLine("<>Help -> details all commands");
            Console.WriteLine("<>Save -> saves all changes to the file");
            Console.WriteLine("<>Exit -> exits the program, saving if the user wishes");
            Console.WriteLine("<>Clear -> clears the console");

            Console.WriteLine("\n|Word Alterations|");
            Console.WriteLine("<>CreateWord [WordName] -> adds a new word to the word bank with the name [WordName]");
            Console.WriteLine("<>RemoveWord [WordName] -> removes the word with the name [WordName] if such word exsists");
            Console.WriteLine("<>RenameWord [WordName] to [NewName] -> renames the word with the name [WordName] to [NewName]");

            Console.WriteLine("\n|Tag Alterations|");
            Console.WriteLine("<>RenameTag [TagName] to [NewName] -> renames the tag with the name [TagName] to [NewName]");
            Console.WriteLine("<>CreateTag [TagName] -> creates a new tag named [TagName]");

            Console.WriteLine("\n|Basic Word Searching|");
            Console.WriteLine("<>FindWords StartsWith [string] -> lists all words that start with [string]");
            Console.WriteLine("<>FindWords EndsWith [string] -> lists all words that end with [string]");
            Console.WriteLine("<>FindWords Contains [string] -> lists all words that contain [string]");
            Console.WriteLine("<>FindWords [WordName] -> finds the word named [WordName]");
            Console.WriteLine("<>FindWords All -> lists all words");

            Console.WriteLine("\n|Word Searching Using Tags|");
            Console.WriteLine("<>FindWords WithTag [TagName] -> lists all words that have the tag [TagName]");
            Console.WriteLine("<>FindWords WithTags [Tag1], [Tag2],... -> lists all words that have all the tags in [Tag1], [Tag2],...");

            Console.WriteLine("\n|Tagging Words|");
            Console.WriteLine("<>AddTag [TagName] to [WordName] -> adds the tag [TagName] to the word [WordName]");
            Console.WriteLine("<>AddTags [Tag1], [Tag2],... to [WordName] -> adds all tags in [Tag1], [Tag2],... to the word [WordName]");

            Console.WriteLine("\n||");
            Console.WriteLine("<>SetTags -> ");
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
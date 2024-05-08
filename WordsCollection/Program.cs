using WordsCollection.Classes;

namespace WordsCollection
{
    internal class Program
    {
        static List<WordItem> words = new List<WordItem>();
        static void Main(string[] args)
        {
            
        }

        static void MenuLoop(string fileName)
        {
            string filePath = $"./{fileName}.txt";

            //tries to load the file
            if(!LoadWords(filePath)) {
                Console.WriteLine("File not found");
                Console.ReadLine();
                return;
            }

            while(true)
            {

            }

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
                        Tag.Tags.Add(new Tag(line));
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
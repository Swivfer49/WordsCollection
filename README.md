This is a work in progress console application with the purpose of organizing "words".

Words can also come with tags.

The reasons why I am making this will be added later

here is a list of all crrently planned commands

more will be added in the future

|Utility Commands|
- <>Help -> details all commands
- <>Save -> saves all changes to the file
- <>Exit -> exits the program, saving if the user wishes
- <>Clear -> clears the console

|Word Alterations|
- <>CreateWord [WordName] -> adds a new word to the word bank with the name [WordName]
- <>RemoveWord [WordName] -> removes the word with the name [WordName] if such word exsists
- <>RenameWord [WordName] to [NewName] -> renames the word with the name [WordName] to [NewName]

|Tag Alterations|
- <>RenameTag [TagName] to [NewName] -> renames the tag with the name [TagName] to [NewName]
- <>CreateTag [TagName] -> creates a new tag named [TagName]

|Basic Word Searching|
- <>FindWords StartsWith [string] -> lists all words that start with [string]
- <>FindWords EndsWith [string] -> lists all words that end with [string]
- <>FindWords Contains [string] -> lists all words that contain [string]
- <>FindWords [WordName] -> finds the word named [WordName]
- <>FindWords All -> lists all words

|Word Searching Using Tags|
- <>FindWords WithTag [TagName] -> lists all words that have the tag [TagName]
- <>FindWords WithTags [Tag1], [Tag2],... -> lists all words that have all the tags in [Tag1], [Tag2],...

|Tagging Words|
- <>AddTag [TagName] to [WordName] -> adds the tag [TagName] to the word [WordName]
- <>AddTags [Tag1], [Tag2],... to [WordName] -> adds all tags in [Tag1], [Tag2],... to the word [WordName]

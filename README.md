# BulkRenamer
Windows utility for bulk renaming files based on regular expressions with preview.

## Usage
Type or browse to the folder you want to search.
Enter a regular expression for the files you want to match such as "(?<fileName>.*)\.(?<extension>(?<extPrefix>[^.])(?<extSuffix>[^.]*))".
Enter a replace expression such as "[${extPrefix}_${extSuffix}] ${fileName}.${extension}".
Confirm the changes you expect in the preview.  Uncheck any changes you don't want to apply.
Either click Go to do the rename or Export to save a batch file that you can edit and run later.

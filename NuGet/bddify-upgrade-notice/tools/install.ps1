param($installPath, $toolsPath, $package, $project) 
$path = [System.IO.Path] 
$readmefile = $path::Combine($installPath, "content\readme.txt") 
$DTE.ItemOperations.OpenFile($readmefile) 

***************************************************
*************** IMPORTANT NOTICE ******************
***************************************************

Firstly thanks a lot for using bddify; but I can see that you are using the old package. 
bddify is moved to GitHub and is renamed to BDDfy. The new nuget package name is 
TestStack.BDDfy. The samples have also all merged into one called TestStack.BDDfy.Samples.

To see the full details you may refer to my post about the change and how you can easily 
upgrade your solutions: http://www.mehdi-khalili.com/bddify-moved-to-github-and-renamed-to-teststack-bddfy

If you do not have time to go there you may just read on as I have included an abstract here for you.

##bddify is moved to GitHub and is renamed to BDDfy
I will just come out and say it: bddify was a confusing name. The reason I called the project bddify, 
as mentioned in the introduction post (http://www.mehdi-khalili.com/bddify-in-action/introduction), 
was to turn that action into a verb: the framework can BDD-fy your otherwise traditional tests. 
Well, that did not go down the way I expected and I have always been faced with a 'how do you pronounce it' 
question which has not been fun. 

The pronunciation issue along with a few other things led us to rename the project and to move it to GitHub. 
So bddify is now renamed to BDDfy (http://teststack.github.com/TestStack.BDDfy/) and lives in GitHub. 

Everything in the framework has been renamed from bddify to BDDfy. 
That includes nuget package, assembly name, APIs, namespaces, samples ETC. 
Since that was a (serious) breaking change we have also pushed the version up to V3.0!

##What does that mean for you?
You will need to uninstall bddify package and install BDDfy.

Well, before that I would highly recommend to upgrade to Bddify V2.11 if you have not done already as 
that had a few breaking changes from previous versions. 
Once that is done you can change to BDDfy with less hassle:

PM> Uninstall-Package Bddify

PM> Install-Package TestStack.BDDfy

After that your tests will not compile; but do not worry - it is VERY simple to fix:

 - Replace All (ctrl+shift+h) instances of '.Bddify(' with '.BDDfy('
 - Replace All instances of '.Bddify<' with '.BDDfy<' if you are using the overload with TStory type argument.
 - Replace All instances of 'using Bddify' with 'using TestStack.BDDfy'
 
 That is it. You are now using the latest version of BDDfy.  BDDfy V3 also has a few nice 
 additions that I have explained in the post mentioned above.
 
 Hope to see you over at BDDfy :)
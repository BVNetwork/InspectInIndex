# Inspect In Index #
A quick and easy way to inspect EPiServer content in the EPiServer Find index, without having to use the EPiServer Find Explore view.

When installed, you may select a new view called Inspect In Index. This will show the current content's raw form in the EPiServer Find Index, with the options to index, re-index or delete the content on the fly.

![](https://raw.githubusercontent.com/BVNetwork/InspectInIndex/master/doc/img/iii.png)

## Installation ##
Run the following command in the NuGet Package Manager Console for your web site:
```
install-package EPiCode.InspectInIndex
```
You need to add the EPiServer NuGet Feed to Visual Studio (see http://nuget.episerver.com/en/Feed/)

## Configuration ##

By default, only users with the Administrators role will have access to the view. You may override this by adding the following to appSettings in web.config:

``` 
<add key="EPiCode.InspectInIndex.AllowedRoles"
     value="developers,administrators,searchfanatics" />
```


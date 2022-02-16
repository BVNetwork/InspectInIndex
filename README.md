# Inspect In Index #
A quick and easy way to inspect Optimizely content in the Optimizely Search and Navigation index, without having to use the Optimizely Search and Navigation Explore view.

When installed, you may select a new view called Inspect In Index. This will show the current content's raw form in the EPiServer Find Index, with the options to index, re-index or delete content from the index.

![](https://raw.githubusercontent.com/BVNetwork/InspectInIndex/master/doc/img/iii.png)

## Installation ##
Run the following command in the NuGet Package Manager Console for your web site:
```
install-package EPiCode.InspectInIndex
```
You need to add the Optimizely NuGet Feed to Visual Studio (see http://nuget.optimizely.com/en/Feed/)

## Configuration ##

Add the Inspect In Index in the Startup.cs in the ConfigureServices method. Below is an example

``` 
public void ConfigureServices(IServiceCollection services)
{
...
    
	services.AddInspectInIndex();

...
}
```

By default, only users with the "CmsAdmins" or "SearchAdmins" role will have access to the view. You may override this with your own authorization policy:

``` 
 services.AddInspectInIndex(policy =>
            {
                policy.RequireRole("MyRole");
            });
```


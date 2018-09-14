# HttpsReplacementFilter HttpModule
Forces HTTPS output of image links within your .NET site.

## Setup
**Add** the ReplacementFilterModule.cs to your ASP.NET website code.

**Register** the HttpModule in the *web.config*:

```xml
...
<system.webServer>
  <modules>
    <add name="Website.ReplacementFilterModule" type="Website.ReplacementFilterModule" />
  </modules>
</system.webServer>
```


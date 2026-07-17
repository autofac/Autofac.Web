# Autofac.Web

ASP.NET web forms integration for [Autofac](https://autofac.org).

[![Build status](https://github.com/autofac/Autofac.Web/actions/workflows/main.yml/badge.svg)](https://github.com/autofac/Autofac.Web/actions/workflows/main.yml) [![codecov](https://codecov.io/gh/Autofac/Autofac.Web/branch/develop/graph/badge.svg)](https://codecov.io/gh/Autofac/Autofac.Web) [![NuGet](https://img.shields.io/nuget/v/Autofac.Web.svg)](https://nuget.org/packages/Autofac.Web)

Please file issues and pull requests for this package [in this repository](https://github.com/autofac/Autofac.Web/issues) rather than in the Autofac core repo.

- [Documentation](https://autofac.readthedocs.io/en/latest/integration/webforms.html)
- [NuGet](https://www.nuget.org/packages/Autofac.Web/)
- [Contributing](https://autofac.readthedocs.io/en/latest/contributors.html)
- [Open in Visual Studio Code](https://open.vscode.dev/autofac/Autofac.Web)

## Quick Start

To get Autofac integrated with web forms you need to reference the web forms integration NuGet package, add the modules to `web.config`, and implement `IContainerProviderAccessor` on your `Global` application class.

Add the modules to `web.config`:

```xml
<configuration>
  <system.web>
    <httpModules>
      <!-- This section is used for IIS6 -->
      <add
        name="ContainerDisposal"
        type="Autofac.Integration.Web.ContainerDisposalModule, Autofac.Integration.Web"/>
      <add
        name="PropertyInjection"
        type="Autofac.Integration.Web.Forms.PropertyInjectionModule, Autofac.Integration.Web"/>
    </httpModules>
  </system.web>
  <system.webServer>
    <!-- This section is used for IIS7 -->
    <modules>
      <add
        name="ContainerDisposal"
        type="Autofac.Integration.Web.ContainerDisposalModule, Autofac.Integration.Web"
        preCondition="managedHandler"/>
      <add
        name="PropertyInjection"
        type="Autofac.Integration.Web.Forms.PropertyInjectionModule, Autofac.Integration.Web"
        preCondition="managedHandler"/>
    </modules>
  </system.webServer>
</configuration>
```

Implement `IContainerProviderAccessor`:

```csharp
public class Global : HttpApplication, IContainerProviderAccessor
{
  // Provider that holds the application container.
  static IContainerProvider _containerProvider;

  // Instance property that will be used by Autofac HttpModules
  // to resolve and inject dependencies.
  public IContainerProvider ContainerProvider
  {
    get { return _containerProvider; }
  }

  protected void Application_Start(object sender, EventArgs e)
  {
    // Build up your application container and register your dependencies.
    var builder = new ContainerBuilder();
    builder.RegisterType<SomeDependency>();
    // ... continue registering dependencies...

    // Once you're done registering things, set the container
    // provider up with your registrations.
    _containerProvider = new ContainerProvider(builder.Build());
  }
}
```

[Check out the documentation](https://autofac.readthedocs.io/en/latest/integration/webforms.html) for more usage details.

## Get Help

**Need help with Autofac?** We have [a documentation site](https://autofac.readthedocs.io/) as well as [API documentation](https://autofac.org/apidoc/). We're ready to answer your questions on [Stack Overflow](https://stackoverflow.com/questions/tagged/autofac) or check out the [discussion forum](https://groups.google.com/forum/#forum/autofac).

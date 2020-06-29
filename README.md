# ServiceLocator

[![openupm](https://img.shields.io/npm/v/com.apollo.servicelocator?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.apollo.servicelocator/)

## Install Package

### Install via OpenUPM

The package is available on the [openupm registry](https://openupm.com). It's recommended to install it via [openupm-cli](https://github.com/openupm/openupm-cli).

```
openupm add com.apollo.servicelocator
```

### Install via Git URL

Since Unity 2019.3, use *Add a package from git URL* button from the Package Manager UI to install the package.

```
https://github.com/hajimeku/ServiceLocator.git
```

For previous Unity versions, add the following line to the project *manifest.json* file.

    {
        "dependencies": {
            "com.apollo.servicelocator": "https://github.com/hajimeku/ServiceLocator.git"
        }
    }

## Summary
This is a very lightweight Service Locator Pattern library that is intended to help facilitate structure and support for Unit Testing without the need of heavy libraries such as DI.

## Functions
You can register a service like this:
```c#
//For Regular Classes
ServiceLocatorManager.Instance.Register<IExampleRegularClass>(new ExampleRegularClass());
//For Monobehaviour classes        
ServiceLocatorManager.Instance.Register<IExampleMonobehaviourClass>(ServiceLocatorManager.AsMono<ExampleMonobehaviourClass>());
```

To fetch a service you can request it like this:
```c#
private IExampleRegularClass regularClass => ServiceLocatorManager.Instance.Resolve<IExampleRegularClass>();
private IExampleMonobehaviourClass monobehaviourClass => ServiceLocatorManager.Instance.Resolve<IExampleMonobehaviourClass>();
```

Example Bootstrap Class. Make sure to create one of these and register your services on a static function marked like ConfigureServices
```c#
public static class ExampleBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void ConfigureServices()
    {
        //Register this for debug log
        ServiceLocatorManager.Instance.Register<IExampleLog>(new ExampleProductionLog());
        
        //Register this for a Debug log
        //ServiceLocatorManager.Instance.Register<IExampleLog>(new ExampleDebugLog());
    }
}
```




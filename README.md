# ProxyChecker
> .NET application for checking web proxies.

ProxyChecker is a WPF application with a modern design for cheking proxies.

![](https://github.com/JessieSharp/ProxyChecker/blob/main/demo.png?raw=true)

## Installation

Windows:

```sh
The easiest way will be to download the compiled build at https://github.com/JessieSharp/ProxyChecker/releases/tag/Latest
```

## Instrucion

1. Click File->Load proxies
2. Adjust settings
  2a. Timeout - Stands for how long the application should wait for response
  2b. Speed - Means how fast the proxy list should be checking. Lower means faster.
  2c. Target - I a link which will be used for testing proxies.
3. Press start and wait for the application to finish
4. After the "Left" value will hit zero you can safely export the proxies to .txt by clicking "Export proxies". The program will save only the good one to file in the same directory as the application.
5. Enjoy your proxies :)
  

## Release History

* 1.0.0.0
    * First release

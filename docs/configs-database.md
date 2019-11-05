# Configs database documentation

Original source file located at `assets/configs.xml` of current repository.

## XML database example

```xml
<?xml version="1.0" encoding="utf-8" ?>
<Configs>
    <Config>
        <Name>foo-bar</Name>
        <FileName>foo-bar.cfg</FileName>
        <SupportedGames>240;440</SupportedGames>
        <ru>
        <![CDATA[Description in Russian.]]>
        </ru>
        <en>
        <![CDATA[Description in English.]]>
        </en>
    </Config>
</Configs>
```

## Properties

Available properties:

  * `Configs` - XML root element;
  * `Config` - database entry base element;
  * `Name` - user-friendly name of FPS-config;
  * `FileName` - file name of FPS-config (must be located at `assets/cfgs` directory; custom paths are not allowed);
  * `SupportedGames` - comma-separated list of supported game IDs;
  * `ru` - user-friendly description in Russian (CDATA escaping is required);
  * `en` - user-friendly description in English (CDATA escaping is required).

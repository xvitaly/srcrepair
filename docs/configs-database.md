# Configs database documentation

Original source file located at `assets/configs.xml` of current repository.

## XML database example

```xml
<?xml version="1.0" encoding="utf-8" ?>
<Configs>
    <Config>
        <Name>foo-bar</Name>
        <URI>https://example.org/foo-bar.zip</URI>
        <Mirror>https://example.com/foo-bar.zip</Mirror>
        <SupportedGames>240;440</SupportedGames>
        <ru>
        <![CDATA[Description in Russian.]]>
        </ru>
        <en>
        <![CDATA[Description in English.]]>
        </en>
        <ArchiveDir>foo-bar</ArchiveDir>
        <InstallDir>foo-bar</InstallDir>
        <Hash2>SHA-512</Hash2>
    </Config>
</Configs>
```

## Properties

### Level 0

  * `Configs` - XML root element.

### Level 1

  * `Config` - database entry base element.

### Level 2

  * `Name` - user-friendly name of FPS-config;
  * `URI` - fully-qualified Zip archive download URL (safe redirects (3XX HTTP codes) are allowed);
  * `Mirror` - fully-qualified Zip archive download mirror URL (safe redirects (3XX HTTP codes) are allowed);
  * `SupportedGames` - comma-separated list of supported game IDs;
  * `ru` - user-friendly description in Russian (CDATA escaping is required);
  * `en` - user-friendly description in English (CDATA escaping is required);
  * `ArchiveDir` - name of directory in archive (subdirectories are supported (use `/` symbol));
  * `InstallDir` - installation directory name;
  * `Hash2` - SHA2 (SHA-512) hash of download file, speficied in `URI`.

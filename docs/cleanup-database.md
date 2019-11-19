# Cleanup database documentation

Original source file located at `assets/cleanup.xml` of current repository.

## XML database example

```xml
<?xml version="1.0" encoding="utf-8" ?>
<Targets>
    <Target>
        <ID>1</ID>
        <Name>Example entry</Name>
        <Directories>
            <Directory Class="Safe">$GamePath$/foo/*.*</Directory>
            <Directory Class="Unsafe">$FullGamePath$/bar/*.*</Directory>
        </Directories>
    </Target>
</Targets>
```

## Properties

### Level 0

  * `Targets` - XML root element.

### Level 1

  * `Target` - database entry base element.

### Level 2

  * `ID` - unique identifier (integer, starting from 1);
  * `Name` - user-friendly name;
  * `Directories` - list of directories.

### Level 3

  * `Directory` - path to a single directory with templates support:
    - `Safe` - this directory can be safely cleaned;
    - `Unsafe` - cleaning up this directory may be dangerous (can be disabled in settings).

## Directories

Each path must ends with a file mask. All matched files will be marked to deletion.

Use `*.*` to mark all files in a specified directory.

## Templates

Available templates (can be used within `Directory` property):

  * `$GamePath$` - will be replaced by `SelectedGame.GamePath`;
  * `$FullGamePath$` - will be replaced by `SelectedGame.FullGamePath`;
  * `$AppWorkshopDir$` - will be replaced by `SelectedGame.AppWorkshopDir`;
  * `$CloudScreenshotsPath$` - will be replaced by `SelectedGame.CloudScreenshotsPath`;
  * `\` - will be replaced by correct trailing path directory separator character, depending on running platform.

Multiple templates are supported in a single entry.

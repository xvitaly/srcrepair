# Games database documentation

Original source file located at `assets/games.xml` of current repository.

## XML database example

```xml
<?xml version="1.0" encoding="utf-8" ?>
<Games>
    <Game Name="Team Fortress 2">
        <Enabled>1</Enabled>
        <HasVF>0</HasVF>
        <DirName>Team Fortress 2</DirName>
        <SmallName>tf</SmallName>
        <VFDir>tf</VFDir>
        <UserDir>1</UserDir>
        <HUDsAvail>1</HUDsAvail>
        <SID>440</SID>
        <SVer>1</SVer>
        <Executable>hl2.exe</Executable>
    </Game>
</Games>
```

## Properties

### Level 0

  * `Games` - XML root element.

### Level 1

  * `Game` - database entry base element:
    - `Name` - user-friendly game name.

### Level 2

  * `Enabled` - `1` if current game is enabled or `0` - if don't;
  * `HasVF` - `1` if current game use file to store video settings or `0` if current game use Windows registry to store video settings (deprecated);
  * `DirName` - game installation directory in `SteamApps/common`;
  * `SmallName` - game subdirectory name in `SteamApps/common/$DirName`;
  * `VFDir` - directory (or registry key) name of video settings storage;
  * `UserDir` - `1` if current game supports custom user stuff, located in `custom` directory or `0` - if don't;
  * `HUDsAvail` - `1` if current game supports custom HUDs or `0` - if don't;
  * `SID` - Steam database game internal ID;
  * `SVer` - Source Engine version:
    - `1` - Source Engine 1, Type 1 (use Windows registry to store video settings);
    - `2` - Source Engine 1, Type 2 (use `video.txt` file to store video settings);
    - `3` - Source Engine 2;
  * `Executable` - game executable file name (only for Windows).

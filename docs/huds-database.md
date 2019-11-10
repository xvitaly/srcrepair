# HUDs database documentation

Original source file located at `assets/huds.xml` of current repository.

## XML database example

```xml
<?xml version="1.0" encoding="utf-8" ?>
<HUDs>
    <HUD>
        <Name>7HUD</Name>
        <Game>tf</Game>
        <IsUpdated>1</IsUpdated>
        <URI>https://sourceforge.net/projects/srcrepair/files/huds/7hud/7hud_28903d1b.zip</URI>
        <UpURI>https://github.com/Sevin7/7HUD/archive/master.zip</UpURI>
        <Preview>https://www.easycoding.org/files/srcrepair/huds/7hud.jpg</Preview>
        <RepoPath>https://github.com/Sevin7/7HUD</RepoPath>
        <Hash>44e154bb825269e9e0c039759353d287</Hash>
        <Hash2>2c35b35d3e58dc75f3bc40134c4d137353d994d6dcc879e3edc35b837cbe2ae91cda0b2f698741fda17111a4543b7a002534b609de720e5125655d5b23e65217</Hash2>
        <LastUpdate>1572411245</LastUpdate>
        <Site>https://huds.tf/forum/showthread.php?tid=261</Site>
        <ArchiveDir>7HUD-master</ArchiveDir>
        <InstallDir>7hud</InstallDir>
    </HUD>
</HUDs>
```

## Properties

Available properties:

  * `HUDs` - XML root element;
  * `HUD` - database entry base element;
  * `Name` - user-friendly name of HUD;
  * `Game` - short name of supported by this HUD game (`SmallName` from [games database](games-database.md));
  * `IsUpdated` - `1` if HUD support latest version of game, `0` - if don't;
  * `URI` - fully-qualified Zip archive download URL (safe redirects (3XX HTTP codes) are allowed);
  * `UpURI` - upstream download URL (no redirects allowed);
  * `Preview` - screenshot of game using this HUD or any custom image (JPEG and PNG formats are supported);
  * `RepoPath` - path to repository on GitHub or `null`;
  * `Hash` - MD5 hash of download file, speficied in `URI` (deprecated);
  * `Hash2` - SHA2 (SHA-512) hash of download file, speficied in `URI`;
  * `LastUpdate` - HUD last update time in Unix timestamp format;
  * `Site` - website or homepage;
  * `ArchiveDir` - name of directory in archive (subdirectories are supported (use `/` symbol));
  * `InstallDir` - installation directory.

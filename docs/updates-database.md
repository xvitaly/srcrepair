# Updates database documentation

Original source file located at `assets/updates.xml` of current repository.

## Properties

### Level 0

  * `Updates` - XML root element.

### Level 1

  * `Application` - sub-element with application update metadata;
  * `GameDB` - sub-element with [games database](games-database.md) update metadata;
  * `HUDDB` - sub-element with [HUDs database](huds-database.md) update metadata;
  * `CfgDB` - sub-element with [configs database](configs-database.md) update metadata;
  * `ClnDB` - sub-element with [cleanup database](cleanup-database.md) update metadata.

### Level 2

  * `Version` - application or database version;
  * `URL` - direct download URL (no redirects allowed);
  * `Hash` - MD5 hash of download file (deprecated);
  * `Hash2` - SHA2 (SHA-512) hash of download file.

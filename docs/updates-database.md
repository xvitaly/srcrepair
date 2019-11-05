# Updates database documentation

Original source file located at `assets/updates.xml` of current repository.

## Properties

### Level 1

  * `Updates` - XML root element;
  * `Application` - sub-element with application update metadata;
  * `GameDB` - sub-element with game database update metadata;
  * `HUDDB` - sub-element with HUD database update metadata;
  * `CfgDB` - sub-element with config database update metadata;
  * `ClnDB` - sub-element with [cleanup database](cleanup-database.md) update metadata.
  
### Level 2

  * `Version` - application or database version;
  * `URL` - direct download URL (no redirects allowed);
  * `Hash` - MD5 hash of download file (deprecated);
  * `Hash2` - SHA2 (SHA-512) hash of download file.
  

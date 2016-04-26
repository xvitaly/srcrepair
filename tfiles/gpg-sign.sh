#!/bin/bash

function sign {
  gpg2 --sign --detach-sign --default-key D45AB90A --homedir E:/.gnupg/ $1
}

sign srcrepair.exe
sign Ionic.Zip.Reduced.dll
sign ru/srcrepair.resources.dll

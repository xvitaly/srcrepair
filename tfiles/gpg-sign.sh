#!/bin/bash

function sign {
  gpg2 --sign --detach-sign --default-key D45AB90A $1
}

sign srcrepair.exe
sign DotNetZip.dll
sign ru/srcrepair.resources.dll

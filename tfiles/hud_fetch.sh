#!/bin/bash

function fetch_hud
{
    # Проверяем существование файла со скриншотом и если он существует, удаляем...
    if [ -f "$3.png" ]; then
        rm -f $3.png
    fi

    # Загружаем скриншот с удалённого сервера...
    wget $1 -O $3.png > /dev/null 2> /dev/null

    # Проверяем существование архива и если он существует, удаляем...
    if [ -f "$3.zip" ]; then
        rm -f $3.zip
    fi

    # Загружаем новую версию архива из апстрима...
    wget $2 -O $3.zip > /dev/null 2> /dev/null
}

fetch_hud http://huds.tf/img/main/7hud.png https://github.com/Sevin7/7HUD/archive/master.zip 7HUD
fetch_hud http://huds.tf/img/main/herganhud.png https://github.com/Hergan5/herganhud/archive/master.zip herganhud
fetch_hud http://huds.tf/img/main/pikleshud.png https://github.com/piklestf2/pikles-hud/archive/master.zip pikles-hud

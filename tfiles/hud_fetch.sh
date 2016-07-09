#!/bin/bash

# 
# Скрипт создания зеркала HUD.
# 
# Copyright 2011 - 2016 EasyCoding Team (ECTeam).
# Copyright 2005 - 2016 EasyCoding Team.
# 
# Лицензия: GPL v3 (см. файл GPL.txt).
# Лицензия контента: Creative Commons 3.0 BY.
# 
# Запрещается использовать этот файл при использовании любой
# лицензии, отличной от GNU GPL версии 3 и с ней совместимой.
# 
# Официальный блог EasyCoding Team: http://www.easycoding.org/
# Официальная страница проекта: http://www.easycoding.org/projects/srcrepair
# 
# Более подробная инфорация о программе в readme.txt,
# о лицензии - в GPL.txt.
# 

function fetch_hud
{
    # Выводим текст, оповещающий пользователя о начале загрузки...
    echo -n "Downloading $2..."
    
    # Проверим существование каталога для HUD и если он не существует, создаём...
    if [ ! -d "$2" ]; then
        mkdir -p $2
    fi

    # Проверяем существование архива и если он не существует, загружаем...
    if [ ! -f "$2/$2.zip" ]; then
        # Загружаем новую версию архива из апстрима...
        wget $1 -O $2/$2.zip > /dev/null 2> /dev/null
    fi

    # Генерируем окончательное имя архива...
    nf=$(sha256sum $2/$2.zip | awk '{print $1}')
    mv $2/$2.zip $2/$2_${nf:0:8}.zip
    
    # Выводим текст, оповещающий пользователя о завершении загрузки...
    echo " Done."
}

# From GitHub...
fetch_hud https://github.com/Sevin7/7HUD/archive/master.zip 7HUD
fetch_hud https://github.com/piklestf2/pikles-hud/archive/master.zip pikles-hud
fetch_hud https://github.com/fblue/broeselhud_blue/archive/master.zip broeselhud_blue
fetch_hud https://github.com/basbanaan/bastHUD/archive/master.zip basthud
fetch_hud https://github.com/sirgrey/SirHUD/archive/master.zip sirhud
fetch_hud https://github.com/SnowshoeIceboot/TF2HudPlus/archive/master.zip tf2hudplus
fetch_hud https://github.com/TheStaticVoid/VoidHUD2.0/archive/master.zip voidhud
fetch_hud https://github.com/n0kk/ahud/archive/master.zip ahud
fetch_hud https://github.com/Slayer89/slayhud/archive/master.zip calmlikeabomb
fetch_hud https://github.com/basbanaan/EJP-HUD/archive/master.zip ejphud
fetch_hud https://github.com/flatlinee/flatHUD-/archive/master.zip flathud
fetch_hud https://github.com/bw-/bw-HUD/archive/master.zip bwhud
fetch_hud https://github.com/WhiskerBiscuit/budhud/archive/master.zip budhud
fetch_hud https://github.com/Eniere/idhud/archive/master.zip idhud
fetch_hud https://github.com/omnibombulator/noto/archive/master.zip noto
fetch_hud https://github.com/ItsMorgus/MorgHUD/archive/master.zip morghud
fetch_hud https://github.com/Jotunn/KBNHud/archive/master.zip kbnhud
fetch_hud https://github.com/Intellectualbadass/medHUD/archive/master.zip medhud
fetch_hud https://github.com/raysfire/rayshud/archive/master.zip rayshud
fetch_hud https://github.com/MedicodiBiscotti/biscottiHUD/archive/master.zip biscottihud
fetch_hud https://github.com/mattr0d/flamehud/archive/master.zip flamehud
fetch_hud https://github.com/TheKins/frankenhud/archive/master.zip frankenhud
fetch_hud https://github.com/Xeletron/Isaac-Hud/archive/master.zip isaachud
fetch_hud https://github.com/kermit-tf/JayHUD/archive/master.zip jayhud
fetch_hud https://github.com/mannterface/Mannterface/archive/master.zip mannterface
fetch_hud https://github.com/Yttrium-tYcLief/yayahud/archive/master.zip yayahud
fetch_hud https://github.com/Smesi/SmesiHud/archive/master.zip smesihud
fetch_hud https://github.com/rainoflight/rainhud/archive/master.zip rainhud
fetch_hud https://github.com/Stochast1c/solarhud/archive/master.zip solarhud
fetch_hud https://github.com/Yo5hi/ysHUD/archive/master.zip yshud
fetch_hud https://github.com/TheStaticVoid/boredHUD/archive/master.zip boredhud
fetch_hud https://github.com/hoXyy/GMang_HUD/archive/master.zip gmanghud
fetch_hud https://github.com/CriticalFlaw/FlawHUD/archive/master.zip flawhud

# Other sources...
fetch_hud https://gitgud.io/JediThug/JediHUD/repository/archive.zip jedihud
fetch_hud http://files.gamebanana.com/guis/eve_hud_v378.zip evehud
fetch_hud http://files.gamebanana.com/guis/revhud-jan17.zip revhud
fetch_hud http://huds.tf/forum/xthreads_attach.php/151_1468069037_c96274f0/2e863cebac5b6ff6c2c39aa2b42fdf99/DoggyHudV2.zip doggyhud

# Need to be repacked...
fetch_hud https://github.com/JackStanley/TF2slimHUD/archive/Main.zip slimhud

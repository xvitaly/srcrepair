#!/usr/bin/env python3
# coding=utf-8

#
# Скрипт создания и обновления зеркала HUD.
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


def parsedb(dbname):
    # Importing XML parser library...
    from xml.dom import minidom

    # Creating list for result...
    result = []

    # Opening HUD database...
    huddb = minidom.parse(dbname)

    # Parsing...
    for hud in huddb.getElementsByTagName('HUD'):
        result.append([hud.getElementsByTagName("Name")[0].firstChild.data,
                       hud.getElementsByTagName("UpURI")[0].firstChild.data,
                       hud.getElementsByTagName("RepoPath")[0].firstChild.data,
                       hud.getElementsByTagName("LastUpdate")[0].firstChild.data])

    # Returning result...
    return result


def main():
    try:
        # Main exec...
        print (parsedb('huds.xml'))

    except:
        # Exception detected...
        print('An error occurred. Try again later.')


if __name__ == '__main__':
    main()

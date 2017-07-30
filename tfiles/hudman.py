#!/usr/bin/env python2
# coding=utf-8

#
# Скрипт создания и обновления зеркала HUD.
#
# Copyright 2011 - 2017 EasyCoding Team (ECTeam).
# Copyright 2005 - 2017 EasyCoding Team.
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

from calendar import timegm
from datetime import datetime
from hashlib import md5, sha1
from json import loads
from os import path, getcwd, makedirs, rename
from shutil import rmtree
from urllib import URLopener, urlopen, urlretrieve
from xml.dom import minidom


def parsedb(dbname):
    result = []
    huddb = minidom.parse(dbname)
    for hud in huddb.getElementsByTagName('HUD'):
        result.append([hud.getElementsByTagName("InstallDir")[0].firstChild.data,
                       hud.getElementsByTagName("UpURI")[0].firstChild.data,
                       hud.getElementsByTagName("RepoPath")[0].firstChild.data,
                       int(hud.getElementsByTagName("LastUpdate")[0].firstChild.data),
                       path.basename(hud.getElementsByTagName("URI")[0].firstChild.data)])
    return result


def gmt2unix(gtime):
    do = datetime.strptime(gtime, '%Y-%m-%dT%H:%M:%SZ')
    return int(timegm(do.timetuple()))


def getghinfo(repourl):
    url = repourl.replace('https://github.com/', 'https://api.github.com/repos/') + '/commits?per_page=1'
    response = urlopen(url)
    respcode = response.getcode()
    if respcode != 200:
        raise Exception('GitHub API returned %d error code.' % respcode)
    data = loads(response.read().decode('utf8'))
    return [data[0]['sha'], gmt2unix(data[0]['commit']['committer']['date'])]


def downloadfile(url, name):
    fdir = path.join(getcwd(), 'huds', name)
    if not path.exists(fdir):
        makedirs(fdir)
    filepath = path.join(fdir, '%s.zip' % name)
    urlretrieve(url, filepath)
    return filepath


def renamefile(fname, chash):
    fdir = path.dirname(fname)
    result = path.join(fdir, '%s_%s.zip' % (path.splitext(path.basename(fname))[0], chash[:8]))
    rename(fname, result)
    return result


def md5hash(fname):
    return md5(open(fname, 'rb').read()).hexdigest()


def sha1hash(fname):
    return sha1(open(fname, 'rb').read()).hexdigest()


def handlehud(name, url, repo, ltime, lfname):
    if repo.find('https://github.com/') != -1:
        r = getghinfo(repo)
        if r[1] > ltime:
            print('%s has been updated. Hash: %s, time: %s, filename: %s.' % (
                name, r[0], r[1], path.basename(renamefile(downloadfile(url, name), r[0]))))
        else:
            print('%s is up to date.' % name)
    else:
        filednl = downloadfile(url, name)
        fullfile = renamefile(filednl, sha1hash(filednl))
        shortfile = path.basename(fullfile)
        if shortfile != lfname:
            print('%s downloaded. Filename: %s.' % (name, shortfile))
        else:
            rmtree(path.dirname(fullfile))
            print('%s is up to date.' % name)


def main():
    try:
        URLopener.version = 'wget'
        for hud in parsedb('huds.xml'):
            try:
                handlehud(hud[0], hud[1], hud[2], hud[3], hud[4])
            except Exception as ex:
                print('Error while checking %s updates: %s' % (hud[0], ex.message))
    except Exception as ex:
        print('An error occurred: %s' % ex.message)


if __name__ == '__main__':
    main()

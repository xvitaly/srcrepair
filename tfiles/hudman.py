#!/usr/bin/env python2
# coding=utf-8

#
# This file is a part of SRC Repair project. For more information
# visit official site: https://www.easycoding.org/projects/srcrepair
# 
# Copyright (c) 2011 - 2018 EasyCoding Team (ECTeam).
# Copyright (c) 2005 - 2018 EasyCoding Team.
# 
# This program is free software: you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.
# 
# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
# 
# You should have received a copy of the GNU General Public License
# along with this program. If not, see <http://www.gnu.org/licenses/>.
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
            f = renamefile(downloadfile(url, name), r[0]);
            print('%s has been updated. Hash: %s, time: %s, filename: %s.' % (
                name, md5hash(f), r[1], path.basename(f)))
        else:
            print('%s is up to date.' % name)
    else:
        filednl = downloadfile(url, name)
        fullfile = renamefile(filednl, sha1hash(filednl))
        shortfile = path.basename(fullfile)
        if shortfile != lfname:
            print('%s downloaded. Hash: %s, filename: %s.' % (name, md5hash(fullfile), shortfile))
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

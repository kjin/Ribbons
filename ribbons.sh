#!/bin/bash

# Set up paths based on username
if [ "$USER" = jink2 ]
then
    ribbons_source_path='/cygdrive/c/Users/jink2/Documents/Ribbons/Ribbons'
    ribbons_dropbox_path='/cygdrive/c/Users/jink2/Dropbox/Ribbons'
fi
if [ "$USER" = Danny ]
then
    ribbons_source_path='/cygdrive/c/Users/Danny/Documents/Ribbons/Ribbons'
    ribbons_dropbox_path='/cygdrive/c/Users/Danny/Dropbox/Ribbons'
fi
ribbons_repo_path='https://github.com/kjin/Ribbons.git'
ribbons_rel_content_path='RibbonsContent/RibbonsContentContent'
ribbons_rel_bin_path='Ribbons/bin/WindowsGL'
ribbons_assets[1]='Textures'
ribbons_assets[2]='SFX'
ribbons_assets[3]='XACT'

ribbons()
{
    if [ -z "$1" ]
    then
        echo 'ribbons clone             Clones Ribbons in the current directory'
        echo 'ribbons debug             Goes to the Ribbons debug build folder'
        echo 'ribbons commit [message]  Fast add current folder and commit with message (use with caution)'
        echo 'ribbons go                Goes to the Ribbons source directory'
        echo 'ribbons pull              Pulls changes to Ribbons source from Git and Dropbox'
        echo 'ribbons push              Pushes changes to Ribbons source to Git and Dropbox'
        echo 'ribbons release           Goes to the Ribbons release build folder'
    fi
    if [ "$1" = 'go' ]
    then
        cd $ribbons_source_path
    fi
    if [ "$1" = 'clone' ]
    then
        git clone $ribbons_repo_path
    fi
    if [ "$1" = 'commit' ]
    then
        if [ -z "$2" ]
        then
            echo 'No message provided - nothing was added or committed.'
        else
            git reset HEAD .
            git add .
            git commit -m "$2"
        fi
    fi
    if [ "$1" = 'debug' ]
    then
        cd $ribbons_source_path/$ribbons_rel_bin_path/Debug
    fi
    if [ "$1" = 'release' ]
    then
        cd $ribbons_source_path/$ribbons_rel_bin_path/Release
    fi
    if [ "$1" = 'pull' ]
    then
        git pull
        if [ $? -eq 0 ]
        then
            for index in 1 2 3
            do
                rm -rf $ribbons_source_path/$ribbons_rel_content_path/${ribbons_assets[index]}.old
                mv -f $ribbons_source_path/$ribbons_rel_content_path/${ribbons_assets[index]} $ribbons_source_path/$ribbons_rel_content_path/${ribbons_assets[index]}.old
                cp -r $ribbons_dropbox_path/${ribbons_assets[index]} $ribbons_source_path/$ribbons_rel_content_path/${ribbons_assets[index]}
            done
        fi
    fi
    if [ "$1" = 'push' ]
    then
        git push
        if [ $? -eq 0 ]
        then
            for index in 1 2 3
            do
                rm -rf $ribbons_dropbox_path/${ribbons_assets[index]}.old
                mv -f $ribbons_dropbox_path/${ribbons_assets[index]} $ribbons_dropbox_path/${ribbons_assets[index]}.old
                cp -r $ribbons_source_path/$ribbons_rel_content_path/${ribbons_assets[index]} $ribbons_dropbox_path/${ribbons_assets[index]}
            done
        fi
    fi
}

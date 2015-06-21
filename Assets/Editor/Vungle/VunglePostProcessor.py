#!/usr/bin/python

import sys
from mod_pbxproj import XcodeProject

pathToProjectFile = sys.argv[1] + '/Unity-iPhone.xcodeproj/project.pbxproj'
pathToNativeCodeFiles = sys.argv[2]

project = XcodeProject.Load( pathToProjectFile )

project.add_folder( pathToNativeCodeFiles, excludes=["^.*\.meta$"] )
project.add_file_if_doesnt_exist( 'System/Library/Frameworks/AdSupport.framework', tree='SDKROOT', weak=True, parent='Frameworks' )
project.add_file_if_doesnt_exist( 'System/Library/Frameworks/StoreKit.framework', tree='SDKROOT', parent='Frameworks' )
project.add_file_if_doesnt_exist( 'usr/lib/libsqlite3.dylib', tree='SDKROOT', parent='Frameworks' )
project.add_file_if_doesnt_exist( 'usr/lib/libz.1.1.3.dylib', tree='SDKROOT', parent='Frameworks' )

if project.modified:
	project.save()
#!/bin/bash

root_dir=$(cd `dirname $0` && pwd -P)

export PATH="~/test/bin/net6.0/osx-x64:$PATH"

# ilasm -dll -arm64 -output=Fiddler.WebUi.dll Fiddler.WebUi.il

ilasm -dll -arm64 -output=FiddlerBackendSDK.dll FiddlerBackendSDK.il
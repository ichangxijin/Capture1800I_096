::删除system执行文件目录下的编译中间文件；
del system\*.exp
del system\*.pdb
rd /Q/S system\CareRay\logs

::删除data文件夹下面的文件；
del /Q data\system\*.*

::删除Capture文件夹下编译中间文件
rd /Q/S Capture\obj
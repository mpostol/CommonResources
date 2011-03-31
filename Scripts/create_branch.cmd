rem//  $LastChangedDate$
rem//  $Rev$
rem//  $LastChangedBy$
rem//  $URL$
rem//  $Id$

if "%1"=="" goto ERROR
set branchtype=%2
if "%branchtype%"=="" goto setbranch

:dothejob
svn mkdir svn://svnserver.hq.cas.com.pl/VS/%branchtype%/PR37-CAS_LIB_PCKG/%1  -m "created new %branchtype%  %1 (%branchtype% folder)"
svn copy svn://svnserver.hq.cas.com.pl/VS/trunk/ImageLibrary svn://svnserver.hq.cas.com.pl/VS/%branchtype%/PR37-CAS_LIB_PCKG/%1/ImageLibrary -m "created new %branchtype% %1 (project ImageLibrary)"
svn copy svn://svnserver.hq.cas.com.pl/VS/trunk/CommonBinaries svn://svnserver.hq.cas.com.pl/VS/%branchtype%/PR37-CAS_LIB_PCKG/%1/CommonBinaries -m "created new %branchtype% %1 (project CommonBinaries)"
svn copy svn://svnserver.hq.cas.com.pl/VS/trunk/PR37-CAS_LIB_PCKG svn://svnserver.hq.cas.com.pl/VS/%branchtype%/PR37-CAS_LIB_PCKG/%1/PR37-CAS_LIB_PCKG -m "created new %branchtype% %1 (project PR37-CAS_LIB_PCKG)"

goto EXIT

:setbranch
set branchtype=branches
goto dothejob
:ERROR
echo Parametr must be set
:EXIT
